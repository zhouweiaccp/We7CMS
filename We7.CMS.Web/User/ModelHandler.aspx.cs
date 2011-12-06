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
using We7.Framework.Util;

namespace We7.CMS.Web.User
{
    public partial class ModelHandler : UserBasePage
    {
        protected global::We7.Model.UI.Panel.system.MultiPanel MultiPanel1;
        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        //取得当前信息的ID
        public string RecordID
        {
            get
            {
                if (ViewState["$RecordID"] == null)
                {
                    ViewState["$RecordID"] = Utils.CreateGUID();
                }
                return ViewState["$RecordID"] as string;
            }
            set
            {
                ViewState["$RecordID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MultiPanel1.ModelName =Request["model"];
        }
    }
}
