using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace We7.Model.Core.Entity
{
    /// <summary>
    /// 数据驱动信息
    /// </summary>
    public class DbProviderInfo
    {
        /// <summary>
        /// 数据驱动的模型类型
        /// </summary>
        [XmlAttribute("type")]
        public ModelType Type { get; set; }

        [XmlAttribute("provider")]
        public string Provider { get; set; }

        private NameTypeCollection items = new NameTypeCollection();
        /// <summary>
        /// 数据驱动类型
        /// </summary>
        [XmlElement("cmd")]
        public NameTypeCollection Items
        {
            get { return items; }
            set { items = value; }
        }
    }

    /// <summary>
    /// 数据驱动信息集合
    /// </summary>
    public class DbProviderInfoCollection : Collection<DbProviderInfo>
    {
        public DbProviderInfo this[ModelType type]
        {
            get
            {
                foreach (DbProviderInfo info in this)
                {
                    if (info.Type == type)
                        return info;
                }
                return null;
            }
        }
    }
}
