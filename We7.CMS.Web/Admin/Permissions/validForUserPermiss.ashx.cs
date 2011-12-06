using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using We7.CMS.Accounts;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin.Permissions
{
    /// <summary>
    /// validForUserPermiss 的摘要说明
    /// </summary>
    public class validForUserPermiss : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string Massage = string.Empty;
            string kw=context.Request.QueryString["kw"];
            int i;
            if (int.TryParse(context.Request.QueryString["type"], out i) && !string.IsNullOrEmpty(kw))
            {
                if ((int)validForUserType.ExistCurrentUser==i)
                {
                    if (AccountHelper.ExistUserName(kw))
                    {
                        Massage = "当前用户已存在";
                    }
                    else if (SiteConfigs.GetConfig().AdministratorName.Equals(kw))
                    {
                        Massage = "当前用户名为系统关键字";
                    }
                }
                if ((int)validForUserType.ExistEmail == i)
                {
                    if (AccountHelper.ExistEmail(kw))
                    {
                        Massage = "当前Email已存在";
                    }
                  
                }
            }
            else
            {
                Massage = "Ajax参数不正确，请勿恶意请求！";
               
            }
            context.Response.Write(Massage);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 权限业务对象
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }
    }
    /// <summary>
    /// Ajax验证类型
    /// </summary>
    public enum validForUserType
    {
        /// <summary>
        /// 是否存在当前用户
        /// </summary>
        ExistCurrentUser=0,
        /// <summary>
        /// 是否存在当前Email
        /// </summary>
        ExistEmail=1
    }
}