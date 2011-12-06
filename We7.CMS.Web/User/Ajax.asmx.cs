using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using We7.CMS.Accounts;
using System.Web.Script.Services;

namespace We7.CMS.Web.User
{
    /// <summary>
    /// Summary description for Ajax
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class Ajax : System.Web.Services.WebService
    {

        /// <summary>
        /// 会员中心登陆
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [WebMethod(EnableSession=true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] Login(string username, string password)
        {
            IAccountHelper AccountHelper = AccountFactory.CreateInstance();

            return AccountHelper.Login(username, password);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Signout()
        {
            IAccountHelper AccountHelper = AccountFactory.CreateInstance();
            return AccountHelper.SignOut();
        }
    }
}
