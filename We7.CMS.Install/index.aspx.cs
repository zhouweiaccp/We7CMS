using System;
using We7.CMS.Config;

namespace We7.CMS.Install
{
    public class index : System.Web.UI.Page
    {
        public string preproduct = "We7";
        public string productname = "We7 V2.1";

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (BaseConfigs.ConfigFileExist() && !SetupPage.LockFileExist())
                Response.Redirect("upgrade.aspx", true);

            SetupPage.Init();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
