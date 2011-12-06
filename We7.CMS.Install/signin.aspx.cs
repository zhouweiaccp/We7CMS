using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Xml;

using We7.CMS.Config;
using Thinkment.Data;
using System.Security.Cryptography;
using We7.Framework.Config;

namespace We7.CMS.Install
{
    /// <summary>
    /// setup4 的摘要说明. 
    /// </summary>
    public class signin : SetupPage
    {
        protected System.Web.UI.WebControls.Button LoginButton;
        protected System.Web.UI.WebControls.TextBox VerifyCodeTextBox;
        protected System.Web.UI.WebControls.TextBox PasswordTextBox;
       
        string LogFile = "";

        string ReturnURL
        {
            get
            {
                if (Request["ReturnURL"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return Server.UrlDecode(Request["ReturnURL"].ToString());
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Init();
        }
        

       #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.LoginButton.Click += new EventHandler(this.LoginButton_Click);
        }
        #endregion

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (Request.Cookies["CheckCode"] == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('您的浏览器设置已被禁用 Cookies，您必须设置浏览器允许使用 Cookies 选项后才能使用本系统。');</script>"); 
                return;
            }

            if (String.Compare(Request.Cookies["CheckCode"].Value, VerifyCodeTextBox.Text.ToString().Trim(), true) != 0)
            {
                VerifyCodeTextBox.Text = "";
                VerifyCodeTextBox.Focus();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('对不起，验证码错误！');</script>");
                return;
            }
            SiteConfigInfo si=SiteConfigs.GetConfig();
            if (si != null && AdminPasswordIsValid(PasswordTextBox.Text,si))
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie("administrator",true);
                Response.Redirect(ReturnURL);
            }
            else
            {
                PasswordTextBox.Focus();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('对不起，密码错误！');</script>");
            }
        }

        public bool AdminPasswordIsValid(string password,SiteConfigInfo si)
        {
            if (si.IsPasswordHashed)
            {
                password = Encrypt(password);
            }
            string adminPass = si.AdministratorKey;// GetSystemParameter("CD.AdministratorKey");
            return string.Compare(password, adminPass, false) == 0;
        }

        public static string Encrypt(string password)
        {
            password = password.ToLower();

            Byte[] clearBytes = new UnicodeEncoding().GetBytes(password);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

            return BitConverter.ToString(hashedBytes);
        }
    }
}