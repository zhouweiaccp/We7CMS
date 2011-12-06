using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Util;
using System.Web;

namespace We7.CMS.Helpers
{
        /// <summary>
    /// 控件业务类
    /// </summary>
    [Serializable]
    [Helper("We7.WidgetHelper")]
    public class WidgetHelper : BaseHelper
    {

        //#region 扩展方法创建部件索引
        ///// <summary>
        ///// 创建部件索引
        ///// </summary>
        //public void CreateWidegetsIndex()
        //{
        //    string path = Path.Combine(Constants.We7WidgetsPhysicalFolder, "WidgetsIndex.xml");
        //    List<DataControlInfo> ctrs = GetWidgetControls("/Widgets");
        //    CreateWidgetConfig(ctrs, path);
        //}


        ///// <summary>
        ///// 创建部件配置
        ///// </summary>
        ///// <param name="dcs"></param>
        ///// <returns></returns>
        //private void CreateWidgetConfig(List<DataControlInfo> dcs, string filePath)
        //{
        //    WidgetCollection wdCollection = new WidgetCollection();
        //    wdCollection.Name = "default";
        //    wdCollection.Label = "部件列表信息";
        //    wdCollection.Description = "部件列表信息";

        //    Dictionary<string, WidgetGroup> groups = new Dictionary<string, WidgetGroup>();
        //    foreach (DataControlInfo dci in dcs)
        //    {
        //        if (dci.Controls.Count == 0)
        //            continue;
        //        WidgetGroup group;
        //        if (!groups.ContainsKey(dci.Group))
        //        {
        //            group = new WidgetGroup();
        //            group.Description = dci.Group;
        //            group.Label = dci.Group;
        //            group.Name = dci.Group;
        //            groups.Add(dci.Group, group);
        //            wdCollection.Groups.Add(group);
        //        }
        //        else
        //        {
        //            group = groups[dci.Group];
        //        }

        //        Widget wd = new Widget();
        //        wd.Name = dci.Name;
        //        wd.Label = dci.GroupLabel;
        //        wd.Icon = dci.GroupIcon;
        //        wd.File = dci.Path;
        //        wd.Description = dci.GroupDesc;
        //        wd.DefaultType = dci.GroupDefaultType;

        //        foreach (DataControl dc in dci.Controls)
        //        {
        //            WidgetType wdType = new WidgetType();
        //            wdType.Name = dc.Name;
        //            wdType.File = dc.FileName;
        //            wdType.Label = dc.Description;
        //            wd.Types.Add(wdType);
        //        }
        //        group.Widgets.Add(wd);
        //    }
        //    SerializationHelper.Save(wdCollection, filePath);
        //}

        ///// <summary>
        ///// 获取Widget目录下所有控件信息 
        ///// </summary>
        ///// <returns></returns>
        //#region List<DataControlInfo> GetControls(string prefixPath)
        //public List<DataControlInfo> GetWidgetControls(string prefixPath)
        //{
        //    List<DataControlInfo> lstctrs = new List<DataControlInfo>();
        //    LoadDataControls(lstctrs, Constants.We7WidgetsPhysicalFolder, prefixPath, "系统部件");
        //    return lstctrs;
        //}

        ///// <summary>
        ///// 取得指定目录下的控件
        ///// </summary>
        ///// <param name="dir"></param>
        ///// <param name="prefixPath">前置目录 Weidget/We7Controls</param>
        //private void LoadDataControls(List<DataControlInfo> controls, string dir, string prefixPath, string group)
        //{
        //    DirectoryInfo di = new DirectoryInfo(dir);

        //    if (!di.Exists)
        //        return;

        //    DirectoryInfo[] cdi = di.GetDirectories();
        //    foreach (DirectoryInfo d in cdi)
        //    {
        //        try
        //        {
        //            FileInfo[] fs = d.GetFiles("*.ascx");
        //            if (fs != null && fs.Length > 0)
        //            {
        //                DataControlInfo dci = new DataControlInfo();
        //                dci.Group = group;

        //                foreach (FileInfo fileInfo in fs)
        //                {
        //                    string virtualPath = string.Format("{0}/{1}/{2}", prefixPath, d.Name, fileInfo.Name);
        //                    //获取控件上的描述属性，组描述属性
        //                    DataControl ctrl = BaseControlHelper.GetDataControlInfo(virtualPath);
        //                    if (ctrl != null)
        //                    {
        //                        ctrl.FileName = virtualPath;
        //                        ctrl.Name = fileInfo.Name.Replace(".ascx", "");
        //                        dci.Controls.Add(ctrl);

        //                        //组描述设置
        //                        if (string.IsNullOrEmpty(dci.GroupLabel) && !string.IsNullOrEmpty(ctrl.GroupLabel))
        //                            dci.GroupLabel = ctrl.GroupLabel;
        //                        if (string.IsNullOrEmpty(dci.GroupIcon) && !string.IsNullOrEmpty(ctrl.GroupIcon))
        //                            dci.GroupIcon = ctrl.GroupIcon;
        //                        if (string.IsNullOrEmpty(dci.GroupDesc) && !string.IsNullOrEmpty(ctrl.GroupDesc))
        //                            dci.GroupDesc = ctrl.GroupDesc;
        //                        if (string.IsNullOrEmpty(dci.GroupDefaultType) && !string.IsNullOrEmpty(ctrl.GroupDefaultType))
        //                            dci.GroupDefaultType = ctrl.GroupDefaultType;
        //                    }
        //                }
        //                if (dci.Controls != null && dci.Controls.Count > 0)
        //                {
        //                    //Default.ascx放前面，排序 
        //                    dci.Controls.Sort(SortDataControl);
        //                    dci.DefaultControl = dci.Controls[0];
        //                    dci.Default = dci.Controls[0].Name;
        //                    dci.Name = dci.Controls[0].Name.Split('.')[0];
        //                    dci.Desc = dci.Controls[0].Description;
        //                    if (string.IsNullOrEmpty(dci.GroupLabel))
        //                        dci.GroupLabel = dci.Desc;
        //                    if (string.IsNullOrEmpty(dci.GroupIcon))
        //                        dci.GroupIcon = dci.Desc;
        //                    if (string.IsNullOrEmpty(dci.GroupDesc))
        //                        dci.GroupDesc = dci.Desc;
        //                    if (string.IsNullOrEmpty(dci.GroupDefaultType))
        //                        dci.GroupDefaultType = dci.Default;
        //                    else
        //                    {
        //                        //验证默认控件是否存在
        //                        string filePath = string.Format("{0}\\{1}.ascx", d.FullName, dci.GroupDefaultType);
        //                        if (!File.Exists(filePath))
        //                            dci.GroupDefaultType = dci.Default;
        //                    }

        //                }
        //                //添加到组
        //                controls.Add(dci);
        //            }
        //            else
        //            {
        //                LoadDataControls(controls, prefixPath, d.FullName, group);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        //#endregion

        ///// <summary>
        ///// 从控件上获取描述信息
        ///// </summary>
        ///// <param name="path"></param>
        ///// <param name="groupAttr">组描述信息</param>
        //#region static DataControl GetDataControlInfo(string path)
        //public static DataControl GetDataControlInfo(string path)
        //{
        //    Page page = new Page();
        //    if (page != null)
        //    {
        //        Control p = page.LoadControl(path);

        //        //获取类上的ControlDescriptionAttribute
        //        MemberInfo mi = p.GetType();

        //        //控件描述
        //        List<ControlDescriptionAttribute> listParams = new List<ControlDescriptionAttribute>();
        //        ControlDescriptionAttribute cda =
        //            (ControlDescriptionAttribute)Attribute.GetCustomAttribute
        //            (mi, typeof(ControlDescriptionAttribute));
        //        if (cda != null)
        //            listParams.Add(cda);

        //        //获取MetaData属性上的ControlDescriptionAttribute
        //        foreach (PropertyInfo prop in p.GetType().GetProperties())
        //        {
        //            if (prop.Name == "MetaData")
        //            {
        //                //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
        //                ControlDescriptionAttribute par = GetAttribute<ControlDescriptionAttribute>(prop);
        //                if (par != null)
        //                {
        //                    listParams.Clear();
        //                    listParams.Add(par);
        //                }
        //            }
        //        }
        //        //获取MetaData属性上的ControlDescriptionAttribute
        //        foreach (FieldInfo field in p.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
        //        {
        //            if (field.Name == "MetaData")
        //            {
        //                //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
        //                ControlDescriptionAttribute par = GetAttribute<ControlDescriptionAttribute>(field);
        //                if (par != null)
        //                {
        //                    listParams.Clear();
        //                    listParams.Add(par);
        //                }
        //            }
        //        }

        //        if (listParams.Count == 0)
        //            return null;

        //        #region 控件组描述
        //        List<ControlGroupDescriptionAttribute> listGroupParams = new List<ControlGroupDescriptionAttribute>();
        //        ControlGroupDescriptionAttribute cgda =
        //            (ControlGroupDescriptionAttribute)Attribute.GetCustomAttribute
        //            (mi, typeof(ControlGroupDescriptionAttribute));
        //        if (cgda != null)
        //            listGroupParams.Add(cgda);

        //        //获取MetaData属性上的ControlDescriptionAttribute
        //        foreach (PropertyInfo prop in p.GetType().GetProperties())
        //        {
        //            if (prop.Name == "MetaData")
        //            {
        //                //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
        //                ControlGroupDescriptionAttribute par = GetAttribute<ControlGroupDescriptionAttribute>(prop);
        //                if (par != null)
        //                {
        //                    listGroupParams.Clear();
        //                    listGroupParams.Add(par);
        //                }
        //            }
        //        }
        //        //获取MetaData属性上的ControlDescriptionAttribute
        //        foreach (FieldInfo field in p.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
        //        {
        //            if (field.Name == "MetaData")
        //            {
        //                //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
        //                ControlGroupDescriptionAttribute par = GetAttribute<ControlGroupDescriptionAttribute>(field);
        //                if (par != null)
        //                {
        //                    listGroupParams.Clear();
        //                    listGroupParams.Add(par);
        //                }
        //            }
        //        }
        //        #endregion

        //        DataControl ctrl = new DataControl();
        //        ctrl.Author = listParams[0].Author;
        //        ctrl.Description = listParams[0].Desc;
        //        ctrl.Created = Convert.ToDateTime(listParams[0].Created);
        //        ctrl.Tag = listParams[0].Tag;
        //        ctrl.Name = listParams[0].Name;
        //        ctrl.Version = listParams[0].Version;

        //        if (listGroupParams.Count > 0)
        //        {
        //            ctrl.GroupLabel = listGroupParams[0].Label;
        //            ctrl.GroupIcon = listGroupParams[0].Icon;
        //            ctrl.GroupDesc = listGroupParams[0].Description;
        //            ctrl.GroupDefaultType = listGroupParams[0].DefaultType;
        //        }

        //        return ctrl;
        //    }

        //    return null;
        //}
        //#endregion

        ///// <summary>
        ///// DataControl排序用
        ///// </summary>
        ///// <param name="obj1"></param>
        ///// <param name="obj2"></param>
        //#region static int SortDataControl(DataControl obj1, DataControl obj2)
        //private static int SortDataControl(DataControl obj1, DataControl obj2)
        //{
        //    int res = 0;
        //    if ((obj1 == null) && (obj2 == null))
        //    {
        //        return 0;
        //    }
        //    else if ((obj1 != null) && (obj2 == null))
        //    {
        //        return -1;
        //    }
        //    else if ((obj1 == null) && (obj2 != null))
        //    {
        //        return 1;
        //    }
        //    if (obj1.FileName.Contains("Default.ascx"))
        //    {
        //        res = -1;
        //    }
        //    else if (obj2.FileName.Contains("Default.ascx"))
        //    {
        //        res = 1;
        //    }
        //    else
        //    {
        //        res = obj1.FileName.CompareTo(obj2.FileName);
        //    }
        //    return res;
        //}
        //#endregion

        //#endregion
    }
}
