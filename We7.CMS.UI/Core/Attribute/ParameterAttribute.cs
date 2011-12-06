using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.WebControls.Core
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class OptionAttribute : Attribute
    {
        public OptionAttribute() { }

        public OptionAttribute(string type)
        {
            Type = type;
        }

        public OptionAttribute(string type, string data)
            : this(type)
        {
            Data = data;
        }

        public string Type { get; set; }

        public string Data { get; set; }

        public bool Required { get; set; }

        public int Maxnum { get; set; }

        public int Minnum { get; set; }

        public int Length { get; set; }

        public string DefaultValue { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true,AllowMultiple=false)]
    public class DefaultAttribute : Attribute
    {
        public DefaultAttribute(object value)
        {
            Value = value;
        }

        public object Value { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class IgnoreAttribute : Attribute
    {
        public string[] Fields { get; set; }

        public IgnoreAttribute(params string[] fields)
        {
            Fields = fields;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class RiseAttribute : Attribute
    {
        public string[] Fields { get; set; }

        public RiseAttribute(params string[] fields)
        {
            Fields = fields;
        }
    }

    /// <summary>
    /// 描述信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DescAttribute : Attribute
    {
        public DescAttribute()
        {
        }

        public DescAttribute(string title)
        {
            Title = title;
        }

        public DescAttribute(string title, string desc)
            : this(title)
        {
            Description = desc;
        }

        public string Description { get; set; }

        public string Title { get; set; }
    }

    /// <summary>
    /// 必选项
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class RequiredAttribute : Attribute
    {

    }

    //[AttributeUsage(AttributeTargets.Property, Inherited = true)]
    //public class ChildrenAttribute : Attribute
    //{
    //}

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class WeightAttribute : Attribute
    {
        public WeightAttribute(int weight)
        {
            Weight = weight;
        }

        public int Weight { get; set; }
    }
}