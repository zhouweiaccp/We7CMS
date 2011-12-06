using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Xml.Serialization;

namespace We7.Model.Core
{
    /// <summary>
    /// 控件版本
    /// </summary>
    public enum CtrVersion
    { 
        None,
        V26
    }

    /// <summary>
    /// 模型查询上下文件
    /// </summary>
    [Serializable]
    public class QueryContext
    {
        public QueryContext()
        {
        }

        public QueryContext(string modelName)
        {
            Model = ModelHelper.GetModelInfo(modelName);
        }

        private int pageSize = 10;
        /// <summary>
        /// 页记录数
        /// </summary>
        [XmlElement("pageSize")]
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        [XmlIgnore]
        public int PageIndex { get; set; }

        private string orders = "ID";
        /// <summary>
        /// 排序字符串
        /// </summary>
        [XmlElement("orders")]
        public string Orders
        {
            get { return orders; }
            set { orders = value; }
        }

        /// <summary>
        /// 当前模型名
        /// </summary>
        [XmlIgnore]
        public string ModelName
        {
            get { return Model.ModelName; }
        }

        /// <summary>
        /// 模型信息
        /// </summary>
        [XmlIgnore]
        public ModelInfo Model { get; set; }

        /// <summary>
        /// 数据集信息
        /// </summary>
        [XmlIgnore]
        public We7DataSet DataSet
        {
            get { return Model.DataSet; }
        }

        private DataFieldCollection row = new DataFieldCollection();
        /// <summary>
        /// 行记录
        /// </summary>
        [XmlIgnore]
        public DataFieldCollection Row
        {
            get
            {
                if (row.Table == null)
                {
                    row.Table = DataSet.Tables[0];
                }
                return row;
            }
            set { row = value; }
        }

        private QueryFieldCollection queryFields = new QueryFieldCollection();
        /// <summary>
        /// 查询字段
        /// </summary>
        [XmlIgnore]
        public QueryFieldCollection QueryFields
        {
            get
            {
                if (queryFields.Table == null)
                {
                    queryFields.Table = DataSet.Tables[0];
                }
                return queryFields;
            }
            set { queryFields = value; }
        }

        private We7DataTable table = null;
        /// <summary>
        /// 当前表信息
        /// </summary>
        [XmlIgnore]
        public We7DataTable Table
        {
            get
            {
                if (table == null)
                {
                    table = DataSet.Tables[0];
                }
                return table;
            }
        }

        /// <summary>
        /// 扩展对象数据集
        /// </summary>
        [XmlIgnore]
        [NonSerialized]
        public Hashtable Objects = new Hashtable();

    }

    /// <summary>
    /// 面板上下文信息
    /// </summary>
    [Serializable]
    public class PanelContext : QueryContext
    {
        private string dataKeyString = "";
        /// <summary>
        /// 主键字符串
        /// </summary>
        [XmlElement("dataKey")]
        public string DataKeyString
        {
            get { return dataKeyString; }
            set { dataKeyString = value; }
        }

        /// <summary>
        /// 主键字段集合
        /// </summary>
        [XmlIgnore]
        public string[] DataKeys
        {
            get
            {
                if (!String.IsNullOrEmpty(DataKeyString))
                {
                    return DataKeyString.Split(',');
                }
                return null;
            }
        }

        /// <summary>
        /// 当前面板名
        /// </summary>
        [XmlIgnore]
        public string PanelName
        {
            get { return Panel.Name; }
        }

        /// <summary>
        /// 当前面板配置信息
        /// </summary>
        [XmlIgnore]
        public Panel Panel { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataKey dataKey = null;
        /// <summary>
        /// 主键
        /// </summary>
        [XmlIgnore]
        public DataKey DataKey
        {
            get { return dataKey; }
            set { dataKey = value; }
        }

        /// <summary>
        /// 扩展状态
        /// </summary>
        [XmlIgnore]
        public object State;

        /// <summary>
        /// 控件版本
        /// </summary>
        [XmlIgnore]
        public CtrVersion CtrVersion { get; set; }
    }
}
