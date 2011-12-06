using System
;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using We7.CMS.Config;
using We7.CMS.Plugin;
using System.IO;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.Modules.Plugin
{
    /// <summary>
    /// PluginInfomation1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class PluginInfomation : System.Web.Services.WebService
    {

        [WebMethod]
        public List<RemotePluginInfo> LoadServerInfo(PluginType pluginType)
        {
            return new PluginHelper(pluginType).LoadServerInfo();
        }

        [WebMethod]
        public RemotePluginInfo LoadRemotePluginInfo(string pluginName, PluginType pluginType)
        {
            List<RemotePluginInfo> list = new PluginHelper(pluginType).LoadServerInfo();
            return list.Find(delegate(RemotePluginInfo info)
            {
                return info.Directory == pluginName;
            });
        }

        [WebMethod]
        public void CheckTempFile(string pluginName, PluginType pluginType)
        {
            new PluginHelper(pluginType).PickUpTempFile(pluginName);
        }

        [WebMethod]
        public void AddDownLoads(string pluginName, PluginType pluginType)
        {
            new PluginHelper(pluginType).AddClicks(pluginName);
        }
    }
}
