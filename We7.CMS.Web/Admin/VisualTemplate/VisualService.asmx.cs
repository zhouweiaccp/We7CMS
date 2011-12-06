using System.ComponentModel;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using We7.CMS.Module.VisualTemplate.Helpers;
using We7.CMS.Module.VisualTemplate.Services;
using VisualDesign.Module;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Text;
using We7.CMS.Module.VisualTemplate;
using System;
using We7.CMS.Module.VisualTemplate.Models;
using System.IO;
using We7.Framework.Config;
using We7.CMS.Plugin;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.VisualTemplate
{
    /// <summary>
    /// VisualService 用于可视化设计基本操作
    /// </summary>
    [WebService(Namespace = "http://we7.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService()]
    public class VisualService : System.Web.Services.WebService, IRequiresSessionState
    {
        private We7.CMS.ShopService.ShopService _ShopService;
        /// <summary>
        /// 商城Service地址 todo
        /// </summary>
        public We7.CMS.ShopService.ShopService ShopService
        {
            get
            {
                if (_ShopService == null)
                {
                    _ShopService = new We7.CMS.ShopService.ShopService();
                    _ShopService.Url = GeneralConfigs.GetConfig().ShopService.TrimEnd('/').TrimEnd('\\') + "/Plugins/ShopPlugin/ShopService.asmx";
                }
                return _ShopService;
            }
        }

        /// <summary>
        /// 获取系统widget列表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        #region string GetSystemWidgets()
        public string GetSystemWidgets()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            try
            {
                WidgetService widgetService = new WidgetService();
                var widgets = widgetService.GetSystemWidgetList();
                ajaxResponse.Data = widgets;
                ajaxResponse.Success = true;

            }
            catch (Exception ex)
            {
                //获取widget列表失败
                ajaxResponse.Success = false;
                ajaxResponse.Message = ex.Message;
            }

            return ajaxResponse.ToJson();
        }
        #endregion

        /// <summary>
        /// 获取layoutskins
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        #region string GetLayoutSkins()
        public string GetLayoutSkins()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();

            var layoutIndexFile = HttpContext.Current.Server.MapPath("~/Widgets/Themes/Themes.xml");
            try
            {

                var skins = SettingFileService.GetSettings(layoutIndexFile);

                ajaxResponse.Data = skins;
                ajaxResponse.Success = true;

            }
            catch (Exception ex)
            {
                //获取widget列表失败
                ajaxResponse.Success = false;
                ajaxResponse.Message = ex.Message;
            }

            return ajaxResponse.ToJson();
        }
        #endregion

        /// <summary>
        /// GetWarpList
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        #region string GetWarpList()
        public string GetWarpList()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();

            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();

                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Widgets/Wrapper");
                DirectoryInfo di = new DirectoryInfo(dir);
                if (di.Exists)
                {
                    FileInfo[] files = di.GetFiles("*.vm");
                    if (files != null)
                    {
                        foreach (FileInfo f in files)
                        {
                            string name = Path.GetFileNameWithoutExtension(f.Name);
                            dic.Add(name, name);
                        }
                    }
                }
                ajaxResponse.Data = dic;
            }


            catch (Exception ex)
            {

                ajaxResponse.Success = false;
                ajaxResponse.Message = ex.Message;
            }

            return ajaxResponse.ToJson();
        }
        #endregion

        /// <summary>
        /// 通过控件获取控件的CSS样式列表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        #region string[] GetStyleByControl(string controlPath)
        public string[] GetStyleByControl(string controlPath)
        {
            string css = null;
            string control = Path.GetFileNameWithoutExtension(controlPath);

            string physicalPath = HttpContext.Current.Server.MapPath(controlPath);
            DirectoryInfo[] dis = new FileInfo(physicalPath).Directory.GetDirectories("Style");
            DirectoryInfo di = dis != null && dis.Length > 0 ? dis[0] : null;

            if (di != null && di.Exists)
            {
                FileInfo[] fs = di.GetFiles("*.css");
                if (fs.Length > 0)
                {
                    using (StreamReader sr = new StreamReader(fs[0].FullName, Encoding.Default))
                    {
                        css = sr.ReadToEnd();
                    }
                }
            }

            System.Text.RegularExpressions.MatchCollection m = System.Text.RegularExpressions.Regex.Matches(css,
                                                                "\\*\\s*Style:.*",
                                                                System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (m.Count > 0)
            {
                System.Collections.ArrayList list = new System.Collections.ArrayList();
                foreach (System.Text.RegularExpressions.Match it in m)
                {
                    string s = it.Value.Substring(it.Value.IndexOf(":") + 1).Replace("_", ".").Trim();
                    if (!list.Contains(s))
                        list.Add(s);
                }
                return (string[])list.ToArray(typeof(string));
            }

            return null;
        }
        #endregion

        /// <summary>
        /// 检查站点的绑定状态
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        #region string GetSiteAuthroizeState()
        public string GetSiteAuthroizeState()
        {
            AjaxResponse ajaxResponse = new AjaxResponse();            
            ajaxResponse.Success = IsSiteBindShop();
            if (ajaxResponse.Success)
            {
                string msg;
                ajaxResponse.Data = GetFreeWidgetList(out msg);
                ajaxResponse.Message = msg;
            }

            return ajaxResponse.ToJson();
        }

        /// <summary>
        /// 获取免费部件列表
        /// </summary>
        /// <returns></returns>
        private List<We7.CMS.ShopService.ProductInfo> GetFreeWidgetList(out string msg)
        {
            try
            {
                We7.CMS.ShopService.ProductInfo[] ps =  ShopService.GetRecommendProductByType(10, "kj", 1);
                List<We7.CMS.ShopService.ProductInfo> list = new List<We7.CMS.ShopService.ProductInfo>(ps);
                for (int i = 0; i < list.Count; i++)
                {
                    bool installed = CheckInstalled(list[i].Url);
                    list[i].State = installed ? 1 : 0;
                }
                msg = "";
                return list;
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(VisualService), ex);
                msg = ex.ToString();
                return null;
            }
        }

        /// <summary>
        /// 检查是否已经安装过
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool CheckInstalled(object url)
        {
            string fileName = Path.GetFileNameWithoutExtension(url as string);
            PluginHelper helper = new PluginHelper();
            return helper.isInstalled(fileName);
        }

        /// <summary>
        /// 站点是否绑定商城
        /// </summary>
        /// <returns></returns>
        private bool IsSiteBindShop()
        {
            if (string.IsNullOrEmpty(SiteConfigs.GetConfig().ShopLoginName))
                return false;
            string sln = SiteConfigs.GetConfig().ShopLoginName.Trim();

            try
            {
                //帐号检验
                SiteConfigInfo si = SiteConfigs.GetConfig();
                string[] states = ShopService.CheckSite(si.ShopLoginName, si.ShopPassword, si.SiteUrl);
                if (states != null && states.Length > 0 && states[0] == "1")
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage), ex);
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 安装部件
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        #region string Install()
        public string Install()
        {
            string productId = RequestHelper.Get<string>("id");
            //string name = RequestHelper.Get<string>("name");
            //string url = RequestHelper.Get<string>("url");

            return InstallWidget(productId);
        }

        /// <summary>
        /// 安装部件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string InstallWidget(string productId)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();

            SiteConfigInfo si = SiteConfigs.GetConfig();
            string fileUrl = ShopService.InsertOrders(productId, si.ShopLoginName, si.SiteUrl);
            //不是网址就失败了
            if (fileUrl.ToLower().StartsWith("http://"))
            {
                ajaxResponse.Success = true;
                ajaxResponse.Message = fileUrl;
            }
            else
            {
                ajaxResponse.Success = false;
                ajaxResponse.Message = fileUrl;
            }
            return ajaxResponse.ToJson();
        }
        #endregion
    }
}
