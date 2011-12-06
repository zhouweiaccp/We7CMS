using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace We7.CMS.Web.Admin
{
    public partial class SystemStrategy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                ucStrtgy.AfterClick += new EventHandler(ucStrtgy_AfterClick);
            }
            else
            {
                ucStrtgy.IPStrategy = Request.QueryString["ipstrategy"];
                ucStrtgy.PreRender += ucStrtgy_AfterClick;
            }
        }

        void ucStrtgy_AfterClick(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Strategy", "parent.document.getElementById('ctl00_MyContentPlaceHolder_hddnIPStrategy').value='" + ucStrtgy.IPStrategy + "'", true);
        }
    }
}
