using System;
using System.Text;
using System.Web;
using System.IO;

using System.Xml.Serialization;
using System.Xml;


namespace We7.Framework.Config
{
    /// <summary>
    /// 网站基本设置管理类
    /// </summary>
    class SiteConfigFileManager :DefaultConfigFileManager
    {
        private static SiteConfigInfo m_configinfo;

        /// <summary>
        /// 锁对象
        /// </summary>
        private static object m_lockHelper = new object();

        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;

        /// <summary>
        /// 当前配置类的实例
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (SiteConfigInfo) value; }
        }

        /// <summary>
        /// 配置文件所在路径
        /// </summary>
        public static string filename = null;


        /// <summary>
        /// 获取配置文件所在路径
        /// </summary>
        public new static string ConfigFilePath
        {
            get
            {
                if (filename == null)
                {
                    HttpContext context = HttpContext.Current;
                    if (context != null)
                    {
                        filename = context.Server.MapPath("~/Config/site.config");
                    }
                    else
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory;
                        if (path.ToLower().EndsWith("bin"))
                        {
                            path = path.Substring(0, path.Length - 4);
                        }
                        filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config/site.config");
                    }

                    if (!File.Exists(filename))
                    {
                        //throw new WebEngineException("发生错误: Config目录下没有正确的site.config文件");
                        filename = "";
                    }

                }

                return filename;
            }

        }

        /// <summary>
        /// 加载真正有效的配置类
        /// </summary>
        /// <returns></returns>
        public static SiteConfigInfo LoadRealConfig()
        {
            if (ConfigFilePath != "")
            {
                lock (m_lockHelper)
                {
                    ConfigInfo = DeserializeInfo(ConfigFilePath, typeof(SiteConfigInfo));
                }
            }
            return ConfigInfo as SiteConfigInfo;
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static SiteConfigInfo LoadConfig()
        {
            try
            {
                if (ConfigInfo != null)
                {
                    m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
                    ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo, true);
                }
                else
                {
                    filename = HttpContext.Current.Server.MapPath("~/Config/site.config");
                    ConfigInfo = new SiteConfigInfo();
                    ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo, false);
                }
                return ConfigInfo as SiteConfigInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public override bool SaveConfig()
        {
            return base.SaveConfig(ConfigFilePath, ConfigInfo);
        }
    }
}

