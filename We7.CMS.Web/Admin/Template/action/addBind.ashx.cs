using System;
using System.Collections.Generic;
using System.Web;
using We7.CMS.Config;
using System.Web.Services;
using We7.Framework.Config;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class addBind : IHttpHandler
    {
         public void ProcessRequest(HttpContext context)
        {
            string result = "";
            context.Response.ContentType = "text/html";
            TemplateBindConfig bc = new TemplateBindConfig();
            bc.Handler = context.Request["handler"];
            bc.Mode = context.Request["mode"];
            bc.Model = context.Request["model"];
            string fileName = context.Request["filename"];
            string skinFolder = GeneralConfigs.GetConfig().DefaultTemplateGroupFileName;
            skinFolder = skinFolder.Remove(skinFolder.Length - 4);
            try
            {
                TemplateHelper TemplateHelper = HelperFactory.Instance.GetHelper<TemplateHelper>();
                TemplateHelper.SaveTemplateBind(bc, skinFolder, fileName);
                if (We7.Framework.AppCtx.IsDemoSite)
                {
                    context.Response.Write("对不起，此演示站点您没有该操作权限！");
                }
                else
                {
                    context.Response.Write("0");
                }
            }
            catch(Exception ex) 
            {
                context.Response.Write(ex.Message);
            }
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
