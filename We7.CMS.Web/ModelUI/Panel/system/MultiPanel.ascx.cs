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
using We7.Framework;
using We7.Model.Core.Data;
using Thinkment.Data;
using System.Collections.Generic;

namespace We7.Model.UI.Panel.system
{
    public partial class MultiPanel : ModelPanel
    {
        enum CURRENTPANEL { EDITOR, LIST, BOTH }
        protected override void InitPanel()
        {
            if (Navigation != null)
            {
                phNavigation.Controls.Clear();
                phNavigation.Controls.Add(Navigation);
                Navigation.OnPreCommand += new ModelEventHandler(ucNavigation_OnPreCommand);
            }
            if (Editor != null)
            {
                phEditor.Controls.Clear();
                phEditor.Controls.Add(Editor);
                Editor.OnPreCommand += new ModelEventHandler(ucEditor_OnPreCommand);
                Editor.OnCommandComplete += new ModelEventHandler(ucEditor_OnCommandComplete);
                Editor.OnSetData += new EventHandler(Editor_OnSetData);
            }
            if (Condition != null)
            {
                phCondition.Controls.Clear();
                phCondition.Controls.Add(Condition);
                Condition.OnPreCommand += new ModelEventHandler(ucCondition_OnPreCommand);
                Condition.OnCommandComplete += new ModelEventHandler(ucCondition_OnCommandComplete);
            }
            if (List != null)
            {
                phList.Controls.Clear();
                phList.Controls.Add(List);
                List.OnCommandComplete += new ModelEventHandler(ucList_OnCommandComplete);
            }
            if (Command != null)
            {
                phCommand.Controls.Clear();
                phCommand.Controls.Add(Command);
            }
            if (Pager != null)
            {
                phPager.Controls.Clear();
                phPager.Controls.Add(Pager);
                Pager.OnCommand += new ModelEventHandler(ucPager_OnCommand);
            }
        }

        void Editor_OnSetData(object sender, EventArgs e)
        {
            CreateNewArticleID();
        }

        protected override void DoPreCommand(object sender, ModelEventArgs args)
        {
            if (args.CommandName == "publish")
            {
                ShowPanel(CURRENTPANEL.EDITOR);
                Editor.SetData(null, null);
                args.Disable = true;
            }
            else if (args.CommandName == "manage")
            {
                ShowPanel(CURRENTPANEL.LIST);
                args.Disable = true;
            }
            else if (args.CommandArguments != null)
            {
                string cmdarg = args.CommandArguments.ToString();
                if (cmdarg.ToLower() == "multirow")
                {
                    args.PanelContext.State = List.GetDataKeys();
                }
            }
        }

        protected override void DoCommandComplete(object sender, ModelEventArgs args)
        {
            if (args.CommandArguments != null)
            {
                string cmdarg = args.CommandArguments.ToString();
                if (cmdarg.ToLower() == "multirow")
                {
                    Condition.Refresh();
                }
            }
        }

        void ucNavigation_OnPreCommand(object sender, ModelEventArgs args)
        {
        }

        void ucPager_OnCommand(object sender, ModelEventArgs args)
        {
            args.Disable = true;
            Condition.SetPageIndex((int)args.CommandArguments);
            Condition.Refresh();
        }

        void ucList_OnCommandComplete(object sender, ModelEventArgs args)
        {
            if (args.CommandName == "get")
            {
                Editor.SetData(args.State as DataRow, args.PanelContext.DataKey.Values);
                ShowPanel(CURRENTPANEL.EDITOR);
            }
            else
            {
                Condition.Refresh();
            }
        }

        void ucCondition_OnCommandComplete(object sender, ModelEventArgs args)
        {
            ListResult result = args.State as ListResult;
            List.BindData(result);
            Condition.SetPageIndex(result.PageIndex);
            Pager.RecordCount = result.RecoredCount;
            Pager.CurrentPageIndex = result.PageIndex;
        }

        void ucCondition_OnPreCommand(object sender, ModelEventArgs args)
        {
        }

        void ucEditor_OnPreCommand(object sender, ModelEventArgs args)
        {
            if (args.CommandName == "reset")
            {
                CreateNewArticleID();
            }
        }

        void ucEditor_OnCommandComplete(object sender, ModelEventArgs args)
        {
            if (!Page.ClientScript.IsStartupScriptRegistered(this.GetType(), "success"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "suceess", "alert('" + (args.CommandName == "add" ? "添加成功!" : "编辑成功") + "')",true);
            }            
            Condition.Refresh();

            Editor.SetData(QueryRow(args.PanelContext), args.PanelContext.DataKey.Values);
            Editor.ChangeState(true);
        }
        void ShowPanel(CURRENTPANEL showmodel)
        {
            plEditor.Visible = showmodel == CURRENTPANEL.EDITOR || showmodel == CURRENTPANEL.BOTH;
            plList.Visible = showmodel == CURRENTPANEL.LIST || showmodel == CURRENTPANEL.BOTH;
        }

        void CreateNewArticleID()
        {
            if (Editor != null)
            {
                Session["Model$CurrentArticleID"] = Editor.IsEdit ? Request[Constants.EntityID] : We7Helper.CreateNewID();
            }
        }

        DataRow QueryRow(PanelContext ctx)
        {
            ModelDBHelper DbHelper = ModelDBHelper.Create(ctx.ModelName);
            Criteria c = new Criteria(CriteriaType.None);
            foreach (string key in ctx.DataKey.Values.Keys)
            {
                c.Add(CriteriaType.Equals, key, ctx.DataKey.Values[key]);
            }
            DataTable dt = DbHelper.Query(c, new List<Order>() { new Order("ID") }, 0, 0);
            return dt != null && dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }
}