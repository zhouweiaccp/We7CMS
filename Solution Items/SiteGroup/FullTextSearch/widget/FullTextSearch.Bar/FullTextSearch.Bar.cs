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

namespace We7.CMS.Web.Widgets.ShopDownload
{
    [ControlGroupDescription(Label = "站群搜索框", Icon = "站群搜索框", Description = "站群搜索框", DefaultType = "FullTextSearch.Bar")]
    [ControlDescription("站群搜索框")]
    public partial class FullTextSearch_Bar : BaseControl
    {
        [Parameter(Title = "搜索按钮文字", Type = "String", DefaultValue = "搜索")]
        public string SubmitTitle;

        /// <summary>
        /// 自定义Css类名称
        /// </summary>
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "FullTextSearch_Bar")]
        public string CssClass;

        /// <summary>
        /// 控件索引号
        /// </summary>
        [Parameter(Title = "控件索引号", Type = "String", DefaultValue = "1")]
        public string Index;
    }
}