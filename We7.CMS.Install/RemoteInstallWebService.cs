using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using We7.CMS.Config;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using We7.CMS.Common;
using We7.CMS.Plugin;
using We7.CMS.Accounts;

namespace We7.CMS.Install
{
    /// <summary>
    /// insall 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://westengine.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class RemoteInstallWebService : System.Web.Services.WebService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setupDbType"></param>
        /// <param name="dbi"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        [WebMethod]
        public string Install(string siteLoginName, string sitePassword, string pltype, string pluginName, string cmd, string action)
        {

            string source = "";
            try
            {
                if (CheckAdmin(siteLoginName, sitePassword))
                {
                    //PluginHelper 
                    return new PluginHelper(GetPluginType(pltype)).RunCommand(pluginName, cmd, action);
                }
                else
                {
                    return new PluginJsonResult(false, "安装站点的用户名与密码错误！").ToString(); ;
                }
            }
            catch (Exception ex)
            {
               
                EventLogHelper.WriteToLog(source, ex, EventLogEntryType.Error);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setupDbType"></param>
        /// <param name="dbi"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        [WebMethod]
        public string DownLoad(string siteLoginName, string sitePassword, string pltype, string pluginName, string cmd, string action, string pluginPath)
        {

            string source = "";
            try
            {
                if (CheckAdmin(siteLoginName, sitePassword))
                {
                    //PluginHelper 
                    return new PluginHelper(GetPluginType(pltype)).RunCommand(pluginName, cmd, action, pluginPath);
                }
                else
                {
                    return new PluginJsonResult(false, "安装站点的用户名与密码错误！").ToString(); ;
                }
            }
            catch (Exception ex)
            {
                EventLogHelper.WriteToLog(source, ex, EventLogEntryType.Error);
                throw ex;
            }
        }

        private PluginType GetPluginType(string pltype)
        {
            switch (pltype)
            {
                case "plugin":
                    return PluginType.PLUGIN;
                case "resource":
                    return PluginType.RESOURCE;
                default:
                    return PluginType.PLUGIN;
            }
        }
        [WebMethod]
        public bool IsInstall(string siteLoginName, string sitePassword, string plugintype, string pluginName)
        {

            if (CheckAdmin(siteLoginName, sitePassword))
            {

                PluginInfo info = null;
                //if (cmd.ToLower() == "download")
                //{
                //    info = new PluginInfo(plugintype);
                //    info.Directory = pluginName;
                //}
                //else
                //{
                info = PluginInfoCollection.CreateInstance(GetPluginType(plugintype))[pluginName];
                //}
                if (info != null && info.IsInstalled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new Exception("登陆名与密码不正确!");
            }

        }

        /// <summary>
        /// 在本地站点绑定We7Shop的登陆帐号
        /// </summary>
        /// <param name="shopLoginName">插件商场登陆帐号</param>
        /// <param name="shopPassword">插件商场密码</param>
        /// <param name="siteLoginName">本地站点登陆名</param>
        /// <param name="sitePassword">本地站点密码</param>
        /// <returns></returns>
        [WebMethod]
        public bool BindShopLoginName(string shopLoginName, string shopPassword, string siteLoginName, string sitePassword)
        {
            try
            {
                if (CheckAdmin(siteLoginName, sitePassword))
                {
                    SiteConfigInfo si = SiteConfigs.GetConfig();
                    si.ShopLoginName = shopLoginName;
                    si.ShopPassword = shopPassword;
                    SiteConfigs.SaveConfig(si);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        private bool CheckAdmin(string loginName, string password)
        {
            return String.Compare(loginName, SiteConfigs.GetConfig().AdministratorName, true) == 0 &&
          HelperFactory.Instance.GetHelper<SiteSettingHelper>().AdminPasswordIsValid(password);
        }
    }
}
