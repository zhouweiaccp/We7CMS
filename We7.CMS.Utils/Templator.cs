using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using We7.Framework;

namespace We7.CMS
{
    /// <summary>
    /// 模板编辑处理类（已过时：2.1版本以前在用）
    /// </summary>
    [Obsolete]
    public class Templator
    {
        string headContent;
        string bodyContent;
        string fileName;
        string bodyText;
        string title;
        List<WeControl> controls;
        List<string> templates;
        /// <summary>
        /// 公用的一组正则表达式数组
        /// </summary>
        public static Regex[] TheRegexs = new Regex[4];

        /// <summary>
        /// 子模板名称列表
        /// </summary>
        public List<string> Templates
        {
            get { return templates; }
            
        }
        string input;
        //string output;


        public Templator()
        {
            controls = new List<WeControl>();
            templates = new List<string>();

            RegexOptions options = RegexOptions.IgnoreCase;

            TheRegexs[0] = new Regex(@"\<wec:\w+[^>]*\>(\s*)\</wec:\w+\>", options);
            TheRegexs[1] = new Regex(@"\<wet:\w+[^>]*\>(\s*)\</wet:\w+\>", options);
            TheRegexs[2] = new Regex(@"\<div([^>]*)xmlns:wec=[^>]*\>[^<>]*(((?'Open'\<div[^>]*\>)([\w\W]*?))+((?'-Open'\</div\>)[^<>]*)+)*(?(Open)(?!))\</div\>", options);
            TheRegexs[3] = new Regex(@"\<div([^>]*)xmlns:wet=[^>]*\>[^<>]*(((?'Open'\<div[^>]*\>)([\w\W]*?))+((?'-Open'\</div\>)[^<>]*)+)*(?(Open)(?!))\</div\>", options);
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
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 模板输入
        /// </summary>
        public string Input
        {
            get { return input; }
            set { input = value; }
        }

        /// <summary>
        /// 模板控件
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
            set { isSubTemplate = value; }
        }
        

        #region save 保存……
        /// <summary>
        /// 保存模板
        /// </summary>
        public void Save()
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            if (IsSubTemplate)
                SaveSub();
            else
            {
                using (FileStream fs = File.Open(FileName, FileMode.CreateNew, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        List<string> tags = new List<string>();
                        foreach (WeControl c in Controls)
                        {
                            if (!tags.Contains(c.TagName))
                                tags.Add(c.TagName);
                        }

                        sw.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");

                        foreach (string c in tags)
                        {
                            sw.WriteLine(string.Format("<%@ Register Src=\"~" + Constants.ControlUrlPath + "/{0}.ascx\" TagName=\"{0}\" TagPrefix=\"wec\" %>", c));
                        }

                        foreach (String c in Templates)
                        {
                            sw.WriteLine(string.Format("<%@ Register Src=\"~" + Constants.TemplateUrlPath + "/{0}.ascx\" TagName=\"{0}\" TagPrefix=\"wet\" %>", c));
                        }

                        sw.Write("<html xmlns:wec=\"http://www.WestEngine.com\">\r\n");
                        sw.Write("<head runat=\"server\">\r\n");
                        //if(headContent.IndexOf("text/html; charset=UTF-8")<0)
                        //    sw.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\r\n");

                        HeadContent = FormatHeadMeta(HeadContent);
                        sw.Write("<title>");
                        sw.Write(Title);
                        sw.WriteLine("</title>");

                        foreach (WeControl c in Controls)
                        {
                            string cssFile = "";
                            if (CopyStyleSheet(c, ref cssFile))
                            {
                                string linkfile = string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + Constants.TemplateUrlPath + "/styles/{0}\" media=\"screen\" />", cssFile);
                                if (HeadContent.IndexOf(cssFile) < 0)
                                    HeadContent = HeadContent + "\r\n" + linkfile;
                            }
                        }

                        sw.Write(HeadContent);

                        sw.WriteLine("</head>");
                        sw.Write("<body");
                        sw.Write(BodyText);
                        sw.Write(">\r\n");
                        BodyContent = BodyContent.Replace("<#text />", "");
                        sw.Write(BodyContent);
                        sw.Write("</body>");
                        sw.Write("</html>");
                        sw.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// 保存子模板
        /// </summary>
        /// <param name="sub"></param>
        void SaveSub()
        {
            using (FileStream fs = File.Open(FileName, FileMode.CreateNew, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    List<string> tags = new List<string>();
                    foreach (WeControl c in Controls)
                    {
                        if (!tags.Contains(c.TagName))
                            tags.Add(c.TagName);
                    }

                    foreach (string c in tags)
                    {
                        sw.WriteLine(string.Format("<%@ Register Src=\"~" + Constants.ControlUrlPath + "/{0}.ascx\" TagName=\"{0}\" TagPrefix=\"wec\" %>", c));
                    }

                    foreach (String c in Templates)
                    {
                        sw.WriteLine(string.Format("<%@ Register Src=\"~" + Constants.TemplateUrlPath + "/{0}.ascx\" TagName=\"{0}\" TagPrefix=\"wet\" %>", c));
                    }

                    foreach (WeControl c in Controls)
                    {
                        string cssFile = "";
                        if (CopyStyleSheet(c, ref cssFile))
                        {
                            string linkfile = string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + Constants.TemplateUrlPath + "/styles/{0}\" media=\"screen\" />", cssFile);
                            if (HeadContent.IndexOf(cssFile) < 0)
                                HeadContent = HeadContent + "\r\n" + linkfile;
                        }
                    }

                    sw.WriteLine(HeadContent);
                    sw.Write(BodyContent);
                    sw.Flush();
                }
            } 
        }

        /// <summary>
        /// 从输入框的文本转换处理后存入BodyContent
        /// </summary>
         public void FromVisualBoxText()
        {
            String s = Input;
            int start = s.IndexOf("<?");
            while (start >= 0)
            {
                int end = s.IndexOf("/??>", start);
                if (end > 0)
                {
                    s = s.Remove(start, end - start + 4);
                }
                else
                {
                    start = -1;
                }
            }
            s = ConvertTagsToControls(s);

            //检查是否有FORM节点。
            //TODO: 需要确保Control一定在Form节点中。
             Regex r=new Regex(@"\<form");
             Match m = r.Match(s);
             if (!m.Success && !IsSubTemplate)
             {
                 s = "<form id=\"mainForm\" runat=\"server\">\r\n" + s;
                 s = s + "\r\n</form>\r\n";
             }

            bodyContent = We7Helper.FilterXMLChars(s);

        }

 
        /// <summary>
        /// 复制数据控件的css文件到引用模板的相关路径
        /// </summary>
        /// <param name="ControlTag"></param>
        /// <param name="ControlID"></param>
        /// <returns></returns>
        public bool CopyStyleSheet(WeControl control, ref string fullname)
        {
            if (string.IsNullOrEmpty(control.CssFile)) //使用默认css
            {
                HttpContext Context = HttpContext.Current;
                string controlCssFile = Context.Server.MapPath(Constants.ControlUrlPath) + "\\styles\\" + control.TagName + ".css";

                string templateCssFile = fileName.Remove(fileName.LastIndexOf("\\")) + "\\styles\\" + fileName.Substring(fileName.LastIndexOf("\\"), fileName.LastIndexOf(".") - fileName.LastIndexOf("\\")) + "_" + control.TagName + ".css";
                FileInfo cfile = new FileInfo(controlCssFile);

                if (cfile.Exists)
                {
                    FileInfo tfile = new FileInfo(templateCssFile);
                    if (!tfile.Exists)
                    {
                        string path = fileName.Remove(fileName.LastIndexOf("\\")) + "\\styles\\";
                        DirectoryInfo di = new DirectoryInfo(path);
                        if (!di.Exists) di.Create();

                        cfile.CopyTo(templateCssFile);
                    }

                    fullname = templateCssFile.Substring(templateCssFile.LastIndexOf("\\") + 1);
                    return true;
                }
            }
            else //使用指定css
            {
                fullname =  control.CssFile;
                return true;
            }
            return false;
        }


        string FormatHeadMeta(string headContent)
        {
            //删除已有title标签
            StringBuilder sb = new StringBuilder(headContent);
            AbstractAndRemoveMatchSections(@"\<title[^>]*\>([\w\W]*?)\</title\>", sb);
            headContent = sb.ToString();

            //Content-Type：网页编码
            string rs = @"\<(meta)(.*|.*(\r\n).*|.*)(http-equiv)\s*=\s*""(content-type)""(.*|.*(\r\n).*|.*)(content)\s*=\s*""(.*|.*(\r\n).*|.*)""(.*|.*(\r\n).*|.*)\>";
            string strMeta = "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";

            Regex reg = new Regex(rs, RegexOptions.IgnoreCase);
            Match m = reg.Match(headContent);
            if (m.Success)
                headContent = reg.Replace(headContent, strMeta);
            else
                headContent += "" + strMeta;

            //meta：描述description
            rs = @"\<(meta)(.*|.*(\r\n).*|.*)(name)\s*=\s*""(description)""(.*|.*(\r\n).*|.*)(content)\s*=\s*""(.*|.*(\r\n).*|.*)""(.*|.*(\r\n).*|.*)\>";
            strMeta = "";

            reg = new Regex(rs, RegexOptions.IgnoreCase);
            m = reg.Match(headContent);
            if (m.Success)
                headContent = reg.Replace(headContent, strMeta);

            //meta：关键词keywords
            rs = @"\<(meta)(.*|.*(\r\n).*|.*)(name)\s*=\s*""(keywords)""(.*|.*(\r\n).*|.*)(content)\s*=\s*""(.*|.*(\r\n).*|.*)""(.*|.*(\r\n).*|.*)\>";
            strMeta = "";

            reg = new Regex(rs, RegexOptions.IgnoreCase);
            m = reg.Match(headContent);
            if (m.Success)
                headContent = reg.Replace(headContent, strMeta);

            return headContent;
        }



        /// <summary>
        /// 转换文本中的div标签为控件
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        string ConvertTagsToControls(string inputStr)
        {
            string strReturn = "";
            string strTemplate;
            Controls.Clear();
            Templates.Clear();
            strTemplate = inputStr;

            foreach (Match m in TheRegexs[2].Matches(strTemplate))
            {
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), GetWeControl(m.Groups[0].ToString()));
            }

            foreach (Match m in TheRegexs[3].Matches(strTemplate))
            {
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), GetWeTemplate(m.Groups[0].ToString()));
            }

            return strTemplate;
        }

        /// <summary>
        /// 从字符串中返回其中的控件部分wec:
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        string GetWeControl(string inputStr)
        {
            Match match = TheRegexs[0].Match(inputStr);
            WeControl control = GetWeControlFromString(inputStr);
            if (!Controls.Contains(control))
            {
                Controls.Add(control);
            }
            return match.Value;
        }
        /// <summary>
        /// 从字符串中返回其中的模板部分
        /// </summary>
        /// <param name="inputStr"></param>
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

        #endregion

        #region load 载入……

        public void Load()
        {
            if (IsSubTemplate)
                LoadSub();
            else
            {
                using (FileStream fs = File.Open(FileName, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
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
            }
        }

        public void LoadSub()
        {
            using (FileStream fs = File.Open(FileName, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string s = sr.ReadToEnd();
                    StringBuilder sb = new StringBuilder(s);
                    
                    string regexStr = @"\<%@.*.%\>";
                    AbstractAndRemoveMatchSections(regexStr, sb);//去除<%@ Register %>项
                   
                    regexStr = @"\<link.*.\>";
                    headContent += AbstractAndRemoveMatchSections(regexStr, sb);

                    regexStr = @"(<style)+[^<>]*>[^\0]*(<\/style>)+";
                    headContent += AbstractAndRemoveMatchSections(regexStr, sb);
                    /*
                    regexStr = @"(<script)+[^<>]*>[^\0]*(<\/script>)+";
                    headContent += AbstractAndRemoveMatchSections(regexStr, sb);
                    */
                    
                    bodyContent =ConvertControlsToTags(sb.ToString());
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
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), ConvertWeControlsToDiv(m.Groups[0].ToString()));
            }

            foreach (Match m in TheRegexs[1].Matches(strTemplate))
            {
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), ConvertSubtemplateToDiv(m.Groups[0].ToString()));
            }

            return strTemplate;
        }

        /// <summary>
        /// 从文本中解析控件，放到<div>容器里返回
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string  ConvertWeControlsToDiv(string source)
        {
            StringBuilder sb = new StringBuilder();
            WeControl wc = GetWeControlFromString(source);

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
        /// 从文本中解析控件，返回WeControl控件描述对象
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        WeControl GetWeControlFromString(string source)
        {
            WeControl wc = new WeControl();
            Regex r = new Regex(@"\<wec:\w*");
            Match match = r.Match(source);
            if (match.Success)
            {
                wc.TagPrefix = match.Value.Substring(1, 3);
                wc.TagName = match.Value.Substring(5);
                wc.ChineseName = GetChineseNameAttribute(source);
                wc.ID = GetIDAttribute(source);
                wc.CssFile = GetCssFileAttribute(source);
            }
            return wc;
        }
        /// <summary>
        /// 从文本中解析子模板，放到<div>容器里返回
        /// </summary>
        /// <param name="source"></param>
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

        #endregion

        #region 公用函数
        string GetCssFileAttribute(string content)
        {
            //正则表达式匹配 <.. cssfile='' />
            string pat = @"cssfile=""([^""]*)""";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match match = r.Match(content);
            string file = match.Value.Substring(match.Value.IndexOf("=") + 1).Replace("\"", "").Replace("'", "");
            return file;
        }

        string GetChineseNameAttribute(string content)
        {
            //正则表达式匹配 <.. chinesename='' />
            string pat = @"chinesename\s*=\s*""?([^""\s]*)""?";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match match = r.Match(content);
            string value = match.Value.Substring(match.Value.IndexOf("=") + 1).Replace("\"", "").Replace("'", "");
            return value;
        }

        string GetIDAttribute(string content)
        {
            //正则表达式匹配 <.. chinesename='' />
            string pat = @"id\s*=\s*""?([^""\s]*)""?";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match match = r.Match(content);
            string value = match.Value.Substring(match.Value.IndexOf("=") + 1).Replace("\"", "").Replace("'", "");
            return value;
        }

        string GetHeadContentFromText(string s)
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

        string GetBodyContentFromText(string s)
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
                AbstractAndRemoveMatchSections(@"\<form[^>]*\>", sb);
                AbstractAndRemoveMatchSections(@"\</form\>", sb);
                return sb.ToString().Trim();
            }
        }

        string GetBodyTextFromText(string s)
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

        string Cleanup(Match m)
        {
            return String.Empty;
        }

        #endregion
    }



    /// <summary>
    /// 控件对象，用于描述在模板中表述的数据控件
    /// </summary>
    public class WeControl
    {
        string id;
        string tagName;
        string cssfile;
        string tagPrefix;
        string chineseName;
        string style;
        string ctrdir,ctrtag;

        public string ChineseName
        {
            get { return chineseName; }
            set { chineseName = value; }
        }

        public string TagPrefix
        {
            get { return tagPrefix; }
            set { tagPrefix = value; }
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        public string TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }

        public string CssFile
        {
            get { return cssfile; }
            set { cssfile = value; }
        }

        public string Style
        {
            get { return style; }
            set { style = value; }
        }

        public string Control
        {
            get
            {
                return TagName.Replace("_", ".");
            }
        }

        public string FileName { get; set; }


        public string CtrDir
        {
            get
            {
                if (string.IsNullOrEmpty(ctrdir))
                {
                    ctrdir = Control.Split('.')[0];
                }
                return ctrdir;
            }
        }

        public string CtrTag
        {
            get
            {
                if (string.IsNullOrEmpty(ctrtag))
                {
                    ctrtag = Control.Split('.')[1];
                }
                return ctrtag;
            }
        }

    }
}