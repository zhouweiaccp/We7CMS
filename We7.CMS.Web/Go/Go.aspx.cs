using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

using Thinkment.Data;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.Framework.Config;
using We7.CMS.Accounts;
using We7.Framework.Util;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace We7.CMS.Web
{
    /// <summary>
    /// 模板路由类：综合处理（已过期-2009-12-17）
    /// </summary>
    public partial class Go : FrontBasePage
    {

        string ColumnID;

        string ColumnMode;

        string SearchWord
        {
            get { return Request["keyword"]; }
        }
        string SeSearchWord
        {
            get { return Request["sekeyword"]; }
        }

        string ColumnAlias
        {
            get { return Request["alias"]; }
        }

        string ArticleID;

        protected override string[] Permissions
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

        protected override string PermissionObjectID
        {
            get
            {
                if (ThisChannel != null)
                {
                    if (ThisChannel.SecurityLevel > 0)
                    {
                        if (AccountID == null)
                        {
                            Response.Redirect("/go/login.aspx?ReturnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl));
                        }
                        else
                            return ColumnID;
                    }
                }
                return base.PermissionObjectID;
            }
        }

        protected IPSecurityHelper IPSecurityHelper
        {
            get { return HelperFactory.GetHelper<IPSecurityHelper>(); }
        }

        protected override string TemplatePath { get; set; }        

        Channel ThisChannel;


        void Page_Init(object sender, System.EventArgs e)
        {
            if (!BaseConfigs.ConfigFileExist())
            {
                Response.Write("您的数据库配置文件尚未生成，看起来数据库尚未建立，您需要建立数据库配置文件或生成数据库。现在开始吗？<a href='/install/index.aspx'><u>现在配置数据库</u></a>");
                Response.End();
            }

            if (UnLoginCheck())
            {
                Response.Redirect("/go/login.aspx?returnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl));
            }

            Initialize();

            if (!CheckIPStrategy())
            {
                Response.Write("IP受限，您的IP没有在受访范围内！");
                Response.End();
                return;
            }

            if (TemplatePath != null)
            {
                if (File.Exists(Context.Server.MapPath(TemplatePath)))
                {
                    #region 页面处加缓存也需要对页面缓存进行三级划分
                    //绝不能用普通Cache的方法，去处理页面缓存，这里也需要
                    //分情况分种类去规划，然后找出模板自动加缓存的机制与方法。
                    #endregion


                    Control ctl = LoadControl(TemplatePath);

                    this.Controls.Add(ctl);

                    try
                    {
                        if (this.Title != null)
                        {
                            this.Title = GetCurrentPageTitle(ColumnID, ArticleID);
                        }
                        //meta标记
                        HtmlGenericControl KeywordsMeta = new HtmlGenericControl("meta");
                        KeywordsMeta.Attributes["name"] = "keywords";
                        string keyWordString = "";
                        keyWordString = CDHelper.Config.KeywordPageMeta;

                        HtmlGenericControl DescriptionMeta = new HtmlGenericControl("meta");
                        DescriptionMeta.Attributes["name"] = "description";
                        string descriptionWordString = "";
                        descriptionWordString = CDHelper.Config.DescriptionPageMeta;

                        if (ColumnID != "" && ArticleID != "")
                        {
                            Channel channel = ChannelHelper.GetChannel(ColumnID, null);
                            Article article = ArticleHelper.GetArticle(ArticleID);
                            if ((channel.KeyWord + article.KeyWord).Length > 0)
                                keyWordString = channel.KeyWord + article.KeyWord;
                            if ((channel.DescriptionKey + article.DescriptionKey).Length > 0)
                                descriptionWordString = channel.DescriptionKey + article.DescriptionKey;
                        }
                        else if (ColumnID == "" && ArticleID != "")
                        {
                            Article article = ArticleHelper.GetArticle(ArticleID);
                            if (article.KeyWord != null && article.KeyWord.Length > 0)
                                keyWordString = article.KeyWord;
                            if (article.DescriptionKey != null && article.DescriptionKey.Length > 0)
                                descriptionWordString = article.DescriptionKey;
                        }
                        else if (ColumnID != "" && ArticleID == "")
                        {
                            Channel channel = ChannelHelper.GetChannel(ColumnID, null);
                            if (channel.KeyWord != null && channel.KeyWord.Length > 0)
                                keyWordString = channel.KeyWord;
                            if (channel.DescriptionKey != null && channel.DescriptionKey.Length > 0)
                                descriptionWordString = channel.DescriptionKey;
                        }

                        KeywordsMeta.Attributes["content"] = keyWordString.ToString();
                        this.Header.Controls.Add(KeywordsMeta);

                        DescriptionMeta.Attributes["content"] = descriptionWordString.ToString();
                        this.Header.Controls.Add(DescriptionMeta);

                    }
                    catch (Exception ex)
                    {
                        We7.Framework.LogHelper.WriteLog(typeof(Go), ex);
                    }

                    //点击量统计
                    AddStatistic();
                }
                else
                {
                    Response.Write("警告：该栏目指定的模板不存在，请重新指定或检查模板的位置是否正确！");
                }
            }
        }

        protected void Initialize()
        {
            bool enableCache = (CDHelper.Config.EnableCache == "true");
            string result = "";
            string curUrl = Context.Request.Path.Replace('/', '_');
            result = ChannelHelper.GetChannelIDFromURL();
            ColumnID = result;

            //初始化ArticleID
            result = ArticleHelper.GetArticleIDFromURL();
            ArticleID = result;

            //初始化ColumnMode
            result = GetColumnMode();
            ColumnMode = result;

            //初始化ThisChannel
            Channel ch = null;
            if (ColumnID != null && ColumnID != "")
            {
                ch = ChannelHelper.GetChannel(ColumnID, null);
            }
            else if (ColumnAlias != null)
            {
                ch = ChannelHelper.GetChannelByAlias(ColumnAlias);
            }
            else
            {
                ch = null;
            }

            ThisChannel = ch;
            //初始化TemplatePath
            result = TemplateHelper.GetThisPageTemplate(ColumnMode, ColumnID, SearchWord, SeSearchWord);
            if (result != null)
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

        void AddStatistic()
        {
            PageVisitor pv;
            if (Session[PageVisitorHelper.PageVisitorSessionKey] == null)
            {
                pv = PageVisitorHelper.AddPageVisitor(AccountID);
                Session[PageVisitorHelper.PageVisitorSessionKey] = pv;
            }
            else
                pv = (PageVisitor)Session[PageVisitorHelper.PageVisitorSessionKey];

            if (pv != null)
            {
                StatisticsHelper.AddStatistics(pv, ArticleID, ColumnID);
                TimeSpan ts = DateTime.Now - pv.OnlineTime;
                pv.Clicks += 1;
                if (ts.TotalSeconds > 10)//改为10秒 刷新在线时间
                {
                    pv.OnlineTime = DateTime.Now;
                    pv.LogoutDate = DateTime.Now;
                    PageVisitorHelper.UpdatePageVisitor(pv, new string[] { "OnlineTime", "LogoutDate","Clicks" });
                }
            }
            else
                Session[PageVisitorHelper.PageVisitorSessionKey] = null;
        }

        /// <summary>
        /// 获取前台当前页面栏目内容类型
        /// </summary>
        /// <returns></returns>
        string GetColumnMode()
        {
            if (Request["mode"] != null)
                return Request["mode"];
            else
            {
                if (ArticleID != null && ArticleID != "")
                {
                    Article a = ArticleHelper.GetArticle(ArticleID);
                    string channelID = "";
                    if (a != null) channelID = a.OwnerID;
                    if (channelID != "")
                    {
                        Channel ch = this.ChannelHelper.GetChannel(channelID, new string[] { "EnumState" });
                        //string type = StateManagement.GetStateName(ch.EnumState, UserEnumLibrary.Business.ArticleType).ToString();
                        EnumLibrary.ArticleType type = (EnumLibrary.ArticleType)StateMgr.GetStateValueEnum(ch.EnumState, EnumLibrary.Business.ArticleType);
                        if (type == EnumLibrary.ArticleType.Product)
                        {
                            return "productDetail";
                        }
                        else if (type == EnumLibrary.ArticleType.Article)
                        {
                            return "detail";
                        }
                        else
                        {
                            return "contentMode";
                        }
                    }
                    else if (AdviceHelper.Exist(ArticleID))
                    {
                        return "adviceMode";
                    }
                    else
                    { return string.Empty; }
                }

                else
                    return string.Empty;
            }
        }


        private bool UnLoginCheck()
        {
            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            return ci.OnlyLoginUserCanVisit && !Security.IsAuthenticated();
        }

        bool CheckIPStrategy()
        {
            string ip = Context.Request.ServerVariables["REMOTE_ADDR"];
            return IPSecurityHelper.CheckIPStrategy(ip,ColumnID,ArticleID);
        }

        #region 静态化添加

        /// <summary>
        /// 重写Render方法
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(Request["Createhtml"]) && Request["Createhtml"] == "1")
            {
                StringWriter strWriter = new StringWriter();
                HtmlTextWriter tempWriter = new HtmlTextWriter(strWriter);
                try
                {
                    base.Render(tempWriter);
                }
                catch (Exception ex)
                {
                    We7.Framework.LogHelper.WriteLog(typeof(Go), ex);
                    strWriter.Write("");
                };

                //读取原始模板内容
                HtmlDocument doc = new HtmlDocument();
                doc.OptionAutoCloseOnEnd = true;
                doc.OptionCheckSyntax = true;
                doc.OptionOutputOriginalCase = true;
                try
                {
                    doc.Load(Server.MapPath(TemplatePath), Encoding.UTF8);
                }
                catch(Exception ex)
                {
                    We7.Framework.LogHelper.WriteLog(typeof(Go), ex);
                    throw new Exception("格式化HTML错误");
                }
                string strContent = doc.DocumentNode.InnerText;
                //提取控件注册信息
                string pat = @"<%@[^>]*>";
                Regex reg = new Regex(pat);
                MatchCollection m = reg.Matches(strContent);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < m.Count; i++)
                {
                    string temp = m[i].Value;
                    if (!m[i].ToString().Contains("Src=\"/"))
                    {
                        temp = m[i].Value.Replace("Src=\"", "Src=\"" + TemplatePath.Remove(TemplatePath.LastIndexOf("/")) + "/");
                    }
                    sb.Append(temp + "\r\n");
                }
                //给生成后模板添加控件注册信息  
                string content = strWriter.ToString();
                string pat1 = @"<html[^>]*>";
                string RegAndHtml = sb.ToString() + "\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">";
                content = Regex.Replace(content, pat1, RegAndHtml, RegexOptions.IgnoreCase);
                content = content.Replace("<head>", "<head runat=\"server\">");

                writer.Write(content);
                string channelUrl = ChannelHelper.GetChannelUrlFromUrl(Context.Request.RawUrl, Context.Request.ApplicationPath);
                string fileName = "";
                if (string.IsNullOrEmpty(ColumnMode))
                {
                    fileName = "index.ascx";
                }
                else
                {
                    fileName = ColumnMode + ".ascx";
                }
                string resultPath = Server.MapPath(TemplatePath.Remove(TemplatePath.LastIndexOf("/")) + "/HtmlTemplate/" + channelUrl + fileName);
                FileHelper.WriteFile(resultPath, content);
            }
            else
            {
                StringWriter strWriter = new StringWriter();
                HtmlTextWriter tempWriter = new HtmlTextWriter(strWriter);
                try
                {
                    base.Render(tempWriter);
                }
                catch (Exception ex)
                {
                    We7.Framework.LogHelper.WriteLog(typeof(Go), ex);
                    strWriter.Write("");
                };
                string content = strWriter.ToString();
                writer.Write(content);
            }
        }

        #endregion
    }
}