using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Web;
using We7.Framework.Util;

namespace We7.Framework.Config
{
	/// <summary>
	/// 网站基本设置类
	/// </summary>
    public class GeneralConfigs
	{
		private static object lockHelper = new object();

        private static GeneralConfigInfo m_configinfo;

        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetConfig()
        {
            m_configinfo = GeneralConfigFileManager.LoadRealConfig();
        }

        /// <summary>
        /// 获取当前系统常用参数
        /// </summary>
        /// <returns></returns>
		public static GeneralConfigInfo GetConfig()
		{
            string configid = "generalconfig";
            GeneralConfigInfo config = AppCtx.Cache.RetrieveObject<GeneralConfigInfo>(configid);
            if (config == null)
            {
                config = GeneralConfigFileManager.LoadConfig();
                AppCtx.Cache.AddObjectWithFileChange(configid, config, ConfigFile);
            }
            return config;
        }

        /// <summary>
        /// 保存配置到config文件
        /// </summary>
        /// <param name="configinfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(GeneralConfigInfo configinfo)
        {
            bool ret = false;
            lock (lockHelper)
            {
                ret=SerializationHelper.Save(configinfo, GeneralConfigFileManager.ConfigFilePath);
                ResetConfig();
            }
            return ret;
        }

        public static string ConfigFile
        {
            get { return GeneralConfigFileManager.ConfigFilePath; }
        }

		#region Helper
		/// <summary>
		/// 序列化配置信息为XML
		/// </summary>
		/// <param name="configinfo">配置信息</param>
		/// <param name="configFilePath">配置文件完整路径</param>
		public static GeneralConfigInfo Serialiaze(GeneralConfigInfo configinfo, string configFilePath)
		{
			lock(lockHelper) 
			{
                SerializationHelper.Save(configinfo, configFilePath);
			}
			return configinfo;
		}


		public static GeneralConfigInfo Deserialize(string configFilePath)
		{
            return (GeneralConfigInfo)SerializationHelper.Load(typeof(GeneralConfigInfo), configFilePath);
		}

		#endregion



	}
}
