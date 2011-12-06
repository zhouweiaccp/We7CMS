using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

using We7.CMS.Config;
using We7.Framework.Config;

namespace We7.CMS
{
    /// <summary>
    /// 常量信息
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// 数据存放目录，默认/_Data
        /// </summary>
        public static string DataUrlPath = "/_Data";
        /// <summary>
        /// 2.5版以前控件存放的目录
        /// </summary>
        [Obsolete]
        public static string ControlBasePath = "cgi-bin\\templates\\controls";

       /// <summary>
       /// 2.5版以前控件的Url路径
       /// </summary>
        [Obsolete]
        public static string ControlUrlPath
        {
            get
            {
                string temp = ControlBasePath.Replace("\\", "/");
                if (!temp.StartsWith("/")) temp = "/" + temp;
                return temp;
            }
        }

        /// <summary>
        /// We7控件根目录的目录名
        /// </summary>
        public const string We7ControlsBasePath = "We7Controls";

        /// <summary>
        /// We7插件的根目录目录名
        /// </summary>
        public const string We7PluginBasePath = "Plugins";

        /// <summary>
        /// 模型文件根目录目录名
        /// </summary>
        public const string We7ModelBasePath = "Models";

        /// <summary>
        /// 部件索引的根目录
        /// </summary>
        public const string We7WidgetsFolder = "Widgets";

        /// <summary>
        /// 部件的根目录
        /// </summary>
        public const string We7WidgetCollectionFolder = "Widgets\\WidgetCollection";

        /// <summary>
        /// 部件主题的根目录
        /// </summary>
        public const string We7ThemeFolder = "Widgets\\Themes";

        /// <summary>
        /// 静态部件的目录
        /// </summary>
        public const string We7HtmlWidgetFolder = "Widgets\\WidgetCollection\\静态类";

        /// <summary>
        /// We7控件的配置文件名,带扩展名
        /// </summary>
        public const string We7ControlConfigFile = "DataControl.xml";

        /// <summary>
        /// We7Widget的配置文件名,带扩展名
        /// </summary>
        public const string We7Widget = "Widget.xml";

         /// <summary>
        /// We7控件根目录的物理路径
        /// </summary>
        public readonly static string We7ControlPhysicalPath;
        /// <summary>
        /// 插件的物理目录
        /// </summary>
        public readonly static string We7PluginPhysicalPath;




        /// <summary>
        /// 插件的模型物理目录
        /// </summary>
        public readonly static string We7ModelPhysicalPath;

        public readonly static string We7WidgetsPhysicalFolder;
        /// <summary>
        /// 控件文件位置
        /// </summary>
        public readonly static string We7WidgetsFileFolder;

        /// <summary>
        /// 静态部件文件位置
        /// </summary>
        public readonly static string We7HtmlWidgetsFileFolder;

        /// <summary>
        /// 部件主题文件位置
        /// </summary>
        public readonly static string We7ThemeFileFolder;

        static Constants()
        {
            We7ControlPhysicalPath= Path.Combine(AppDomain.CurrentDomain.BaseDirectory,We7ControlsBasePath);
            We7PluginPhysicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7PluginBasePath);
            We7ModelPhysicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7ModelBasePath);
            We7WidgetsPhysicalFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7WidgetsFolder);
            We7WidgetsFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7WidgetCollectionFolder);
            We7HtmlWidgetsFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7HtmlWidgetFolder);
            We7ThemeFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7ThemeFolder);
        }

        /// <summary>
        ///  启用的站点皮肤
        /// </summary>
        public static bool EnableSiteSkins
        {
            get
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                string _default = si.EnableSiteSkins;
                if (_default != null && _default.ToLower() == "true")
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 站点皮脚基本路径
        /// </summary>
        public static string SiteSkinsBasePath
        {
            get
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                string _default = si.SiteSkinsBasePath;
                if (_default==null || _default == string.Empty)
                    _default = "_skins";
                return _default;
            }
        }

        /// <summary>
        /// 模板组基本路径
        /// </summary>
        public static string TemplateGroupBasePath 
        {
            get
            {
                    return SiteSkinsBasePath;
            }
        }

        /// <summary>
        /// 模板路径
        /// </summary>
        public static string TemplateBasePath
        {
            get
            {
                if (EnableSiteSkins)
                {
                    return SiteSkinsBasePath;
                }
                else
                {
                    GeneralConfigInfo si = GeneralConfigs.GetConfig();
                    string _default = si.TemplateBasePath;
                    if (_default == null || _default == string.Empty)
                        _default = "cgi-bin\\templates";
                    return _default;
                }
            }
        }

        /// <summary>
        /// 模板的Url路径
        /// </summary>
        public static string TemplateUrlPath
        {
            get
            {
                string temp = TemplateBasePath.Replace("\\", "/");
                if (!temp.StartsWith("/")) temp = "/" + temp;

                if (EnableSiteSkins)
                {
                    //HelperFactory helperFactory = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                    //CDHelper CDHelper = helperFactory.GetHelper<CDHelper>();
                    GeneralConfigInfo si = GeneralConfigs.GetConfig();
                    string _default = Path.GetFileNameWithoutExtension(si.DefaultTemplateGroupFileName);
                    temp = String.Format("{0}/{1}", temp, _default);
                }
                
                return temp;
            }
        }

        /// <summary>
        /// 本地模板路径
        /// </summary>
        public static string TemplateLocalPath
        {
            get
            {
                string temp = TemplateBasePath;
                if (!temp.StartsWith("\\")) temp = "\\" + temp;
                return temp;
            }
        }

        /// <summary>
        /// 临时文件目录
        /// </summary>
        public static string TempBasePath = "\\_temp";
        //public static string TemporaryPath = "Temp";
       
        //此代码已移到We7.CMS.Common下Channel.cs中
        public static string ChannelPath = "_data\\Channels";
        //此代码已移到We7.CMS.Common下Channel.cs中
        public static string ChannelUrlPath
        {
            get
            {
                string temp = ChannelPath.Replace("\\", "/");
                if (!temp.StartsWith("/")) temp = "/" + temp;
                //temp = "~" + temp;
                return temp;
            }
        }

        /// <summary>
        /// 附件路径，如 /_data/2010/02/25/
        /// </summary>
        public static string AttachmentUrlPath
        {
            get
            {
                string year=DateTime.Today.ToString("yyyy");
                string month=DateTime.Today.ToString("MM");
                string day=DateTime.Today.ToString("dd");
                return string.Format("/_data/{0}/{1}/{2}/", year, month, day);
            }
        }

        /// <summary>
        /// 别名目录
        /// </summary>
        public static string AliasPath = "Config\\Dictionary";

        /// <summary>
        /// 标签目录
        /// </summary>
        public static string TagsPath = "Config\\Dictionary";
        
        /// <summary>
        /// 关键字目录
        /// </summary>
        public static string KeywordsPath = "Config\\Dictionary";

        /// <summary>
        /// 控件配置文件后辍
        /// </summary>
        public static string ControlFileExtension = ".ascx.xml";

        /// <summary>
        /// 子模型配置文件后辍
        /// </summary>
        public static string TemplateFileExtension = ".xml";

        /// <summary>
        /// 模板组配置文件后辍
        /// </summary>
        public static string TemplateGroupFileExtension = ".xml";

        /// <summary>
        /// 默认访问者
        /// </summary>
        public static int OwnerAccount = 0;

        /// <summary>
        /// 默认角色
        /// </summary>
        public static int OwnerRole = 1;

        /// <summary>
        /// 标题最大长度
        /// </summary>
        public static int TitleMaxWord =20;

        /// <summary>
        /// 模板版本信息文件路径
        /// </summary>
        public static string TemplateVersionFileName = "TemplateVersion.config";

        /// <summary>
        /// 主题目录
        /// </summary>
        public static string ThemePath = "theme";

        /// <summary>
        /// 商业信息目录
        /// </summary>
        public static string IndustryAttrXmlPath = "_data\\IndustrAttrXml";

        /// <summary>
        /// 内容模型目录
        /// </summary>
        public static string ContentModelXmlPath = "Config\\c-model";

        /// <summary>
        /// 导入数据目录
        /// </summary>
        public static string Import3rdDataPath = "_data\\Import3rdData";

        /// <summary>
        /// 可视化设计模板目录虚拟路径
        /// </summary>
        public static string VisualTemplateTemplateVirtualDirectory = "~/Admin/VisualTemplate/Templates/";
        /// <summary>
        /// 可视化设计模板路径物理目录路径
        /// </summary>
        public static string VisualTemplatePhysicalTemplateDirectory
        {
            get
            {

                return HttpContext.Current.Server.MapPath(VisualTemplateTemplateVirtualDirectory);
            }
        }

        /// <summary>
        /// 包装器路径
        /// </summary>
        public static string WidgetsWrapperFolder = "~/Widgets/Wrapper/";
        /// <summary>
        /// 包装器物理目录路径
        /// </summary>
        public static string WidgetsWrapperFolderPhysicalDirectory
        {
            get
            {
                return HttpContext.Current.Server.MapPath(WidgetsWrapperFolder);
            }
        }
        /// <summary>
        /// 文章类内容模型名称
        /// </summary>
        public const string ArticleModelName = "System.Article";
        /// <summary>
        /// 所有内容模型
        /// </summary>
        public const string AllInfomationModelName = "We7_Article_AllInfomation";

    }

    /// <summary>
    /// 关键字
    /// </summary>
    //public sealed class Keys
    //{
    //    private Keys() { }
    //    /// <summary>
    //    /// 页码关键字
    //    /// </summary>
    //    public const string QRYSTR_PAGEINDEX = "pg";

    //    /// <summary>
    //    /// Session关键字
    //    /// </summary>
    //    internal const string SESSION_COOKIETEST = "CookieTest";
    //}
}
