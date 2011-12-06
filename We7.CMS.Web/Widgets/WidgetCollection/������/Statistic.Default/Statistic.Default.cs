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
using System.Text;
using We7.CMS.WebControls;
using We7.CMS.WebControls.Core;
using We7.Framework;
using We7.CMS.Common;

namespace We7.CMS.Web.Widgets
{
    [ControlGroupDescription(Label = "网站统计", Icon = "网站统计", Description = "网站统计", DefaultType = "Statistic.Default")]
    [ControlDescription(Desc = "网站统计")]
    public partial class Statistic_Default : BaseControl
    {
        

        /// </summary>
        [Parameter(Title = "自定义图标样式", Type = "CustomImage", DefaultValue = "")]
        public string Icon;

        /// <summary>
        /// 自定义图标
        /// </summary>
        protected virtual string CustomIcon
        {
            get
            {
                return Icon;
            }
        }
        protected string BackgroundIcon()
        {
            if (!string.IsNullOrEmpty(CustomIcon))
            {
                return string.Format("style=\"background:url({0}) no-repeat;\"", CustomIcon);
            }
            return string.Empty;
        }
        /// </summary>
        [Parameter(Title = "自定义边框样式", Type = "ColorSelector", DefaultValue = "")]
        public string BorderColor;

        protected virtual string BoxBorderColor
        {
            get
            {
                return BorderColor;
            }
        }
        protected string SetBoxBorderColor()
        {
            if (!string.IsNullOrEmpty(BoxBorderColor))
            {
                return string.Format("style=\"border-color:{0};\"", BoxBorderColor);
            }
            return string.Empty;
        }
        /// <summary>
        /// 自定义Css类名称
        /// </summary>
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "Statistic_Default")]
        public string CssClass;

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

        protected string GetVisitorCount()
        {
            PageVisitorHelper helper = ((HelperFactory)Application[Framework.HelperFactory.ApplicationID]).GetHelper<PageVisitorHelper>();
            VisiteCount vc = helper.GetCurrentVisiteCount();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"    总访问量："+vc.TotalVisitors+@"人次<br>
                            总浏览量："+vc.TotalPageView+@"人次<br>
                            今日访问："+vc.DayVisitors+@"人次<br>
                            日均访问：" + vc.AverageDayVisitors + "人次<br>");
            return sb.ToString();
        }

    }
}