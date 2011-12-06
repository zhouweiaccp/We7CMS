using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using We7.Framework;
using We7.Framework.Util;
using System.Web;

namespace We7.Model.Core.UI
{
    public class ModelPanel : UserControl
    {
        protected sealed override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UIHelper = new UIHelper(Page);
            InitPanel();
            InitContainer(this);
        }

        protected override void OnPreRender(EventArgs e)
        {
        }
        private string modelName;
        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName
        {
            get { return modelName; }
            set { modelName = ModelHelper.GetModelName(value); }
        }

        /// <summary>
        /// 当前面板配置名称
        /// </summary>
        public string PanelName { get; set; }

        /// <summary>
        /// 模型信息
        /// </summary>
        public ModelInfo Model
        {
            get { return PanelContext.Model; }
        }

        private PanelContext panelContext = null;
        public PanelContext PanelContext
        {
            get
            {
                if (panelContext == null)
                {
                    panelContext = ModelHelper.GetPanelContext(ModelName, PanelName);
                }
                return panelContext;
            }
            set
            {
                panelContext = value;
            }
        }

        protected UIHelper UIHelper { get; set; }

        /// <summary>
        /// 初始化模型容器
        /// </summary>
        protected void InitContainer(Control container)
        {
            if (container != null)
            {
                if (container is IModelContainer)
                {
                    ModelContainer mc = container as ModelContainer;
                    mc.ModelName = ModelName;
                    mc.PanelName = PanelName;
                    mc.OnInteralCommand += new ModelEventHandler(DoCommand);
                    mc.OnPreCommand += new ModelEventHandler(DoPreCommand);
                    mc.OnCommandComplete += new ModelEventHandler(DoCommandComplete);
                }
                foreach (Control c in container.Controls)
                {
                    InitContainer(c);
                }
            }
        }

        /// <summary>
        /// 默认命令完成事件
        /// </summary>
        /// <param name="sender">触发者</param>
        /// <param name="args">事件参数</param>
        protected virtual void DoCommandComplete(object sender, ModelEventArgs args)
        {
        }

        /// <summary>
        /// 初始化面板
        /// </summary>
        protected virtual void InitPanel()
        {
        }

        /// <summary>
        /// 默认命令处理流程
        /// </summary>
        /// <param name="sender">触发者</param>
        /// <param name="args">事件参数</param>
        protected virtual void DoCommand(object sender, ModelEventArgs args)
        {
            if (!args.Disable)
            {
                try
                {
                    ICommand cmd = CommandFactory.GetCommand(args.CommandName);
                    if (cmd == null)
                    {
                        throw new SysException(cmd + "模型命令不存在");
                    }
                    args.State = cmd.Do(args.PanelContext);
                }
                catch (Exception ex)
                {
                    UIHelper.SendError(ex.Message);
                    args.Disable = true;
                }
            }
        }

        /// <summary>
        /// 默认预处理命令
        /// </summary>
        /// <param name="sender">触发者</param>
        /// <param name="args">事件参数</param>
        protected virtual void DoPreCommand(object sender, ModelEventArgs args)
        {
        }

        #region 预设的一些容器

        private EditorContainer editor;
        /// <summary>
        /// 编辑器容器
        /// </summary>
        public virtual EditorContainer Editor
        {
            get
            {
                if (editor == null)
                {
                    if (PanelContext.Panel.EditInfo != null)
                    {
                        string path = PanelContext.Panel.EditInfo.Path;
                        if (!String.IsNullOrEmpty(path))
                        {
                            editor = UIHelper.GetContainer(path) as EditorContainer;

                            if (editor != null)
                            {
                                editor.Visible = PanelContext.Panel.EditInfo.Visible;
                            }
                        }
                    }
                }
                return editor;
            }
        }

        private ViewerContainer viewer;
        /// <summary>
        /// 编辑器容器
        /// </summary>
        public virtual ViewerContainer Viewer
        {
            get
            {
                if (viewer == null)
                {
                    if (PanelContext.Panel.EditInfo != null)
                    {
                        string path = PanelContext.Panel.EditInfo.ViewerPath;
                        if (String.IsNullOrEmpty(path))
                            path = "Viewer";                           
                        viewer = UIHelper.GetContainer(path) as ViewerContainer;
                    }
                }
                return viewer;
            }
        }

        private ListContainer list;
        /// <summary>
        /// 列表容器
        /// </summary>
        public virtual ListContainer List
        {
            get
            {
                if (list == null)
                {
                    if (PanelContext.Panel.ListInfo != null)
                    {
                        string path = PanelContext.Panel.ListInfo.Path;
                        if (!String.IsNullOrEmpty(path))
                        {
                            list = UIHelper.GetContainer(path) as ListContainer;
                            if (list != null)
                            {
                                list.Visible = PanelContext.Panel.ListInfo.Visible;
                            }
                        }
                    }
                }
                return list;
            }
        }

        private ConditionContainer condition;
        /// <summary>
        /// 条件容器
        /// </summary>
        public virtual ConditionContainer Condition
        {
            get
            {
                if (condition == null)
                {
                    if (PanelContext.Panel.ConditionInfo != null)
                    {
                        string path = PanelContext.Panel.ConditionInfo.Path;
                        if (!String.IsNullOrEmpty(path))
                        {
                            condition = UIHelper.GetContainer(path) as ConditionContainer;
                            if (condition != null)
                            {
                                condition.Visible = PanelContext.Panel.ConditionInfo.Visible;
                            }
                        }
                    }
                }
                return condition;
            }
        }

        private CommandContainer navigation;
        /// <summary>
        /// 条件容器
        /// </summary>
        public virtual CommandContainer Navigation
        {
            get
            {
                if (navigation == null)
                {
                    if (PanelContext.Panel.NavigationInfo != null)
                    {
                        string path = PanelContext.Panel.NavigationInfo.Path;
                        if (!String.IsNullOrEmpty(path))
                        {
                            navigation = UIHelper.GetContainer(path) as CommandContainer;
                            if (navigation != null)
                            {
                                navigation.Visible = PanelContext.Panel.NavigationInfo.Visible;
                            }
                        }
                    }
                }
                return navigation;
            }
        }

        private PagerContainer pager;
        /// <summary>
        /// 条件容器
        /// </summary>
        public virtual PagerContainer Pager
        {
            get
            {
                if (pager == null)
                {
                    if (PanelContext.Panel.PagerInfo != null)
                    {
                        string path = PanelContext.Panel.PagerInfo.Path;
                        if (!String.IsNullOrEmpty(path))
                        {
                            pager = UIHelper.GetContainer(path) as PagerContainer;
                            if (pager != null)
                            {
                                pager.Visible = PanelContext.Panel.PagerInfo.Visible;
                            }
                        }
                    }
                }
                return pager;
            }
        }

        private CommandContainer command;
        /// <summary>
        /// 条件容器
        /// </summary>
        public virtual CommandContainer Command
        {
            get
            {
                if (command == null)
                {
                    if (PanelContext.Panel.ConditionInfo != null)
                    {
                        string path = PanelContext.Panel.CommandInfo.Path;
                        if (!String.IsNullOrEmpty(path))
                        {
                            command = UIHelper.GetContainer(path) as CommandContainer;
                            if (command != null)
                            {
                                command.Visible = PanelContext.Panel.CommandInfo.Visible;
                            }
                        }
                    }
                }
                return command;
            }
        }

        #endregion
    }
}
