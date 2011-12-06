using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Util;

namespace We7.Model.Core.UI
{
    public abstract class ConditionContainer : CommandContainer
    {
        protected Action<PanelContext> initQueryParam;

        protected override void LoadContainer()
        {
            InitContainer();
            if (!IsPostBack)
            {
                Refresh();
            }
        }

        /// <summary>
        /// 当前页记录
        /// </summary>
        protected int PageIndex
        {
            get
            {
                if (ViewState["__PageIndex__"] == null)
                {
                    ViewState["__PageIndex__"] = 1;
                }
                return (int)ViewState["__PageIndex__"];
            }
            set
            {
                ViewState["__PageIndex__"] = value;
            }
        }

        /// <summary>
        /// 设置当前的页码
        /// </summary>
        /// <param name="pageIndex"></param>
        public void SetPageIndex(int pageIndex)
        {
            PageIndex = pageIndex;
        }

        protected override void OnCommandSubmit(string cmd, object args)
        {
            base.OnCommandSubmit(cmd, args);
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="cmd">查询命令</param>
        /// <param name="args">查询参数</param>
        /// <param name="action">外挂动作</param>
        protected void SubmitQuery(string cmd,object args)
        {
            OnCommandSubmit(cmd, args);
        }

        protected void InitQueryParamHandler(Action<PanelContext> action)
        {
            if (initQueryParam == null)
                initQueryParam = action;
            else
                initQueryParam += action;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// 初始化容器
        /// </summary>
        protected abstract void InitContainer();

        protected override void InitModelData()
        {
            PanelContext.PageIndex = PageIndex;
            PanelContext.QueryFields.Clear();
            foreach (We7Control field in PanelContext.Panel.ConditionInfo.Controls)
            {                
                FieldControl fc = UIHelper.GetControl(field.ID, this) as FieldControl;
                object val = fc.GetValue();
                if (field.IgnoreEmpty&&String.IsNullOrEmpty(val as string))
                {
                    continue;
                }
                PanelContext.QueryFields[field.Name] = val;
                if (String.Compare(field.Type, "textinput", true) == 0 || String.Compare(field.Type, "textarea", true) == 0)
                    PanelContext.QueryFields.IndexOf(field.Name).Operator = OperationType.LIKE;
                else
                    PanelContext.QueryFields.IndexOf(field.Name).Operator = ModelHelper.GetOperation(field.Params["operater"]);
            }
            if (initQueryParam != null)
            {
                initQueryParam(PanelContext);
            }
        }
    }
}
