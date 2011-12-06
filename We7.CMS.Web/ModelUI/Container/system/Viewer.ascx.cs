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
using We7.Model.Core.Config;
using System.Collections.Generic;

namespace We7.Model.UI.Container.system
{
    public partial class Viewer : ViewerContainer//EditorContainer
    {
        protected override void InitContainer()
        {
            rpEditor.DataSource = PanelContext.Panel.EditInfo.Controls;
            rpEditor.DataBind();
            trBttn.Visible = PanelContext.Model.Type == ModelType.ARTICLE;
        }

        protected void rpEditor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
            {
                We7Control ctr = e.Item.DataItem as We7Control;
                e.Item.Controls.Add(CreateItem(ctr));
            }
        }
        /// <summary>
        /// 创建行控件
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <returns>行控件</returns>
        protected Control CreateItem(We7Control control)
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell c = new HtmlTableCell("TH");
            HtmlGenericControl lable = new HtmlGenericControl("strong");
            lable.InnerHtml = control.Label + "：";
            c.Controls.Add(lable);
            row.Cells.Add(c);

            c = new HtmlTableCell();
            We7Control ctr = control.Clone() as We7Control;
            if (!EnableControls.Contains(ctr.Type))
            {
                ctr.Type = "Text";
            }
            FieldControl fc = UIHelper.GetControl(ctr);
            fc.IsEdit = IsEdit;
            c.Controls.Add(fc);
            row.Cells.Add(c);

            row.Style.Add("display", control.Visible ? "" : "none");

            return row;
        }

        protected void bttnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/admin/addins/ModelEditor.aspx?notiframe={2}&model={0}&ID={1}", PanelContext.Model.ModelName, We7Helper.CreateNewID(),Request["notiframe"]));
        }

        protected void bttnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/admin/addins/ModelEditor.aspx?notiframe={2}&model={0}&ID={1}&groupIndex=0", PanelContext.Model.ModelName, Request[Constants.EntityID],Request["notiframe"]));
        }

        private List<string> enableControls;
        protected List<string> EnableControls
        {
            get
            {
                if(enableControls==null)
                {
                    enableControls= ModelConfig.GetConfig().ViewerControl??new List<string>();
                }
                return enableControls;
            }
        }
    }
}