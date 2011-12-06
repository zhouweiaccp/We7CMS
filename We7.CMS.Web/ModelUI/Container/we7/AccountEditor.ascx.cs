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

namespace We7.Model.UI.Container.we7
{
    public partial class SimpleEditor : EditorContainer
    {
        protected string NewUrl
        {
            get
            {
                return We7Helper.AddParamToUrl(Request.RawUrl, We7.Model.Core.UI.Constants.EntityID, We7Helper.CreateNewID());
            }
        }

        protected override void InitContainer()
        {
            rpEditor.DataSource = PanelContext.Panel.EditInfo.Controls;
            rpEditor.DataBind();
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
            HtmlTableCell c = new HtmlTableCell();
            c.InnerHtml = control.Label + "：";
            c.Attributes["class"] = "formTitle";
            row.Cells.Add(c);

            c = new HtmlTableCell();
            c.Attributes["class"] = "formValue";
            FieldControl fc = UIHelper.GetControl(control);
            fc.IsEdit = IsEdit;
            c.Controls.Add(fc);

            Literal ltlMsg = new Literal();
            ltlMsg.Text = control.Desc;
            c.Controls.Add(ltlMsg);

            row.Cells.Add(c);

            row.Style.Add("display", control.Visible ? "" : "none");

            return row;
        }
    }
}