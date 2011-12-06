using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.PF;
using We7.Framework.Util;
using We7.Framework.Config;
using System.Web;

namespace We7.CMS.WebControls
{
    public class RetrievePasswordProvider : BaseWebControl
    {

        private bool _isHtml = false;
        /// <summary>
        /// 是否需要静态化
        /// </summary>
        public override bool IsHtml
        {
            get { return false; }
            set { _isHtml = value; }
        }
        string cssClass;
        /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }
        protected global::System.Web.UI.WebControls.TextBox txtUserName;
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.TextBox txtValidate;
        protected global::System.Web.UI.WebControls.TextBox txtNewPWD;
        protected global::System.Web.UI.WebControls.TextBox txtReNewPWD;


        protected global::System.Web.UI.WebControls.Button bttnRetrieve;
        protected global::System.Web.UI.WebControls.Button btnChangePWD;
        
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.Label lblError; 
        protected global::System.Web.UI.WebControls.Label lblUserName;
        protected global::System.Web.UI.WebControls.Label lblUpdateMessage; 
        
        protected global::System.Web.UI.WebControls.Panel pnlRetrieve;
        protected global::System.Web.UI.WebControls.Panel pnlSuccess;
        protected global::System.Web.UI.WebControls.Panel pnlSendEmail;
        protected global::System.Web.UI.WebControls.Panel pnlUpdate;
        protected global::System.Web.UI.WebControls.Panel pnlError;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!GetCheckResult())
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 第一步：找回密码，发送邮件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        protected void bttnRetrieve_Onclick(object sender, EventArgs arg)
        {
            bttnRetrieve.Enabled = false;
            StringBuilder checkResult = new StringBuilder("");
            string userName = this.txtUserName.Text.Trim();
            string email = this.txtEmail.Text.Trim();

            string validateCodeError = CheckValidateCode();
            if (!string.IsNullOrEmpty(validateCodeError))
            {
                checkResult.Append(validateCodeError + "<br/>");
                lblMsg.Text = "<font color='red'>" + checkResult.ToString() + "</font>";
                return;
            }

            if (!AccountHelper.ExistUserName(userName))
            {
                checkResult.Append("该用户名不存在！"+"<br/>");
            }
            else
            {
                Account account = AccountHelper.GetAccountByLoginName(userName);
                if(account.Email != email)
                {
                    checkResult.Append("用户名和邮箱不匹配");
                }
            }
            if (!string.IsNullOrEmpty(checkResult.ToString()))
            {
                lblMsg.Text = "<font color='red'>" + checkResult.ToString() + "</font>";
            }
            else
            {
                //Account account = AccountHelper.GetAccountByLoginName(userName);
                //string pwd = CreateVerifyCode(8);
                //account.Password = pwd;
                //account.Updated =  DateTime.Now;
                //account.PasswordHashed = (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.noneEncrypt;
                //AccountHelper.UpdatePassword(account,pwd);
                //AccountHelper.UpdateAccount(account, new string[] { "Password", "Updated", "IsPasswordHashed" });

                if(SendMailOfRetrieveTradePWD(userName))
                    ChangePnlVisible(2,"");
            }
            bttnRetrieve.Enabled = true;
        }

        /// <summary>
        /// 第三步：修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        protected void btnChangePWD_Onclick(object sender, EventArgs arg)
        {
            string strNewPwd = txtNewPWD.Text;
            string strReNewPwd = txtReNewPWD.Text;
            StringBuilder checkResult = new StringBuilder("");
            if (strNewPwd.Length < 6)
            {
                checkResult.Append("密码不能小于六位！<br/>");
            }
            if(strReNewPwd != strNewPwd )
            {
                checkResult.Append("确认新密码与新密码不一致！<br/>");
            }
            if (!string.IsNullOrEmpty(checkResult.ToString()))
            {
                lblUpdateMessage.Text = "<font color='red'>" + checkResult.ToString() + "</font>";
            }
            else
            {
                Account account = AccountHelper.GetAccountByLoginName(lblUserName.Text);               
                account.Password = txtNewPWD.Text;
                account.Updated = DateTime.Now;
                account.PasswordHashed = (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.noneEncrypt;
                AccountHelper.UpdatePassword(account, account.Password);
                ChangePnlVisible(4, "");
            }
            
        }

        /// <summary>
        /// 执行检查
        /// </summary>
        /// <returns></returns>
        private bool GetCheckResult()
        {
            string field = Request["field"];
            string validate = Request["validate"];
            if (!string.IsNullOrEmpty(field) && !string.IsNullOrEmpty(validate))
            {
                
                Account accountModel = AccountHelper.GetAccount(field, null);
                if (accountModel == null)
                {
                    ChangePnlVisible(5, "验证错误！");
                    return false;
                }
                string validatePWD = "";
                try
                {
                    validatePWD = EncryptString.Decrypt(validate);
                }
                catch (Exception ex)
                {
                    ChangePnlVisible(5, "验证错误！");
                    return false;
                }
                if (accountModel.Password != validatePWD)
                {
                    ChangePnlVisible(5, "错误，此验证已过期！");
                    return false;
                }
                ChangePnlVisible(3, accountModel.LoginName);
            }
            else
            {
                ChangePnlVisible(1, "");
            }
            return true;
        }

        /// <summary>
        /// 更改pannel显示
        /// </summary>
        /// <param name="step"></param>
        private void ChangePnlVisible(int step, string message)
        {
            pnlRetrieve.Visible = false;
            pnlSendEmail.Visible = false;            
            pnlUpdate.Visible = false;
            pnlSuccess.Visible = false;
            pnlError.Visible = false;
            switch (step)
            {
                case 1:
                    pnlRetrieve.Visible = true;
                    break;
                case 2:
                    pnlSendEmail.Visible = true;
                    break;
                case 3:
                    pnlUpdate.Visible = true;
                    lblUserName.Text = message;
                    break;
                case 4:
                    pnlSuccess.Visible = true;
                    break;
                case 5:
                    pnlError.Visible = true;
                    lblError.Text = message;
                    break;
                default:
                    pnlRetrieve.Visible = true;
                    break;
            }

        }

        /// <summary>
        /// 检测验证码是否有效
        /// </summary>
        /// <returns></returns>
        string CheckValidateCode()
        {
            if (String.Compare(txtValidate.Text.Trim(), Request.Cookies["CheckCode"].Value, true) == 0)
            {
                return "";
            }
            else
            {
                return "验证码错误";
            }
        }


        /// <summary>
        /// 邮件配置信息
        /// </summary>
        /// <param name="adviceType"></param>
        MailHelper GetMailHelper()
        {
            MailHelper mailHelper = new MailHelper();

            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            if (ci != null)
            {
                mailHelper.SmtpServer = ci.SysMailServer;
                mailHelper.AdminEmail = ci.SystemMail;
                mailHelper.UserName = ci.SysMailUser;
                mailHelper.Password = ci.SysMailPassword;
                mailHelper.PopServer = ci.SysPopServer;
            }
            return mailHelper;
        }


        /// <summary>
        /// 发送更改验证通知
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="type"></param>
        public  bool SendMailOfRetrieveTradePWD(string loginName)
        {
            try
            {
                MailHelper mailHelper = GetMailHelper();
                if (mailHelper.AdminEmail.Length == 0)
                {
                    lblMsg.Text = "<font color='red'>管理员邮箱未设置，请与管理员联系！</font>";
                    return false;
                }
                Account account = AccountHelper.GetAccountByLoginName(loginName);
                MailMessageTemplate mt = new MailMessageTemplate("UserEmailConfig.xml", "密码找回通知");
                string subject = mt.Subject.Replace("${LoginName}", account.LoginName);
                string message = mt.Body.Replace("${LoginName}", account.LoginName);
                HttpContext context = HttpContext.Current;

                string url = context.Request.Url.Scheme + "://" + context.Request.Url.Host + ":" + context.Request.Url.Port + "/retrievepassword.aspx?field=" + account.ID + "&validate=" + EncryptString.Encrypt(account.Password);
                message = message.Replace("${ValidateUrl}", string.Format("{0}", "<a href='" + url + "' target='_blank'>" + url + "</a>"));
                mailHelper.Send(account.Email, mailHelper.AdminEmail, subject, message, "Low");
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
