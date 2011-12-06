using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Controls;

namespace We7.CMS.Web.Admin
{
    public partial class About : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //CDMenuControl Menu1 = (CDMenuControl)Master.FindControl("MainTab");
            //Menu1.ActiveID = "{99999}";
            //Menu1.ParentID = "{00000000-0000-0000-0000-000000000000}";
            //CDMenuControl Menu2 = (CDMenuControl)Master.FindControl("MainMenu");
            //Menu2.ActiveID = "{88888}";
            //Menu2.ParentID = "{99999}";
            //this.Master.SiteHeadTitle = SiteHeadTitle;
            //this.Master.TitleName = "¹ØÓÚ...";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblVersion.Text = CDHelper.Config.ProductVersion;
            }
        }
    }
}
