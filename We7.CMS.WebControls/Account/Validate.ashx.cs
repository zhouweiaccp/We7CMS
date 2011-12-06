using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using We7.Framework;
using We7.CMS.Common.PF;
using We7.CMS.Accounts;
using We7.Framework.Util;
using We7.Framework.Config;

namespace We7.CMS.WebControls.AccountEx
{
	/// <summary>
	/// $codebehindclassname$ 的摘要说明
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class Validate : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Cache.SetNoStore();
			context.Response.Clear();
			string action = context.Request["action"];
			string msg = "success";
			if (!String.IsNullOrEmpty(action))
			{
				IAccountHelper helper = AccountFactory.CreateInstance();
				string key = context.Request["value"];
				action = action.Trim().ToLower();
				Account act = null;
				if (action == "user")
				{
					act = helper.GetAccountByLoginName(key);
					if (act != null)
					{
						context.Response.Write("当前用户已存在");
						return;
					}
				}
				if (action == "email")
				{
					act = helper.GetAccountByEmail(key);
					if (act != null)
					{
						context.Response.Write("当前Email已被注册");
						return;
					}
				}
				if (action == "validate")
				{
					act = helper.GetAccount(context.Request["AccountID"], null);
					if (act == null)
					{
						context.Response.Write("验证帐号不存在，请重新申请帐号!");
					}
					else
					{
						act.EmailValidate = 1;
						act.State = 1;
						helper.UpdateAccount(act, new string[] { "EmailValidate", "State" });
					}
				}
				if (action == "submit")
				{
					Account newAccout = new Account();
					newAccout.LoginName = context.Request["name"];
					newAccout.Password = context.Request["pwd"];
					if (SiteConfigs.GetConfig().IsPasswordHashed)
						newAccout.Password = Security.Encrypt(newAccout.Password);
					newAccout.Email = context.Request["email"];
					newAccout.UserType = 1;
					newAccout.Created = DateTime.Now;
					try
					{
						helper.AddAccount(newAccout);
						if (SendEmail(newAccout, context.Request))
							msg += ":email";
					}
					catch (Exception ex) { context.Response.Write(ex.Message); return; }
				}
			}
			context.Response.Write(msg);
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		bool SendEmail(Account account, HttpRequest Request)
		{
			try
			{
				MailHelper mailHelper = GetMailHelper();
				string subject = "注册系统邮件验证";
				string message = String.Format("亲爱的" + account.LoginName + "，您好！<p/>感谢您在我们网站注册成为会员，故系统自动为你发送了这封邮件。请点击下面链接进行验证：<br /><a href='{0}'>{0}</a>",
					Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + We7Helper.AddParamToUrl(Request["url"], "v", account.ID));
				mailHelper.Send(account.Email, mailHelper.AdminEmail, subject, message, "Low");
				return true;
			}
			catch { return false; }

		}
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
	}
}
