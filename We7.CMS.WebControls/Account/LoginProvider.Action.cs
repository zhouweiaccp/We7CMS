using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS;
using We7;
using We7.CMS.Common;
using We7.CMS.Common.PF;
using We7.CMS.Accounts;
using System.Web;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 登录数据处理类
    /// </summary>
    public class LoginAction : BaseAction
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否登陆
        /// </summary>
        public bool IsSignin { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidateCode { get; set; }

        /// <summary>
        /// 当前用户
        /// </summary>
        public string CurrentAccount { get; set; }

        /// <summary>
        /// 是否需要验证码
        /// </summary>
        public bool ISValidate { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 转向URL
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 当前动作
        /// </summary>
        public string Action { get; set; }


        public override void Execute()
        {
            if (IsSignin)
            {
                if (Request["Authenticator"] == null)
                {
                    Session["$ActionFrom"] = Request.UrlReferrer.PathAndQuery;
                    Session["$_ActionID"] = _ActionID;
                    IAccountHelper AccountHelper = AccountFactory.CreateInstance();
                    string result = AccountHelper.SignOut();
                }
            }
            else
            {
                if (CheckValidateCode())
                    Authenticate();
            }
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        void Authenticate()
        {
            if (Request["Authenticator"] != null && Request["accountID"] != null)
            {
                SSORequest ssoRequest = SSORequest.GetRequest(HttpContext.Current);
                string actID = ssoRequest.AccountID;
                if (Authentication.ValidateEACToken(ssoRequest) && !string.IsNullOrEmpty(actID) && We7Helper.IsGUID(actID))
                {
                    Security.SetAccountID(actID);
                }
                else if(Request["message"]!=null)
                {
                    Message = Request["message"];
                    return;
                }
            }
            else
            {
                Session["$ActionFrom"] = Request.UrlReferrer.PathAndQuery;
                Session["$_ActionID"] = _ActionID;
                IAccountHelper AccountHelper = AccountFactory.CreateInstance();
                string loginName = Name;
                //邮箱格式 
                if (Name.IndexOf('@') > -1)
                {
                    Account account =  AccountHelper.GetAccountByEmail(Name);
                    if(account!=null)
                        loginName = account.LoginName;
                }

                string[] result = AccountHelper.Login(loginName, Password);
                if (result[0] == "false")
                {
                    Message = result[1];
                    return;
                }
                else
                {
                    Author = result[1];
                }

            }

            if (!string.IsNullOrEmpty(ReturnUrl))
                Response.Redirect(ReturnUrl);
        }

        /// <summary>
        /// 检测验证码
        /// </summary>
        /// <returns>验证码是否正确</returns>
        bool CheckValidateCode()
        {
            if (ISValidate &&  ValidateCode != Request.Cookies["AreYouHuman"].Value)
            {
                Message = "验证码出错";
                return false;
            }
            return true;
        }
    }
}
