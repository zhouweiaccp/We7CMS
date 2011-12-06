using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Web;

using Thinkment.Data;
using We7.Framework.Util;
using We7.Framework.Cache;
using System.Web.Caching;
using System.IO;

namespace We7.Framework
{
    /// <summary>
    /// Helper助手抽象工厂类
    /// </summary>
    [Serializable]
    public class HelperFactory
    {
        Dictionary<Type, IHelper> helpers;
        ObjectAssistant assistant;
        string root;
        List<Assembly> assemblies;

        public static string ApplicationID = "We7.HelperFactory";
        public const string CacheKey = "$We7$HelperFactory$CacheKey";

        static HelperFactory()
        {
            InitDb();
        }

        static void InitDb()
        {
            string dbconfigid = "dbconfigid";
            string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Config/db.config");
            AppCtx.Cache.AddObjectWithFileChange(dbconfigid, new object(),
                 CacheItemRemovedCallback,
                configFile);
        }

        static bool IsExproe;

        static void CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            IsExproe = true;
            InitDb();
        }

        /// <summary>
        /// 获取业务助手工厂实例
        /// </summary>
        public static HelperFactory Instance
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (IsExproe)
                    {
                        HttpContext.Current.Application[HelperFactory.ApplicationID] = null;
                        IsExproe = false;
                    }
                    if (HttpContext.Current.Application[HelperFactory.ApplicationID] == null)
                    {
                        object helper = Utils.CreateInstance("We7.CMS.ApplicationHelper,We7.CMS.Install");
                        if (helper != null)
                        {
                            MethodInfo method = helper.GetType().GetMethod("ResetApplication");
                            if (method != null)
                            {
                                method.Invoke(helper, null);
                            }
                        }
                    }
                    return HttpContext.Current.Application[HelperFactory.ApplicationID] as HelperFactory;
                }
                else
                {
                    if (IsExproe)
                    {
                        AppCtx.Cache.RemoveObject(HelperFactory.CacheKey);
                        IsExproe = false;
                    }
                    HelperFactory factory = AppCtx.Cache.RetrieveObject<HelperFactory>(HelperFactory.CacheKey);
                    if (factory == null)
                    {
                        object helper = Utils.CreateInstance("We7.CMS.ApplicationHelper,We7.CMS.Install");
                        if (helper != null)
                        {
                            MethodInfo method = helper.GetType().GetMethod("LoadHelperFactory");
                            if (method != null)
                            {
                                method.Invoke(helper, null);
                            }
                        }
                    }
                    return AppCtx.Cache.RetrieveObject<HelperFactory>(HelperFactory.CacheKey);
                }
            }
        }

        /// <summary>
        /// 工厂构造
        /// </summary>
        public HelperFactory()
        {
            helpers = new Dictionary<Type, IHelper>();
            assemblies = new List<Assembly>();
        }
        /// <summary>
        /// 持久层对象
        /// </summary>
        public ObjectAssistant Assistant
        {
            get { return assistant; }
            set { assistant = value; }
        }

        public string Root
        {
            get { return root; }
            set { root = value; }
        }

        /// <summary>
        /// 初始化：加载dll中实体对象，对象字典清空
        /// </summary>
        public void Initialize()
        {
            We7Helper.AssertNotNull(Assistant, "HelperFactory.Assistant");
            helpers.Clear();
            Load();
        }

        void Load()
        {
            AppDomain ad = AppDomain.CurrentDomain;
            Assembly[] asses = ad.GetAssemblies();
            foreach (Assembly ass in asses)
            {
                ProcessAssembly(ass);
            }
        }

        /// <summary>
        /// 取得具体助手类的可用实例
        /// </summary>
        /// <typeparam name="T">类名</typeparam>
        /// <returns>可用对象</returns>
        public T GetHelper<T>()
        {
            if (helpers.ContainsKey(typeof(T)))
            {
                return (T)helpers[typeof(T)];
            }
            Load();
            if (helpers.ContainsKey(typeof(T)))
            {
                return (T)helpers[typeof(T)];
            }
            throw new Exception("没有这个类型的Helper。");
        }

        void ProcessAssembly(Assembly ass)
        {

            if (!ass.FullName.StartsWith("We7.CMS.Web") && (ass.FullName.StartsWith("We7") || ass.FullName.StartsWith("WebEngine2007")))
            {
                if (!assemblies.Contains(ass))
                {

                    assemblies.Add(ass);

                    try
                    {
                        Type[] types = ass.GetTypes();
                        foreach (Type type in types)
                        {
                            ProcessType(type);
                        }
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write(ass.FullName + "<br />");
                        //throw new Exception("获取dll“" +ass.FullName+ "”出错：" + ex.Message);
                    }
                }
            }
        }

        void ProcessType(Type type)
        {
            try
            {
                object[] objs = type.GetCustomAttributes(typeof(HelperAttribute), false);
                if (objs != null && objs.Length > 0)
                {
                    foreach (object obj in objs)
                    {
                        HelperAttribute ha = obj as HelperAttribute;
                        if (ha != null)
                        {
                            IHelper helper = Activator.CreateInstance(type) as IHelper;
                            if (helper != null)
                            {
                                helper.Assistant = Assistant;
                                helper.Name = ha.Name;
                                helper.Description = ha.Description;
                                helper.Root = Root;

                                helpers.Add(type, helper);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogHelper.SaveLogToFile(ex.Source, ex.Message);
            }
        }

    }
}
