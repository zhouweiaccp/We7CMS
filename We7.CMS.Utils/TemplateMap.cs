using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using We7.Framework.Config;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS
{
    /// <summary>
    /// 栏目映射模板地图类
    /// </summary>
    public class TemplateMap
    {
        #region 内部属性和方法
        private static object lockHelper = new object();
        private static volatile TemplateMap instance = null;

        string TemplatesMapFile = HttpContext.Current.Server.MapPath("~/_skins/templates.map");
        private System.Collections.ArrayList _Maps;
        /// <summary>
        /// 模板数据数组
        /// </summary>
        public System.Collections.ArrayList Maps
        {
            get
            {
                return _Maps;
            }
            set
            {
                _Maps = value;
            }
        }

        private System.Collections.Specialized.NameValueCollection _Templates;
        public System.Collections.Specialized.NameValueCollection Templates
        {
            get
            {
                return _Templates;
            }
            set
            {
                _Templates = value;
            }
        }

        private TemplateMap()
        {
            Maps = new System.Collections.ArrayList();
            Templates = new System.Collections.Specialized.NameValueCollection();
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
            string skinMap = gi.DefaultTemplateGroupFileName;
            if (!string.IsNullOrEmpty(skinMap))
            {
                skinMap = skinMap.Remove(skinMap.ToLower().IndexOf(".xml"));
                skinMap = Path.Combine(HttpContext.Current.Server.MapPath("~/" + gi.SiteSkinsBasePath), skinMap + ".map");
                LoadMaps(skinMap, Maps);
                LoadMaps(TemplatesMapFile, Maps);
            }
        }

        /// <summary>
        /// 模板地图构造方法
        /// </summary>
        /// <param name="mapFile">地图文件</param>
        public TemplateMap(string mapFile)
        {
            Maps = new System.Collections.ArrayList();
            Templates = new System.Collections.Specialized.NameValueCollection();
            LoadMaps(mapFile,Maps);
        }

        /// <summary>
        /// 从文件中加载地图数据
        /// </summary>
        /// <param name="mapfile">地图文件</param>
        /// <param name="maps">地图数据数组</param>
        private static void LoadMaps(string mapfile, ArrayList maps)
        {
            if (File.Exists(mapfile))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(mapfile);
                XmlNode root = xml.SelectSingleNode("items");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "item")
                    {
                        XmlAttribute name = n.Attributes["url"];
                        XmlAttribute page = n.Attributes["template"];

                        if (name != null && page != null)
                        {
                            //Templates.Add(name.Value, page.Value);
                            if(Array.Find<MapItem>((MapItem[])maps.ToArray(typeof(MapItem)),(MapItem item)=>String.Compare(item.Url,name.Value,true)==0)==null)
                                maps.Add(new MapItem(name.Value, page.Value.Replace("^", "&")));
                        }
                    }
                }
            }
            //else
            //    throw new Exception(mapfile + "不存在！");
        }

        /// <summary>
        /// 根据栏目URL获取对应的所有栏目绑定模板列表
        /// </summary>
        /// <param name="channelUrl">栏目Url</param>
        /// <returns>栏目绑定模板</returns>
        public ChannelTemplateGroup GetChannelTemplate(string channelUrl)
        {
            if (!channelUrl.EndsWith("/")) channelUrl += "/";
            ChannelTemplateGroup ctg = new ChannelTemplateGroup();
            foreach (MapItem mi in Maps)
            {
                if (mi.Url == channelUrl || mi.Url == channelUrl + "*/")
                    ctg.IndexTemplate = mi.Template;
                if (mi.Url == channelUrl + "*/")
                    ctg.IndexInherit = true;

                if (mi.Url == channelUrl + "*/list" || mi.Url == channelUrl + "list")
                    ctg.ListTemplate = mi.Template;
                if (mi.Url == channelUrl + "*/list")
                    ctg.ListInherit = true;

                if (mi.Url == channelUrl + "*/detail" || mi.Url == channelUrl + "detail")
                    ctg.DetailTemplate = mi.Template;
                if (mi.Url == channelUrl + "*/detail")
                    ctg.DetailInherit = true;

                if (mi.Url == channelUrl + "*/search" || mi.Url == channelUrl + "search")
                    ctg.SearchTemplate = mi.Template;
                if (mi.Url == channelUrl + "*/search")
                    ctg.SearchInherit = true;
            }
            return ctg;
        }

        /// <summary>
        /// 栏目的绑定模板列表保存到地图文件
        /// </summary>
        /// <param name="cg">栏目的绑定模板</param>
        /// <param name="channelUrl">栏目URL</param>
        /// <param name="file">文件</param>
        public static void SaveToTemplateMapFile(ChannelTemplateGroup cg, string channelUrl, string file)
        {
            ArrayList myMaps = new ArrayList();
            LoadMaps(file, myMaps);
            if (!channelUrl.EndsWith("/")) channelUrl += "/";
            myMaps = RemoveMap(myMaps, channelUrl);
            MapItem map = new MapItem();
            if (!string.IsNullOrEmpty(cg.IndexTemplate))
            {
                string url = channelUrl;
                if (cg.IndexInherit) url += "*/";
                myMaps.Add(new MapItem(url, cg.IndexTemplate));
            }
            if (!string.IsNullOrEmpty(cg.ListTemplate))
            {
                string url = channelUrl;
                if (cg.ListInherit) url += "*/";
                url = url + "list";
                myMaps.Add(new MapItem(url, cg.ListTemplate));
            }
            if (!string.IsNullOrEmpty(cg.DetailTemplate))
            {
                string url = channelUrl;
                if (cg.DetailInherit) url += "*/";
                url = url + "detail";
                myMaps.Add(new MapItem(url,cg.DetailTemplate));
            }
            if (!string.IsNullOrEmpty(cg.SearchTemplate))
            {
                string url = channelUrl;
                if (cg.SearchInherit) url += "*/";
                url = url + "search";
                myMaps.Add(new MapItem(url, cg.SearchTemplate));
            }

            SaveToMapFile(myMaps, file);
        }

        static ArrayList RemoveMap(ArrayList maps, string url)
        {
            ArrayList myArray = new ArrayList();
            foreach (MapItem map in maps)
            {
                if (map.Url.ToLower() != url && map.Url.ToLower() != url + "*/"
                    && map.Url.ToLower() != url + "list" && map.Url.ToLower() != url + "*/list"
                    && map.Url.ToLower() != url + "detail" && map.Url.ToLower() != url + "*/detail"
                    && map.Url.ToLower() != url + "search" && map.Url.ToLower() != url + "*/search")
                    myArray.Add(map);
            }
            return myArray;
        }
        /// <summary>
        /// 保存到地图文件中
        /// </summary>
        /// <param name="maps">地图数据数组</param>
        /// <param name="file">文件</param>
        public static void SaveToMapFile(ArrayList maps, string file)
        {
            HttpContext Context = HttpContext.Current;
            if (Context != null)
            {
                if (File.Exists(file)) File.Delete(file);
                XmlDocument doc = new XmlDocument();
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<items>{0}</items>";
                string items = "";
                foreach (MapItem map in maps)
                {
                    string itemString = "<item url=\"{0}\" template=\"{1}\" />\r\n";
                    items += string.Format(itemString, map.Url, map.Template);
                }
                xml = string.Format(xml, items);
                doc.LoadXml(xml);
                doc.Save(file);
            }
        }

        #endregion

        #region 静态变量
        /// <summary>
        /// 获取模板地图实例
        /// </summary>
        /// <returns></returns>
        public static TemplateMap GetTemplateMap()
        {
            if (instance == null)
            {
                lock (lockHelper)
                {
                    if (instance == null)
                    {
                        instance = new TemplateMap();
                    }
                }
            }
            return instance;

        }

        /// <summary>
        /// 设置实例
        /// </summary>
        /// <param name="anInstance"></param>
        public static void SetInstance(TemplateMap anInstance)
        {
            if (anInstance != null)
                instance = anInstance;
        }

        /// <summary>
        /// 初始化实例
        /// </summary>
        public static void SetInstance()
        {
            SetInstance(new TemplateMap());
        }

        /// <summary>
        /// 重置，清空实例
        /// </summary>
        public static void ResetInstance()
        {
            instance = null;
        }

        /// <summary>
        /// 从模版地图获取url的匹配模板文件
        /// </summary>
        /// <param name="columnMode">模式，如“list，detail，search"等</param>
        /// <param name="requestPath">栏目url</param>
        /// <returns></returns>
        public static string GetTemplateFromMap(string columnMode,string requestPath)
        {
            string url = requestPath + columnMode;
            MapItem theMap = null;
            foreach (MapItem map in GetTemplateMap().Maps)
            {
                string regexString = "^" + map.Url.Replace("/*", @"((\w|(-)|(_)|\/)*)") + "$";
                if (Regex.IsMatch(url, regexString, RegexOptions.IgnoreCase))
                {
                    if (theMap == null)
                        theMap = map;
                    else
                    {
                        string lastResult = theMap.Url.Remove(theMap.Url.LastIndexOf('/'));
                        if (lastResult.EndsWith("*")) lastResult = lastResult.Remove(lastResult.Length - 1);
                        string thisResult = map.Url.Remove(map.Url.LastIndexOf('/'));
                        if (thisResult.StartsWith(lastResult, true, null))
                            theMap = map;
                    }
                }
            }
            if (theMap == null)
                return null;
            else
                return theMap.Template;
        }

        /// <summary>
        /// 获取模板映射的URL列表
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static List<string> GetUrlListFromTemplate(string template)
        {
            List<string> urls = new List<string>();
            foreach (MapItem map in GetTemplateMap().Maps)
            {
                if (template.Trim().ToLower()==map.Template.Trim().ToLower())
                {
                    urls.Add(map.Url);
                }
            }
            return urls;
        }

        /// <summary>
        /// 替换Url，用于栏目移动等操作
        /// </summary>
        /// <param name="file"></param>
        /// <param name="oldUrl"></param>
        /// <param name="newUrl"></param>
        public static void ReplaceChannelUrls(string file, string oldUrl, string newUrl)
        {
            System.Collections.ArrayList myMaps=new ArrayList();
            LoadMaps(file, myMaps);
            foreach (MapItem map in myMaps)
            {
                map.Url = map.Url.Replace(oldUrl, newUrl);
            }
            SaveToMapFile(myMaps, file);
        }

        /// <summary>
        /// 替换Url，用于栏目移动等操作
        /// </summary>
        /// <param name="oldUrl"></param>
        /// <param name="newUrl"></param>
        public static void ReplaceChannelUrls(string oldUrl, string newUrl)
        {
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
            string skinMap = gi.DefaultTemplateGroupFileName;
            if (skinMap != "") skinMap = skinMap.Remove(skinMap.ToLower().IndexOf(".xml"));
            skinMap = Path.Combine(HttpContext.Current.Server.MapPath("~/" + gi.SiteSkinsBasePath), skinMap + ".map");
            ReplaceChannelUrls(skinMap, oldUrl, newUrl);
        }

        /// <summary>
        /// 删除Url项目，用于栏目删除等操作
        /// </summary>
        /// <param name="file"></param>
        /// <param name="delUrl"></param>
        public static void DeleteChannelUrls(string file, string delUrl)
        {
            ArrayList myMaps = new ArrayList();
            LoadMaps(file, myMaps);
            ArrayList newMaps = new ArrayList();
            foreach (MapItem map in myMaps)
            {
                if (!map.Url.StartsWith(delUrl)) newMaps.Add(map);
            }
            SaveToMapFile(newMaps, file);
        }
        public static void DeleteChannelUrls(string delUrl)
        {
            GeneralConfigInfo gi = GeneralConfigs.GetConfig();
            string skinMap = gi.DefaultTemplateGroupFileName;
            if (skinMap != "") skinMap = skinMap.Remove(skinMap.ToLower().IndexOf(".xml"));
            skinMap = Path.Combine(HttpContext.Current.Server.MapPath("~/" + gi.SiteSkinsBasePath), skinMap + ".map");
            DeleteChannelUrls(skinMap, delUrl);
        }


        /// <summary>
        /// 根据模板项列表生成地图列表
        /// </summary>
        /// <param name="skinItems"></param>
        /// <returns></returns>
        public static ArrayList CreateMapList(List<SkinInfo.SkinItem> skinItems)
        {
            ChannelHelper ChannelHelper = HelperFactory.Instance.GetHelper<ChannelHelper>();
            ArrayList mapList = new ArrayList();
            List<string> urlNames = new List<string>();
            foreach (SkinInfo.SkinItem item in skinItems)
            {
                if (item.Type == "栏目" || item.Type == "channel")
                {
                    if (!string.IsNullOrEmpty(item.C_Model))
                    {
                        List<Channel> channels = ChannelHelper.GetChannelByModelName(item.C_Model);
                        foreach (Channel ch in channels)
                        {
                            string url = TemplateMap.GenerateMapUrl(ch.FullUrl, item.Location);
                            mapList.Add(new TemplateMap.MapItem(url, item.Template));
                            urlNames.Add(url);
                        }
                    }
                    else
                    {
                        string url = TemplateMap.GenerateMapUrl("/*/", item.Location);
                        mapList.Add(new TemplateMap.MapItem(url, item.Template));
                        urlNames.Add(url);
                    }

                    if (!string.IsNullOrEmpty(item.Tag))
                    {
                        List<Channel> channels = ChannelHelper.GetChannelsByTags(item.Tag.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries));
                        foreach (Channel ch in channels)
                        {
                            string url = TemplateMap.GenerateMapUrl(ch.FullUrl, item.Location);
                            if (!urlNames.Contains(url))
                            {
                                mapList.Add(new TemplateMap.MapItem(url, item.Template));
                                urlNames.Add(url);
                            }
                        }
                    }
                }
                else if (item.Type == "站点" || item.Type == "site")
                {
                    string url = TemplateMap.GenerateMapUrl("/", item.Location);
                    if (!urlNames.Contains(url))
                    {
                        mapList.Add(new TemplateMap.MapItem(url, item.Template));
                        urlNames.Add(url);
                    }
                }
            }

            return mapList;
        }

        /// <summary>
        /// 创建模板地图
        /// </summary>
        /// <param name="template">模板组</param>
        /// <param name="skin">模板组皮肤信息</param>
        /// <returns></returns>
        public static ArrayList CreateMapListFromTemplate(string template, SkinInfo skin)
        {
            List<SkinInfo.SkinItem> myItems = new List<SkinInfo.SkinItem>();
            foreach (SkinInfo.SkinItem item in skin.Items)
            {
                if (item.Template.ToLower() == template.ToLower())
                    myItems.Add(item);
            }
            ArrayList mapList = CreateMapList(myItems);
            return mapList;
        }

        #endregion

        /// <summary>
        /// 模版映射项
        /// </summary>
        public class MapItem
        {
            #region 成员变量
            public string Url { get; set; }
            public string Template { get; set; }
            #endregion

            #region 构造函数
            public MapItem() { }
            public MapItem(string name, string page)
            {
                Url = name;
                Template = page;
            }
            #endregion
        }

        /// <summary>
        /// 根据url与位置生成url表达式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static string GenerateMapUrl(string url, string pos)
        {
            if (!url.EndsWith("/")) url += "/";
            pos = pos.Replace("index", "");
            pos = pos.Replace("default", "");
            return url + pos;
        }
    }

    /// <summary>
    /// 栏目的绑定模板
    /// </summary>
    public class ChannelTemplateGroup
    {
        /// <summary>
        /// 索引模板（栏目首页）
        /// </summary>
        public string IndexTemplate { get; set; }
        /// <summary>
        /// 列表模板
        /// </summary>
        public string ListTemplate { get; set; }
        /// <summary>
        /// 详细页模板
        /// </summary>
        public string DetailTemplate { get; set; }
        /// <summary>
        /// 搜索页模板
        /// </summary>
        public string SearchTemplate { get; set; }
        /// <summary>
        /// 索引模板继承
        /// </summary>
        public bool IndexInherit { get; set; }
        /// <summary>
        /// 列表模板继承
        /// </summary>
        public bool ListInherit { get; set; }
        /// <summary>
        /// 详细页模板继承
        /// </summary>
        public bool DetailInherit { get; set; }
        /// <summary>
        /// 搜索页模板继承
        /// </summary>
        public bool SearchInherit { get; set; }
    }
}
