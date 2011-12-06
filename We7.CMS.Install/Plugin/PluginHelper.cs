using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Reflection;
using System.Web;
using We7.CMS.Config;
using Thinkment.Data;
using System.Xml;
using System.Net;
using System.Threading;
using We7.CMS.Common;
using We7.Framework.Zip;
using System.Text.RegularExpressions;

namespace We7.CMS.Plugin
{
    /// <summary>
    /// 当前执行操作
    /// </summary>
    public enum PluginAction
    {
        /// <summary>
        /// 无操作
        /// </summary>
        NULL = 0,
        /// <summary>
        /// 安装
        /// </summary>
        INSTALL,
        /// <summary>
        /// 更新
        /// </summary>
        UPDATE,
        /// <summary>
        /// 卸载
        /// </summary>
        UNSTALL,
        /// <summary>
        /// 删除
        /// </summary>
        DELETE,
        /// <summary>
        /// 远程安装
        /// </summary>
        REMOTEINSTALL,
        /// <summary>
        /// 远程更新
        /// </summary>
        REMOTEUPDATE
    }
    /// <summary>
    /// 插件的帮助文件
    /// </summary>
    public class PluginHelper
    {
        /// <summary>
        /// 当前操作的委托
        /// </summary>
        /// <param name="info">插件信息</param>
        public delegate void InstallHandler(PluginInfo info);

        /// <summary>
        /// 安装完成后的事件
        /// </summary>
        public event InstallHandler AfterInstall;
        private PluginType plugintype;
        private PluginInfo info;

        #region 远程插件信息

        private static List<RemotePluginInfo> RemotePluginInfoCollection = new List<RemotePluginInfo>();//远程插件信息
        private static DateTime LastUpdateTime, SpecialUpdateTime;
        private static PluginStatisticCollection pluginStatistics = new PluginStatisticCollection();//插件统计信息

        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pluginType">插件类型</param>
        public PluginHelper(PluginType pluginType)
        {
            plugintype = pluginType;
            info = new PluginInfo(plugintype);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PluginHelper() : this(PluginType.PLUGIN) { }

        /// <summary>
        /// 解压ZIP文件
        /// </summary>
        /// <param name="stream">解压的数据流</param>
        /// <param name="directory">解压目录</param>
        public void ExtractZipFile(Stream stream, string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            ZipUtils.ExtractZip(stream, directory);
        }

        /// <summary>
        /// 解压ZIP文件
        /// </summary>
        /// <param name="buffer">解压的字节流</param>
        /// <param name="directory">解压的目录</param>
        public void ExtractZipFile(byte[] buffer, string directory)
        {

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                ms.Position = 0;
                ZipUtils.ExtractZip(ms, directory);
            }
        }

        /// <summary>
        /// 下载插件．先压缩，再生成字节流
        /// </summary>
        /// <param name="dir">下载目录</param>
        /// <returns>下载的字节流</returns>
        public byte[] DownLoad(string[] dir)
        {
            string tempDirectory = Path.Combine(info.PluginsClientPath, "_temp");
            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory, true);

            foreach (string dr in dir)
            {
                string srcpath = Path.Combine(info.PluginsClientPath, dr);
                string targetPath = Path.Combine(tempDirectory, dr);
                if (Directory.Exists(srcpath))
                {
                    CopyDirectory(srcpath, targetPath);
                }
            }

            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                string dirpath = Path.Combine(info.PluginsClientPath, tempDirectory);
                if (Directory.Exists(dirpath))
                {
                    ZipUtils.CreateZip(dirpath, ms);
                }
                buffer = ms.ToArray();
            }

            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory, true);
            return buffer;
        }

        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="source">源目录</param>
        /// <param name="target">目标目录</param>
        public static void CopyDirectory(string source, string target)
        {
            DirectoryInfo dirsrc = new DirectoryInfo(source);
            DirectoryInfo dirtarget = new DirectoryInfo(target);

            if (!dirsrc.Exists)
                throw new Exception("源目录不存在");

            if (!dirtarget.Exists)
                dirtarget.Create();

            foreach (FileInfo f in dirsrc.GetFiles())
            {
                f.CopyTo(Path.Combine(target, f.Name), true);
            }

            foreach (DirectoryInfo dir in dirsrc.GetDirectories())
            {
                CopyDirectory(dir.FullName, Path.Combine(target, dir.Name));
            }
        }

        /// <summary>
        /// 加载远程服务器中的插件信息。如果数据量大的情况下，就不能把数据保存在内存中了。
        /// </summary>
        /// <returns>远程插件信息列表</returns>
        public List<RemotePluginInfo> LoadServerInfo()
        {
            RemotePluginInfo info = null;
            string statisticFile;
            bool isFirstTime = true;

            DirectoryInfo dirInfo = new DirectoryInfo(PluginGalleryPath);
            DirectoryInfo dirSpecial = new DirectoryInfo(Path.Combine(PluginGalleryPath, "Special"));
            if (RemotePluginInfoCollection.Count == 0 || dirInfo.LastWriteTime != LastUpdateTime || dirSpecial.LastWriteTime != SpecialUpdateTime)
            {
                lock (RemotePluginInfoCollection)
                {
                    #region 生成服务器上的插件信息

                    RemotePluginInfoCollection.Clear();

                    statisticFile = Path.Combine(PluginGalleryPath, "Statistic.xml");
                    if (File.Exists(statisticFile))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(statisticFile);
                        pluginStatistics.LoadXML(doc);
                        isFirstTime = false;
                    }
                    else
                    {
                        pluginStatistics.Clear();
                    }


                    FileInfo[] files = dirInfo.GetFiles("*.zip", SearchOption.AllDirectories);

                    PluginStatistic statistic;
                    List<string> curPluginList = new List<string>();
                    foreach (FileInfo file in files)
                    {
                        info = new RemotePluginInfo();

                        using (Stream stream = File.OpenRead(file.FullName))
                        {

                            info.LoadXml(ZipUtils.GetFileFromZip(stream, Path.Combine(Path.GetFileNameWithoutExtension(file.Name), "Plugin.xml")));
                            stream.Close();
                            stream.Dispose();
                        }

                        string pluginName = Path.GetFileNameWithoutExtension(file.Name);
                        if (isFirstTime || ((statistic = pluginStatistics[pluginName]) == null))
                        {
                            statistic = new PluginStatistic();
                            statistic.CreateTime = file.LastWriteTime;
                            statistic.UpdateTime = file.LastWriteTime;
                            statistic.PluginName = pluginName;
                            statistic.Clicks = 0;
                            pluginStatistics.Add(statistic);
                        }
                        statistic.UpdateTime = file.LastWriteTime;


                        info.UpdateTime = statistic.UpdateTime;
                        info.CreateTime = statistic.CreateTime;

                        info.Clicks = statistic.Clicks;
                        info.IsSpecial = file.DirectoryName.EndsWith("Special");

                        RemotePluginInfoCollection.Add(info);
                        curPluginList.Add(pluginName);
                    }

                    pluginStatistics.RemoveAll(delegate(PluginStatistic item)
                    {
                        return !curPluginList.Contains(item.PluginName);
                    });

                    pluginStatistics.ToXml().Save(statisticFile);

                    LastUpdateTime = dirInfo.LastWriteTime;
                    SpecialUpdateTime = dirSpecial.LastWriteTime;

                    #endregion
                }
            }

            return RemotePluginInfoCollection;
        }

        /// <summary>
        /// 增加插件的下载数
        /// </summary>
        public void AddClicks(string pluginName)
        {
            string statisticFile = Path.Combine(PluginGalleryPath, "Statistic.xml");
            pluginStatistics.LoadXML(statisticFile);
            PluginStatistic statistic = pluginStatistics[pluginName];
            if (statistic == null)
            {
                RemotePluginInfoCollection.Clear();
                LoadServerInfo();
                statistic = pluginStatistics[pluginName];
                if (statistic == null)
                    return;
            }

            statistic.Clicks++;
            RemotePluginInfo rinfo = RemotePluginInfoCollection.Find(delegate(RemotePluginInfo info)
            {
                return info.Directory == pluginName;
            });

            if (rinfo != null) rinfo.Clicks = statistic.Clicks;

            pluginStatistics.SaveXML(statisticFile);
        }

        /// <summary>
        /// 检测本地是否已经安装相关插件
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <returns>是否安装</returns>
        public bool isInstalled(string pluginName)
        {
            return Directory.Exists(Path.Combine(PluginPath, pluginName));
        }

        /// <summary>
        /// 运行客户端传来的命令
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="cmd">执行的命令</param>
        /// <param name="action">当前动作</param>
        /// <returns>执行命令的结果信息</returns>
        public string RunCommand(string pluginName, string cmd, string action)
        {
            return RunCommand(pluginName, cmd, GetAction(action));
        }


        /// <summary>
        /// 运行客户端传来的命令(资源平台使用)
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="cmd">执行的命令</param>
        /// <param name="action">当前动作</param>
        /// <returns>执行命令的结果信息</returns>
        public string RunCommand(string pluginName, string cmd, string action, string pluginPath)
        {
            return RunCommand(pluginName, cmd, GetAction(action), pluginPath);
        }


        /// <summary>
        /// 运行客户端传来的命令
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="cmd">执行的命令</param>
        /// <param name="action">当前动作</param>
        /// <returns>执行命令的结果信息</returns>
        public string RunCommand(string pluginName, string cmd, PluginAction action)
        {
            string result = "";
            IPluginCommand command = PluginCommandFactory.CreateCommand(cmd);
            PluginInfo info = GetPluginInfo(pluginName, cmd);
            if (info != null)
            {
                result = command.Run(info, action);
            }
            else if (IsTemplate(pluginName))
            {
                info = new PluginInfo(PluginType.RESOURCE);
                info.Directory = pluginName;
                result = command.Run(info, action);
            }
            else
            {

                result = new PluginJsonResult(false, "当前插件不存在").ToString();
            }
            return result;
        }

        /// <summary>
        /// 运行客户端传来的命令(资源平台使用)
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="cmd">执行的命令</param>
        /// <param name="action">当前动作</param>
        /// <returns>执行命令的结果信息</returns>
        public string RunCommand(string pluginName, string cmd, PluginAction action, string pluginPath)
        {
            string result = "";
            //IPluginCommand command = PluginCommandFactory.CreateCommand(cmd);
            ResourcePlatformPluginDownLoadCommand command = new ResourcePlatformPluginDownLoadCommand();
            PluginInfo info = GetPluginInfo(pluginName, cmd);
            //if (!File.Exists(realPath))
            //{
            //    result = new PluginJsonResult(false, "当前插件不存在").ToString();
            //    return result;
            //}
            if (info != null)
            {
                result = command.Run(info, action, pluginPath);
            }
            else
            {
                //result = new PluginJsonResult(false, "当前插件不存在").ToString();
            }
            return result;
        }

        /// <summary>
        /// 取得插件信息
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="cmd">执行的命令</param>
        /// <returns>插件信息</returns>
        private PluginInfo GetPluginInfo(string pluginName, string cmd)
        {
            PluginInfo info = null;
            if (cmd.ToLower() == "download")
            {
                info = new PluginInfo(plugintype);
                info.Directory = Path.GetFileNameWithoutExtension(pluginName);
                info.FilePath = pluginName;
            }
            else if (IsTemplate(pluginName))
            {
                info = new PluginInfo(PluginType.RESOURCE);
                info.Directory = pluginName;
            }
            else //if (pluginName.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                string dir=pluginName.StartsWith("http://", StringComparison.OrdinalIgnoreCase)?Path.GetFileNameWithoutExtension(pluginName):pluginName;
                info = PluginInfoCollection.CreateInstance(plugintype)[dir];
                if(info==null)
                {
                    info = new PluginInfo(PluginType.RESOURCE);
                    info.Directory = dir;
                    info.FilePath = pluginName;
                }
            }
            return info;
        }

        private bool IsTemplate(string pluginName)
        {
            Regex regex = new Regex(@"^Templates\.\w+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex.IsMatch(pluginName);
        }

        /// <summary>
        /// 行为
        /// </summary>
        /// <param name="action">动作名称</param>
        /// <returns>当前动作枚举</returns>
        private PluginAction GetAction(string action)
        {
            switch (action)
            {
                case "install":
                    return PluginAction.INSTALL;
                case "remoteinstall":
                    return PluginAction.REMOTEINSTALL;
                case "update":
                    return PluginAction.UPDATE;
                case "remoteupdate":
                    return PluginAction.REMOTEUPDATE;
                case "uninstall":
                    return PluginAction.UNSTALL;
                case "delete":
                    return PluginAction.DELETE;
            }
            return PluginAction.NULL;
        }

        /// <summary>
        /// 抽取临时截图
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        public void PickUpSnapshot(FileInfo file, RemotePluginInfo info)
        {
            string path = Path.Combine(PluginGalleryPath, "Temp");
            string pluginPath = Path.Combine(path, Path.GetFileNameWithoutExtension(file.FullName));
            if (Directory.Exists(pluginPath))
                Directory.Delete(pluginPath, true);
            Directory.CreateDirectory(path);

            if (!String.IsNullOrEmpty(info.Thumbnail))
                CreateTempFile(file.FullName, info.Thumbnail, path);

            foreach (string str in info.Snapshot)
            {
                CreateTempFile(file.FullName, str, path);
            }
        }

        /// <summary>
        /// 提取插件的临时文件
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        public void PickUpTempFile(string pluginName)
        {
            RemotePluginInfo plugin = LoadServerInfo().Find(delegate(RemotePluginInfo info)
            {
                return pluginName == info.Directory;
            });

            if (plugin != null)
            {
                string fileName = Path.Combine(PluginGalleryPath, pluginName + ".zip");
                if (!File.Exists(fileName))
                    fileName = Path.Combine(PluginGalleryPath, "Special/" + pluginName + ".zip");

                FileInfo file = new FileInfo(fileName);

                if (file.Exists && file.LastWriteTime != plugin.UpdateTime || !CheckPluginTempFile(pluginName))
                {
                    PickUpSnapshot(new FileInfo(fileName), plugin);
                }
            }
        }


        /// <summary>
        /// 创建临时文件
        /// </summary>
        /// <param name="zipFile">要解压的文件名称</param>
        /// <param name="filePath">要解压的路径</param>
        /// <param name="directory">文件目录</param>
        private void CreateTempFile(string zipFile, string filePath, string directory)
        {
            filePath = Path.Combine(Path.GetFileNameWithoutExtension(zipFile), filePath);
            using (FileStream stream = File.OpenRead(zipFile))
            {
                if (stream != null)
                {
                    using (Stream zipstream = ZipUtils.GetFileFromZip(stream, filePath))
                    {
                        if (zipstream != null)
                        {
                            filePath = Path.Combine(directory, filePath);
                            string dirPath = Path.GetDirectoryName(filePath);
                            if (!Directory.Exists(dirPath))
                                Directory.CreateDirectory(dirPath);

                            using (FileStream fs = File.Open(filePath, FileMode.Create, FileAccess.Write))
                            {
                                byte[] buffer = new byte[0x1000];
                                int num = 0;
                                while ((num = zipstream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    fs.Write(buffer, 0, num);
                                }
                                fs.Close();
                                fs.Dispose();
                            }
                            zipstream.Close();
                            zipstream.Dispose();
                        }
                    }
                    stream.Close();
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// 检测临时插件是否存在。
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        public bool CheckPluginTempFile(string pluginName)
        {
            string path = Path.Combine(PluginGalleryPath, "Temp");
            path = Path.Combine(path, pluginName);
            return Directory.Exists(path);
        }

        /// <summary>
        /// 插件路径
        /// </summary>
        private string PluginPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, info.PluginsClientPath);
            }
        }

        /// <summary>
        /// 插件在服务器上存放的目录名称
        /// </summary>
        private string PluginGalleryPath
        {
            get
            {
                return info.PluginLocalGalleryPath;
            }
        }
    }
}

