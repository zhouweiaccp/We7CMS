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
using System.Drawing;

namespace We7.CMS.Web.Admin
{
    public partial class FontList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbSysFont.Items.Clear();
                foreach (FontFamily f in FontFamily.Families)
                    lbSysFont.Items.Add(f.Name);
            }
        }
    }
}
