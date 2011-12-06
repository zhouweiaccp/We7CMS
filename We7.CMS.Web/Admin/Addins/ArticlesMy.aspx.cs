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
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class ArticlesMy : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string OwnerID
        {
            get
            {
                string oid = Request["oid"];
                if (oid == null)
                {
                    return We7Helper.EmptyGUID;
                }
                return oid;
            }
        }

        bool IsWap
        {
            get { return Request["wap"] != null; }
        }

        protected override void Initialize()
        {
            SummaryLabel.Text = We7Helper.IsEmptyID(OwnerID) ? "¶¥¼¶Ä¿Â¼" : ChannelHelper.GetChannel(OwnerID, new string[] { "ID", "FullPath" }).FullPath;
        }
    }
}
