using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using System.Xml;
using System.IO;

using We7.CMS.Config;
using Thinkment.Data;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.Model.Core;
using We7.Model.Core.Data;

namespace We7.CMS
{

    /// <summary>
    /// We7网站应用操作
    /// </summary>
    public class ApplicationHelper
    {
        static object lockHelper = new object();//互斥锁
        static object lockHelper2 = new object();

        /// <summary>
        /// 重启网站
        /// </summary>
        public static void ResetApplication()
        {
            lock (lockHelper)
            {
                HttpContext context = HttpContext.Current;
                context.Application.Clear();

                if (context.Session != null)
                    context.Session.Clear();

                BaseConfigs.ResetConfig();
                SiteConfigs.ResetConfig();
                GeneralConfigs.ResetConfig();
                PluginManager.LoadPlugins();
                context.Application["We7.Application.OnlinePeople.Key"] = 0;

                //如果数据库配置文件存在，加载配置
                if (BaseConfigs.ConfigFileExist())
                {
                    BaseConfigInfo baseconfig = BaseConfigs.GetBaseConfig();

                    //加载数据库映射表
                    string root = context.Server.MapPath("~/");
                    string dataPath = context.Server.MapPath("~/App_Data/XML");
                    ObjectAssistant assistat = new ObjectAssistant();
                    try
                    {
                        if (baseconfig != null && baseconfig.DBConnectionString != "")
                        {
                            baseconfig.DBConnectionString = baseconfig.DBConnectionString.
                                Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory).Replace("\\\\", "\\");
                            assistat.LoadDBConnectionString(baseconfig.DBConnectionString, baseconfig.DBDriver);
                        }
                        assistat.LoadDataSource(dataPath);
                    }
                    catch (Thinkment.Data.DataException ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        string source = "CD.Utils_CDHelper_ResetApplication";
                        EventLogHelper.WriteToLog(source, ex, EventLogEntryType.Error);

                        string msg = string.Format("注意检查：/app_data/xml里的配置文件：\r\n错误代码：{0}\r\n错误消息：{1}"
                            , ex.ErrorCode, ex.Message);
                        We7.Framework.LogHelper.WriteLog(typeof(ApplicationHelper), msg);
                        throw ex;
                    }

                    HelperFactory hf = new HelperFactory();
                    /*
                     * 添加内容模型表结构(系统内置字段已在LoadDataSource里添加)
                     * author:丁乐
                     */
                    ModelHelper.ReCreateModelIndex();
                    MoudelMonitoring.SetModelDataDic(assistat);

                    hf.Assistant = assistat;
                    hf.Root = root;
                    hf.Initialize();
                    context.Application.Add("We7.HelperFactory", hf);
                    AppCtx.Cache.AddObject(HelperFactory.CacheKey, hf);

                    MoudelMonitoring ml = new MoudelMonitoring();  //监控内容模型
                }
            }

        }

      


        /// <summary>
        /// 重启应用程序
        /// </summary>
        public static void LoadHelperFactory()
        {
            lock (lockHelper2)
            {
                //如果数据库配置文件存在，加载配置
                if (BaseConfigs.ConfigFileExist())
                {
                    BaseConfigInfo baseconfig = BaseConfigs.GetBaseConfig();

                    //加载数据库映射表
                    string root = AppDomain.CurrentDomain.BaseDirectory;
                    string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data/XML");
                    ObjectAssistant assistat = new ObjectAssistant();
                    try
                    {
                        if (baseconfig != null && baseconfig.DBConnectionString != "")
                        {
                            baseconfig.DBConnectionString = baseconfig.DBConnectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
                            assistat.LoadDBConnectionString(baseconfig.DBConnectionString, baseconfig.DBDriver);
                        }
                        assistat.LoadDataSource(dataPath);
                    }
                    catch (Thinkment.Data.DataException ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        string source = "CD.Utils_CDHelper_ResetApplication";
                        EventLogHelper.WriteToLog(source, ex, EventLogEntryType.Error);
                        throw ex;
                    }

                    HelperFactory hf = new HelperFactory();

                    hf.Assistant = assistat;
                    hf.Root = root;
                    hf.Initialize();
                    AppCtx.Cache.AddObject(HelperFactory.CacheKey, hf);
                }
            }
        }
    }
}
