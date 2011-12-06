using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7;
using System.Text.RegularExpressions;
using We7.CMS.Controls;
using System.Net;
using System.IO;
using System.Text;
using We7.CMS.Config;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using We7.Framework.Config;
using We7.CMS.Accounts;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin
{
    public partial class Signin : BasePage
    {
        protected override bool NeedAnAccount
        {
            get { return false; }
        }
        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        string ReturnURL
        {
            get
            {
                if (Request["ReturnURL"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return Server.UrlDecode(Request["ReturnURL"].ToString());
                }
            }
        }
        protected string ProductBrand
        {
            get
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si != null)
                {
                    return si.ProductName;
                }
                else
                    return "We7";
            }
        }
        int LoginCount
        {
            get
            {
                int count = 0;
                if (ViewState[this.ID + "_LoginCount"] != null)
                {
                    count = (int)ViewState[this.ID + "_LoginCount"];
                }
                return count;
            }
            set
            {
                ViewState[this.ID + "_LoginCount"] = value;
            }
        }
        void ShowMessage(string m)
        {
            MessageLabel.Text = m;
        }
        private void GenerateRandomCode()
        {
            if (CDHelper.Config.EnableLoginAuhenCode == "true")
            {
                tbAuthenCode2.Visible = true;
                Response.Cookies["AreYouHuman"].Value = CaptchaImage.GenerateRandomCode();
            }
        }


        /// <summary>
        /// 原始登录的方法
        /// </summary>
        /// <param name="loginName">本地用户名</param>
        /// <param name="password">本地用户的密码</param>
        /// <param name="checkPassword">是否校验密码</param>
        void LoginAction(string loginName, string password)
        {
            if (String.IsNullOrEmpty(loginName) || String.IsNullOrEmpty(loginName.Trim()))
            {
                ShowMessage("错误：用户名不能为空！");
                return;
            }

            if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password.Trim()))
            {
                ShowMessage("错误：密码不能为空！");
                return;
            }

            if (GeneralConfigs.GetConfig().EnableLoginAuhenCode == "true" && this.CodeNumberTextBox.Text != Request.Cookies["AreYouHuman"].Value)
            {
                ShowMessage("错误：您输入的验证码不正确，请重新输入！");
                this.CodeNumberTextBox.Text = "";
                Response.Cookies["AreYouHuman"].Value = CaptchaImage.GenerateRandomCode();
                return;
            }

            bool loginSuccess = false;
            if (CheckLocalAdministrator(loginName))
            {
                if (CDHelper.AdminPasswordIsValid(password))
                {
                    Security.SetAccountID(We7Helper.EmptyGUID);
                    loginSuccess = true;
                    SSOLogin(loginName, password);
                }
                else
                {
                    ShowMessage("无法登录，原因：密码错误！");
                    return;
                }
            }
            else
            {
                string[] result = AccountHelper.Login(loginName, password);
                if (result[0] == "false")
                {
                    ShowMessage("无法登录，原因：" + result[1]);
                    return;
                }
                else
                {
                    SSOLogin(loginName, password);
                }
            }

            GoWhere();
        }



        private void GoWhere()
        {
            NewSiteConfig();
            if (ReturnURL == null || ReturnURL == string.Empty)
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                Response.Redirect(ReturnURL);
            }
        }

        /// <summary>
        /// 是否超级用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        bool CheckLocalAdministrator(string loginName)
        {
            if (String.Compare(loginName, SiteConfigs.GetConfig().AdministratorName, true) == 0  )
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 初始化站点
        /// </summary>
        /// <returns></returns>
        private void NewSiteConfig()
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (string.IsNullOrEmpty(si.SiteTitle) || string.IsNullOrEmpty(si.Copyright) || string.IsNullOrEmpty(si.SiteFullName) || string.IsNullOrEmpty(si.IcpInfo) || string.IsNullOrEmpty(si.SiteLogo))
            {
                Response.Redirect(AppPath + "/NewSiteWizard.aspx?nomenu=1");
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si != null)
                {
                    if (si.IsOEM)
                        CopyrightLiteral.Text = si.Copyright;
                    else
                        CopyrightLiteral.Text = si.CopyrightOfWe7;
                }

                SiteConfigInfo sci = SiteConfigs.GetConfig();
                if (sci == null)
                {
                    Response.Write("对不起,您的系统已升级，但配置文件尚未升级，您需要对配置数据进行升级。现在升级吗？<a href='../install/upgradeconfig.aspx'><u>现在升级</u></a>");
                    Response.End();
                }
                else
                {
#if DEBUG
                    LoginNameTextBox.Text = sci.AdministratorName;
#endif
                    GenerateRandomCode();
                    if (Request["action"] != null && Request["action"].ToString() == "logout" && Request["Authenticator"] == null)
                    {
                        //记录日志
                        string content = string.Format("退出站点");
                        AddLog("站点登录", content);
                        string result = SignOut();
                        if (!string.IsNullOrEmpty(result))
                            ShowMessage("登录退出没有成功！原因：" + result);
                        else
                            SSOLogout();
                    }
                }
            }

            if (Request["user"] != null && Request["pass"] != null)
            {
                LoginAction(Request["user"].ToString(), Request["pass"].ToString());
            }

            if (Request["Authenticator"] != null && Request["accountID"] != null)
            {
                SSORequest ssoRequest = SSORequest.GetRequest(HttpContext.Current);
                string actID = ssoRequest.AccountID;
                if (Authentication.ValidateEACToken(ssoRequest) && !string.IsNullOrEmpty(actID) && We7Helper.IsGUID(actID))
                {
                    Security.SetAccountID(actID);
                    SSOLogin(ssoRequest.UserName, ssoRequest.Password);
                    GoWhere();
                }
                else if (Request["message"] != null)
                {
                    ShowMessage("登录失败！原因：" + Request["message"]);
                    return;
                }
            }
        }

        void AddUserLoginStatistics()
        {
            PageVisitorHelper.AddPageVisitor(AccountID);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            LoginAction(LoginNameTextBox.Text.Trim(), PasswordTextBox.Text);
        }

        private void SSOLogin(string userName, string password)
        {
            if (!String.IsNullOrEmpty(GeneralConfigs.GetConfig().SSOSiteUrls))
            {
                SSORequest ssoRequest = new SSORequest();
                ssoRequest.ToUrls = GeneralConfigs.GetConfig().SSOSiteUrls;
                ssoRequest.AppUrl = String.Format("{0}/{1}", Utils.GetRootUrl(), String.IsNullOrEmpty(ReturnURL) ? "Admin/theme/classic/main.aspx" : ReturnURL.TrimStart('/'));
                ssoRequest.Action = "signin";
                ssoRequest.UserName = userName;
                ssoRequest.Password = password;
                Authentication.PostChains(ssoRequest);
            }
        }

        private void SSOLogout()
        {
            if (!String.IsNullOrEmpty(GeneralConfigs.GetConfig().SSOSiteUrls))
            {
                SSORequest ssoRequest = new SSORequest();
                ssoRequest.ToUrls = GeneralConfigs.GetConfig().SSOSiteUrls;
                ssoRequest.AppUrl = String.Format("{0}/{1}", Utils.GetRootUrl(),"Admin/Signin.aspx");
                ssoRequest.Action = "logout";
                Authentication.PostChains(ssoRequest);
            }
        }
    }
}
