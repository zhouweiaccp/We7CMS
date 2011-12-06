using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Web;

namespace We7.CMS.Common
{
    /// <summary>
    /// 插件集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PluginInfoCollection:List<PluginInfo>
    {
        #region Fields

        private static Dictionary<PluginType,PluginInfoCollection> instance=new Dictionary<PluginType,PluginInfoCollection>();
        private PluginType pluginType;

        #endregion


        #region Constructor

        private PluginInfoCollection():this(PluginType.PLUGIN)
        {
        }

        private PluginInfoCollection(PluginType plugintype)
        {
            pluginType = plugintype;
        }

        #endregion


        #region Properties

        public PluginInfo this[string directory]
        {
            get
            {
                PluginInfo result=this.Find(delegate(PluginInfo info)
                {
                    return info.Directory == directory.Trim();
                });
                if (result == null)
                {
                    this.Load();
                    result=this.Find(delegate(PluginInfo info)
                    {
                        return info.Directory == directory.Trim();
                    });
                }
                return result;
            }
        }

        /// <summary>
        /// 插件配置信息集合的实例
        /// </summary>
        public static PluginInfoCollection CreateInstance(PluginType type)
        {
            if(!instance.ContainsKey(type))
            {
                instance.Add(type,new PluginInfoCollection(type));
            }
            return instance[type];
        }

        private string PluginBasePath
        {
            get
            {
                PluginInfo info=new PluginInfo(pluginType);
                return info.PluginsClientPath;
            }
        }

        #endregion


        #region Methods

        /// <summary>
        /// 加载每一个插件中的配置文件
        /// </summary>
        public void Load()
        {
            this.Clear();

            DirectoryInfo directory = new DirectoryInfo(PluginBasePath);

            if (!directory.Exists)
            {
                directory.Create();
                //throw new Exception("插件路径不存在!");
            }

            DirectoryInfo[] pluginDirs=directory.GetDirectories();

            foreach (DirectoryInfo dir in pluginDirs)
            {
                if (dir.Exists)
                {
                    FileInfo[] fileInfos = dir.GetFiles("Plugin.xml");
                    if (fileInfos.Length == 1)
                    {
                        this.Add(new PluginInfo(fileInfos[0].FullName,pluginType));
                    }
                }
            }
        }

        /// <summary>
        /// 从插件中移除指定目录下的内容
        /// </summary>
        /// <param name="directory"></param>
        public void Remove(string directory)
        {
            PluginInfo info = this[directory];
            if (info != null)
                Remove(info);
        }

        public void SwitchPlugin(string directory)
        {
            PluginInfo info = this[directory];
            if (info != null)
            {
                info.Enable= !info.Enable;
                info.Save();
            }
        }

        public void SavaPlugin(string directory)
        {
            PluginInfo info = this[directory];
            if (info != null)
            {
                info.Save();
            }
        }
        #endregion
    }
}
