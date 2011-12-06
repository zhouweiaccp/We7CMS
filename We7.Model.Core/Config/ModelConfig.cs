using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Util;
using We7.Model.Core.Data;
using We7.Model.Core.Entity;

namespace We7.Model.Core.Config
{
    /// <summary>
    /// 内容模型配置
    /// </summary>
    public class ModelConfig
    {
        /// <summary>
        /// 锁变量
        /// </summary>
        private static object lockHelper = new object();
        private static string modelconfigpath, defaultModelFile, modelIndexFile, modelGroupIndexFile;

        /// <summary>
        /// 数据库帮助类
        /// </summary>
        public static string DataBaseHelper
        {
            get { return GetConfig().DataBaseHelper; }
        }
        /// <summary>
        /// 内容模型根目录
        /// </summary>
        public static string BaseDirectory
        {
            get
            {
                return GetConfig().BaseDirectory;
            }
        }

        /// <summary>
        /// 模型文件存放路径
        /// </summary>
        public static string ModelsDirectory
        {
            get { return GetConfig().ModelsDirectory.TrimEnd('/'); }
        }

        /// <summary>
        /// 内容核心配置根目录
        /// </summary>
        public static string BaseModelDirectory
        {
            get { return GetConfig().BaseModelDirectory.TrimEnd('/'); }
        }

        /// <summary>
        /// 模型控件模板路径
        /// </summary>
        public static string ModelControlTemplatePath
        {
            get { return Utils.GetMapPath(BaseModelDirectory + "/GeneratorTemplate/"); }
        }

        /// <summary>
        /// 配置文件的绝对路径
        /// </summary>
        public static string ConfigFilePath
        {
            get
            {
                if (String.IsNullOrEmpty(modelconfigpath))
                {
                    modelconfigpath = Utils.GetMapPath(AppCtx.ModelConfigPath);
                }
                return modelconfigpath;
            }
        }

        public static string DefaultModelFile
        {
            get
            {
                if (String.IsNullOrEmpty(defaultModelFile))
                {
                    defaultModelFile = Utils.GetMapPath(GetConfig().DefaultModelFile);
                }
                return defaultModelFile;
            }
        }

        public static string ModelIndexFile
        {
            get
            {
                if (String.IsNullOrEmpty(modelIndexFile))
                {
                    modelIndexFile = Utils.GetMapPath(GetConfig().ModelIndexFile);
                }
                return modelIndexFile;
            }
        }

        /// <summary>
        /// 内容模型组索引文件路径
        /// </summary>
        public static string ModelGroupIndexFile
        {
            get
            {
                if (String.IsNullOrEmpty(modelGroupIndexFile))
                {
                    modelGroupIndexFile = Utils.GetMapPath(GetConfig().ModelGroupIndexFile);
                }
                return modelGroupIndexFile;
            }
        }

        /// <summary>
        /// 控件根目录
        /// </summary>
        public static string ControlsDirectory
        {
            get
            {
                return GetConfig().ControlsDirectory;
            }
        }

        /// <summary>
        /// 控件根目录
        /// </summary>
        public static string ContainerDirectory
        {
            get
            {
                return GetConfig().ContainerDirectory;
            }
        }

        /// <summary>
        /// 模型控件路径
        /// </summary>
        public static string ModelControlsIndex
        {
            get { return GetConfig().ModelControlsIndex; }
        }

        /// <summary>
        /// 数据库映射路径
        /// </summary>
        public static string CDPath
        {
            get { return GetConfig().CDPath; }
        }

        /// <summary>
        /// 模型前台控件路径
        /// </summary>
        public static string ModelUCConfigTemplate
        {
            get { return GetConfig().ModelUCConfigTemplate; }
        }

        /// <summary>
        /// 图片上传根目录
        /// </summary>
        public static string ImageFolder
        {
            get { return GetConfig().ImageFolder.Trim('/'); }
        }

        /// <summary>
        /// 是否生成文章控件
        /// </summary>
        public static bool IsCreateArticleUC
        {
            get { return GetConfig().IsCreateArticleUC; }
        }

        /// <summary>
        /// 命令集合
        /// </summary>
        public static NameTypeCollection Commands
        {
            get { return GetConfig().Commands; }
        }

        /// <summary>
        /// 命令转化器
        /// </summary>
        public static NameTypeCollection Converters
        {
            get { return GetConfig().Converters; }
        }

        /// <summary>
        /// 内容模型默认值
        /// </summary>
        public static NameTypeCollection Defaults
        {
            get { return GetConfig().Defaults; }
        }

        /// <summary>
        /// 表表控件
        /// </summary>
        public static NameTypeCollection ListControls
        {
            get { return GetConfig().ListControls; }
        }

        public static NameTypeCollection ColumnConvert
        {
            get { return GetConfig().ColumnConvert; }
        }

        /// <summary>
        /// 列表命令
        /// </summary>
        public static NameValueCollection ListCommands
        {
            get { return GetConfig().ListCommands; }
        }

        internal static DbProviderInfo GetDbProvider(ModelType Type)
        {
            DbProviderInfo provider = GetConfig().Providers[Type];
            if (provider == null || String.IsNullOrEmpty(provider.Provider))
            {
                throw new Exception("不存在" + Type + "驱动信息");
            }
            return provider;
        }

        /// <summary>
        /// 获取聚合配置类实例
        /// </summary>
        /// <returns></returns>
        public static ModelConfigInfo GetConfig()
        {
            ModelConfigInfo cfg = AppCtx.Cache.RetrieveObject<ModelConfigInfo>("___ModelConfigInfo___zs4wf50j4dfkxh__");
            if (cfg == null)
            {
                ModelConfigFileManager.ConfigFilePath = ConfigFilePath;
                cfg = ModelConfigFileManager.LoadConfig();
                if (cfg == null)
                {
                    throw new SysException("获取ModelCofnig失败");
                }
                AppCtx.Cache.AddObjectWithFileChange("___ModelConfigInfo___zs4wf50j4dfkxh__", cfg, ModelConfig.ConfigFilePath);
            }
            return cfg;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(ModelConfigInfo modelconfiginfo)
        {
            ModelConfigFileManager mcfm = new ModelConfigFileManager();
            ModelConfigFileManager.ConfigInfo = modelconfiginfo;
            return mcfm.SaveConfig();
        }
    }
}
