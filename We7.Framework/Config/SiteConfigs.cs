using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using We7.Framework.Util;

namespace We7.Framework.Config
{
	/// <summary>
	/// 网站基本设置类
	/// </summary>
    public class SiteConfigs
	{
		private static object lockHelper = new object();

        private static SiteConfigInfo m_configinfo;

        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetConfig()
        {
            m_configinfo = SiteConfigFileManager.LoadRealConfig();
        }

        /// <summary>
        /// 获取当前站点配置
        /// </summary>
        /// <returns></returns>
		public static SiteConfigInfo GetConfig()
		{
            //if (m_configinfo == null)
            //{
            //    lock (lockHelper)
            //    {
            //        if (m_configinfo == null)
            //        {
            //            m_configinfo = SiteConfigFileManager.LoadConfig();
            //        }
            //    }
            //}
            //return m_configinfo;

            string configid = "siteconfig";
            SiteConfigInfo config = AppCtx.Cache.RetrieveObject<SiteConfigInfo>(configid);
            if (config == null)
            {
                config = SiteConfigFileManager.LoadConfig();
                AppCtx.Cache.AddObjectWithFileChange(configid, config, ConfigFile);
            }
            return config;
  		}

        /// <summary>
        /// 保存配置到config文件
        /// </summary>
        /// <param name="configinfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(SiteConfigInfo configinfo)
        {
            bool ret = false;
            lock (lockHelper)
            {
                ret = SerializationHelper.Save(configinfo, SiteConfigFileManager.ConfigFilePath);
                ResetConfig();
            }
            return ret;
        }

        public static bool ConfigExists()
        {
            return File.Exists(SiteConfigFileManager.ConfigFilePath);
        }

        public static string ConfigFile
        {
            get { return SiteConfigFileManager.ConfigFilePath; }
        }

		#region Helper
		/// <summary>
		/// 序列化配置信息为XML
		/// </summary>
		/// <param name="configinfo">配置信息</param>
		/// <param name="configFilePath">配置文件完整路径</param>
		public static SiteConfigInfo Serialiaze(SiteConfigInfo configinfo, string configFilePath)
		{
			lock(lockHelper) 
			{
                SerializationHelper.Save(configinfo, configFilePath);
			}
			return configinfo;
		}


		public static SiteConfigInfo Deserialize(string configFilePath)
		{
            return (SiteConfigInfo)SerializationHelper.Load(typeof(SiteConfigInfo), configFilePath);
		}

		#endregion



	}
}
