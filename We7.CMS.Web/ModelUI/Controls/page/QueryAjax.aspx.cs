using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Thinkment.Data;
using System.Data;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS;
using We7.Model.Core;
using System.Xml.Serialization;
using We7.Model.UI.Controls.cs;

namespace We7.Model.UI.Controls.page
{
    public partial class QueryAjax : System.Web.UI.Page
    {
        private ParamCollection _Params = new ParamCollection();
        /// <summary>
        /// 参数集合
        /// </summary>
        [XmlElement("param")]
        public ParamCollection Params
        {
            get { return _Params; }
            set { _Params = value; }
        }

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

        /// <summary>
        /// 数据访问对象
        /// </summary>
        protected ObjectAssistant Assistant
        {
            get { return ArticleHelper.Assistant; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //清空缓存
                Response.CacheControl = "no-cache";
                Response.Expires = 0;


                //联动控件加载二级菜单数据
                if (!string.IsNullOrEmpty(Request["name"]) && Request["name"] == "DoubleCascade.ascx" && !string.IsNullOrEmpty(Request["field1Value"]))
                {
                    if (String.IsNullOrEmpty(Request["type"]) || Request["type"] == "xml")
                    {
                        ProcessXml2LevelCascade();
                    }
                    else
                    {
                        GetDoubleCascadeField2();
                    }

                }
                //联动控件加载三级菜单数据
                else if (!string.IsNullOrEmpty(Request["name"]) && Request["name"] == "ThreeCascade.ascx" && !string.IsNullOrEmpty(Request["field2Value"]))
                {
                    if (String.IsNullOrEmpty(Request["type"]) || Request["type"] == "xml")
                    {
                        ProcessXml3LevelCascade();
                    }
                    else
                    {
                        GetThreeCascadeField3();
                    }                    
                }

            }
        }

        /// <summary>
        /// xml的二级数据
        /// </summary>
        void ProcessXml2LevelCascade()
        {
            string path = Server.MapPath(HttpUtility.HtmlEncode(Request["path"]));
            string nodesName = HttpUtility.HtmlEncode(Request["nodesName"]);
            string attributesName = HttpUtility.HtmlEncode(Request["attributesName"]);
            ICascadeDataProvider provider = new XmlCascadeDataProvider() { XmlPath = path, NodesName = nodesName, AttributesName = attributesName };
            Dictionary<string, string> dic = provider.QuerySecondLevel(Server.UrlDecode(Request["field1Value"]));
            //将数据拼接到一个字符串
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.Append(kvp.Key + "&" + kvp.Value + ",");
            }
            if (sb.Length > 1)
            {
                if (sb.ToString().Substring(sb.Length - 1, 1) == ",")
                {
                    sb.Remove(sb.ToString().Length - 1, 1);
                }
            }
            Response.Clear();
            Response.Write(sb.ToString());
            Response.End();
        }

        /// <summary>
        /// xml的三级数据
        /// </summary>
        void ProcessXml3LevelCascade()
        {
            string path = Server.MapPath(Server.UrlDecode(Request["path"]));
            string nodesName = HttpUtility.HtmlEncode(Request["nodesName"]);
            string attributesName = HttpUtility.HtmlEncode(Request["attributesName"]);
            ICascadeDataProvider provider = new XmlCascadeDataProvider() { XmlPath = path, NodesName = nodesName, AttributesName = attributesName };
            Dictionary<string, string> dic = provider.QueryThirdLevel(Server.UrlDecode(Request["field1Value"]),Server.UrlDecode(Request["field2Value"]));
            //将数据拼接到一个字符串
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                sb.Append(kvp.Key + "&" + kvp.Value + ",");
            }
            if (sb.Length > 1)
            {
                if (sb.ToString().Substring(sb.Length - 1, 1) == ",")
                {
                    sb.Remove(sb.ToString().Length - 1, 1);
                }
            }
            Response.Clear();
            Response.Write(sb.ToString());
            Response.End();
        }

        /// <summary>
        /// 联动控件加载二级菜单数据
        /// </summary>
        /// <param name="field2TextMapping">二级菜单文本字段</param>
        /// <param name="field2ValueMapping">二级菜单值字段</param>
        /// <param name="table">查询表名</param>
        /// <param name="field1ValueMapping">一级菜单值字段</param>
        /// <param name="field1">一级菜单当前选中的值</param>
        private void GetDoubleCascadeField2()
        {            
            string field2TextMapping = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["field2TextMapping"]));
            string field2ValueMapping = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["field2ValueMapping"]));
            string field1ValueMapping = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["field1ValueMapping"]));
            string table = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["table"]));
            string field1Value = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["field1Value"]));

            //加载数据
            string sqlField2 = @" SELECT DISTINCT [" + field2TextMapping + "],[" + field2ValueMapping + "] FROM [" + table + "] WHERE [" + field1ValueMapping + "]='" + field1Value + "'  ";//WHERE [ID]={0}            
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            SqlStatement sqlstatement = new SqlStatement();
            sqlstatement.SqlClause = sqlField2;
            db.DbDriver.FormatSQL(sqlstatement);
            DataTable dt = new DataTable();
            using (IConnection conn = db.CreateConnection())
            {
                dt = conn.Query(sqlstatement);
            }
            //将数据拼接到一个字符串
            StringBuilder sb = new StringBuilder();
            foreach(DataRow row in dt.Rows)
            {
                sb.Append(row["" + field2TextMapping + ""] + "&" + row["" + field2ValueMapping + ""] + ",");
            }
            if(sb.Length > 1)
            {
                if (sb.ToString().Substring(sb.Length - 1, 1) == ",")
                {
                    sb.Remove(sb.ToString().Length - 1, 1);
                }                
            }
            Response.Clear();
            Response.Write(sb.ToString());
            Response.End();
        }


        /// <summary>
        /// 联动控件加载三级菜单数据
        /// </summary>
        /// <param name="field2TextMapping">二级菜单文本字段</param>
        /// <param name="field2ValueMapping">二级菜单值字段</param>
        /// <param name="table">查询表名</param>
        /// <param name="field1ValueMapping">一级菜单值字段</param>
        /// <param name="field1">一级菜单当前选中的值</param>
        private void GetThreeCascadeField3()
        {
            string field3TextMapping = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["field3TextMapping"]));
            string field3ValueMapping = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["field3ValueMapping"]));
            string field2ValueMapping = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["field2ValueMapping"]));
            string field1ValueMapping = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["field1ValueMapping"]));
            string table = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["table"]));
            string field1Value = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["field1Value"]));
            string field2Value = We7Helper.FilterHtmlChars(Server.UrlDecode(Request["field2Value"]));
            //加载数据
            string sqlField3 = @" SELECT DISTINCT [" + field3TextMapping + "],[" + field3ValueMapping + "] FROM [" + table + "] WHERE [" + field2ValueMapping + "]='" + field2Value + "' and [" + field1ValueMapping + "]='" + field1Value + "'  ";//WHERE [ID]={0}            
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            SqlStatement sqlstatement = new SqlStatement();
            sqlstatement.SqlClause = sqlField3;
            db.DbDriver.FormatSQL(sqlstatement);
            DataTable dt = new DataTable();
            using (IConnection conn = db.CreateConnection())
            {
                dt = conn.Query(sqlstatement);
            }
            //将数据拼接到一个字符串
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(row["" + field3TextMapping + ""] + "&" + row["" + field3ValueMapping + ""] + ",");
            }
            if (sb.Length > 1)
            {
                if (sb.ToString().Substring(sb.Length - 1, 1) == ",")
                {
                    sb.Remove(sb.ToString().Length - 1, 1);
                }
            }
            Response.Clear();
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}
