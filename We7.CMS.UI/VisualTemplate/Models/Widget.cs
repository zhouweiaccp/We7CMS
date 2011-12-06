using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace We7.CMS.Module.VisualTemplate.Models
{
    /// <summary>
    /// Widget分类
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "widgets", IsNullable = false)]
    public class WidgetCollection
    {
        public WidgetCollection()
        {
            Groups = new List<WidgetGroup>();
        }
        /// <summary>
        /// 英文名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        [XmlAttribute("label")]
        public string Label
        {
            get;
            set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 部件集合的集合
        /// </summary>
        [XmlElement(ElementName = "group")]
        public List<WidgetGroup> Groups
        {
            get;
            set;
        }
    }
    /// <summary>
    /// Widget分组
    /// </summary>
    [Serializable]
    public class WidgetGroup
    {
        public WidgetGroup()
        {
            Widgets = new List<Widget>();
        }
        /// <summary>
        /// 英文名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        [XmlAttribute("label")]
        public string Label
        {
            get;
            set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 部件集合
        /// </summary>
        [XmlElement(ElementName = "widget")]
        public List<Widget> Widgets
        {
            get;
            set;
        }
    }

    /// <summary>
    /// widget
    /// </summary>
    [Serializable]
    public class Widget
    {
        private string defaultType;

        public Widget()
        {
            Types = new List<WidgetType>();
        }
        /// <summary>
        /// 英文名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        [XmlAttribute("label")]
        public string Label
        {
            get;
            set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 图标
        /// </summary>
        [XmlAttribute("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [XmlAttribute("file")]
        public string File { get; set; }

        /// <summary>
        /// 默认样式
        /// </summary>
        [XmlAttribute("defaulttype")]
        public string DefaultType
        {
            get
            {
                if (String.IsNullOrEmpty(defaultType) && Types != null && Types.Count > 0)
                {
                    defaultType = Types[0].Name;
                }
                return defaultType;
            }
            set { defaultType = value; }
        }

        /// <summary>
        /// 样式集合
        /// </summary>
        [XmlElement(ElementName = "type")]
        public List<WidgetType> Types { get; set; }
    }
    /// <summary>
    /// Widget样式
    /// </summary>
    [Serializable]
    public class WidgetType
    {
        /// <summary>
        /// 英文名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        [XmlAttribute("label")]
        public string Label
        {
            get;
            set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        [XmlAttribute("file")]
        public string File { get; set; }

        /// <summary>
        /// 控件类型
        /// </summary>
        [XmlAttribute("control")]
        public string Control { get; set; }
    }

}
