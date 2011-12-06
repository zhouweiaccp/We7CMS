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
using We7.CMS.Controls;

namespace We7.CMS.Web.Admin.theme.classic
{
    public partial class MenuData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            We7MenuControl menu = new We7MenuControl();
            Response.Write(menu.AllMenuHtml());
            Response.End();
        }
    }
}
