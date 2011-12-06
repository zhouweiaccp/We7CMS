using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace We7.Model.Core
{
    /// <summary>
    /// 表集合
    /// </summary>
    [Serializable]
    public class We7DataTableCollection : Collection<We7DataTable>
    {
        /// <summary>
        /// 索引，根据表名取得数据表
        /// </summary>
        /// <param name="name">表名</param>
        /// <returns>表名对应的数据表</returns>
        public We7DataTable this[string name]
        {
            get
            {
                foreach (We7DataTable dt in this)
                {
                    if (dt.Name == name)
                        return dt;
                }
                return null;
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Name==name)
                    {
                        this[i] = value;
                    }
                }
            }
        }

        public void AddOrUpdate(We7DataTable dateTable)
        {
            if (this[dateTable.Name] != null)
            {
                this[dateTable.Name] = dateTable;
            }
            else
            {
                this.Add(dateTable);
            }
        }
    }

    /// <summary>
    /// 数据表配置信息
    /// </summary>
    [Serializable]
    public class We7DataTable
    {
        /// <summary>
        /// 表名
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        private We7DataColumnCollection columns = new We7DataColumnCollection();
        /// <summary>
        /// 字段集合
        /// </summary>
        [XmlElement("dataColumn")]
        public We7DataColumnCollection Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        /// <summary>
        /// 取得设计时字段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetDesignField(string name)
        {
            foreach (We7DataColumn dc in Columns)
            {
                if (String.Compare(name, dc.Name, true) == 0)
                {
                    return String.IsNullOrEmpty(dc.DesignField) ? dc.Name : dc.DesignField;
                }
            }
            return String.Empty;
        }

    }
}
