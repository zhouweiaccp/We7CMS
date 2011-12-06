using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace We7.CMS.Module.VisualTemplate.Models
{
    /// <summary>
    /// 主题分类
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName="Root",IsNullable=false)]
    public class Themes
    {
        public Themes()
        {
            item = new List<ThemesType>();
        }
        /// <summary>
        /// 主题集合
        /// </summary>
        [XmlElement(ElementName="item")]
        public List<ThemesType> item { get; set; }
    }
    [Serializable]
    public class ThemesType
    {
        /// <summary>
        /// 主题名称
        /// </summary>
        [XmlAttribute("name")]
        public string name { get; set; }
        /// <summary>
        /// 主题图片路径
        /// </summary>
        [XmlAttribute("img")]
        public string img { get; set; }
        /// <summary>
        /// 主题显示名称
        /// </summary>
        [XmlAttribute("label")]
        public string label { get; set; }
    }
}
