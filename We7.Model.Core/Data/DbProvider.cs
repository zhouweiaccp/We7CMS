using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using We7.Model.Core.Config;
using We7.Framework.Util;
using We7.Framework;
using We7.CMS.Config;
using We7.Model.Core.Entity;
using We7.Framework.Config;

namespace We7.Model.Core.Data
{

    public class DbProvider
    {
        /// <summary>
        /// 实例化当前数据驱动
        /// </summary>
        /// <param name="type">驱动类型名</param>
        /// <returns>当前的驱动</returns>
        public static IDbProvider Instance(ModelType type)
        {
            DbProviderInfo providerInfo = ModelConfig.GetDbProvider(type);
            string typename=providerInfo.Provider;
            IDbProvider provider = AppCtx.Cache.RetrieveObject<IDbProvider>(typename);
            if (provider==null)
            {
                string[] s = typename.Split(',');
                provider =Utils.CreateInstance<IDbProvider>(s[0],s[1]);

                if (provider == null)
                    throw new SystemException("不存在当前数据驱动:" + typename);

                if (provider is CompositeProvider)
                {
                    CompositeProvider cmp = provider as CompositeProvider;
                    foreach(NameType item in providerInfo.Items)
                    {
                        if (String.Compare("singletable", item.Name, true) == 0 && !GeneralConfigs.GetConfig().EnableSingleTable)
                            continue;

                        IDbProvider subProvider=AppCtx.Cache.RetrieveObject<IDbProvider>(item.Type);
                        if (subProvider == null)
                        {
                            subProvider = Utils.CreateInstance<IDbProvider>(item.Type);
                            if (subProvider != null)
                            {
                                AppCtx.Cache.AddObjectWithFileChange(item.Type, subProvider, ModelConfig.ConfigFilePath, GeneralConfigs.ConfigFile);
                            }
                        }
                        if(subProvider!=null)
                            cmp.Add(subProvider);
                    }
                }
                AppCtx.Cache.AddObjectWithFileChange(typename, provider, ModelConfig.ConfigFilePath, GeneralConfigs.ConfigFile);
            }
            return provider;
        }
    }
}
