using System;
using System.Text;
using System.Web;
using System.IO;
using We7;
using We7.Framework.Config;

namespace We7.CMS.Config
{

    /// <summary>
    /// 基本设置信息管理类
    /// </summary>
    class BaseConfigFileManager : DefaultConfigFileManager
    {
        private static  BaseConfigInfo m_configinfo ;

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
            set { m_configinfo = (BaseConfigInfo)value; }
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
                if (filename == null || filename=="")
                {
                    HttpContext context = HttpContext.Current;
                    if (context != null)
                    {
                        filename = context.Server.MapPath("~/Config/db.config");
                    }
                    else
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory;
                        if (path.ToLower().EndsWith("bin"))
                        {
                            path = path.Substring(0, path.Length - 4);
                        }
                        filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config/db.config");
                    }

                    if (!File.Exists(filename))
                    {
                        //throw new WebEngineException("发生错误: Config目录下没有正确的db.config文件");
                        filename = "";
                    }
                }
                return filename;
            }

        }

        /// <summary>
        /// 加载配置类
        /// </summary>
        /// <returns></returns>
        public static BaseConfigInfo LoadConfig()
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
                    filename = HttpContext.Current.Server.MapPath("~/Config/db.config");
                    ConfigInfo = new BaseConfigInfo();
                    ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo, false);
                }
                return ConfigInfo as BaseConfigInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载真正有效的配置类
        /// </summary>
        /// <returns></returns>
        public static BaseConfigInfo LoadRealConfig()
        {
            if (ConfigFilePath != "")
            {
                lock (m_lockHelper)
                {
                    ConfigInfo = DeserializeInfo(ConfigFilePath, typeof(BaseConfigInfo));
                }
            }
            return ConfigInfo as BaseConfigInfo;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <returns></returns>
        public override bool SaveConfig()
        {
            return base.SaveConfig(ConfigFilePath, ConfigInfo);
        }
    }
}
