using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Reflection;
using We7.Framework.Util;

namespace We7.Framework.Config
{
    /// <summary>
    /// 网站基本设置描述类, 加[Serializable]标记为可序列化
    /// </summary>
    [Serializable]
    public class GeneralConfigInfo : IConfigInfo
    {
        private static FileVersionInfo AssemblyFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        string productName = "We7";
        static string productVersion = "V" + AssemblyFileVersion.ProductVersion + " 正式版";
        string articleAutoPublish;
        string defaultTemplateGroup;
        string defaultTemplateGroupFileName;
        string defaultChannelPageTitle = "{$ChannelName} - We7站点";
        string defaultHomePageTitle = "首页 - We7站点";
        string defaultContentPageTitle = "{$ArticleTitle}{$ChannelName} - We7站点";
        bool isAddLog;
        bool isAuditComment;
        string allowSignup;
        string allCutCheckBox;
        string systemMail;
        string sysMailUser;
        string sysMailPassword;
        string sysMailServer;
        string notifyMail;
        string emailGarbageKey;
        string genericUserManageType;
        string templateGroupBasePath;
        string templateBasePath;
        string enableSiteSkins = "true";
        string siteSkinsBasePath = "_skins";

        string articleUrlGenerator;
        string enableLoginAuhenCode;

        string keywordPageMeta = "开源CMS,asp.net CMS,可扩展站群,内容模型,表单自定义";
        string descriptionPageMeta = "基于.net技术的功能强大、操作简单、开放源码的新一代企业级CMS内容管理系统。";
        string cMSTheme;
        string defaultCompanyRole;
        string defaultPersonRole;

        string sysPopServer;
        string articleAutoShare;
        private string siteTitle = "We7";

        private string siteFullName = "西部动力（北京）科技有限公司";//所属公司全名(2010-07-27添加)

        private string pluginDirectory;
        private string pluginService; //插件服务地址
        private string pluginServer = "http://plugins.we7.cn";//下载插件的目录
        private string pluginDownloadDirectory;
        private string pluginGallery; //插件的目录，这是服务器端用的。
        private string urlFormat = "html";
        private string urlRewriterProvider = "asp.net";
        private bool singletonSite = true;
        string shopService = "http://m.we7.cn";

        bool startTemplateMap = true;
        string siteBuildState = "edit";
        bool isOEM = false;
        bool startPageViewModule = false;
        private string bbsDomain = "";
        private string bbsPwdKey = "";
        private bool allowParentArticle = false;

        public const bool SingletonSite = true;

        private string jiaMiKey = "xbdongli";

        private string userRegisterMode = "none";

        private string _siteLogo = "";

        private string defaultTemplateEditor = "0";

        private bool enableCookieAuthentication = true;

        private string _SSOSiteUrls = string.Empty;
        private bool enableHtmlTemplate = true;
        private string theme = "theme";


        /// <summary>
        /// 是否启用Html静态化的模板
        /// </summary>
        public bool EnableHtmlTemplate
        {
            get { return enableHtmlTemplate; }
            set { enableHtmlTemplate = value; }
        }

        /// <summary>
        /// 允许Cookie验证
        /// </summary>
        public bool EnableCookieAuthentication
        {
            get { return enableCookieAuthentication; }
            set { enableCookieAuthentication = value; }
        }

        /// <summary>
        /// 单点登陆Url
        /// </summary>
        public string SSOSiteUrls
        {
            get { return _SSOSiteUrls; }
            set { _SSOSiteUrls = value; }
        }

        /// <summary>
        /// 站点logo
        /// </summary>
        public string SiteLogo
        {
            get { return _siteLogo; }
            set { _siteLogo = value; }
        }
        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        /// <summary>
        /// 产品版本
        /// </summary>
        public string ProductVersion
        {
            get { return productVersion; }
        }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string SiteTitle
        {
            get { return siteTitle; }
            set { siteTitle = value; }
        }
        /// <summary>
        /// 网站所属公司全名
        /// </summary>
        public string SiteFullName
        {
            get { return siteFullName; }
            set { siteFullName = value; }
        }
        private string icpInfo = "";
        /// <summary>
        /// 网站备案信息
        /// </summary>
        public string IcpInfo
        {
            get { return icpInfo; }
            set { icpInfo = value; }
        }

        private string rewriteUrl = "";
        /// <summary>
        /// 伪静态url的替换规则
        /// </summary>
        public string RewriteUrl
        {
            get { return rewriteUrl; }
            set { rewriteUrl = value; }
        }
        private string urlExtName = ".aspx";
        /// <summary>
        /// 伪静态url的扩展名:".aspx"/".html"
        /// </summary>
        public string UrlExtName
        {
            get { return urlExtName; }
            set { urlExtName = value; }
        }

        private int postInterval = 0;
        /// <summary>
        /// 发帖灌水预防(秒)
        /// </summary>
        public int PostInterval
        {
            get { return postInterval; }
            set { postInterval = value; }
        }
        private int waterMarkStatus = 3;
        /// <summary>
        /// 图片附件添加水印 
        /// 0=不使用 1=左上 2=中上 3=右上 4=左中 ... 9=右下
        /// </summary>
        public int WaterMarkStatus
        {
            get { return waterMarkStatus; }
            set { waterMarkStatus = value; }
        }
        private int waterMarkType = 0;
        /// <summary>
        /// 图片附件添加何种水印 0=文字 1=图片
        /// </summary>
        public int WaterMarkType
        {
            get { return waterMarkType; }
            set { waterMarkType = value; }
        }
        private int waterMarkTransparency = 5;
        /// <summary>
        /// 图片水印透明度 取值范围1--10 (10为不透明)
        /// </summary>
        public int WaterMarkTransparency
        {
            get { return waterMarkTransparency; }
            set { waterMarkTransparency = value; }
        }
        private string waterMarkText = "We7.cn";
        /// <summary>
        /// 图片附件添加文字水印的内容 
        /// {1}表示网站标题 {2}表示网站地址 {3}表示当前日期 {4}表示当前时间, 
        /// 例如: {3} {4}上传于{1} {2}
        /// </summary>
        public string WaterMarkText
        {
            get { return waterMarkText; }
            set { waterMarkText = value; }
        }

        private string waterMarkPicfile;
        /// <summary>
        /// 图片水印文件
        /// </summary>
        public string WaterMarkPicfile
        {
            get { return waterMarkPicfile; }
            set { waterMarkPicfile = value; }
        }

        private string waterMarkPic = "watermark.gif";
        /// <summary>
        /// 使用的水印图片的名称
        /// </summary>
        public string WaterMarkPic
        {
            get { return waterMarkPic; }
            set { waterMarkPic = value; }
        }

        private string waterMarkFontName = "Tahoma";
        /// <summary>
        /// 图片附件添加文字水印的字体
        /// </summary>
        public string WaterMarkFontName
        {
            get { return waterMarkFontName; }
            set { waterMarkFontName = value; }
        }
        private int waterMarkFontSize = 12;
        /// <summary>
        /// 图片添加文字水印的大小(像素)
        /// </summary>
        public int WaterMarkFontSize
        {
            get { return waterMarkFontSize; }
            set { waterMarkFontSize = value; }
        }

        int attachImageQuality = 80;
        /// <summary>
        /// 图片压缩质量
        /// </summary>
        public int AttachImageQuality
        {
            get { return attachImageQuality; }
            set { attachImageQuality = value; }
        }

        int maxWidthOfUploadedImg = 800;
        /// <summary>
        /// 上传图片的最大宽度，单位：像素
        /// </summary>
        public int MaxWidthOfUploadedImg
        {
            get { return maxWidthOfUploadedImg; }
            set { maxWidthOfUploadedImg = value; }
        }

        /// <summary>
        /// 文章自动同步
        /// </summary>
        public string ArticleAutoShare
        {
            get { return articleAutoShare; }
            set { articleAutoShare = value; }
        }

        int cutToMaxWidthOfUploadedImg = 0;
        /// <summary>
        /// 是否自动缩小上传图片到最大限定尺寸
        /// </summary>
        public int CutToMaxWidthOfUploadedImg
        {
            get { return cutToMaxWidthOfUploadedImg; }
            set { cutToMaxWidthOfUploadedImg = value; }
        }
        int overdueDateTime = 1000;
        /// <summary>
        /// 设置文章，产品，展会等的过期时间
        /// </summary>
        public int OverdueDateTime
        {
            get { return overdueDateTime; }
            set { overdueDateTime = value; }
        }

        string articlePhotoSize = "50*50";
        /// <summary>
        /// 设置文章内小图片尺寸
        /// </summary>
        public string ArticlePhotoSize
        {
            get { return articlePhotoSize; }
            set { articlePhotoSize = value; }
        }

        int aDVisbleToSite = 0;
        /// <summary>
        /// 区分站群总站和分站，0为分站广告过滤，1为总站，广告不过滤
        /// </summary>
        public int ADVisbleToSite
        {
            get { return aDVisbleToSite; }
            set { aDVisbleToSite = value; }
        }

        /// <summary>
        /// 默认模板组文件名
        /// </summary>
        public string DefaultTemplateGroupFileName
        {
            get { return defaultTemplateGroupFileName; }
            set { defaultTemplateGroupFileName = value; }
        }

        /// <summary>
        /// 默认模板组
        /// </summary>
        public string DefaultTemplateGroup
        {
            get { return defaultTemplateGroup; }
            set { defaultTemplateGroup = value; }
        }

        /// <summary>
        /// 默认首页标题
        /// </summary>
        public string DefaultHomePageTitle
        {
            get { return defaultHomePageTitle; }
            set { defaultHomePageTitle = value; }
        }

        /// <summary>
        /// 默认频道页标题
        /// </summary>
        public string DefaultChannelPageTitle
        {
            get { return defaultChannelPageTitle; }
            set { defaultChannelPageTitle = value; }
        }

        /// <summary>
        /// 默认内容页标题
        /// </summary>
        public string DefaultContentPageTitle
        {
            get { return defaultContentPageTitle; }
            set { defaultContentPageTitle = value; }
        }

        /// <summary>
        /// 文章自动发布
        /// </summary>
        public string ArticleAutoPublish
        {
            get { return articleAutoPublish; }
            set { articleAutoPublish = value; }
        }

        /// <summary>
        /// 允许申请注册
        /// </summary>
        public string AllowSignup
        {
            get { return allowSignup; }
            set { allowSignup = value; }
        }
        /// <summary>
        /// 允许用户裁剪
        /// </summary>
        public string AllCutCheckBox
        {
            get { return allCutCheckBox; }
            set { allCutCheckBox = value; }
        }
        /// <summary>
        /// 系统邮件
        /// </summary>
        public string SystemMail
        {
            get { return systemMail; }
            set { systemMail = value; }
        }

        /// <summary>
        /// 邮件用户
        /// </summary>
        public string SysMailUser
        {
            get { return sysMailUser; }
            set { sysMailUser = value; }
        }

        /// <summary>
        /// 邮件密码
        /// </summary>
        public string SysMailPassword
        {
            get { return sysMailPassword; }
            set { sysMailPassword = value; }
        }
        /// <summary>
        /// 系统邮件服务器
        /// </summary>
        public string SysMailServer
        {
            get { return sysMailServer; }
            set { sysMailServer = value; }
        }

        /// <summary>
        /// 邮件通知地址
        /// </summary>
        public string NotifyMail
        {
            get { return notifyMail; }
            set { notifyMail = value; }
        }

        /// <summary>
        /// 垃圾邮件关键字
        /// </summary>
        public string EmailGarbageKey
        {
            get { return emailGarbageKey; }
            set { emailGarbageKey = value; }
        }

        /// <summary>
        /// 通用用户管理类型
        /// </summary>
        public string GenericUserManageType
        {
            get { return genericUserManageType; }
            set { genericUserManageType = value; }
        }

        /// <summary>
        /// 是否开启日志记录功能
        /// </summary>
        public bool IsAddLog
        {
            get { return isAddLog; }
            set { isAddLog = value; }
        }
        /// <summary>
        /// 评论是否审核后发布
        /// </summary>
        public bool IsAuditComment
        {
            get { return isAuditComment; }
            set { isAuditComment = value; }
        }

        /// <summary>
        /// 登录是否验证
        /// </summary>
        public string EnableLoginAuhenCode
        {
            get { return enableLoginAuhenCode; }
            set { enableLoginAuhenCode = value; }
        }

        /// <summary>
        /// 文章URL样式
        /// </summary>
        public string ArticleUrlGenerator
        {
            get { return articleUrlGenerator; }
            set { articleUrlGenerator = value; }
        }

        /// <summary>
        /// 站点皮肤根目录
        /// </summary>
        public string SiteSkinsBasePath
        {
            get { return siteSkinsBasePath; }
            set { siteSkinsBasePath = value; }
        }

        /// <summary>
        /// 启用皮肤功能文件夹
        /// </summary>
        public string EnableSiteSkins
        {
            get { return enableSiteSkins; }
            set { enableSiteSkins = value; }
        }

        /// <summary>
        /// 模板根目录
        /// </summary>
        public string TemplateBasePath
        {
            get { return templateBasePath; }
            set { templateBasePath = value; }
        }

        /// <summary>
        /// 模板组根目录
        /// </summary>
        public string TemplateGroupBasePath
        {
            get { return templateGroupBasePath; }
            set { templateGroupBasePath = value; }
        }
        /// <summary>
        ///  页面描述(description)
        /// </summary>
        public string DescriptionPageMeta
        {
            get { return descriptionPageMeta; }
            set { descriptionPageMeta = value; }
        }

        /// <summary>
        /// 页面关键词(keyword)
        /// </summary>
        public string KeywordPageMeta
        {
            get { return keywordPageMeta; }
            set { keywordPageMeta = value; }
        }

        /// <summary>
        /// 是否单站点广告模式
        /// </summary>
        public bool AdMode_SingletonSite
        {
            get { return singletonSite; }
            set { singletonSite = value; }
        }

        /// <summary>
        /// 后台风格主题
        /// </summary>
        public string CMSTheme
        {
            get { return cMSTheme; }
            set { cMSTheme = value; }
        }
        private string articleSourceDefault;
        /// <summary>
        /// 文章来源默认值
        /// </summary>
        public string ArticleSourceDefault
        {
            get { return articleSourceDefault; }
            set { articleSourceDefault = value; }
        }

        /// <summary>
        /// 设置企业会员默认角色
        /// </summary>
        public string DefaultCompanyRole
        {
            get { return defaultCompanyRole; }
            set { defaultCompanyRole = value; }
        }
        /// <summary>
        /// 设置个人会员默认角色
        /// </summary>
        public string DefaultPersonRole
        {
            get { return defaultPersonRole; }
            set { defaultPersonRole = value; }
        }

        private string enableCache;
        /// <summary>
        /// 是否开启缓存
        /// </summary>
        public string EnableCache
        {
            get { return enableCache; }
            set { enableCache = value; }
        }

        bool ipSecurityEnabled = false;
        /// <summary>
        /// 是否开启IP安全策略
        /// </summary>
        public bool IpSecurityEnabled
        {
            get { return ipSecurityEnabled; }
            set { ipSecurityEnabled = value; }
        }

        int ipSecurityDefaultValue = 0;
        /// <summary>
        /// IP安全策略栏目默认值
        /// </summary>
        public int IpSecurityDefaultValue
        {
            get { return ipSecurityDefaultValue; }
            set { ipSecurityDefaultValue = value; }
        }


        private Int16 cacheTimeSpan = 60;
        /// <summary>
        /// 缓存更新时间间隔（秒）
        /// </summary>
        public Int16 CacheTimeSpan
        {
            get { return cacheTimeSpan; }
            set { cacheTimeSpan = value; }
        }

        /// <summary>
        /// 收邮件服务
        /// </summary>
        public string SysPopServer
        {
            get { return sysPopServer; }
            set { sysPopServer = value; }
        }

        string copyright = "Powered by <a href=\"http://we7.cn/\" target=\"_blank\">We7</a> " + productVersion + " ©2011 <a href=\"http://www.westengine.com/\" target=\"_blank\">WestEngine Inc.</a> </span>";
        /// <summary>
        /// 版权申明
        /// </summary>
        public string Copyright
        {
            get { return copyright; }
            set { copyright = value; }
        }
        /// <summary>
        /// We7固定版权申明
        /// </summary>
        public string CopyrightOfWe7
        {
            get
            {
                return "Powered by <a href=\"http://we7.cn/\" target=\"_blank\">We7</a> " + productVersion + " ©2011 <a href=\"http://www.westengine.com/\" target=\"_blank\">WestEngine Inc.</a> </span>";
            }
        }
        string links = "网站简介 - 广告服务 - <a href=\"SiteMap.aspx\">网站地图</a> - <a target=\"_blank\" href=\"/Admin/Signin.aspx\">后台管理</a> - 联系方式";
        /// <summary>
        /// 相关链接（IsOem为true时启用）
        /// </summary>
        public string Links
        {
            get { return links; }
            set { links = value; }
        }

        /// <summary>
        /// We7
        /// </summary>
        public string LinksOfWe7
        {
            get
            {
                return " | <a href=\"http://help.we7.cn/\" target=\"_blank\">帮助文档</a> | <a href=\"http://qa.we7.cn\" target=\"_blank\">问答中心</a> | <a href=\"http://bbs.we7.cn\" target=\"_blank\">交流</a>";
            }
        }

        bool onlyLoginUserCanVisit = false;
        /// <summary>
        /// 仅允许登录用户访问网站
        /// </summary>
        public bool OnlyLoginUserCanVisit
        {
            get { return onlyLoginUserCanVisit; }
            set { onlyLoginUserCanVisit = value; }
        }

        /// <summary>
        /// 插件服务器地址
        /// </summary>
        public string PluginServer
        {
            get { return pluginServer; }
            set { pluginServer = value; }
        }
        /// <summary>
        /// url静态文件后缀：html？ashx？
        /// </summary>
        public string UrlFormat
        {
            get { return urlFormat; }
            set { urlFormat = value; }
        }

        /// <summary>
        /// URL重写级别：IIS？Asp.net？
        /// </summary>
        public string UrlRewriterProvider
        {
            get { return urlRewriterProvider; }
            set { urlRewriterProvider = value; }
        }

        /// <summary>
        /// IP过滤策略
        /// </summary>
        public string IPStrategy { get; set; }

        /// <summary>
        /// 启用模版地图
        /// </summary>
        public bool StartTemplateMap
        {
            get { return startTemplateMap; }
            set { startTemplateMap = value; }
        }

        /// <summary>
        /// 网站状态：debug-控件调试，edit-网站建设，run-网站运行
        /// </summary>
        public string SiteBuildState
        {
            get { return siteBuildState; }
            set { siteBuildState = value; }
        }

        /// <summary>
        /// 是否为OEM版本，开启此开关，配置文件中的版权信息才会起作用
        /// </summary>
        public bool IsOEM
        {
            get { return isOEM; }
            set { isOEM = value; }
        }

        string upload_AllowImageType = ".gif;.jpg;.bmp;.png;.jpeg;.jpe;.swf;";
        /// <summary>
        /// 文件上传允许扩展名
        /// </summary>
        public string Upload_AllowImageType
        {
            get { return upload_AllowImageType; }
            set { upload_AllowImageType = value; }
        }
        string upload_Forbid = ".exe;.bat;.pif;.com;.aspx;.asp;.asmx;.ascx;";

        /// <summary>
        /// 文件上传禁止扩展名
        /// </summary>
        public string Upload_Forbid
        {
            get { return upload_Forbid; }
            set { upload_Forbid = value; }
        }

        string iPDBConnection = "New=False;Compress=True;Synchronous=Off;UTF8Encoding=True;Version=3;Data Source={$Current}\\IP.db";
        /// <summary>
        /// IP地址数据库连接串
        /// </summary>
        public string IPDBConnection
        {
            get { return iPDBConnection; }
            set { iPDBConnection = value; }
        }

        /// <summary>
        /// 是否启用流量统计功能
        /// </summary>
        public bool StartPageViewModule
        {
            get { return startPageViewModule; }
            set { startPageViewModule = value; }
        }

        /// <summary>
        /// bbs网站domain
        /// </summary>
        public string BbsDomain
        {
            get { return bbsDomain; }
            set { bbsDomain = value; }
        }


        /// <summary>
        /// bbs网站密码key
        /// </summary>
        public string BbsPwdKey
        {
            get { return bbsPwdKey; }
            set { bbsPwdKey = value; }
        }
        /// <summary>
        /// 是否显示所有信息
        /// </summary>
        public bool ShowAllInfo { get; set; }

        /// <summary>
        /// 允许文章具备父子关系
        /// </summary>
        public bool AllowParentArticle
        {
            get { return allowParentArticle; }
            set { allowParentArticle = value; }
        }

        /// <summary>
        /// <!--网站字符串加密，加密密钥，请设置8位字符串-->
        /// </summary>
        public string JiaMiKey
        {
            get { return jiaMiKey; }
            set { jiaMiKey = value; }
        }

        /// <summary>
        /// 是否启用单表存储
        /// </summary>
        public bool EnableSingleTable { get; set; }

        /// <summary>
        /// 插件商店服务地址
        /// </summary>
        public string ShopService
        {
            get { return shopService; }
            set { shopService = value; }
        }

        /// <summary>
        /// 用户注册验证模式；
        /// none-无；email-邮箱验证; manual-人工审核
        /// </summary>
        public string UserRegisterMode
        {
            get { return userRegisterMode; }
            set { userRegisterMode = value; }
        }

        /// <summary>
        /// 默认模板编辑器文件；
        /// 默认为0：/admin/DataControlUI/Compose.aspx
        /// 1为可拖拽编辑器：/admin/Template/TemplateFileDetail.aspx
        /// </summary>
        public string DefaultTemplateEditor
        {
            get { return defaultTemplateEditor; }
            set { defaultTemplateEditor = value; }
        }

        /// <summary>
        /// 可视化的主题样式
        /// </summary>
        public string Theme
        {
            get
            {
                if (String.IsNullOrEmpty(theme))
                {
                    string path = HttpContext.Current.Server.MapPath("~/Widgets/WidgetsIndex.xml");
                    if (File.Exists(path))
                    {
                        XmlNodeList nodes = XmlHelper.GetXmlNodeList(path, "item");
                        if (nodes != null && nodes.Count > 0)
                        {
                            theme = ((XmlElement)nodes[0]).GetAttribute("name");
                        }
                    }
                }
                return theme;
            }
            set { theme = value; }
        }
        /// <summary>
        /// 是否是演示站点
        /// </summary>
        public bool IsDemoSite { get; set; }
    }
}