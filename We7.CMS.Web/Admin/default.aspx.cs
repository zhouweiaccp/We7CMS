using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class Administration : BasePage
    {
        protected override bool IsCheckInstallation
        {
            get
            {
                return base.IsCheckInstallation;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string theme = GeneralConfigs.GetConfig().CMSTheme;
            if (theme == null || theme == ""){ theme = "classic";}                                    
            string url = "/admin/" + Constants.ThemePath + "/" + theme + "/main.aspx";
            Response.Redirect(url,false);
        }


    }
}
