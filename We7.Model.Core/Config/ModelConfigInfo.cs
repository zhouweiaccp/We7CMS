using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Config;
using System.Xml.Serialization;
using We7.Model.Core.Entity;
using We7.Framework;

namespace We7.Model.Core.Config
{
    /// <summary>
    /// 内容模型基本配置信息
    /// </summary>
    public class ModelConfigInfo : IConfigInfo
    {
        /// <summary>
        /// 内容模型根目录
        /// </summary>
        public string BaseDirectory { get; set; }

        /// <summary>
        /// 配置文件根目录
        /// </summary>
        public string ConfigsDirecotry { get; set; }

        /// <summary>
        /// 控件根目录
        /// </summary>
        public string ControlsDirectory { get; set; }

        /// <summary>
        /// 容器根目录
        /// </summary>
        public string ContainerDirectory { get; set; }

        /// <summary>
        /// 图片上传路径
        /// </summary>
        public string ImageFolder { get; set; }

        private bool isCreateArticleUC = true;
        /// <summary>
        /// 是否生成文章前台控件
        /// </summary>
        public bool IsCreateArticleUC
        {
            get { return isCreateArticleUC; }
            set { isCreateArticleUC = value; }
        }

        /// <summary>
        /// 数据库帮助类
        /// </summary>
        public string DataBaseHelper { get; set; }

        /// <summary>
        /// 模型类型
        /// </summary>
        [XmlArray("Providers")]
        [XmlArrayItem("Item")]
        public DbProviderInfoCollection Providers { get; set; }

        /// <summary>
        /// 模块存放的路径
        /// </summary>
        public string ModelsDirectory { get; set; }

        /// <summary>
        /// 模型配置的路径
        /// </summary>
        public string BaseModelDirectory { get; set; }

        /// <summary>
        /// 默认模型配置文件
        /// </summary>
        public string DefaultModelFile { get; set; }

        /// <summary>
        /// 模型索引文件
        /// </summary>
        public string ModelIndexFile { get; set; }

        /// <summary>
        /// 模型组索引文件
        /// </summary>
        public string ModelGroupIndexFile { get; set; }

        /// <summary>
        /// 模型控件索引文件
        /// </summary>
        public string ModelControlsIndex { get; set; }

        /// <summary>
        /// 数据库映射路径
        /// </summary>
        public string CDPath { get; set; }

        /// <summary>
        /// 模型前台控件路径
        /// </summary>
        public string ModelUCConfigTemplate { get; set; }

        /// <summary>
        /// 命令类型列表
        /// </summary>
        [XmlArray("Commands")]
        [XmlArrayItem("Item")]
        public NameTypeCollection Commands { get; set; }

        /// <summary>
        /// 转化器类型列表
        /// </summary>
        [XmlArray("Converters")]
        [XmlArrayItem("Item")]
        public NameTypeCollection Converters { get; set; }

        [XmlArray("Defaults")]
        [XmlArrayItem("Item")]
        public NameTypeCollection Defaults { get; set; }

        /// <summary>
        /// 数据列控件集合
        /// </summary>
        [XmlArray("ListControls")]
        [XmlArrayItem("Item")]
        public NameTypeCollection ListControls { get; set; }

        /// <summary>
        /// 数据命令集合
        /// </summary>
        [XmlArray("ListCommands")]
        [XmlArrayItem("Item")]
        public NameValueCollection ListCommands { get; set; }

        /// <summary>
        /// 数据转化集合
        /// </summary>
        [XmlArray("ColumnConvert")]
        [XmlArrayItem("Item")]
        public NameTypeCollection ColumnConvert { get; set; }

        [XmlArray("ViewerControl")]
        [XmlArrayItem("Item")]
        public List<string> ViewerControl { get; set; }
    }
}
