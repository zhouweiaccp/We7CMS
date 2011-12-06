using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using We7.Model.Core.Config;
using We7.Model.Core.Entity;

namespace We7.Model.Core
{
    [Serializable]
    public class ModelInfo
    {
        private We7DataSet dataSet = new We7DataSet();
        private Layout layout = new Layout();
        private string groupName;
        private string name;

        /// <summary>
        /// 索引，根据面板名取得当前的上下文
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        [XmlIgnore]
        public PanelContext this[string panelName]
        {
            get
            {
                foreach (Panel panel in Layout.Panels)
                {
                    if (panel.Name == panelName)
                    {
                        panel.Context.Panel = panel;
                        panel.Context.Model = this;
                        return panel.Context;
                    }
                }
                throw new Exception("不存在当前面板");
            }

        }

        /// <summary>
        /// 数据集配置
        /// </summary>
        [XmlElement("dataSet")]
        public We7DataSet DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }

        /// <summary>
        /// 布局
        /// </summary>
        [XmlElement("layout")]
        public Layout Layout
        {
            get { return layout; }
            set { layout = value; }
        }

        /// <summary>
        /// 模型名称
        /// </summary>
        [XmlIgnore]
        public string ModelName { get; set; }

        /// <summary>
        /// 模型组名称
        /// </summary>
        [XmlIgnore]
        public string GroupName
        {
            get
            {
                if (String.IsNullOrEmpty(groupName))
                {
                    if (!ModelName.Contains("."))
                    {
                        groupName = "System";
                    }
                    else
                    {
                        groupName = ModelName.Split('.')[0];
                    }
                }
                return groupName;
            }
        }

        /// <summary>
        /// 模型名称
        /// </summary>
        [XmlIgnore]
        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(name))
                {
                    if (!ModelName.Contains("."))
                    {
                        name = ModelName;
                    }
                    else
                    {
                        name = ModelName.Split('.')[1];
                    }
                }
                return name;
            }
        }

        /// <summary>
        /// 数据提供者
        /// </summary>
        [XmlIgnore]
        public DbProviderInfo DbProvider
        {
            get
            {
                return ModelConfig.GetDbProvider(Type);
            }
        }

        /// <summary>
        /// 模型类型
        /// </summary>
        [XmlAttribute("type")]
        public ModelType Type { get; set; }

        /// <summary>
        /// 模型标签
        /// </summary>
        [XmlAttribute("label")]
        public string Label { get; set; }

        /// <summary>
        /// 关于模型的描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }

        /// <summary>
        /// 模型的其他参数
        /// </summary>
        [XmlAttribute("parameters")]
        public string Parameters { get; set; }

        /// <summary>
        /// 文章查询权限:True,则可以查看全部信息,False则只有Admin能查看全部信息 默认为flase
        /// </summary>
        [XmlAttribute("authority")]
        public bool AuthorityType { get; set; }

        /// <summary>
        /// 关联反馈模型:如果为空则不关联
        /// </summary>
        [XmlAttribute("RelationModelName")]
        public string RelationModelName { get; set; }

    }
}
