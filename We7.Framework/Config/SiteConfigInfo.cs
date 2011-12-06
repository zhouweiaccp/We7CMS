using System;
using System.Web;

namespace We7.Framework.Config
{
    /// <summary>
    /// 网站基本设置描述类, 加[Serializable]标记为可序列化
    /// </summary>
    [Serializable]
    public class SiteConfigInfo : IConfigInfo
    {
        string administratorID = "{00000000-0000-0000-0000-000000000000}";
        string administratorName = "Administrator";
        string administratorKey = "1";
        bool isPasswordHashed = false;
        string companyID;
        string siteID;
        string companyName = "您的站点";
        string compangDescription;
        string rootUrl;

        string wdKey;
        string wdUrl;
        string idUrl;
        string ssoUrl;
        string ssoServername;
        string ssoUsername;
        string ssoPassword;
        string ssoDomain;
        string ssoWdID;

        bool siteGroupEnabled = false;

        string wDWebUrl;

        /// <summary>
        /// 站群控制台Web地址
        /// </summary>
        public string WDWebUrl
        {
            get { return wDWebUrl; }
            set { wDWebUrl = value; }
        }

        /// <summary>
        /// 站群功能是否启用
        /// </summary>
        public bool SiteGroupEnabled
        {
            get { return siteGroupEnabled; }
            set { siteGroupEnabled = value; }
        }

        /// <summary>
        /// 网站Webtie的ID号
        /// </summary>
        public string SiteID
        {
            get { return siteID; }
            set { siteID = value; }
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string SiteName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        /// <summary>
        /// 站点名称，兼容过去，暂时留着
        /// </summary>
        public string CompanyName
        {
            get
            {
                return companyName;
            }
            set
            {
                companyName = value;
            }
        }

        /// <summary>
        /// 站点描述
        /// </summary>
        public string SiteDescription
        {
            get { return compangDescription; }
            set { compangDescription = value; }
        }

        /// <summary>
        /// 根地址
        /// </summary>
        public string RootUrl
        {
            get { return rootUrl; }
            set { rootUrl = value; }
        }

        /// <summary>
        /// WD服务验证字
        /// </summary>
        public string WebGroupSericeKey
        {
            get { return wdKey; }
            set { wdKey = value; }
        }

        /// <summary>
        /// WD服务URL接口地址
        /// </summary>
        public string WebGroupServiceUrl
        {
            get { return wdUrl; }
            set { wdUrl = value; }
        }

        /// <summary>
        /// ID共享服务器URL地址
        /// </summary>
        public string InformationServiceUrl
        {
            get { return idUrl; }
            set { idUrl = value; }
        }

        /// <summary>
        /// 相当于产生AccountID
        /// </summary>
        public string AdministratorID
        {
            get { return administratorID; }
            set { administratorID = value; }
        }

        /// <summary>
        /// 默认为Administrator
        /// </summary>
        public string AdministratorName
        {
            get { return administratorName; }
            set { administratorName = value; }
        }

        /// <summary>
        /// administrator密码
        /// </summary>
        public string AdministratorKey
        {
            get { return administratorKey; }
            set { administratorKey = value; }
        }

        /// <summary>
        /// 管理员密码是否加密
        /// </summary>
        public bool IsPasswordHashed
        {
            get { return isPasswordHashed; }
            set { isPasswordHashed = value; }
        }

        /// <summary>
        /// SSO接口URL
        /// </summary>
        public string SsoUrl
        {
            get { return ssoUrl; }
            set { ssoUrl = value; }
        }

        /// <summary>
        /// SSO服务器名称
        /// </summary>
        public string SsoServername
        {
            get { return ssoServername; }
            set { ssoServername = value; }
        }

        /// <summary>
        /// SSO用户
        /// </summary>
        public string SsoUsername
        {
            get { return ssoUsername; }
            set { ssoUsername = value; }
        }

        /// <summary>
        /// SSO密码
        /// </summary>
        public string SsoPassword
        {
            get { return ssoPassword; }
            set { ssoPassword = value; }
        }

        /// <summary>
        /// SSO域名
        /// </summary>
        public string SsoDomain
        {
            get { return ssoDomain; }
            set { ssoDomain = value; }
        }

        /// <summary>
        /// WD的 ID
        /// </summary>
        public string SsoWdID
        {
            get { return ssoWdID; }
            set { ssoWdID = value; }
        }

        string adUrl;
        /// <summary>
        /// 站群广告管理站点地址
        /// </summary>
        public string ADUrl
        {
            get { return adUrl; }
            set { adUrl = value; }
        }

        string fullTextSearchUrl;
        /// <summary>
        /// 全文检索服务器地址
        /// </summary>
        public string FullTextSearchUrl
        {
            get
            {
                return fullTextSearchUrl;
            }
            set
            {
                fullTextSearchUrl = value;
            }
        }

        private string siteUrl;
        /// <summary>
        /// 当前站点的Url
        /// </summary>
        public string SiteUrl
        {
            get
            {
                if (String.IsNullOrEmpty(siteUrl))
                {
                    siteUrl="http://"+HttpContext.Current.Request.Url.Host;

                    string port = HttpContext.Current.Request.Url.Port.ToString();
                    if (port != "80")
                    {
                        siteUrl += ":" + port;
                    }
                    siteUrl = siteUrl.ToLower();
                }
                return siteUrl;
            }
            set { siteUrl = value; }
        }

        /// <summary>
        /// 站点插件序列号
        /// </summary>
        public string PluginSN { get; set; }

        /// <summary>
        /// 插件商店登陆名
        /// </summary>
        public string ShopLoginName { get; set; }

        /// <summary>
        /// 插件商店密码
        /// </summary>
        public string ShopPassword { get; set; }

        /// <summary>
        /// 是否启用LDAP服务器，如AD
        /// </summary>
        public bool LDAPEnable { get; set; }

        /// <summary>
        /// 集中身份认证服务地址
        /// </summary>
        public string PassportServiceUrl { get; set; }
        /// <summary>
        /// 集中身份认证，登录验证页面
        /// </summary>
        public string PassportAuthPage
        {
            get
            {
                if (!string.IsNullOrEmpty(PassportServiceUrl))
                {
                    string url = PassportServiceUrl.Remove(PassportServiceUrl.LastIndexOf("/"));
                    url += "/Authentication.aspx";
                    return url;
                }
                else
                    return string.Empty;
            }
        }

    }
}