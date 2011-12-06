using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using We7.CMS.Config;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;
using We7.Framework;
using HtmlAgilityPack;

namespace We7.CMS
{
    /// <summary>
    /// 模板编辑处理类
    /// </summary>
    public class TemplateProcessor
    {
        string headContent;
        string bodyContent;
        string fileName;
        string bodyText;
        string title;
        List<WeControl> controls;
        List<string> templates;
        Dictionary<string, int> oldDic = new Dictionary<string, int>();
        Dictionary<string, int> newDic = new Dictionary<string, int>();


        TemplateProcessor originalTemplateProcessor;
        ITemplateProcessorHelper ProcessorHelper = new TemplateProcessorHelper();

        /// <summary>
        /// 公用的一组正则表达式数组
        /// </summary>
        public static Regex[] TheRegexs = new Regex[12];
        /// <summary>
        /// 子模板文件名称（不带扩展名）
        /// </summary>
        public List<string> Templates
        {
            get { return templates; }

        }
        string input;
        //string output;
        string we7TemplateGroup;//当前模版组

        /// <summary>
        /// 构造函数
        /// </summary>
        public TemplateProcessor()
        {
            controls = new List<WeControl>();
            templates = new List<string>();

            RegexOptions options = RegexOptions.IgnoreCase;

            TheRegexs[0] = new Regex(@"\<wec:\w+[^>]*\>(\s*)\</wec:\w+\>", options);
            TheRegexs[1] = new Regex(@"\<wet:\w+[^>]*\>(\s*)\</wet:\w+\>", options);
            TheRegexs[2] = new Regex(@"\<div([^>]*)xmlns:wec=[^>]*\>[^<>]*(((?'Open'\<div[^>]*\>)([\w\W]*?))+((?'-Open'\</div\>)[^<>]*)+)*(?(Open)(?!))\</div\>", options);
            TheRegexs[3] = new Regex(@"\<div([^>]*)xmlns:wet=[^>]*\>[^<>]*(((?'Open'\<div[^>]*\>)([\w\W]*?))+((?'-Open'\</div\>)[^<>]*)+)*(?(Open)(?!))\</div\>", options);
            TheRegexs[4] = new Regex(@"control=\s*[""'].*?[""']", RegexOptions.Singleline | RegexOptions.Compiled);
            TheRegexs[5] = new Regex(@"(?<=<wec:\w+)\s+?(?=\w)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            TheRegexs[6] = new Regex(@"(?<=<wec:)\w+(?=\s)|(?<=</wec:)\w+(?=\s*>)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            TheRegexs[7] = new Regex(@"\<asp:ContentPlaceHolder[^>]*\>(\s*)\</asp:ContentPlaceHolder\>", options);
            TheRegexs[8] = new Regex(@"\<div([^>]*)xmlns:wem=[^>]*\>[^<>]*(((?'Open'\<div[^>]*\>)([\w\W]*?))+((?'-Open'\</div\>)[^<>]*)+)*(?(Open)(?!))\</div\>", options);
            TheRegexs[9] = new Regex(@"<img([^>]*)xmlns:wec=[^>]*>", options);
            TheRegexs[10] = new Regex(@"<img([^>]*)xmlns:wet=[^>]*>", options);
            TheRegexs[11] = new Regex(@"<img([^>]*)xmlns:wem=[^>]*>", options);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="templateGroup">模板组</param>
        public TemplateProcessor(string templateGroup)
            : this()
        {
            we7TemplateGroup = templateGroup;
        }

        public TemplateProcessor(string templateGroup, ITemplateProcessorHelper helper)
            : this(templateGroup)
        {
            ProcessorHelper = helper;
        }

        public TemplateProcessor(ITemplateProcessorHelper helper)
            : this()
        {
            ProcessorHelper = helper;
        }

        /// <summary>
        /// 当前使用的模板组
        /// </summary>
        public static string CurrentGroup
        {
            get
            {
                string result = String.Empty;
                string url = HttpContext.Current.Request.RawUrl;
                Regex regex = new Regex("(?<=/_skins/).*?(?=/)");
                Match m = regex.Match(url);
                if (m.Success)
                {
                    result = m.Value;
                }
                return String.IsNullOrEmpty(result) ? GeneralConfigs.GetConfig().DefaultTemplateGroup : result;
            }
        }

        /// <summary>
        /// body标记的描述部分，<body class=myclass ……> 括号里的部分
        /// </summary>
        public string BodyText
        {
            get { return bodyText; }
            set { bodyText = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        /// <summary>
        /// head部分内容
        /// </summary>
        public string HeadContent
        {
            get { return headContent; }
            set { headContent = value; }
        }
        /// <summary>
        /// body部分内容
        /// </summary>
        public string BodyContent
        {
            get { return bodyContent; }
            set { bodyContent = value; }
        }

        /// <summary>
        /// 文件的html标记部分
        /// </summary>
        public string HtmlContent
        {
            get
            {
                //if (IsSubTemplate)
                //    return BodyContent;
                //else
                //{
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<html >");
                sb.AppendLine("<head>{0}</head>");
                sb.AppendLine("<body {1}>");
                sb.AppendLine("{2}");
                sb.AppendLine("</body></html>");
                return string.Format(sb.ToString(), HeadContent, BodyText, BodyContent);
                //}
            }
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 输入的内容
        /// </summary>
        public string Input
        {
            get { return input; }
            set { input = value; }
        }

        private string originInput;
        /// <summary>
        /// 前一版本内容
        /// </summary>
        public string OrignInput
        {
            get
            {
                if (originalTemplateProcessor != null)
                {
                    return originalTemplateProcessor.Input;
                }
                return String.Empty;
            }
            set
            {
                if (originalTemplateProcessor == null)
                    originalTemplateProcessor = new TemplateProcessor();
                originalTemplateProcessor.Input = value;
            }
        }

        /// <summary>
        /// 当前模板内容的所有控件信息
        /// </summary>
        List<WeControl> Controls
        {
            get { return controls; }
        }

        bool isSubTemplate = false;
        /// <summary>
        /// 当前是否子模板
        /// </summary>
        public bool IsSubTemplate
        {
            get { return isSubTemplate; }
            set
            {
                isSubTemplate = value;
                if (originalTemplateProcessor != null)
                {
                    originalTemplateProcessor.IsSubTemplate = value;
                }
            }
        }

        bool isMasterPage = false;
        /// <summary>
        /// 当前是否母版
        /// </summary>
        public bool IsMasterPage
        {
            get { return isMasterPage; }
            set
            {
                isMasterPage = value;
                if (originalTemplateProcessor != null)
                {
                    originalTemplateProcessor.IsMasterPage = value;
                }
            }
        }

        /// <summary>
        /// 模版组的针对根目录的相对路径
        /// _skins/mydefault
        /// </summary>
        public string We7TemplateGroupRalatePath
        {
            get
            {
                return String.Format("_skins/{0}", We7TemplateGroup);
            }
        }

        /// <summary>
        /// 模版组
        /// </summary>
        public string We7TemplateGroup
        {
            get
            {
                if (String.IsNullOrEmpty(we7TemplateGroup))
                    throw new Exception("当前模版组为空");
                return we7TemplateGroup;
            }
        }

        /// <summary>
        /// 当前模版组的物理路径
        /// </summary>
        public string We7TemplateGroupPhysicalPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("_skins\\{0}", We7TemplateGroup));
            }
        }

        /// <summary>
        /// 当前模版组的副本物理路径
        /// </summary>
        public string We7TemplateGroupCopyPhysicalPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("_skins\\{0}", "~" + We7TemplateGroup.TrimStart('~')));
            }
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
        /// 当前模版组相对根目录的样式表路径.
        /// </summary>
        public string We7TemplateCopyStylePath
        {
            get
            {
                string gp = GeneralConfigs.GetConfig().SiteBuildState == "run" ? ("~" + We7TemplateGroup.TrimStart('~')) : We7TemplateGroup.TrimStart('~');
                return string.Format("_skins/{0}/css/We7Control.css", gp);
            }
        }

        /// <summary>
        /// 模版组的路径
        /// </summary>
        string TemplateGroupPath
        {
            get
            {
                return "_skins/" + We7TemplateGroup.Replace("~", "");
            }
        }

        /// <summary>
        /// 模版组的路径
        /// </summary>
        string TemplateGroupPathCopy
        {
            get
            {
                return "_skins/~" + We7TemplateGroup.Replace("~", "");
            }
        }


        #region process stylesheet

        /// <summary>
        /// 保存所有CSS文件到一个文件
        /// </summary>
        public void SaveStyle()
        {
            try
            {
                originalTemplateProcessor.FromVisualBoxText();
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

        void AppendLink()
        {
            if (!IsSubTemplate)
            {
                string linkfile = string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"/{0}\" media=\"screen\" />", We7TemplateCopyStylePath);

                //if (HeadContent.IndexOf(linkfile) < 0)
                //   HeadContent = HeadContent + "\r\n" + linkfile + "\r\n";
                HeadContent = ReplaceLink(HeadContent, linkfile);
            }
        }

        /// <summary>
        /// 发布CSS样式
        /// </summary>
        public void PublishStyle()
        {
            try
            {
                originalTemplateProcessor.FromVisualBoxText();
                SortControl();

                string cssPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7TemplateStylePath);
                string cssCopyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7TemplateCopyStylePath);
                FileInfo cssfile = new FileInfo(cssPath);
                string csstxt = "", csscopytxt = "";
                if (cssfile.Exists)
                {
                    using (StreamReader reader = OpenFile(cssfile.FullName))
                    {
                        csstxt = reader.ReadToEnd();
                    }
                }

                if (File.Exists(cssCopyPath))
                {
                    using (StreamReader reader = OpenFile(cssCopyPath))
                    {
                        csscopytxt = reader.ReadToEnd();
                    }
                }

                TemplateStyleHelper styleHelper = new TemplateStyleHelper();

                foreach (KeyValuePair<string, int> kvp in oldDic) //处理过期的控件样式
                {
                    string control = kvp.Key.Split('_')[0];
                    string style = kvp.Key.Split('_')[1];
                    csstxt = styleHelper.DeleteStyle(control.Trim(), style.Trim(), Path.GetFileNameWithoutExtension(FileName), csstxt);
                }

                foreach (KeyValuePair<string, int> kvp in newDic) //处理新增的控件样式
                {
                    string control = kvp.Key.Split('_')[0].Trim();
                    string style = kvp.Key.Split('_')[1].Trim();
                    if (styleHelper.Contains(control, style, csscopytxt))
                    {
                        string stylecopy = styleHelper.LoadCss(control, style, csscopytxt);
                        csstxt = styleHelper.ReplaceAppendCss(control, style, Path.GetFileNameWithoutExtension(FileName), csstxt, stylecopy);
                    }
                    else
                    {
                        csstxt = styleHelper.AddStyle(control, style, Path.GetFileNameWithoutExtension(FileName), csstxt);
                    }
                }
                using (FileStream f = File.Open(cssPath, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = OpenWriteFile(f))
                    {
                        writer.Write(csstxt);
                        writer.Close();
                        writer.Dispose();
                    }
                    f.Close();
                    f.Dispose();
                }

                if (!IsSubTemplate)
                {

                    string linkfile = string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"/{0}\" media=\"screen\" />", We7TemplateCopyStylePath);
                    //if (HeadContent.IndexOf(We7TemplateCopyStylePath) < 0)
                    //    HeadContent = HeadContent + "\r\n" + linkfile + "\r\n";
                    HeadContent = ReplaceLink(HeadContent, linkfile);
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// 发布控件
        /// </summary>
        public void PublishControl()
        {
            try
            {
                foreach (WeControl c in Controls)
                {
                    string dir = Path.Combine(We7TemplateGroupPhysicalPath, Constants.We7ControlsBasePath + "/" + c.Control.Split('.')[0]);
                    if (!Directory.Exists(dir))
                    {
                        string copydir = Path.Combine(We7TemplateGroupCopyPhysicalPath, Constants.We7ControlsBasePath + "/" + c.Control.Split('.')[0]);
                        CopyDir(copydir, dir);
                    }
                    else
                    {
                        string copypath = Path.Combine(We7TemplateGroupCopyPhysicalPath, String.Format("{0}/{1}/Page/{2}.ascx", Constants.We7ControlsBasePath, c.Control.Split('.')[0], c.Control));
                        string path = Path.Combine(We7TemplateGroupPhysicalPath, String.Format("{0}/{1}/Page/{2}.ascx", Constants.We7ControlsBasePath, c.Control.Split('.')[0], c.Control));

                        FileInfo fc = new FileInfo(copypath);
                        FileInfo f = new FileInfo(path);

                        if (fc.Exists && (!f.Exists || f.LastWriteTime != fc.LastWriteTime || f.Length != fc.Length))
                        {
                            fc.CopyTo(f.FullName, true);
                        }
                    }
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// 检查CSS文件
        /// </summary>
        public void CheckCssFile()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7TemplateStylePath);
            string cpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7TemplateCopyStylePath);

            FileInfo f = new FileInfo(path);
            FileInfo fc = new FileInfo(cpath);

            if (f.Exists && (!fc.Exists || fc.LastWriteTime < f.LastWriteTime))
            {
                if (!f.Directory.Exists)
                    f.Directory.Create();
                f.CopyTo(fc.FullName, true);
            }
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        public void CheckControl()
        {

            foreach (WeControl c in Controls)
            {
                string path = We7Utils.Combine(AppDomain.CurrentDomain.BaseDirectory, TemplateGroupPath, Constants.We7ControlsBasePath, GetControlPath(c.Control));
                string cpath = We7Utils.Combine(AppDomain.CurrentDomain.BaseDirectory, TemplateGroupPathCopy, Constants.We7ControlsBasePath, GetControlPath(c.Control));
                FileInfo f = new FileInfo(path);
                FileInfo fc = new FileInfo(cpath);
                if (f.Exists && (!fc.Exists || fc.LastWriteTime < f.LastWriteTime))
                {
                    if (!f.Directory.Exists)
                        f.Directory.Create();
                    f.CopyTo(fc.FullName, true);
                }
            }
        }


        /// <summary>
        /// 发布模板
        /// </summary>
        /// <param name="src"></param>
        /// <param name="target"></param>
        public static void PublicTemplate(string src, string target)
        {
            Regex regex = new Regex("(?<=_skins/).*?(?=/css/We7Control.css)", RegexOptions.Compiled | RegexOptions.Singleline);
            string s = String.Empty;
            using (StreamReader sr = OpenFile(src))
            {
                s = sr.ReadToEnd();
                Match m = regex.Match(s);
                if (m.Success)
                {
                    s = regex.Replace(s, m.Value.TrimStart('~'));
                }
            }
            using (StreamWriter sw = OpenWriteFile(target))
            {
                sw.Write(s);
            }
        }



        /// <summary>
        /// 加载控件Css样式
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <returns></returns>
        string LoadControlCss(string control, string style)
        {
            string css = "", basestyle;
            try
            {
                string stylePath = Path.Combine(Constants.We7ControlPhysicalPath, control.Split('.')[0].Trim() + "/Style");
                DirectoryInfo di = new DirectoryInfo(stylePath);
                FileInfo[] fs = di.GetFiles(String.Format("{0}_{1}.*.css", control, style));
                if (fs != null && fs.Length > 0)
                {
                    FileInfo f = fs[0];
                    using (StreamReader reader = OpenFile(f.FullName))
                    {
                        css = reader.ReadToEnd();
                        basestyle = String.Format(".{0}_{1}", control.Replace(".", "_"), style);

                        css = css.Replace("{WE:STYLE}", basestyle);
                    }
                }
            }
            catch
            {
            }
            return css;
        }

        /// <summary>
        /// 加载控件Css样式
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <returns></returns>
        string LoadControlCssByPath(string fileName, string style)
        {
            string css = "", basestyle, control = Path.GetFileNameWithoutExtension(fileName);
            try
            {
                FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath(fileName));
                DirectoryInfo[] dis = fi.Directory.Parent.GetDirectories("Style");
                DirectoryInfo di = dis != null && dis.Length > 0 ? dis[0] : null;
                if (di != null)
                {
                    FileInfo[] fs = di.GetFiles(String.Format("{0}_{1}.*.css", control, style));
                    if (fs != null && fs.Length > 0)
                    {
                        FileInfo f = fs[0];
                        using (StreamReader reader = OpenFile(f.FullName))
                        {
                            css = reader.ReadToEnd();
                            basestyle = String.Format(".{0}_{1}", control.Replace(".", "_"), style);

                            css = css.Replace("{WE:STYLE}", basestyle);
                        }
                    }
                }
            }
            catch
            {
            }
            return css;
        }

        /// <summary>
        /// 取得当模板中的样式,如果没有会从文件中加入
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <returns></returns>
        public string LoadAppendCss(string fileName, string style)
        {
            string css = "";
            string control = Path.GetFileNameWithoutExtension(fileName);
            try
            {
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

                if (styleHelper.Contains(control, style, csstxt))
                {
                    css = styleHelper.LoadCss(control, style, csstxt);
                }
                else
                {
                    css = LoadControlCssByPath(fileName, style); //LoadControlCss(control, style);
                    csstxt = styleHelper.AddStyleByPath(fileName, style, "Temp", csstxt);
                    using (StreamWriter writer = OpenWriteFile(cssPath))
                    {
                        writer.Write(csstxt);
                    }
                }
            }
            catch
            {
            }
            return css;
        }

        /// <summary>
        /// css被使用次数
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="stylename">样式名称</param>
        /// <param name="csstxt">We7Controls的Css文本</param>
        /// <returns></returns>
        int CssUsedCount(string ctr, string stylename, string csstxt)
        {
            TemplateStyleHelper helper = new TemplateStyleHelper();
            return helper.StyleUsedCount(ctr, stylename, csstxt);
        }

        /// <summary>
        /// 当前样式使用次数
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <returns></returns>
        public int StyleUsageCounter(string ctr, string style)
        {
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
            return CssUsedCount(ctr, style, csstxt);
        }

        /// <summary>
        /// 用当前CSS替换以前的CSS
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <param name="css">当前的Css</param>
        /// <returns></returns>
        public void OverrideCss(string ctr, string style, string css)
        {
            string cssPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7TemplateStylePath);
            FileInfo cssfile = new FileInfo(cssPath);

            string csstxt = "";
            if (CheckFile(cssfile))
            {
                using (StreamReader reader = OpenFile(cssfile.FullName))
                {
                    csstxt = reader.ReadToEnd();
                }
                TemplateStyleHelper styleHelper = new TemplateStyleHelper();
                csstxt = styleHelper.ReplaceCss(ctr, style, csstxt, css);
                csstxt = csstxt.Replace("{WE:STYLE}", String.Format(".{0}_{1}", ctr.Replace(".", "_"), style));
                using (FileStream fs = cssfile.Open(FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter wr = OpenWriteFile(fs))
                    {
                        wr.Write(csstxt);
                    }
                }
            }
        }

        /// <summary>
        /// 用当前CSS替换以前的CSS
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <param name="css">当前的Css</param>
        /// <returns></returns>
        public void OverrideCssByPath(string fileName, string style, string css)
        {
            string ctr = Path.GetFileNameWithoutExtension(fileName);

            string cssPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7TemplateStylePath);
            FileInfo cssfile = new FileInfo(cssPath);

            string csstxt = "";
            if (CheckFile(cssfile))
            {
                using (StreamReader reader = OpenFile(cssfile.FullName))
                {
                    csstxt = reader.ReadToEnd();
                }
                TemplateStyleHelper styleHelper = new TemplateStyleHelper();
                csstxt = styleHelper.ReplaceCss(ctr, style, csstxt, css);
                csstxt = csstxt.Replace("{WE:STYLE}", String.Format(".{0}_{1}", ctr.Replace(".", "_"), style));
                using (FileStream fs = cssfile.Open(FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter wr = OpenWriteFile(fs))
                    {
                        wr.Write(csstxt);
                    }
                }
            }
        }

        /// <summary>
        /// 取得当前下在使用的Css
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <returns></returns>
        public string LoadCurrentCss(string ctr, string style)
        {
            TemplateStyleHelper styleHelper = new TemplateStyleHelper();
            string cssPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7TemplateStylePath);
            FileInfo cssfile = new FileInfo(cssPath);

            string csstxt = "";
            if (CheckFile(cssfile))
            {
                using (StreamReader reader = OpenFile(cssfile.FullName))
                {
                    csstxt = reader.ReadToEnd();
                }
                string regtxt = styleHelper.LoadCss(ctr, style, csstxt);
                return regtxt.Replace(String.Format(".{0}_{1}", ctr.Replace(".", "_"), style), "{WE:STYLE}");
            }
            return String.Empty;
        }

        /// <summary>
        /// 用当前CSS替换以前的CSS
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <returns></returns>
        public void DelCss(string ctr, string style)
        {
            string cssPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7TemplateStylePath);
            FileInfo cssfile = new FileInfo(cssPath);

            string csstxt = "";
            if (CheckFile(cssfile))
            {
                using (StreamReader reader = OpenFile(cssfile.FullName))
                {
                    csstxt = reader.ReadToEnd();
                }
                TemplateStyleHelper styleHelper = new TemplateStyleHelper();
                csstxt = styleHelper.RemoveStyle(ctr, style, csstxt);

                using (FileStream fs = cssfile.Open(FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter wr = OpenWriteFile(fs))
                    {
                        wr.Write(csstxt);
                    }
                }
            }
        }

        /// <summary>
        /// 对控件分类
        /// </summary>
        void SortControl()
        {
            foreach (WeControl c in Controls)//新控件的样式数
            {
                //string key = String.Format("{0}_{1}", c.Control, c.Style);
                string key = String.Format("{0}_{1}", c.FileName, c.Style);
                newDic[key] = newDic.ContainsKey(key) ? newDic[key] + 1 : 1;
            }

            foreach (WeControl c in originalTemplateProcessor.Controls) //旧控件样式数
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

        #endregion

        #region process template

        /// <summary>
        /// 保存子模板
        /// </summary>
        void SaveSub()
        {
            if (We7.Framework.AppCtx.IsDemoSite)
            {
                throw new Exception("此演示站点，您没有该操作权限！");
            }
            StringBuilder sb = new StringBuilder();

            List<string> tags = new List<string>();
            foreach (WeControl c in Controls)
            {
                if (!tags.Contains(c.Control))
                    tags.Add(c.Control);
            }

            foreach (string c in tags)
            {
                sb.AppendLine(string.Format("<%@ Register Src=\"" + GetControlRelatePath(c) + "\" TagName=\"{0}\" TagPrefix=\"wec\" %>", GetUCName(c)));
            }

            foreach (String c in Templates)
            {
                sb.AppendLine(string.Format("<%@ Register Src=\"{0}.ascx\" TagName=\"{0}\" TagPrefix=\"wet\" %>", c));
            }

            SaveStyle();//保存样式
            CopyControl();

            StringBuilder sbHead = new StringBuilder(headContent);
            AbstractAndRemoveMatchSections(@"\<title[^>]*\>([\w\W]*?)\</title\>", sbHead);
            headContent = sbHead.ToString();
            sb.AppendLine(HeadContent);
            sb.AppendLine(BodyContent);
            File.WriteAllText(FileName, sb.ToString(), Encoding.UTF8);
        }

        #endregion

        #region process the string of template

        /// <summary>
        /// 保存模板
        /// </summary>
        public void Save()
        {
            if (IsSubTemplate)
            {
                SaveSub();
                string tagname = Path.GetFileNameWithoutExtension(fileName);
                CreateSubTemplateIcon(tagname);
            }
            else
                SaveTemplate();
            CopyControl();
        }


        /// <summary>
        /// 保存模版
        /// </summary>
        void SaveTemplate()
        {
            if (We7.Framework.AppCtx.IsDemoSite)
            {
                throw new Exception("此演示站点，您没有该操作权限！");
            }
            StringBuilder sb = new StringBuilder();
            List<string> tags = new List<string>();
            foreach (WeControl c in Controls)
            {
                if (!tags.Contains(c.FileName))
                    tags.Add(c.FileName);
            }

            if (IsMasterPage)
                sb.AppendLine("<%@ Master Language=\"C#\" AutoEventWireup=\"true\" Inherits=\"We7.CMS.Web.User.DefaultMaster.content\" %>");

            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");

            foreach (string c in tags)
            {
                //string path = GetTemplatePath(c);
                //string uc = GetUCName(path);
                string path = ProcessorHelper.GetTemplatePath(c);
                string uc = ProcessorHelper.GetUCName(path);

                if (String.IsNullOrEmpty(path) || String.IsNullOrEmpty(uc))
                    continue;
                sb.AppendLine(string.Format("<%@ Register Src=\"" + path + "\" TagName=\"{0}\" TagPrefix=\"wec\" %>\r\n", uc));
            }

            foreach (String c in Templates)
            {
                sb.AppendLine(string.Format("<%@ Register Src=\"{0}.ascx\" TagName=\"{0}\" TagPrefix=\"wet\" %>\r\n", c));
            }

            sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            sb.AppendLine("<head runat=\"server\">");

            HeadContent = FormatHeadMeta(HeadContent);
            AppendLink();

            sb.Append("<title>");
            sb.Append(Title);
            sb.AppendLine("</title>");
            sb.Append(HeadContent);

            //添加head部分的母版占位标签
            if (IsMasterPage && !TheRegexs[7].Match(headContent).Success)
            {
                sb.AppendLine("<asp:ContentPlaceHolder ID=\"MyHeadPlaceHolder\" runat=\"server\">");
                sb.AppendLine("</asp:ContentPlaceHolder>");
            }
            sb.AppendLine("</head>");
            sb.Append("<body ");
            sb.Append(BodyText);
            sb.Append(">\r\n");
            sb.AppendLine(BodyContent);
            sb.Append("</body>");
            sb.Append("</html>");
            File.WriteAllText(FileName, sb.ToString(), Encoding.UTF8);
            CopyControl();
            SaveStyle();//保存样式

            CombineStyle();
        }

        public void CombineStyle()
        {
            string uxStyle = string.Format("/_skins/{0}/Style/UxStyle.css", We7TemplateGroup);
            string themeStyle = string.Format("/Widgets/Themes/{0}/Style.css", GeneralConfigs.GetConfig().Theme);
            CheckFile(HttpContext.Current.Server.MapPath(uxStyle));

            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionCheckSyntax = true;
            doc.OptionOutputOriginalCase = true;

            doc.Load(this.FileName);

            IEnumerable<HtmlNode> nodes = doc.DocumentNode.DescendantNodes();

            bool uxStyleExist = false;
            bool themeStyleExist = false;
            foreach (HtmlNode node in nodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.Attributes.Contains("filename"))
                    {
                        if (node.Attributes["filename"].Value.ToLower().IndexOf("widgets") > -1)
                        {
                            if (node.Attributes.Contains("cssclass") || node.Attributes.Contains("CssClass"))
                            {
                                string tmp = string.IsNullOrEmpty(node.Attributes["cssclass"].Value) ? node.Attributes["CssClass"].Value : node.Attributes["cssclass"].Value;
                                if (!string.IsNullOrEmpty(tmp))
                                    UpdatePublishStyle(node.Attributes["cssclass"].Value.Replace("_", "."), node.Attributes["filename"].Value, HttpContext.Current.Server.MapPath(uxStyle));
                            }
                        }
                    }
                    if (node.Name.ToLower().Equals("link"))
                    {
                        if (node.Attributes["href"].Value.Equals(uxStyle))
                        {
                            uxStyleExist = true;
                            break;
                        }
                    }
                    if (node.Name.ToLower().Equals("link"))
                    {
                        if (node.Attributes["href"].Value.Equals(themeStyle))
                        {
                            themeStyleExist = true;
                            break;
                        }
                    }
                }
            }

            if (!uxStyleExist)
            {
                var node = HtmlNode.CreateNode(string.Format("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\" />", uxStyle));
                doc.DocumentNode.SelectSingleNode("//head").ChildNodes.Append(node);
            }

            if (!themeStyleExist)
            {
                var node = HtmlNode.CreateNode(string.Format("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\" />", themeStyle));
                doc.DocumentNode.SelectSingleNode("//head").ChildNodes.Append(node);
            }
            doc.Save(this.FileName, Encoding.UTF8);
        }

        private void UpdatePublishStyle(string style, string controlPath, string PublishStyle)
        {
            string uxStyle = File.ReadAllText(PublishStyle, Encoding.UTF8);
            string css = CssTxt(style, controlPath);
            string control = Path.GetFileNameWithoutExtension(controlPath);
            css = UpdateStyleImage(css, control, controlPath);
            TemplateStyleHelper styleHelper = new TemplateStyleHelper();
            css = styleHelper.LoadCss(control, style, css);
            uxStyle = styleHelper.ReplaceAppendCss(control, style, We7TemplateGroup, uxStyle, css);
            File.WriteAllText(PublishStyle, uxStyle, Encoding.UTF8);
        }

        private string UpdateStyleImage(string cssTxt, string styleFileName, string controlPath)
        {
            string imageRootPath = Path.GetDirectoryName(controlPath);
            string imgRex = @"background\s*:\s*url[^\)]+";
            MatchCollection mc = Regex.Matches(cssTxt, imgRex, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
            if (mc.Count > 0)
            {
                foreach (Match it in mc)
                {
                    string image = Path.GetFileName(it.Value);
                    cssTxt = cssTxt.Replace(it.Value, string.Format("background:url({0}", string.Format("{0}/Images/{1}", imageRootPath, image).Replace("\\", "/")));
                }
            }
            return cssTxt;
        }

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

        /// <summary>
        /// 从输入框的文本转换处理后存入BodyContent
        /// </summary>
        public void FromVisualBoxText()
        {
            String s = Input;
            HeadContent = GetHeadContentFromText(s);
            BodyText = GetBodyTextFromText(s);
            BodyContent = GetBodyContentFromText(s);
            BodyContent = ConvertTagsToControls(BodyContent);
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

            //thehim:跟踪发现系统的css此处已经被过滤了，无需我们再次进行过滤
            //rs = @"<link[^>]*href\s*=\s*['""].*?fck_media\.css.*?['""]\s*[^>]*>\s*";
            //reg = new Regex(rs, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //headContent = reg.Replace(headContent, "");

            //rs = @"<link[^>]*href\s*=\s*['""].*?fck_editorarea\.css.*?['""]\s*[^>]*>\s*";
            //reg = new Regex(rs, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //headContent = reg.Replace(headContent, "");

            return headContent.Trim() + "\r\n";
        }


        /// <summary>
        /// 转换文本中的div标签为控件
        /// </summary>
        /// <param name="inputStr">模板文本</param>
        /// <returns></returns>
        string ConvertTagsToControls(string inputStr)
        {
            string strReturn = "";
            string strTemplate;
            Controls.Clear();
            Templates.Clear();
            strTemplate = inputStr;

            //foreach (Match m in TheRegexs[2].Matches(strTemplate))
            //{
            //    strTemplate = strTemplate.Replace(m.Groups[0].ToString(), GetWeControl(m.Groups[0].ToString()));
            //}

            //foreach (Match m in TheRegexs[3].Matches(strTemplate))
            //{
            //    strTemplate = strTemplate.Replace(m.Groups[0].ToString(), GetWeTemplate(m.Groups[0].ToString()));
            //}

            //新增：处理img标记
            foreach (Match m in TheRegexs[10].Matches(strTemplate))
            {
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), GetWeTemplateFromImg(m.Groups[0].ToString()));
            }

            foreach (Match m in TheRegexs[9].Matches(strTemplate))
            {
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), GetWeControlFromImg(m.Groups[0].ToString()));
            }

            if (IsMasterPage)
            {
                MatchCollection mc = TheRegexs[11].Matches(strTemplate);
                if (mc.Count == 0)
                    throw new Exception("MasterPage页面中没有找到ContentPlaceHolder占位控件。");
                else if (mc.Count > 1)
                    throw new Exception("MasterPage母版中仅允许存在一个ContentPlaceHolder占位控件，发现多个占位控件。");
                else
                {
                    foreach (Match m in mc)
                    {
                        //strTemplate = strTemplate.Replace(m.Groups[0].ToString(), GetWePlaceHolder(m.Groups[0].ToString()));
                        strTemplate = strTemplate.Replace(m.Groups[0].ToString(), GetWePlaceHolderFromImg(m.Groups[0].ToString()));
                    }
                }
            }

            return strTemplate;
        }

        private string GetWeControlFromImg(string p)
        {
            Regex regex = new Regex(@"control\s*=\s*""[^""]*""", RegexOptions.IgnoreCase);
            Match match = regex.Match(p);
            if (match.Success)
            {
                string strControl = match.Value;
                strControl = strControl.Substring(strControl.IndexOf("\"") + 1);
                strControl = strControl.Substring(0, strControl.IndexOf("\""));
                strControl = We7Helper.HtmlDecode(strControl);
                WeControl control = GetWeControlFromString(strControl, true);
                if (!Controls.Contains(control))
                {
                    Controls.Add(control);
                }
                return TheRegexs[6].Replace(strControl, control.TagName);
            }
            else
                return p;
        }

        /// <summary>
        /// 从字符串中返回其中的控件部分wec:
        /// </summary>
        /// <param name="inputStr">模板文本</param>
        /// <returns></returns>
        string GetWeControl(string inputStr)
        {

            Match match = TheRegexs[0].Match(inputStr);
            WeControl control = GetWeControlFromString(inputStr, true);
            if (!Controls.Contains(control))
            {
                Controls.Add(control);
            }
            inputStr = TheRegexs[4].Replace(match.Value, "");
            inputStr = inputStr.Replace("&amp;", "&");
            return TheRegexs[6].Replace(inputStr, control.TagName);
            //return match.Value;
        }
        /// <summary>
        /// 从字符串中返回其中的模板部分
        /// </summary>
        /// <param name="inputStr">模板文件</param>
        /// <returns></returns>
        string GetWeTemplate(string inputStr)
        {
            Regex r = new Regex(@"\<wet:\w*");
            Match match = r.Match(inputStr);

            if (match.Success)
            {
                string tagName = match.Value.Substring(5);
                if (!Templates.Contains(tagName))
                {
                    Templates.Add(tagName);
                }
            }

            return TheRegexs[1].Match(inputStr).Value;
        }

        string GetWeTemplateFromImg(string p)
        {
            Regex regex = new Regex(@"control\s*=\s*""[^""]*""", RegexOptions.IgnoreCase);
            Match match = regex.Match(p);
            if (match.Success)
            {
                string strControl = match.Value;
                strControl = strControl.Substring(strControl.IndexOf("\"") + 1);
                strControl = strControl.Substring(0, strControl.IndexOf("\""));
                strControl = We7Helper.HtmlDecode(strControl);
                Regex r = new Regex(@"\<wet:\w*");
                Match match2 = r.Match(strControl);
                if (match2.Success)
                {
                    string tagName = match2.Value.Substring(5);
                    if (!Templates.Contains(tagName))
                    {
                        Templates.Add(tagName);
                    }
                }
                return strControl;
            }
            else
                return p;
        }


        /// <summary>
        /// 从字符串中返回其中的ContentPlaceHolder部分
        /// </summary>
        /// <param name="inputStr">模板文件</param>
        /// <returns></returns>
        string GetWePlaceHolder(string inputStr)
        {
            return TheRegexs[7].Match(inputStr).Value;
        }

        string GetWePlaceHolderFromImg(string p)
        {
            Regex regex = new Regex(@"control\s*=\s*""[^""]*""", RegexOptions.IgnoreCase);
            Match match = regex.Match(p);
            if (match.Success)
            {
                string strControl = match.Value;
                strControl = strControl.Substring(strControl.IndexOf("\"") + 1);
                strControl = strControl.Substring(0, strControl.IndexOf("\""));
                strControl = We7Helper.HtmlDecode(strControl);
                return strControl;
            }
            else
                return p;
        }

        /// <summary>
        /// 复制控件到
        /// </summary>
        void CopyControl()
        {
            try
            {
                GeneralConfigInfo info = GeneralConfigs.GetConfig();


                string tpdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TemplateGroupPath + "\\" + Constants.We7ControlsBasePath);
                string tpcdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TemplateGroupPathCopy + "\\" + Constants.We7ControlsBasePath);
                foreach (WeControl c in Controls)
                {
                    string ctrdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, c.FileName.TrimStart('/', '\\'));
                    ctrdir = ctrdir.Substring(0, ctrdir.LastIndexOf("/Page/", StringComparison.OrdinalIgnoreCase));//上面两句是为了取得控件的实际目录。
                    ctrdir = Directory.GetParent(ctrdir).FullName;

                    string file = HttpContext.Current.Server.MapPath(c.FileName); //Path.Combine(ctrdir, GetControlPath(c.Control));
                    string tpfile = Path.Combine(tpdir, GetControlPath(c.Control));
                    string tpcfile = Path.Combine(tpcdir, GetControlPath(c.Control));

                    FileInfo f = new FileInfo(file);
                    FileInfo tf = new FileInfo(tpfile);
                    FileInfo tfc = new FileInfo(tpcfile);

                    if (f.Exists && (!tf.Exists || info.SiteBuildState.ToLower() == "debug"))
                    {
                        string srcdir = Path.Combine(ctrdir, c.Control.Split('.')[0]);
                        string targetdir = Path.Combine(tpdir, c.Control.Split('.')[0]);
                        CopyDir(srcdir + "/Page", targetdir + "/Page");
                        //CopyDir(srcdir + "/CS", targetdir + "/CS");
                    }

                    if (f.Exists && !tfc.Exists)
                    {
                        string srcdir = Path.Combine(ctrdir, c.Control.Split('.')[0]);
                        string targetdir = Path.Combine(tpcdir, c.Control.Split('.')[0]);
                        CopyDir(srcdir + "/Page", targetdir + "/Page");
                        //CopyDir(srcdir + "/CS", targetdir + "/CS");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 复制文件夹下的内容
        /// </summary>
        /// <param name="src">源目录</param>
        /// <param name="target">目标目录</param>
        void CopyDir(string src, string target)
        {
            DirectoryInfo dir = new DirectoryInfo(src);
            if (dir.Name.StartsWith(".") || dir.Name.StartsWith("~"))
                return;
            DirectoryInfo dirtarget = new DirectoryInfo(target);
            if (!dirtarget.Exists)
                dirtarget.Create();

            foreach (FileInfo f in dir.GetFiles())
            {
                if (f.Name.StartsWith(".") || f.Name.StartsWith("~"))
                    continue;
                try
                {
                    string targetFile = Path.Combine(target, f.Name);
                    if (f.Name.EndsWith("aspx", true, null) || f.Name.EndsWith("ascx", true, null))
                    {
                        using (StreamReader sr = OpenFile(f.FullName))
                        {
                            string txt = sr.ReadToEnd();
                            using (StreamWriter sw = OpenWriteFile(targetFile))
                            {
                                sw.Write(txt);
                            }
                        }
                    }
                    else
                    {
                        File.Copy(f.FullName, targetFile);
                    }
                }
                catch (Exception ex)
                {
                }
            }

            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                CopyDir(d.FullName, Path.Combine(target, d.Name));
            }
        }

        #endregion

        #region load 载入……

        /// <summary>
        /// 加载数据
        /// </summary>
        public void Load()
        {
            if (!File.Exists(FileName))
                return;

            if (IsSubTemplate)
                LoadSub();
            else
                LoadTemplate();
        }

        /// <summary>
        /// 加载模板
        /// </summary>
        public void LoadTemplate()
        {
            using (StreamReader sr = OpenFile(FileName))
            {
                string s = sr.ReadToEnd();
                //TODO:此处未处理引用部分，相当于直接丢弃，而后重新生成；
                //存在问题：如果引用的子模板或控件不在默认路径下会出错
                headContent = GetHeadContentFromText(s);
                bodyText = GetBodyTextFromText(s);
                bodyContent = GetBodyContentFromText(s);
                bodyContent = ConvertControlsToTags(bodyContent);
            }
        }

        /// <summary>
        /// 加载子模板
        /// </summary>
        public void LoadSub()
        {
            using (FileStream fs = File.Open(FileName, FileMode.Open))
            {
                using (StreamReader sr = OpenFile(fs))
                {
                    string s = sr.ReadToEnd();
                    StringBuilder sb = new StringBuilder(s);

                    string regexStr = @"\<%@.*.%\>";
                    AbstractAndRemoveMatchSections(regexStr, sb);//去除<%@ Register %>项

                    regexStr = @"\<link.*.\>";
                    headContent += AbstractAndRemoveMatchSections(regexStr, sb);

                    regexStr = @"(<style)+[^<>]*>[^\0]*(<\/style>)+";
                    headContent += AbstractAndRemoveMatchSections(regexStr, sb);

                    bodyContent = ConvertControlsToTags(sb.ToString());
                }
            }
        }
        /// <summary>
        /// 转换文本中的控件标签<wec:control ></wec:control>为<div>标签
        /// </summary>
        /// <param name="inputStr">输入文本</param>
        /// <returns>替换后文本</returns>
        private string ConvertControlsToTags(string inputStr)
        {
            string strReturn = "";
            string strTemplate;
            strTemplate = inputStr;

            foreach (Match m in TheRegexs[0].Matches(strTemplate))
            {
                //strTemplate = strTemplate.Replace(m.Groups[0].ToString(), ConvertWeControlsToDiv(m.Groups[0].ToString()));
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), ConvertWeControlsToImg(m.Groups[0].ToString()));
            }

            foreach (Match m in TheRegexs[1].Matches(strTemplate))
            {
                //strTemplate = strTemplate.Replace(m.Groups[0].ToString(), ConvertSubtemplateToDiv(m.Groups[0].ToString()));
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), ConvertSubtemplateToImg(m.Groups[0].ToString()));
            }

            if (isMasterPage)
            {
                foreach (Match m in TheRegexs[7].Matches(strTemplate))
                {
                    //strTemplate = strTemplate.Replace(m.Groups[0].ToString(), ConvertPlaceHolderToDiv(m.Groups[0].ToString()));
                    strTemplate = strTemplate.Replace(m.Groups[0].ToString(), ConvertPlaceHolderToImg(m.Groups[0].ToString()));
                }
            }

            return strTemplate;
        }

        /// <summary>
        /// 从文本中解析控件，放到<div>容器里返回
        /// </summary>
        /// <param name="source">模板文本</param>
        /// <returns></returns>
        private string ConvertWeControlsToDiv(string source)
        {

            StringBuilder sb = new StringBuilder();
            WeControl wc = GetWeControlFromString(source, false);

            source = TheRegexs[5].Replace(source, " control='" + wc.TagName.Replace("_", ".") + "' ");
            source = source.Replace("&", "&amp;");
            sb.Append(String.Format("<DIV xmlns:wec=\"http://www.WestEngine.com\" tag=\"{0}\" contenteditable=\"false\" style=\"border:solid gray 1px dotted;width:100px;height:50px;background-color: #aaffaa;\">\r\n", wc.TagPrefix));
            sb.Append(String.Format("<DIV tag=\"{0}\">控件:{1}<br>{2}</div>\r\n", wc.TagName, wc.ChineseName, wc.ID));
            sb.Append("<DIV tag=\"content\">\r\n");
            sb.Append(String.Format("<?xml:namespace prefx = {0} />\r\n", wc.TagPrefix));
            sb.Append(source);
            sb.Append("</DIV>\r\n");
            sb.Append("</DIV>\r\n");

            return sb.ToString();
        }

        /// <summary>
        /// 最新：2010-8-8,thehim
        /// 使用img标记代替div标记，来解决Firefox下模板编辑的问题
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string ConvertWeControlsToImg(string source)
        {
            source = new Regex(@"(?<=\s+)\w+\s*(?==)", RegexOptions.Compiled | RegexOptions.IgnoreCase).Replace(source, (Match m) => m.Value.ToLower());
            AppendControl(ref source);
            StringBuilder sb = new StringBuilder();
            WeControl wc = GetWeControlFromString(source, false);
            //source = TheRegexs[5].Replace(source, " control='" + wc.TagName.Replace("_", ".") + "' ");
            //source = source.Replace("&", "&amp;");            
            source = We7Helper.HtmlEncode(source);
            HelperFactory helperFactory = HelperFactory.Instance;
            DataControlHelper instance = new DataControlHelper();
            string controlName = "";
            //DataControl dc = instance.GetDataControlByPath(wc.FileName);
            //DataControlInfo info = instance.GetDataControlInfoByPath(wc.FileName);

            DataControl dc = ProcessorHelper.GetDataControlByPath(wc.FileName);
            DataControlInfo info = ProcessorHelper.GetDataControlInfoByPath(wc.FileName);

            if (dc == null && info != null)
            {
                if (String.IsNullOrEmpty(info.Name))
                    controlName = "当前控件不存在：" + wc.Control;
                else
                    controlName = info.Name;
            }
            else if (dc != null)
            {
                wc.ChineseName = dc.Name;
                controlName = dc.Name;
                if (info != null) controlName = info.Name + ":" + controlName;
            }
            else
            {
                controlName = "当前控件不存在：" + wc.Control;
            }
            //string icon = info != null && info.DefaultControl != null ? info.DefaultControl.DemoUrl : "/admin/images/s.jpg";
            //if (!icon.StartsWith("/")) icon = "/" + icon;
            if (!string.IsNullOrEmpty(controlName))
            {
                string icon = CreateDataControlIcon(controlName, wc.ID);
                sb.Append(String.Format("<IMG xmlns:wec=\"http://www.WestEngine.com\" tag=\"{0}\" class=\"{0}\"  control=\"{1}\"  controlName=\"{3}\" filename=\"{5}\"  src=\"{2}\"   alt=\"{4}\"  />", wc.TagPrefix, source, icon, wc.TagName.Replace("_", "."), wc.ChineseName, wc.FileName));
                return new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.Singleline).Replace(sb.ToString(), " ");
            }
            else
                return source;
        }

        /// <summary>
        /// 为了适应以前的,在没有control属性的控件上添加上control
        /// </summary>
        /// <param name="ctr"></param>
        private void AppendControl(ref string ctr)
        {
            Regex regex = new Regex(@"control\s*=\s*['""].*?['""]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!regex.IsMatch(ctr))
            {

                Regex regexId = new Regex(@"\bid=\s*['""].*?['""]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Match m = regexId.Match(ctr);
                if (m.Success)
                {
                    Regex regexCtr = new Regex("<wec:(?<ctr>.*?)\\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Match ctrTag = regexCtr.Match(ctr);
                    if (ctrTag.Success && ctrTag.Groups["ctr"].Success)
                    {
                        ctr = regexId.Replace(ctr, m.Value + " control=\"" + ctrTag.Groups["ctr"].Value.Replace("_", ".") + "\" ");
                    }
                    ctr = new Regex("\r\n").Replace(ctr, "");
                }
            }
        }

        /// <summary>
        /// 从文本中解析控件，返回WeControl控件描述对象
        /// </summary>
        /// <param name="source">模板源文件</param>
        /// <param name="toFile">是否生成文件</param>
        /// <returns></returns>
        WeControl GetWeControlFromString(string source, bool toFile)
        {
            WeControl wc = new WeControl();
            Regex r = new Regex(@"\<wec:\w*");
            Match match = r.Match(source);
            if (match.Success)
            {
                wc.TagPrefix = match.Value.Substring(1, 3);
                wc.TagName = toFile ? GetControlAttribute(source).Replace(".", "_") : match.Value.Substring(5);
                //wc.TagName = toFile ? match.Value.Substring(5).Replace(".", "_") : match.Value.Substring(5);
                wc.ChineseName = GetChineseNameAttribute(source);
                wc.ID = GetIDAttribute(source);
                wc.CssFile = GetCssFileAttribute(source);
                wc.Style = GetStyleAttribute(source);
                wc.FileName = GetFileNameAttribute(source);
            }
            return wc;
        }
        /// <summary>
        /// 从文本中解析子模板，放到<div>容器里返回
        /// </summary>
        /// <param name="source">模板文本</param>
        /// <returns></returns>
        private string ConvertSubtemplateToDiv(string source)
        {
            StringBuilder sb = new StringBuilder();
            WeControl wc = new WeControl();
            Regex r = new Regex(@"\<wet:\w*");
            Match match = r.Match(source);

            string tagPrefix = match.Value.Substring(1, 3);
            string tagName = match.Value.Substring(5);
            string templateID = GetIDAttribute(source);

            sb.Append(String.Format("<DIV xmlns:wet=\"http://www.WestEngine.com\" tag=\"{0}\" contenteditable=\"false\" style=\"border:solid gray 1px dotted;width:100px;height:50px;background-color: #faf0aa;\">\r\n", tagPrefix));
            sb.Append(String.Format("<DIV tag=\"{0}\">子模板:<br>{1}</div>\r\n", tagName, templateID));
            sb.Append("<DIV tag=\"content\">\r\n");
            sb.Append(String.Format("<?xml:namespace prefx = {0} />\r\n", tagPrefix));
            sb.Append(source);
            sb.Append("</DIV>\r\n");
            sb.Append("</DIV>\r\n");

            return sb.ToString();
        }


        private string ConvertSubtemplateToImg(string source)
        {
            StringBuilder sb = new StringBuilder();
            WeControl wc = new WeControl();
            Regex r = new Regex(@"\<wet:\w*");
            Match match = r.Match(source);

            string tagPrefix = match.Value.Substring(1, 3);
            string tagName = match.Value.Substring(5);
            string templateID = GetIDAttribute(source);

            source = We7Helper.HtmlEncode(source);
            string icon = "/admin/images/sub_temp.png";
            CreateSubTemplateIcon(tagName);
            string thisIconPath = Path.Combine(We7TemplateGroupPhysicalPath, tagName + ".png");
            if (File.Exists(thisIconPath))
                icon = "/" + We7TemplateGroupRalatePath + "/" + tagName + ".png";

            sb.Append(String.Format("<IMG xmlns:wet=\"http://www.WestEngine.com\" tag=\"{0}\" class=\"{0}\"  control=\"{1}\"  controlName=\"{3}\"  src=\"{2}\"   alt=\"{4}\"  />", tagPrefix, source, icon, tagName, templateID));
            return sb.ToString();
        }

        /// <summary>
        /// 生成子模板标签 图标
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public string CreateSubTemplateIcon(string tagName)
        {
            string icon = "";
            string thisIconPath = Path.Combine(We7TemplateGroupPhysicalPath, tagName + ".png");
            if (!File.Exists(thisIconPath))
            {
                string blankImgPath = HttpContext.Current.Server.MapPath("/admin/images/subtmp_bk.png");
                System.Drawing.Image img = System.Drawing.Image.FromFile(blankImgPath);
                ImageUtils.AddImageSignText(img, thisIconPath, "子模板：" + tagName, 5, 100, "宋体", 18);
                icon = "/" + We7TemplateGroupRalatePath + "/" + tagName + ".png";
            }
            return icon;
        }

        /// <summary>
        /// 用于外部调用
        /// </summary>
        /// <param name="dcontrol">控件英文名称</param>
        /// <param name="id">控件ID</param>
        /// <returns></returns>
        public string CreateControlIcon(string dcontrol, string id)
        {
            HelperFactory helperFactory = HelperFactory.Instance;
            DataControlHelper instance = new DataControlHelper();
            string controlName = "";
            DataControl dc = instance.GetDataControl(dcontrol);
            DataControlInfo info = instance.GetDataControlInfo(dcontrol);
            if (dc == null && info != null)
            {
                controlName = info.Name;
            }
            else
            {
                controlName = dc.Name;
                if (info != null) controlName = info.Name + ":" + controlName;
            }

            return CreateDataControlIcon(controlName, id);
        }


        public string CreateDataControlIcon(string dcName, string id)
        {
            string icon = "/" + We7TemplateGroupRalatePath + "/icons/" + id + ".png"; ;
            string thisIconPath = Path.Combine(We7TemplateGroupPhysicalPath, "icons");
            if (!Directory.Exists(thisIconPath))
                Directory.CreateDirectory(thisIconPath);
            thisIconPath = Path.Combine(thisIconPath, id + ".png");
            if (!File.Exists(thisIconPath))
            {
                string blankImgPath = HttpContext.Current.Server.MapPath("/admin/images/control_bk.png");
                System.Drawing.Image img = System.Drawing.Image.FromFile(blankImgPath);
                ImageUtils.AddImageSignText(img, thisIconPath, dcName + id, 4, 100, "宋体", 12);
            }
            return icon;
        }


        private string ConvertPlaceHolderToDiv(string source)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<DIV xmlns:wem=\"http://www.WestEngine.com\" tag=\"asp\" contenteditable=\"false\" style=\"border:solid gray 1px dotted;width:100px;height:50px;background-color: #ff0000;\">\r\n");
            sb.Append("<DIV tag=\"asp\">内容页位置:MyContentPlaceHolder</div>\r\n");
            sb.Append("<DIV tag=\"content\">\r\n");
            sb.Append("<asp:ContentPlaceHolder ID=\"MyContentPlaceHolder\" runat=\"server\">");
            sb.Append("</asp:ContentPlaceHolder>");
            sb.Append("</DIV>\r\n");
            sb.Append("</DIV>\r\n");

            return sb.ToString();
        }

        private string ConvertPlaceHolderToImg(string source)
        {
            StringBuilder sb = new StringBuilder();
            //string source = "<asp:ContentPlaceHolder ID=\"MyContentPlaceHolder\" runat=\"server\"></asp:ContentPlaceHolder>";
            source = We7Helper.HtmlEncode(source);
            string icon = "/admin/images/icon_holdplace.png";
            sb.Append(String.Format("<IMG xmlns:wem=\"http://www.WestEngine.com\" tag=\"{0}\" class=\"{0}\"  control=\"{1}\"  src=\"{2}\"    />", "wem", source, icon));
            return sb.ToString();
        }

        #endregion

        #region 公用函数


        /// <summary>
        /// 取得控件的路径
        /// </summary>
        /// <param name="control">控件名</param>
        /// <returns></returns>
        string GetControlPath(string control)
        {
            return Path.Combine(control.Split('.')[0] + "/Page/", control + ".ascx");
        }

        /// <summary>
        /// 取得Css文件路径
        /// </summary>
        /// <param name="content">控件标签内容</param>
        /// <returns></returns>
        string GetCssFileAttribute(string content)
        {
            //正则表达式匹配 <.. cssfile='' />
            string pat = @"cssfile=\s*['""](?<file>[^""]*?)['""]";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match match = r.Match(content);
            return match.Groups["file"].Success ? match.Groups["file"].Value : String.Empty;
        }

        /// <summary>
        /// 取得Style中的内容
        /// </summary>
        /// <param name="content">控件标签内容</param>
        /// <returns></returns>
        string GetStyleAttribute(string content)
        {
            //正则表达式匹配 <.. style='' />
            //string pat = @"maincssclass=""([^""]*)""";
            string pat = @"cssclass=\s*['""](?<css>[^""]*?)['""]";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match match = r.Match(content);
            return match.Groups["css"].Success ? match.Groups["css"].Value : String.Empty;
        }

        /// <summary>
        /// 取得控件路径目录
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        string GetFileNameAttribute(string content)
        {
            string pat = @"\sfilename=\s*['""](?<filename>[^""]*?)['""]";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match match = r.Match(content);
            string path = String.Empty;
            if (match.Groups["filename"].Success)
            {
                path = match.Groups["filename"].Value;
            }
            else
            {
                string ctr = GetControlAttribute(content);
                //path = "/We7Controls/" + Path.Combine(ctr.Split('.')[0] + "/Page/", ctr + ".ascx");
                DataControlHelper helper = HelperFactory.Instance.GetHelper<DataControlHelper>();
                if (helper != null)
                {
                    DataControl dc = helper.GetDCByCtrName(ctr);
                    if (dc != null)
                        path = dc.FileName;
                }
            }
            return path;
        }

        /// <summary>
        /// 取得控件属性
        /// </summary>
        /// <param name="content">控件标签内容</param>
        /// <returns></returns>
        string GetControlAttribute(string content)
        {
            //正则表达式匹配 <.. control='' />
            string pat = @"control=\s*[""'](?<ctr>[^""']*?)[""']";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match match = r.Match(content);
            return match.Groups["ctr"].Success ? match.Groups["ctr"].Value : String.Empty;
        }

        /// <summary>
        /// 取得控件中文名
        /// </summary>
        /// <param name="content">控件标签内容</param>
        /// <returns></returns>
        string GetChineseNameAttribute(string content)
        {
            //正则表达式匹配 <.. chinesename='' />
            string pat = @"chinesename\s*=\s*""?([^""\s]*)""?";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match match = r.Match(content);
            string value = match.Value.Substring(match.Value.IndexOf("=") + 1).Replace("\"", "").Replace("'", "");
            return value;
        }

        /// <summary>
        /// 取得控件ID
        /// </summary>
        /// <param name="content">控件标签内容</param>
        /// <returns></returns>
        string GetIDAttribute(string content)
        {
            //正则表达式匹配 <.. chinesename='' />
            string pat = @"id\s*=\s*""?([^""\s]*)""?";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match match = r.Match(content);
            string value = match.Value.Substring(match.Value.IndexOf("=") + 1).Replace("\"", "").Replace("'", "");
            return value;
        }

        /// <summary>
        /// 模板头部信息
        /// </summary>
        /// <param name="content">模板内容</param>
        /// <returns></returns>
        string GetHeadContentFromText(string s)
        {
            try
            {
                string pat = @"\<head[^>]*\>([\w\W]*?)\</head\>";
                Regex r = new Regex(pat, RegexOptions.IgnoreCase);
                Match match = r.Match(s);
                if (!match.Success)
                {
                    throw new Exception("head标记不正确，无法解析，请检查模版文件的正确性！");
                }
                else
                    return match.Groups[1].Value;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 模板内容信息
        /// </summary>
        /// <param name="s">模板内容</param>
        /// <returns></returns>
        string GetBodyContentFromText(string s)
        {
            try
            {
                string pat = @"\<body[^>]*\>([\w\W]*?)\</body\>";
                Regex r = new Regex(pat, RegexOptions.IgnoreCase);
                Match match = r.Match(s);
                if (!match.Success)
                {
                    throw new Exception("body标记不正确，无法解析，请检查模版文件的正确性！");
                }
                else
                {
                    //去除form标记
                    StringBuilder sb = new StringBuilder(match.Groups[1].Value);
                    //AbstractAndRemoveMatchSections(@"\<form[^>]*\>", sb);
                    //AbstractAndRemoveMatchSections(@"\</form\>", sb);
                    return sb.ToString().Trim();
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Body的内容信息
        /// </summary>
        /// <param name="s">模板内容</param>
        /// <returns></returns>
        string GetBodyTextFromText(string s)
        {
            try
            {
                string pat = @"\<body[^>]*";
                Regex r = new Regex(pat, RegexOptions.IgnoreCase);
                Match match = r.Match(s);
                if (!match.Success)
                {
                    throw new Exception("body标记不正确，无法解析，请检查模版文件的正确性！");
                }
                else
                {
                    return match.Value.Remove(0, 5);
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 依据正则表达式字串，将原字符串中相符合规则的内容提取出来
        /// 并把相关内容从原符串中删除
        /// </summary>
        /// <param name="regexStr">正则表达式字串</param>
        /// <param name="sourceStr">原字符串</param>
        /// <returns>提取出的字符串</returns>
        string AbstractAndRemoveMatchSections(string regexStr, StringBuilder sourceStr)
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

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="m">匹配</param>
        /// <returns></returns>
        string Cleanup(Match m)
        {
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
        /// 取得控件在模板下的相对路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string GetTemplatePath(string fileName)
        {
            Regex regex = new Regex(@"(?<=/)\w+/Page/.*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match mc = regex.Match(fileName);
            return mc.Success ? "We7Controls/" + mc.Value : String.Empty;
        }

        /// <summary>
        /// 取得控件名称
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <returns></returns>
        string GetUCName(string filePath)
        {
            //return control.Replace(".", "_");

            string ctr = filePath;
            if (filePath.ToLower().EndsWith(".ascx"))
                ctr = Path.GetFileNameWithoutExtension(filePath);

            if (!String.IsNullOrEmpty(ctr))
                return ctr.Replace(".", "_");
            return String.Empty;
        }

        /// <summary>
        /// 模板编码
        /// </summary>
        public static Encoding DefaultEnCoding = Encoding.Default;

        /// <summary>
        /// 添加新的Link
        /// </summary>
        /// <param name="header">头部信息</param>
        /// <param name="link">链接</param>
        /// <returns></returns>
        string ReplaceLink(string header, string link)
        {
            Regex LinkRegex = new Regex(@"<link([^>]*)href=""/_skins/([^>]*)/css/We7Control.css""([^>]*)?/>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
            if (LinkRegex.IsMatch(header))
            {
                return LinkRegex.Replace(header, link);
            }
            else
            {
                return header + "\r\n" + link + "\r\n";
            }
        }


        #endregion

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
    }

    /// <summary>
    /// 模板处理程序用到的辅助接口
    /// </summary>
    public interface ITemplateProcessorHelper
    {
        /// <summary>
        /// 根据相对路径取得控件的详细配置信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        DataControl GetDataControlByPath(string path);

        /// <summary>
        /// 根据相对路径取得控件组的详细信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        DataControlInfo GetDataControlInfoByPath(string path);

        /// <summary>
        /// 取得控件在模板下的相对路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string GetTemplatePath(string fileName);


        /// <summary>
        /// 取得控件名称
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <returns></returns>
        string GetUCName(string filePath);
    }

    /// <summary>
    /// 模板处理程序用到的辅助接口的默认实现
    /// </summary>
    public class TemplateProcessorHelper : ITemplateProcessorHelper
    {
        DataControlHelper DataControlHelper = HelperFactory.Instance.GetHelper<DataControlHelper>();

        public DataControl GetDataControlByPath(string path)
        {
            return DataControlHelper.GetDataControlByPath(path);
        }

        public DataControlInfo GetDataControlInfoByPath(string path)
        {
            return DataControlHelper.GetDataControlInfoByPath(path);
        }

        /// <summary>
        /// 取得控件在模板下的相对路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetTemplatePath(string fileName)
        {
            Regex regex = new Regex(@"(?<=/)\w+/Page/.*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match mc = regex.Match(fileName);
            return mc.Success ? "We7Controls/" + mc.Value : String.Empty;
        }

        /// <summary>
        /// 取得控件名称
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <returns></returns>
        public string GetUCName(string filePath)
        {
            //return control.Replace(".", "_");

            string ctr = filePath;
            if (filePath.ToLower().EndsWith(".ascx"))
                ctr = Path.GetFileNameWithoutExtension(filePath);

            if (!String.IsNullOrEmpty(ctr))
                return ctr.Replace(".", "_");
            return String.Empty;
        }
    }
}
