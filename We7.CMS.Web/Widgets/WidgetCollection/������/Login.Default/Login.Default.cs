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
using We7.CMS.WebControls;
using We7.CMS.WebControls.Core;
using We7.CMS.Accounts;
using We7.CMS.Common.PF;

namespace We7.CMS.Web.Widgets
{
    [ControlGroupDescription(Label = "登陆", Icon = "登陆", Description = "登陆", DefaultType = "Login.Default")]
    [ControlDescription(Desc = "登录")]
    public partial class Login_Default : BaseControl
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
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "Login_Default")]
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
        private Account currentAccount;
        private Account CurrentAccount
        {
            get
            {
                if (currentAccount == null)
                {
                    IAccountHelper helper = AccountFactory.CreateInstance();
                    currentAccount = helper.GetAccount(Security.CurrentAccountID, null);
                }
                return currentAccount;
            }
        }

        public string AccountName
        {
            get
            {
                return CurrentAccount != null ? CurrentAccount.LoginName : String.Empty;
            }
        }

        public bool IsLogin
        {
            get
            {
                return !string.IsNullOrEmpty(Security.CurrentAccountID);
            }
        }
    }
}