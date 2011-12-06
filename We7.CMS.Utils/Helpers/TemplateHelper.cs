using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Collections;

using We7;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Config;
using We7.CMS.Common;
using System.Xml;
using We7.Model.Core;

namespace We7.CMS
{
    /// <summary>
    /// 新版（2.2+）模板处理类；
    /// 新版模板修改、地图映射等方法放在这里
    /// </summary>
    public partial class TemplateHelper : BaseHelper
    {
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID]; }
        }

        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }
        /// <summary>
        /// 模板组路径
        /// </summary>
        public string TemplateGroupPath
        {
            get { return Path.Combine(Root, Constants.TemplateGroupBasePath); }
        }

        /// <summary>
        /// 默认模板组路径
        /// </summary>
        public string DefaultTemplateGroupPath
        {
            get
            {
                if (EnableSiteSkins)
                {
                    SiteSettingHelper cdHelper = new SiteSettingHelper();
                    GeneralConfigInfo si = GeneralConfigs.GetConfig();
                    return Path.Combine(Root, String.Format("{0}\\{1}", Constants.SiteSkinsBasePath, Path.GetFileNameWithoutExtension(si.DefaultTemplateGroupFileName)));
                }
                else
                    return Path.Combine(Root, Constants.TemplateBasePath);
            }
        }

        private string groupName;
        public string GroupName
        {
            get
            {
                if (String.IsNullOrEmpty(groupName))
                {
                    groupName = Path.GetFileNameWithoutExtension(GeneralConfigs.GetConfig().DefaultTemplateGroupFileName);
                }
                return groupName;
            }
        }

        /// <summary>
        /// 是否启用模板组皮肤路径
        /// </summary>
        public static bool EnableSiteSkins
        {
            get
            {
                string _default = SiteSettingHelper.Instance.Config.EnableSiteSkins;
                if (_default.ToLower() == "true")
                    return true;
                return false;
            }
        }
        
        /// <summary>
        /// 模板组皮肤路径
        /// </summary>
        public string SkinPath
        {
            get { return Path.Combine(Root, Constants.TemplateGroupBasePath); }
        }

        private string GetHtmlTemplatePath(string tplName)
        {
            return String.Format("/_skins/{0}/HtmlTemplate/{1}.ascx", GroupName, tplName);
        }

        /// <summary>
        /// 获取某一状态模板数量
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetTemplateCount(TemplateType type, string skinFolder)
        {
            if (skinFolder.EndsWith(".xml"))
                skinFolder = skinFolder.Remove(skinFolder.Length - 4);
            Template[] ts = GetTemplates("", skinFolder, type);
            return ts.Length;
        }

        public int GetTemplateCount(TemplateType type)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (!string.IsNullOrEmpty(si.DefaultTemplateGroupFileName))
            {
                return GetTemplateCount(type, si.DefaultTemplateGroupFileName.Remove(si.DefaultTemplateGroupFileName.Length - 4));
            }
            return 0;
        }

        /// <summary>
        /// 查询模板．
        /// </summary>
        /// <param name="queryName">模板文件名称关键字</param>
        /// <returns></returns>
        public Template[] GetTemplates(string queryName)
        {
            string skinFolder = GeneralConfigs.GetConfig().DefaultTemplateGroupFileName;
            if (skinFolder.EndsWith(".xml"))
                skinFolder = skinFolder.Remove(skinFolder.Length - 4);
            return GetTemplates(queryName, skinFolder, TemplateType.All);
        }

        public Template[] GetTemplates(string queryName, string templatefolder, TemplateType type)
        {
            List<Template> allTemplates = GetAllTemplates(templatefolder);
            List<Template> ts = new List<Template>();
            foreach (Template t in allTemplates)
            {
                if (t != null)
                {
                    if (queryName == null || queryName.Length == 0 || t.Name.Contains(queryName))
                    {
                        if (type == TemplateType.HaveBinded)
                        {
                            if (!string.IsNullOrEmpty(t.DefaultBindText)) ts.Add(t);
                        }
                        else if (type == TemplateType.Sub)
                        {
                            if (t.IsSubTemplate) ts.Add(t);
                        }
                        else if (type == TemplateType.MasterPage)
                        {
                            if (t.TemplateType == ((int)TemplateType.MasterPage).ToString())
                                ts.Add(t);
                        }
                        else if (type == TemplateType.Common)
                        {
                            if (t.TemplateType != ((int)TemplateType.Sub).ToString() &&
                                t.TemplateType != ((int)TemplateType.MasterPage).ToString() && !t.IsSubTemplate)
                                ts.Add(t);
                        }
                        else
                            ts.Add(t);
                    }
                }
            }
            return ts.ToArray();
        }

        private static readonly string TemplatesKeyID = "Skin_{0}";
        /// <summary>
        /// 获取所有模板列表信息，缓存一下
        /// </summary>
        /// <param name="templatefolder"></param>
        /// <returns></returns>
        List<Template> GetAllTemplates(string templatefolder)
        {
            HttpContext Context = HttpContext.Current;
            List<Template> ts = null;
            string key = string.Format(TemplatesKeyID, templatefolder);
            ts = (List<Template>)Context.Cache[key];//缓存
            if (ts == null)
            {
                string templatePath = DefaultTemplateGroupPath;
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si.DefaultTemplateGroupFileName != templatefolder)
                {
                    string basePath = Constants.TemplateBasePath;
                    if (basePath.StartsWith("/") || basePath.StartsWith("\\"))
                        basePath = basePath.Remove(0, 1);
                    templatePath = String.Format("{0}\\{1}", Path.Combine(Root, basePath), templatefolder);
                }
                else
                {
                    templatePath = String.Format("{0}", DefaultTemplateGroupPath);
                }

                ts = new List<Template>();
                if (!Directory.Exists(templatePath))
                {
                    Directory.CreateDirectory(templatePath);
                }
                DirectoryInfo di = new DirectoryInfo(templatePath);
                FileInfo[] files = di.GetFiles("*" + Constants.TemplateFileExtension, SearchOption.TopDirectoryOnly);
                foreach (FileInfo f in files)
                {
                    Template t = GetTemplate(f.Name, templatefolder);
                    ts.Add(t);
                }

                if (ts != null)
                {
                    CacherCache(key, Context, ts, CacheTime.Long);
                }
            }
            return ts;
        }

        /// <summary>
        /// 查询模板
        /// </summary>
        /// <param name="queryName">模板名称关键字</param>
        /// <param name="templatefolder">模板组名称</param>
        /// <returns></returns>
        public Template[] GetTemplates(string queryName, string templatefolder)
        {
            return GetTemplates(queryName, templatefolder, TemplateType.All);
        }

        /// <summary>
        /// 依据模板组文件名与路径获取模板组对象
        /// </summary>
        /// <param name="fileName">模板组文件名</param>
        /// <param name="templatefolder">模板组路径</param>
        /// <returns>模板组对象</returns>
        public Template GetTemplate(string fileName, string templatefolder)
        {
            if (!fileName.ToLower().EndsWith(".xml")) fileName += ".xml";
            string templatePath = DefaultTemplateGroupPath;
            if (EnableSiteSkins)
                templatePath = String.Format("{0}\\{1}", TemplateGroupPath, templatefolder);

            if (File.Exists(Path.Combine(templatePath, fileName)))
            {
                Template t = new Template();
                t.FromFile(templatePath, fileName);
                return t;
            }
            return null;
        }

        /// <summary>
        /// 依据模板描述XML文件获取模板对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Template GetTemplate(string fileName)
        {
            if (File.Exists(Path.Combine(DefaultTemplateGroupPath, fileName)))
            {
                Template t = new Template();
                if (fileName.IndexOf(".xml") > -1)
                {
                    t.FromFile(DefaultTemplateGroupPath, fileName);
                }
                else
                {
                    t.FromFile(DefaultTemplateGroupPath, fileName + ".xml");
                }
                return t;
            }
            return null;
        }

        /// <summary>
        /// 获取模板名称
        /// </summary>
        /// <param name="tp">模板XML文件名称</param>
        /// <returns>模板名称</returns>
        public string GetTemplateName(string tp)
        {
            if (tp != null && tp.Length > 0)
            {
                string tpName = "";
                if (!tp.EndsWith(".xml"))
                {
                    tpName = tp + ".xml";
                }
                else
                {
                    tpName = tp;
                }

                Template t = GetTemplate(tpName);
                if (t != null)
                {
                    return t.Name;
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 根据配置选择不同的模板选择器
        /// </summary>
        /// <param name="ColumnMode"></param>
        /// <param name="ColumnID"></param>
        /// <param name="SearchWord"></param>
        /// <param name="SeSearchWord"></param>
        /// <returns></returns>
        public string GetTemplateByHandlers(string ColumnMode, string ColumnID, string SearchWord, string SeSearchWord)
        {
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
            if (gi.StartTemplateMap)
            {
                HttpContext Context = HttpContext.Current;
                string channelUrl = We7Helper.GetChannelUrlFromUrl(Context.Request.RawUrl);
                string templatePath = We7Helper.GetParamValueFromUrl(Context.Request.RawUrl, "template");
                if (string.IsNullOrEmpty(templatePath))
                {
                    if (channelUrl == "/" && ColumnMode == "")
                    {
                        templatePath = TemplateMap.GetTemplateFromMap("welcome", channelUrl);
                        if (string.IsNullOrEmpty(templatePath))
                            templatePath = templatePath = TemplateMap.GetTemplateFromMap("home", channelUrl);
                    }
                    else
                        templatePath = TemplateMap.GetTemplateFromMap(ColumnMode, channelUrl);

                    if (!string.IsNullOrEmpty(templatePath))
                    {
                        GeneralConfigInfo si = GeneralConfigs.GetConfig();
                        templatePath = GetTemplatePath(si.DefaultTemplateGroupFileName, templatePath);
                    }
                }
                return templatePath;
            }
            else
                return GetThisPageTemplate(ColumnMode, ColumnID, SearchWord, SeSearchWord);

        }

        public string GetHtmlTemplateByHandlers(string ColumnMode, string ColumnID, string SearchWord, string SeSearchWord)
        {
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
           
            if (gi.StartTemplateMap)
            {
                HttpContext Context = HttpContext.Current;
                string channelUrl = We7Helper.GetChannelUrlFromUrl(Context.Request.RawUrl);
                string htmlTemplate = We7Helper.GetParamValueFromUrl(Context.Request.RawUrl, "template");
                if (String.IsNullOrEmpty(htmlTemplate))
                {
                    if (channelUrl == "/" && ColumnMode == "")
                    {
                        htmlTemplate = GetHtmlTemplatePath("welcome");
                        if (!File.Exists(HttpContext.Current.Server.MapPath(htmlTemplate)))
                        {
                            htmlTemplate = GetHtmlTemplatePath("index");
                            if (!File.Exists(HttpContext.Current.Server.MapPath(htmlTemplate)))
                            {
                                return GetTemplateByHandlers(ColumnMode, ColumnID, SearchWord, SeSearchWord);
                            }
                        }
                    }
                    else
                    {
                        htmlTemplate = GetHtmlTemplatePath(channelUrl.Trim('/').Trim('\\') + "/" +(String.IsNullOrEmpty(ColumnMode)?"index":ColumnMode));
                        if (!File.Exists(HttpContext.Current.Server.MapPath(htmlTemplate)))
                        {
                            return GetTemplateByHandlers(ColumnMode, ColumnID, SearchWord, SeSearchWord);
                        }
                    }
                }
                return htmlTemplate;
            }
            else
                return GetThisHtmlPageTemplate(ColumnMode, ColumnID, SearchWord, SeSearchWord);
        }

        /// <summary>
        /// 获取模板组对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public SkinInfo GetSkinInfo(string fileName)
        {
            HttpContext context = HttpContext.Current;
            string key = "CD.SkinInfo." + fileName;
            if (context.Application[key] == null)
            {
                context.Application[key] = GetSkinInfoFromFile(fileName);
            }
            return (SkinInfo)context.Application[key];
        }

        /// <summary>
        /// 无缓存获取皮肤信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public SkinInfo GetSkinInfoFromFile(string fileName)
        {
            SkinInfo skinInfo = new SkinInfo();
            if (!fileName.ToLower().EndsWith(".xml")) 
                fileName += ".xml";
            skinInfo.FromFile(SkinPath, fileName);
            return skinInfo;
        }

        /// <summary>
        /// 保存模板皮肤信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public string SaveSkinInfoAndPreviewFile(SkinInfo data, string folderName)
        {
            if (data.FileName == null)
            {
                DateTime dt = DateTime.Now;
                data.Created = dt;
                data.FileName = String.Format("{0}.xml", folderName);
            }
            data.ToFile(TemplateGroupPath, data.FileName);
            ClearSkinInfoCache(data.FileName);
            return data.FileName;
        }

        /// <summary>
        /// 清除application变量值，使其可以重新取得最新值
        /// </summary>
        /// <param name="fileName"></param>
        void ClearSkinInfoCache(string fileName)
        {
            HttpContext context = HttpContext.Current;
            string key = "CD.SkinInfo." + fileName;
            context.Application.Remove(key);
        }

        /// <summary>
        /// 保存模板信息
        /// </summary>
        /// <param name="tp">模板信息</param>
        /// <param name="Templatefolder">模板文件夹</param>
        public void SaveTemplate(Template tp, string templatefolder)
        {
            string templatePath = DefaultTemplateGroupPath;
            SiteSettingHelper cdHelper = new SiteSettingHelper();
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (string.IsNullOrEmpty(templatefolder))
                templatefolder = si.DefaultTemplateGroupFileName;
            else if (templatefolder.ToLower().EndsWith(".xml"))
            {
                templatefolder = templatefolder.Substring(0, templatefolder.Length - 4);
            }
            if (si.DefaultTemplateGroupFileName != templatefolder)
            {
                templatePath = String.Format("{0}\\{1}", Path.Combine(Root, Constants.TemplateBasePath), templatefolder);

            }
            else
            {
                templatePath = String.Format("{0}", DefaultTemplateGroupPath);
            }

            string fn = tp.FileName;
            fn += Constants.TemplateFileExtension;

            string target = Path.Combine(templatePath, fn);
            tp.ToFile(target);

            //清除模板列表缓存
            HttpContext Context = HttpContext.Current;
            string key = string.Format(TemplatesKeyID, templatefolder);
            Context.Cache.Remove(key);
        }

        /// <summary>
        /// 根据模板组文件生成模版地图
        /// </summary>
        /// <param name="skinInfo"></param>
        public void CreateMapFileFromSkinInfo(SkinInfo skinInfo)
        {
            string templatesMapFile = HttpContext.Current.Server.MapPath("~/_skins/templates.map");
            TemplateMap tm = new TemplateMap(templatesMapFile);
            tm.Maps.Clear();
            tm.Maps = TemplateMap.CreateMapList(skinInfo.Items);

            TemplateMap.SaveToMapFile(tm.Maps, templatesMapFile);
            TemplateMap.ResetInstance();
        }

        /// <summary>
        /// 获取模板绑定配置项列表
        /// </summary>
        /// <returns></returns>
        public List<TemplateBindConfig> GetTemplateBindConfigList()
        {
            XmlDocument doc = new XmlDocument();
            XmlNodeList nodes = null;
            string typePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
            string typeName = Path.Combine(typePath, "TemplateType.xml");
            doc.Load(typeName);
            nodes = doc.SelectNodes("/configuration/item");
            List<TemplateBindConfig> list = new List<TemplateBindConfig>();
            foreach (XmlNode nodeType in nodes)
            {
                string name = nodeType.Attributes["name"].Value;
                string value = nodeType.Attributes["value"].Value;
                foreach (XmlNode subItem in nodeType.ChildNodes)
                {
                    TemplateBindConfig tbc = new TemplateBindConfig();
                    tbc.Handler = value;
                    tbc.HandlerName = name;
                    string n = subItem.Attributes["name"].Value;
                    string v = subItem.Attributes["value"].Value;
                    tbc.Description = name + n;
                    tbc.Mode = v;
                    tbc.ModeText = n;
                    list.Add(tbc);
                }
            }
            return list;
        }

        /// <summary>
        /// 保存模板绑定项
        /// </summary>
        /// <param name="bc"></param>
        /// <param name="skinFolder"></param>
        /// <param name="templateFile"></param>
        public void SaveTemplateBind(TemplateBindConfig bc,string skinFolder,string templateFile)
        {
            if (bc != null)
            {
                string handler = bc.Handler;
                string mode = bc.Mode;
                string model = bc.Model;
                bc = GetTemplateConfigSentence(bc);

                SkinInfo tg = GetSkinInfo(skinFolder + ".xml");//读取模板信息
                SkinInfo.SkinItem item = new SkinInfo.SkinItem();
                Template tp = GetTemplate(templateFile);
                if (tp != null)
                {
                    item.TemplateText = tp.Name;
                    item.Name = tp.Name;
                    item.Description = tp.Description;
                    item.Template = tp.FileName;
                    if (handler == "model" && !string.IsNullOrEmpty(model))
                    {
                        ModelInfo mi= ModelHelper.GetModelInfoByName(model);
                        if (mi != null)
                        {
                            item.C_Model = model;
                            item.C_ModelText = mi.Label;
                        }
                        else
                            throw new Exception("没有找到模型的信息文件，请重建内容模型索引再试。");
                    }

                    item.Location = mode;
                    item.LocationText =bc.HandlerName+ bc.ModeText;
                    item.Tag = "";
                    item.Type = handler;
                    if (!HaveSameItem(item, tg))
                    {
                        tg.Items.Add(item);
                        string sentence = item.C_ModelText + item.LocationText;
                        string fn = SaveSkinInfoAndPreviewFile(tg, Path.GetFileNameWithoutExtension(skinFolder));
                        CreateMapFileFromSkinInfo(tg);
                        RefreshOneTempplateBindText(tg, tp);
                    }
                    else
                        throw new Exception("对不起，已经存在相同的绑定项，不能重复增加！如想修改绑定项，请先删除旧的再试。");
                }
                else
                    throw new Exception("无法保存！不存在模板文件“" + templateFile + "”！");

            }
        }

        /// <summary>
        /// 获取模板指定配置中文描述
        /// </summary>
        /// <param name="bc"></param>
        /// <returns></returns>
        public  TemplateBindConfig GetTemplateConfigSentence(TemplateBindConfig bc)
        {
            if (bc.Handler == "site" && string.IsNullOrEmpty(bc.Mode))
                bc.Mode = "home";
            else if (string.IsNullOrEmpty(bc.Mode))
                bc.Mode = "default";

            XmlDocument doc = new XmlDocument();
            XmlNodeList nodes = null;
            string typePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
            string typeName = Path.Combine(typePath, "TemplateType.xml");
            doc.Load(typeName);
            nodes = doc.SelectNodes("/configuration/item");
            string sentence = "";
            foreach (XmlNode nodeType in nodes)
            {
                string name = nodeType.Attributes["name"].Value;
                string value = nodeType.Attributes["value"].Value;
                if (value == bc.Handler)
                {
                    sentence = name;
                    bc.HandlerName=name;
                    foreach (XmlNode subItem in nodeType.ChildNodes)
                    {
                        string n = subItem.Attributes["name"].Value;
                        string v = subItem.Attributes["value"].Value;
                        if (v == bc.Mode)
                        {
                            sentence += n;
                            bc.ModeText = n;
                            bc.Description = sentence;
                            break;
                        }
                    }
                    break;
                }
            }
            if (!string.IsNullOrEmpty(bc.Model))
            {
                ModelInfo mi = ModelHelper.GetModelInfoByName(bc.Model);
                bc.Description = mi.Label + bc.Description;
            }
            return bc;
        }

        public bool HaveSameItem(SkinInfo.SkinItem item, SkinInfo data)
        {
            foreach (SkinInfo.SkinItem skin in data.Items)
            {
                if (StringCompare(item.C_Model, skin.C_Model) &&
                    StringCompare(item.Location, skin.Location) &&
                    StringCompare(item.Tag, skin.Tag) &&
                    StringCompare(item.Type, skin.Type))
                    return true;
            }
            return false;
        }

        bool StringCompare(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
                return true;
            else
                return s1 == s2;
        }

        /// <summary>
        /// 加载模板绑定项，并生成包含删除js的html代码
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string LoadTemplateBinds(string skinFile, string templateFile)
        {
            StringBuilder sb = new StringBuilder();
            string bindLi = "<LI id='{5}' ><IMG class=Icon height=16 src=\"/admin/images/icon_globe.gif\" width=16>{3}<A class=Del title=\"删除指定 {3}? \"  href=\"javascript:removeBind('{0}','{1}','{2}','{4}','{5}');\"  >[删除]</A> </LI>";

            SkinInfo skin = GetSkinInfoFromFile(skinFile);
            int i = 0;
            foreach (SkinInfo.SkinItem s in skin.Items)
            {
                if (string.Equals(s.Template, templateFile, StringComparison.OrdinalIgnoreCase))
                {
                    SkinInfo.SkinItem item = s;
                    item = ConvertOldVersionToNew(s);
                    i++;
                    sb.AppendLine(string.Format(bindLi, item.Type, item.Location, item.C_Model, item.C_ModelText + item.LocationText, templateFile,"li"+i.ToString()));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 刷新模板组中所有模板信息中的冗余字段 DefaultBindText
        /// </summary>
        public void RefreshTemplateDefaultBindText(SkinInfo skin)
        {
            Template[] tps = GetTemplates(null, Path.GetFileNameWithoutExtension(skin.FileName));
            foreach (Template t in tps)
            {
                RefreshOneTempplateBindText(skin, t);
            }
        }

        /// <summary>
        /// 刷新单个模板信息中的冗余字段 DefaultBindText
        /// </summary>
        /// <param name="skin"></param>
        /// <param name="t"></param>
        void RefreshOneTempplateBindText(SkinInfo skin, Template t)
        {
            string defaultBindText = "";
            foreach (SkinInfo.SkinItem s in skin.Items)
            {
                if (string.Equals(s.Template, t.FileName, StringComparison.OrdinalIgnoreCase))
                {
                    SkinInfo.SkinItem item = s;
                    item = ConvertOldVersionToNew(s);
                    if (defaultBindText == "")
                        defaultBindText = item.C_ModelText + item.LocationText;
                    else
                        defaultBindText += "," + item.C_ModelText + item.LocationText;
                }
            }
            t.DefaultBindText = defaultBindText;
            SaveTemplate(t, skin.FileName);
        }

        /// <summary>
        /// 兼容旧版2.6及以前版本
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        SkinInfo.SkinItem ConvertOldVersionToNew(SkinInfo.SkinItem item)
        {
            if (item.Type == "站点")
            {
                item.LocationText = item.Type + item.LocationText;
                item.Type = "site";
            }
            if (item.Type == "栏目")
            {

                if (string.IsNullOrEmpty(item.C_Model))
                {
                    item.Type = "channel";
                    item.LocationText = "通用" + item.LocationText;
                }
                else
                {
                    item.Type = "model";
                    item.LocationText = "模型" + item.LocationText;
                }
            }
            return item;
        }
        /// <summary>
        /// 删除模板指定项
        /// </summary>
        /// <param name="bc"></param>
        /// <param name="skinFolder"></param>
        /// <param name="fileName"></param>
        public void RemoveTemplateBind(TemplateBindConfig bc, string skinFolder, string fileName)
        {
            SkinInfo tg = GetSkinInfo(skinFolder + ".xml");//读取模板信息
            if (bc != null)
            {
                string handler = bc.Handler;
                string mode = bc.Mode;
                string model = bc.Model;

                foreach (SkinInfo.SkinItem item in tg.Items)
                {
                    SkinInfo.SkinItem skin = item;
                    skin = ConvertOldVersionToNew(item);
                    if (StringCompare(model, skin.C_Model) &&
                        StringCompare(mode, skin.Location) &&
                        StringCompare("", skin.Tag) &&
                        StringCompare(handler, skin.Type) &&
                        StringCompare(fileName, skin.Template))
                    {
                        tg.Items.Remove(skin);
                        break;
                    }
                }

                SaveSkinInfoAndPreviewFile(tg, Path.GetFileNameWithoutExtension(skinFolder));
                CreateMapFileFromSkinInfo(tg);
                Template t = GetTemplate(fileName, skinFolder);
                RefreshOneTempplateBindText(tg, t);
            }
            else
            {
                foreach (SkinInfo.SkinItem item in tg.Items)
                {
                    SkinInfo.SkinItem skin = item;
                    skin = ConvertOldVersionToNew(item);
                    if (StringCompare(fileName, skin.Template))
                    {
                        tg.Items.Remove(skin);
                        break;  //修正删除模板要删除两次的BUG。原因是集合修改后不能foreach，会有异常。Update By dl
                    }
                }
                SaveSkinInfoAndPreviewFile(tg, Path.GetFileNameWithoutExtension(skinFolder));
                CreateMapFileFromSkinInfo(tg);

                //清除模板列表缓存
                HttpContext Context = HttpContext.Current;
                string key = string.Format(TemplatesKeyID, skinFolder);
                Context.Cache.Remove(key);
            }
        }

        /// <summary>
        /// 获取模板可用来预览的url地址列表
        /// </summary>
        /// <param name="template">模板</param>
        /// <param name="skinFolder">模板组目录</param>
        /// <returns></returns>
        public List<string> GetPrevewUrls(string template,string skinFolder)
        {
            List<string> urls = TemplateMap.GetUrlListFromTemplate(template);
            List<string> realUrls = new List<string>();
            if (urls.Count == 0)
            {
                string skinfile = Path.Combine(HttpContext.Current.Server.MapPath("/" + GeneralConfigs.GetConfig().SiteSkinsBasePath + "/"), skinFolder + ".xml");
                if (File.Exists(skinfile))
                {
                    SkinInfo data = GetSkinInfo(skinfile);
                    ArrayList maplist = TemplateMap.CreateMapListFromTemplate(template, data);
                    foreach (TemplateMap.MapItem map in maplist)
                    {
                        urls.Add(map.Url);
                    }
                }
            }
            realUrls = CreateUrlsFromMaps(urls);
            return realUrls;
        }

        /// <summary>
        /// 根据地图列表生成URL列表
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        List<string> CreateUrlsFromMaps(List<string> urls)
        {
            List<string> realUrls = new List<string>();
            foreach (string url in urls)
            {
                string churl = url.Remove(url.LastIndexOf('/'));
                if (churl == "")
                {
                    realUrls.Add("/");
                    break;
                }
                if (!churl.EndsWith("/") && !churl.EndsWith("*")) churl += "/";
                List<Channel> channels = ChannelHelper.GetChannels(churl.Replace('*', '%'), 3);
                if (channels != null)
                {
                    foreach (Channel ch in channels)
                    {
                        if (url.EndsWith("list") && !realUrls.Contains(ch.ListUrl))
                            realUrls.Add(ch.ListUrl);
                        else if (url.EndsWith("search") && !realUrls.Contains(ch.SearchUrl))
                            realUrls.Add(ch.SearchUrl);
                        else if (url.EndsWith("detail"))
                        {
                            string[] fields = new string[] { "ID", "SN", "Updated", "ChannelFullUrl", "State" };
                            List<Article> al = ArticleHelper.GetArticlesByUrl(churl.Replace('*', '%'), 0, 1, fields, true);
                            if (al != null && al.Count > 0 && !realUrls.Contains(ch.FullUrl + al[0].FullUrl))
                                realUrls.Add(ch.FullUrl + al[0].FullUrl);
                        }
                        else if (!realUrls.Contains(ch.RealUrl))
                            realUrls.Add(ch.RealUrl);
                    }
                }
            }
            return realUrls;
        }

        /// <summary>
        /// 创建一个默认模板组
        ///     ~/_skin/Default
        /// </summary>
        public void CreateDefaultTemplateGroup()
        {
            //模板组名称:Default
            string groupName = "Default";
            string folderPath = HttpContext.Current.Server.MapPath("/" + string.Format("{0}\\{1}", Constants.SiteSkinsBasePath, groupName));

            //删除模板组
            if (Directory.Exists(folderPath))
            {
                DeleteTemplateGroup(folderPath);
                GeneralConfigInfo config = GeneralConfigs.GetConfig();
                if (config.DefaultTemplateGroupFileName.ToLower() == groupName)
                {
                    config.DefaultTemplateGroupFileName = "";
                    GeneralConfigs.SaveConfig(config);
                    GeneralConfigs.ResetConfig();
                }
            }

            //创建
            SkinInfo Data = new SkinInfo();
            Data.Name = groupName;
            Data.Description = "系统创建的默认模板组";
            Data.Ver = GeneralConfigs.GetConfig().ProductVersion;
            string fileName = "";
            if (CreateFolder(groupName))
            {
                fileName = SaveSkinInfoAndPreviewFile(Data, groupName);
                GeneralConfigInfo config = GeneralConfigs.GetConfig();
                config.DefaultTemplateGroupFileName = groupName + ".xml";
                GeneralConfigs.SaveConfig(config);
                GeneralConfigs.ResetConfig();                
            }
        }

        /// <summary>
        /// 创建存放文件夹
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        bool CreateFolder(string groupName)
        {
            string folderPath = HttpContext.Current.Server.MapPath("/" + string.Format("{0}\\{1}", Constants.SiteSkinsBasePath, groupName));
            string PreviewPath = HttpContext.Current.Server.MapPath("/" + string.Format("{0}\\~{1}", Constants.SiteSkinsBasePath, groupName));
            bool isCreate = false;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                //Directory.CreateDirectory(PreviewPath);
                isCreate = true;
            }
            return isCreate;
        }

        /// <summary>
        /// 检查general.config中的当前模板组是否物理存在
        /// </summary>
        /// <returns></returns>
        public string GetCurrentExistTemplateGroup()
        {
            string currentTemplateGroupName = GeneralConfigs.GetConfig().DefaultTemplateGroupFileName;
            if (!string.IsNullOrEmpty(currentTemplateGroupName))
            {
                string groupName = currentTemplateGroupName.Replace(".xml","");
                string groupXmlPath = HttpContext.Current.Server.MapPath("/" + string.Format("{0}\\{1}", Constants.SiteSkinsBasePath, currentTemplateGroupName));
                string groupPath = HttpContext.Current.Server.MapPath("/" + string.Format("{0}\\{1}", Constants.SiteSkinsBasePath, groupName));
                if (File.Exists(groupXmlPath) && Directory.Exists(groupPath))
                {
                    return currentTemplateGroupName;
                }
                else
                {
                     GeneralConfigInfo config = GeneralConfigs.GetConfig();
                     config.DefaultTemplateGroupFileName = "";
                     GeneralConfigs.SaveConfig(config);
                     GeneralConfigs.ResetConfig();
                }
            }
            return string.Empty;
        }
    }
}
