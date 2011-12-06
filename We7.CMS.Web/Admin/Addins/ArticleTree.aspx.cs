using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin.Addins
{
    public partial class ArticleTree : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                if (Request["notiframe"] != null)
                    return MasterPageMode.FullMenu;
                else
                    return MasterPageMode.NoMenu;
            }
        }

        public string OwnerID
        {
            get
            {
                string oid = Request["oid"];
                return oid;
            }
        }

        public string ArticleID
        {
            get { return Request["id"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
