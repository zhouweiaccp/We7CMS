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
using We7.CMS.Accounts;
using We7.CMS.Common.PF;
using We7.Framework.Config;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class AuthenticationAPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            try
            {
                string action = Request["Action"] ?? "".ToLower();
                switch (action)
                {
                    case "signin":
                        Signin();
                        break;
                    case "logout":
                        LogOut();
                        break;
                }
            }
            catch { }
            finally
            {
                SSORequest request = SSORequest.GetRequest(HttpContext.Current);
                if (String.IsNullOrEmpty(Request["ToUrls"]))
                {
                    request.ToUrls = Request["ToUrls"];
                    request.AppUrl = Request["AppUrl"];
                    request.UserName = Request["UserName"];
                    request.Password = Request["Password"];
                    request.Action = Request["Action"];
                }
                Authentication.PostChains(request);
            }
        }


        /// <summary>
        /// 通用信息业务对象
        /// </summary>
        protected SiteSettingHelper CDHelper
        {
            get { return HelperFactory.Instance.GetHelper<SiteSettingHelper>(); }
        }

        void Signin()
        {
            string name = Request["UserName"];
            string password = Request["Password"];

            if (String.Compare(name, SiteConfigs.GetConfig().AdministratorName, true) == 0 &&
                CDHelper.AdminPasswordIsValid(password))
            {
                Security.SetAccountID(We7Helper.EmptyGUID);
            }
            else
            {
                IAccountHelper helper = AccountFactory.CreateInstance();
                Account account = helper.GetAccountByLoginName(name);
                if (account != null && helper.IsValidPassword(account, password))
                {
                    Security.SetAccountID(account.ID);
                }
            }
        }

        void LogOut()
        {
            Security.SignOut();
        }
    }
}
