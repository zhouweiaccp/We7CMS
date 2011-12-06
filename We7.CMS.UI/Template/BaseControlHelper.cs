using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Reflection;
using We7.CMS.Common;
using System.IO;
using We7.CMS.Module.VisualTemplate.Models;
using We7.Framework.Util;
using We7.Framework;
using System.Xml;
using System.Text.RegularExpressions;

namespace We7.CMS.WebControls.Core
{
    /// <summary>
    /// 控件基类的JSON序列化助手
    /// </summary>
    public class BaseControlHelper
    {
        /// <summary>
        /// DataControlInfo的关键字
        /// </summary>
        private const string DCIKEY = "$DCIKEY";

        private const string prefixPath = "/Widgets/WidgetCollection";
        private const string group = "系统部件";

        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return HelperFactory.Instance; }
        }
        /// <summary>
        /// 用户控件业务对象
        /// </summary>
        protected DataControlHelper DataControlHelper
        {
            get { return HelperFactory.GetHelper<DataControlHelper>(); }
        }
        
        
        /// <summary>
        /// 从指定控件中取得配置信息
        /// </summary>
        /// <param name="path"></param>
        #region  从指定控件中取得配置信息
        public DCInfo PickUp(string path)
        {
            DCInfo info = new DCInfo();
            info.Path = path;

            Page page = new Page();
            if (page != null)
            {
                Control p = page.LoadControl(path);
                PickUp(p, "", "基本信息", info);
            }

            return info;
        }

        public DCInfo PickUp(Control ctr)
        {
            DCInfo info = new DCInfo();
            PickUp(ctr, "", "基本信息", info);
            return info;
        }

        private void PickUp(object o, string prefix, string label, DCInfo info)
        {
            //反射元标记MetaData,筛选[属性名/ParameterAttribute]键值对
            Dictionary<string, ParameterAttribute> dictParams = BaseControlHelper.FilterParamAttrs(o, prefix);

            //获取DataControlParameters集合
            DCPartInfo part = CreateDCPart(info, label);
            part.Params = GetDataControlParameters(dictParams);

            //排序
            SortParam(part);
        }


        #endregion

        /// <summary>
        /// 创建DcPart
        /// </summary>
        /// <param name="info"></param>
        /// <param name="label"></param>
        #region DCPartInfo CreateDCPart(DCInfo info, string label)
        private DCPartInfo CreateDCPart(DCInfo info, string label)
        {
            DCPartInfo part = new DCPartInfo();
            part.Name = label;
            info.Parts.Add(part);
            return part;
        }
        #endregion

        /// <summary>
        /// 按Weight排序
        /// </summary>
        /// <param name="part"></param>
        #region void SortParam(DCPartInfo part)
        private void SortParam(DCPartInfo part)
        {
            part.Params.Sort((a, b) => b.Weight.CompareTo(a.Weight));
        }
        #endregion


        /// <summary>
        /// 通过属性名ParameterAttribute键值对获取DataControlParameter列表
        /// </summary>
        /// <param name="dictParams"></param>
        #region List<DataControlParameter> GetDataControlParameters(Dictionary<string, ParameterAttribute> dictParams)
        public List<DataControlParameter> GetDataControlParameters(Dictionary<string, ParameterAttribute> dictParams)
        {
            List<DataControlParameter> result = new List<DataControlParameter>();
            if (dictParams != null && dictParams.Count > 0)
            {
                foreach (KeyValuePair<string, ParameterAttribute> pair in dictParams)
                {
                    DataControlParameter dcp = CopyDCPfromParam(pair.Key, pair.Value);
                    result.Add(dcp);
                }
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 用ParameterAttribute复制一个DataControlParameter
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="pa"></param>
        #region DataControlParameter CopyDCPfromParam(string name,ParameterAttribute pa)
        private DataControlParameter CopyDCPfromParam(string name, ParameterAttribute pa)
        {
            DataControlParameter param = new DataControlParameter();

            param.Name = name;
            param.Data = pa.Data;
            param.DefaultValue = pa.DefaultValue;
            param.Description = pa.Description;
            param.Ignore = pa.Ignore;
            param.Length = pa.Length;
            param.Maximum = pa.Maxnum.ToString();
            param.Minium = pa.Minnum.ToString();
            param.Required = pa.Required;
            param.Title = pa.Title;
            param.Type = pa.Type;
            param.Weight = pa.Weight;
            param.SupportCopy = pa.SupportCopy;

            return param;
        }
        #endregion

        /// <summary>
        /// 通过MetaData字段来筛选ParameterAttribute属性
        /// </summary>
        /// <param name="o">控件</param>
        /// <param name="prefix">前缀</param>
        /// <returns>属性名 ParameterAttribute 键值对</returns>
        #region static Dictionary<string,ParameterAttribute> FilterParamAttrs(object o,string prefix)
        public static Dictionary<string, ParameterAttribute> FilterParamAttrs(object o, string prefix)
        {
            Dictionary<string, ParameterAttribute> dictParamAttr = new Dictionary<string, ParameterAttribute>();
            Dictionary<string, ParameterAttribute> dictParamAttrOnMetaData = new Dictionary<string, ParameterAttribute>();
            List<RemoveParameterAttribute> listParamRemoveOnMetaData = new List<RemoveParameterAttribute>();
            List<ClearParameterAttribute> listParamClearOnMetaData = new List<ClearParameterAttribute>();

            foreach (PropertyInfo prop in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                string name = !String.IsNullOrEmpty(prefix) ? String.Format("{0}-{1}", prefix, prop.Name) : prop.Name;

                if (Attribute.IsDefined(prop, typeof(ParameterAttribute)))
                {
                    if (prop.Name == "MetaData")
                    {
                        //MetaData可有多个元标记
                        //覆盖(在MetaData上使用ParameterAttribute的Name属性来进行覆盖)
                        List<ParameterAttribute> pars = GetAttributes<ParameterAttribute>(prop);
                        foreach (ParameterAttribute par in pars)
                            dictParamAttrOnMetaData.Add(par.Name, par);
                        //移除
                        List<RemoveParameterAttribute> parsRemove = GetAttributes<RemoveParameterAttribute>(prop);
                        listParamRemoveOnMetaData.AddRange(parsRemove);
                        //清除
                        List<ClearParameterAttribute> parsClear = GetAttributes<ClearParameterAttribute>(prop);
                        listParamClearOnMetaData.AddRange(parsClear);
                    }
                    else
                    {
                        ParameterAttribute par = GetAttribute<ParameterAttribute>(prop);
                        if (par != null)
                        {
                            //如果Name为空，则Name对应属性名称
                            if (string.IsNullOrEmpty(par.Name))
                                par.Name = prop.Name;
                            dictParamAttr.Add(prop.Name, par);
                        }
                    }
                }
                //反射属性字段的内部
                if (Attribute.IsDefined(prop, typeof(ChildrenAttribute)))
                {
                    object child = prop.GetValue(o, null);
                    GetParamAttrs(o, prop.Name, ref dictParamAttr);
                }
            }
            foreach (FieldInfo prop in o.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
            {
                string name = !String.IsNullOrEmpty(prefix) ? String.Format("{0}-{1}", prefix, prop.Name) : prop.Name;


                if (prop.Name == "MetaData")
                {
                    //MetaData可有多个元标记
                    //覆盖(在MetaData上使用ParameterAttribute的Name属性来进行覆盖)
                    List<ParameterAttribute> pars = GetAttributes<ParameterAttribute>(prop);
                    foreach (ParameterAttribute par in pars)
                    {
                        if (!String.IsNullOrEmpty(par.Name))
                        {
                            dictParamAttrOnMetaData.Add(par.Name, par);
                        }
                    }
                    //移除
                    List<RemoveParameterAttribute> parsRemove = GetAttributes<RemoveParameterAttribute>(prop);
                    listParamRemoveOnMetaData.AddRange(parsRemove);
                    //清除
                    List<ClearParameterAttribute> parsClear = GetAttributes<ClearParameterAttribute>(prop);
                    listParamClearOnMetaData.AddRange(parsClear);
                }
                else
                {
                    ParameterAttribute par = GetAttribute<ParameterAttribute>(prop);
                    if (par != null)
                    {
                        //如果Name为空，则Name对应属性名称
                        if (string.IsNullOrEmpty(par.Name))
                            par.Name = prop.Name;
                        if (!dictParamAttr.ContainsKey(prop.Name))
                            dictParamAttr.Add(prop.Name, par);
                    }
                }

                //反射属性字段的内部
                if (Attribute.IsDefined(prop, typeof(ChildrenAttribute)))
                {
                    object child = prop.GetValue(o);
                    GetParamAttrs(child, prop.Name, ref dictParamAttr);
                }
            }

            if (dictParamAttr.Count == 0)
                return null;
            if (dictParamAttrOnMetaData.Count == 0 && listParamClearOnMetaData.Count == 0 && listParamRemoveOnMetaData.Count == 0)
                return dictParamAttr;

            //清空属性不为空，则返回MetaData上的ParamterAttribute集合
            if (listParamClearOnMetaData.Count > 0)
                return dictParamAttrOnMetaData;

            if (listParamRemoveOnMetaData.Count > 0)
            {
                //移除
                foreach (RemoveParameterAttribute parRemove in listParamRemoveOnMetaData)
                    dictParamAttr.Remove(parRemove.PropertyName);
                //重写（移除属性重名项）
                foreach (KeyValuePair<string, ParameterAttribute> pair in dictParamAttrOnMetaData)
                    dictParamAttr.Remove(pair.Key);
            }
            if (dictParamAttr.Count > 0)
            {
                //转储
                foreach (KeyValuePair<string, ParameterAttribute> pair in dictParamAttr)
                    if (!dictParamAttrOnMetaData.ContainsKey(pair.Key))
                        dictParamAttrOnMetaData.Add(pair.Key, pair.Value);
            }

            return dictParamAttrOnMetaData;
        }
        #endregion


        /// <summary>
        /// 获取对象上的ParameterAttribute集合
        /// </summary>
        /// <param name="o"></param>
        /// <param name="PropName">字段/属性名称</param>
        #region static void GetParamAttrs(object o,string PropName,ref Dictionary<string, ParameterAttribute> dictParamAttr)
        private static void GetParamAttrs(object o, string PropName, ref Dictionary<string, ParameterAttribute> dictParamAttr)
        {
            foreach (PropertyInfo prop in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                List<ParameterAttribute> pars = GetAttributes<ParameterAttribute>(prop);
                foreach (ParameterAttribute par in pars)
                    dictParamAttr.Add(string.Format("{0}-{1}", PropName, prop.Name), par);
            }

            foreach (FieldInfo prop in o.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
            {
                List<ParameterAttribute> pars = GetAttributes<ParameterAttribute>(prop);
                foreach (ParameterAttribute par in pars)
                    dictParamAttr.Add(string.Format("{0}-{1}", PropName, prop.Name), par);
            }
        }
        #endregion

        #region 反射获取元标记
        /// <summary>
        /// 反射获取元标记
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(MemberInfo member)
            where T : Attribute
        {
            T result = default(T);

            object[] attrs = member.GetCustomAttributes(typeof(T), false);
            if (attrs != null && attrs.Length > 0)
            {
                result = attrs[0] as T;
            }

            return result;
        }
        /// <summary>
        /// 反射获取元标记
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public static List<T> GetAttributes<T>(MemberInfo member)
            where T : Attribute
        {
            List<T> result = new List<T>();

            object[] attrs = member.GetCustomAttributes(typeof(T), false);
            if (attrs != null && attrs.Length > 0)
            {
                result.Add(attrs[0] as T);
            }

            return result;
        }
        #endregion



        #region 创建主题索引相关操作
        /// <summary>
        /// 创建索引索引
        /// </summary>
        public void CreateThemeIndex()
        {
            string path = Path.Combine(Constants.We7ThemeFileFolder, "Themes.xml");
            Themes themes = GetThemes("/Widgets/Themes");
            SerializationHelper.Save(themes, path);
        }
        /// <summary>
        /// 获取目录下所有的主题配置
        /// </summary>
        /// <param name="prefixPath"></param>
        /// <returns></returns>
        public Themes GetThemes(string prefixPath)
        {
            Themes theme = new Themes();
            LoadThemes(theme, Constants.We7ThemeFileFolder, prefixPath);
            return theme;
        }

        /// <summary>
        /// 取得指定目录下的主题
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="prefixPath">前置目录 /Widgets/Themes</param>
        private void LoadThemes(Themes themes, string dir, string prefixPath)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            if (!di.Exists)
                return;

            DirectoryInfo[] cdi = di.GetDirectories();

            for (int i = 0; i < cdi.Length; i++)
            {
                try
                {
                    FileInfo[] fs = cdi[i].GetFiles("face.jpg");
                    if (fs != null && fs.Length > 0)
                    {
                        foreach (FileInfo fileInfo in fs)
                        {
                            ThemesType Tempthemes = new ThemesType();
                            string virtualPath = string.Format("{0}/{1}/{2}", prefixPath, cdi[i].Name, fileInfo.Name);
                            Tempthemes.img = virtualPath;
                            Tempthemes.name = cdi[i].Name;
                            Tempthemes.label = cdi[i].Name;
                            themes.item.Add(Tempthemes);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion




        /// <summary>
        /// 创建部件索引
        /// </summary>
        #region void CreateWidegetsIndex()
        public void CreateWidegetsIndex()
        {
            string path = Path.Combine(Constants.We7WidgetsPhysicalFolder, "WidgetsIndex.xml");
            List<DataControlInfo> ctrs = GetControls("/Widgets/WidgetCollection");
            CreateWidgetConfig(ctrs, path);
        }


        /// <summary>
        /// 创建部件配置
        /// </summary>
        /// <param name="dcs"></param>
        /// <returns></returns>
        private void CreateWidgetConfig(List<DataControlInfo> dcs, string filePath)
        {
            WidgetCollection wdCollection = new WidgetCollection();
            wdCollection.Name = "default";
            wdCollection.Label = "部件列表信息";
            wdCollection.Description = "部件列表信息";

            Dictionary<string, WidgetGroup> groups = new Dictionary<string, WidgetGroup>();
            foreach (DataControlInfo dci in dcs)
            {
                if (dci.Controls.Count == 0)
                    continue;
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
                wd.Name = dci.Name;
                wd.Label = dci.GroupLabel;
                wd.Icon = dci.GroupIcon;
                wd.File = dci.Path;
                wd.Description = dci.GroupDesc;
                wd.DefaultType = dci.GroupDefaultType;

                foreach (DataControl dc in dci.Controls)
                {
                    WidgetType wdType = new WidgetType();
                    wdType.Name = dc.Name;
                    wdType.File = dc.FileName;
                    wdType.Label = dc.Description;
                    wdType.Control = dc.Name.Replace(".", "_");
                    wd.Types.Add(wdType);
                }
                group.Widgets.Add(wd);
            }
            SerializationHelper.Save(wdCollection, filePath);
        }
        #endregion

        /// <summary>
        /// 获取Weidget目录下所有控件信息 
        /// </summary>
        /// <returns></returns>
        #region List<DataControlInfo> GetControls(string prefixPath)
        public List<DataControlInfo> GetControls(string prefixPath)
        {
            List<DataControlInfo> lstctrs = new List<DataControlInfo>();
            LoadDataControls(lstctrs, Constants.We7WidgetsFileFolder, prefixPath, "系统部件");
            return lstctrs;
        }
        /// <summary>
        /// 取得指定目录下的控件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="prefixPath">前置目录 Weidget/We7Controls</param>
        private void LoadDataControls(List<DataControlInfo> controls, string dir, string prefixPath, string group)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            if (!di.Exists)
                return;

            DirectoryInfo[] cdi = di.GetDirectories();
            DataControlInfo dci = new DataControlInfo();
            for (int i = 0; i < cdi.Length; i++)
            {
                try
                {
                    FileInfo[] fs = cdi[i].GetFiles("*.ascx");
                    if (fs != null && fs.Length > 0)
                    {
                        // DataControlInfo dci = new DataControlInfo();
                        dci.Group = group;

                        foreach (FileInfo fileInfo in fs)
                        {
                            string virtualPath = string.Format("{0}/{1}/{2}", prefixPath, cdi[i].Name, fileInfo.Name);
                            //获取控件上的描述属性，组描述属性
                            DataControl ctrl = BaseControlHelper.GetDataControlInfo(virtualPath);
                            if (ctrl != null)
                            {
                                ctrl.FileName = virtualPath;
                                ctrl.Name = fileInfo.Name.Replace(".ascx", "");
                                dci.Controls.Add(ctrl);

                                //组描述设置
                                if (string.IsNullOrEmpty(dci.GroupLabel) && !string.IsNullOrEmpty(ctrl.GroupLabel))
                                    dci.GroupLabel = prefixPath.Substring(prefixPath.LastIndexOf('/') + 1, prefixPath.Length - (prefixPath.LastIndexOf('/') + 1));
                                if (string.IsNullOrEmpty(dci.GroupIcon) && !string.IsNullOrEmpty(ctrl.GroupIcon))
                                    dci.GroupIcon = prefixPath.Substring(prefixPath.LastIndexOf('/') + 1, prefixPath.Length - (prefixPath.LastIndexOf('/') + 1));
                                if (string.IsNullOrEmpty(dci.GroupDesc) && !string.IsNullOrEmpty(ctrl.GroupDesc))
                                    dci.GroupDesc = prefixPath.Substring(prefixPath.LastIndexOf('/') + 1, prefixPath.Length - (prefixPath.LastIndexOf('/') + 1));
                                if (string.IsNullOrEmpty(dci.GroupDefaultType) && !string.IsNullOrEmpty(ctrl.GroupDefaultType))
                                    dci.GroupDefaultType = ctrl.GroupDefaultType;
                            }
                        }
                        if (dci.Controls != null && dci.Controls.Count > 0)
                        {
                            //Default.ascx放前面，排序 
                            dci.Controls.Sort(SortDataControl);
                            dci.DefaultControl = dci.Controls[0];
                            dci.Default = dci.Controls[0].Name;
                            // dci.Name = dci.Controls[0].Name.Split('.')[0];
                            dci.Desc = dci.Controls[0].Description;
                            if (string.IsNullOrEmpty(dci.Name))
                                dci.Name = new CNspellTranslator().GetSpells(prefixPath.Substring(prefixPath.LastIndexOf('/') + 1, prefixPath.Length - (prefixPath.LastIndexOf('/') + 1))) + DateTime.Now.Ticks;
                            if (string.IsNullOrEmpty(dci.GroupLabel))
                                dci.GroupLabel = dci.Desc;
                            if (string.IsNullOrEmpty(dci.GroupIcon))
                                dci.GroupIcon = dci.Desc;
                            if (string.IsNullOrEmpty(dci.GroupDesc))
                                dci.GroupDesc = dci.Desc;
                            if (string.IsNullOrEmpty(dci.GroupDefaultType))
                                dci.GroupDefaultType = dci.Default;
                            else
                            {
                                //验证默认控件是否存在
                                string filePath = string.Format("{0}\\{1}.ascx", cdi[i].FullName, dci.GroupDefaultType);
                                if (!File.Exists(filePath))
                                    dci.GroupDefaultType = dci.Default;
                            }

                        }
                    }
                    else
                    {
                        LoadDataControls(controls, cdi[i].FullName, prefixPath + "/" + cdi[i].Name, group);

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (dci != null && dci.Controls.Count > 0) controls.Add(dci);
        }
        #endregion

        /// <summary>
        /// 从控件上获取描述信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="groupAttr">组描述信息</param>
        #region static DataControl GetDataControlInfo(string path)
        public static DataControl GetDataControlInfo(string path)
        {
            Page page = new Page();
            if (page != null)
            {
                Control p = page.LoadControl(path);

                //获取类上的ControlDescriptionAttribute
                MemberInfo mi = p.GetType();

                //控件描述
                List<ControlDescriptionAttribute> listParams = new List<ControlDescriptionAttribute>();
                ControlDescriptionAttribute cda =
                    (ControlDescriptionAttribute)Attribute.GetCustomAttribute
                    (mi, typeof(ControlDescriptionAttribute));
                if (cda != null)
                    listParams.Add(cda);

                //获取MetaData属性上的ControlDescriptionAttribute
                foreach (PropertyInfo prop in p.GetType().GetProperties())
                {
                    if (prop.Name == "MetaData")
                    {
                        //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
                        ControlDescriptionAttribute par = GetAttribute<ControlDescriptionAttribute>(prop);
                        if (par != null)
                        {
                            listParams.Clear();
                            listParams.Add(par);
                        }
                    }
                }
                //获取MetaData属性上的ControlDescriptionAttribute
                foreach (FieldInfo field in p.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
                {
                    if (field.Name == "MetaData")
                    {
                        //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
                        ControlDescriptionAttribute par = GetAttribute<ControlDescriptionAttribute>(field);
                        if (par != null)
                        {
                            listParams.Clear();
                            listParams.Add(par);
                        }
                    }
                }

                if (listParams.Count == 0)
                    return null;

                #region 控件组描述
                List<ControlGroupDescriptionAttribute> listGroupParams = new List<ControlGroupDescriptionAttribute>();
                ControlGroupDescriptionAttribute cgda =
                    (ControlGroupDescriptionAttribute)Attribute.GetCustomAttribute
                    (mi, typeof(ControlGroupDescriptionAttribute));
                if (cgda != null)
                    listGroupParams.Add(cgda);

                //获取MetaData属性上的ControlDescriptionAttribute
                foreach (PropertyInfo prop in p.GetType().GetProperties())
                {
                    if (prop.Name == "MetaData")
                    {
                        //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
                        ControlGroupDescriptionAttribute par = GetAttribute<ControlGroupDescriptionAttribute>(prop);
                        if (par != null)
                        {
                            listGroupParams.Clear();
                            listGroupParams.Add(par);
                        }
                    }
                }
                //获取MetaData属性上的ControlDescriptionAttribute
                foreach (FieldInfo field in p.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
                {
                    if (field.Name == "MetaData")
                    {
                        //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
                        ControlGroupDescriptionAttribute par = GetAttribute<ControlGroupDescriptionAttribute>(field);
                        if (par != null)
                        {
                            listGroupParams.Clear();
                            listGroupParams.Add(par);
                        }
                    }
                }
                #endregion

                DataControl ctrl = new DataControl();
                ctrl.Control = Path.GetFileNameWithoutExtension(path);
                ctrl.Author = listParams[0].Author;
                ctrl.Description = listParams[0].Desc;
                ctrl.Created = Convert.ToDateTime(listParams[0].Created);
                ctrl.Tag = listParams[0].Tag;
                ctrl.Name = listParams[0].Name;
                ctrl.Version = listParams[0].Version;

                if (listGroupParams.Count > 0)
                {
                    ctrl.GroupLabel = listGroupParams[0].Label;
                    ctrl.GroupIcon = listGroupParams[0].Icon;
                    ctrl.GroupDesc = listGroupParams[0].Description;
                    ctrl.GroupDefaultType = listGroupParams[0].DefaultType;
                }

                return ctrl;
            }

            return null;
        }


        public DataControl GetDataControlInfoDeeply(string path)
        {
            Page page = new Page();
            if (page != null)
            {
                Control p = page.LoadControl(path);

                //获取类上的ControlDescriptionAttribute
                MemberInfo mi = p.GetType();

                //控件描述
                List<ControlDescriptionAttribute> listParams = new List<ControlDescriptionAttribute>();
                ControlDescriptionAttribute cda =
                    (ControlDescriptionAttribute)Attribute.GetCustomAttribute
                    (mi, typeof(ControlDescriptionAttribute));
                if (cda != null)
                    listParams.Add(cda);

                //获取MetaData属性上的ControlDescriptionAttribute
                foreach (PropertyInfo prop in p.GetType().GetProperties())
                {
                    if (prop.Name == "MetaData")
                    {
                        //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
                        ControlDescriptionAttribute par = GetAttribute<ControlDescriptionAttribute>(prop);
                        if (par != null)
                        {
                            listParams.Clear();
                            listParams.Add(par);
                        }
                    }
                }
                //获取MetaData属性上的ControlDescriptionAttribute
                foreach (FieldInfo field in p.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
                {
                    if (field.Name == "MetaData")
                    {
                        //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
                        ControlDescriptionAttribute par = GetAttribute<ControlDescriptionAttribute>(field);
                        if (par != null)
                        {
                            listParams.Clear();
                            listParams.Add(par);
                        }
                    }
                }

                if (listParams.Count == 0)
                    return null;

                #region 控件组描述
                List<ControlGroupDescriptionAttribute> listGroupParams = new List<ControlGroupDescriptionAttribute>();
                ControlGroupDescriptionAttribute cgda =
                    (ControlGroupDescriptionAttribute)Attribute.GetCustomAttribute
                    (mi, typeof(ControlGroupDescriptionAttribute));
                if (cgda != null)
                    listGroupParams.Add(cgda);

                //获取MetaData属性上的ControlDescriptionAttribute
                foreach (PropertyInfo prop in p.GetType().GetProperties())
                {
                    if (prop.Name == "MetaData")
                    {
                        //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
                        ControlGroupDescriptionAttribute par = GetAttribute<ControlGroupDescriptionAttribute>(prop);
                        if (par != null)
                        {
                            listGroupParams.Clear();
                            listGroupParams.Add(par);
                        }
                    }
                }
                //获取MetaData属性上的ControlDescriptionAttribute
                foreach (FieldInfo field in p.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
                {
                    if (field.Name == "MetaData")
                    {
                        //覆盖(使用MetaData上的ControlDescriptionAttribute属性覆盖类上的)
                        ControlGroupDescriptionAttribute par = GetAttribute<ControlGroupDescriptionAttribute>(field);
                        if (par != null)
                        {
                            listGroupParams.Clear();
                            listGroupParams.Add(par);
                        }
                    }
                }
                #endregion

                DataControl ctrl = new DataControl();
                ctrl.Control = Path.GetFileNameWithoutExtension(path);
                ctrl.Author = listParams[0].Author;
                ctrl.Description = listParams[0].Desc;
                //ctrl.Created = Convert.ToDateTime(listParams[0].Created);
                DateTime dt;
                ctrl.Created = DateTime.TryParse(listParams[0].Created, out dt) ? dt : DateTime.MinValue;
                ctrl.Tag = listParams[0].Tag;
                //ctrl.Name = listParams[0].Name;
                ctrl.Name = listParams[0].Desc;
                ctrl.Version = listParams[0].Version;

                if (listGroupParams.Count > 0)
                {
                    ctrl.GroupLabel = listGroupParams[0].Label;
                    ctrl.GroupIcon = listGroupParams[0].Icon;
                    ctrl.GroupDesc = listGroupParams[0].Description;
                    ctrl.GroupDefaultType = listGroupParams[0].DefaultType;
                }

                DCInfo dc = PickUp(path);
                if (dc != null)
                {
                    foreach (DCPartInfo dpi in dc.Parts)
                    {
                        foreach (DataControlParameter dcp in dpi.Params)
                        {
                            ctrl.Parameters.Add(dcp);
                        }
                    }
                }
                return ctrl;
            }

            return null;
        }
        #endregion

        /// <summary>
        /// DataControl排序用
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        #region static int SortDataControl(DataControl obj1, DataControl obj2)
        private static int SortDataControl(DataControl obj1, DataControl obj2)
        {
            int res = 0;
            if ((obj1 == null || String.IsNullOrEmpty(obj1.FileName)) && (obj2 == null || String.IsNullOrEmpty(obj2.FileName)))
            {
                return 0;
            }
            else if ((obj1 != null && !String.IsNullOrEmpty(obj1.FileName)) && (obj2 == null || String.IsNullOrEmpty(obj2.FileName)))
            {
                return 1;
            }
            else if ((obj1 == null || String.IsNullOrEmpty(obj1.FileName)) && (obj2 != null && String.IsNullOrEmpty(obj2.FileName)))
            {
                return -1;
            }

            Regex regex = new Regex("Default.ascx$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (regex.IsMatch(obj1.FileName))
            {
                res = 1;
            }
            else if (regex.IsMatch(obj2.FileName))
            {
                res = -1;
            }
            else
            {
                res = obj1.FileName.CompareTo(obj2.FileName);
            }
            return res;
        }
        #endregion

        #region 整合部件和控件
        /// <summary>
        /// 将模型、插件、控件信息整合在一起
        /// </summary>
        /// <returns></returns>
        private List<DataControlInfo> IntegrationWidgetsAndControls()
        {
            //部件索引与控件索引分开
            //List<DataControlInfo> ctrs = GetControls("/Widgets/WidgetCollection");
            DataControlHelper dchelper = HelperFactory.Instance.GetHelper<DataControlHelper>();
            List<DataControlInfo> list = dchelper.GetControls() ?? new List<DataControlInfo>();
            //if (ctrs != null)
            //{
            //    foreach (DataControlInfo di in ctrs)
            //    {
            //        foreach (DataControl dc in di.Controls)
            //        {
            //            dc.Name = dc.Description;
            //        }
            //        list.Add(di);
            //    }
            //}
            return list;
        }

        /// <summary>
        /// 创建整合过后的索引文件
        /// </summary>
        public void CreateIntegrationIndexConfig()
        {
            List<DataControlInfo> ctrs = IntegrationWidgetsAndControls();

            if (Directory.Exists(Constants.We7WidgetsPhysicalFolder))
            {
                string path = Path.Combine(Constants.We7WidgetsPhysicalFolder, "We7DataControlIndex.xml");
                XmlDocument doc = new XmlDocument();
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
                XmlNode root = doc.CreateElement("Controls");
                doc.AppendChild(root);
                foreach (DataControlInfo dcInfo in ctrs)
                {
                    XmlElement ctrEl = doc.CreateElement("control");
                    ctrEl.SetAttribute("name", dcInfo.Name);
                    ctrEl.SetAttribute("group", dcInfo.Group);
                    ctrEl.SetAttribute("path", dcInfo.Path);
                    ctrEl.SetAttribute("directory", dcInfo.Directory);
                    ctrEl.SetAttribute("desc", dcInfo.Desc);

                    foreach (DataControl dc in dcInfo.Controls)
                    {
                        XmlElement itemEl = doc.CreateElement("item");
                        itemEl.SetAttribute("control", dc.Control);
                        itemEl.SetAttribute("name", dc.Name);
                        itemEl.SetAttribute("type", dc.Type);
                        itemEl.SetAttribute("version", dc.Version);
                        itemEl.SetAttribute("created", dc.Created.ToString("yyyy-MM-dd"));
                        itemEl.SetAttribute("desc", dc.Description);
                        itemEl.SetAttribute("demoUrl", dc.DemoUrl);
                        itemEl.SetAttribute("fileName", dc.FileName);
                        ctrEl.AppendChild(itemEl);
                    }
                    root.AppendChild(ctrEl);
                }

                doc.Save(path);
            }
        }

        /// <summary>
        /// 判定当前控件是不是控件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsControl(string path)
        {
            string physicalPath = HttpContext.Current.Server.MapPath(path);
            FileInfo fi = new FileInfo(physicalPath);
            string root = fi.Directory.Parent.FullName;
            string dataContrlXmlPath = Path.Combine(root, "DataControl.xml");
            return File.Exists(dataContrlXmlPath);
        }

        /// <summary>
        /// 取得整合过后的控件信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataControlInfo GetIntegrationInfoByPath(string path)
        {
            //在控件目录下会有DataControl.xml文件，而部件目录下没有
            string physicalPath = HttpContext.Current.Server.MapPath(path);
            FileInfo fi = new FileInfo(physicalPath);
            string root = fi.Directory.Parent.FullName;
            string dataContrlXmlPath = Path.Combine(root, "DataControl.xml");

            if (File.Exists(dataContrlXmlPath))
            {
                return HelperFactory.Instance.GetHelper<DataControlHelper>().GetDataControlInfoByPath(path);
            }
            else
            {
                return GetWidgetControlInfo(path);
            }
        }

        /// <summary>
        /// 取得单个部件组的
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataControlInfo GetWidgetControlInfo(string path)
        {
            DataControlInfo dci = null;
            string virtualPath = path;
            path = HttpContext.Current.Server.MapPath(path);
            FileInfo fileInfo = new FileInfo(path);
            DirectoryInfo d = fileInfo.Directory;

            if (d.Exists && fileInfo.Exists)
            {
                dci = new DataControlInfo();
                dci.Group = group;
                //string virtualPath = string.Format("{0}/{1}/{2}", prefixPath, d.Name, fileInfo.Name);
                //获取控件上的描述属性，组描述属性
                DataControl ctrl = GetDataControlInfoDeeply(virtualPath);
                if (ctrl != null)
                {
                    ctrl.FileName = virtualPath;
                    //ctrl.Name = fileInfo.Name.Replace(".ascx", "");
                    dci.Controls.Add(ctrl);

                    //组描述设置
                    if (string.IsNullOrEmpty(dci.GroupLabel) && !string.IsNullOrEmpty(ctrl.GroupLabel))
                        dci.GroupLabel = ctrl.GroupLabel;
                    if (string.IsNullOrEmpty(dci.GroupIcon) && !string.IsNullOrEmpty(ctrl.GroupIcon))
                        dci.GroupIcon = ctrl.GroupIcon;
                    if (string.IsNullOrEmpty(dci.GroupDesc) && !string.IsNullOrEmpty(ctrl.GroupDesc))
                        dci.GroupDesc = ctrl.GroupDesc;
                    if (string.IsNullOrEmpty(dci.GroupDefaultType) && !string.IsNullOrEmpty(ctrl.GroupDefaultType))
                        dci.GroupDefaultType = ctrl.GroupDefaultType;
                }
            }
            if (dci.Controls != null && dci.Controls.Count > 0)
            {
                //Default.ascx放前面，排序 
                dci.Controls.Sort(SortDataControl);
                dci.DefaultControl = dci.Controls[0];
                dci.Default = dci.Controls[0].Name;
                dci.Name = dci.Controls[0].Name.Split('.')[0];
                dci.Desc = dci.Controls[0].Description;
                if (string.IsNullOrEmpty(dci.GroupLabel))
                    dci.GroupLabel = dci.Desc;
                if (string.IsNullOrEmpty(dci.GroupIcon))
                    dci.GroupIcon = dci.Desc;
                if (string.IsNullOrEmpty(dci.GroupDesc))
                    dci.GroupDesc = dci.Desc;
                if (string.IsNullOrEmpty(dci.GroupDefaultType))
                    dci.GroupDefaultType = dci.Default;
                else
                {
                    //验证默认控件是否存在
                    string filePath = string.Format("{0}\\{1}.ascx", d.FullName, dci.GroupDefaultType);
                    if (!File.Exists(filePath))
                        dci.GroupDefaultType = dci.Default;
                }

            }
            return dci;
        }

        /// <summary>
        /// 获取包括 部件/控件 的列表
        /// </summary>
        /// <returns></returns>
        public List<DataControlInfo> GetDataControlsInfos()
        {
            List<DataControlInfo> result = AppCtx.Cache.RetrieveObject<List<DataControlInfo>>(DCIKEY);
            if (result == null)
            {
                if (Directory.Exists(Constants.We7ControlPhysicalPath) || Directory.Exists(Constants.We7ModelPhysicalPath) || Directory.Exists(Constants.We7PluginPhysicalPath))
                {
                    string path = Path.Combine(Constants.We7WidgetsPhysicalFolder, "We7DataControlIndex.xml");
                    if (!File.Exists(path))
                    {
                        DataControlHelper.CreateDataControlIndex();
                        if (!File.Exists(path))
                            throw new Exception(path + "文件不存在!");
                    }

                    result = new List<DataControlInfo>();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList ctrs = doc.DocumentElement.SelectNodes("control");
                    foreach (XmlElement ctr in ctrs)
                    {
                        DataControlInfo dci = new DataControlInfo();
                        dci.Name = ctr.GetAttribute("name");
                        dci.Group = ctr.GetAttribute("group");
                        dci.Path = ctr.GetAttribute("path");
                        dci.Directory = ctr.GetAttribute("directory");
                        dci.Desc = ctr.GetAttribute("desc");
                        dci.Default = ctr.GetAttribute("default");
                        XmlNodeList items = ctr.SelectNodes("item");
                        foreach (XmlElement item in items)
                        {
                            DataControl dc = new DataControl();
                            dc.Control = item.GetAttribute("control");
                            dc.Name = item.GetAttribute("name");
                            dc.Type = item.GetAttribute("type");
                            dc.Version = item.GetAttribute("version");
                            DateTime dt;
                            DateTime.TryParse(item.GetAttribute("created"), out dt);
                            dc.Created = dt;
                            dc.FileName = item.GetAttribute("fileName");
                            dc.DemoUrl = item.GetAttribute("demoUrl");
                            dci.Controls.Add(dc);
                        }
                        if (dci.Controls == null || dci.Controls.Count == 0)
                            continue;

                        if (String.IsNullOrEmpty(dci.Default))
                        {
                            dci.DefaultControl = dci.Controls.Find(delegate(DataControl dctr)
                            {
                                return String.Compare(dci.Default, dctr.Control, true) == 0;
                            });
                        }

                        if (dci.DefaultControl == null)
                        {
                            dci.DefaultControl = dci.Controls[0];
                        }

                        result.Add(dci);
                    }                    
                }
                if (Directory.Exists(Constants.We7WidgetsPhysicalFolder))
                {
                    string pathWidgets = Path.Combine(Constants.We7WidgetsPhysicalFolder, "WidgetsIndex.xml");
                    string pathContorl = Path.Combine(Constants.We7WidgetsPhysicalFolder, "We7DataControlIndex.xml");
                    List<DataControlInfo> ctrs = GetControls("/Widgets/WidgetCollection");

                    if (result == null)
                        result = new List<DataControlInfo>();

                    //添加部件
                    result.AddRange(ctrs);
                    string[] files = new string[] { pathWidgets, pathContorl };
                    AppCtx.Cache.AddObjectWithFileChange(DCIKEY, result, files);
                }

            }
            return result;
        }

        #endregion

    }
}
