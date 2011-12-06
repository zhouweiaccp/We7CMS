using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Cache;
using We7.Framework.Config;

namespace We7.Framework
{
    /// <summary>
    /// 系统上下文
    /// </summary>
    public static class AppCtx
    {
        private static ICacheStrategy cache;
        #region 常量
        /// <summary>
        /// 模型配置文件路径
        /// </summary>
        public const string ModelConfigPath = "~/ModelUI/Config/ModelConfig.xml";

        #endregion

        static AppCtx()
        {
            //TODO::初始化
        }

        /// <summary>
        /// 缓存
        /// </summary>
        public static ICacheStrategy Cache
        {
            get
            {
                if (cache == null)
                {
                    cache = new DefaultCacheStrategy();
                }
                return cache;
            }
        }
                
        /// <summary>
        /// 是否是演示站点
        /// </summary>
        public static bool IsDemoSite
        {
            get
            {                
                return GeneralConfigs.GetConfig().IsDemoSite;
            }
        }

    }
}
