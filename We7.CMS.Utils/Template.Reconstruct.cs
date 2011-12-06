using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

namespace We7.CMS
{
    /// <summary>
    /// 模板样式处理类
    /// </summary>
    public class TemplateStyleHelper
    {
        /// <summary>
        /// We7Controls中一个控件样式所占区域的正则表达示
        /// </summary>
        //        const string StyleExp = @"\s*/\*+\s+
        //                                    \*\s+Command:\s*start\s*
        //                                    \*\s+Control:\s*{0}\s*
        //                                    \*\s+Style:\s*{1}\s*
        //                                    \*\s+Desc:\s*(?<desc>.*?)\s*
        //                                    \*\s+Quot:\s*(?<quot>.*?)\s*
        //                                (?=\s+\*+/)
        //                                .*?
        //                                (?<=\s+\*+/)
        //                                \s*(?<css>.*)\s*
        //                                (?=/\*+\s+)
        //                                .*?
        //                                (?<=/\*+\s+)
        //                                    \*\s+Command:\s*end\s*
        //                                    \*\s+Control:\s*{0}\s*
        //                                    \*\s+Style:\s*{1}\s*
        //                                    \*\s+Desc:\s*\k<desc>\s*
        //                                    \*\s+Quot:\s*\k<quot>\s*
        //                                \s+\*+/\s*";

        const string StyleExp = "/[*]+[\\s.*]*Command:\\s*start[\\s.*]*Control:{0}[\\s*]*Style:{1}[\\s*]*Desc:(?<desc>.*?)[\\s*]*Quot:(?<quot>.*)[^/]+/(?<css>[\\s\\S]*?)/[*]+[\\s.*]*Command:\\s*end[\\s.*]*Control:{0}[\\s*]*Style:{1}[\\s*]*Desc:\\k<desc>[\\s*]*Quot:\\k<quot>[^/]*/";

        /// <summary>
        /// 控件信息正则表达示
        /// </summary>
        const string ControlExp = @"(?<!\.\s*)\b{0}\b(?!\s*\.)";
        /// <summary>
        /// 相关引用的正则表达示
        /// </summary>
        const string QuotExp = @"(?<=\*\s*Quot:\s*){0}(?!\w)";
        /// <summary>
        /// 正则表达示选项
        /// </summary>
        const RegexOptions StyleOp = RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase;

        /// <summary>
        /// 添加样式
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="style">样式</param>
        /// <param name="csstxt">样式内容</param>
        /// <param name="template">当前模板</param>
        /// <returns></returns>
        public string AddStyle(string control, string style, string template, string csstxt)
        {
            string pattern = String.Format(StyleExp, control, style);
            //Regex StyleRegex = new Regex(pattern, StyleOp);
            //Match m=StyleRegex.Match(csstxt);
            Match m = Regex.Match(csstxt, pattern, StyleOp);
            if (m.Success)
            {
                Group quot = m.Groups["quot"];
                if (quot != null && quot.Success)
                {
                    string tmp = String.Format(ControlExp, template.Replace(".", @"\."));

                    Regex RegexCtr = new Regex(tmp, RegexOptions.Compiled | RegexOptions.Singleline);
                    if (!RegexCtr.IsMatch(quot.Value))
                    {
                        Regex RegexQuot = new Regex(String.Format(QuotExp, quot.Value.Replace("|", @"\|")), StyleOp);
                        string block = RegexQuot.Replace(m.Value, String.Format("{0}|{1}", quot.Value, template));
                        csstxt = csstxt.Replace(m.Value, block);
                    }
                }
                else
                {
                    throw new Exception("数据配置出错");
                }
            }
            else
            {
                csstxt = csstxt + CreateAppendCssByPath(control, style, template);
            }
            return csstxt.Trim();
        }

        /// <summary>
        /// 添加样式
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="style">样式</param>
        /// <param name="csstxt">样式内容</param>
        /// <param name="template">当前模板</param>
        /// <returns></returns>
        public string AddStyleByPath(string path, string style, string template, string csstxt)
        {
            string control = Path.GetFileNameWithoutExtension(path);

            string pattern = String.Format(StyleExp, control, style);
            //Regex StyleRegex = new Regex(pattern, StyleOp);
            //Match m = StyleRegex.Match(csstxt);
            Match m = Regex.Match(csstxt, pattern, StyleOp);
            if (m.Success)
            {
                Group quot = m.Groups["quot"];
                if (quot != null && quot.Success)
                {
                    string tmp = String.Format(ControlExp, template.Replace(".", @"\."));

                    Regex RegexCtr = new Regex(tmp, RegexOptions.Compiled | RegexOptions.Singleline);
                    if (!RegexCtr.IsMatch(quot.Value))
                    {
                        Regex RegexQuot = new Regex(String.Format(QuotExp, quot.Value.Replace("|", @"\|")), StyleOp);
                        string block = RegexQuot.Replace(m.Value, String.Format("{0}|{1}", quot.Value, template));
                        csstxt = csstxt.Replace(m.Value, block);
                    }
                }
                else
                {
                    throw new Exception("数据配置出错");
                }
            }
            else
            {
                csstxt = csstxt + CreateAppendCssByPath(path, style, template);
            }
            return csstxt.Trim();
        }

        /// <summary>
        /// 删除样式
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="csstxt">Css内容</param>
        /// <param name="template">模板</param>
        /// <param name="style">样式名称</param>
        public string DeleteStyle(string control, string style, string template, string csstxt)
        {
            if (String.IsNullOrEmpty(csstxt))
                return String.Empty;

            string pattern = String.Format(StyleExp, control, style);
            //Regex StyleRegex = new Regex(pattern, StyleOp);
            //Match m = StyleRegex.Match(csstxt);
            Match m = Regex.Match(csstxt, pattern, StyleOp);
            if (m.Success)
            {
                Group quot = m.Groups["quot"];
                if (quot != null && quot.Success)
                {
                    string qs = quot.Value.Trim();
                    string[] qsl = qs.Split('|');
                    template = template.Trim();
                    StringBuilder sb = new StringBuilder();
                    foreach (string s in qsl)
                    {
                        if (s.Trim() == template)
                            continue;
                        sb.AppendFormat("{0}|", s);
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        Regex RegexQuot = new Regex(String.Format(QuotExp, quot.Value.Replace("|", @"\|")), StyleOp);
                        string block = RegexQuot.Replace(m.Value, sb.ToString());
                        csstxt = csstxt.Replace(m.Value, block);
                    }
                    else
                    {
                        csstxt = Regex.Replace(csstxt, pattern, "\r\n", StyleOp);
                    }
                }
                else
                {
                    throw new Exception("数据配置出错");
                }
            }
            return csstxt.Trim();
        }

        /// <summary>
        /// 移除样式
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="style">样式</param>
        /// <param name="csstxt">We7Controls中所有的Css文本</param>
        /// <returns></returns>
        public string RemoveStyle(string control, string style, string csstxt)
        {
            if (String.IsNullOrEmpty(csstxt))
                return String.Empty;

            string pattern = String.Format(StyleExp, control, style);
            //Regex StyleRegex = new Regex(pattern, StyleOp);
            //csstxt=csstxt.Replace(csstxt, "\r\n");            
            csstxt = Regex.Replace(csstxt, pattern, "\r\n", StyleOp);
            return csstxt.Trim();
        }

        /// <summary>
        /// 返回指定的Css样式
        /// </summary>
        /// <param name="constrol">控件</param>
        /// <param name="style">样式</param>
        /// <param name="csstxt">Css文本（We7Controls中所有的）</param>
        /// <returns></returns>
        public string LoadCss(string constrol, string style, string csstxt)
        {
            string s = "";
            try
            {
                string pattern = String.Format(StyleExp, constrol, style);
                //Regex StyleRegex = new Regex(pattern, StyleOp);
                //Match m = StyleRegex.Match(csstxt);
                Match m = Regex.Match(csstxt, pattern, StyleOp);
                if (m.Success)
                {
                    Group css = m.Groups["css"];
                    if (css != null && css.Success)
                    {
                        s = css.Value;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return s;
        }

        /// <summary>
        /// 替换老的Css样式
        /// </summary>
        /// <param name="constrol">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <param name="csstxt">总的Css文本</param>
        /// <param name="newcss">新Css</param>
        /// <returns></returns>
        public string ReplaceCss(string constrol, string style, string csstxt, string newcss)
        {
            try
            {
                string pattern = String.Format(StyleExp, constrol, style);
                //Regex StyleRegex = new Regex(pattern, StyleOp);
                //Match m = StyleRegex.Match(csstxt);
                Match m = Regex.Match(csstxt, pattern, StyleOp);
                if (m.Success)
                {
                    Group css = m.Groups["css"];
                    string h = csstxt.Substring(0, css.Index).Trim();
                    string e = csstxt.Substring(css.Index + css.Length).Trim();
                    string s = h + "\r\n" + newcss + "\r\n" + e;
                    csstxt = s;
                }
            }
            catch (Exception ex)
            {
            }
            return csstxt;
        }

        /// <summary>
        /// 替换老的Css样式
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="style">样式</param>
        /// <param name="csstxt">Css文本</param>
        /// <param name="newcss">新的Css</param>
        /// <param name="template">模板组</param>
        /// <returns></returns>
        public string ReplaceAppendCss(string control, string style, string template, string csstxt, string newcss)
        {
            try
            {
                string pattern = String.Format(StyleExp, control, style);
                //Regex StyleRegex = new Regex(pattern, StyleOp);
                //Match m = StyleRegex.Match(csstxt);
                Match m = Regex.Match(csstxt, pattern, StyleOp);
                if (m.Success)
                {
                    Group css = m.Groups["css"];
                    string h = csstxt.Substring(0, css.Index).Trim();
                    string e = csstxt.Substring(css.Index + css.Length).Trim();
                    string s = h + "\r\n" + newcss + "\r\n" + e;
                    csstxt = s;
                }
                else
                {
                    csstxt += CreateAppendCss(control, style, template, newcss);
                }
            }
            catch (Exception ex)
            {
            }
            return csstxt.Trim();
        }

        /// <summary>
        /// 返回Css样式使用的次数
        /// </summary>
        /// <param name="constrol">控件名称</param>
        /// <param name="style">样式</param>
        /// <param name="csstxt">Css文本</param>
        /// <returns></returns>
        public int StyleUsedCount(string constrol, string style, string csstxt)
        {
            int count = 0;
            try
            {
                string pattern = String.Format(StyleExp, constrol, style);
                //Regex StyleRegex = new Regex(pattern, StyleOp);
                //Match m = StyleRegex.Match(csstxt);
                Match m = Regex.Match(csstxt, pattern, StyleOp);
                if (m.Success)
                {
                    Group quot = m.Groups["quot"];
                    if (quot != null && quot.Success)
                    {
                        string s = quot.Value;
                        if (s.Length > 0)
                            count = s.Trim().Split("|".ToCharArray()).Length;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return count;
        }

        /// <summary>
        /// 是否包含指定样式
        /// </summary>
        /// <param name="constrol">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <param name="csstxt">样式内容</param>
        /// <returns></returns>
        public bool Contains(string constrol, string style, string csstxt)
        {
            try
            {
                string pattern = String.Format(StyleExp, constrol, style);
                //Regex StyleRegex = new Regex(pattern, StyleOp);
                //Match m = StyleRegex.Match(csstxt);
                Match m = Regex.Match(csstxt, pattern, StyleOp);
                return m.Success;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        /// <summary>
        /// 创建创加的样式
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <param name="template">模板</param>
        /// <returns></returns>
        string CreateAppendCss(string control, string style, string template)
        {
            return CreateAppendCss(control, style, template, LoadControlStyle(control, style).Replace("{WE:STYLE}", "." + (control + "_" + style).Replace(".", "_")));

        }

        /// <summary>
        /// 创建创加的样式
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <param name="template">模板</param>
        /// <returns></returns>
        string CreateAppendCssByPath(string path, string style, string template)
        {
            string control = Path.GetFileNameWithoutExtension(path);
            return CreateAppendCss(control, style, template, LoadControlStyleByPath(path, style).Replace("{WE:STYLE}", "." + (control + "_" + style).Replace(".", "_")));

        }

        /// <summary>
        /// 创建创加的样式
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <returns></returns>
        public string CreateAppendCss(string control, string style, string template, string css)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\r\n\r\n\r\n/*************************************************************\r\n");
            sb.Append(" * Command:Start\r\n");
            sb.AppendFormat(" * Control:{0}\r\n", control);
            sb.AppendFormat(" * Style:{0}\r\n", style);
            sb.AppendFormat(" * Desc:{0}\r\n", "目前没有");
            sb.AppendFormat(" * Quot:{0}\r\n", template);
            sb.Append("*************************************************************/\r\n");

            sb.Append(css);

            sb.Append("\r\n");
            sb.Append("/*************************************************************\r\n");
            sb.Append(" * Command:End\r\n");
            sb.AppendFormat(" * Control:{0}\r\n", control);
            sb.AppendFormat(" * Style:{0}\r\n", style);
            sb.AppendFormat(" * Desc:{0}\r\n", "目前没有");
            sb.AppendFormat(" * Quot:{0}\r\n", template);
            sb.Append("*************************************************************/");
            return sb.ToString();

        }

        /// <summary>
        /// 加载控件样式
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <returns></returns>
        string LoadControlStyleByPath(string path, string style)
        {
            string control = Path.GetFileNameWithoutExtension(path);
            string css = "";
            try
            {
                string physicalPath = HttpContext.Current.Server.MapPath(path);
                //string cssdir = Path.Combine(Constants.We7ControlPhysicalPath, control.Split(".".ToCharArray())[0] + "/Style");
                DirectoryInfo[] dis = new FileInfo(physicalPath).Directory.Parent.GetDirectories("Style");
                DirectoryInfo di = dis != null && dis.Length > 0 ? dis[0] : null;

                if (di != null && di.Exists)
                {
                    FileInfo[] fs = di.GetFiles(String.Format("{0}_{1}.*.css", control, style));
                    if (fs.Length > 0)
                    {
                        using (StreamReader sr = new StreamReader(fs[0].FullName, Encoding.Default))
                        {
                            css = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return css;
        }

        /// <summary>
        /// 加载控件样式
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="style">样式名称</param>
        /// <returns></returns>
        string LoadControlStyle(string control, string style)
        {
            string css = "";
            try
            {
                string cssdir = Path.Combine(Constants.We7ControlPhysicalPath, control.Split(".".ToCharArray())[0] + "/Style");
                DirectoryInfo di = new DirectoryInfo(cssdir);

                if (di.Exists)
                {
                    FileInfo[] fs = di.GetFiles(String.Format("{0}_{1}.*.css", control, style));
                    if (fs.Length > 0)
                    {
                        using (StreamReader sr = new StreamReader(fs[0].FullName, Encoding.Default))
                        {
                            css = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return css;
        }
    }

}
