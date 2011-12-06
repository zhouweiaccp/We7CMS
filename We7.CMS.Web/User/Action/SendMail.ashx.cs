using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.Framework;
using System.Web.SessionState;
using We7.CMS.Accounts;

namespace We7.CMS.Web.User.Action
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SendMail :IHttpHandler, IRequiresSessionState  
    {
        /// <summary>
        /// 权限业务对象
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string loginName = context.Request["loginName"];
                string email = context.Request["email"];
                Account a = AccountHelper.GetAccountByLoginName(loginName);
                if (a == null)
                    context.Response.Write("用户名不存在！");
                else if (a.Email != email)
                    context.Response.Write("您输入的邮件地址不正确！");
                else if (AccountMails.SendMailOfValidate(a, We7.Model.Core.UI.Constants.FEID))
                    context.Response.Write("0");
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
