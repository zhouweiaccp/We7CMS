using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using System.IO;
using System.Web.UI;
using We7.Framework.Cache;
using We7.Framework;
using System.Reflection;
using We7.Framework.Zip;
using System.Threading;
using We7.Model.Core;

namespace We7.CMS.Module.VisualTemplate
{
    public delegate DataTable ConvertEventHandler(DataTable dt);

    public class DesignHelper
    {
        private UserControl container;
        private DataTable recoreds;
        private static ReaderWriterLock readWriteLock = new ReaderWriterLock();

        public event ConvertEventHandler ConvertCommonData;

        public DesignHelper(UserControl uc)
        {
            container = uc;
            IsDesigning = String.Compare(Request["virtualdata"], "virtualdata", true) == 0;
        }

        /// <summary>
        /// 是否是设计时
        /// </summary>
        public bool IsDesigning
        {
            get;
            set;
        }

        private DataTable CommonData
        {
            get
            {
                ICacheStrategy cache = AppCtx.Cache;
                string key = GetType().FullName + "_DesigningData";
                DataTable dt = cache.RetrieveObject<DataTable>(key);
                if (dt == null)
                {
                    string file = Server.MapPath("~/We7Controls/Data/Data.xml");
                    string schema = Server.MapPath("~/We7Controls/Data/Schema.xsd");
                    dt = ReadData(file, schema);
                    if (dt != null)
                    {
                        cache.AddObjectWithFileChange(key, dt, file, schema);
                    }
                    else
                    {
                        dt = new DataTable();
                    }
                }
                return dt;
            }
        }

        private DataTable Records
        {
            get
            {
                if (recoreds == null)
                {
                    string filePath =Server.MapPath(Path.Combine(container.TemplateSourceDirectory, "Data/Data.xml"));
                    string schemaPath =Server.MapPath(Path.Combine(container.TemplateSourceDirectory, "Data/Schema.xsd"));
                    
                    recoreds = ReadData(filePath, schemaPath);
                    if (recoreds == null)
                    {
                        OnConvertCommonData();
                    }

                    if (recoreds == null)
                    {
                        throw new Exception("没有设计时数据");
                    }
                }
                return recoreds;
            }
        }

        /// <summary>
        /// 导出示例数据
        /// </summary>
        /// <param name="dt"></param>
        public static void ExprotData(DataTable dt)
        {
            readWriteLock.AcquireWriterLock(10000);
            try
            {
                DataSet ds = new DataSet();
                ds.DataSetName = "We7DataSet";
                dt.TableName = "Design";
                ds.Tables.Add(dt);


                string tempDir = Server.MapPath("~/_temp/Data");
                string tempXml = Server.MapPath("~/_temp/Data/Data.xml");
                string tempSchema = Server.MapPath("~/_temp/Data/Schema.xsd");
                DirectoryInfo di = new DirectoryInfo(tempDir);

                if (!di.Exists)
                    di.Create();

                ds.WriteXml(tempXml);
                ds.WriteXmlSchema(tempSchema);


                Response.Clear();
                Response.ContentType = "application/zip";
                Response.AddHeader("Content-Disposition", "attachment;filename=Data.zip");
                ZipUtils.CreateZip(tempDir, Response.OutputStream);
                Response.End();
            }
            finally
            {
                readWriteLock.ReleaseWriterLock();
            }           
        }

        /// <summary>
        /// 取得当前的数据记录，如果数据为空时可以根据转换委托来实现将公共数据转化为符合要求的数据
        /// </summary>
        /// <param name="convertCommonData"></param>
        /// <returns></returns>
        public DataTable GetRecords(ConvertEventHandler convertCommonData)
        {
            string filePath = Path.Combine(container.TemplateSourceDirectory, "Data/Data.xml");
            string schemaPath = Path.Combine(container.TemplateSourceDirectory, "Data/Schema.xsd");
            recoreds = ReadData(filePath, schemaPath);
            if (recoreds == null && convertCommonData != null)
            {
                recoreds = convertCommonData(CommonData.Copy());
            }

            if (recoreds == null)
            {
                throw new Exception("没有设计时数据");
            }
            return recoreds;
        }

        /// <summary>
        /// 填充数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        public int FillItems<T>(out List<T> items, int pageSize) where T : class, new()
        {
            int recordCount;
            DataTable dt = GetRecords(pageSize, out recordCount);
            items = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T t = new T();
                foreach (PropertyInfo p in t.GetType().GetProperties())
                {
                    if (!p.CanWrite)
                        continue;
                    object v = null;
                    if (dt.Columns.Contains(p.Name))
                    {
                        v = row[p.Name];
                    }
                    else
                    {
                        DataColumn cur = null;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.DataType == p.PropertyType)
                                cur = dc;
                        }
                        if (cur != null)
                            v = row[cur];
                    }
                    if (v != null && v != DBNull.Value)
                    {
                        p.SetValue(t, v, null);
                    }

                }
                items.Add(t);
            }
            return recordCount;
        }

        /// <summary>
        /// 填充数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="item"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        public int FillItems<T>(out List<T> items, out T item, int pageSize) where T : class, new()
        {
            int recordCount=FillItems<T>(out items, pageSize);
            item = items.Count > 0 ? items[0] : new T();
            return recordCount;
        }

        /// <summary>
        /// 填充数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        public int FillItems<T>(out T item, int pageSize) where T : class, new()
        {
            List<T> items = new List<T>();
            int recordCount=FillItems<T>(out items, pageSize);
            item = items.Count > 0 ? items[0] : new T();
            return recordCount;
        }



        /// <summary>
        /// 取得内容模型的数据
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="row"></param>
        public int FillItems(out DataRowCollection items, out DataRow item,string modelName,int pageSize)
        {
            int recordCount=0;
            DataTable records = GetRecords((DataTable data) =>
            {
                recordCount=data.Rows.Count;
                ModelInfo model = ModelHelper.GetModelInfo(modelName);
                We7DataTable mt = model.DataSet.Tables[0];
                DataSet ds = ModelHelper.CreateDataSet(model);
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < pageSize && i < recordCount;i++)
                {
                    DataRow row = data.Rows[i];
                    DataRow r = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        //如果存在映射字段，则添加映射数据，否则根据数据类型来取得值
                        if (data.Columns.Contains(mt.GetDesignField(dc.ColumnName)))
                        {
                            r[dc] = row[mt.GetDesignField(dc.ColumnName)];
                        }
                        else
                        {
                            //根据类型相同，并且数据长度离得最近的值作为近似值
                            int distance = int.MaxValue;
                            DataColumn cur = null;
                            foreach (DataColumn dc2 in data.Columns)
                            {
                                if (dc.DataType == dc2.DataType)
                                {
                                    int distance2 = Math.Abs(dc2.MaxLength - dc.MaxLength);
                                    if (distance2 < distance)
                                    {
                                        distance = distance2;
                                        cur = dc2;
                                    }
                                }
                            }
                            if (cur != null)
                            {
                                r[dc] = row[cur];
                            }
                        }
                    }
                    dt.Rows.Add(r);
                }
                return dt;
            });
            items = recoreds.Rows;
            item = recoreds.Rows.Count > 0 ? recoreds.Rows[0] : recoreds.NewRow();
            return recordCount;
        }

        /// <summary>
        /// 取得内容模型的数据
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="row"></param>
        public int FillItems(out DataRow item, string modelName)
        {
            DataRowCollection items;
            return FillItems(out items, out item, modelName,1);
        }

        /// <summary>
        /// 取得内容模型的数据
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="row"></param>
        public int FillItems(out DataRowCollection items, string modelName, int pageSize)
        {
            DataRow item;
            return FillItems(out items, out item, modelName, pageSize);
        }

        /// <summary>
        /// 取得缩略图
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string GetTagThumbnail(string tag)
        {
            string path = container.TemplateSourceDirectory + "/Data/Thumbnail";
            string thumb = GetThumbnail(path, tag);
            if (String.IsNullOrEmpty(thumb))
            {
                path = "~/Widgets/Data/Thumbnail";
                thumb = GetThumbnail(path, tag);
            }
            if (String.IsNullOrEmpty(thumb))
            {
                thumb = "/Admin/images/flower.jpg";
            }
            return thumb;
        }

        /// <summary>
        /// 取得路径
        /// </summary>
        /// <returns></returns>
        public string GetAttachment()
        {
            string path =container.TemplateSourceDirectory+"/Data/Attachment";
            string attachment = GetAttachment(path);
            if (String.IsNullOrEmpty(path))
            {
                path = "~/We7Controls/Data/Attachment";
                attachment = GetAttachment(path);
            }
            if (String.IsNullOrEmpty(attachment))
            {
                attachment = "/Admin/images/flower.jpg";
            }
            return attachment;
        }

        private string GetAttachment(string relativeDir)
        {
            string dir = Server.MapPath(relativeDir);
            if (Directory.Exists(dir))
            {
                DirectoryInfo di = new DirectoryInfo(dir);
                FileInfo[] fs = di.GetFiles("*.*",SearchOption.TopDirectoryOnly);
                if (fs != null && fs.Length > 0)
                {
                    Random r = new Random((int)DateTime.Now.Ticks);
                    int index = r.Next(fs.Length);
                    if (index >= fs.Length)
                        index = fs.Length - 1;
                    return container.ResolveUrl(Path.Combine(relativeDir, fs[index].Name));
                }
            }
            return String.Empty;
        }

        private string GetThumbnail(string relativeDir,string tag)
        {
            string dir=Server.MapPath(relativeDir);

            if (Directory.Exists(dir))
            {
                DirectoryInfo di = new DirectoryInfo(dir);
                string parttern = String.IsNullOrEmpty(tag) ? "*.*" : ("*_" + tag + ".*");
                FileInfo[] fs=di.GetFiles(parttern, SearchOption.TopDirectoryOnly);
                if (fs != null && fs.Length > 0)
                {
                    Random r = new Random((int)DateTime.Now.Ticks);
                    int index=r.Next(fs.Length);
                    if (index >= fs.Length)
                        index = fs.Length - 1;
                    return container.ResolveUrl(Path.Combine(relativeDir, fs[index].Name));
                }
            }
            return String.Empty;
        }

        private DataTable GetRecords(int pageSize, out int recordCount)
        {
            recordCount = Records.Rows.Count;
            DataTable dt = Records.Clone();
            for (int i = 0; i < recordCount && i < pageSize; i++)
            {
                dt.ImportRow(Records.Rows[i]);
            }
            return dt;
        }

        private DataTable ReadData(string file, string schema)
        {
            DataTable dt = null;
            if (File.Exists(file) && File.Exists(schema))
            {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(schema);
                ds.ReadXml(file);
                dt = ds.Tables[0];
            }
            return dt;
        }

        /// <summary>
        /// 从公共数据中格式化数据
        /// </summary>
        private void OnConvertCommonData()
        {
            if (ConvertCommonData != null)
            {
                recoreds = ConvertCommonData(CommonData.Copy());
            }
            else
            {
                recoreds = CommonData.Copy();
            }
        }



        #region 辅助属性

        private static HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        private static HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private static HttpServerUtility Server
        {
            get { return HttpContext.Current.Server; }
        }

        #endregion
    }
}
