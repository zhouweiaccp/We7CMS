using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using We7.CMS;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7;
using System.Text.RegularExpressions;
using We7.CMS.Common;
using We7.CMS.Config;
using System.Xml;
using We7.CMS.Module.VisualTemplate;
using System.IO;
using HtmlAgilityPack;
using We7.Framework.TemplateEnginer;
using We7.CMS.Accounts;
using We7.CMS.Common.PF;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 前台用户控件基类其他控件均继承与此
    /// 注意：请保持此基类的清洁，需要用到的助手定义请在继承类里注册
    /// </summary>
    [Serializable]
    public class BaseWebControl : UserControl, IPager
    {
        protected DesignHelper DesignHelper;

        #region 静态化设计添加
        private bool _isHtml = true;
        /// <summary>
        /// 是否生成静态
        /// </summary>
        public virtual bool IsHtml
        {
            get { return _isHtml; }
            set { _isHtml = value; }
        }

        /// <summary>
        /// 是否是正在生成静态
        /// </summary>
        public bool CreateHtml
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["CreateHtml"]) && Request["CreateHtml"] == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 模板子分类，如登录、注册、搜索等
        /// </summary>
        protected string ColumnMode
        {
            get
            {
                if (Request["mode"] != null)
                    return Request["mode"].ToString();
                else
                    return "";
            }
        }

        private string TemplatePath;
        /// <summary>
        /// 模板业务对象
        /// </summary>
        protected TemplateHelper TemplateHelper
        {
            get { return HelperFactory.GetHelper<TemplateHelper>(); }
        }
        /// <summary>
        /// 获取控件的原始字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string GetOriginalControl()
        {
            string path = Server.MapPath(TemplatePath);

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

            var node = doc.GetElementbyId(this.ID);
            if (node != null)
            {
                return node.OuterHtml;
            }
            return "";
        }

        private void InitTemplatePath()
        {
            string result = TemplateHelper.GetTemplateByHandlers(ColumnMode, "/", null, null);
            if (!string.IsNullOrEmpty(result))
            {
                if (!result.StartsWith("/"))
                {
                    TemplatePath = "/" + result;
                }
                else
                {
                    TemplatePath = result;
                }
            }
        }

        #endregion


        public BaseWebControl()
            : base()
        {
            DesignHelper = new DesignHelper(this);
            //默认设计状态来自Request["design"]
            string design = Context.Request["state"];
            //VisualDesign = design != null;

        }

        #region 业务助手
        /// <summary>
        /// 业务助手工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get
            {
                return HelperFactory.Instance;
            }
        }

        /// <summary>
        /// 用户管理业务助手
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// 内容管理业务助手
        /// </summary>
        protected SiteSettingHelper CDHelper
        {
            get { return HelperFactory.GetHelper<SiteSettingHelper>(); }
        }

        /// <summary>
        /// 栏目类业务助手
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        #endregion

        #region 显示字段
        /// <summary>
        /// 要显示的字段
        /// </summary>
        public string ShowFields { get; set; }
        /// <summary>
        ///　排序字段
        /// </summary>
        public string OrderFields { get; set; }

        List<string> fields;
        /// <summary>
        /// 列表数据字段数组（将显示的字段构造于此）
        /// </summary>
        public List<string> Fields
        {
            get
            {
                if (fields == null)
                {
                    fields = new List<string>();
                }
                return fields;
            }
            set { fields = value; }
        }

        /// <summary>
        /// 构造列表数据字段数组，此过程在Initialize()过程中被自动调用
        /// </summary>
        protected virtual void ConstructFelds()
        {
            if (!String.IsNullOrEmpty(ShowFields))
            {
                //构造显示的数据字段
                string[] tmpFields = ShowFields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in tmpFields)
                {
                    if (!Fields.Contains(item))
                        Fields.Add(item);
                }
            }
        }


        /// <summary>
        /// 是否显示字段
        /// </summary>
        /// <param name="key">显示的字段</param>
        /// <returns>当前字符是否显示</returns>
        public bool ShowField(string key)
        {
            if (!String.IsNullOrEmpty(ShowFields))
            {
                Regex regex = new Regex(@"\b" + key + @"\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                return regex.IsMatch(ShowFields);
            }
            return false;
        }

        /// <summary>
        /// 判断是否显示字段
        /// </summary>
        /// <param name="key">字段名</param>
        /// <returns>是否显示</returns>
        public bool Show(string key)
        {
            return ShowField(key);
        }
        #endregion

        #region 初始化事件
        /// <summary>
        /// 代码手工赋值控件属性值，此过程在Initialize()过程中被自动调用
        /// </summary>
        protected virtual void SetAttribute()
        { }

        /// <summary>
        /// 加载事件重写
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            RegisterScript();
            CreateActionID();
            base.OnLoad(e);
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        protected void RegisterScript()
        {
        }

        /// <summary>
        /// 自定义控件初始化过程（已做过IsPostBack判断）
        /// </summary>
        protected virtual void Initialize()
        {
            //InitTemplatePath();
            if (!string.IsNullOrEmpty(ShowFields))
                ConstructFelds();
        }

        #endregion

        #region 属性变量
        /// <summary>
        /// 获取用户ID
        /// </summary>
        protected virtual string AccountID
        {
            get { return Security.CurrentAccountID; }
        }
        /// <summary>
        /// 检测用户是否登录
        /// </summary>
        protected bool IsSignin
        {
            get { return Security.IsAuthenticated(); }
        }
        /// <summary>
        /// 是否显示消息(已经加了Display)
        /// </summary>
        public string MessageDisplay
        {
            get
            {
                string message = (Get("Message") ?? "").ToString();
                return String.IsNullOrEmpty(message) ? "display:none" : "";
            }
        }

        /// <summary>
        /// 模板组名称
        /// </summary>
        public string TemplateGroup
        {
            get
            {
                string path = Request["template"];
                if (!String.IsNullOrEmpty(path))
                {
                    Regex regex = new Regex("(?<=_skins/).*?(?=/)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Match mc = regex.Match(path);
                    path = mc.Value;
                }
                return path;
            }
        }

        /// <summary>
        /// 首页Url地址：/home.html或/home.aspx
        /// </summary>
        public string HomeURL
        {
            get
            {
                GeneralConfigInfo gci = GeneralConfigs.GetConfig();
                if (gci != null && gci.UrlExtName == ".html")
                    return "/home.html";
                else
                    return "/home.aspx";
            }
        }

        /// <summary>
        /// 当前控件的关联值
        /// </summary>
        public virtual string RelationValue
        {
            get { throw new Exception("没有重写RelationValue"); }
        }

        #endregion

        #region 通用函数/方法
        /// <summary>
        ///  JS文件注册；可以引用多个文件，如：
        ///  IncludeJavaScript("jquery.bgiframe.js", "jquery.dimensions.js",
        ///  "jquery.jdMenu.js", "SlideMenuReady.js");
        /// </summary>
        /// <param name="files"></param>
        public void IncludeJavaScript(params string[] files)
        {
            List<string> paths = new List<string>();
            if (String.IsNullOrEmpty(Request["state"]))
            {
                paths.Add("/Scripts/jquery/jquery-1.4.2.js");
            }
            foreach (string file in files)
            {
                if (file.StartsWith("/"))
                {
                    paths.Add(file);
                }
                else
                {
                    paths.Add(this.TemplateSourceDirectory + "/js/" + file);
                }
            }
            JavaScriptManager.Include(paths.ToArray());
        }

        public string GetTagThumbnail(Article article, string tag)
        {
            if (DesignHelper.IsDesigning)
                return DesignHelper.GetTagThumbnail(tag);
            else
            {
                if (!String.IsNullOrEmpty(article.Thumbnail))
                {
                    return article != null ? article.GetTagThumbnail(tag) : String.Empty;
                }
            }
            return String.Empty;
        }

        #endregion

        #region form Action 提交处理
        /// <summary>
        /// 取得Action传回来的值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>字段的数据值</returns>
        public object Get(string key)
        {
            return BaseAction.Get(key, ActionID);
        }

        /// <summary>
        /// 取得Action传回来的值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">字段名</param>
        /// <returns>字段的数据值</returns>
        public T Get<T>(string key)
        {
            try
            {
                return (T)BaseAction.Get(key, ActionID);
            }
            catch
            {
            }
            return default(T);
        }

        private string actionID;
        /// <summary>
        /// 当前的ActionID
        /// </summary>
        public string ActionID
        {
            set { actionID = value; }
            get
            {
                if (String.IsNullOrEmpty(actionID))
                {
                    actionID = this.ClientID;
                }
                return actionID;
            }
        }

        private int minActionID = 0;
        /// <summary>
        /// 创建新的ActionID
        /// </summary>
        public string CreateActionID()
        {
            ActionID = this.ClientID + minActionID;
            minActionID++;
            return ActionID;
        }

        /// <summary>
        /// 取得当前的Action
        /// </summary>
        /// <typeparam name="T">Action类型</typeparam>
        /// <returns>Action对象</returns>
        public T GetAction<T>()
        {
            return BaseAction.GetAction<T>(ActionID);
        }

        private HtmlHelper htmlHelper;
        /// <summary>
        /// 前台Html控件帮助类
        /// </summary>
        protected HtmlHelper HtmlHelper
        {
            get { return htmlHelper ?? (htmlHelper = new HtmlHelper(ref actionID, CreateActionID)); }
        }

        private HtmlHelper2 htmlHelper2;
        protected HtmlHelper2 Html
        {
            get { return htmlHelper2 ?? (htmlHelper2 = new HtmlHelper2(this)); }
        }


        #endregion

        #region 分页参数
        public bool Disable { get; set; }

        /// <summary>
        /// 禁止分页
        /// </summary>
        public virtual bool DisablePager
        {
            get { return Disable; }
            set { Disable = value; }
        }

        private int pageIndex = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public virtual int PageIndex
        {
            get
            {
                if (!DisablePager)
                {
                    try
                    {
                        string sp = Request != null ? Request["PageIndex"] : "0";
                        pageIndex = String.IsNullOrEmpty(sp) ? 1 : Convert.ToInt32(sp);
                    }
                    catch { pageIndex = 1; }
                }
                else
                {
                    pageIndex = 1;
                }
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        /// <summary>
        /// 记录总条数
        /// </summary>
        public virtual int RecordCount { get; set; }

        private int pageCount;
        /// <summary>
        /// 总页数
        /// </summary>
        public virtual int PageCount
        {
            get
            {
                pageCount = RecordCount / PageSize;
                pageCount = RecordCount % PageSize == 0 ? pageCount : (pageCount + 1);
                return pageCount;
            }
            set
            {
                pageCount = value;
            }
        }

        private int pageSize = 10;
        /// <summary>
        /// 每页条数
        /// </summary>
        public virtual int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        /// <summary>
        ///　分页开始的条目
        /// </summary>
        public int StartItem
        {
            get
            {
                int start = (PageIndex - 1) * PageSize;
                return start < 0 ? 0 : start;
            }
        }

        /// <summary>
        ///  分页结束的条目
        /// </summary>
        public int EndItem
        {
            get
            {
                int end = PageIndex * PageSize - 1;
                return end >= RecordCount ? (RecordCount - 1) : end;
            }
        }

        /// <summary>
        /// 当前页面的记录数
        /// </summary>
        public int PageItemsCount
        {
            get
            {
                int count = EndItem - StartItem + 1;
                return count > RecordCount ? 0 : count;
            }
        }
        #endregion

        #region 数据源

        /// <summary>
        /// 数据源ID
        /// </summary>
        public string DataSourceID { get; set; }

        /// <summary>
        /// 数据实体
        /// </summary>
        public Object Data { get; set; }

        /// <summary>
        /// 取得数据源中的数据.
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <returns>数据对象</returns>
        public T GetDataSourceData<T>()
        {
            T t = default(T);
            Control c = Parent.FindControl(DataSourceID);
            if (c == null || !(c is IDataSource))
                throw new Exception("所需数据源控件不存在");
            IDataSource DSControl = c as IDataSource;
            try
            {
                t = (T)(DSControl.Data);
            }
            catch (Exception ex)
            {
                throw new Exception("从数据源" + DataSourceID + "中读取数据出错:" + ex.Message);
            }
            return t;
        }

        #endregion

        #region 获取数据的辅助方法

        public string GetXmlConst(object value, string field, string type)
        {
            return GetXmlConst(value as string, field, type);
        }

        public string GetXmlConst(string value, string field, string type)
        {
            return !String.IsNullOrEmpty(value) && value.Contains(field) ? GetXmlFieldConst(field, type) : "";
        }

        public string ContainsValue(string value, string field, string y, string n)
        {
            y = String.IsNullOrEmpty(y) ? "是" : y;
            n = String.IsNullOrEmpty(n) ? "否" : n;
            return !String.IsNullOrEmpty(value) && value.Contains(field) ? y : n;
        }

        public string ContainsValue(string value, string field)
        {
            return ContainsValue(value, field, "", "");
        }

        public string ContainsValue(object value, string type, string y, string n)
        {
            return ContainsValue(value as string, type, y, n);
        }

        public string ContainsValue(object value, string type)
        {
            return ContainsValue(value as string, type);
        }

        public string DateFormat(object obj, string fmt)
        {
            return obj != null && obj != DBNull.Value ? ((DateTime)obj).ToString(fmt) : "";
        }

        public string DateFormat(object obj)
        {
            return obj != null && obj != DBNull.Value ? ((DateTime)obj).ToString("yyyy-MM-dd") : "";
        }

        public string GetXmlFieldConst(string field, string type)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/Models/Inc/Const.xml"));
            XmlElement xe = doc.DocumentElement.SelectSingleNode(type) as XmlElement;
            string txt = xe.InnerText.Trim();
            string[] strs = txt.Split(',');
            foreach (string s in strs)
            {
                string[] ss = s.Split('|');
                if (ss[0] == field)
                {
                    return ss.Length > 1 ? ss[1] : ss[0];
                }
            }
            return "";
        }
        #endregion


        #region DesignHelper

        public virtual string FileName
        {
            get
            {
                var file = Context.Request["file"];
                return file;
            }
        }

        public virtual string Group
        {
            get
            {
                var folder = Context.Request["folder"];
                return folder;
            }
        }
        /// <summary>
        /// 获取属性JSON字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string AttributesJsonData()
        {
            StringBuilder sb = new StringBuilder();

            string path = Server.MapPath(string.Format("~/_skins/{0}/{1}", Group, FileName));

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

            var node = doc.GetElementbyId(this.ID);
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
                        sb.AppendFormat("{0}:\"{1}\",", attr.Name, attr.Value);
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("}");
                sb.Append("}");
                sb.Append("}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 可视化设计时
        /// </summary>
        public bool VisualDesign { get; set; }


        /// <summary>
        /// 头部控件类型
        /// </summary>
        public string VHeadType { get; set; }

        /// <summary>
        /// 重写Render方法
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (VisualDesign)//在可视化设计时生成辅助标签
            {
                //可视化设计时
                StringWriter output = new StringWriter();
                HtmlTextWriter tw = new HtmlTextWriter(output);
                try
                {
                    base.Render(tw);
                }
                catch (Exception ex)
                {
                    tw.Write("<div class=\"vdControlError\">");
                    tw.Write(ex.Message);
                    tw.Write("</div>");
                };

                string ControlHtml = output.ToString();
                //格式化代码
                string formatControlHtml = FormatHtml(ControlHtml);

                NVelocityHelper helper = new NVelocityHelper(We7.CMS.Constants.VisualTemplatePhysicalTemplateDirectory);

                helper.Put("controlId", this.ID);
                helper.Put("controlName", "");
                helper.Put("controlContent", formatControlHtml);
                helper.Put("controlData", AttributesJsonData());

                var rendHtml = helper.Save("We7ControlDesign.vm");

                //格式化
                rendHtml = FormatHtml(rendHtml);
                //输出代码
                writer.Write(rendHtml);
            }
            else
            {
                if (CreateHtml)//生成静态过程中
                {
                    if (IsHtml)//需要生成静态
                    {
                        StringWriter strWriter = new StringWriter();
                        HtmlTextWriter tempWriter = new HtmlTextWriter(strWriter);
                        try
                        {
                            base.Render(tempWriter);
                        }
                        catch (Exception ex)
                        {
                            strWriter.Write("");
                        };
                        string content = strWriter.ToString();
                        //格式化代码
                        string formatControlHtml = FormatHtml(content);
                        //输出html代码
                        writer.Write(formatControlHtml);
                    }
                    else
                    {
                        writer.Write(GetOriginalControl());
                    }
                }
                else
                {
                    base.Render(writer);
                }
            }

            //Response.Write("<br />当前控件执行时间：：" + DateTime.Now.Subtract(processStartTime).TotalMilliseconds / 1000);

        }

        ///// <summary>
        ///// 当前页面开始载入时间(毫秒)
        ///// </summary>
        //private DateTime processStartTime;

        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    processStartTime = DateTime.Now;
        //}

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

        #endregion

        #region 获取数据

        /// <summary>
        /// 将指定值转化为字符串
        /// </summary>
        /// <param name="fieldValue">字段值</param>
        /// <returns></returns>
        protected string ToStr(object fieldValue)
        {
            return fieldValue != null ? fieldValue.ToString() : String.Empty;
        }

        /// <summary>
        /// 将指定值转化为字符串
        /// </summary>
        /// <param name="fieldValue">字段值</param>
        /// <param name="maxlength">值的最大长度</param>
        /// <returns></returns>
        protected string ToStr(object fieldValue, int maxlength)
        {
            return ToStr(fieldValue, maxlength, "...");
        }

        /// <summary>
        /// 将指定值转化为字符串
        /// </summary>
        /// <param name="fieldValue">字段值</param>
        /// <param name="maxlength">值的最大长度</param>
        /// <param name="tail">超过指定长度的结尾字符</param>
        /// <returns></returns>
        protected string ToStr(object fieldValue, int maxlength, string tail)
        {
            string str = ToStr(fieldValue);
            return Utils.GetUnicodeSubString(str, maxlength, tail);
        }

        /// <summary>
        /// 将时间数据按指定格式转化
        /// </summary>
        /// <param name="fieldValue">时间值</param>
        /// <param name="fmt">日期格式</param>
        /// <returns></returns>
        protected string ToDateStr(object fieldValue, string fmt)
        {
            if (fieldValue != null && fieldValue is DateTime)
            {
                return Convert.ToDateTime(fieldValue).ToString(fmt);
            }
            return String.Empty;
        }

        /// <summary>
        /// 将时间数据按指定格式转化
        /// </summary>
        /// <param name="fieldValue">时间值</param>
        /// <returns></returns>
        protected string ToDateStr(object fieldValue)
        {
            return ToDateStr(fieldValue, "yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 将数据转化为字整数
        /// </summary>
        /// <param name="fieldValue">要转化的值</param>
        /// <returns></returns>
        protected int? ToInt(object fieldValue)
        {
            if (fieldValue != null && fieldValue is int)
            {
                return Convert.ToInt32(fieldValue);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 取得部门名称
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        protected string GetDepartName(object fieldValue)
        {
            if (fieldValue != null)
            {
                IAccountHelper helper = AccountFactory.CreateInstance();
                Department depart = helper.GetDepartment(fieldValue.ToString(), new string[] { "Name" });
                return depart != null ? depart.Name : String.Empty;
            }
            return String.Empty;
        }

        #endregion

        public override void RenderControl(HtmlTextWriter writer)
        {
            Html.Writer = writer;
            base.RenderControl(writer);
        }
    }
}
