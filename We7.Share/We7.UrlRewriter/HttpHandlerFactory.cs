using System;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Configuration;
using We7.Framework.Config;
using We7.CMS.Config;

namespace We7.UrlRewriter
{
    /// <summary>
    /// 暂时没用：HttpHandler重写
    /// </summary>
	public class HttpHandlerFactory:  IHttpHandlerFactory
	{
        public HttpHandlerFactory() { }


        public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string newUrl, string path)
        {
            string pagepath = path;
            BaseConfigInfo baseconfig = BaseConfigs.GetBaseConfig();
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            if (baseconfig != null && config != null)
            {
                string requestPath = context.Request.Path.ToLower();
                foreach (SiteUrls.URLRewrite url in SiteUrls.GetSiteUrls().Urls)
                {
                    if (Regex.IsMatch(requestPath, url.Pattern, RegexOptions.IgnoreCase))
                    {
                        if (url.Page != "")
                        {
                            string queryString = Regex.Replace(requestPath, url.Pattern, url.QueryString, RegexOptions.IgnoreCase);
                            context.RewritePath(url.Page, string.Empty, queryString);
                            pagepath = context.Server.MapPath(url.Page);
                            break;
                        }
                    }
                }
                if (!newUrl.EndsWith(config.UrlExtName, true, null))
                    newUrl += "default." + config.UrlExtName;
            }
            return PageParser.GetCompiledPageInstance(newUrl, pagepath, context);
        }

		public virtual void ReleaseHandler(IHttpHandler handler) 
		{

		}
	}
}
