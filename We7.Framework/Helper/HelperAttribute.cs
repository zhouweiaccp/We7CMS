using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework
{
    /// <summary>
    /// 助手属性：用于描述dll加载获取的实体对象
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class HelperAttribute : Attribute
    {
        string helperName;
        string description;

        public HelperAttribute(string name)
        {
            helperName = name;
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        public string Name
        {
            get { return helperName; }
            set { helperName = value; }
        }
    }
}
