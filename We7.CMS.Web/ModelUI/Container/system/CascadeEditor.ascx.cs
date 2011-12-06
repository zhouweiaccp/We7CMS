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
using System.Collections.Generic;

namespace CModel.Container.system
{
    public partial class CascadeEditor:EditorContainer
    {
        protected override void InitContainer()
        {
            ChangeState();  
            rpEditor.DataSource = PanelContext.Panel.EditInfo.Controls;
            rpEditor.DataBind();
        }

        protected override void ChangeState()
        {
            bttnEdit.Visible = IsEdit;
            bttnSave.Visible = !IsEdit;
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
            FieldControl fc = UIHelper.GetControl(control);
            fc.IsEdit = IsEdit;
            c.Controls.Add(fc);
            row.Cells.Add(c);

            row.Style.Add("display", control.Visible ? "" : "none");

            return row;
        }

        protected void bttnReset_Click(object sender, EventArgs e)
        {
            SetData(null, null);
        }
    }
}