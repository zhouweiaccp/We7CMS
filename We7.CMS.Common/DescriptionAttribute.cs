using System;

namespace We7.CMS.Common
{
    /// <summary>
    /// 用于描述的属性类
    /// </summary>
    public class DescriptionAttribute : Attribute
    {
        private string name;
        private string message;

        public DescriptionAttribute(string name, string message)
        {
            this.name = name;
            this.message = message;
        }

        public DescriptionAttribute(string name)
            : this(name, "")
        { }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
    }
}
