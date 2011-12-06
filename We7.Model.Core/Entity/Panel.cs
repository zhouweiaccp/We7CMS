using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace We7.Model.Core
{
    [Serializable]
    public class PanelCollection : Collection<Panel>
    {
        /// <summary>
        /// 索引，按面板名称获取指定面板
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Panel this[string name]
        {
            get
            {
                foreach (Panel p in this)
                {
                    if (p.Name == name)
                        return p;
                }
                return null;
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Name==name)
                    {
                        this[i] = value;
                    }
                }
            }
        }

        public void AddOrUpdate(Panel panel)
        {
            if (this[panel.Name]!=null)
            {
                this[panel.Name] = panel;
            }
            else
            {
                this.Add(panel);
            }
        }
    }

    /// <summary>
    /// 面板
    /// </summary>
    [Serializable]
    public class Panel
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }


        /// <summary>
        /// 显示名称
        /// </summary>
        [XmlAttribute("label")]
        public string Label { get; set; }

        private EditInfo editInfo = new EditInfo();
        /// <summary>
        /// 编辑容器配置信息
        /// </summary>
        [XmlElement("edit")]
        public EditInfo EditInfo
        {
            get { return editInfo; }
            set { editInfo = value; }
        }

        private ListInfo listInfo = new ListInfo();
        /// <summary>
        /// 列表容器配置信息
        /// </summary>
        [XmlElement("list")]
        public ListInfo ListInfo
        {
            get { return listInfo; }
            set { listInfo = value; }
        }

        private PagerInfo pagerInfo = new PagerInfo();
        /// <summary>
        /// 编辑容器配置信息
        /// </summary>
        [XmlElement("pager")]
        public PagerInfo PagerInfo
        {
            get { return pagerInfo; }
            set { pagerInfo = value; }
        }

        private ConditionInfo conditionInfo = new ConditionInfo();
        /// <summary>
        /// 条件容器配置信息
        /// </summary>
        [XmlElement("condition")]
        public ConditionInfo ConditionInfo
        {
            get { return conditionInfo; }
            set { conditionInfo = value; }
        }

        private CommandInfo commandInfo = new CommandInfo();
        /// <summary>
        /// 命令容器配置信息
        /// </summary>
        [XmlElement("command")]
        public CommandInfo CommandInfo
        {
            get { return commandInfo; }
            set { commandInfo = value; }
        }

        private NavigationInfo navigationInfo = new NavigationInfo();
        /// <summary>
        /// 导航容器配置信息
        /// </summary>
        [XmlElement("navigation")]
        public NavigationInfo NavigationInfo
        {
            get { return navigationInfo; }
            set { navigationInfo = value; }
        }

        private PanelContext context = new PanelContext();
        /// <summary>
        /// 当前面板上下文
        /// </summary>
        [XmlElement("context")]
        public PanelContext Context
        {
            get { return context; }
            set { context = value; }
        }
    }
}
