using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Util;
using System.Web.Caching;
using System.Xml;
using System.IO;

namespace We7.Framework.Cache
{
    public class CacheRecord
    {

        private Type recordType;
        private string preCacheKey;
        private List<string> cacheKeyList = new List<string>();
        private static Dictionary<string, CacheRecord> records = new Dictionary<string, CacheRecord>();
        private int defaultTimeout;

        private CacheRecord(Type type)
        {
            recordType = type;
            preCacheKey = String.Format("$CacheRecord${0}$", type.FullName.Replace(".", "$"));
        }

        private CacheRecord(string key)
        {
            preCacheKey = String.Format("$CacheRecord${0}$", key.Replace(".", "$"));
            defaultTimeout = CacheConfig.Instance.GetDefaultTimeout(key);
        }

        /// <summary>
        /// 创建当前类型的缓存记录
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static CacheRecord Create(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("记录类型不能为空");

            return Create(type.FullName);
        }

        /// <summary>
        /// 创建当前类型的缓存记录
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static CacheRecord Create(object o)
        {
            if (o == null)
                throw new ArgumentNullException("记录的类型对象不能为空");
            return Create(o.GetType());
        }

        /// <summary>
        /// 创建当前类型的缓存记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static CacheRecord Create(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("记录的关键字不能为空");

            key = key.ToLower();
            if (!records.ContainsKey(key))
            {
                records.Add(key, new CacheRecord(key));
            }
            return records[key];
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="keyword">缓存关键字</param>
        /// <param name="handler">生成缓存数据的委托</param>
        /// <returns></returns>
        public T GetInstance<T>(string keyword, CreateInstanceHandler<T> handler)
            where T : class
        {
            keyword = preCacheKey + keyword;

            if (!cacheKeyList.Contains(keyword))
            {
                cacheKeyList.Add(keyword);
            }

            if (defaultTimeout == -1)
            {
                return handler != null ? handler() : default(T);
            }
            else if (defaultTimeout == 0)
            {
                return We7Utils.GetCacheInstance<T>(keyword, handler);
            }
            else
            {
                return We7Utils.GetCacheInstance<T>(keyword, handler, defaultTimeout);
            }
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="keyword">缓存关键字</param>
        /// <param name="handler">生成缓存数据的委托</param>
        /// <returns></returns>
        public T GetInstance<T>(string keyword, CreateInstanceHandler<T> handler, int timeout)
            where T : class
        {
            keyword = preCacheKey + keyword;

            if (!cacheKeyList.Contains(keyword))
            {
                cacheKeyList.Add(keyword);
            }

            return We7Utils.GetCacheInstance<T>(keyword, handler, timeout);
        }

        /// <summary>
        /// 获取文件依赖的缓存数据
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="keyword">缓存关键字</param>
        /// <param name="handler">生成缓存数据的委托</param>
        /// <param name="filePath">文件依赖的路径</param>
        /// <returns></returns>
        public T GetInstance<T>(string keyword, CreateInstanceHandler2<T> handler, params string[] filePath)
            where T : class
        {
            keyword = preCacheKey + keyword;

            if (!cacheKeyList.Contains(keyword))
            {
                cacheKeyList.Add(keyword);
            }

            return We7Utils.GetCacheInstance<T>(keyword, handler, filePath);
        }

        /// <summary>
        /// 释放缓存
        /// </summary>
        public void Release()
        {
            foreach (string key in cacheKeyList)
            {
                AppCtx.Cache.RemoveObject(key);
            }
            cacheKeyList.Clear();
        }
    }

    /// <summary>
    /// 缓存配置信息
    /// </summary>
    public class CacheConfig
    {
        private const string CacheConfigKey = "We7$Framework$Cache$CacheConfig";

        /// <summary>
        /// 当前对象实例
        /// </summary>
        public static CacheConfig Instance
        {
            get
            {
                CacheConfig obj = AppCtx.Cache.RetrieveObject<CacheConfig>(CacheConfigKey);
                if (obj == null)
                {
                    obj = new CacheConfig();
                    AppCtx.Cache.AddObjectWithFileChange(CacheConfigKey, obj, delegate(string key, object value, CacheItemRemovedReason reason)
                    {
                        CacheConfig config = value as CacheConfig;
                        if (config != null)
                        {
                            foreach (KeyValuePair<string, int> kvp in config.Items)
                            {
                                CacheRecord.Create(kvp.Key).Release();
                            }
                        }
                    }, We7Utils.GetMapPath("/Config/Cache.config"));
                }
                return obj;
            }
        }

        private CacheConfig()
        {
            Init();
        }

        /// <summary>
        /// 当前记录的值信息
        /// </summary>
        public Dictionary<string, int> Items = new Dictionary<string, int>();

        /// <summary>
        /// 当前记录的标签信息
        /// </summary>
        public Dictionary<string, string> Labels = new Dictionary<string, string>();

        /// <summary>
        /// 根据关键字取得默认值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetDefaultTimeout(string key)
        {
            return Items.ContainsKey(key) ? Items[key] : -1;
        }

        void Init()
        {
            string path = We7Utils.GetMapPath("/Config/Cache.config");
            if (File.Exists(path))
            {
                XmlNodeList list = XmlHelper.GetXmlNodeList(path, "//item");
                if (list != null)
                {
                    foreach (XmlElement xe in list)
                    {
                        string key = (xe.GetAttribute("name") ?? String.Empty).ToLower();
                        int val;
                        Int32.TryParse(xe.GetAttribute("value"), out val);
                        Items.Add(key, val);
                        Labels.Add(key, xe.GetAttribute("label"));
                    }
                }
            }
        }
    }
}
