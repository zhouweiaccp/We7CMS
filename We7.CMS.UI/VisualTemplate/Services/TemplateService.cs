using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using We7.Framework.Config;

namespace We7.CMS.Module.VisualTemplate.Services
{
    public class TemplateService
    {
        #region Const

        private const string WeControlTagprifix = "wec";
        private const string SubControlTagprfix = "wet";

        #endregion

        #region 属性

        /// <summary>
        /// 原始模板
        /// </summary>
        public TemplateService OriginalTemplate
        {
            get;
            set;
        }
        /// <summary>
        /// 当前模板内容的所有控件信息
        /// </summary>
        public List<WeControl> Controls
        {
            get;
            set;
        }
        /// <summary>
        /// 子模板文件名称（不带扩展名）
        /// </summary>
        public List<string> SubTemplates
        {
            get;
            set;

        }

        /// <summary>
        /// 模板内容(HTML标签部分)
        /// </summary>
        public string TemplateHtmlContent
        {
            get;
            set;
        }

        public HtmlDocument Document
        {
            get;
            set;
        }
        /// <summary>
        /// head内容
        /// </summary>
        public string HeadContent
        {
            get;
            set;
        }
        /// <summary>
        /// BODY内容
        /// </summary>
        public string BodyContent
        {
            get;
            set;
        }
        #endregion

        #region 构造函数

        private TemplateService()
        {
            Controls = new List<WeControl>();
            SubTemplates = new List<string>();
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// 文件夹名称
        /// </summary>
        public string Folder
        {
            get;
            set;
        }
        public TemplateService(HtmlDocument document)
            : this()
        {
            Document = document;
            InitControls();

        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 保存模版
        /// </summary>
        public void SaveTemplate(string filePath)
        {
            StringBuilder sb = new StringBuilder();

            //整理控件TAG
            List<string> tags = new List<string>();
            foreach (WeControl c in Controls)
            {
                if (!tags.Contains(c.Control))
                    tags.Add(c.Control);
            }

            sb.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");

            foreach (string c in tags)
            {
                string path = GetControlRelatePath(c);
                string uc = GetUCName(path);
                if (String.IsNullOrEmpty(path) || String.IsNullOrEmpty(uc))
                    continue;
                sb.AppendLine(string.Format("<%@ Register Src=\"" + path + "\" TagName=\"{0}\" TagPrefix=\"wec\" %>", uc));
            }

            foreach (String c in SubTemplates)
            {
                sb.AppendLine(string.Format("<%@ Register Src=\"{0}.ascx\" TagName=\"{0}\" TagPrefix=\"wet\" %>", c));
            }

            sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            sb.AppendLine("<head runat=\"server\">");
            sb.Append("<title></title>");
            HeadContent = FormatHeadMeta(HeadContent);
            sb.AppendLine(HeadContent);
            sb.AppendLine("</head>");
            sb.AppendLine("<body id=\"body\">");
            sb.AppendLine(BodyContent);
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            //写入文件
            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
        /// <summary>
        /// 初始化headContent,bodyContent
        /// </summary>
        public void InitContent()
        {
            var head = Document.DocumentNode.SelectSingleNode("//head");
            if (head != null)
            {
                HeadContent = head.InnerHtml;
            }

            var body = Document.DocumentNode.SelectSingleNode("//body");
            if (body != null)
            {
                BodyContent = body.InnerHtml;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControls()
        {
            var nodes = Document.DocumentNode.DescendantNodes();

            foreach (var node in nodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    //节点类型
                    if (node.Name.ToLower().StartsWith(WeControlTagprifix))
                    {
                        //We7控件
                        WeControl control = new WeControl();
                        control.ID = node.Id;
                        control.TagName = node.Name.Remove(0, 4).Replace('_', '.');//替换

                        if (node.Attributes.Contains("cssclass"))
                        {
                            control.Style = node.Attributes["cssclass"].Value;
                        }
                        //添加
                        if (!Controls.Contains(control))
                        {
                            Controls.Add(control);
                        }
                    }
                    else if (node.Name.ToLower().StartsWith(SubControlTagprfix))
                    {
                        //子模板
                        string subName = node.Name.Remove(0, 4).Replace("_", ".");

                        if (!SubTemplates.Contains(subName))
                        {
                            SubTemplates.Add(subName);
                        }
                    }
                }
            }
        }



        /// <summary>
        /// 取得控件名称
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <returns></returns>
        string GetUCName(string filePath)
        {
            //return control.Replace(".", "_");
            string ctr = Path.GetFileNameWithoutExtension(filePath);
            if (!String.IsNullOrEmpty(ctr))
                return ctr.Replace(".", "_");
            return String.Empty;
        }

        /// <summary>
        /// 取得控件与根目录的相对路径
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <returns></returns>
        string GetControlRelatePath(string control)
        {
            string[] strs = control.Split('.');
            string path = String.Format("{0}/{1}/Page/{2}.ascx", Constants.We7ControlsBasePath, strs[0], control);
            return path;
        }
        /// <summary>
        /// 当前模版组相对根目录的样式表路径.
        /// </summary>
        public string We7TemplateStylePath
        {
            get
            {
                return string.Format("_skins/{0}/css/We7Control.css", We7TemplateGroup);
            }
        }

        /// <summary>
        /// 模版组
        /// </summary>
        public string We7TemplateGroup
        {
            get
            {
                //if (String.IsNullOrEmpty(we7TemplateGroup))
                //    throw new Exception("当前模版组为空");
                //return we7TemplateGroup;
                return "visualtemplate";
            }
        }

        Dictionary<string, int> oldDic = new Dictionary<string, int>();
        Dictionary<string, int> newDic = new Dictionary<string, int>();

        void SortControl()
        {
            foreach (WeControl c in Controls)//新控件的样式数
            {
                //string key = String.Format("{0}_{1}", c.Control, c.Style);
                string key = String.Format("{0}_{1}", c.FileName, c.Style);
                newDic[key] = newDic.ContainsKey(key) ? newDic[key] + 1 : 1;
            }

            foreach (WeControl c in OriginalTemplate.Controls) //旧控件样式数
            {
                //string key = String.Format("{0}_{1}", c.Control, c.Style);
                string key = String.Format("{0}_{1}", c.FileName, c.Style);
                oldDic[key] = oldDic.ContainsKey(key) ? oldDic[key] + 1 : 1;
            }

            List<string> strl = new List<string>();
            List<string> strl2 = new List<string>();
            foreach (string key in oldDic.Keys)
            {
                if (newDic.ContainsKey(key))
                    strl.Add(key);
            }

            foreach (string key in newDic.Keys)
            {
                if (oldDic.ContainsKey(key))
                    strl2.Add(key);
            }

            foreach (string s in strl)
            {
                oldDic.Remove(s);
            }

            foreach (string s in strl2)
            {
                newDic.Remove(s);
            }
        }

        public void SaveStyle()
        {
            try
            {
                OriginalTemplate.InitControls();

                SortControl();
                string cssPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7TemplateStylePath);
                FileInfo cssfile = new FileInfo(cssPath);

                string csstxt = "";
                if (CheckFile(cssfile))
                {
                    using (StreamReader reader = OpenFile(cssfile.FullName))
                    {
                        csstxt = reader.ReadToEnd();
                    }
                }

                TemplateStyleHelper styleHelper = new TemplateStyleHelper();

                foreach (KeyValuePair<string, int> kvp in oldDic) //处理过期的控件样式
                {
                    string path = kvp.Key.Split('_')[0];
                    string style = kvp.Key.Split('_')[1];
                    string control = Path.GetFileNameWithoutExtension(path).Trim();
                    csstxt = styleHelper.DeleteStyle(control, style.Trim(), Path.GetFileNameWithoutExtension(FileName), csstxt);
                }

                foreach (KeyValuePair<string, int> kvp in newDic) //处理新增的控件样式
                {
                    string path = kvp.Key.Split('_')[0];
                    string style = kvp.Key.Split('_')[1];
                    csstxt = styleHelper.AddStyleByPath(path, style.Trim(), Path.GetFileNameWithoutExtension(FileName), csstxt);
                }
                using (FileStream f = File.Open(cssPath, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = OpenWriteFile(f))
                    {
                        writer.Write(csstxt);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 格式化Html头部信息
        /// </summary>
        /// <param name="headContent">头部内容</param>
        /// <returns></returns>
        string FormatHeadMeta(string headContent)
        {
            //删除已有title标签
            StringBuilder sb = new StringBuilder(headContent);
            AbstractAndRemoveMatchSections(@"\<title[^>]*\>([\w\W]*?)\</title\>", sb);
            headContent = sb.ToString();

            //Content-Type：网页编码
            string rs = @"\<(meta)([^>]*)(content)\s*=\s*""([^>]*)""([^>]*)(http-equiv)\s*=\s*""(content-type)""([^>]*)\>\s*";
            string rs2 = @"\<(meta)([^>]*)(http-equiv)\s*=\s*""(content-type)""([^>]*)(content)\s*=\s*""([^>]*)""([^>]*)\>\s*";
            string strMeta = "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";

            Regex reg = new Regex(rs2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            headContent = reg.Replace(headContent, "");
            reg = new Regex(rs, RegexOptions.IgnoreCase);
            headContent = reg.Replace(headContent, "");
            headContent = headContent.Trim();

            headContent += "\r\n" + strMeta;

            //meta：描述description
            rs = @"\<(meta)([^>]*)(content)\s*=\s*""([^>]*)""([^>]*)(name)\s*=\s*""(description)""([^>]*)\>\s*";
            rs2 = @"\<(meta)([^>]*)([^>]*)(name)\s*=\s*""(description)""(content)\s*=\s*""([^>]*)""([^>]*)\>\s*";
            strMeta = "";

            reg = new Regex(rs2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            headContent = reg.Replace(headContent, "");
            reg = new Regex(rs, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            headContent = reg.Replace(headContent, "");
            headContent = headContent.Trim();

            //meta：关键词keywords
            rs = @"\<(meta)([^>]*)(content)\s*=\s*""([^>]*)""([^>]*)(name)\s*=\s*""(keywords)""([^>]*)\>\s*";
            rs2 = @"\<(meta)([^>]*)(name)\s*=\s*""(keywords)""([^>]*)(content)\s*=\s*""([^>]*)""([^>]*)\>\s*";
            strMeta = "";

            reg = new Regex(rs2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            headContent = reg.Replace(headContent, "");
            reg = new Regex(rs, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            headContent = reg.Replace(headContent, "");
            headContent = headContent.Trim();

            //meta：描述author
            rs = @"\<(meta)([^>]*)(content)\s*=\s*""([^>]*)""([^>]*)(name)\s*=\s*""(author)""([^>]*)\>\s*";
            rs2 = @"\<(meta)([^>]*)(name)\s*=\s*""(author)""([^>]*)(content)\s*=\s*""([^>]*)""([^>]*)\>\s*";
            strMeta = "<meta content=\"We7\" name=\"author\" /> ";

            reg = new Regex(rs2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            headContent = reg.Replace(headContent, "");
            reg = new Regex(rs, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            headContent = reg.Replace(headContent, "");

            headContent = headContent.Trim();
            headContent += "\r\n" + strMeta;

            //meta：描述generator
            rs = @"\<(meta)([^>]*)(content)\s*=\s*""([^>]*)""([^>]*)(name)\s*=\s*""(generator)""([^>]*)\>\s*";
            rs2 = @"\<(meta)([^>]*)(name)\s*=\s*""(generator)""([^>]*)(content)\s*=\s*""([^>]*)""([^>]*)\>\s*";
            strMeta = "<meta content=\"We7 {0}\" name=\"generator\" /> ";
            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            strMeta = string.Format(strMeta, ci.ProductVersion);

            reg = new Regex(rs2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            headContent = reg.Replace(headContent, "");
            reg = new Regex(rs, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            headContent = reg.Replace(headContent, "");
            headContent = headContent.Trim();

            headContent += "\r\n" + strMeta;

            //meta：描述copyright
            rs = @"\<(meta)([^>]*)(content)\s*=\s*""([^>]*)""([^>]*)(name)\s*=\s*""(copyright)""([^>]*)\>\s*";
            rs2 = @"\<(meta)([^>]*)(name)\s*=\s*""(copyright)""([^>]*)(content)\s*=\s*""([^>]*)""([^>]*)\>\s*";
            strMeta = "<meta content=\"Copyright (c) 2010 WestEngine Inc. All Rights Reserved.\" name=\"copyright\" /> ";

            reg = new Regex(rs2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            headContent = reg.Replace(headContent, "");
            reg = new Regex(rs, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            headContent = reg.Replace(headContent, "");

            headContent = headContent.Trim();
            headContent += "\r\n" + strMeta;

            return headContent.Trim() + "\r\n";
        }

        /// <summary>
        /// 依据正则表达式字串，将原字符串中相符合规则的内容提取出来
        /// 并把相关内容从原符串中删除
        /// </summary>
        /// <param name="regexStr">正则表达式字串</param>
        /// <param name="sourceStr">原字符串</param>
        /// <returns>提取出的字符串</returns>
        private string AbstractAndRemoveMatchSections(string regexStr, StringBuilder sourceStr)
        {
            string result = "";
            Regex r = new Regex(regexStr, RegexOptions.IgnoreCase);

            MatchCollection matchs = r.Matches(sourceStr.ToString());
            foreach (Match m in matchs)
            {
                if (result.ToLower().IndexOf(m.Value.ToLower()) < 0)  //判断重复
                {
                    result += m.Value;
                    result += "\r\n";
                }
            }

            MatchEvaluator mav = new MatchEvaluator(Cleanup);
            string tempStr = r.Replace(sourceStr.ToString(), mav);
            sourceStr.Remove(0, sourceStr.Length);
            sourceStr.Append(tempStr);

            return result;

        }
        string Cleanup(Match m)
        {
            return String.Empty;
        }

        /// <summary>
        /// 通过路径取得StreamReader
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        static StreamReader OpenFile(string path)
        {
            return new StreamReader(path, Encoding.UTF8);
        }

        /// <summary>
        /// 通过流取得StreamReader
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        static StreamReader OpenFile(Stream stream)
        {
            return new StreamReader(stream, Encoding.UTF8);
        }

        /// <summary>
        /// 通过路径取得StreamWriter
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        static StreamWriter OpenWriteFile(string path)
        {
            return new StreamWriter(path, false, Encoding.UTF8);
        }

        ///<summary>
        /// 通过流取得StreamWriter
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        static StreamWriter OpenWriteFile(Stream stream)
        {
            return new StreamWriter(stream, Encoding.UTF8);
        }

        /// <summary>
        /// 检测文件是否存在,如果不存在就创建文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        static bool CheckFile(string path)
        {
            FileInfo f = new FileInfo(path);
            if (!f.Exists)
            {
                if (!f.Directory.Exists)
                    f.Directory.Create();
                f.Create();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="file">文件信息类</param>
        /// <returns></returns>
        static bool CheckFile(FileInfo file)
        {
            if (!file.Exists)
            {
                if (!file.Directory.Exists)
                    file.Directory.Create();
                file.Create();
                return false;
            }
            return true;
        }
        #endregion

    }
}
