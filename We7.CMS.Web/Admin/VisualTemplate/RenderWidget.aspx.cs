using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HtmlAgilityPack;
using Newtonsoft.Json;
using VisualDesign.Module;
using We7.CMS.Module.VisualTemplate;
using We7.CMS.Module.VisualTemplate.Utils;
using We7.Framework.Util;
using We7.CMS.WebControls.Core;
namespace We7.CMS.Web.Admin.VisualTemplate
{
    /// <summary>
    /// 异步获取Widget呈现的HTML代码
    /// </summary>
    public partial class RenderWidget : System.Web.UI.Page
    {
        #region Private Field

        /// <summary>
        /// 控件虚拟路径私有值
        /// </summary>
        private string _virtualWidgetPath = string.Empty;
        /// <summary>
        /// 包裹容器的ID
        /// </summary>
        private const string WarpDivId = "We7RenderUCWarpId";
        private const string WarpHeadId = "We7RenderUCHeadId";

        #endregion

        #region Property

        /// <summary>
        /// 操作类型（添加或者修改 add/edit）
        /// </summary>
        protected string Action
        {
            get;
            set;
        }
        /// <summary>
        /// widget参数
        /// </summary>
        public Dictionary<string, object> WidgetParameter
        {
            get;
            set;

        }
        /// <summary>
        /// 模板组名称
        /// </summary>
        protected string TemplateGroup
        {
            get;
            set;
        }
        /// <summary>
        /// 模板文件完全名称(包括扩展名)
        /// </summary>
        protected string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 兄弟节点ID
        /// </summary>
        protected string NextId
        {
            get;
            set;
        }

        /// <summary>
        /// 父节点ID
        /// </summary>
        protected string Target
        {
            get;
            set;
        }

        /// <summary>
        /// widget虚拟路径
        /// </summary>
        protected string VirtualWidgetPath
        {
            get;
            set;
        }

        #endregion


        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            //初始化参数
            GetUrlParamenter();

            //添加控件到页面
            AddUserControl(VirtualWidgetPath, WidgetParameter);
        }
        /// <summary>
        /// 重写Render
        /// </summary>
        /// <param name="writer">输出流</param>
        protected override void Render(HtmlTextWriter writer)
        {

            StringWriter output = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(output);

            base.Render(tw);
            string controlHtml = output.ToString();

            Response.Write(Format(controlHtml));
            //格式化所需数据,分离head和body
            //var ajaxData = CreateAjaxData(controlHtml);
            //AjaxReturn.Success = true;
            //AjaxReturn.Data = ajaxData;

            //插入节点
            if (string.Compare(Action, "add", true) == 0)
            {
                AddNode();
            }
            //修改节点
            if (string.Compare(Action, "edit", true) == 0)
            {
                EditNode();
            }
            if (string.Compare(Action, "paste", true) == 0)
            {
                PasteNode();
            }

            Response.ContentType = "text/plain; charset=utf-8";

            Response.Flush();
            Response.End();
        }

        #region Private Method

        /// <summary>
        /// 添加节点到文件
        /// </summary>
        private void AddNode()
        {
            string tagName = Path.GetFileNameWithoutExtension(WidgetParameter["filename"].ToString()).Replace(".", "_");
            VisualDesignFile vdFile = new VisualDesignFile(TemplateGroup, FileName);
            var node = vdFile.CreateNode("wew", tagName, WidgetParameter);
            vdFile.Insert(Target, node, NextId, null);

            ///添加样式
            vdFile.Styles(ConvertControlPathToStylePath(WidgetParameter["filename"].ToString()));

            vdFile.SaveDraft(true);

        }

        /// <summary>
        /// 复制节点属性
        /// </summary>
        private void PasteNode()
        {
            VisualDesignFile vdFile = new VisualDesignFile(TemplateGroup, FileName);
            var node = vdFile.GetElementById(NextId);
            //for (int i = 0; i < node.Attributes.Count; i++)
            //{
            //    if (WidgetParameter.ContainsKey(node.Attributes[i].Name) && (string.Compare(node.Attributes[i].Name, "id", true) != 0))
            //    {
            //        node.Attributes[i].Value = WidgetParameter[node.Attributes[i].Name].ToString();
            //    }

            //}
            foreach (KeyValuePair<string, object> item in WidgetParameter)
            {
                if (item.Key != "id")
                {
                    if (node.Attributes.Contains(item.Key))
                    {
                        node.Attributes[item.Key].Value = item.Value.ToString();
                    }
                    else
                        node.Attributes.Add(item.Key, item.Value.ToString());
                }
            }
            vdFile.Replace(NextId, node);
            vdFile.SaveDraft(true);
        }


        /// <summary>
        /// 修改节点到文件
        /// </summary>
        private void EditNode()
        {
            string tagName = Path.GetFileNameWithoutExtension(WidgetParameter["filename"].ToString()).Replace(".", "_");
            VisualDesignFile vdFile = new VisualDesignFile(TemplateGroup, FileName);
            var newNode = vdFile.CreateNode("wew", tagName, WidgetParameter);

            var originalId = RequestHelper.Get<string>("original");

            vdFile.Replace(originalId, newNode);

            ///添加样式
            vdFile.Styles(ConvertControlPathToStylePath(WidgetParameter["filename"].ToString()));

            vdFile.SaveDraft(true);

        }

        /// <summary>
        /// 从URL获取参数赋值
        /// </summary>
        private void GetUrlParamenter()
        {
            var param = Request["params"]; //RequestHelper.Get<string>("params");
            if (!string.IsNullOrEmpty(param))
            {
                param = Base64Helper.Decode(param);
                WidgetParameter = JavaScriptConvert.DeserializeObject<Dictionary<string, object>>(param);
            }
            var tempFile = WidgetParameter["filename"];

            /*
             * update by glg 2011-6-22
             * update:
             *       orginal: if (WidgetParameter.ContainsKey("CssClass"))
             *       now:     if (WidgetParameter.ContainsKey("CssClass") || WidgetParameter.ContainsKey("cssclass"))
             * reason: 调用PasteNode()时 传入的参数为小写
             */
            if (WidgetParameter.ContainsKey("CssClass") || WidgetParameter.ContainsKey("cssclass"))
                if (WidgetParameter.ContainsKey("CssClass"))
                    WidgetParameter["CssClass"] = WidgetParameter["CssClass"].ToString().Replace(".", "_");
                else
                    WidgetParameter["cssclass"] = WidgetParameter["cssclass"].ToString().Replace(".", "_");
            else
                WidgetParameter.Add("CssClass", Path.GetFileNameWithoutExtension(tempFile.ToString()).Replace(".", "_"));

            if (tempFile != null)
            {
                var file = tempFile.ToString();

                if (file.StartsWith("/"))
                {
                    file = "~" + file;
                }
                VirtualWidgetPath = file;
            }
            Action = RequestHelper.Get<string>("action");
            Target = RequestHelper.Get<string>("target");
            NextId = RequestHelper.Get<string>("nextid");
            TemplateGroup = RequestHelper.Get<string>("folder");
            FileName = RequestHelper.Get<string>("file");

            /*
             * add by glg 2011-6-22
             * reson:粘贴样式时只传过来可复制的属性
             */
            if (string.Compare(Action, "paste", true) == 0 && !WidgetParameter.ContainsKey("id"))
            {
                WidgetParameter.Add("id", NextId);
            }
        }

        public string Format(string html)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionCheckSyntax = true;
            doc.OptionOutputOriginalCase = true;
            doc.LoadHtml(html);
            string head = string.Empty;
            string body = string.Empty;

            var headNode = doc.GetElementbyId(WarpHeadId);
            var warpNode = doc.GetElementbyId(WarpDivId);
            if (headNode != null)
            {
                doc.DocumentNode.SelectSingleNode("//title").Remove();
                head = headNode.InnerHtml.Trim();
            }
            if (warpNode != null)
            {
                body = warpNode.InnerHtml.Trim();
            }
            return head + body;

        }
        /// <summary>
        /// 分离head和control html
        /// </summary>
        /// <param name="html">生成的HTML</param>
        /// <returns></returns>
        private Dictionary<string, string> CreateAjaxData(string html)
        {
            HtmlDocument doc = new HtmlDocument();

            doc.OptionAutoCloseOnEnd = true;
            doc.OptionCheckSyntax = true;
            doc.OptionOutputOriginalCase = true;
            doc.LoadHtml(html);
            Dictionary<string, string> ajaxData = new Dictionary<string, string>();
            string head = string.Empty;
            string body = string.Empty;

            var headNode = doc.GetElementbyId(WarpHeadId);
            var warpNode = doc.GetElementbyId(WarpDivId);

            if (headNode != null)
            {
                headNode.SelectSingleNode("//title").RemoveAll();
                head = headNode.InnerHtml.Trim();
            }
            if (warpNode != null)
            {
                body = warpNode.InnerHtml.Trim();
            }

            if (!string.IsNullOrEmpty(head))
            {
                head = Base64Helper.Encode(head);
            }
            if (!string.IsNullOrEmpty(body))
            {
                body = Base64Helper.Encode(body);
            }

            ajaxData.Add("head", head);
            ajaxData.Add("body", body);
            return ajaxData;

        }

        private void SetTypeProperty(object o, string prefix, Dictionary<string, object> attrs)
        {
            //foreach (PropertyInfo prop in o.GetType().GetProperties())
            //{
            //    string name = !String.IsNullOrEmpty(prefix) ? String.Format("{0}-{1}", prefix.ToString(), prop.Name.ToLower()) : prop.Name.ToLower();

            //    if (Attribute.IsDefined(prop, typeof(ChildrenAttribute)) )
            //    {
            //        var temp=prop.GetValue(o, null);

            //        SetTypeProperty( temp, name, attrs);

            //      //  prop.SetValue(o, temp, null);

            //    }
            //    else if (Attribute.IsDefined(prop, typeof(OptionAttribute)) && prop.CanWrite && attrs.ContainsKey(name))
            //    {
            //            //TODO：：转换value值的类型
            //            object value = ConvertComplexObj(attrs[name].ToString(), prop.PropertyType);
            //            try
            //            {

            //                prop.SetValue(o, value, null);
            //            }
            //            catch
            //            {
            //                throw new Exception("设置属性" + prop.Name + "出错啦!");
            //            }
            //    }
            //}
            foreach (PropertyInfo prop in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                string name = !String.IsNullOrEmpty(prefix) ? String.Format("{0}-{1}", prefix, prop.Name) : prop.Name;

                if (Attribute.IsDefined(prop, typeof(ParameterAttribute)))
                {
                    //TODO：：转换value值的类型
                    if (attrs.ContainsKey(name.ToLower()))
                    {
                        object value = ConvertComplexObj(attrs[name.ToLower()].ToString(), prop.PropertyType);
                        try
                        {

                            prop.SetValue(o, value, null);
                        }
                        catch
                        {
                            throw new Exception("设置属性" + prop.Name + "出错啦!");
                        }
                    }
                }

                if (Attribute.IsDefined(prop, typeof(ChildrenAttribute)))
                {
                    object innerObject = prop.GetValue(o, null);
                    if (innerObject != null)
                    {
                        SetTypeProperty(innerObject, prop.Name, attrs);
                    }
                    //foreach (string key in attrs.Keys)
                    //{
                    //    string[] ss = key.Split('-');
                    //    if (ss.Length > 1)
                    //    {
                    //        if (ss[0].Equals(prop.Name, StringComparison.OrdinalIgnoreCase))
                    //        {
                    //            object innerObject = prop.GetValue(o, null);
                    //            MemberInfo[] mis=innerObject.GetType().GetMember(ss[1], BindingFlags.Instance|BindingFlags.Public);
                    //            if (mis != null && mis.Length > 0)
                    //            {
                    //                MemberInfo mi = mis[0];
                    //                if (mi.MemberType == MemberTypes.Field)
                    //                {
                    //                    FieldInfo fi = mi as FieldInfo;
                    //                    fi.SetValue(innerObject, ConvertComplexObj(attrs[key] as string, fi.FieldType));
                    //                }
                    //                if (mi.MemberType == MemberTypes.Property)
                    //                {
                    //                    PropertyInfo pi = mi as PropertyInfo;
                    //                    pi.SetValue(innerObject, ConvertComplexObj(attrs[key] as string, pi.PropertyType), null);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            foreach (FieldInfo prop in o.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
            {
                string name = !String.IsNullOrEmpty(prefix) ? String.Format("{0}-{1}", prefix, prop.Name) : prop.Name;

                if (Attribute.IsDefined(prop, typeof(ParameterAttribute)))
                {
                    //TODO：：转换value值的类型
                    if (attrs.ContainsKey(name.ToLower()))
                    {
                        object value = ConvertComplexObj(attrs[name.ToLower()].ToString(), prop.FieldType);
                        try
                        {

                            prop.SetValue(o, value);
                        }
                        catch
                        {
                            throw new Exception("设置字段的值" + prop.Name + "出错啦!");
                        }
                    }
                }

                if (Attribute.IsDefined(prop, typeof(ChildrenAttribute)))
                {
                    object innerObject = prop.GetValue(o);
                    if (innerObject != null)
                    {
                        SetTypeProperty(innerObject, prop.Name, attrs);
                    }
                    //foreach (string key in attrs.Keys)
                    //{
                    //    string[] ss = key.Split('-');
                    //    if (ss.Length > 1)
                    //    {
                    //        if (ss[0].Equals(prop.Name, StringComparison.OrdinalIgnoreCase))
                    //        {
                    //            object innerObject = prop.GetValue(o);
                    //            MemberInfo[] mis = innerObject.GetType().GetMember(ss[1], BindingFlags.Instance | BindingFlags.Public|BindingFlags.IgnoreCase|BindingFlags.Static);
                    //            if (mis != null && mis.Length > 0)
                    //            {
                    //                MemberInfo mi = mis[0];
                    //                if (mi.MemberType == MemberTypes.Field)
                    //                {
                    //                    FieldInfo fi = mi as FieldInfo;
                    //                    fi.SetValue(innerObject, ConvertComplexObj(attrs[key] as string, fi.FieldType));
                    //                }
                    //                if (mi.MemberType == MemberTypes.Property)
                    //                {
                    //                    PropertyInfo pi = mi as PropertyInfo;
                    //                    pi.SetValue(innerObject, ConvertComplexObj(attrs[key] as string, pi.PropertyType),null);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ConvertControlPathToStylePath(string path)
        {
            if (path.StartsWith("~/"))
                path = path.Replace("~/", "/");

            string controlRootPath = string.Empty;
            string style = Path.GetFileNameWithoutExtension(WidgetParameter["filename"].ToString());
            controlRootPath = Path.GetDirectoryName(path);

            return string.Format("{0}/Style/{1}.css", controlRootPath.Replace("\\", "/"), style);
        }

        /// <summary>
        /// 动态添加UserControl
        /// </summary>
        /// <param name="path">控件虚拟路径</param>
        /// <param name="attrs">控件属性</param>
        protected void AddUserControl(string path, Dictionary<string, object> attrs)
        {

            HtmlHead head = new HtmlHead();
            head.ID = WarpHeadId;
            Page.Controls.Add(head);

            //用于装载usercontrol
            Panel warpPanel = new Panel();
            warpPanel.ID = WarpDivId;

            this.Page.Controls.Add(warpPanel);

            //加载控件
            UserControl viewControl = (UserControl)Page.LoadControl(path);
            //把属性都转变为小写

            var tempAttributes = new Dictionary<string, object>();
            foreach (var item in attrs)
            {

                tempAttributes.Add(item.Key.ToLower(), item.Value.ToString().Replace("&amp;nbsp;", " "));
            }
            //设置属性
            if (attrs != null && tempAttributes.Count > 0)
            {
                viewControl.GetType().GetProperty("ID").SetValue(viewControl, tempAttributes["id"], null);

                SetTypeProperty(viewControl, null, tempAttributes);
            }

            warpPanel.Controls.Add(viewControl);
        }
        /// <summary>
        /// 转换复杂类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object ConvertComplexObj(string value, Type type)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            if (type == null)
            {
                return null;
            }
            if (type.IsArray)
            {
                Type typeEle = type.GetElementType();
                string[] strs = value.Split(new char[] { ';' });
                Array array = Array.CreateInstance(typeEle, value.Length);

                for (int i = 0, j = value.Length; i < j; ++i)
                {
                    array.SetValue(ConvertObj(value[i], typeEle), i);
                }

                return array;
            }

            return ConvertObj(value, type);
        }
        /// <summary>
        /// 转换简单类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object ConvertObj(object value, Type type)
        {
            object returnValue;
            if ((value == null) || type.IsInstanceOfType(value))
            {
                return value;
            }
            string str = value as string;
            if ((str != null) && (str.Length == 0))
            {
                return null;
            }
            System.ComponentModel.TypeConverter converter = TypeDescriptor.GetConverter(type);
            bool flag = converter.CanConvertFrom(value.GetType());
            if (!flag)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!flag && !converter.CanConvertTo(type))
            {
                throw new InvalidOperationException("无法转换成类型：" + value.ToString() + "==>" + type);
            }
            try
            {
                returnValue = flag ? converter.ConvertFrom(null, null, value) : converter.ConvertTo(null, null, value, type);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("类型转换出错：" + value.ToString() + "==>" + type, e);
            }
            return returnValue;
        }

        #endregion
    }
}
