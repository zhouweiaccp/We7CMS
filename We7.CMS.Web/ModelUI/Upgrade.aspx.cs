using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Thinkment.Data;
using We7.CMS;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using System.Data;
using We7.Model.Core;
using We7.CMS.Common;
using System.IO;
using We7.Model.UI.Data;
using System.Text;
using System.Xml;
using System.Reflection;
using We7.Framework.Util;

namespace We7.Model.UI
{
    public partial class Upgrade : System.Web.UI.Page
    {
        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        /// <summary>
        /// 文章业务对象
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }      
        protected void bttnUpgrade_Click(object sender, EventArgs e)
        {
            int total = 0, success = 0, error = 0,jump=0,empty=0;
            List<Article> list=ArticleHelper.GetAllArticle();
            foreach (Article a in list)
            {
                try
                {
                    if (!String.IsNullOrEmpty(a.ModelName))
                    {
                        total++;
                        if (a.ModelName.Contains("Course."))
                        {
                            jump++;
                            Log("****************************跳过一条记录*****************************");
                            continue;
                        }
                        if (String.IsNullOrEmpty(a.ModelXml))
                        {
                            empty++;
                            ArticleHelper.DeleteArticle(a.ID);
                            Log("+++++++++++++++++++++++++++删除一条空记录+++++++++++++++++++++");
                            continue;
                        }
                        List<string> fields = new List<string>();
                        ModelInfo model = ModelHelper.GetModelInfo("Course." + a.ModelName);
                        DataSet ds = BaseDataProvider.CreateDataSet(model);
                        a.ModelName = model.ModelName;
                        a.TableName = model.DataSet.Tables[0].Name;
                        a.ModelConfig = File.ReadAllText(ModelHelper.GetModelPath(a.ModelName));

                        StringBuilder sb = new StringBuilder();
                        StringWriter writer = new StringWriter(sb);
                        ds.WriteXmlSchema(writer);
                        a.ModelSchema = sb.ToString();
                        fields.AddRange(new string[] { "ModelName", "TableName", "ModelConfig", "ModelSchema", "ModelXml" });

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(a.ModelXml);

                        DataRow row = ds.Tables[0].NewRow();
                        ds.Tables[0].Rows.Add(row);

                        foreach (We7DataColumn dc in model.DataSet.Tables[0].Columns)
                        {

                            if (dc.Direction == ParameterDirection.ReturnValue)
                                continue;
                            List<string> exclude = new List<string>() { "OwnerID", "State", "IsShow", "Source", "ContentType" };
                            if (exclude.Contains(dc.Mapping))
                                continue;
                            if (dc.Mapping == "ID")
                            {
                                row[dc.Name] = a.ID;
                                continue;
                            }
                            XmlElement xe = doc.SelectSingleNode("//" + dc.Name) as XmlElement;
                            if (xe != null)
                            {
                                object value = TypeConverter.StrToObjectByTypeCode(xe.InnerText.Trim(), dc.DataType);
                                if (dc.Direction == ParameterDirection.Output || dc.Direction == ParameterDirection.InputOutput)
                                {
                                    string mapping = String.IsNullOrEmpty(dc.Mapping) ? dc.Name : dc.Mapping;
                                    PropertyInfo pro = a.GetType().GetProperty(mapping);
                                    if (pro != null)
                                    {
                                        fields.Add(mapping);
                                        pro.SetValue(a, value, null);
                                    }
                                }
                                if (dc.Direction == ParameterDirection.InputOutput || dc.Direction == ParameterDirection.Input)
                                {
                                    row[dc.Name] = value;
                                }
                            }
                        }
                        StringBuilder sb2 = new StringBuilder();
                        StringWriter reader2 = new StringWriter(sb2);
                        ds.WriteXml(reader2);
                        a.ModelXml = sb2.ToString();
                        ArticleHelper.UpdateArticle(a, fields.ToArray());
                        success++;
                        Log("#################################更新成功#################################");
                    }
                }
                catch (Exception ex)
                {
                    Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!更新失败!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    Log(ex.Source);
                    Log(ex.Message);
                    Log(ex.StackTrace);
                    error++;
                }
            }

            Log("###################################---------当前操作共"+total+"条记录,成功"+success+"条,失败"+error+"条,跳过"+jump+"条,空记录"+empty+"条------------############################");
            //IDatabase db = ArticleHelper.Assistant.GetDatabases()["We7.CMS.Common"];
            //SqlStatement sqlstatement = new SqlStatement();
            //sqlstatement.SqlClause = "SELECT * FROME [Article]";
            //sqlstatement.CommandType = CommandType.Text;
            //db.DbDriver.FormatSQL(sqlstatement);
            //DataTable dt = new DataTable();
            //using (IConnection conn = db.CreateConnection())
            //{
            //    dt = conn.Query(sqlstatement);
            //}

            //foreach (DataRow row in dt.Rows)
            //{
            //    string modelName=row["ModelName"] as string;
            //    if (!String.IsNullOrEmpty(modelName))
            //    {
            //        ModelInfo model=ModelHelper.GetModelInfo("Course." + modelName);
                    
            //        ArticleHelper.UpdateArticle(null,null);
            //    }
            //}
        }
        void ClearLog()
        {
            string path = Server.MapPath("~/We7TransferLog.txt");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        void Log(string text)
        {
            Stream stream = null;
            try
            {
                string path = Server.MapPath("~/We7TransferLog.txt");

                if (!File.Exists(path))
                {
                    stream = File.Create(path);
                }
                else
                {
                    stream = File.Open(path, FileMode.Append);
                }
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("-----------------------------------------------------------------------------------");
                    writer.WriteLine("当前时间::" + DateTime.Now);
                    writer.WriteLine();
                    writer.Write(text);
                    writer.WriteLine();
                    writer.WriteLine("-----------------------------------------------------------------------------------");
                }
            }
            catch
            {
            }
            finally
            {
                try
                {
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
                catch
                {
                }
            }
        }
    }
}
