using System;
using System.Web;
using System.Web.Hosting;
using System.Reflection;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Xml.Serialization;
using System.Web.Configuration;
using We7.CMS;

/// <summary>
/// Plugin 管理器 
/// 装载插件对象，并初始化插件对象的事件等
/// </summary>
[XmlRoot]
public static class PluginManager
{
    static List<Assembly> LoadPluginAssemblies()
    {
        AppDomain ad = AppDomain.CurrentDomain;
        Assembly[] asses = ad.GetAssemblies();
       List<Assembly> assemblies=new List<Assembly>();
        foreach (Assembly ass in asses)
        {
            if (ass.FullName.StartsWith("We7.Plugin") )
            {
                if (!assemblies.Contains(ass))
                {
                    assemblies.Add(ass);
                }
            }
        }
        return assemblies;
    }

    /// <summary>
    /// 装载插件，并实例化插件对象
    /// </summary>
    public static void LoadPlugins()
    {
        List<Assembly> assemblies = LoadPluginAssemblies();
        List<SortedPlugin> sortedPlugins = new List<SortedPlugin>();

        foreach (Assembly a in assemblies)
        {
            Type[] types = a.GetTypes();
            foreach (Type type in types)
            {
                object[] attributes = type.GetCustomAttributes(typeof(PluginAttribute), false);
                foreach (object attribute in attributes)
                {
                    if (attribute.GetType().Name == "PluginAttribute")
                    {
                        PluginAttribute ext = (PluginAttribute)attribute;
                        sortedPlugins.Add(new SortedPlugin(ext.Priority, type.Name, type.FullName));
                    }
                }
            }

            sortedPlugins.Sort(delegate(SortedPlugin e1, SortedPlugin e2)
            {
                if (e1.Priority == e2.Priority)
                    return string.CompareOrdinal(e1.Name, e2.Name);
                return e1.Priority.CompareTo(e2.Priority);
            });

            foreach (SortedPlugin x in sortedPlugins)
            {
                a.CreateInstance(x.Type);
            }
        }
    }

}