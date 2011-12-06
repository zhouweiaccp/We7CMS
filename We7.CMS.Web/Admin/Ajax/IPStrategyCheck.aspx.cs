using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Config;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin.Ajax
{
    public partial class IPStrategyCheck1 : BasePage
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
            ProcessRequest();
        }

        private void ProcessRequest()
        {
            bool result = false;
            Response.ContentType = "text/html";
            string key = Request.QueryString["key"];
            string type = Request.QueryString["type"];

            if (!String.IsNullOrEmpty(key))
            {

                if (type == "1")
                {

                    StrategyInfo info = StrategyConfigs.Instance[key];
                    result = info == null ? false : true;
                }
                else
                {
                    result = StrategyConfigs.Instance.ContainsName(key.Trim());
                }
            }
            Response.Clear();
            Response.Write(result.ToString().ToLower());
        }
    }
}
