using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.UI;
using We7.Model.Core;

namespace We7.CMS.WebControls
{
    public abstract class ModelContainer : ThinkmentDataControl, IModelContainer
    {
        private PanelContext panelContext;

        private PanelContext modelData;
        private UIHelper uiHelper;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            uiHelper = new UIHelper(Page, PanelContext);
            LoadContainer();
        }

        protected virtual void LoadContainer() { }

        internal event ModelEventHandler OnInteralCommand;
        /// <summary>
        /// 执行的命令事件
        /// </summary>
        public event ModelEventHandler OnCommand;

        /// <summary>
        /// 执行命令之间的事件
        /// </summary>
        public event ModelEventHandler OnPreCommand;

        /// <summary>
        /// 执行命令完成之后的事件
        /// </summary>
        public event ModelEventHandler OnCommandComplete;

        /// <summary>
        /// 模型名
        /// </summary>
        public virtual string ModelName { get; set; }

        /// <summary>
        /// 面板配置名
        /// </summary>
        public virtual string PanelName { get; set; }

        /// <summary>
        /// UI处理对象
        /// </summary>
        protected UIHelper UIHelper { get { return uiHelper; } }

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
        }

        /// <summary>
        /// 模型信息
        /// </summary>
        public ModelInfo Info
        {
            get { return PanelContext.Model; }
        }

        /// <summary>
        /// 当前面板信息
        /// </summary>
        public Panel Panel
        {
            get { return PanelContext.Panel; }
        }

        /// <summary>
        /// 命令提交时执行的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        protected virtual void OnCommandSubmit(string cmd, object args)
        {
            DoCommand(this, cmd, PanelContext, args);
        }

        /// <summary>
        /// 初始化模型数据
        /// </summary>
        protected abstract void InitModelData();

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmd">命令名称</param>
        /// <param name="model">模型信息</param>
        /// <param name="state">附加数据</param>
        protected void DoCommand(object sender, string cmd, PanelContext data, object args)
        {
            //初始化模型数据
            InitModelData();

            //构建模型参数
            ModelEventArgs modelArgs = new ModelEventArgs(cmd, args, data);

            //处理预处理命令
            if (OnPreCommand != null)
            {
                OnPreCommand(sender, modelArgs);
            }
            if (!modelArgs.Disable)
            {
                //处理命令
                if (OnCommand != null)
                {
                    OnCommand(sender, modelArgs);
                }
                else
                {
                    OnInteralCommand(sender, modelArgs);
                }
            }
            //处理命令完成后事件
            if (!modelArgs.Disable && OnCommandComplete != null)
            {
                OnCommandComplete(sender, modelArgs);
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmd">命令名称</param>
        /// <param name="state">附加数据</param>
        protected void DoCommand(object sender, string cmd, object args)
        {
            DoCommand(sender, cmd, PanelContext, args);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmd">命令名称</param>
        protected void DoCommand(object sender, string cmd)
        {
            DoCommand(cmd, null);
        }
    }
}
