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
using We7.CMS.WebControls;
using We7.CMS.Common.Enum;
using We7.CMS.Accounts;

namespace We7.CMS.Web.User
{
    public partial class Logout : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Request["Authenticator"] == null)
            {
                IAccountHelper AccountHelper = AccountFactory.CreateInstance();
                string result = AccountHelper.SignOut();
            }
            //退出时是否跳转对应页面
            if (!string.IsNullOrEmpty(Request.Params["returnurl"]))
            {
                Response.Redirect(Request.Params["returnurl"]);
            }
            Response.Redirect("/");
        }
    }
}
