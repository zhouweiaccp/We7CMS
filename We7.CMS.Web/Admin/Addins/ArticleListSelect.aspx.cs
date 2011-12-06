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
using We7.CMS;

namespace We7.CMS.Web.Admin.Addins
{
    public partial class ArticleListSelect : BasePage
    {
        public string QuoteOwnerID
        {
            get{return Request["oid"];}
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
