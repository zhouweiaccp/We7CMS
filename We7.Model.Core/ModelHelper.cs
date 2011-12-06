using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using We7.Framework.Util;
using System.Web;
using System.IO;
using We7.Model.Core.Config;
using System.Web.UI.WebControls;
using We7.Model.Core.Data;
using We7.Framework;
using We7.Model.Core.Converter;
using We7.Model.Core.UI;
using System.Xml;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace We7.Model.Core
{
    /// <summary>
    /// 模型业务类
    /// </summary>
    public partial class ModelHelper
    {
        /// <summary>
        /// 默认的ListControl
        /// </summary>
        const string DEFAULTLISTCONTROL = "We7.Model.Core.ListControl.HtmlField,We7.Model.Core";
        public const string OBJECTCOLUMN = "Object";

        public static PanelContext GetPanelContext(string modelName, string panelName)
        {
            ModelInfo model = GetModelInfo(modelName);
            PanelContext ctx = model[panelName];
            ctx.Objects = new Hashtable();
            ctx.Row = new DataFieldCollection();
            ctx.PageIndex = 0;
            ctx.Model = model;
            return model[panelName];
        }

        /// <summary>
        /// 根据模型信息创建数据集
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataSet CreateDataSet(string modeltype)
        {
            ModelInfo model = GetModelInfo(modeltype);
            DataSet ds = new DataSet("We7Data");
            DataTable dt = new DataTable(model.DataSet.Tables[0].Name);
            ds.Tables.Add(dt);

            foreach (We7DataColumn field in model.DataSet.Tables[0].Columns)
            {
                dt.Columns.Add(field.Name, Type.GetType("System." + field.DataType.ToString()));
            }
            dt.Columns.Add(OBJECTCOLUMN, typeof(object));
            return ds;
        }

        public static DataSet ReadXml(string modelxml, string modelSchema)
        {
            if (String.IsNullOrEmpty(modelxml) || String.IsNullOrEmpty(modelSchema))
                return null;

            DataSet ds = new DataSet();
            using (TextReader reader = new StringReader(modelSchema))
            {
                ds.ReadXmlSchema(reader);
            }
            using (TextReader reader = new StringReader(modelxml))
            {
                ds.ReadXml(reader);
            }
            return ds;
        }

        /// <summary>
        /// 根据模型类型获取模型路径
        /// </summary>
        /// <param name="modeltype">模型类型</param>
        /// <returns>模型路径</returns>
        public static string GetModelPath(string modeltype)
        {
            modeltype = modeltype.Contains(".") ? modeltype.Replace(".", "/") : String.Format("system/{0}", modeltype);
            string path = "";

            if (modeltype.StartsWith("Template/", StringComparison.CurrentCultureIgnoreCase))
            {
                path = Utils.GetMapPath(ModelConfig.BaseModelDirectory + "/" + modeltype + ".xml");
            }
            else
            {
                path = Utils.GetMapPath(ModelConfig.ModelsDirectory + "/" + modeltype + ".xml");
            }
            return path;
        }

        /// <summary>
        /// 根据模型类型取得模型数据
        /// </summary>
        /// <param name="modelname">根据模型名称拿模型信息</param>
        /// <returns></returns>
        public static ModelInfo GetModelInfo(string modelname)
        {
            string modelcacheid = String.Format("model_{0}", modelname);
            ModelInfo model = AppCtx.Cache.RetrieveObject<ModelInfo>(modelcacheid);
            if (model == null)
            {
                string path = GetModelPath(modelname);
                if (File.Exists(path))
                {
                    model = SerializationHelper.Load(typeof(ModelInfo), path) as ModelInfo;
                    model.ModelName = GetModelName(modelname);
                    AppCtx.Cache.AddObjectWithFileChange(modelcacheid, model, path); 
                }
            }
            return model;
        }

        /// <summary>
        /// 用数据行更新模型数据
        /// </summary>
        /// <param name="data">模型数据</param>
        /// <param name="row"></param>
        public static void UpdateFields(PanelContext data, DataRow row)
        {
            data.Row.Clear();
            if (row == null)
            {
                foreach (We7DataColumn field in data.Table.Columns)
                {
                    data.Row[field] = null;
                }
            }
            else
            {
                foreach (We7DataColumn field in data.Table.Columns)
                {
                    if (field.Direction != ParameterDirection.ReturnValue)
                        data.Row[field] = row[field.Name];
                }
            }
        }

        /// <summary>
        /// 获取模型数据控件
        /// </summary>
        /// <param name="lcontrol"></param>
        /// <returns></returns>
        public static ModelControlField GetDataControl(string listcontrol)
        {
            string type = String.IsNullOrEmpty(listcontrol) ? DEFAULTLISTCONTROL : ModelConfig.ListControls[listcontrol];
            return Utils.CreateInstance<ModelControlField>(type);
        }

        /// <summary>
        /// 扩展数据表格
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="data"></param>
        public static void ExtendDataTable(DataTable dt, PanelContext data)
        {
            ExtendDataTable(dt, data.Table.Columns);
        }

        /// <summary>
        /// 扩展数据表格
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dcc"></param>
        public static void ExtendDataTable(DataTable dt, We7DataColumnCollection dcc)
        {
            foreach (We7DataColumn column in dcc)
            {
                if (!dt.Columns.Contains(column.Name) && (column.Direction == ParameterDirection.ReturnValue || column.Direction == ParameterDirection.Output || column.Direction == ParameterDirection.InputOutput))
                {
                    dt.Columns.Add(column.Name);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                if (!row.Table.Columns.Contains("Object"))
                    continue;
                foreach (We7DataColumn column in dcc)
                {
                    IOutputConvert convert = null;
                    string[] fields;

                    if (column.Direction == ParameterDirection.InputOutput || column.Direction == ParameterDirection.Output)
                    {
                        fields = new string[] { String.IsNullOrEmpty(column.Mapping) ? column.Name : column.Mapping };
                        convert = new GetEntityValue();
                        if (row[column.Name] == DBNull.Value)
                        {
                            row[column.Name] = convert.Convert(column, row, fields, row["Object"]);
                        }
                    }
                    else if (column.Direction == ParameterDirection.ReturnValue)
                    {
                        convert = ConvertHelper.GetOutputConvert(column.Expression, out fields);
                        row[column.Name] = convert.Convert(column, row, fields, row["Object"]);
                    }
                }
            }
        }

        public static string GetModelName(string name)
        {
            return name.Contains(".") ? name : String.Format("System.{0}", name);
        }

        /// <summary>
        /// 通过模型名取得数据加构XML
        /// </summary>
        /// <param name="modelname"></param>
        /// <returns></returns>
        public static string GetModelSchema(string modelname)
        {
            DataSet ds = CreateDataset(modelname);
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                ds.WriteXmlSchema(writer);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 取得模型XML
        /// </summary>
        /// <param name="modelname"></param>
        /// <returns></returns>
        public static string GetModelConfigXml(string modelname)
        {
            string path = GetModelPath(modelname);
            if (!File.Exists(path))
                return null;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return doc.OuterXml;
        }

        /// <summary>
        /// 根据模型名创建数据集
        /// </summary>
        /// <param name="modelname"></param>
        /// <returns></returns>
        public static DataSet CreateDataset(string modelname)
        {
            ModelInfo model = GetModelInfo(modelname);
            return CreateDataSet(model);
        }

        /// <summary>
        /// 根据模型创建数据集
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataSet CreateDataSet(ModelInfo model)
        {
            DataSet ds = new DataSet("We7Data");
            DataTable dt = new DataTable(model.DataSet.Tables[0].Name);
            ds.Tables.Add(dt);

            foreach (We7DataColumn field in model.DataSet.Tables[0].Columns)
            {
                dt.Columns.Add(field.Name, Type.GetType("System." + field.DataType.ToString()));
                if (field.MaxLength > 0)
                {
                    dt.Columns[field.Name].MaxLength = field.MaxLength;
                }
            }
            dt.Columns.Add(OBJECTCOLUMN, typeof(object));
            return ds;
        }

        /// <summary>
        /// 取得操作类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static OperationType GetOperation(string type)
        {
            if (String.IsNullOrEmpty(type))
                return OperationType.EQUER;
            type = type.Trim().ToLower();
            if (type == "=")
            {
                return OperationType.EQUER;
            }
            else if (type == "<>")
            {
                return OperationType.NOTEQUER;
            }
            else if (type == "like")
            {
                return OperationType.LIKE;
            }
            else if (type == ">")
            {
                return OperationType.MORETHAN;
            }
            else if (type == "<")
            {
                return OperationType.LESSTHAN;
            }
            else if (type == ">=")
            {
                return OperationType.MORETHANEQURE;
            }
            else if (type == "<=")
            {
                return OperationType.LESSTHANEQURE;
            }
            else
            {
                return OperationType.EQUER;
            }
        }

        public static Dictionary<string, string> GetModelLayout(string modeltype)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] ss = modeltype.Split('.');
            string groupName = modeltype.Contains(".") ? ss[0] : "System";
            string modelName = modeltype.Contains(".") ? ss[1] : ss[0];
            string path = HttpContext.Current.Server.MapPath(Path.Combine(ModelConfig.ModelsDirectory, groupName + "/" + modelName));  //Utils.MapPath Path.Combine(ModelConfig.ModelsDirectory, groupName, modelName);

            if (Directory.Exists(path))
            {
                FileInfo[] files = new DirectoryInfo(path).GetFiles("*.ascx", SearchOption.TopDirectoryOnly);
                Regex regex = new Regex("(?<=<!-+#+)[^#].*?(?=#+-+>)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Regex regexName = new Regex("(?<=name\\s*=\\s*['|\"]).*?(?=['|\"])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                foreach (FileInfo f in files)
                {
                    using (StreamReader reader = f.OpenText())
                    {
                        string s = reader.ReadToEnd();
                        Match mc = regex.Match(s);
                        if (mc != null && mc.Success)
                        {
                            Match m = regexName.Match(mc.Value);
                            if (m != null && m.Success)
                            {
                                dic.Add(String.Format("{0}/{1}/{2}/{3}", ModelConfig.ModelsDirectory, groupName, modelName, f.Name), m.Value);
                            }
                        }
                    }
                }
            }
            return dic;
        }

        public static Dictionary<string, string> GetModelLayoutCss(string modeltype,string ctrName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] ss = modeltype.Split('.');
            string groupName = modeltype.Contains(".") ? ss[0] : "System";
            string modelName = modeltype.Contains(".") ? ss[1] : ss[0];
            string path = HttpContext.Current.Server.MapPath(Path.Combine(ModelConfig.ModelsDirectory, groupName + "/" + modelName));  //Utils.MapPath Path.Combine(ModelConfig.ModelsDirectory, groupName, modelName);

            if (Directory.Exists(path))
            {
                FileInfo[] files = new DirectoryInfo(path).GetFiles(ctrName + ".*.css", SearchOption.TopDirectoryOnly);
                foreach (FileInfo f in files)
                {
                    string name=Path.GetFileNameWithoutExtension(f.Name);
                    dic.Add(String.Format("{0}/{1}/{2}/{3}", ModelConfig.ModelsDirectory.Replace("~/", "/"), groupName, modelName, f.Name), name.Split('.')[1]);
                }
            }
            return dic;
        }
    }
}
