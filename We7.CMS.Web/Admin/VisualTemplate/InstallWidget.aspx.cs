using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.VisualTemplate
{
    public partial class InstallWidget : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdAction.Value = RequestHelper.Get<string>("action");
                hdUrl.Value = RequestHelper.Get<string>("purl");
            }
        }
    }
}