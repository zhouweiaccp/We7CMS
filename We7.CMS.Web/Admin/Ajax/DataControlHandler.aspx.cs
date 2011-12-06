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
using We7.CMS;
using System.IO;
using We7.CMS.Common.Enum;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.Ajax
{
    public partial class DataControlHandler : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            JsonResult result;
            String ctr = Request["ctr"];
            if (String.IsNullOrEmpty(ctr))
            {
                result = new JsonResult(false);
            }
            else
            {
                try
                {
                    DataControl datacontrol = DataControlHelper.GetDataControl(ctr.Trim());
                    result = new JsonResult(true, datacontrol);
                }
                catch (Exception ex)
                {
                    result = new JsonResult(false, ex.Message);
                }
            }
            result.Response();
        }
    }
}

