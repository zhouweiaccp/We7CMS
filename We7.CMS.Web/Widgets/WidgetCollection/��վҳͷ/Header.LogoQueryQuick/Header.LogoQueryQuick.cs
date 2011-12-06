using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.CMS.WebControls.Core;
using We7.CMS.WebControls;
using We7.Framework.Config;
using We7.Framework;
using We7.CMS.Common;

namespace We7.CMS.Web.Widgets
{
    [ControlGroupDescription(Label = "网页头部", Icon = "网页头部", Description = "网页头部", DefaultType = "Header.LogoQueryQuick")]
    [ControlDescription("带Logo带搜索带快捷导航(头)")]
    public partial class Header_LogoQueryQuick : BaseControl
    {
        /// <summary>
        /// 自定义Css类名称
        /// </summary>
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "Header_LogoQueryQuick")]
        public string CssClass;

        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }
        protected ChannelHelper myChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        private string channelUrl;
        /// <summary>
        /// 当前的栏目URL
        /// </summary>
        public string ChannelUrl
        {
            get
            {
                if (string.IsNullOrEmpty(channelUrl))
                {
                    string channelID = myChannelHelper.GetChannelIDFromURL();
                    if (channelID.Equals(We7Helper.EmptyGUID))
                        channelUrl = "/";
                    else
                    {
                        Channel ch = myChannelHelper.GetChannel(channelID, new string[] { "FullUrl" });
                        channelUrl = ch.FullUrl;
                    }
                }
                return channelUrl;
            }
        }

        /// <summary>
        /// 自定义的css样式
        /// </summary>
        protected virtual string Css
        {
            get
            {
                return CssClass;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter(Title = "自定义会员登录图标样式", Type = "CustomImage", DefaultValue = "")]
        public string IconLogin;

        /// <summary>
        /// 
        /// </summary>
        protected virtual string CustomIconLogin
        {
            get {
                return IconLogin;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter(Title = "自定义RSS图标样式", Type = "CustomImage", DefaultValue = "")]
        public string IconRss;

        /// <summary>
        /// 
        /// </summary>
        protected virtual string CustomIconRss
        {
            get
            {
                return IconRss;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter(Title = "自定义WAP图标样式", Type = "CustomImage", DefaultValue = "")]
        public string IconWap;

        /// <summary>
        /// 
        /// </summary>
        protected virtual string CustomIconWap
        {
            get
            {
                return IconWap;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter(Title = "自定义设为首页图标样式", Type = "CustomImage", DefaultValue = "")]
        public string IconHomePage;

        /// <summary>
        /// 
        /// </summary>
        protected virtual string CustomHomePage
        {
            get
            {
                return IconHomePage;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter(Title = "自定义加入收藏图标样式", Type = "CustomImage", DefaultValue = "")]
        public string IconCollection;

        /// <summary>
        /// 
        /// </summary>
        protected virtual string CustomIconCollection
        {
            get
            {
                return IconCollection;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Logo路径
        /// </summary>
        public string LogoPath
        {
            get
            {
                if (String.IsNullOrEmpty(GeneralConfigs.GetConfig().SiteLogo))
                {
                    return ThemePath + "/images/logo.gif";
                }
                else
                {
                    return GeneralConfigs.GetConfig().SiteLogo;
                }
            }
        }

        public string SiteName
        {
            get { return GeneralConfigs.GetConfig().SiteTitle; }
        }

        protected string BackgroundIcon(int Icon)
        {
            switch (Icon)
            { 
                case 1:
                    if (!string.IsNullOrEmpty(CustomIconLogin))
                    {
                        return string.Format("style=\"background:url({0}) no-repeat;\"", CustomIconLogin);
                    }
                    break;
                case 2:
                    if (!string.IsNullOrEmpty(CustomIconRss))
                    {
                        return string.Format("style=\"background:url({0}) no-repeat;\"", CustomIconRss);
                    }
                    break;
                case 3:
                    if (!string.IsNullOrEmpty(CustomIconWap))
                    {
                        return string.Format("style=\"background:url({0}) no-repeat;\"", CustomIconWap);
                    }
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(CustomHomePage))
                    {
                        return string.Format("style=\"background:url({0}) no-repeat;\"", CustomHomePage);
                    }
                    break;
                case  5:
                    if (!string.IsNullOrEmpty(CustomIconCollection))
                    {
                        return string.Format("style=\"background:url({0}) no-repeat;\"", CustomIconCollection);
                    }
                    break;
            }            
            return string.Empty;
        }
    }

}