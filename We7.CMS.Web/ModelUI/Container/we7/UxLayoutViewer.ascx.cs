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
    public partial class UxLayoutViewer : ViewerContainer
    {
        protected string NewUrl
        {
            get
            {
                return We7Helper.AddParamToUrl(Request.RawUrl, We7.Model.Core.UI.Constants.EntityID, We7Helper.CreateNewID());
            }
        }

        LayoutEditor editor;

        protected override void InitContainer()
        {
            if (editor == null)
            {
                editor = UIHelper.LoadLayoutEditor(PanelContext.Panel.EditInfo.ViewerLayout);
                editor.ID = "UxLayoutCtr";
                editor.IsViewer = true;
                UxLayout.Controls.Clear();
                UxLayout.Controls.Add(editor);
            }
            editor.InitLayout(PanelContext);
            ModelLabel.Text = PanelContext.Model.Label;
            MenuTabLabel.Text = BuildNavString();
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
            Response.Redirect(String.Format("~/admin/addins/ModelEditor.aspx?notiframe={2}&model={0}&ID={1}", PanelContext.Model.ModelName, We7Helper.CreateNewID(), Request["notiframe"]));
        }

        protected void bttnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/admin/addins/ModelEditor.aspx?notiframe={2}&model={0}&ID={1}&groupIndex=0", PanelContext.Model.ModelName, Request[Constants.EntityID], Request["notiframe"]));
        }
    }
}