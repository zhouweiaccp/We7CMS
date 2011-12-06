using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.CMS.WebControls.Core;
using We7.CMS.WebControls;
using We7.Framework.Config;
using We7.CMS.Common;
using System.Collections.Generic;
using We7.Plugin.FullTextSearch;
using WebEngine2007.SE;
using We7.CMS.Common.Enum;
using System.Text.RegularExpressions;
using We7.CMS.Module.VisualTemplate;

namespace We7.CMS.Web.Widgets.ShopDownload
{
    [ControlGroupDescription(Label = "站群搜索结果列表", Icon = "站群搜索结果列表", Description = "站群搜索结果列表", DefaultType = "FullTextSearch.Bar")]
    [ControlDescription("站群搜索结果列表")]
    public partial class FullTextSearch_Result : ThinkmentDataControl
    {
        /// <summary>
        /// 栏目类业务助手
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// 附件类业务助手
        /// </summary>
        protected AttachmentHelper AttachmentHelper
        {
            get { return HelperFactory.GetHelper<AttachmentHelper>(); }
        }

        public string Keyword
        {
            get
            {
                return Request["keyword"];
            }
        }
        private List<Article> articles;

        #region 字段
        string thumbnailTag;
        /// <summary>
        /// 缩略图标签
        /// </summary>
        public string ThumbnailTag
        {
            get { return thumbnailTag; }
            set { thumbnailTag = value; }
        }

        int titleMaxLength;
        /// <summary>
        /// 标题显示最大长度（默认30个字符）
        /// </summary>
        public int TitleMaxLength
        {
            get { return titleMaxLength; }
            set { titleMaxLength = value; }
        }

        string showChannel = "";
        /// <summary>
        /// 是否显示文章所在栏目（默认不，显示则设置为类似于“[{0}]”格式）
        /// </summary>
        public string ShowChannel
        {
            get { return showChannel; }
            set { showChannel = value; }
        }

        bool channelHasLink = false;
        /// <summary>
        /// 频道是否有链接（默认否）
        /// </summary>
        public bool ChannelHasLink
        {
            get { return channelHasLink; }
            set { channelHasLink = value; }
        }

        bool noLink = false;
        /// <summary>
        /// 是否只需显示内容，不需要进入详细页链接
        /// </summary>
        public bool NoLink
        {
            get { return noLink; }
            set { noLink = value; }
        }

        string linkTarget;
        /// <summary>
        /// 新页面的打开模式（默认弹出新窗口）
        /// </summary>
        public string LinkTarget
        {
            get
            {
                if (linkTarget != null && linkTarget != "")
                    return linkTarget;
                else
                    return "_blank";
            }
            set { linkTarget = value; }
        }

        int summaryMaxLength;
        /// <summary>
        /// 摘要显示的最大长度（默认200个字符）
        /// </summary>
        public int SummaryMaxLength
        {
            get { return summaryMaxLength; }
            set { summaryMaxLength = value; }
        }

        private string showFields;
        /// <summary>
        /// 要显示的字段
        /// </summary>
        public string ShowFields
        {
            get { return showFields; }
            set { showFields = value; }
        }

        string dateFormat = "yyyy-MM-dd";
        /// <summary>
        /// 日期显示格式（不填为“十五分钟前”样式，可以填写“yyyy-MM-dd”格式）
        /// </summary>
        public string DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        public int attachmentNum;
        /// <summary>
        /// 显示附件数量
        /// </summary>
        public int AttachmentNum
        {
            get { return attachmentNum; }
            set { attachmentNum = value; }
        }
        #endregion

        /// <summary>
        /// 是否仅搜索本站信息
        /// </summary>
        [Parameter(Title = "仅搜索本站", Type = "Boolean", DefaultValue = "false",Required=true)]
        public bool OnlyThisSite;

        /// <summary>
        /// 自定义Css类名称
        /// </summary>
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "FullTextSearch_Result")]
        public string CssClass;


        /// <summary>
        /// 分页器
        /// </summary>
        [Children]
        public ControlPager Pager = new ControlPager();

        /// <summary>
        /// 页面初始化时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            articles = GetArtciles();
        }

        private List<Article> GetArtciles()
        {
            ShowFields = "ID,Title,Description,Updated,Source";
            SearchResult result = QueryAdvance();
            List<Article> items = new List<Article>();
            if (result != null)
            {
                Pager.RecordCount = result.RecordCount;
                foreach (QueryArticle a in result.ArticleList)
                {
                    Article article = new Article();
                    article.ID = a.ArcicleID;
                    article.Title = a.ArticleTitle;
                    article.Content = a.ArticleContent;
                    string modelXml = a.ArticleModelXML;
                    if (!string.IsNullOrEmpty(modelXml))
                        modelXml = We7Helper.RemoveHtml(modelXml);
                    if (!string.IsNullOrEmpty(modelXml))
                        modelXml = We7Helper.RemoveGUID(modelXml);

                    if (string.IsNullOrEmpty(article.Content))
                        article.Content = modelXml;
                    else
                        article.Description = modelXml;

                    article.ChannelFullUrl = a.ArticleCategoryUrl;
                    article.FullChannelPath = a.ArticleCategory;
                    article.ContentUrl = a.ArticleUrl;
                    article.ModelName = a.ArticleType;
                    article.SiteName = a.SiteName;
                    article.SiteUrl = a.SiteUrl;
                    article.Created = Convert.ToDateTime(a.ArticleCreateTime);
                    article.ContentType = (int)TypeOfArticle.LinkArticle;

                    items.Add(article);
                }
                if(SummaryMaxLength==0)
                    SummaryMaxLength =200;
                items = FormatArticlesData(items);
            }
            return items;
        }

        /// <summary>
        /// 文章列表
        /// </summary>
        public List<Article> Articles
        {
            get
            {
                if (articles == null)
                {
                    try
                    {
                        articles = GetArtciles();
                        Pager.RecordCount = articles.Count;
                    }
                    catch
                    {
                        articles = null;
                    }
                }
                return articles;
            }
        }

        SearchResult QueryAdvance()
        {
            SearchResult result = null;
            SiteConfigInfo si = SiteConfigs.GetConfig();
            string url = "tcp://localhost:11001";
            if (!string.IsNullOrEmpty(si.FullTextSearchUrl))
                url = si.FullTextSearchUrl;

            ISearchService searcher = Activator.GetObject(typeof(ISearchService), url + "/SearchService") as ISearchService;
            if (searcher != null)
            {
                Query query = new Query();
                bool onlyArticleType;
                int days;
                byte keywordsRange;

                if (bool.TryParse(Request["ArticleTypeFlags"], out onlyArticleType)
                    && !string.IsNullOrEmpty(Request["ArticleType"]))
                    query.ArticleType = Request["ArticleType"];

                if (int.TryParse(Request["ArticleCreateTime"], out days) && days > 0)
                    query.ArticleCreateTime = DateTime.Now.AddDays(days * -1).ToString("yyyyMMddHHmmss");

                if (OnlyThisSite) query.Url = si.RootUrl;
                byte.TryParse(Request["KeywordsRange"], out keywordsRange);

                query.Keywords = Keyword;
                query.OnlyArticleType = onlyArticleType;
                query.KeywordsRange = (KeywordsRange)keywordsRange;
                query.OnlyUrl = OnlyThisSite;
                query.PageCount = Pager.PageSize;
                query.PageIndex = Pager.PageIndex;

                result = searcher.AdvanceSearch(query);
            }
            return result;
        }

        SearchResult QuerySimple()
        {
            SearchResult result = null;
            SiteConfigInfo si = SiteConfigs.GetConfig();
            string url = "tcp://localhost:11001";
            if (!string.IsNullOrEmpty(si.FullTextSearchUrl))
                url = si.FullTextSearchUrl;

            ISearchService searcher = Activator.GetObject(typeof(ISearchService), url + "/IndexService") as ISearchService;
            {
                Query query = new Query();
                query.Keywords = Keyword;
                query.PageCount = Pager.PageSize;
                query.PageIndex = Pager.PageIndex;
                result = searcher.SimpleSearch(query);
            }
            return result;
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

        public string GetTagThumbnail(Article article, string tag)
        {
            if (!String.IsNullOrEmpty(article.Thumbnail))
            {
                return article != null ? article.GetTagThumbnail(tag) : String.Empty;
            }            
            return String.Empty;
        }

        /// <summary>
        /// 获取文章状态的对应图标
        /// </summary>
        /// <param name="state">文章状态</param>
        /// <returns>文章状态对应的图片</returns>
        protected virtual string GetIcon(int state)
        {
            switch (state)
            {
                case 0:
                    return "images/icon_lock.gif";

                case 1:
                    return "images/icon_doc.gif";

                case 2:
                    return "images/icon_file.gif";

                default:
                    return "images/icon_info.gif";
            }
        }

        /// <summary>
        /// 获取时间的相对描述（5小时以前、三天以前、……）
        /// </summary>
        /// <param name="ts">时间间隔</param>
        /// <returns>格式化后的时间间隔</returns>
        protected virtual string GetTimeNote(TimeSpan ts)
        {
            if (ts >= new TimeSpan(1, 0, 0, 0))
            {
                return String.Format("{0:N0}天以前", ts.TotalDays);
            }
            else if (ts >= new TimeSpan(1, 0, 0))
            {
                return String.Format("{0:N0}小时以前", ts.TotalHours);
            }
            else if (ts >= new TimeSpan(0, 1, 0))
            {
                return String.Format("{0:N0}分钟以前", ts.TotalMinutes);
            }
            else
            {
                return "刚才";
            }
        }

        /// <summary>
        /// 格式化列表中的数据
        /// </summary>
        /// <param name="list">文章列表</param>
        List<Article> FormatArticlesData(List<Article> list)
        {
            DateTime now = DateTime.Now;

            foreach (Article a in list)
            {
                if (ShowField("Thumbnail"))
                {
                    a.TagThumbnail = GetTagThumbnail(a, ThumbnailTag); //a.GetTagThumbnail(ThumbnailTag);
                }
                //Channel ch = ChannelHelper.GetChannel(a.OwnerID, null);
                if (!String.IsNullOrEmpty(ShowChannel))
                {
                    string channelName = a.ChannelName;
                    if (ChannelHasLink)
                        channelName = string.Format("<a href='{1}' target='{2}'>{0}</a>", a.ChannelName, a.ChannelFullUrl, LinkTarget);

                    a.FullChannelPath = string.Format(ShowChannel, channelName);
                }

                a.Icon = GetIcon(a.State);

                if (NoLink)
                    a.LinkUrl = "";
                else
                {
                    if (a.ContentType == (int)TypeOfArticle.LinkArticle)
                    {
                        a.LinkUrl = a.ContentUrl;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(a.ChannelFullUrl) && !String.IsNullOrEmpty(a.ModelName))
                        {
                            //这儿只能做一个容错处理
                            List<Channel> chs = ChannelHelper.GetChannelByModelName(a.ModelName);
                            if (chs.Count > 0)
                            {
                                a.ChannelFullUrl = chs[0].FullUrl;
                            }
                        }
                        a.LinkUrl = String.Format("{0}{1}", a.ChannelFullUrl, a.FullUrl);

                    }
                }

                if (DateFormat == null || DateFormat == "")
                {
                    TimeSpan ts = now - a.Updated;
                    a.TimeNote = GetTimeNote(ts);
                }
                else
                {
                    a.TimeNote = a.Updated.ToString(DateFormat);
                }

                if (ShowField("ToolTip"))
                {
                    a.FullTitle = a.Title;
                    a.FullTitle += "," + a.TimeNote;
                    if (a.Clicks > 0) a.FullTitle += ",阅读量:" + a.Clicks;
                }
                else
                    a.FullTitle = a.Title;

                if (TitleMaxLength > 0 && a.Title.Length > TitleMaxLength)
                {
                    a.Title = a.Title.Substring(0, TitleMaxLength) + "...";
                }

                if (!string.IsNullOrEmpty(Keyword))
                    a.Title = a.Title.Replace(Keyword, "<em>" + Keyword + "</em>");

                if (ShowField("Description"))
                {
                    if (SummaryMaxLength > 0 && a.Description != null &&
                        a.Description.Length > SummaryMaxLength)
                    {
                        a.Description = a.Description.Substring(0, SummaryMaxLength) + "...";
                    }

                    if (string.IsNullOrEmpty(a.Description) && !string.IsNullOrEmpty(a.Content))
                    {
                        string content = We7Helper.RemoveHtml(a.Content);
                        if (content.Length > summaryMaxLength)
                            a.Description = content.Substring(0, SummaryMaxLength) + "...";
                        else
                            a.Description = content;
                    }

                    if (!string.IsNullOrEmpty(Keyword) && !string.IsNullOrEmpty(a.Description))
                        a.Description = a.Description.Replace(Keyword, "<em>" + Keyword + "</em>");
                }

                if (AttachmentNum > 0)
                {
                    a.Attachments = AttachmentHelper.GetAttachments(a.ID);
                }
            }
            return list;
        }
    }
}