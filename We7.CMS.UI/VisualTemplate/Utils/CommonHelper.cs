using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Web;
using We7.Framework.Util;
using System.IO;

namespace We7.CMS.Module.VisualTemplate.Utils
{
    public class CommonHelper
    {
        //匹配BODY
        private const string RegBodyPattern = @"<body(?<bodyattr>[^>]*)>(?<body>[\s\S]*)</body>";
        //匹配HEAD
        private const string RegHeadPattern = @"<head[^>]*>(?<head>[\s\S]*)</head>";

        /// <summary>
        /// 去除空白行
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveWhitespace(string str)
        {

            try
            {

                return new Regex(@"\s*").Replace(str, string.Empty);

            }

            catch (Exception)
            {

                return str;

            }

        }

        /// <summary>
        /// 获取HTML的head部分
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetHeadContent(string html)
        {
            string headContent = string.Empty;
            Match matchHead = Regex.Match(html, RegHeadPattern, RegexOptions.IgnoreCase);
            if (matchHead.Success)
            {
                headContent = matchHead.Groups["head"].Value;
            }

            return headContent;
        }

        public static string GetBodyContent(string html)
        {
            string bodyContent = string.Empty;
            Match matchBody = Regex.Match(html, RegBodyPattern, RegexOptions.IgnoreCase);
            if (matchBody.Success)
            {
                bodyContent = matchBody.Groups["body"].Value;
            }

            return bodyContent;
        }

        public static string GetBodyAttribute(string html)
        {
            string bodyAttr = string.Empty;

            Match matchBody = Regex.Match(html, RegBodyPattern, RegexOptions.IgnoreCase);
            if (matchBody.Success)
            {
                bodyAttr = matchBody.Groups["bodyattr"].Value;
            }

            return bodyAttr;
        }
        public static void Publish(string designpath, string filepath)
        {
           string file= FileHelper.ReadFile(designpath);
           file = ConvertDivTagToControl(file);
           file = ConvertDivTagToSub(file);
           FileHelper.WriteFile(filepath,file);
        }
        public static string ConvertDivTagToSub(string input)
        {
            string result = string.Empty;
            string pattern = @"<we7design[^>]*data\s*=\s*\""(?<data>[^\""]*)\""[^>]*>([\s\S]*?)</we7design>";
            //替换we7control
            result = Regex.Replace(input, pattern, delegate(Match m)
            {
                var data = m.Groups["data"].Value;

                var type = data;
                return  BuildeWET(type);
            }, RegexOptions.IgnoreCase);
            return result;
        }

        private static string BuildeWET(string type)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 将DIV标签转换为服务器控件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertDivTagToControl(string input)
        {
            string result = string.Empty;
            string pattern = @"<we7design[^>]*class[^>]+we7control[^>]+data\s*=\s*\""(?<data>[^\""]*)\""[^>]*>([\s\S]*?)</we7design>";
            //替换we7control
            result = Regex.Replace(input, pattern, delegate(Match m)
            {
                var data = m.Groups["data"].Value;
                data = Base64Helper.Decode(data);
                var dataDic = JavaScriptConvert.DeserializeObject<Dictionary<string, object>>(data);

                string type = dataDic["ctr"].ToString();
                dataDic.Remove("ctr");
                return BuildeWEC(type, dataDic);
            }, RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// 将子模板包装
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertSubToDiv(string input)
        {
            string result = string.Empty;
            string pattern = @"\<wet:\w+[^>]*\>(\s*)\</wet:\w+\>";
            //替换subcontrol
            result = Regex.Replace(input, pattern, delegate(Match m)
            {
                //获取字符串
                var control = m.Value;
                //获取属性
                Dictionary<string, object> attributes = GetAttributes(control);
                attributes.Add("subtemplate", true);
                string data = JavaScriptConvert.SerializeObject(attributes);
                data = Base64Helper.Encode(data);
                return CreateUserControlWarp("we7subtempalte", data, control);
            }, RegexOptions.IgnoreCase);
            return result;
        }

        private static string ConvertWET(string input)
        {
            string tagPattern = @"</wet:(?<tagname>[^>]+)>";
            string tag = Regex.Match(input, tagPattern, RegexOptions.IgnoreCase).Groups["tagname"].Value;

            var type = ConvertName(tag);
            string data = type;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("<we7design class=\"we7sub\" data=\"{0}\">", data));
            sb.AppendLine(input);
            sb.AppendLine("</we7design>");

            return sb.ToString();
        }
        /// <summary>
        /// 将服务器控件包装
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertControlToDiv(string input)
        {
            string result = string.Empty;
            string pattern = @"\<wec:\w+[^>]*\>(\s*)\</wec:\w+\>";
            //替换we7control
            result = Regex.Replace(input, pattern, delegate(Match m)
            {
                //获取字符串
                var control = m.Value;
                //获取属性
                Dictionary<string,object> attributes=GetAttributes(control);
                attributes.Add("subtemplate", false);
                string data=JavaScriptConvert.SerializeObject(attributes);
                data = Base64Helper.Encode(data);
               return   CreateUserControlWarp("we7controltempalte", data, control);
            }, RegexOptions.IgnoreCase);
            return result;
        }
        public static string ConvertWEC(string input)
        {
            string tagPattern = @"</wec:(?<tagname>[^>]+)>";
            string tag = Regex.Match(input, tagPattern, RegexOptions.IgnoreCase).Groups["tagname"].Value;

            string attrPattern = @"(?<attrname>\w+)\s*=\""(?<attrvalue>[^\""]*)\""";
            MatchCollection attributes = Regex.Matches(input, attrPattern, RegexOptions.IgnoreCase);

      
             Dictionary<string, object> Parameters = new Dictionary<string, object>();
             var type = ConvertName(tag);
            Parameters.Add("ctr",type);
            for (int i = 0; i < attributes.Count; i++)
            {
                if (attributes[i].Success)
                {
                    var name = attributes[i].Groups["attrname"].Value;
                    var value = attributes[i].Groups["attrvalue"].Value;
                    if (name.Trim().ToLower() == "runat")
                    {
                        continue;
                    }
                    else
                    {
                      Parameters.Add(name.ToLower().Trim(), value);
                    }
                }
            }
            string data=JavaScriptConvert.SerializeObject(Parameters);
            data = Base64Helper.Encode(data);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("<we7design class=\"we7control\" data=\"{0}\">", data));
            sb.AppendLine(input);
            sb.AppendLine("</we7design>");

            return sb.ToString();
        }


        public static string BuildeWEC(string name, Dictionary<string, object> data)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<wec:{0}", ConvertUCName(name));
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    sb.AppendFormat(" {0}=\"{1}\"", item.Key, HttpUtility.HtmlDecode(item.Value.ToString()));
                }
            }
            sb.Append(" runat=\"server\"");
            sb.Append(">");
            sb.AppendFormat("</wec:{0}>", ConvertUCName(name));

            return sb.ToString();
        }
        private static string ConvertUCName(string type)
        {
            return type.Replace('.', '_');
        }
        private static string ConvertName(string type)
        {
            return type.Replace('_', '.');
        }

        /// <summary>
        /// 将页面转换成设计时页面
        /// </summary>
        /// <param name="path">原文件路径</param>
        /// <param name="designPath">设计时文件路径</param>
        public static void Convert2DesignPage(string path,string designPath)
        {
            string inputHtml = FileHelper.ReadFile(path);
            //转换控件
           string tempHtml= ConvertControlToDiv(inputHtml);
            //转换子模板
           tempHtml = ConvertSubToDiv(tempHtml);
           FileHelper.WriteFile(designPath,tempHtml);
        }
        /// <summary>
        /// 发布页面
        /// </summary>
        /// <param name="designPath"></param>
        /// <param name="path"></param>
        public static void Convert2Page(string designPath, string path)
        {
            string inputHtml = FileHelper.ReadFile(designPath);

            string tempHtml = ConvertDivTag(inputHtml);

            FileHelper.WriteFile(path, tempHtml);
        }
        /// <summary>
        /// 将包裹后的字符串更换为服务器控件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertDivTag(string input)
        {
            string result = string.Empty;
            string pattern = @"<we7design[^>]*data\s*=\s*\""(?<data>[^\""]*)\""[^>]*>([\s\S]*?)</we7design>";
            //替换we7control
            result = Regex.Replace(input, pattern, delegate(Match m)
            {
                var data = m.Groups["data"].Value;

                string attributeString = Base64Helper.Decode(data);
                Dictionary<string, object> attribute = JavaScriptConvert.DeserializeObject<Dictionary<string, object>>(attributeString);
                var tagName = attribute["ctr"].ToString();
                tagName = ConvertUCName(tagName);

                var id = attribute["id"].ToString();

                var sub = attribute["subtemplate"];
                attribute.Remove("subtemplate");
                attribute.Remove("ctr");
                attribute.Remove("id");

                if (sub.ToString().ToLower() == "true")
                {
                    return CreateUserControl("wet", tagName,id, null);
                }
                else
                {
                 return CreateUserControl("wec", tagName,id, attribute);
                }

            }, RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// 根据服务端字符串获取属性
        /// </summary>
        /// <param name="controlString"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetAttributes(string controlString)
        {
            string tagPattern = @"</\w+:(?<tagname>[^>]+)>";
            string tag = Regex.Match(controlString, tagPattern, RegexOptions.IgnoreCase).Groups["tagname"].Value;
            string type = ConvertName(tag);

            string attrPattern = @"(?<attrname>\w+)\s*=\""(?<attrvalue>[^\""]*)\""";
           
            MatchCollection attributes = Regex.Matches(controlString, attrPattern, RegexOptions.IgnoreCase);

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("ctr", type);
            for (int i = 0; i < attributes.Count; i++)
            {
                if (attributes[i].Success)
                {
                    var name = attributes[i].Groups["attrname"].Value;
                    var value = attributes[i].Groups["attrvalue"].Value;
                    if (name.Trim().ToLower() == "runat")
                    {
                        continue;
                    }
                    else
                    {
                        Parameters.Add(name.ToLower().Trim(), value);
                    }
                }
            }

            return Parameters;
        }
        /// <summary>
        /// 包装服务器控件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="usercontrol"></param>
        /// <returns></returns>
        public static string CreateUserControlWarp(string type, string data,string usercontrol)
        {
           return  string.Format("<we7design class=\"{0}\" data=\"{1}\">{2}</we7design>",type,data,usercontrol);
        }
        /// <summary>
        /// 生成服务端控件字符串
        /// </summary>
        /// <param name="tagPrefix">前缀</param>
        /// <param name="tagName">名称</param>
        /// <param name="id">控件Id</param>
        /// <param name="attributes">属性</param>
        /// <returns></returns>
        public static string CreateUserControl(string tagPrefix, string tagName, string id, IDictionary<string, object> attributes)
        {
            //Check Arguments
            if (string.IsNullOrEmpty(tagPrefix))
            {
                throw new ArgumentNullException("TagPrefix为空或Null");
            }
            if (string.IsNullOrEmpty(tagName))
            {
                throw new ArgumentNullException("TagName为空或Null");
            }
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Id为空或Null");
            }
            //保存attribute字符串
            StringBuilder sb = new StringBuilder();

            //attribute以为空
            if (attributes != null && attributes.Count > 0)
            {
                foreach (var item in attributes)
                {
                    sb.AppendFormat(" {0}=\"{1}\"", item.Key, item.Value);
                }
            }
            return string.Format("<{0}:{1} id=\"{3}\"  {2}  runat=\"server\"></{0}:{1}>", tagPrefix, tagName, sb.ToString(), id);
        }


        public static void WriteFile(string file, string content)
        {

        }
    }
}
