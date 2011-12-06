using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core.UI;
using We7.Model.Core;
using We7.CMS.Common.Enum;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS;
using We7.CMS.Common;
using Thinkment.Data;
using We7.Framework.Helper;
using We7.Model.Core.Data;

namespace We7.Model.UI.Container.we7
{
    public partial class Condition : ConditionContainer
    {
        protected override void InitContainer()
        {
            foreach (We7Control control in PanelContext.Panel.ConditionInfo.Controls)
            {
                HtmlTableCell cell = new HtmlTableCell();
                cell.Visible = control.Visible;
                cell.InnerHtml = control.Label + "：";
                cell.Attributes["class"] = "formTitle";
                trQuery.Cells.Insert(trQuery.Cells.Count - 1, cell);

                cell = new HtmlTableCell();
                cell.Visible = control.Visible;
                FieldControl fc = UIHelper.GetControl(control);
                cell.Attributes["class"] = "formValue";
                cell.Controls.Add(fc);

                trQuery.Cells.Insert(trQuery.Cells.Count - 1, cell);
            }
			if (PanelContext.Panel.ConditionInfo.Controls.Count == 0) bttnQuery.Visible = false;
            InitQueryParamHandler(delegate(PanelContext ctx)
            {
                if (State != 99)
                {
                    ctx.QueryFields["State"] = State;
                    ctx.QueryFields.IndexOf("State").Operator = OperationType.EQUER;
                }
            });
        }

        public override void Refresh()
        {
            SubmitQuery(bttnQuery.CommandName, bttnQuery.CommandArgument);
        }

        public int GetCount(ArticleStates state)
        {
            try
            {
                if (state == ArticleStates.All)
                {
                    if (PanelContext.QueryFields.IndexOf("State") != null)
                    {
                        PanelContext.QueryFields.Remove(PanelContext.QueryFields.IndexOf("State"));
                    }
                }
                else
                {
                    PanelContext.QueryFields["State"] = (int)state;
                    PanelContext.QueryFields.IndexOf("State").Operator = OperationType.EQUER;
                }
                return DbProvider.Instance(ModelType.ARTICLE).GetCount(PanelContext);
            }
            catch (Exception ex)
            {
                //处理没有表的情况下异常处理
                //UIHelper.SendError(ex.Message);

                //显示错误
                We7.Framework.LogHelper.WriteLog(typeof(Condition), ex);
                string messageNew = string.Format("{0}<br/>您是否忘记创建表了？", ex.Message);
                errMsg.ShowError(messageNew);
                return 0;
            }
        }


        /// <summary>
        /// 当前过滤条件，文章状态：启用|禁用|审核
        /// </summary>
        protected ArticleStates CurrentState
        {
            get
            {
                ArticleStates s = ArticleStates.All;
                if (Request["state"] != null)
                {
                    if (We7Helper.IsNumber(Request["state"].ToString()))
                        s = (ArticleStates)int.Parse(Request["state"].ToString());
                }
                return s;
            }
        }

        protected void lnkAll_Click(object sender, EventArgs args)
        {
            IButtonControl bttn = sender as IButtonControl;
            State = 99;
            SubmitQuery(bttn.CommandName, bttn.CommandArgument);
            SetStyle(bttn);
        }

        protected void lnkPublish_Click(object sender, EventArgs args)
        {
            IButtonControl bttn = sender as IButtonControl;
            State = 1;
            SubmitQuery(bttn.CommandName, bttn.CommandArgument);
            SetStyle(bttn);
        }

        protected void lnkDraft_Click(object sender, EventArgs args)
        {
            IButtonControl bttn = sender as IButtonControl;
            State = 0;
            SubmitQuery(bttn.CommandName, bttn.CommandArgument);
            SetStyle(bttn);
        }

        protected void lnkAudit_Click(object sender, EventArgs args)
        {
            IButtonControl bttn = sender as IButtonControl;
            State = 2;
            SubmitQuery(bttn.CommandName, bttn.CommandArgument);
            SetStyle(bttn);
        }

        protected void lnkOverdue_Click(object sender, EventArgs args)
        {
            IButtonControl bttn = sender as IButtonControl;
            State = 3;
            SubmitQuery(bttn.CommandName, bttn.CommandArgument);
            SetStyle(bttn);
        }

        protected void bttnQuery_Click(object sender, EventArgs args)
        {
            IButtonControl bttn = sender as IButtonControl;
            SubmitQuery(bttn.CommandName, bttn.CommandArgument);
        }

        protected int State
        {
            get
            {
                if (ViewState["$Condition$State"] == null)
                {
                    ViewState["$Condition$State"] = 99;
                }
                return (int)ViewState["$Condition$State"];
            }
            set { ViewState["$Condition$State"] = value; }
        }

        void SetStyle(object bttn)
        {
            lnkAll.CssClass = "";
            lnkOverdue.CssClass = "";
            lnkPublish.CssClass = "";
            lnkDraft.CssClass = "";
            lnkAudit.CssClass = "";
            if (bttn != null && bttn is WebControl)
            {
                ((WebControl)bttn).CssClass = "current";
            }
        }

        protected global::System.Web.UI.WebControls.LinkButton lnkOverdue, lnkAll, lnkPublish, lnkDraft, lnkAudit;
    }
}