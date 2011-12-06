using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;

using Thinkment.Data;

using We7.CMS.Config;
using We7.CMS.Common;
using We7.CMS.Common.PF;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Accounts;

namespace We7.CMS
{
    /// <summary>
    /// 网站基本参数配置类
    /// </summary>
    [Helper("We7.CMS.Helper")]
    public class SiteSettingHelper : BaseHelper
    {
        #region 定义网站基本参数名称
        static string SIAdministratorID = "{00000000-0000-0000-0000-000000000000}";
         static string SIAdministratorKey = "CD.AdministratorKey";
         static string SICompanyName = "WD.CompanyName";
         static string SICompanyDescription = "WD.CompanyDescription";
         static string SICompanyID = "WD.CompanyID";
         static string SIProduct = "CD.Product";
         static string SIVersion = "CD.Version";
         static string SIWDUrl = "WD.Url";
         static string SIIDUrl = "ID.Url";
         static string SISiteID = "CD.SiteID";
         static string SIAllowSignup = "CD.AllowSignup";
         static string SIRootUrl = "CD.RootUrl";
         static string SIArticleAutoPublish = "CD.ArticleAutoPublish";
         static string SIArticleAutoShare = "CD.ArticleAutoShare";
         static string SIAllCutCheckBox = "CD.AllCutCheckBox";
         static string SIDefaultTemplateGroup = "CD.DefaultTemplateGroup";
         static string SIDefaultTemplateGroupFileName = "CD.DefaultTemplateGroupFileName";
         static string SIDefaultHomePageTitle = "CD.DefaultHomePageTitle";
         static string SIDefaultChannelPageTitle = "CD.DefaultChannelPageTitle";
         static string SIDefaultContentPageTitle = "CD.DefaultContentPageTitle";
         static string CDMenuDefault = "{00000000-0000-0001-0000-000000000000}";
         static string CDMenuAdministration = "{00000000-0000-0002-0000-000000000000}";
         static string CDMenuWebGroup = "{00000000-0000-0003-0000-000000000000}";

         static string SISystemMail = "CD.SystemMail";
         static string SISysMailUser = "CD.SysMailUser";
         static string SISysMailPassword = "CD.SysMailPassword";
         static string SISysMailServer = "CD.SysMailServer";
         static string SINotifyMail = "CD.NotifyMail";

         static string SIIsPasswordHashed = "CD.IsPasswordHashed";

         static string SIGenericUserManage = "CD.GenericUserManage";
         static string SIIsAddLog = "CD.IsAddLog";
         static string SIIsAuditComment = "CD.IsAuditComment";

         static string SITemplateGroupBasePath = "TemplateGroupBasePath";
         static string SITemplateBasePath = "TemplateBasePath";
         static string SIEnableSiteSkins = "EnableSiteSkins";
         static string SISiteSkinsBasePath = "SiteSkinsBasePath";

         static string SIArticleUrlGenerator = "CD.ArticleUrlGenerator";
         static string SIEnableLoginAuhenCode = "EnableLoginAuhenCode";

         static string SIDescriptionPageMeta = "DescriptionPageMeta";
         static string SIKeywordPageMeta = "KeywordPageMeta";

         static string SICMSTheme = "CMSTheme";

         static string SISSOUrl = "SSO.Url";
         static string SISSOServername = "SSO.Servername";
         static string SISSOUsername = "SSO.Username";
         static string SISSOPassword = "SSO.Password";
         static string SISSODomain = "SSO.Domain";
         static string SISSOWdID = "SSO.WdID";
         static string SIDefaultCompanyRole = "CD.DefaultCompanyRole";
         static string SIDefaultPersonalRole = "CD.DefaultPersonalRole";
         static string SIArticleSourceDefault = "CD.ArticleSourceDefault";

         static string SIEnableCache = "CD.EnableCache";

         public static string SIADUrl = "AD.Url";

        #endregion

         public SiteSettingHelper()
        {
        }

        /// <summary>
        /// 当前的实例对象
        /// </summary>
        public static SiteSettingHelper Instance
        {
            get
            {
                HelperFactory helperFactory = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                SiteSettingHelper cdHelper = helperFactory.GetHelper<SiteSettingHelper>();
                return cdHelper;
            }
        }

        /// <summary>
        /// 禁止上传类型
        /// </summary>
        public static string[] forbidType
        {
            get
            {
                string fs = GeneralConfigs.GetConfig().Upload_Forbid;
                return (fs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }
        /// <summary>
        /// 允许图片类型
        /// </summary>
        public static string[] allowImageType
        {
            get
            {
                string fs =GeneralConfigs.GetConfig().Upload_AllowImageType  ;
                return (fs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// 是否存在指定模板
        /// </summary>
        /// <param name="fn">模板文件名</param>
        /// <returns></returns>
        public bool ExistTemplate(string fn)
        {
            return false;
        }

        /// <summary>
        /// 通用配置参数
        /// </summary>
        public GeneralConfigInfo Config
        {
            get
            {
                return GeneralConfigs.GetConfig();
            }
        }

        /// <summary>
        /// 站点配置参数
        /// </summary>
        public  SiteConfigInfo SiteConfig
        {
            get
            {
                return SiteConfigs.GetConfig();
            }
        }

        /// <summary>
        /// 站点参数从数据库迁移到config文件
        /// </summary>
        public bool MigrateConfig()
        {
            SiteConfigInfo sconfig = new SiteConfigInfo();
            GeneralConfigInfo gconfig = GeneralConfigs.GetConfig();
            if (gconfig == null)
            {
                HttpContext Context = HttpContext.Current;
                string configFile = Context.Server.MapPath("~/Config/general.config");
                gconfig = new GeneralConfigInfo();
                GeneralConfigs.Serialiaze(gconfig, configFile);
            }
            SystemInformation si = GetSystemInformation();

            sconfig.AdministratorID = si.AdministratorID;
            if( si.AdministratorKey!=null && si.AdministratorKey!="")
                sconfig.AdministratorKey = si.AdministratorKey;
            if(si.CompanyName !=null && si.CompanyName !="")
                sconfig.SiteName = si.CompanyName;
            sconfig.SiteDescription = si.CompangDescription;
            sconfig.InformationServiceUrl = si.InformationServiceUrl;
            gconfig.ProductName = si.ProductName;
            //gconfig.ProductVersion = si.ProdcutVersion;
            sconfig.WebGroupServiceUrl = si.WebGroupServiceUrl;
            sconfig.SiteID = si.SiteID;
            sconfig.RootUrl = si.RootUrl;
            sconfig.SsoUrl = si.SsoUrl;
            sconfig.SsoServername = si.SsoServername;
            sconfig.SsoUsername = si.SsoUsername;
            sconfig.SsoPassword = si.SsoPassword;
            sconfig.SsoDomain = si.SsoDomain;
            sconfig.SsoWdID = si.SsoWdID;
            sconfig.ADUrl = si.ADUrl;
            sconfig.IsPasswordHashed = si.IsPasswordHashed;

            gconfig.ArticleAutoPublish = si.ArticleAutoPublish;
            gconfig.ArticleAutoShare = si.ArticleAutoShare;
            gconfig.AllCutCheckBox = si.AllCutCheckBox;
            gconfig.AllowSignup = si.AllowSignup;
            gconfig.DefaultTemplateGroup = si.DefaultTemplateGroup;
            gconfig.DefaultTemplateGroupFileName = si.DefaultTemplateGroupFileName;
            gconfig.DefaultHomePageTitle = si.DefaultHomePageTitle;
            gconfig.DefaultChannelPageTitle = si.DefaultChannelPageTitle;
            gconfig.DefaultContentPageTitle = si.DefaultContentPageTitle;

            gconfig.SystemMail = si.SystemMail;
            gconfig.SysMailUser = si.SysMailUser;
            gconfig.SysMailServer = si.SysMailServer;
            gconfig.SysMailPassword = si.SysMailPassword;
            gconfig.NotifyMail = si.NotifyMail;
            gconfig.GenericUserManageType = si.GenericUserManageType;

            gconfig.IsAddLog = si.IsAddLog;
            gconfig.IsAuditComment = si.IsAuditComment;
            gconfig.EnableLoginAuhenCode = si.EnableLoginAuhenCode;
            gconfig.EnableSiteSkins = si.EnableSiteSkins;
            gconfig.TemplateBasePath = si.TemplateBasePath;
            gconfig.TemplateGroupBasePath = si.TemplateGroupBasePath;
            gconfig.SiteSkinsBasePath = si.SiteSkinsBasePath;
            gconfig.ArticleUrlGenerator = si.ArticleUrlGenerator;
            gconfig.KeywordPageMeta = si.KeywordPageMeta;
            gconfig.DescriptionPageMeta = si.DescriptionPageMeta;
            gconfig.CMSTheme = si.CMSTheme;
            gconfig.DefaultPersonRole = si.DefaultPersonRole;
            gconfig.DefaultCompanyRole = si.DefaultCompanyRole;
            gconfig.ArticleSourceDefault = si.ArticleSourceDefault;
            gconfig.EnableCache = si.EnableCache;

            return SiteConfigs.SaveConfig(sconfig) && GeneralConfigs.SaveConfig(gconfig);
        }

        /// <summary>
        /// 通过key获取站点配置的Value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string gettempsys(string key)
        {
            We7Helper.AssertNotNull(Assistant, "BaseHelper.Assistant");
            We7Helper.AssertNotNull(key, "BaseHelper.GetSystemParameter.key");

            Criteria c = new Criteria(CriteriaType.Equals, "ID", key);
            List<SiteSetting> sets = Assistant.List<SiteSetting>(c, null, 0, 1, new string[] { "ID", "Title", "Value", "SequenceIndex" });
            if (sets.Count > 0)
                return sets[0].Value;
            else
                return "";
        }

        /// <summary>
        /// 获取站点配置文件
        /// </summary>
        /// <returns></returns>
        public SystemInformation GetSystemInformation()
        {
            HttpContext context = HttpContext.Current;
            if (context.Application["CD.SIVAVUES"]==null)
            {
                SystemInformation si = new SystemInformation();

                si.AdministratorID = SIAdministratorID;
                si.AdministratorKey = gettempsys(SIAdministratorKey);
                si.CompanyName = gettempsys(SICompanyName);
                si.CompanyID = gettempsys(SICompanyID);
                si.CompangDescription = gettempsys(SICompanyDescription);
                si.InformationServiceUrl = gettempsys(SIIDUrl);
                si.ProductName = gettempsys(SIProduct);
                si.ProdcutVersion = gettempsys(SIVersion);
                si.WebGroupServiceUrl = gettempsys(SIWDUrl);
                si.SiteID = gettempsys(SISiteID);
                si.RootUrl = gettempsys(SIRootUrl);
                si.ArticleAutoPublish = gettempsys(SIArticleAutoPublish);
                si.ArticleAutoShare = gettempsys(SIArticleAutoShare);
                si.AllCutCheckBox = gettempsys(SIAllCutCheckBox);
                si.AllowSignup = gettempsys(SIAllowSignup);
                si.DefaultTemplateGroup = gettempsys(SIDefaultTemplateGroup);
                si.DefaultTemplateGroupFileName = gettempsys(SIDefaultTemplateGroupFileName);

                si.DefaultHomePageTitle = gettempsys(SIDefaultHomePageTitle);
                si.DefaultChannelPageTitle = gettempsys(SIDefaultChannelPageTitle);
                si.DefaultContentPageTitle = gettempsys(SIDefaultContentPageTitle);

                si.SystemMail = gettempsys(SISystemMail);
                si.SysMailUser = gettempsys(SISysMailUser);
                si.SysMailServer = gettempsys(SISysMailServer);
                si.SysMailPassword = gettempsys(SISysMailPassword);
                si.NotifyMail = gettempsys(SINotifyMail);
                si.GenericUserManageType = gettempsys(SIGenericUserManage);

                si.IsPasswordHashed = gettempsys(SIIsPasswordHashed) == "1";
                si.IsAddLog = gettempsys(SIIsAddLog) == "1";
                si.IsAuditComment = gettempsys(SIIsAuditComment) == "1";

                si.EnableLoginAuhenCode = gettempsys(SIEnableLoginAuhenCode);
                si.EnableSiteSkins = gettempsys(SIEnableSiteSkins);
                si.TemplateBasePath = gettempsys(SITemplateBasePath);
                si.TemplateGroupBasePath = gettempsys(SITemplateGroupBasePath);
                si.SiteSkinsBasePath = gettempsys(SISiteSkinsBasePath);
                si.ArticleUrlGenerator = gettempsys(SIArticleUrlGenerator);
                si.KeywordPageMeta = gettempsys(SIKeywordPageMeta);
                si.DescriptionPageMeta = gettempsys(SIDescriptionPageMeta);
                si.CMSTheme = gettempsys(SICMSTheme);

                si.SsoUrl = gettempsys(SISSOUrl);
                si.SsoServername = gettempsys(SISSOServername);
                si.SsoUsername = gettempsys(SISSOUsername);
                si.SsoPassword = gettempsys(SISSOPassword);
                si.SsoDomain = gettempsys(SISSODomain);
                si.SsoWdID = gettempsys(SISSOWdID);

                si.ADUrl = gettempsys(SIADUrl);
                si.DefaultPersonRole = gettempsys(SIDefaultPersonalRole);
                si.DefaultCompanyRole = gettempsys(SIDefaultCompanyRole);
                si.ArticleSourceDefault = gettempsys(SIArticleSourceDefault);

                si.EnableCache = gettempsys(SIEnableCache);

                context.Application["CD.SIVAVUES"] = si;
            }
            return (SystemInformation)context.Application["CD.SIVAVUES"];
        }

        /// <summary>
        /// 取得公司名称
        /// </summary>
        /// <returns></returns>
        public string GetCompanyName()
        {
            return SiteConfig.SiteName;
        }

        /// <summary>
        /// 取得站点ID
        /// </summary>
        /// <returns></returns>
        public string GetSiteID()
        {
            return SiteConfig.SiteID;
        }

        /// <summary>
        /// 取得默认主页标题
        /// </summary>
        /// <returns></returns>
        public string GetDefaultHomePageTitle()
        {
            return Config.DefaultHomePageTitle;
        }

        /// <summary>
        /// 取得默认内容页标题
        /// </summary>
        /// <returns></returns>
        public string GetDefaultContentPageTitle()
        {
            return Config.DefaultContentPageTitle;
        }

        /// <summary>
        /// 取得默认栏目页标题
        /// </summary>
        /// <returns></returns>
        public string GetDefaultChannelPageTitle()
        {
            return Config.DefaultChannelPageTitle;
        }

        /// <summary>
        /// 密码是否应用Hash编码
        /// </summary>
        /// <returns></returns>
        public bool GetPasswordIsHashed()
        {
            return SiteConfig.IsPasswordHashed;
        }

        /// <summary>
        /// 取得系统邮件信息
        /// </summary>
        /// <returns></returns>
        public string GetSystemMail()
        {
            return Config.NotifyMail;
        }

        /// <summary>
        /// 取得信息处理发送方式
        /// </summary>
        /// <returns></returns>
        public int GetGUMSParam()
        {
            int type = 0;
            try
            {
                type = int.Parse(Config.GenericUserManageType);
            }
            catch {}
            
            return(type);
        }

        /// <summary>
        /// 是否添加日志
        /// </summary>
        /// <returns></returns>
        public bool GetIsAddLog()
        {
            return Config.IsAddLog;
        }

        /// <summary>
        /// 验证登陆密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool AdminPasswordIsValid(string password)
        {
            if (SiteConfig.IsPasswordHashed)
            {                                
                password = Security.Encrypt(password);
            }
            string adminPass = SiteConfig.AdministratorKey;// GetSystemParameter("CD.AdministratorKey");
            return string.Compare(password, adminPass, false) == 0;
        }


        /// <summary>
        /// 当前文件是否能够上传
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public bool CanUpload(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            foreach (string f in allowImageType)
            {
                if (String.Compare(f, ext, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 当前附件是否能上传
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool CanUploadAttachment(string fileName)
        {
            string ext = Path.GetExtension(fileName);

            foreach (string f in forbidType)
            {
                if (String.Compare(f, ext, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 找回用户密码
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <param name="Mail">Email</param>
        /// <param name="AccountHelper">权限业务对象</param>
        /// <returns></returns>
        public string GetMyPassword(string loginName, string Mail, IAccountHelper AccountHelper)
        {

            if (String.Compare(loginName, SiteConfigs.GetConfig().AdministratorName, true) == 0)
            {
                if (Mail == GetSystemMail())
                {
                    Account ad = new Account();
                    ad.LastName = "管理员";
                    ad.Email = Mail;
                    ad.LoginName = SiteConfigs.GetConfig().AdministratorName;
                    ad.Password = SiteConfigs.GetConfig().AdministratorKey;                    
                    ad.IsPasswordHashed = GetPasswordIsHashed();
                    return SendPasswordByMail(ad,AccountHelper);
                }
                else
                {
                    return "对不起，您输入的邮箱不是管理员指定的系统邮件地址！";
                }
            }
            else
            {
                Account act = AccountHelper.GetAccountByLoginName(loginName);
                if (act == null)
                {
                    return "指定的用户不存在。";
                }

                else if (act.State != 1)
                {
                    return "该帐户不可用。";
                }
                else if (act.Email != Mail)
                {
                    return "对不起，您输入的邮箱不是您注册时填写的有效邮件地址！";
                }
                else
                {
                    return SendPasswordByMail(act,AccountHelper);
                }
            }
        }

        /// <summary>
        /// 通过Email发送密码信息
        /// </summary>
        /// <param name="account">权限信息</param>
        /// <param name="AccountHelper">权限业务对象</param>
        /// <returns></returns>
        public string SendPasswordByMail(Account account, IAccountHelper AccountHelper)
        {
            GeneralConfigInfo si = Config;
            if (si.SystemMail == "" || si.SysMailUser == "" || si.SysMailServer == "")
                return "系统邮件信息不全，请到网站菜单“系统设置”中设置相关参数。" ;
            else
            {
                MailHelper mailHelper = new MailHelper();
                mailHelper.AdminEmail = si.SystemMail;
                mailHelper.UserName = si.SysMailUser;
                mailHelper.Password = si.SysMailPassword;
                mailHelper.SmtpServer = si.SysMailServer;

                string password = null;
                if (account.IsPasswordHashed)
                {
                    password = Security.ResetPassword(account);

                    string secuPassword = "";
                    if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt)
                    {
                        secuPassword = Security.Encrypt(password);
                    }
                    else if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.bbsEncrypt)
                    {
                        secuPassword = Security.BbsEncrypt(password);
                    }
                    if (String.Compare(account.LoginName, SiteConfigs.GetConfig().AdministratorName, true) == 0)
                    {
                        //UpdateSystemParameter("CD.AdministratorKey", secuPassword);
                        SiteConfig.AdministratorKey = secuPassword;
                        SiteConfigs.SaveConfig(SiteConfig);
                    }
                    else
                    {
                        AccountHelper.UpdatePassword(account, secuPassword);
                    }

                }
                else
                {
                    password = account.Password;
                }

                string message = "亲爱的{0}，您好：\r\n您的登录帐户:\r\n用户名: {3}\r\n密码: {1}\r\n\r\n{5}-{2}\r\n网站管理员\r\n{4}";

                string mainurl = SiteConfig.RootUrl;
                string To = account.Email;
                string From = string.Format("\"{1}系统邮件\" <{0}>", si.SystemMail, SiteConfig.SiteName);
                string Subject = "您的网站登录信息";
                string Body = string.Format(message, account.LastName, password, mainurl, account.LoginName, DateTime.Now.ToLongDateString(), SiteConfig.SiteName);
                mailHelper.Send(To, From, Subject, Body,"");
                //Message.Text = "Login Credentials Sent<br>";
                return "新密码已发至你的邮箱";
            }
        }



        #region 静态URL
        /// <summary>
        /// 从url获取非id型的值
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetSearcherKeyFromUrl(string path)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si == null) return "";
            string ext = si.UrlFormat;
            if (ext == null || ext.Length == 0) ext = "html";

            string mathstr = @"/(\w|\s|(-)|(_))+\." + ext + "$";
            if (path.ToLower().EndsWith("default." + ext))
                path = path.Remove(path.Length - 12);
            if (path.ToLower().EndsWith("index." + ext))
                path = path.Remove(path.Length - 10);

            if (Regex.IsMatch(path, mathstr))
            {
                int lastSlash = path.LastIndexOf("/");
                if (lastSlash > -1)
                {
                    path = path.Remove(0, lastSlash + 1);
                }

                int lastDot = path.LastIndexOf(".");
                if (lastDot > -1)
                {
                    path = path.Remove(lastDot, path.Length - lastDot);
                }

                return path;
            }
            else
                return string.Empty;

        }
        #endregion
        /// <summary>
        /// 增加一条操作记录
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="content"></param>
        public void AddLog(string pages, string content)
        {
            HelperFactory HelperFactory = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
            LogHelper LogHelper = HelperFactory.GetHelper<LogHelper>();

            if (Config.IsAddLog)
            {
                LogHelper.WriteLog(Security.CurrentAccountID, pages, content, Config.DefaultHomePageTitle);
            }
        }
    }
}
