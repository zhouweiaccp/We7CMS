using System;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using We7.CMS.Common.PF;
using We7.Framework.Config;

namespace We7.CMS.Accounts
{
    /// <summary>
    /// Cookie类型
    /// </summary>
    internal enum CookieSupportType
    {
        Untested,
        Testing,
        Allowed,
        NotAllowed
    }

    /// <summary>
    /// 设置与获取用户验证票信息
    /// </summary>
    public class Security
    {
        private Security() { }

        private const string TESTCOOKIE_NAME = "TestCookie";

        public static event Action<string> AfterSetAccountID;

        /// <summary>
        /// 上下文
        /// </summary>
        static HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;
            else
            {
                password = password.ToLower();

                Byte[] clearBytes = new UnicodeEncoding().GetBytes(password);
                Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

                return BitConverter.ToString(hashedBytes);
            }
        }

        /// <summary>
        /// BBS论坛MD5加密字符串处理
        /// </summary>
        /// <param name="Half">加密是16位还是32位；如果为true为16位</param>
        /// <param name="Input">待加密码字符串</param>
        /// <returns></returns>
        public static string MD5(string Input, bool Half)
        {
            string output = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Input, "MD5").ToLower();
            if (Half)//16位MD5加密（取32位加密的6~22字符）
                output = output.Substring(8, 16);
            return output;
        }


        /// <summary>
        /// BBS密码加密方式
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string BbsEncrypt(string password)
        {
            return MD5(password, false);
        }

        /// <summary>
        /// 初始化密码
        /// </summary>
        /// <param name="current_account"></param>
        /// <returns></returns>
        public static string ResetPassword(Account current_account)
        {
            string password = RandomPassword();
            return password;
        }

        /// <summary>
        /// 随机取一个密码
        /// </summary>
        /// <returns></returns>
        public static string RandomPassword()
        {
            return Guid.NewGuid().ToString().Substring(0, 5);
        }

        /// <summary>
        /// Ajax进行数据验证
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <param name="persist">是否把用户信息保存在Cookie中</param>
        /// <returns></returns>
        static bool AjaxAuthenticate(string accountID, bool persist)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, accountID.Trim(), DateTime.Now, DateTime.Now.AddMinutes(30), false, "pwq", FormsAuthentication.FormsCookiePath);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (persist)
            {
                cookie.Expires.AddYears(10);
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
            return true;
        }

        /// <summary>
        /// 设置验证票证
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <param name="persist">是否持久保存</param>
        public static void SetTicket(string accountID, bool persist)
        {
            FormsAuthentication.SetAuthCookie(accountID, persist);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public static void SignOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Context.Session.Clear();
        }

        /// <summary>
        /// 设定当前用户ID到session或cookie
        /// </summary>
        /// <param name="accountID"></param>
        public static void SetAccountID(string accountID)
        {
            SetTicket(accountID, GeneralConfigs.GetConfig().EnableCookieAuthentication);
            Context.Session[AccountLocalHelper.AccountSessionKey] = accountID;
            if (AfterSetAccountID != null)
            {
                AfterSetAccountID(accountID);
            }
        }

        /// <summary>
        /// 设定当前用户ID到session或cookie
        /// </summary>
        /// <param name="accountID"></param>
        public static void SetAccountID(string accountID, bool persist)
        {
            SetTicket(accountID, persist);
            Context.Session[AccountLocalHelper.AccountSessionKey] = accountID;
            if (AfterSetAccountID != null)
            {
                AfterSetAccountID(accountID);
            }
        }

        /// <summary>
        /// 当前用户ID
        /// </summary>
        public static string CurrentAccountID
        {
            get
            {
                string currentAccountID = String.Empty;
                if (Context != null)
                {
                    if (Context.Request != null && HttpContext.Current.Request.IsAuthenticated)
                    {
                        try
                        {
                            currentAccountID = HttpContext.Current.User.Identity.Name;
                        }
                        catch
                        {
                        }
                    }
                    else if (Context.Session != null && Context.Session[AccountLocalHelper.AccountSessionKey] != null)
                    {
                        currentAccountID = Context.Session[AccountLocalHelper.AccountSessionKey] as string;
                    }
                }
                return currentAccountID;
            }
        }

        /// <summary>
        /// form验证是否通过
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(CurrentAccountID);
        }

        /// <summary>
        /// 浏览器是否支持Cookies
        /// </summary>
        /// <returns></returns>
        protected static bool AreCookiesAllowed()
        {
            if (!HasBeenTestedForCookies())
            {
                StartCookieTest();
                return false;
            }
            else
            {
                CookieSupportType testStatus = (CookieSupportType)Context.Session[Keys.SESSION_COOKIETEST];

                if (CookieSupportType.Testing != testStatus)
                {
                    if (CookieSupportType.Allowed == testStatus)
                        return true;
                    else
                        return false;
                }
                else
                    return FinishCookieTest();
            }
        }

        /// <summary>
        /// 检测Cookie是否可用
        /// </summary>
        /// <returns></returns>
        private static bool HasBeenTestedForCookies()
        {
            try
            {
                return (null != Context.Session[Keys.SESSION_COOKIETEST]);
            }
            catch (HttpException)
            {
                return false;
            }
        }


        /// <summary>
        /// 开始Cookie检测
        /// </summary>
        private static void StartCookieTest()
        {
            try
            {
                Context.Session[Keys.SESSION_COOKIETEST] = CookieSupportType.Testing;
                Context.Response.Cookies.Add(new HttpCookie(TESTCOOKIE_NAME, DateTime.Now.ToString()));
            }
            catch (HttpException)
            {
                return;
            }
        }

        /// <summary>
        /// Cookie检测后的动作
        /// </summary>
        /// <returns></returns>
        private static bool FinishCookieTest()
        {
            string testValue = Context.Request.Cookies[TESTCOOKIE_NAME].Value;
            if (0 != testValue.Length)
            {
                Context.Response.Cookies.Remove(TESTCOOKIE_NAME);
                Context.Session[Keys.SESSION_COOKIETEST] = CookieSupportType.Allowed;
                return true;
            }
            else
            {
                Context.Session[Keys.SESSION_COOKIETEST] = CookieSupportType.NotAllowed;
                return false;
            }
        }
    }
}
