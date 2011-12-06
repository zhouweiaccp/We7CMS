using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace We7.Model.Core
{
    /// <summary>
    /// 数据集信息
    /// </summary>
    [Serializable]
    public class We7DataSet
    {
        private We7DataTableCollection tables = new We7DataTableCollection();
        /// <summary>
        /// 表集合
        /// </summary>
        [XmlElement("dataTable")]
        public We7DataTableCollection Tables
        {
            get { return tables; }
            set { tables = value; }
        }
    }
}
