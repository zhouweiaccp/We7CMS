using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Module.VisualTemplate.Models;
using We7.Framework.Util;
using System.IO;
using We7.CMS.Common;
using System.Xml;
using System.Web;

namespace We7.CMS.Module.VisualTemplate.Helpers
{
    /// <summary>
    /// Widget操作
    /// </summary>
    public class WidgetHelper
    {
        /// <summary>
        /// 根据配置文件路径 获取一个Widget大分类
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public WidgetCollection GetWidgetCollection(string file)
        {
          if(!FileHelper.Exists(file))
          {
            throw new ArgumentException("文件不存在");
          }
            try
            {
               return SerializationHelper.Load<WidgetCollection>(file);
            }
            catch
            {
                throw new Exception("反序列化出错!");
            }
        }

        /// <summary>
        /// 根据文件路径取得部件信息
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public DataControl GetDataControl(string fileName)
        {
            DataControlInfo info = GetDataControlInfoByPath(fileName);
            string ctr = GetControlName(fileName);
            if (info.Controls != null)
            {
                return info.Controls.Find(delegate(DataControl dc)
                {
                    return dc.Control == ctr;
                });
            }
            return null;
        }

        /// <summary>
        /// 取得部件完整信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public DataControlInfo GetDataControlInfoByPath(string filePath)
        {
            DataControlInfo info = new DataControlInfo(Constants.We7Widget);
            info.Load(GetDataControlPathFromPath(filePath));
            return info;
        }

        /// <summary>
        /// 取得部件的物理目录
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetDataControlPathFromPath(string filePath)
        {
            filePath = HttpContext.Current.Server.MapPath(filePath);
            FileInfo fi = new FileInfo(filePath);
            return fi.Directory.Parent.FullName;
        }

        public string GetDataControlPath(string filePath)
        {
            filePath = HttpContext.Current.Server.MapPath(filePath);

            return filePath;
        }
        /// <summary>
        /// 取得控件名称
        /// </summary>
        /// <param name="filePath">控件文件路径</param>
        /// <returns></returns>
        public string GetControlName(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// 测试序列化生成
        /// </summary>
        /// <param name="file"></param>
        public static void CreateTestConfigFile(string file)
        {

            WidgetCollection wdCollection = new WidgetCollection();
            wdCollection.Name = "Test";
            wdCollection.Label = "测试";
            wdCollection.Description = "描述";

            for (int i = 0; i < 4; i++)
            {
                WidgetGroup wdGroup = new WidgetGroup();
                wdGroup.Name = "group" + i.ToString();
                wdGroup.Label = "组" + i.ToString();
                wdGroup.Description = "描述" + i.ToString();
                for (int j = 0; j < 5; j++)
                {
                    Widget wd = new Widget();
                    wd.Name = "Name" + j.ToString();
                    wd.Label = "Label" + j.ToString();
                    wd.Icon = "ICOn" + j.ToString();
                    wd.File = "File" + j.ToString();
                    wd.DefaultType = "Name1";
                    wd.Description = "description1";

                    for (int k = 0; k < 3; k++)
                    {
                        WidgetType wdType = new WidgetType();
                        wdType.Name = "Name" + k.ToString();
                        wdType.Label = "Label" + k.ToString();
                        wd.Types.Add(wdType);
                    }
                    wdGroup.Widgets.Add(wd);

                }
                wdCollection.Groups.Add(wdGroup);

                SerializationHelper.Save(wdCollection, file);
            }       
           
            
        }

        /// <summary>
        /// 创建部件索引
        /// </summary>
        public void CreateWidegetsIndex()
        {
            string path = Path.Combine(Constants.We7WidgetsPhysicalFolder, "WidgetsIndex.xml");
            List<DataControlInfo> ctrs = GetControls();
            CreateWidgetConfig(ctrs, path);
        }

        /// <summary>
        /// 创建部件配置
        /// </summary>
        /// <param name="dcs"></param>
        /// <returns></returns>
        void CreateWidgetConfig(List<DataControlInfo> dcs,string filePath)
        {
            WidgetCollection wdCollection = new WidgetCollection();
            wdCollection.Name = "default";
            wdCollection.Label = "部件列表信息";
            wdCollection.Description = "部件列表信息";

            Dictionary<string, WidgetGroup> groups = new Dictionary<string, WidgetGroup>();
            foreach (DataControlInfo dci in dcs)
            {
                WidgetGroup group;
                if (!groups.ContainsKey(dci.Group))
                {
                    group = new WidgetGroup();
                    group.Description = dci.Group;
                    group.Label = dci.Group;
                    group.Name = dci.Group;
                    groups.Add(dci.Group, group);
                    wdCollection.Groups.Add(group);
                }
                else
                {
                    group = groups[dci.Group];
                }

                Widget wd = new Widget();
                wd.Name = dci.Directory;
                wd.Label = dci.Name;
                wd.Icon = dci.Desc;
                wd.File = dci.Path;
                wd.Description = dci.Desc;

                foreach (DataControl dc in dci.Controls)
                {
                    WidgetType wdType = new WidgetType();
                    wdType.Name = dc.Control;
                    wdType.Label = dc.Name;
                    wdType.File = dc.FileName;
                    wd.Types.Add(wdType);
                }
                group.Widgets.Add(wd);
            }
            SerializationHelper.Save(wdCollection, filePath);
        }

        /// <summary>
        /// 取得所有控件信息
        /// </summary>
        /// <returns></returns>
        public List<DataControlInfo> GetControls()
        {
            List<DataControlInfo> lstctrs = new List<DataControlInfo>();
            LoadDataControls(lstctrs, Constants.We7WidgetsFileFolder, "系统部件");
            LoadModelsControls(lstctrs);
            LoadPluginContrls(lstctrs);
            return lstctrs;
        }

        private void LoadModelsControls(List<DataControlInfo> controls)
        {
            DirectoryInfo di = new DirectoryInfo(Constants.We7ModelPhysicalPath);
            if (di.Exists)
            {
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    LoadDataControls(controls, dir.FullName, "模型部件");
                }
            }
        }

        /// <summary>
        /// 加载插件控件
        /// </summary>
        /// <param name="controls"></param>
        private void LoadPluginContrls(List<DataControlInfo> controls)
        {
            DirectoryInfo di = new DirectoryInfo(Constants.We7PluginPhysicalPath);
            if (di.Exists)
            {
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    LoadDataControls(controls, Path.Combine(dir.FullName, "Control"), "插件部件");
                }
            }
        }

        /// <summary>
        /// 取得指定目录下的控件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private void LoadDataControls(List<DataControlInfo> controls, string dir, string group)
        {
            DirectoryInfo di = new DirectoryInfo(dir);

            if (!di.Exists)
                return;

            DirectoryInfo[] cdi = di.GetDirectories();
            foreach (DirectoryInfo d in cdi)
            {
                try
                {
                    FileInfo[] fs = d.GetFiles(Constants.We7Widget);
                    if (fs != null && fs.Length > 0)
                    {
                        DataControlInfo dci = DataControlInfo.LoadInfo(d.FullName,Constants.We7Widget);// GetDataControlInfo(d.Name);
                        if (dci != null)
                        {
                            dci.Group = group;
                            if (dci.Controls.Count > 0)
                            {
                                controls.Add(dci);
                            }
                        }
                    }
                    else
                    {
                        LoadDataControls(controls, d.FullName, group);
                    }
                }
                catch { }
            }
        }
    }
}
