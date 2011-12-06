using System;
using System.Collections.Generic;
using System.Web;
using We7.CMS.Config;
using System.Web.Services;

namespace We7.CMS.Web.Admin.Ajax
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class IPStrategyCheck : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            bool result=false;
            context.Response.ContentType = "text/html";
            string key = context.Request.QueryString["key"];
            string type = context.Request.QueryString["type"];

            if (!String.IsNullOrEmpty(key))
            {

                if (type == "1")
                {

                    StrategyInfo info = StrategyConfigs.Instance[key];
                    result = info == null ? false : true;
                }
                else
                {
                    result = StrategyConfigs.Instance.ContainsName(key.Trim());
                }
            }
            context.Response.Write(result.ToString().ToLower());
        }

        public bool IsReusable
        {
            get
            {
                return　false;
            }
        }
    }
}
