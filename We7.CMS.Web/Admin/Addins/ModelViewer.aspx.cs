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

namespace We7.CMS.Web.Admin.Addins
{
    public partial class ModelViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string model = Request["model"];
            ucEditor.ModelName = model;
            ucEditor.PanelName = "edit";
            ucEditor.IsViewer = true;
            ModelInfo info = ModelHelper.GetModelInfo(model);
            PagePathLiteral.Text = info.Label + "管理>发布" + info.Label;
            NameLabel.Text = info.Label + "管理";
        }
    }
}
