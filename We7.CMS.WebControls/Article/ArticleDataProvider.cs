using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS;
using We7;
using We7.CMS.Controls;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using System.IO;
using We7.Framework;
using We7.Framework.Cache;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 文章类控件数据提供者
    /// </summary>
    public partial class ArticleDataProvider : BaseWebControl
    {
        private ArticleQuery articleQuery;
        private int recordcount = -1;

        /// <summary>
        /// 附件业务助手
        /// </summary>
        protected AttachmentHelper AttachmentHelper
        {
            get { return HelperFactory.GetHelper<AttachmentHelper>(); }
        }

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

        /// <summary>
        /// 取得当前的文章
        /// </summary>
        /// <returns>文章实体</returns>
        protected virtual Article GetThisArticle()
        {
            Article article = null;
            Attachments = new List<Attachment>();
            if (DesignHelper.IsDesigning)
            {
                DesignHelper.FillItems<Article>(out article, PageSize);
                Attachments.Add(new Attachment() { ArticleID = article.ID, FilePath = DesignHelper.GetAttachment() });
                Attachments.Add(new Attachment() { ArticleID = article.ID, FilePath = DesignHelper.GetAttachment() });
                Attachments.Add(new Attachment() { ArticleID = article.ID, FilePath = DesignHelper.GetAttachment() });
                Attachments.Add(new Attachment() { ArticleID = article.ID, FilePath = DesignHelper.GetAttachment() });
            }
            else
            {
                if (We7Helper.IsEmptyID(BindColumnID) && !We7Helper.IsEmptyID(ArticleID))
                {
                    article = ArticleHelper.GetArticle(ArticleID, null);
                }
                else
                {
                    List<Article> articles = GetArticleListFromDB();
                    if (articles.Count > 0)
                        article = articles[0];
                }
            }

            if (article != null)
            {
                List<Article> list = new List<Article>();
                list.Add(article);
                FormatArticlesData(list);
                article = list[0];
                if (AttachmentNum > 0)
                {
                    Attachments = AttachmentHelper.GetAttachments(article.ID);
                }
            }
            else
            {
                article = new Article();
            }

            return article;
        }

        /// <summary>
        /// 取得数据列表
        /// </summary>
        /// <returns>文章列表</returns>
        protected List<Article> GetArticleListFromDB()
        {
            return QueryArticles();
        }

        protected ArticleQuery CreateArticleQuery()
        {
            if (articleQuery == null)
            {
                articleQuery = new ArticleQuery();
                if (IsAllInfomation)
                {
                    articleQuery.ModelName = Constants.AllInfomationModelName;
                }
                else
                {
                    articleQuery.ModelName = Constants.ArticleModelName;
                }
                List<Article> list = new List<Article>();
                if (string.IsNullOrEmpty(BindColumnID))
                {
                    if (Tag != null && Tag.Length > 0)
                    {
                    }
                    else
                        articleQuery.ChannelID = OwnerID;
                }
                else
                {
                    articleQuery.ChannelID = BindColumnID;
                }

                Channel ch = ChannelHelper.GetChannel(articleQuery.ChannelID, new string[] { "FullFolderPath", "FullUrl" });
                thisChannel = ch;
                articleQuery.ChannelFullUrl = ch != null ? ch.FullUrl : "";
                articleQuery.IncludeAllSons = ShowSubArticle;

                if (InformationType != "0")
                {
                    int type = Convert.ToInt16(InformationType);
                    articleQuery.EnumState = "0" + type.ToString();
                }
                else
                {
                    articleQuery.EnumState = "";
                }

                articleQuery.State = ArticleStates.Started;
                articleQuery.IsShowHome = ShowAtHome ? "1" : "";
                //query.IsImage = IsImage ? "1" : "";

                if (!String.IsNullOrEmpty(OrderFields))
                    articleQuery.OrderKeys = OrderFields;
                else
                    articleQuery.OrderKeys = "Updated|Desc";

                articleQuery.Overdue = true;
                if (Tag != null && Tag.Length > 0)
                {
                    articleQuery.Tag = Tag;
                }

                if (!String.IsNullOrEmpty(KeyWord))
                {
                    articleQuery.KeyWord = KeyWord;
                }
            }

            return articleQuery;
        }


        protected string CreateCacheKey(string preKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(preKey);
            sb.AppendFormat("ID:{0}$Url:{1}$", ID,Request.RawUrl);
            //sb.AppendFormat("{0}:{1}", "PageIndex", PageIndex);
            //sb.AppendFormat("{0}:{1}", "PageSize", PageSize);
            //sb.AppendFormat("{0}:{1}", "BindColumnID", BindColumnID);
            //sb.AppendFormat("{0}:{1}", "Tag", Tag);
            //sb.AppendFormat("{0}:{1}", "OwnerID", OwnerID);
            //sb.AppendFormat("{0}:{1}", "ShowSubArticle", ShowSubArticle);
            //sb.AppendFormat("{0}:{1}", "ShowAtHome", ShowAtHome);
            //sb.AppendFormat("{0}:{1}", "OrderFields", OrderFields);
            //sb.AppendFormat("{0}:{1}", "Overdue", true);
            //sb.AppendFormat("{0}:{1}", "KeyWord", KeyWord);
            //sb.AppendFormat("{0}:{1}", "AllowPager", AllowPager);
            //sb.AppendFormat("{0}:{1}", "ShowField(Thumbnail)", ShowField("Thumbnail"));
            //sb.AppendFormat("{0}:{1}", "ShowChannel", ShowChannel);
            //sb.AppendFormat("{0}:{1}", "NoLink", NoLink);
            //sb.AppendFormat("{0}:{1}", "DateFormat", DateFormat);
            //sb.AppendFormat("{0}:{1}", "ShowField(ToolTip)", ShowField("ToolTip"));
            //sb.AppendFormat("{0}:{1}", "TitleMaxLength", TitleMaxLength);
            //sb.AppendFormat("{0}:{1}", "Show(Description)", Show("Description"));
            //sb.AppendFormat("{0}:{1}", "AttachmentNum", AttachmentNum);
            return sb.ToString();
        }



        public override int RecordCount
        {
            get
            {
                if (recordcount == -1)
                {
                    recordcount = ArticleHelper.QueryArtilceCountByAll(CreateArticleQuery());
                    //(int)CacheRecord.Create("article").GetInstance<object>(CreateCacheKey("ArticleDataProvider$RecordCount$"), delegate()
                    //{
                    //    ArticleQuery query = CreateArticleQuery();
                    //    return ArticleHelper.QueryArtilceCountByAll(CreateArticleQuery);
                    //});
                }
                return recordcount;
            }
            set { recordcount = value; }
        }

        List<Article> QueryArticles()
        {
            //return CacheRecord.Create("article").GetInstance<List<Article>>(CreateCacheKey("ArticleDataProvider$QueryArticles$"), delegate()
            //{
                List<Article> list = null;
                ArticleQuery query = CreateArticleQuery();
                if (PageSize <= 0)
                {
                    list = ArticleHelper.QueryArtilcesByAll(query, 0, RecordCount, null);
                }
                else
                {
                    if (AllowPager)
                    {
                        list = ArticleHelper.QueryArtilcesByAll(query, StartItem, PageItemsCount, null);
                    }
                    else
                    {
                        if (RecordCount > PageSize)
                        {
                            list = ArticleHelper.QueryArtilcesByAll(query, 0, PageSize, null);
                        }
                        else
                        {
                            list = ArticleHelper.QueryArtilcesByAll(query, 0, RecordCount, null);
                        }
                    }
                }
                FormatArticlesData(list);
                return list;
            //});
        }

        /// <summary>
        /// 格式化列表中的数据
        /// </summary>
        /// <param name="list">文章列表</param>
        protected List<Article> FormatArticlesData(List<Article> list)
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

                if (!string.IsNullOrEmpty(KeyWord))
                    a.Title = a.Title.Replace(KeyWord, "<em>" + KeyWord + "</em>");

                if (Show("Description"))
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

                    if (!string.IsNullOrEmpty(KeyWord) && !string.IsNullOrEmpty(a.Description))
                        a.Description = a.Description.Replace(KeyWord, "<em>" + KeyWord + "</em>");
                }

                if (AttachmentNum > 0)
                {
                    a.Attachments = AttachmentHelper.GetAttachments(a.ID);
                }
            }
            return list;
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
        /// 取得缩略图
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns>缩略图地址</returns>
        public string GetThumbnail(string id)
        {
            Attachment attach = AttachmentHelper.GetAttachments(id).Find(a => a.FileName.StartsWith("thumb_", true, null));
            return attach != null ? Path.Combine(attach.FilePath, attach.FileName).Replace("\\", "/") : "";
        }

        /// <summary>
        /// 取得图片
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns>图片地址</returns>
        public string GetImage(string id)
        {
            Attachment attach = AttachmentHelper.GetAttachments(id).Find(a => !a.FileName.StartsWith("thumb_", true, null));
            return attach != null ? Path.Combine(attach.FilePath, attach.FileName).Replace("\\", "/") : "";
        }

        /// <summary>
        /// 绑定其它信息
        /// </summary>
        /// <param name="attributeName">属性名称</param>
        /// <returns>格式化后的数据</returns>
        public string BuildAttributeString(string attributeName)
        {
            if (Articles != null && Articles.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Article a in Articles)
                {
                    switch (attributeName)
                    {
                        case "Title":
                            sb.Append(a.Title);
                            break;
                        case "LinkUrl":
                            sb.Append(a.LinkUrl);
                            break;
                        case "Thumbnail":
                            //sb.Append(a.GetTagThumbnail(ThumbnailTag));
                            sb.Append(GetTagThumbnail(a, ThumbnailTag));
                            break;

                        default:
                            break;
                    }
                    sb.Append("|");
                }

                return sb.ToString().Remove(sb.Length - 1);
            }
            else
                return "";
        }


        public string GetContent(Article a)
        {
            if (!String.IsNullOrEmpty(a.Content))
            {
                return a.Content.Length > ContentMaxLength ? (a.Content.Substring(0, ContentMaxLength) + "...") : a.Content;
            }
            return "";
        }

        public override string RelationValue
        {
            get
            {
                return ThisArticle.ID;
            }
        }

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (Show("Description")) Fields.Add("Content");
        }

        #endregion

        /// <summary>
        /// 取得文章标题
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns></returns>
        public string GetUrl(string id)
        {
            if (!ChannelMap.ContainsKey(BindColumnID))
            {
                ChannelMap[BindColumnID] = ChannelHelper.GetChannel(BindColumnID, null).FullUrl;
            }
            return String.Format("{0}{1}.html", ChannelMap[BindColumnID], id.Trim('{').Trim('}').Replace("-", "_"));
        }

        /// <summary>
        /// 取得文章标题
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns></returns>
        public string GetUrl(object id)
        {
            if (id == null)
            {
                return "";
            }
            return GetUrl(id.ToString());
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

        protected List<KeyValuePair<string, string>> GetPhotos(Article article)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            if (!String.IsNullOrEmpty(article.Photos))
            {
                string[] ss = article.Photos.Split('|');
                foreach (string s in ss)
                {
                    int lastIndex = s.LastIndexOf(".");
                    string thumb = s.Substring(0, lastIndex) + "_S" + s.Substring(lastIndex);
                    list.Add(new KeyValuePair<string, string>(s, thumb));
                }
            }
            return list;
        }
    }
}
