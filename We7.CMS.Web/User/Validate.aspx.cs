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
using We7.Framework;
using We7.CMS.Common.PF;
using We7.CMS.Accounts;

namespace We7.CMS.Web.User
{
    public partial class Validate : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string accoundID = Request[We7.Model.Core.UI.Constants.FEID];
            if (!String.IsNullOrEmpty(accoundID))
            {
                Account account = AccountHelper.GetAccount(accoundID, null);
                if (account == null)
                {
                    Response.Write("验证帐号不存在，请重新申请帐号!");
                }
                else
                {
                    account.EmailValidate = 1;
                    account.State = 1;
                    AccountHelper.UpdateAccount(account, new string[] { "EmailValidate", "State" });
                    if (!string.IsNullOrEmpty(Request["returnUrl"]))
                    {
                        string url = Request["returnUrl"];
                        url = We7Helper.AddParamToUrl(url, We7.Model.Core.UI.Constants.FEID, account.ID);
                        url = We7Helper.AddParamToUrl(url, "activeIndex", "2");
                        Response.Redirect(url);
                    }

                }
            }

        }

        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get {               
                return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }
    }
}
