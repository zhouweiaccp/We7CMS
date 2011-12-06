using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace We7.CMS.WebControls.Core
{
    /// <summary>
    /// 控件组描述信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, Inherited = true)]
    #region class ControlGroupDescriptionAttribute : Attribute
    public class ControlGroupDescriptionAttribute : Attribute
    {
        public ControlGroupDescriptionAttribute()
        {
        }


        public ControlGroupDescriptionAttribute(string label)
        {
            Label = label;
        }

        public ControlGroupDescriptionAttribute(string label, string desc)
            : this(label)
        {
            Description = desc;
        }

        public ControlGroupDescriptionAttribute(string label, string desc, string icon)
            : this(label, desc)
        {
            Icon = icon;
        }

        public ControlGroupDescriptionAttribute(string label, string desc, string icon, string defaulttype)
            : this(label, desc, icon)
        {
            DefaultType = defaulttype;
        }

        /// <summary>
        /// 标签 
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// ICON
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 默认控件
        /// </summary>
        public string DefaultType { get; set; }
    }
    #endregion

    /// <summary>
    /// 控件描述信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, Inherited = true)]
    #region class ControlDescriptionAttribute : Attribute
    public class ControlDescriptionAttribute : Attribute
    {
        public ControlDescriptionAttribute()
        {
        }

        public ControlDescriptionAttribute(string desc)
        {
            Desc = desc;
        }

        public ControlDescriptionAttribute(string desc, string name)
            : this(desc)
        {
            Name = name;
        }

        public ControlDescriptionAttribute(string desc, string name, string author)
            : this(desc, name)
        {
            Author = author;
        }

        public ControlDescriptionAttribute(string desc, string name, string author, string version)
            : this(desc, name, author)
        {
            Version = version;
        }

        /// <summary>
        /// 描述 
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string Created { get; set; }
    }
    #endregion

    /// <summary>
    /// 控件字段属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, Inherited = true)]
    #region class ParameterAttribute : Attribute
    public class ParameterAttribute : Attribute
    {
        #region 构造
        public ParameterAttribute()
        {
            SupportCopy = true;
        }

        public ParameterAttribute(string name)
        {
            Name = name;
            SupportCopy = true;
        }

        public ParameterAttribute(string name, string title)
            : this(name)
        {
            Title = title;
        }

        public ParameterAttribute(string name, string title, string type)
            : this(name, title)
        {
            Type = type;
        }

        public ParameterAttribute(string name, string title, string type, string data) :
            this(name, title, type)
        {
            Data = data;
        }

        public ParameterAttribute(string name, string title, string type, string data, string description) :
            this(name, title, type, data)
        {
            Description = description;
        }

        public ParameterAttribute(string name, string title, string type, string data, string description, int weight) :
            this(name, title, type, data, description)
        {
            Weight = weight;
        }

        public ParameterAttribute(string name, string title, string type, string data, string description, int weight, bool required) :
            this(name, title, type, data, description, weight)
        {
            Required = required;
        }

        public ParameterAttribute(string name, string title, string type, string data, string description, int weight, bool required, bool ignore) :
            this(name, title, type, data, description, weight, required)
        {
            Ignore = ignore;
        }

        public ParameterAttribute(string name, string title, string type, string data, string description, int weight, bool required, bool ignore, int length) :
            this(name, title, type, data, description, weight, required, ignore)
        {
            Length = length;
        }

        public ParameterAttribute(string name, string title, string type, string data, string description, int weight, bool required, bool ignore, int length, int maxnum) :
            this(name, title, type, data, description, weight, required, ignore, length)
        {
            Maxnum = maxnum;
        }

        public ParameterAttribute(string name, string title, string type, string data, string description, int weight, bool required, bool ignore, int length, int maxnum, int minnum) :
            this(name, title, type, data, description, weight, required, ignore, length, maxnum)
        {
            Minnum = minnum;
        }

        public ParameterAttribute(string name, string title, string type, string data, string description, int weight, bool required, bool ignore, int length, int maxnum, int minnum, string defaultValue) :
            this(name, title, type, data, description, weight, required, ignore, length, maxnum, minnum)
        {
            DefaultValue = defaultValue;
        }

        public ParameterAttribute(string name, string title, string type, string data, string description, int weight, bool required, bool ignore, int length, int maxnum, int minnum, string defaultValue, bool supportCopy) :
            this(name, title, type, data, description, weight, required, ignore, length, maxnum, minnum, defaultValue)
        {
            SupportCopy = supportCopy;
        }
        #endregion

        #region 字段

        /// <summary>
        /// 属性名字(应用在哪个属性上)
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// 权重（排序用）
        /// </summary>
        [JsonProperty("weight")]
        public int Weight { get; set; }

        /// <summary>
        /// 是否必选项
        /// </summary>
        [JsonProperty("required")]
        public bool Required { get; set; }

        /// <summary>
        /// 是否可忽略
        /// </summary>
        [JsonProperty("ignore")]
        public bool Ignore { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        [JsonProperty("length")]
        public int Length { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        [JsonProperty("maximum")]
        public int Maxnum { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        [JsonProperty("minium")]
        public int Minnum { get; set; }

        /// <summary>
        /// 默认值 
        /// </summary>
        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 支持样式复制
        /// </summary>
        /// 
        [JsonProperty("supportCopy")]
        public bool SupportCopy { get; set; }

        #endregion
    }
    #endregion

    /// <summary>
    /// 控件的字段内部属性/字段是否需要反射
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, Inherited = true)]
    #region class ChildrenAttribute : Attribute
    public class ChildrenAttribute : Attribute
    {

    }
    #endregion

    /// <summary>
    /// 移除所有ParameterAttribute属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, Inherited = true)]
    public class ClearParameterAttribute : Attribute
    {
    }

    /// <summary>
    /// 移除指定的ParameterAttribute属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, Inherited = true)]
    public class RemoveParameterAttribute : Attribute
    {
        /// <summary>
        /// 移除指定的ParameterAttribute属性
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        public RemoveParameterAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }
    }
}
