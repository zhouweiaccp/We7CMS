using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

using We7.CMS.Config;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Config;
using We7.CMS.Accounts;
using System.IO;
using System.Web.UI.HtmlControls;
using Thinkment.Data;
using We7.Framework.Util;
using System.Text.RegularExpressions;
using System.Reflection;

namespace We7.CMS
{
    /// <summary>
    /// 前台页面基础类
    /// </summary>
    public class FrontBasePage : Page,IDataAccessPage
    {

        /// <summary>
        /// 模板路径
        /// </summary>
        protected virtual string TemplatePath
        {
            get;
            set;
        }

        #region load time
        /// <summary>
        /// 构建类
        /// </summary>
        public FrontBasePage()
        {
            processStartTime = DateTime.Now;
        }

        /// <summary>
        /// 当前页面执行时间(毫秒)
        /// </summary>
        private double processTimeSpan;

        /// <summary>
        /// 得到当前页面的载入时间供模板中调用(单位:毫秒)
        /// </summary>
        /// <returns>当前页面的载入时间</returns>
        public double ProcessTimeSpan
        {
            get { return processTimeSpan; }
        }

        /// <summary>
        /// 当前页面开始载入时间(毫秒)
        /// </summary>
        private DateTime processStartTime;

        /// <summary>
        /// 本页所属栏目
        /// </summary>
        protected Channel ThisChannel
        {
            get
            {
                HttpContext Context = HttpContext.Current;
                string key = "thisChannel";
                Channel ch = Context.Items[key] as Channel;
                if (ch != null)
                    return ch;
                else
                {
                    string columnID = ChannelHelper.GetChannelIDFromURL();

                    //初始化ThisChannel
                    if (!We7Helper.IsEmptyID(columnID))
                    {
                        ch = ChannelHelper.GetChannel(columnID, null);
                        Context.Items.Remove(key);
                        Context.Items.Add(key, ch);
                        return ch;
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// 内容模型
        /// </summary>
        public string ModelName
        {
            get
            {
                if (ThisChannel != null)
                    return ThisChannel.ModelName;
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// 模板制作向导URL
        /// </summary>
        public string TemplateGuideUrl
        {
            get
            {
                if (GeneralConfigs.GetConfig().StartTemplateMap)
                    return string.Format("/go/TemplateGuide.aspx?handler={0}&mode={1}&model={2}", GoHandler, ColumnMode, ModelName);
                else
                    return "/Errors.aspx?t=notemplate";
            }
        }

        #endregion

        #region helpers

        /// <summary>
        /// 权限业务对象
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        /// <summary>
        /// 模板业务对象
        /// </summary>
        protected TemplateHelper TemplateHelper
        {
            get { return HelperFactory.GetHelper<TemplateHelper>(); }
        }

        /// <summary>
        /// 栏目业务对象
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// 文章业务对象
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        /// <summary>
        /// 反馈业务对象
        /// </summary>
        protected AdviceHelper AdviceHelper
        {
            get { return HelperFactory.GetHelper<AdviceHelper>(); }
        }

        /// <summary>
        /// 网站基本信息业务对象
        /// </summary>
        protected SiteSettingHelper CDHelper
        {
            get { return HelperFactory.GetHelper<SiteSettingHelper>(); }
        }

        /// <summary>
        /// 统计业务对象
        /// </summary>
        protected StatisticsHelper StatisticsHelper
        {
            get { return HelperFactory.GetHelper<StatisticsHelper>(); }
        }

        /// <summary>
        /// 页面访问业务对象
        /// </summary>
        protected PageVisitorHelper PageVisitorHelper
        {
            get { return HelperFactory.GetHelper<PageVisitorHelper>(); }
        }

        /// <summary>
        /// IP安全业务对象
        /// </summary>
        protected IPSecurityHelper IPSecurityHelper
        {
            get { return HelperFactory.GetHelper<IPSecurityHelper>(); }
        }
        #endregion

        protected virtual string GoHandler { get { return ""; } }

        protected virtual string ColumnMode { get { return ""; } }

        protected bool IsHtmlTemplate
        {
            get
            {
                return GeneralConfigs.GetConfig().EnableHtmlTemplate && String.IsNullOrEmpty(Request["CreateHtml"]);
            }
        }

        /// <summary>
        /// 前台页面载入
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Response.Expires = -1;
                base.OnLoad(e);

                if (!BaseConfigs.ConfigFileExist())
                {
                    Response.Write("您的数据库配置文件尚未生成，看起来数据库尚未建立，您需要建立数据库配置文件或生成数据库。现在开始吗？<a href='/install/index.aspx'><u>现在配置数据库</u></a>");
                    return;
                }

                if (UnLoginCheck())
                {
                    Response.Redirect("/login.aspx?returnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl),false);
                    return;
                }

                if (!CheckIPStrategy())
                {
                    Response.Write("IP受限，您的IP没有在受访范围内！");                    
                    return;
                }

                if (!CheckPermission())
                {
                    Response.Write("您没有权限访问此栏目！");                    
                    return;
                }

                Initialize();
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(FrontBasePage), ex);
                DisplayError(ex.Message);
            }
        }

        /// <summary>
        /// 初始化信息
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// 返回许可权限串
        /// </summary>
        protected virtual string[] Permissions
        {
            get
            {
                if (ThisChannel != null)
                {
                    if (ThisChannel.SecurityLevel > 0)
                    {
                        return new string[] { "Channel.Read" };
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 授权对象ID：如栏目或模块ID
        /// </summary>
        protected virtual string PermissionObjectID
        {
            get
            {
                if (ThisChannel != null)
                {
                    if (ThisChannel.SecurityLevel > 0)
                    {
                        if (string.IsNullOrEmpty(AccountID))
                        {
                            Response.Redirect("/login.aspx?ReturnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl),false);
                        }
                        else
                            return ThisChannel.ID;
                    }
                }
                return We7Helper.EmptyGUID;
            }
        }

        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        protected virtual string AccountID
        {
            get
            {
                return Security.CurrentAccountID;
            }
        }

        /// <summary>
        /// 检查用户是否有权限访问本页
        /// </summary>
        protected virtual bool CheckPermission()
        {
            if (Permissions == null || AccountID == We7Helper.EmptyGUID)
            {
                return true;
            }

            // 依次检查所有的权限
            // TODO: 增加一个权限缓存，不必每次都去访问数据库来获取权限信息
            List<string> ps = AccountHelper.GetPermissionContents(AccountID, PermissionObjectID);
            foreach (string r in Permissions)
            {
                foreach (string p in ps)
                {
                    if (p == r)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 无访问权限跳转
        /// </summary>
        protected virtual void HanldeNoPermission()
        {
            Server.Transfer("/Errors.aspx?t=permission");
        }

        /// <summary>
        /// 获取当前页标题设置参数
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public string GetCurrentPageTitle(string channelID, string articleID)
        {
            string titleFormart = CDHelper.GetDefaultHomePageTitle();

            if (articleID != "" && channelID != "")//内容页
            {
                titleFormart = CDHelper.GetDefaultContentPageTitle();
            }
            else if (channelID != "") //栏目页
            {
                titleFormart = CDHelper.GetDefaultChannelPageTitle();
            }
            return ParselFormatTitle(titleFormart, channelID, articleID);
        }

        /// <summary>
        /// 格式化标题
        /// </summary>
        /// <param name="titleFormat">标题格式</param>
        /// <param name="channelID">栏目ID</param>
        /// <param name="articleID">文章ID</param>
        /// <returns></returns>
        private string ParselFormatTitle(string titleFormat, string channelID, string articleID)
        {
            string channelParam = "{$ChannelName}";
            string articleParam = "{$ArticleTitle}";

            if (titleFormat.IndexOf(channelParam) > -1)
            {
                string chName = ChannelHelper.GetChannelName(channelID);
                titleFormat = titleFormat.Replace(channelParam, chName);
            }
            if (titleFormat.IndexOf(articleParam) > -1 && articleID != "")
            {
                string title = "";
                try
                {
                    Article ar = ArticleHelper.GetArticle(articleID, null);
                    if (ar != null)
                    {
                        if (ar.Title != null)
                        {
                            title = ar.Title;
                        }
                        else
                        {
                            title = "详细页";
                        }
                    }
                    else
                    {
                        title = "详细页";
                    }
                }
                catch (Exception ex)
                {
                    We7.Framework.LogHelper.WriteLog(typeof(FrontBasePage), ex);
                }

                titleFormat = titleFormat.Replace(articleParam, title);
            }

            return titleFormat;

        }

        /// <summary>
        /// 检测IP策略
        /// </summary>
        /// <returns></returns>
        bool CheckIPStrategy()
        {
            string ip = Context.Request.ServerVariables["REMOTE_ADDR"];
            string articleID = ArticleHelper.GetArticleIDFromURL();
            string channelID = string.Empty;
            if (ThisChannel != null) channelID = ThisChannel.ID;
            return IPSecurityHelper.CheckIPStrategy(ip, channelID, articleID);
        }

        /// <summary>
        /// 未登录检测
        /// </summary>
        /// <returns></returns>
        protected bool UnLoginCheck()
        {
            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            return ci.OnlyLoginUserCanVisit && !Security.IsAuthenticated();

        }

        /// <summary>
        /// 增加一个点击统计
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="columnID"></param>
        protected void AddStatistic(string articleID, string columnID)
        {
            if (GeneralConfigs.GetConfig().StartPageViewModule)
            {
                PageVisitorHandler handler = new PageVisitorHandler();
                PageVisitor pv;
                if (Session[PageVisitorHelper.PageVisitorSessionKey] == null)
                {
                    pv = PageVisitorHelper.AddPageVisitor(AccountID);
                    Session[PageVisitorHelper.PageVisitorSessionKey] = pv;
                    handler.AddVisitor();
                }
                else
                    pv = (PageVisitor)Session[PageVisitorHelper.PageVisitorSessionKey];

                if (pv != null)
                {
                    StatisticsHelper.AddStatistics(pv, articleID, columnID);
                    handler.AddPageVisit();
                    TimeSpan ts = DateTime.Now - pv.OnlineTime;
                    if (ts.TotalSeconds > 10)//过10秒刷新在线时间
                    {
                        pv.OnlineTime = DateTime.Now;
                        PageVisitorHelper.UpdatePageVisitor(pv, new string[] { "OnlineTime" });
                    }
                }
                else
                    Session[PageVisitorHelper.PageVisitorSessionKey] = null;
            }
        }

        class PageVisitorHandler
        {
            VisiteCount vc = AppCtx.Cache.RetrieveObject<VisiteCount>(We7.CMS.PageVisitorHelper.VisiteCountCacheKey);
            public void AddVisitor()
            {
                if (vc != null)
                {
                    vc.DayVisitors = vc.DayVisitors + 1;
                    vc.MonthVisitors = vc.MonthVisitors + 1;
                    vc.OnlineVisitors = vc.OnlineVisitors + 1;
                    vc.TotalVisitors = vc.TotalVisitors + 1;
                    vc.YearVisitors = vc.YearVisitors + 1;
                    vc.AverageDayVisitors = vc.TotalVisitors / ((DateTime.Now - vc.StartDate).Days + 1);
                }
            }

            public void AddPageVisit()
            {
                if (vc != null)
                {
                    vc.DayPageview = vc.DayPageview + 1;
                    vc.MonthPageview = vc.MonthPageview + 1;
                    vc.TotalPageView = vc.TotalPageView + 1;
                    vc.YearPageview = vc.YearPageview + 1;
                    vc.AverageDayPageview = vc.TotalPageView / ((DateTime.Now - vc.StartDate).Days + 1);
                }
            }
        }

        /// <summary>
        /// 添加一个js文件引用到Header
        /// </summary>
        /// <param name="src"></param>
        protected void AddJavascriptFile2Header(string src)
        {
            HtmlGenericControl scriptElement = new HtmlGenericControl("script");
            scriptElement.Attributes["src"] = src;
            scriptElement.Attributes["type"] = "text/javascript";
            this.Header.Controls.Add(scriptElement);
        }


        //protected override void Render(HtmlTextWriter writer)
        //{
        //    base.Render(writer);
        //    Response.Write("<br />" + DateTime.Now.Subtract(processStartTime).TotalMilliseconds / 1000);
        //}

        /// <summary>
        /// 输出错误
        /// </summary>
        /// <param name="error"></param>
        protected void DisplayError(string error)
        {
            string ErrMsg = error;
            Response.Write(ErrMsg);
            Response.End();
        }

        #region 过滤错误部件

        #region 正则匹配字符串
        string ALlRegisterPattetns = "<%@[\\s]*?Register[\\s]*?Src=\"(?<Src>[\\s|\\S]*?)\"[\\s|\\S]*?TagName=\"(?<TagName>[\\s|\\S]*?)\"[\\s|\\S]*?TagPrefix=\"(?<TagPrefix>[\\s|\\S]*?)\"[\\s]*?%>";   //所有的注册信息
        string ContentPattentn = "<{0}:{1}(?<paramet>[\\s|\\S]*?filename=\"{2}\"[\\s|\\S]*?)>[\\s]*?</{0}:{1}>"; //指定部件信息实例信息
        string RegisterPattetn = "<%@[\\s]*?Register[\\s]*?Src=\"{0}\"[\\s|\\S]*?TagName=\"{1}\"[\\s|\\S]*?TagPrefix=\"{2}\"[\\s]*?%>"; //指定部件注册信息 
        #endregion

        /// <summary>
        /// 出错部件字典
        /// </summary>
        private Dictionary<string, templetaInfo> ErrorDic = new Dictionary<string, templetaInfo>();
        public Control CheckControlByBuilder()
        {
            if (IsHtmlTemplate)
            {
                return LoadControl(TemplatePath);
            }
            Control ctl = null;

            string templateHtml = string.Empty;
            if (AppCtx.Cache.RetrieveObject(TemplatePath) != null)  //走缓存
            {
                templateHtml = AppCtx.Cache.RetrieveObject<string>(TemplatePath);
            }
            else
            {
                templateHtml = FileHelper.ReadFileWithLine(Context.Server.MapPath(TemplatePath), Encoding.UTF8);  //读取文件
                AppCtx.Cache.AddObjectWithFileChange(TemplatePath, templateHtml, new string[] { Context.Server.MapPath(TemplatePath) });
            }

            MatchCollection mc = Regex.Matches(templateHtml, ALlRegisterPattetns);//获取所有注册信息

            Dictionary<string, templetaInfo> dic = InstanceTempletaInfo(templateHtml, mc);

            ctl = InstanceControl(ctl, templateHtml, dic);

            return ctl;
        }

        /// <summary>
        /// 实例化控件
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="templateHtml"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        private Control InstanceControl(Control ctl, string templateHtml, Dictionary<string, templetaInfo> dic)
        {
            if (dic != null && dic.Count != 0)
            {
                foreach (var item in dic)
                {
                    //System.Diagnostics.Stopwatch swtime = new System.Diagnostics.Stopwatch();
                    //swtime.Start();

                    #region 处理过程
                    try
                    {
                        //LoadControl(LoadControl(item.Key).GetType(), item.Value.Parameter);
                        Type t = LoadControl(item.Key).GetType();
                        object instance = t.Assembly.CreateInstance(t.FullName);
                        foreach (var field in item.Value.Parameter)  //遍历字段赋值
                        {
                            FieldInfo fieldinfo = t.GetField(field.Key, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.SetField);  //取得字段信息
                            if (fieldinfo != null)
                            {
                                object c = Convert.ChangeType(field.Value, fieldinfo.FieldType); //动态转换类型
                                fieldinfo.SetValue(instance, c);    //赋值
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        item.Value.Error = ex.Message;//错误消息

                        ErrorDic.Add(item.Key, item.Value);  //添加错误部件字典
                        We7.Framework.LogHelper.WriteLog(this.GetType(), ex);
                    }
                    #endregion

                    //swtime.Stop();
                    //We7.Framework.LogHelper.WriteFileLog("showpageTimeTest.txt", "部件加载时间测试", "部件路径：" + item.Key + "\n部件运行加载时间：" + swtime.ElapsedMilliseconds.ToString() + "毫秒");
                }
                if (ErrorDic == null || ErrorDic.Count == 0)  //如果没有发生错误,则走正常流程
                {
                    ctl = LoadControl(TemplatePath);
                }
                else  //错误处理流程
                {
                    string ErroAscx = string.Empty;
                    foreach (var item in ErrorDic)
                    {
                        ErroAscx = Regex.Replace(templateHtml, ContentPattentn.Replace("{0}", item.Value.Wew).Replace("{1}", item.Value.Tagname).Replace("{2}", item.Value.Src), "<span style='color:Red' title='" + item.Value.Error + "'>此部件发生错误</span>");  //显示错误信息
                        ErroAscx = Regex.Replace(ErroAscx, RegisterPattetn.Replace("{0}", item.Value.Src).Replace("{1}", item.Value.Tagname).Replace("{2}", item.Value.Wew), string.Empty);  //去掉错误部件注册信息
                    }

                    string errorPath = TemplatePath.Insert(TemplatePath.LastIndexOf('.'), ".error"); ;//错误模板副本路径
                    if (AppCtx.Cache.RetrieveObject<string>(errorPath) != null && AppCtx.Cache.RetrieveObject<string>(errorPath).Equals(ErroAscx)) //如果有缓存
                    {
                        ctl = LoadControl(errorPath);
                    }
                    else
                    {
                        FileHelper.WriteFileEx(Context.Server.MapPath(errorPath), ErroAscx, false);  //写错误模板副本
                        AppCtx.Cache.AddObjectWithFileChange(errorPath, ErroAscx, new string[] { Context.Server.MapPath(errorPath) });  //添加缓存
                        ctl = LoadControl(errorPath);
                    }

                }
            }
            return ctl;
        }

        #region 部件实体信息实例化
        /// <summary>
        /// 实例化部件实体信息
        /// </summary>
        /// <param name="templateHtml"></param>
        /// <param name="mc"></param>
        /// <returns></returns>
        private Dictionary<string, templetaInfo> InstanceTempletaInfo(string templateHtml, MatchCollection mc)
        {
            Dictionary<string, templetaInfo> dic = new Dictionary<string, templetaInfo>();   //部件信息
            foreach (Match item in mc) //部件实体实例化
            {
                MatchCollection contentMc = Regex.Matches(templateHtml, ContentPattentn.Replace("{0}", item.Groups["TagPrefix"].Value).Replace("{1}", item.Groups["TagName"].Value).Replace("{2}", item.Groups["Src"].Value));   //部件内容集合
                foreach (Match content in contentMc)
                {
                    Dictionary<string, object> Dicpara = getParameter(content.Groups["paramet"].Value); //获取参数

                    templetaInfo templeta = new templetaInfo() { Src = item.Groups["Src"].Value, Tagname = item.Groups["TagName"].Value, Wew = item.Groups["TagPrefix"].Value, Parameter = Dicpara };
                    if (dic != null && !dic.ContainsKey(templeta.Src))
                    {
                        dic.Add(templeta.Src, templeta);
                    }
                }
            }

            return dic;
        }
        #endregion

        /// <summary>
        /// 获取字段字典
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Dictionary<string, object> getParameter(string value)
        {
            MatchCollection mc = Regex.Matches(value, "(?<key>[\\w]*?)=\"(?<value>[\\w\\W\\s]*?)\"");
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            foreach (Match item in mc)
            {
                if (!item.Groups["key"].Value.ToLower().Equals("control") && !item.Groups["key"].Value.ToLower().Equals("filename") && !item.Groups["key"].Value.ToLower().Equals("id") && !item.Groups["key"].Value.ToLower().Equals("runat")) //过滤条件(这些不是字段)
                {
                    dic.Add(item.Groups["key"].Value, item.Groups["value"].Value);
                }
            }
            return dic;
        }
        #endregion
    }
    /// <summary>
    /// 部件信息
    /// </summary>
    class templetaInfo
    {
        private string src;

        public string Src
        {
            get { return src; }
            set { src = value; }
        }
        private string tagname;


        public string Tagname
        {
            get { return tagname; }
            set { tagname = value; }
        }

        private string wew;

        public string Wew
        {
            get { return wew; }
            set { wew = value; }
        }

        private Dictionary<string, Object> parameter;
        /// <summary>
        /// 字段
        /// </summary>
        public Dictionary<string, Object> Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }
        private string error;

        public string Error
        {
            get { return error; }
            set { error = value; }
        }
    }
}
