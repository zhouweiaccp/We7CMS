using System;
using System.Collections.Generic;
using System.Web;
using We7.CMS.Config;
using System.Web.Services;
using We7.Framework.Config;
using We7.CMS.Common;
using We7.CMS.WebControls.Core;

namespace We7.CMS.Web.Admin.Ajax
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CreateControlIcon : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string result = "";
            context.Response.ContentType = "text/html";
            string control = context.Request.QueryString["control"];
            string filePath = context.Request["filepath"];
            DataControlInfo dc = new BaseControlHelper().GetIntegrationInfoByPath(filePath);
            string id = context.Request.QueryString["id"];
            try
            {
                if (!String.IsNullOrEmpty(control))
                {
                    string tmpfolder = GeneralConfigs.GetConfig().DefaultTemplateGroupFileName;
                    tmpfolder = tmpfolder.Remove(tmpfolder.IndexOf("."));
                    TemplateProcessor tp = new TemplateProcessor(tmpfolder);
                    //result = tp.CreateControlIcon(control, id);
                    string name = "";
                    if (!String.IsNullOrEmpty(dc.Name) && dc.Controls.Count > 0 && !String.IsNullOrEmpty(dc.Controls[0].Name))
                    {
                        name = dc.Name + "：" + dc.Controls[0].Name;
                    }
                    result = tp.CreateDataControlIcon(name, id);
                }
            }
            catch {  }
            context.Response.Write(result);
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
