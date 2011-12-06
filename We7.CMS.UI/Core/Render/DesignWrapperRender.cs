using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Util;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;
using We7.Framework.TemplateEnginer;
using HtmlAgilityPack;
using System.IO;
using We7.CMS.WebControls.Core;

namespace We7.CMS.WebControls
{
    public class DesignWrapperRender : IRender
    {
        /// <summary>
        /// 是否为设计状态
        /// </summary>
        private bool Design
        {
            get;
            set;
        }
        /// <summary>
        /// 是否是AJAX操作
        /// </summary>
        private bool Action
        {
            get;
            set;
        }

        /// <summary>
        /// 控件完全限定名
        /// </summary>
        public virtual string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 模板组名称
        /// </summary>
        public virtual string Group
        {
            get;
            set;
        }
        /// <summary>
        /// 控件属性
        /// </summary>
        Dictionary<string, object> WidgetParameter
        {
            get;
            set;
        }
        /// <summary>
        /// 获取属性JSON字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string AttributesJsonData(UserControl control)
        {

          //DCHelper help = new DCHelper();
          //var p=  help.PickUp(control);

          // return p.ToJson();
            StringBuilder sb = new StringBuilder();

            //ajax操作
            if (Action)
            {


                sb.Append("{\"type\":\"wec\"");
                sb.Append(",\"data\":{");
                sb.AppendFormat("\"ctr\":\"{0}\"", Path.GetFileNameWithoutExtension(WidgetParameter["filename"].ToString()));
                sb.Append(",\"atts\":{");
                if (WidgetParameter != null && WidgetParameter.Count > 0)
                {
                    foreach (var attr in WidgetParameter)
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\",", attr.Key.ToLower(), attr.Value);
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("}");
                sb.Append("}");
                sb.Append("}");
            }
            //页面加载
            else
            {
                Group = RequestHelper.Get<string>("folder");
                FileName = RequestHelper.Get<string>("file");
                string path = HttpContext
                .Current.Server.MapPath(string.Format("~/_skins/~{0}/{1}", Group, FileName));

                HtmlDocument doc = new HtmlDocument();
                doc.OptionAutoCloseOnEnd = true;
                doc.OptionCheckSyntax = true;
                doc.OptionOutputOriginalCase = true;
                try
                {
                    doc.Load(path, Encoding.UTF8);
                }
                catch
                {
                    throw new Exception("格式化HTML错误");
                }

                var node = doc.GetElementbyId(control.ID);
                if (node != null)
                {
                    string ctr = node.Name.Split(new char[] { ':' })[1];

                    sb.Append("{type:\"wec\"");
                    sb.Append(",data:{");
                    sb.AppendFormat("ctr:\"{0}\"", ctr.Replace("_", "."));
                    sb.Append(",atts:{");
                    if (node != null && node.Attributes.Count > 0)
                    {
                        foreach (var attr in node.Attributes)
                        {
                            sb.AppendFormat("\"{0}\":\"{1}\",", attr.Name.ToLower(), attr.Value);
                        }
                        sb.Remove(sb.Length - 1, 1);
                    }
                    sb.Append("}");
                    sb.Append("}");
                    sb.Append("}");
                }
            }
            return sb.ToString();
        }

    
        #region IRender 成员

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        protected virtual string FormatHtml(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.OptionAutoCloseOnEnd = true;
            doc.OptionCheckSyntax = true;
            doc.OptionOutputOriginalCase = true;
            try
            {
                doc.LoadHtml(html);
            }
            catch
            {
                throw new Exception("格式化HTML错误");
            }
            StringWriter output = new StringWriter();
            doc.Save(output);

            return output.ToString();
        }

        public void Render(RenderChain renders, ref string content, RenderArguments args)
        {
            Design = !string.IsNullOrEmpty(args.Request["state"]);
            if (Design)
            {
                UserControl uc = null;

                Action = !string.IsNullOrEmpty(args.Request["action"]);
                if (Action)
                {
                    WidgetParameter = new Dictionary<string, object>();
                    var param = RequestHelper.Get<string>("params");
                    if (!string.IsNullOrEmpty(param))
                    {
                        param = Base64Helper.Decode(param);
                        WidgetParameter = JavaScriptConvert.DeserializeObject<Dictionary<string, object>>(param);
                    }
                }
                else
                {
                    uc = args.Control;
                    FileName = args.Request["file"].ToString();
                }
                //执行
                //格式化代码
                content = FormatHtml(content);
                

                NVelocityHelper helper = new NVelocityHelper(We7.CMS.Constants.VisualTemplatePhysicalTemplateDirectory);
                helper.Put("controlId", args.Control.ID);
                helper.Put("controlName", string.Empty);
                helper.Put("controlContent", content);
                helper.Put("controlData", AttributesJsonData(args.Control));

                var rendHtml = helper.Save("We7ControlDesign.vm");

                //格式化
                content = FormatHtml(rendHtml);
        
            }
            renders.DoRender(ref content, args);
        }

        #endregion

        private class Base64Helper
        {
            /// <summary>
            /// 解码。
            /// </summary>
            /// <param name="base64Str"></param>
            /// <returns></returns>
            public static string Decode(string base64Str)
            {
                return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64Str));
            }
            /// <summary>
            /// 编码。
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static string Encode(string utf8Str)
            {
                return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(utf8Str));
            }
        }
    }
}
