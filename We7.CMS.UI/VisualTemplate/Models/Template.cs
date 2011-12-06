using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;

namespace We7.CMS.Module.VisualTemplate.Models
{
    /// <summary>
    /// 模板集合
    /// </summary>
    [Serializable]
    [XmlRoot("TemplateList")]
    public class TemplateList
    {
        public TemplateList()
        {
            Templates = new List<Template>();
        }
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        /// <summary>
        /// 初始路径
        /// </summary>
        [XmlAttribute(AttributeName = "floder")]
        public string Floder { get; set; }
        /// <summary>
        /// 默认图片路径
        /// </summary>
        [XmlAttribute(AttributeName = "baseicopath")]
        public string BaseIcoPath { get; set; }
        /// <summary>
        /// 模板模板名称
        /// </summary>
        [XmlAttribute(AttributeName = "default")]
        public string DefaultTemplateName { get; set; }

        /// <summary>
        /// 模板集合
        /// </summary>
        [XmlArrayItem("Template")]
        public List<Template> Templates
        {
            get;
            set;
        }
        /// <summary>
        /// 默认模板
        /// </summary>
        [XmlIgnore]
        public Template DefaultTemplate
        {
            get
            {
                if (string.IsNullOrEmpty(DefaultTemplateName) && Templates.Count > 0)
                {
                    return Templates[0];
                }
                else
                {
                    foreach (var item in Templates)
                    {
                        if (item.Name == DefaultTemplateName)
                        {
                            return item;
                        }
                    }
                }
                return null;
            }
        }
    }
    /// <summary>
    /// 模板
    /// </summary>
    [Serializable]
    public class Template
    {

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [XmlAttribute(AttributeName = "icon")]
        public string Icon { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        [XmlAttribute(AttributeName = "file")]
        public string File { get; set; }

        /// <summary>
        /// 文件夹
        /// </summary>
        [XmlIgnore]
        public string Floder
        {
            get;
            set;
        }
        /// <summary>
        /// 文件完全路径
        /// </summary>
        [XmlIgnore]
        public string FullFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(Floder))
                {
                    return File;
                }
                else
                {
                    return Path.Combine(Floder, File);
                }
            }
        }
        /// <summary>
        /// 图片完全路径
        /// </summary>
        [XmlIgnore]
        public string FullIconPath
        {
            get { if (string.IsNullOrEmpty(Floder)) { return Icon; } else { return Path.Combine(Floder, Icon); } }
        }
        /// <summary>
        /// 是否继承:flase表示继承
        /// </summary>
        [XmlAttribute(AttributeName = "inherit")]
        public bool NotInherit
        {
            get;
            set;
        }

    }
}
