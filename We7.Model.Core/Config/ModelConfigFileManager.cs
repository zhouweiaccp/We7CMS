using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.Framework;

namespace We7.Model.Core.Config
{
    /// <summary>
    /// 内容模型文件管理
    /// </summary>
    public class ModelConfigFileManager:DefaultConfigFileManager
    {
        //TODO::当内容模型目录结构发生变化时在这里设置基础配置文件目录。
        private static string modelconfigpath;//=Utils.GetMapPath(Context.ModelConfigPath);// "~/Config/ContentModel/ModelConfig.xml";
        private static ModelConfigInfo m_configinfo ;

        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;

        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static ModelConfigFileManager()
        {
            //m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
            //m_configinfo = (ModelConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(ModelConfigInfo));
        }

        /// <summary>
        /// 当前的配置实例对象
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (ModelConfigInfo)value; }
        }

        /// <summary>
        /// 模板配置文件路径
        /// </summary>
        public new static string ConfigFilePath
        {
            get { return modelconfigpath; }
            set { modelconfigpath = value; }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static ModelConfigInfo LoadConfig()
        {
            if (ConfigInfo == null)
            {
                m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
                m_configinfo = (ModelConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(ModelConfigInfo));
            }
            else
            {
                ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
            }
            return ConfigInfo as ModelConfigInfo;
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
