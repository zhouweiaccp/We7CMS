using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using We7.Framework.Util;
using We7.CMS.Module.VisualTemplate.Services;
using System.Web;
using We7.CMS;
using System.IO;
using We7.Framework.Config;
using System.Xml;
using System.Text.RegularExpressions;
namespace VisualDesign.Module
{
    public class VisualDesignFile
    {
        #region 属性字段

        /// <summary>
        /// 设计时模板物理路径
        /// </summary>
        public string DesignFile { get; set; }

        public string PublishFile { get; set; }

        /// <summary>
        /// 原始的文件Document
        /// </summary>
        public static HtmlDocument OriginalDocument
        {
            get;
            set;
        }
        /// <summary>
        /// 当前文件的Document
        /// </summary>
        public static HtmlDocument CurrentDocument
        {
            get;
            set;
        }

        /// <summary>
        /// 模板组
        /// </summary>
        public static string GroupTemplate
        {
            get;
            set;
        }

        /// <summary>
        /// 发布样式表文件
        /// </summary>
        public static string PublishStyle
        {
            get;
            set;
        }

        /// <summary>
        /// 发布之后的文档
        /// </summary>
        public static HtmlDocument PublicedDocument
        {
            get;
            set;
        }

        /// <summary>
        /// 发布的文档
        /// </summary>
        public static HtmlDocument PublicDocument
        {
            get;
            set;
        }

        public HtmlNode Body
        {
            get
            {
                return CurrentDocument.DocumentNode.SelectSingleNode("//body");
            }
        }

        public HtmlNode PageContainer
        {
            get
            {
                return GetElementById("pagecontainer");
            }
        }

        #endregion

        #region Contructor
        public VisualDesignFile(string templateGroup, string file)
        {
            DesignFile = HttpContext.Current.Server.MapPath
                (string.Format("~/_skins/~{0}/{1}", templateGroup, file));
            PublishFile = HttpContext.Current.Server.MapPath
                (string.Format("~/_skins/{0}/{1}", templateGroup, file));
            PublishStyle = HttpContext.Current.Server.MapPath(String.Format("~/_skins/{0}/Style/UxStyle.css", templateGroup));

            OriginalDocument = GetDocument(DesignFile);
            CurrentDocument = new HtmlDocument();

            CurrentDocument = GetDocumentFromHtml(OriginalDocument.DocumentNode.InnerHtml);
            GroupTemplate = templateGroup;
        }
        public VisualDesignFile(string file)
        {
            DesignFile = file;
            OriginalDocument = GetDocument(file);
            CurrentDocument = new HtmlDocument();

            CurrentDocument = GetDocumentFromHtml(OriginalDocument.DocumentNode.InnerHtml);
        }
        #endregion

        /// <summary>
        /// 合并样式表
        /// </summary>
        #region void CombineStyle()
        public void CombineStyle()
        {
            if (!File.Exists(PublishStyle))
            {
                if (!Directory.Exists(Path.GetDirectoryName(PublishStyle)))
                    Directory.CreateDirectory(Path.GetDirectoryName(PublishStyle));

                File.AppendAllText(PublishStyle, "", Encoding.UTF8);
            }


            Dictionary<string, string> styles = new Dictionary<string, string>();

            IEnumerable<HtmlNode> nodes = PublicedDocument.DocumentNode.DescendantNodes();

            foreach (HtmlNode node in nodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.Name.ToLower().StartsWith("wew"))
                    {
                        if (node.Attributes.Contains("cssclass") && !string.IsNullOrEmpty(node.Attributes["cssclass"].Value))
                        {
                            if (!styles.ContainsKey(node.Attributes["cssclass"].Value.Replace("_", ".")))
                                styles.Add(node.Attributes["cssclass"].Value.Replace("_", "."), node.Attributes["filename"].Value);
                        }
                    }
                }
            }

            if (styles.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in styles)
                {
                    UpdatePublishStyle(item.Key, item.Value);
                    DelFromPublishDoc(item.Key, item.Value);
                }
            }

            bool isExist = false;
            string uxStyle = String.Format("/_skins/{0}/Style/UxStyle.css", GroupTemplate);
            foreach (HtmlNode node in nodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.Name.ToLower().Equals("link"))
                    {
                        if (node.Attributes["href"].Value.Equals(PublishStyle))
                        {
                            isExist = true;
                            break;
                        }
                    }
                }
            }
            if (!isExist)
            {
                var node = CreateNode(string.Format("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\" />", uxStyle));
                PublicedDocument.DocumentNode.SelectSingleNode("//head").ChildNodes.Append(node);
            }
            PublicedDocument.Save(PublishFile, Encoding.UTF8);
        }
        #endregion

        /// <summary>
        /// 从发布的文档中删除
        /// </summary>
        /// <param name="style"></param>
        /// <param name="controlPath"></param>
        #region void DelFromPublishDoc(string style, string controlPath)
        private void DelFromPublishDoc(string style, string controlPath)
        {
            IEnumerable<HtmlNode> nodes = PublicedDocument.DocumentNode.DescendantNodes();
            style = GetStyleFile(style, controlPath);
            foreach (HtmlNode node in nodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.Name.ToLower().Equals("link"))
                    {
                        if (style.IndexOf(node.Attributes["href"].Value.Replace("/", "\\")) > -1)
                        {
                            node.ParentNode.RemoveChild(node);
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 更新发布的样式
        /// </summary>
        /// <param name="style"></param>
        /// <param name="controlPath"></param>
        #region void UpdatePublishStyle(string style, string controlPath)
        private void UpdatePublishStyle(string style, string controlPath)
        {
            style = style.Replace(".", "_");
            string uxStyle = File.ReadAllText(PublishStyle, Encoding.UTF8);
            string css = CssTxt(style, controlPath);
            string control = Path.GetFileNameWithoutExtension(controlPath);
            css = UpdateStyleImage(css, control, controlPath);
            TemplateStyleHelper styleHelper = new TemplateStyleHelper();
            css = styleHelper.LoadCss(control, style, css);
            uxStyle = styleHelper.ReplaceAppendCss(control, style, GroupTemplate, uxStyle, css);
            File.WriteAllText(PublishStyle, uxStyle, Encoding.UTF8);
        }
        #endregion

        /// <summary>
        /// 更新样式表图片地址
        /// </summary>
        /// <param name="cssTxt"></param>
        /// <param name="styleFileName"></param>
        /// <param name="controlPath"></param>
        #region string UpdateStyleImage(string cssTxt, string styleFileName, string controlPath)
        private string UpdateStyleImage(string cssTxt, string styleFileName, string controlPath)
        {
            string imageRootPath = Path.GetDirectoryName(controlPath);
            string imgRex = @"background\s*:\s*url[^\)]+";
            MatchCollection mc = Regex.Matches(cssTxt, imgRex, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
            if (mc.Count > 0)
            {
                foreach (Match it in mc)
                {
                    string image = Path.GetFileName(it.Value.Replace("\"",""));
                    cssTxt = cssTxt.Replace(it.Value, string.Format("background:url({0}", string.Format("{0}/Images/{1}", imageRootPath, image).Replace("\\", "/")));
                }
            }
            return cssTxt;
        }
        #endregion

        /// <summary>
        /// Css文本
        /// </summary>
        /// <param name="style"></param>
        /// <param name="controlPath"></param>
        /// <returns></returns>
        #region string CssTxt(string style, string controlPath)
        private string CssTxt(string style, string controlPath)
        {
            string styleFilePath = GetStyleFile(style, controlPath);
            if (!string.IsNullOrEmpty(styleFilePath))
            {
                using (StreamReader reader = new StreamReader(styleFilePath, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            return string.Empty;
        }
        #endregion

        /// <summary>
        /// 获取样式表文件
        /// </summary>
        /// <param name="style"></param>
        /// <param name="controlPath"></param>
        #region string GetStyleFile(string style, string controlPath)
        private string GetStyleFile(string style, string controlPath)
        {
            FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath(controlPath));
            DirectoryInfo[] dis = fi.Directory.GetDirectories("Style");
            DirectoryInfo di = (dis != null && dis.Length > 0) ? dis[0] : null;
            if (di != null)
            {
                FileInfo[] fs = di.GetFiles("*.css");
                if (fs != null && fs.Length > 0)
                {
                    FileInfo f = fs[0];
                    return f.FullName;
                }
            }
            return string.Empty;
        }
        #endregion

        /// <summary>
        /// 样式表
        /// </summary>
        /// <param name="style"></param>
        #region void Styles(string style)
        public void Styles(string style)
        {
            IEnumerable<HtmlNode> nodes = CurrentDocument.DocumentNode.DescendantNodes();
            bool isExist = false;
            foreach (HtmlNode node in nodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.Name.ToLower().Equals("link"))
                    {
                        if (node.Attributes["href"].Value.Equals(style))
                        {
                            isExist = true;
                            break;
                        }
                    }
                }
            }
            if (!isExist)
            {
                string fileName = HttpContext.Current.Server.MapPath(style);
                if (File.Exists(fileName))
                {
                    var node = CreateNode(string.Format("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\" />", style));
                    CurrentDocument.DocumentNode.SelectSingleNode("//head").ChildNodes.Append(node);
                }
            }
        }
        #endregion

        /// <summary>
        /// 添加或更新主题
        /// </summary>
        /// <param name="theme">主题名称</param>
        #region void AddOrUpdateThemeFile(string theme)
        public void AddOrUpdateThemeFile(string theme)
        {
            //在页面上样式的呈现顺序是先是系统提供的样式，再是自定义样式。即先是Style.css,再是UxStyle.css
            var themeNode = GetElementById("themestyle");
            if (themeNode != null)
            {
                themeNode.Attributes["href"].Value = string.Format("/Widgets/Themes/{0}/Style.css", theme);
            }
            else
            {
                var node = CreateNode(string.Format("<link href=\"/Widgets/Themes/{0}/Style.css\" type=\"text/css\" rel=\"stylesheet\" class=\"themestyle\" id=\"themestyle\" />", theme));

                CurrentDocument.DocumentNode.SelectSingleNode("//head").ChildNodes.Append(node);
            }

            //themeNode = GetElementById("uxstyle");
            //if (themeNode != null)
            //{
            //    themeNode.Attributes["href"].Value = string.Format("/Widgets/Skin/{0}/UxStyle.css", theme);
            //}
            //else
            //{
            //    var node = CreateNode(string.Format("<link href=\"/Widgets/Skin/{0}/UxStyle.css\" type=\"text/css\" rel=\"stylesheet\" class=\"uxstyle\" id=\"uxstyle\">", theme));
            //    CurrentDocument.DocumentNode.SelectSingleNode("//head").ChildNodes.Append(node);
            //}

            RegisterCommonFile();

            GeneralConfigs.GetConfig().Theme = theme;
            GeneralConfigs.SaveConfig(GeneralConfigs.GetConfig());
        }
        #endregion

        /// <summary>
        /// 注册公用文件
        /// </summary>
        #region void RegisterCommonFile()
        public void RegisterCommonFile()
        {
            string path = HttpContext.Current.Server.MapPath("~/Widgets/resource.xml");
            if (File.Exists(path))
            {
                XmlNodeList nodes = XmlHelper.GetXmlNodeList(path, "//add");
                foreach (XmlElement xe in nodes)
                {
                    string name = xe.GetAttribute("name");
                    string src = xe.GetAttribute("src");
                    string type = xe.GetAttribute("type");
                    var themeNode = GetElementById(name);
                    if ("script".Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        if (themeNode != null)
                        {
                            themeNode.Attributes["src"].Value = xe.GetAttribute("src");
                        }
                        else
                        {
                            var node = CreateNode(string.Format("<script src=\"{0}\" type=\"text/javascript\" class=\"{1}\" id=\"{1}\">", src, name));
                            CurrentDocument.DocumentNode.SelectSingleNode("//head").ChildNodes.Append(node);
                        }
                    }
                    else if ("style".Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        if (themeNode != null)
                        {
                            themeNode.Attributes["href"].Value = xe.GetAttribute("src");
                        }
                        else
                        {
                            var node = CreateNode(string.Format("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\" class=\"{1}\" id=\"{1}\">", src, name));
                            CurrentDocument.DocumentNode.SelectSingleNode("//head").ChildNodes.Append(node);
                        }
                    }
                }
            }
        }
        #endregion
        
        #region public methods

        public void ReplaceDomAttr(HtmlNode node, string attrString)
        {
            if (node.Attributes.Contains("style"))
            {
                node.Attributes["style"].Value = attrString;
            }
            else
            {
                node.Attributes.Append("style", attrString);
            }
        }

        public void Replace(string id, HtmlNode newNode)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id不能为空!");
            var oldNode = GetElementById(id);
            if (oldNode == null)
                throw new Exception("id为" + id + "的节点不存在");

            oldNode.ParentNode.ReplaceChild(newNode, oldNode);

            if (!GetWidgets().Contains(oldNode.Attributes["filename"].Value))
            {
                IEnumerable<HtmlNode> nodes = CurrentDocument.DocumentNode.DescendantNodes();
                foreach (HtmlNode it in nodes)
                {
                    if (it.NodeType == HtmlNodeType.Element)
                    {
                        if (it.Name.ToLower().Equals("link"))
                        {
                            if (it.Attributes["href"].Value.IndexOf(Path.GetDirectoryName(oldNode.Attributes["filename"].Value).Replace("\\", "/")) > -1)
                            {
                                it.ParentNode.RemoveChild(it);
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <param name="html"></param>
        public void Insert(string target, int index, string html)
        {
            var node = HtmlNode.CreateNode(html);
            var targetNode = CurrentDocument.GetElementbyId(target);

            if (targetNode == null)
            {
                targetNode = CurrentDocument.DocumentNode.SelectSingleNode("//body");
            }
            if (targetNode.ChildNodes.Count >= index)
            {
                targetNode.ChildNodes.Insert(index, node);
            }
            else
            {
                targetNode.ChildNodes.Append(node);
            }
        }
        /// <summary>
        /// 修改节点
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        public void Edit(string id, Dictionary<string, object> attributes)
        {
            var node = GetElementById(id);

            foreach (var attr in attributes)
            {
                node.SetAttributeValue(attr.Key, attr.Value.ToString());
            }
        }

        public void ReplaceNode(string id, HtmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("新节点不可为空!");

            var origalNode = GetElementById(id);

            if (origalNode == null)
                throw new ArgumentNullException("id为:" + id + "的节点未找到!");
            var parent = origalNode.ParentNode;
            var index = origalNode.ParentNode.ChildNodes.IndexOf(origalNode);

            Delete(origalNode);
            parent.ChildNodes.Insert(index, node);
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="node"></param>
        /// <param name="index"></param>
        public void Insert(string containerId, HtmlNode node, int index)
        {
            var container = GetElementById(containerId);
            if (container != null)
            {
                container.ChildNodes.Insert(index, node);
            }
        }

        public bool Insert(string target, HtmlNode newNode, string nextid, string temp)
        {
            try
            {
                if (!string.IsNullOrEmpty(nextid))
                {
                    var nextNode = GetElementById(nextid);
                    if (nextNode != null)
                    {
                        nextNode.ParentNode.InsertBefore(newNode, nextNode);
                        return true;
                    }
                }
                if (!string.IsNullOrEmpty(target))
                {
                    var parent = GetElementById(target);
                    if (parent != null)
                    {
                        parent.AppendChild(newNode);
                        return true;
                    }
                }


                return false;

            }
            catch
            {
                return false;
            }
        }
        public void Move(string target, string id, string nextId)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id:" + id + "为空");
            }
            var node = GetElementById(id);

            if (node == null)
            {
                throw new Exception("id为:" + id + "的节点不存在！");
            }

            var cloneNode = node.CloneNode(true);
            if (Insert(target, cloneNode, nextId, null))
            {
                Delete(node);
            }
        }
        public void Insert(string target, HtmlNode nextNode, HtmlNode newNode)
        {
            //先查找后节点,如果存在则插入在之前
            if (nextNode != null)
            {

            }
            var parentNode = GetElementById(target);


            if (parentNode == null)
            {
                throw new ArgumentNullException("id为" + target + "节点不存在!");
            }


        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="node"></param>
        public void Delete(HtmlNode node)
        {
            node.ParentNode.RemoveChild(node);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            var node = GetElementById(id);

            Delete(node);
            ///
            ///删除Link
            ///
            if (node.Attributes["filename"] != null)
            {
                if (!GetWidgets().Contains(node.Attributes["filename"].Value))
                {
                    IEnumerable<HtmlNode> nodes = CurrentDocument.DocumentNode.DescendantNodes();
                    foreach (HtmlNode it in nodes)
                    {
                        if (it.NodeType == HtmlNodeType.Element)
                        {
                            if (it.Name.ToLower().Equals("link"))
                            {
                                if (it.Attributes["href"].Value.IndexOf(Path.GetDirectoryName(node.Attributes["filename"].Value).Replace("\\", "/")) > -1)
                                {
                                    it.ParentNode.RemoveChild(it);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="targetContainerId"></param>
        /// <param name="node"></param>
        /// <param name="index"></param>
        public void Move(string targetContainerId, HtmlNode node, int index)
        {
            var nodeClone = node.CloneNode(true);
            Delete(node);
            Insert(targetContainerId, nodeClone, index);
        }

        public void Move(string targetContainerId, string widgetId, int index)
        {
            var node = GetElementById(widgetId);
            if (node != null)
            {
                Move(targetContainerId, node, index);
            }
        }
        /// <summary>
        /// 创建一个新的节点
        /// </summary>
        /// <param name="tagPrix"></param>
        /// <param name="tagName"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public HtmlNode CreateNode(string tagPrix, string tagName, IDictionary<string, object> attributes)
        {
            return CreateNode(tagPrix, tagName, attributes, true);
        }


        public HtmlNode CreateNode(string tagPrix, string tagName, IDictionary<string, object> attributes, bool server)
        {
            string name = string.Format("{0}:{1}", tagPrix, tagName);

            return CreateNode(name, attributes, server);
        }
        public HtmlNode CreateNode(string name, IDictionary<string, object> attributes, bool server)
        {
            HtmlNode node = new HtmlNode(HtmlNodeType.Element, CurrentDocument, 0);
            node.Name = name;
            if (attributes != null && attributes.Count > 0)
            {
                foreach (var item in attributes)
                {
                    node.Attributes.Add(item.Key, item.Value.ToString().Replace("&amp;nbsp;", " "));
                }
            }
            if (server)
            {
                if (!node.Attributes.Contains("runat"))
                {
                    node.Attributes.Add("runat", "server");
                }
            }
            return node;
        }

        /// <summary>
        /// HTML只含有唯一父节点
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public HtmlNode CreateNode(string html)
        {
            return HtmlNode.CreateNode(html);
        }
        /// <summary>
        /// 根据ID获取节点,如果是BODY则直接获取BODY
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HtmlNode GetElementById(string id)
        {
            if (string.Compare(id, "body", true) == 0)
            {
                return SelectSingleNode("//body");
            }
            return CurrentDocument.GetElementbyId(id);
        }
        /// <summary>
        /// 获取符合Xpath的第一个节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public HtmlNode SelectSingleNode(string xpath)
        {
            return CurrentDocument.DocumentNode.SelectSingleNode(xpath);
        }

        public void SaveDraft()
        {
            if (!We7.Framework.AppCtx.IsDemoSite)
            {
                CurrentDocument.Save(DesignFile, Encoding.UTF8);
            }
        }

        #endregion

        /// <summary>
        /// 获取HtmlDocument
        /// </summary>
        /// <param name="filePath"></param>
        #region HtmlDocument GetDocument(string filePath)
        private HtmlDocument GetDocument(string filePath)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionCheckSyntax = true;
            doc.OptionOutputOriginalCase = true;

            doc.Load(filePath);

            return doc;
        }
        #endregion

        /// <summary>
        /// 获取文档对象
        /// </summary>
        /// <param name="html"></param>
        #region HtmlDocument GetDocumentFromHtml(string html)
        private HtmlDocument GetDocumentFromHtml(string html)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionCheckSyntax = true;
            doc.OptionOutputOriginalCase = true;

            doc.LoadHtml(html);

            return doc;
        }
        #endregion

        /// <summary>
        /// 存草稿
        /// </summary>
        /// <param name="change"></param>
        #region void SaveDraft(bool change)
        public void SaveDraft(bool change)
        {
            var html = SelectSingleNode("//html").OuterHtml;

            var tags = GetWidgets();
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");

            foreach (string c in tags)
            {
                string path = c;
                string uc = Path.GetFileNameWithoutExtension(c).Replace(".", "_");
                if (String.IsNullOrEmpty(path) || String.IsNullOrEmpty(uc))
                    continue;
                sb.AppendLine(string.Format("<%@ Register Src=\"" + path + "\" TagName=\"{0}\" TagPrefix=\"wew\" %>\r\n", uc));
            }
            sb.Append(html);
            if (!We7.Framework.AppCtx.IsDemoSite)
            {
                File.WriteAllText(DesignFile, sb.ToString(), Encoding.UTF8);
            }
        }
        #endregion

        /// <summary>
        /// 发布
        /// </summary>
        #region void Publish()
        public void Publish()
        {
            if (!File.Exists(DesignFile))
            {
                throw new Exception("可视化设计文件不存在!");
            }
            //简单拷贝
            FileHelper.Copy(DesignFile, PublishFile);

            PublicDocument = GetDocument(PublishFile);
            PublicedDocument = new HtmlDocument();
            PublicedDocument = GetDocumentFromHtml(PublicDocument.DocumentNode.InnerHtml);
        }
        #endregion

        /// <summary>
        /// 获取部件
        /// </summary>
        #region List<string> GetWidgets()
        private List<string> GetWidgets()
        {
            var widgets = new List<string>();
            IEnumerable<HtmlNode> nodes = CurrentDocument.DocumentNode.DescendantNodes();

            foreach (HtmlNode node in nodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.Name.ToLower().StartsWith("wew"))
                    {
                        if (node.Attributes.Contains("filename"))
                        {
                            //widget
                            if (node.Attributes["filename"] != null)
                            {
                                var filename = node.Attributes["filename"].Value;
                                if (!string.IsNullOrEmpty(filename))
                                {
                                    if (!widgets.Contains(filename))
                                    {
                                        widgets.Add(filename);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return widgets;

        }
        #endregion       

    }
}
