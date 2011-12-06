using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using We7.CMS.Common.PF;
using We7.Framework.Config;
using We7.CMS.Accounts;

namespace We7.CMS.WebControls
{
   
    /// <summary>
    /// 重新构建了登陆控件
    /// </summary>
    public class LoginProviderEx : BaseWebControl
    {
        private bool _isHtml = false;
        /// <summary>
        /// 是否需要静态化
        /// </summary>
        public override bool IsHtml
        {
            get { return false; }
            set {_isHtml = value;}
        }
        bool isCustomerReturnUrl = true;

        /// <summary>
        /// 是否使用之定义返回Url
        /// </summary>
        public bool IsCustomerReturnUrl
        {
            get { return isCustomerReturnUrl; }
            set { isCustomerReturnUrl = value; }
        }
        string customerReturnUrl = "/user/index.aspx";
        /// <summary>
        /// 自定义的返回Url
        /// </summary>
        public string CustomerReturnUrl
        {
            get { return customerReturnUrl; }
            set { customerReturnUrl = value; }
        }
        public string LoginName { get; set; }

        public string Password { get; set; }

        public string Action { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public bool IsSignIn { get; set; }

        public bool IsPersist { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IsSignIn = Security.IsAuthenticated();        
            if (Html.IsPostBack)
            {
                InitParam();
                if (Action == "login")
                {
                    Authenticate();
                }
                else if (Action == "logout")
                {
                    Signout();
                }
            }
            if (!String.IsNullOrEmpty(Request["Authenticator"]) && !String.IsNullOrEmpty(Request["accountID"]))
            {
                SSORequest ssoRequest = SSORequest.GetRequest(HttpContext.Current);
                string actID = ssoRequest.AccountID;
                if (Authentication.ValidateEACToken(ssoRequest) && !string.IsNullOrEmpty(actID) && We7Helper.IsGUID(actID))
                {
                    Security.SetAccountID(actID);
                    IsSignIn = true;
                }
            }
            if (Security.IsAuthenticated())
                Response.Redirect(ReturnUrl);
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        void Authenticate()
        {
            if (String.Compare(LoginName, SiteConfigs.GetConfig().AdministratorName, false) == 0)
            {
                if (CDHelper.AdminPasswordIsValid(Password))
                {
                    Security.SetAccountID(We7Helper.EmptyGUID);
                    UserName = SiteConfigs.GetConfig().AdministratorName;
                    IsSignIn = true;
                }
                else
                {
                    IsSignIn = false;
                    Message="密码错误";
                }
            }
            else
            {
                if (Request["Authenticator"] != null && Request["accountID"] != null)
                {
                    SSORequest ssoRequest = SSORequest.GetRequest(HttpContext.Current);
                    string actID = ssoRequest.AccountID;
                    if (Authentication.ValidateEACToken(ssoRequest) && !string.IsNullOrEmpty(actID) && We7Helper.IsGUID(actID))
                    {
                        Security.SetAccountID(actID,IsPersist);
                        UserName = ssoRequest.UserName;
                        IsSignIn = true;
                    }
                    else if (Request["message"] != null)
                    {
                        Message = Request["message"];
                        IsSignIn=false;
                        return;
                    }
                }
                else
                {
                    IAccountHelper AccountHelper = AccountFactory.CreateInstance();

                    string[] result = AccountHelper.Login(LoginName, Password);

                    if (result[0] == "false")
                    {
                        Message = result[1];
                        IsSignIn = false;
                    }
                    else
                    {
                        IsSignIn = true;
                        UserName = AccountHelper.GetAccount(result[1], new string[] { "LoginName" }).LoginName;
                        Response.Redirect(ReturnUrl);
                    }
                }
            }            
        }

        void Signout()
        {
            if (Request["Authenticator"] == null)
            {
                IAccountHelper AccountHelper = AccountFactory.CreateInstance();
                string result = AccountHelper.SignOut();
                IsSignIn = false;
            }
        }

        void InitParam()
        {
            LoginName = Html.Request<string>("LoginName");
            Password = Html.Request<string>("Password");
            Action = Html.Request<string>("Action");
            IsPersist = Html.Request<string>("Persist") == "1";
        }

        #region 私有方法
        void SetCookie(IDToken idtoken)
        {
            HttpCookie cookie = new HttpCookie("wethepowerseven");

            cookie.Values["PassportID"] = idtoken.PassportID;
            cookie.Values["ProviderSiteID"] = idtoken.ProviderSiteID;
            cookie.Values["ProviderSiteTitle"] = idtoken.ProviderSiteTitle;
            cookie.Values["LoginSiteID"] = idtoken.LoginSiteID;
            cookie.Values["LoginSiteTitle"] = idtoken.LoginSiteTitle;
            cookie.Values["UserName"] = idtoken.UserName;
            cookie.Values["Status"] = Convert.ToString(idtoken.Status);
            cookie.Values["ReturnUrl"] = idtoken.ReturnUrl;
            cookie.Values["Action"] = idtoken.Action;
            cookie.Values["Exist"] = idtoken.LoginSiteID;

            cookie.Expires = DateTime.Now.AddHours(12);

            //string tmp1 = Request.Url; //CDHelper.GetSystemParameter(CDHelper.SIRootUrl);
            string domain = "";
            try
            {
                Uri uri = Request.Url;
                //Uri uri = new Uri(tmp1);
                if (uri.HostNameType == UriHostNameType.Dns)
                {
                    int start = uri.Host.IndexOf(".", 0);
                    if (start <= 0)
                    {
                        //domain = uri.Host;
                    }
                    else
                    {
                        domain = uri.Host.Substring(start);
                    }
                }
                else
                {
                    domain = uri.Host;
                }
            }
            catch (Exception)
            {
            }

            if (domain != "")
            {
                cookie.Domain = domain;
            }

            if (Request.Cookies["wethepowerseven"] == null)
            {
                Response.Cookies.Add(cookie);
            }
            else
            {
                Response.Cookies.Set(cookie);
            }

        }

        #endregion


        /// <summary>
        ///　转向的Url
        /// </summary>
        public string ReturnUrl
        {
            get
            {
                if (string.IsNullOrEmpty(Request["ReturnUrl"]))
                {
                    if (isCustomerReturnUrl)
                    {
                        if (customerReturnUrl.Trim().StartsWith("http://") || customerReturnUrl.Trim().StartsWith("https://")
                           || customerReturnUrl.Trim().StartsWith("/"))
                        {
                            return customerReturnUrl;
                        }
                        else
                        {
                            return string.Format("{0}" + customerReturnUrl, "/");
                        }

                    }
                    else
                    {
                        return "/";
                    }
                }
                else
                {
                    return Request["ReturnUrl"];
                }
            }
        }



    }

    /// <summary>
    /// 登陆数据提供者
    /// </summary>
    public class LoginProvider : BaseWebControl
    {
        private bool _isHtml = false;
        /// <summary>
        /// 是否需要静态化
        /// </summary>
        public override bool IsHtml
        {
            get { return false; }
            set {_isHtml = value;}
        }

        string cssClass;
        /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        bool iSValidate;
        /// <summary>
        /// 是否验证
        /// </summary>
        public bool ISValidate
        {
            get { return iSValidate; }
            set { iSValidate = value; }
        }

        bool isCustomerReturnUrl = true;

        /// <summary>
        /// 是否使用之定义返回Url
        /// </summary>
        public bool IsCustomerReturnUrl
        {
            get { return isCustomerReturnUrl; }
            set { isCustomerReturnUrl = value; }
        }
        string customerReturnUrl = "/user/index.aspx";
        /// <summary>
        /// 自定义的返回Url
        /// </summary>
        public string CustomerReturnUrl
        {
            get { return customerReturnUrl; }
            set { customerReturnUrl = value; }
        }
        /// <summary>
        /// 选择验证
        /// </summary>
        public string SelectValidate
        {
            get
            {
                return ISValidate && !Security.IsAuthenticated() ? "" : "none";
            }
        }
        /// <summary>
        /// 当前登录用户账号用户名
        /// </summary>
        public string AccountName
        {
            get
            {
                if (Security.IsAuthenticated())
                {
                    if (Security.CurrentAccountID == We7Helper.EmptyGUID)
                        return "管理员";
                    else
                    {
                        Account ac = AccountHelper.GetAccount(Security.CurrentAccountID, new string[] { "Email", "ID" });
                        if (ac != null)
                        {
                            return ac.LastName;
                        }
                        else
                        {
                            return "未知用户";
                        }
                    }
                }
                else
                {
                    return "";
                }
            }

        }

        /// <summary>
        /// 获取当前登录用户的部门
        /// </summary>
        //TODO::显示部门报NULLreference错误
        public string DepartmentName
        {
            get
            {
                if (!Security.IsAuthenticated())
                {
                    return "";
                }
                else
                {
                    try
                    {
                        if (string.IsNullOrEmpty(AccountName))
                        {
                            return string.Empty;
                        }
                        IAccountHelper helper = AccountFactory.CreateInstance();
                        Account ac = AccountHelper.GetAccount(Security.CurrentAccountID, new string[] { "Email", "ID" });
                        if (ac == null)
                        {
                            return string.Empty;
                        }
                        string departmentId = ac.DepartmentID;
                        if (string.IsNullOrEmpty(departmentId))
                        {
                            return string.Empty;
                        }
                        Department department = helper.GetDepartment(departmentId, new string[] { "Name"});
                        if (department != null)
                        {
                            return department.Name;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    catch { return string.Empty; }
                }
            }
        }
        /// <summary>
        /// 登录后信息显示
        /// </summary>
        public string LoginNameDisplay
        {
            get
            {
                return Security.IsAuthenticated() ? "" : "none";
            }
        }

        /// <summary>
        /// 登录框显示
        /// </summary>
        public string LoginInputDisplay
        {
            get
            {
                return Security.IsAuthenticated() ? "none" : "";
            }
        }


        /// <summary>
        ///　转向的Url
        /// </summary>
        public string ReturnUrl
        {
            get
            {
                if (string.IsNullOrEmpty(Request["ReturnUrl"]))
                {
                    if (isCustomerReturnUrl)
                    {
                        if (customerReturnUrl.Trim().StartsWith("http://") || customerReturnUrl.Trim().StartsWith("https://")
                           || customerReturnUrl.Trim().StartsWith("/"))
                        {
                            return customerReturnUrl;
                        }
                        else
                        {
                            return string.Format("{0}" + customerReturnUrl, "/");
                        }

                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return Request["ReturnUrl"];
                }
            }
        }

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Security.IsAuthenticated() && !string.IsNullOrEmpty(ReturnUrl))
                Response.Redirect(ReturnUrl);
        }
    }

}