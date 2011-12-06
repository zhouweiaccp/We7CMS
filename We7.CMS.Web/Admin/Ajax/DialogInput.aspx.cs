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

namespace We7.CMS.Web.Admin.Ajax
{
    public partial class DialogInput : System.Web.UI.Page
    {
        protected string Value;

        protected void Page_Load(object sender, EventArgs e)
        {
            Value = Request["v"];
            frame.Attributes["src"] = Request["url"]+Request.Url.Query;
        }
    }
}
