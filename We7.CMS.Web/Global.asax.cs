using System;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using System.Xml;
using Thinkment.Data;
using We7.Framework;
using We7.Framework.Util;
using We7.Framework.Config;

using We7.CMS.Config;

namespace We7.CMS.Web
{
    public class Global : HttpApplication
    {
        private static readonly string ERROR_PAGE_LOCATION = "~/errors.aspx";

        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            ApplicationHelper.ResetApplication();
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            //ApplicationHelper.ResetApplication();
            Exception ex = Server.GetLastError();
            We7.Framework.LogHelper.WriteLog(typeof(Global), ex);

            if (Context != null && Context.IsCustomErrorEnabled)
                Server.Transfer(ERROR_PAGE_LOCATION, false);
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码
            Application.Lock();
            if (Application[PageVisitorHelper.OnlinePeopleApplicationKey] != null)
            {
                Application[PageVisitorHelper.OnlinePeopleApplicationKey] = (int)Application[PageVisitorHelper.OnlinePeopleApplicationKey] + 1;
            }
            Application.UnLock();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            Application.Lock();
            Application[PageVisitorHelper.OnlinePeopleApplicationKey] = (int)Application[PageVisitorHelper.OnlinePeopleApplicationKey] - 1;
            Application.UnLock();
            if (GeneralConfigs.GetConfig().StartPageViewModule)
            {
                PageVisitorHelper.PageVisitorLeave();
            }
        }

        protected PageVisitorHelper PageVisitorHelper
        {
            get { return HelperFactory.GetHelper<PageVisitorHelper>(); }
        }
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
             if (custom.ToLower() == "url")
            {
                return Context.Request.Path;
            }

            return base.GetVaryByCustomString(context, custom);
        }
    }
}