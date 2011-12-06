using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Model.Core.Config;
using We7.Framework.Util;

namespace We7.Model.Core.UI
{
    /// <summary>
    /// 默认值生成器
    /// </summary>
    public abstract class DefaultGenerator
    {
        private const string ModelDefaultGeneratorKey = "$Model$Default$Generator$Key$";

        /// <summary>
        /// 取得默认值生成器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DefaultGenerator GetGenerator(string key)
        {
            DefaultGenerator generator = AppCtx.Cache.RetrieveObject<DefaultGenerator>(GetCacheKey(key));
            if (generator == null)
            {
                if (ModelConfig.Defaults.Get(key) == null)
                {
                    generator = new DefaultDefaultGenerator();
                }
                else
                {
                    generator = Utils.CreateInstance<DefaultGenerator>(ModelConfig.Defaults[key]);
                    AppCtx.Cache.AddObjectWithFileChange(GetCacheKey(key), generator, ModelConfig.ConfigFilePath);
                }                
            }
            return generator;
        }

        /// <summary>
        /// 取得默认值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ctx"></param>
        /// <param name="dc"></param>
        /// <returns></returns>
        public static object GetDefaultValue(string key, PanelContext ctx,We7DataColumn dc)
        {
            DefaultGenerator generator = GetGenerator(key);
            generator.Ctx = ctx;
            generator.DC = dc;
            return generator != null ? generator.Generate() : null;
        }

        /// <summary>
        /// 获取缓存关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetCacheKey(string key)
        {
            return String.Format("{0}{1}", ModelDefaultGeneratorKey, key);
        }

        /// <summary>
        /// 数据的上下文信息
        /// </summary>
        public PanelContext Ctx { get; set; }

        /// <summary>
        /// 数据列信息
        /// </summary>
        public We7DataColumn DC { get; set; }

        /// <summary>
        /// 创建默认值
        /// </summary>
        /// <returns></returns>
        public abstract object Generate();
    }

    public sealed class DefaultDefaultGenerator : DefaultGenerator
    {
        public override object Generate()
        {
            return TypeConverter.StrToObjectByTypeCode(DC.DefaultValue, DC.DataType);
        }
    }

    /// <summary>
    /// 当前时间默认值
    /// </summary>
    public sealed class DefaultNowGenerator : DefaultGenerator
    {
        public override object Generate()
        {
            return DateTime.Now;
        }
    }
}
