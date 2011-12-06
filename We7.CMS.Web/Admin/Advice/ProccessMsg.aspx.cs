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
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
    public partial class ProccessMsg :BasePage
    {
        private IAdviceHelper adviceHelper = AdviceFactory.Create();

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        protected int Type
        {
            get 
            {
                int i;
                int.TryParse(HttpUtility.UrlDecode(Request["type"]), out i);
                return i;
            }
        }

        protected string BackUrl
        {
            get 
            {
                if(!String.IsNullOrEmpty(Request["typeID"]))
                {
                    return "/Admin/Advice/AdviceListEx.aspx?typeID=" + HttpUtility.UrlDecode(Request["typeID"]);
                }
                return String.Empty;
            }
        }

        protected string Msg
        {
            get
            {
                return HttpUtility.UrlDecode(Request["msg"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            NameLabel.Text = AdviceType.Title + "办理中心";
            SummaryLabel.Text = AdviceType.Description;
        }

        private AdviceType adviceType;
        public AdviceType AdviceType
        {
            get
            {
                if (adviceType == null)
                {
                    adviceType = adviceHelper.GetAdviceType(Request["typeID"]);
                }
                return adviceType;
            }
        }

        public static void Redirect(int type, string typeID, string message)
        {
            string url = "ProccessMsg.aspx";
            url = We7Helper.AddParamToUrl(url, "type", HttpUtility.UrlEncode(type.ToString()));
            url = We7Helper.AddParamToUrl(url, "typeID", HttpUtility.UrlEncode(typeID));
            url = We7Helper.AddParamToUrl(url, "msg", HttpUtility.UrlEncode(message));
            HttpContext.Current.Response.Redirect(url);
        }
    }
}
