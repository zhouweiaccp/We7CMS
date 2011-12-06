using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin.Addins
{
    public partial class ModelEmpty : BasePage
    {
        protected override We7.CMS.Common.Enum.MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.NoMenu;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Channel ch = ChannelHelper.GetChannel(Request["oid"],null);
            if (ch != null)
            {
                NameLabel.Text = ch.Name;
            }
        }
    }
}
