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
using We7.Model.Core;
using We7.Model.Core.UI;
using We7.Framework;

namespace We7.Model.UI.Container.we7
{
    public partial class Viewer : ViewerContainer
    {
        protected override void InitContainer()
        {
            rpEditor.DataSource =PanelContext.Panel.EditInfo.Controls;
            rpEditor.DataBind();
            ModelLabel.Text = PanelContext.Model.Label;
            MenuTabLabel.Text = BuildNavString();
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
            //control.Type = "Text";
            control = control.Clone() as We7Control;
            control.Type = "Text";

            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell c = new HtmlTableCell();
            c.InnerHtml = GetLabel(control)+ "：";
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

        /// <summary>
        /// 构建标签项
        /// </summary>
        /// <returns></returns>
        string BuildNavString()
        {
            string str1 = @"<LI class=TabOut id=tab{0}  style='display:{2}'><A  href={3}>{1}</A> </LI>";
            string str2 = @"<LI class=TabIn id=tab{0} style='display:{2}'><A>{1}</A> </LI>";
            return String.Format(str2, 1, "发布"+PanelContext.Model.Label, "block");
        }

        protected void bttnNew_Click(object sender, EventArgs e)
        {
            string url = "~/admin/addins/ModelEditor.aspx?notiframe={2}&model={0}&ID={1}";
            url = String.Format(url, PanelContext.Model.ModelName, We7Helper.CreateNewID(), Request["notiframe"]);
            Response.Redirect(url);
        }

        protected void bttnEdit_Click(object sender, EventArgs e)
        {
            string url = "~/admin/addins/ModelEditor.aspx?notiframe={2}&model={0}&ID={1}&groupIndex=0";
            url = String.Format(url, PanelContext.Model.ModelName, Request[Constants.EntityID],Request["notiframe"]);
            Response.Redirect(url);
        }
    }
}