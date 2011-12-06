using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using We7.CMS.Common;
using We7.CMS.Plugin;

namespace We7.CMS.Web.Install.Plugin
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PluginCommandHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
            string pluginName = context.Request["plugin"];
            string cmd = context.Request["cmd"];
            string action = context.Request["action"];
            string pltype = context.Request["pltype"];
            context.Response.Clear();

            if (String.IsNullOrEmpty(pluginName) || String.IsNullOrEmpty(cmd) || String.IsNullOrEmpty(action))
            {
                context.Response.Write(new PluginJsonResult(false, "请求参数不正确!"));
                context.Response.End();
            }

            if (cmd.ToLower() == "reset")
            {
                try
                {
                    ApplicationHelper.ResetApplication();
                    context.Response.Write(new PluginJsonResult(true, "成功启用应用程序!"));
                    if (HttpContext.Current.Items["ALLSHOWMEMUITEM"] != null)
                    {
                        HttpContext.Current.Items.Remove("ALLSHOWMEMUITEM");
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write(new PluginJsonResult(false, "重启应用程序出错!"));
                }
            }
            else
            {                
                context.Response.Write(new PluginHelper(GetPluginType(pltype)).RunCommand(pluginName, cmd, action));
            }
            context.Response.End();
            context.Response.Close();
        }

        private PluginType GetPluginType(string pltype)
        {
            switch (pltype)
            {
                case "plugin":
                    return PluginType.PLUGIN;
                case "constrol":
                    return PluginType.RESOURCE;
                default:
                    return PluginType.PLUGIN;
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
