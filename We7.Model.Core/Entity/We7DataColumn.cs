using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using We7.Framework.Util;

namespace We7.Model.Core
{
    /// <summary>
    /// 字段集合
    /// </summary>
    [Serializable]
    public class We7DataColumnCollection : Collection<We7DataColumn>
    {
        /// <summary>
        /// 索引，根据字段名查询字段
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns>字段信息</returns>
        public We7DataColumn this[string name]
        {
            get
            {
                foreach (We7DataColumn dc in this)
                {
                    if (dc.Name == name)
                        return dc;
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

        public void AddOrUpdate(We7DataColumn column)
        {
            if (this[column.Name]!=null)
            {
                this[column.Name] = column;
            }
            else
            {
                this.Add(column);
            }
        }

        /// <summary>
        /// 是否包含指定列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return Contains(key, false);
        }

        /// <summary>
        /// 是否包含指定列
        /// </summary>
        /// <param name="key">列名</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <returns></returns>
        public bool Contains(string key, bool ignoreCase)
        {
            foreach (We7DataColumn dc in this)
            {
                if (String.Compare(dc.Name, key, ignoreCase) == 0)
                    return true;
            }
            return false;
        }

        public We7DataColumn IndexOfMappingField(string field)
        {
            foreach (We7DataColumn dc in this)
            {
                if (dc.Direction == ParameterDirection.InputOutput || dc.Direction == ParameterDirection.Output)
                {
                    string mappingfield = !String.IsNullOrEmpty(dc.Mapping) ? dc.Mapping : dc.Name;
                    if (mappingfield == field)
                        return dc;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 字段信息
    /// </summary>
    [Serializable]
    public class We7DataColumn
    {
        /// <summary>
        /// 字段名
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        //tedyding add
        /// <summary>
        /// 显示名称
        /// </summary>
        [XmlAttribute("label")]
        public string Label { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        [XmlAttribute("dataType")]
        public TypeCode DataType { get; set; }

        /// <summary>
        /// 表达示
        /// </summary>
        [XmlAttribute("expression")]
        public string Expression { get; set; }

        /// <summary>
        /// 数据方向
        /// </summary>
        [XmlAttribute("direction")]
        public ParameterDirection Direction { get; set; }

        /// <summary>
        /// 当前字段是否为系统字段
        /// </summary>
        [XmlAttribute("system")]
        public bool IsSystem { get; set; }

        /// <summary>
        /// 映射字段
        /// </summary>
        [XmlAttribute("mapping")]
        public string Mapping { get; set; }

        /// <summary>
        /// 数据的最大长度
        /// </summary>
        [XmlAttribute("maxlength")]
        public int MaxLength { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [XmlAttribute("default")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是否可以为空
        /// </summary>
        [XmlAttribute("nullable")]
        public bool Nullable { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [XmlIgnore]
        public object DefaultObject
        {
            get
            {
                if (TypeCode.DateTime == DataType&&String.Compare("now", DefaultValue, true) == 0)
                {
                    return DateTime.Now;
                }
                return TypeConverter.StrToObjectByTypeCode(DefaultValue, DataType);
            }
        }

        //tedyding add


        [XmlAttribute("require")]
        public bool Require { get; set; }

        [XmlAttribute("edit")]
        public bool Edit { get; set; }
        [XmlAttribute("list")]
        public bool List { get; set; }

        /// <summary>
        /// 设计时字段
        /// </summary>
        [XmlAttribute("design")]
        public string DesignField { get; set; }


        private ParamCollection _Params=new ParamCollection();
        /// <summary>
        /// 字段参数
        /// </summary>
        [XmlElement("param")]
        public ParamCollection Params
        {
            get { return _Params;  }
            set { _Params = value; }
        }
    }
}
