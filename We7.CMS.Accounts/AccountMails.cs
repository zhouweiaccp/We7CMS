using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Util;
using We7.Framework.Config;
using We7.CMS.Common.PF;
using System.Web;

namespace We7.CMS.Accounts
{
    /// <summary>
    /// 账户处理相关邮件通知操作
    /// </summary>
    public class AccountMails
    {
        /// <summary>
        /// 新注册用户通知本人
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="password"></param>
        /// <param name="message"></param>
        public static void SendMailOfRegister(Account account, string password, string message)
        {
            try
            {
                MailHelper mailHelper = GetMailHelper();
                if (String.IsNullOrEmpty(mailHelper.AdminEmail))
                    throw new Exception("邮件发送失败");
                MailMessageTemplate mt = new MailMessageTemplate("UserEmailConfig.xml", "新建用户通知");
                if (String.IsNullOrEmpty(mt.Body))
                    mt.Body = String.Empty;
                if (String.IsNullOrEmpty(mt.Subject))
                    mt.Subject = String.Empty;
                string subject = mt.Subject.Replace("${LoginName}", account.LoginName);
                if (string.IsNullOrEmpty(message))
                    message = mt.Body.Replace("${LoginName}", account.LoginName);
                else
                    message = message.Replace("${LoginName}", account.LoginName);

                message = message.Replace("${Password}", password);
                message = We7Helper.ConvertTextToHtml(message);
                mailHelper.Send(account.Email, mailHelper.AdminEmail, subject, message, "Low");
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(AccountMails), ex);
                throw ex;
            }
        }

        /// <summary>
        /// 账号审核通过通知
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="modelName"></param>
        /// <param name="message"></param>
        public static void SendMailOfPassNotify(Account account, string modelName, string message)
        {
            try
            {
                MailHelper mailHelper = GetMailHelper();
                MailMessageTemplate mt = new MailMessageTemplate("UserEmailConfig.xml", "账号审核通过通知");
                string subject = mt.Subject.Replace("${ContentModelName}", modelName);
                if (string.IsNullOrEmpty(message))
                    message = mt.Body;
                message = message.Replace("${LoginName}", account.LoginName);
                message = message.Replace("${ContentModelName}", modelName);
                message = We7Helper.ConvertTextToHtml(message);
                mailHelper.Send(account.Email, mailHelper.AdminEmail, subject, message, "Low");
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(AccountMails), ex);
                throw ex;
            }
        }

        /// <summary>
        /// 注册验证通知
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="type"></param>
        public static bool SendMailOfValidate(Account account, string strFEID)
        {
            try
            {
                MailHelper mailHelper = GetMailHelper();
                MailMessageTemplate mt = new MailMessageTemplate("UserEmailConfig.xml", "注册验证通知");
                string subject = mt.Subject.Replace("${LoginName}", account.LoginName);
                string message = mt.Body.Replace("${LoginName}", account.LoginName);
                HttpContext context = HttpContext.Current;
                string url = "http://" + context.Request.Url.Host;
                if (context.Request.Url.Port != 80)
                    url += ":" + context.Request.Url.Port.ToString();
                url += "/User/Validate.aspx?" + strFEID + "=" + account.ID + "&returnUrl=/register.aspx";
                message = message.Replace("${ValidateUrl}", string.Format("{0}", "<a href='" + url + "' target='_blank'>" + url + "</a>"));
                //message = We7Helper.ConvertTextToHtml(message);
                mailHelper.Send(account.Email, mailHelper.AdminEmail, subject, message, "Low");
                return true;
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(AccountMails), ex);
                throw ex;                
            }
        }

        /// <summary>
        /// 新用户申请通知
        /// </summary>
        public static void SendMailOfHandle(Account account, string modelName)
        {
            try
            {
                MailHelper mailHelper = GetMailHelper();
                MailMessageTemplate mt = new MailMessageTemplate("UserEmailConfig.xml", "新用户申请通知");
                string subject = mt.Subject.Replace("${LoginName}", account.LoginName);
                subject = subject.Replace("${ContentModelName}", modelName);
                string message = mt.Body.Replace("${LoginName}", account.LoginName);
                HttpContext context = HttpContext.Current;
                string url = "http://" + context.Request.Url.Host;
                if (context.Request.Url.Port != 80)
                    url += ":" + context.Request.Url.Port.ToString();
                message = message.Replace("${HandleUrl}", String.Format("{0}", url + "/admin/Permissions/AccountEdit.aspx?id=" + account.ID));
                message = We7Helper.ConvertTextToHtml(message);
                mailHelper.Send(GeneralConfigs.GetConfig().NotifyMail, mailHelper.AdminEmail, subject, message, "Low");
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(AccountMails), ex);
                throw ex;                
            }
        }


        /// <summary>
        /// 用户付款成功通知
        /// </summary>
        public static void SendMailOfSuccessPay(Account account, string productName, decimal payMoney)
        {
            try
            {
                MailHelper mailHelper = GetMailHelper();
                MailMessageTemplate mt = new MailMessageTemplate("UserEmailConfig.xml", "付款成功通知");
                string subject = mt.Subject.Replace("${ProductName}", "[" + productName + "]");
                string message = mt.Body.Replace("${LoginName}", account.LoginName).Replace("${ProductName}", "[" + productName + "]");
                message = message.Replace("${PayMoney}", string.Format("{0:C2}", payMoney));
                message = We7Helper.ConvertTextToHtml(message);
                mailHelper.Send(account.Email, mailHelper.AdminEmail, subject, message, "Low");
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(AccountMails), ex);
                throw ex;
            }
        }

        /// <summary>
        /// 邮件配置信息
        /// </summary>
        /// <param name="adviceType"></param>
        public static MailHelper GetMailHelper()
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
    }
}
