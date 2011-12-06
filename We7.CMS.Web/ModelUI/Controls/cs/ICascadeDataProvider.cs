using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using We7.Model.Core;
using We7.Framework.Util;
using Thinkment.Data;
using We7.CMS;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;

namespace We7.Model.UI.Controls.cs
{
    /// <summary>
    /// 联动数据据接口
    /// </summary>
    public interface ICascadeDataProvider
    {
        Dictionary<string, string> QuryFirstLevel(string value);

        Dictionary<string, string> QuerySecondLevel(string value);

        Dictionary<string, string> QueryThirdLevel(string firstValue, string secondValue);
    }

    /// <summary>
    /// 用于从xml取得联动数据
    /// </summary>
    public  class XmlCascadeDataProvider : ICascadeDataProvider
    {

        private string xmlpath,nodesName,attributesName, rootNode = "root", firstNode = "first", secondNode = "second", thirdNode = "third", keyName = "name", valueName = "value";
        private string field1TextMapping, field1ValueMapping, field2TextMapping, field2ValueMapping, field3TextMapping, field3ValueMapping, tableName;

        public XmlCascadeDataProvider(ParamCollection parameters)
        {
            InitField(parameters);
        }

        public XmlCascadeDataProvider() { }

        /// <summary>
        /// 设置xml文件路径
        /// </summary>
        public string XmlPath
        {
            set { xmlpath = value; }
        }
        public string NodesName
        {
            set { nodesName = value; SetNodesName(nodesName); }
        }
        public string AttributesName
        {
            set { attributesName = value; SetAttributesName(attributesName); }
        }
        #region ICascadeDataProvider 成员

        public Dictionary<string, string> QuryFirstLevel(string value)
        {
            return Query(String.Format("{0}/{1}", rootNode, firstNode));
        }

        public Dictionary<string, string> QuerySecondLevel(string value)
        {
            return Query(String.Format("{0}/{1}[@" + valueName + "='{3}']/{2}", rootNode, firstNode, secondNode, value));
        }

        public Dictionary<string, string> QueryThirdLevel(string firstValue, string secondValue)
        {
            return Query(String.Format("{0}/{1}[@" + valueName + "='{4}']/{2}[@" + valueName + "='{5}']/{3}", rootNode, firstNode, secondNode, thirdNode, firstValue, secondValue));
        }

        #endregion

        void InitField(ParamCollection parameters)
        {
            xmlpath = parameters["data"];
            if (String.IsNullOrEmpty(xmlpath))
            {
                throw new Exception("没有指定联动控件的Xml路径");
            }
            xmlpath = HttpContext.Current.Server.MapPath(xmlpath);
            if (!File.Exists(xmlpath))
            {
                throw new Exception("不存在指定的数据联动文件");
            }
            //读取节点名称配置
            nodesName = parameters["NodesName"];
            SetNodesName(nodesName);
            //读取节点属性名配置
            attributesName = parameters["AttributesName"];
            SetAttributesName(attributesName);
        }

        /// <summary>
        /// 设置节点名配置
        /// </summary>
        void SetNodesName(string nodesName)
        {
            if (!String.IsNullOrEmpty(nodesName))
            {
                string[] nodesNameArray = nodesName.Split(new char[] { ',' });
                if (nodesNameArray.Length > 0)
                {
                    rootNode = nodesNameArray[0];
                    if (nodesNameArray.Length > 1)
                    {
                        firstNode = nodesNameArray[1];
                        if (nodesNameArray.Length > 2)
                        {
                            secondNode = nodesNameArray[2];
                            if (nodesNameArray.Length > 3)
                            {
                                thirdNode = nodesNameArray[3];
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 设置属性名配置
        /// </summary>
        void SetAttributesName(string attributesName)
        {
            if (!String.IsNullOrEmpty(attributesName))
            {
                string[] attributesNameArray = attributesName.Split(new char[] { ',' });
                if (attributesNameArray.Length > 0)
                {
                    keyName = attributesNameArray[0];
                    if (attributesNameArray.Length > 1)
                    {
                        valueName = attributesNameArray[1];
                    }
                }
            }
        }

        Dictionary<string, string> Query(string xpath)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            XmlNodeList nodes = XmlHelper.GetXmlNodeList(xmlpath, xpath);
            foreach (XmlElement xe in nodes)
            {
                string name = xe.GetAttribute(keyName);
                string value = xe.GetAttribute(valueName);
                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(value) && !dic.ContainsKey(value))
                {
                    dic.Add(name, value);
                }
                else if (!String.IsNullOrEmpty(name) && !dic.ContainsKey(name))
                {
                    dic.Add(name, name);
                }
                else if (!String.IsNullOrEmpty(value) && !dic.ContainsKey(value))
                {
                    dic.Add(value, value);
                }
            }
            return dic;
        }
    }


    /// <summary>
    /// 用于从xml取得联动数据
    /// </summary>
    public class DbCascadeDataProvider : ICascadeDataProvider
    {

        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {            
            get {return (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
            }
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
        private string field1TextMapping, field1ValueMapping, field2TextMapping, field2ValueMapping, field3TextMapping, field3ValueMapping, tableName;


        public DbCascadeDataProvider(ParamCollection parameters)
        {
            InitField(parameters);
        }

        public DbCascadeDataProvider() { }

        /// <summary>
        /// db数据库表名
        /// </summary>
        public string TableName
        {
            set { tableName = value; }
        }
        /// <summary>
        /// 一级控件文本对应数据库字段
        /// </summary>
        public string Field1TextMapping
        {
            set { field1TextMapping = value; }
        }
        /// <summary>
        /// 一级控件值对应数据库字段
        /// </summary>
        public string Field1ValueMapping
        {
            set { field1ValueMapping = value; }
        }
        /// <summary>
        /// 二级控件文本对应数据库字段
        /// </summary>
        public string Field2TextMapping
        {
            set { field2TextMapping = value; }
        }
        /// <summary>
        /// 二级控件值对应数据库字段
        /// </summary>
        public string Field2ValueMapping
        {
            set { field2ValueMapping = value; }
        }
        /// <summary>
        /// 三级控件文本对应数据库字段
        /// </summary>
        public string Field3TextMapping
        {
            set { field3TextMapping = value; }
        }
        /// <summary>
        /// 三级控件值对应数据库字段
        /// </summary>
        public string Field3ValueMapping
        {
            set { field3ValueMapping = value; }
        }

        #region ICascadeDataProvider 成员

        public Dictionary<string, string> QuryFirstLevel(string value)
        {
            return Query(field1TextMapping, field1ValueMapping, "");
        }

        public Dictionary<string, string> QuerySecondLevel(string firstValue)
        {
            string strWhere = "[" + field1ValueMapping + "]='" + firstValue + "' ";
            return Query(field1TextMapping, field1ValueMapping, strWhere);
        }

        public Dictionary<string, string> QueryThirdLevel(string firstValue, string secondValue)
        {
            string strWhere = "[" + field1ValueMapping + "]='" + firstValue + "' and [" + field2ValueMapping + "]='" + secondValue + "'";
            return Query(field2TextMapping, field2ValueMapping, strWhere);
        }

        #endregion

        void InitField(ParamCollection parameters)
        {
            //if (String.IsNullOrEmpty(field1TextMapping))
            //    throw new Exception("没有指定联动控件的一级控件文本对应数据库字段");

        }

        Dictionary<string, string> Query(string textField, string valueField, string where)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string sqlField1 = " SELECT DISTINCT [" + textField + "],[" + valueField + "] FROM [" + tableName + "]  ";
            if(!string.IsNullOrEmpty(where))
            {
                sqlField1 += " where " + where;
            }
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            SqlStatement sqlstatement = new SqlStatement();
            sqlstatement.SqlClause = sqlField1;
            db.DbDriver.FormatSQL(sqlstatement);
            DataTable dt = new DataTable();
            using (IConnection conn = db.CreateConnection())
            {
                dt = conn.Query(sqlstatement);
            }
            foreach(DataRow row in dt.Rows)
            {
                string name =  row[textField].ToString();
                string value = row[valueField].ToString();
                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(value) && !dic.ContainsKey(value))
                {
                    dic.Add(name, value);
                }
                else if (!String.IsNullOrEmpty(name) && !dic.ContainsKey(name))
                {
                    dic.Add(name, name);
                }
                else if (!String.IsNullOrEmpty(value) && !dic.ContainsKey(value))
                {
                    dic.Add(value, value);
                }
            }
            return dic;
        }
    }
}
