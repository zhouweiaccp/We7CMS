using System;
using System.Web;
using System.IO;
using We7.Framework.Util;

namespace We7.CMS.Config
{
    
	/// <summary>
    /// 基本设置类
	/// </summary>
	public class BaseConfigs
	{
        private static object lockHelper = new object();

        private static BaseConfigInfo m_configinfo;

        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetConfig()
        {
            m_configinfo = BaseConfigFileManager.LoadRealConfig();
        }

		public static BaseConfigInfo GetBaseConfig()
		{
            if (m_configinfo == null)
            {
                lock (lockHelper)
                {
                    if (m_configinfo == null)
                    {
                        m_configinfo = BaseConfigFileManager.LoadRealConfig();
                    }
                }
            }
            return m_configinfo;
		}

        /// <summary>
        /// 数据库配置文件是否存在
        /// </summary>
        /// <returns></returns>
        public static bool ConfigFileExist()
        {
            BaseConfigFileManager.filename = null;
            return BaseConfigFileManager.ConfigFilePath != "";
        }
		/// <summary>
		/// 返回数据库连接串
		/// </summary>
		public static string GetDBConnectString
		{
			get
			{
				return GetBaseConfig().DBConnectionString;
			}
		}

        /// <summary>
        /// 返回网站数据库类型
        /// </summary>
        public static string GetDbType
        {
            get
            {
                return GetBaseConfig().DBType;
            }
        }

        /// <summary>
        /// 保存配置实例
        /// </summary>
        /// <param name="baseconfiginfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(BaseConfigInfo baseconfiginfo)
        {
            BaseConfigFileManager acfm = new BaseConfigFileManager();
            BaseConfigFileManager.ConfigInfo = baseconfiginfo;
            return acfm.SaveConfig();
        }

        public static bool SaveConfigTo(BaseConfigInfo baseconfiginfo,string fileName)
        {
            BaseConfigFileManager acfm = new BaseConfigFileManager();
            return acfm.SaveConfig(fileName,baseconfiginfo);
        }

        #region Helper
        /// <summary>
        /// 序列化配置信息为XML
        /// </summary>
        /// <param name="configinfo">配置信息</param>
        /// <param name="configFilePath">配置文件完整路径</param>
        public static BaseConfigInfo Serialiaze(BaseConfigInfo configinfo, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(configinfo, configFilePath);
            }
            return configinfo;
        }


        public static BaseConfigInfo Deserialize(string configFilePath)
        {
            return (BaseConfigInfo)SerializationHelper.Load(typeof(BaseConfigInfo), configFilePath);
        }

        #endregion
		
	}
}
