using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
namespace We7.Model.Core
{
    public enum ModelType
    {
        /// <summary>
        /// 文章
        /// </summary>
        [XmlEnum("article")]
        ARTICLE,
        /// <summary>
        /// 反馈
        /// </summary>
        [XmlEnum("advice")]
        ADVICE,
        /// <summary>
        /// 用户
        /// </summary>
        [XmlEnum("account")]
        ACCOUNT
    }

    [Serializable]
    public class ContentModel
    {
        ///// <summary>
        ///// 排序Id
        ///// </summary>
        //[XmlAttribute("id")]
        //public int Id { get; set; }
        /// <summary>
        /// ModelInfo名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// ModelInfo中文名称
        /// </summary>
        [XmlAttribute("label")]
        public string Label { get; set; }
        ///// <summary>
        ///// ModelInfo文件路径
        ///// </summary>
        //[XmlAttribute("path")]
        //public string Path { get; set; }
        /// <summary>
        /// ModelInfo描述文字
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// 模型文件是否启用
        /// </summary>
        [XmlAttribute("state")]
        public int State { get; set; }

        /// <summary>
        /// 默认模板Id(如果为0则为空白模板)
        /// </summary>
        [XmlAttribute("defaultcontextname")]
        public string DefaultContentName { get; set; }

        /// <summary>
        /// 模型的Identity值.
        /// </summary>
        [XmlAttribute("value")]
        public int Value { get; set; }

        /// <summary>
        ///内容模型类型
        /// </summary>
        [XmlAttribute("type")]
        public ModelType Type { get; set; }
        ///// <summary>
        ///// 数据提供者
        ///// </summary>
        //[XmlAttribute("provider")]
        //public string DbProvider { get; set; }

        public string GetDefaultModel(ModelType type)
        {
            switch (type)
            {
                case ModelType.ARTICLE:
                    return "Template.ArticleModel";
                case ModelType.ADVICE:
                    return "Template.AdviceModel";
                case ModelType.ACCOUNT:
                    return "Template.AccountModel";
            }
            return "Template.ArticleModel";
        }
    }
}
