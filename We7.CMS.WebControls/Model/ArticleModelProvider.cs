using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using System.Web;
using We7.CMS.Common.Enum;
using System.Text.RegularExpressions;
using We7.Framework;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 这个类主要用于文章模型数据的查询
    /// </summary>
    public class ArticleModelProvider : BaseWebControl
    {
        #region 属性
        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleID
        {
            get
            {
                return ArticleHelper.GetArticleIDFromURL();
            }
        }

        string bindColumnID;
        /// <summary>
        /// 控件绑定ID
        /// </summary>
        public string BindColumnID
        {
            get 
            {
                if (String.IsNullOrEmpty(bindColumnID) && !UseModel)
                {
                    bindColumnID = ChannelHelper.GetChannelIDFromURL();
                }
                return bindColumnID;
            }
            set { bindColumnID = value; }
        }

        /// <summary>
        /// 是否使用模型
        /// </summary>
        public bool UseModel { get; set; }
        /// <summary>
        /// Css样式
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// 是否允许分页
        /// </summary>
        public bool AllowPager { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

        private int titleMaxLength = 30;
        /// <summary>
        /// 标题最大长度
        /// </summary>
        public int TitleMaxLength
        {
            get { return titleMaxLength; }
            set { titleMaxLength = value; }
        }

        int summaryMaxLength=300;
        /// <summary>
        /// 摘要显示的最大长度（默认200个字符）
        /// </summary>
        public int SummaryMaxLength
        {
            get { return summaryMaxLength; }
            set { summaryMaxLength = value; }
        }

        bool showAtHome = false;
        /// <summary>
        /// 是否显示置顶文章（默认不显示）
        /// </summary>
        public bool ShowAtHome
        {
            get { return showAtHome; }
            set { showAtHome = value; }
        }

        string dateFormat = "yyyy-MM-dd HH:mm";
        /// <summary>
        /// 日期显示格式（不填为“十五分钟前”样式，可以填写“yyyy-MM-dd”格式）
        /// </summary>
        public string DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        /// <summary>
        /// 缩略图标签
        /// </summary>
        public string ThumbnailTag { get; set; }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 是否是图片文章
        /// </summary>
        public bool IsImage { get; set; }

        /// <summary>
        /// 是否显示子栏目文章（默认不）
        /// </summary>
        public bool ShowSubArticle { get; set; }

        /// <summary>
        /// 频道是否有链接（默认否）
        /// </summary>
        public bool ChannelHasLink { get; set; }

        /// <summary>
        /// 是否显示文章所在栏目（默认不，显示则设置为类似于“[{0}]”格式）
        /// </summary>
        public string ShowChannel { get; set; }

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

        /// <summary>
        /// 是否只需显示内容，不需要进入详细页链接
        /// </summary>
        public bool NoLink { get; set; }

        /// <summary>
        /// 通过Url获取查询关键字
        /// </summary>
        public string KeyWord
        {
            get
            {
                if (Request["keyword"] == null || Request["keyword"].ToString() == "")
                    return null;
                else
                    return We7Helper.RemoveHtml(Request["keyword"].ToString());
            }
        }

        /// <summary>
        /// 栏目映射关系
        /// </summary>
        private IDictionary<string, Channel> ChannelMap
        {
            get
            {
                if (HttpContext.Current.Items["$ChannelMap"] == null)
                {
                    HttpContext.Current.Items["$ChannelMap"] = new Dictionary<string, Channel>();
                }
                return HttpContext.Current.Items["$ChannelMap"] as IDictionary<string, Channel>;
            }
        }
        #endregion

        #region 数据访问对象

        /// <summary>
        /// 文章业务助手
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        /// <summary>
        /// 文章索引业务助手
        /// </summary>
        protected ArticleIndexHelper ArticleIndexHelper
        {
            get { return HelperFactory.GetHelper<ArticleIndexHelper>(); }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 加载单条记录
        /// </summary>
        /// <returns></returns>
        Article LoadArticle()
        {
            Article article=ArticleHelper.GetArticle(ArticleID, null);
            FormatArticle(article);
            return article;
        }

        /// <summary>
        /// 加载数据集合
        /// </summary>
        /// <returns></returns>
        List<Article> LoadArticles()
        {
            List<Article> list = GetArticleListFromDB();
            return list;
        }

        void FormatArticle(Article article)
        {
            if (IsImage)
            {
                article.TagThumbnail = article.GetTagThumbnail(ThumbnailTag);
            }
            if (!ChannelMap.ContainsKey(article.OwnerID))
            {
                Channel ch = ChannelHelper.GetChannel(article.OwnerID, null);
                ChannelMap.Add(article.OwnerID,ch);
            }

            if (!String.IsNullOrEmpty(ShowChannel))
            {
                Channel ch = ChannelMap[article.OwnerID];
                string channelName =ch.Name;
                if (ChannelHasLink)
                    channelName = string.Format("<a href='{1}' target='{2}'>{0}</a>", ch.Name, article.ChannelFullUrl, LinkTarget);

                article.FullChannelPath = string.Format(ShowChannel, channelName);
            }

            article.Icon = GetIcon(article.State);

            if (NoLink)
            {
                article.LinkUrl = "";
            }
            else
            {
                if (article.ContentType == (int)TypeOfArticle.LinkArticle)
                {
                    article.LinkUrl = article.ContentUrl;
                }
                else
                {
                    if (We7Helper.IsEmptyID(article.OwnerID))
                    {
                        string key=article.ModelName + "$modelchannelurl$const";
                        if(!ChannelMap.ContainsKey(key))
                        {
                            List<Channel> chs=ChannelHelper.GetChannelByModelName(article.ModelName);
                            article.LinkUrl = String.Format("{0}{1}", chs!=null&&chs.Count>0?chs[0].FullUrl:"", article.FullUrl);
                        }
                    }
                    else
                    {
                        article.LinkUrl = String.Format("{0}{1}", article.ChannelFullUrl, article.FullUrl);
                    }
                }
            }

            if (DateFormat == null || DateFormat == "")
            {
                TimeSpan ts = DateTime.Now - article.Updated;
                article.TimeNote = GetTimeNote(ts);
            }
            else
            {
                article.TimeNote = article.Updated.ToString(DateFormat);
            }

            if (ShowField("ToolTip"))
            {
                article.FullTitle = article.Title;
                article.FullTitle += "," + article.TimeNote;
                if (article.Clicks > 0) article.FullTitle += ",阅读量:" + article.Clicks;
            }
            else
                article.FullTitle = article.Title;

            if (TitleMaxLength > 0 && article.Title.Length > TitleMaxLength)
            {
                article.Title = article.Title.Substring(0, TitleMaxLength) + "...";
            }

            if (!string.IsNullOrEmpty(KeyWord))
                article.Title = article.Title.Replace(KeyWord, "<em>" + KeyWord + "</em>");

            if (Show("Description"))
            {
                if (SummaryMaxLength > 0 && article.Description != null &&
                    article.Description.Length > SummaryMaxLength)
                {
                    article.Description = article.Description.Substring(0, SummaryMaxLength) + "...";
                }

                if (string.IsNullOrEmpty(article.Description) && !string.IsNullOrEmpty(article.Content))
                {
                    string content = We7Helper.RemoveHtml(article.Content);
                    if (content.Length > summaryMaxLength)
                        article.Description = content.Substring(0, SummaryMaxLength) + "...";
                    else
                        article.Description = content;
                }

                if (!string.IsNullOrEmpty(KeyWord) && !string.IsNullOrEmpty(article.Description))
                    article.Description = article.Description.Replace(KeyWord, "<em>" + KeyWord + "</em>");
            }

        }

        /// <summary>
        /// 取得数据列表
        /// </summary>
        /// <returns>文章列表</returns>
        List<Article> GetArticleListFromDB()
        {
            ArticleQuery query = new ArticleQuery();
            List<Article> list = new List<Article>();
            if (String.IsNullOrEmpty(Tag))
            {
                query.ChannelID = BindColumnID;
            }
            else
            {
                query.Tag = Tag;
            }
            Channel ch = ChannelHelper.GetChannel(query.ChannelID, new string[] { "FullFolderPath", "FullUrl" });
            query.ChannelFullUrl = ch != null ? ch.FullUrl : "";
            query.IncludeAllSons = ShowSubArticle;
            query.EnumState = "";
            query.ModelName = ModelName;
            query.UseModel = UseModel;

            query.State = ArticleStates.Started;
            query.IsShowHome = ShowAtHome ? "1" : "";
            query.IsImage = IsImage ? "1" : "";

            if (!String.IsNullOrEmpty(OrderFields))
                query.OrderKeys = OrderFields;
            else
                query.OrderKeys = "Updated|Desc";

            query.Overdue = false;
            if (!String.IsNullOrEmpty(KeyWord))
            {
                query.KeyWord = KeyWord;
            }

            RecordCount = ArticleHelper.QueryArtilceModelCountByAll(query);
            if (PageSize>0)
            {
                list = ArticleHelper.QueryArtilceModelByAll(query, StartItem, PageItemsCount, null);
            }
            else
            {
                list = ArticleHelper.QueryArtilceModelByAll(query, 0, RecordCount, null);
            }

            foreach (Article a in list)
            {
                FormatArticle(a);
            }
            return list;
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
        
        #endregion

        #region 公共接口

        private Article item;
        /// <summary>
        /// 当前记录
        /// </summary>
        public Article Item
        {
            get
            {
                if (item == null)
                {
                    item = LoadArticle();
                }
                return item;
            }
        }

        private List<Article> items;
        /// <summary>
        /// 当前的数据集记录
        /// </summary>
        public List<Article> Items
        {
            get
            {
                if (items == null)
                {
                    items = LoadArticles();
                }
                return items;
            }
        }

        public override string RelationValue
        {
            get
            {
                return item != null ? item.ID : (items != null && items.Count > 0 ? items[0].ID : "");
            }
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="title">要截取的文字</param>
        /// <param name="maxlength">最大长度</param>
        /// <returns></returns>
        public string SubTitle(string title, int maxlength)
        {
            return SubTitle(title, maxlength, "...");
        }

        public string SubTitle(object title, int maxlenght)
        {
            if (title == null)
                return "";
            return SubTitle(title.ToString(), maxlenght);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="title">要截取的文字</param>
        /// <param name="maxlenght">最大长度</param>
        /// <param name="ext">扩展字符</param>
        /// <returns></returns>
        public string SubTitle(string title, int maxlenght, string ext)
        {
            return We7.Framework.Util.Utils.GetUnicodeSubString(title, maxlenght, ext);
        }

        public string SubTitle(object title, int maxlenght, string ext)
        {
            if (title == null)
                return "";
            return SubTitle(title.ToString(), maxlenght, ext);
        }

        #endregion
    }
}
