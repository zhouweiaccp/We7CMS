using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;

using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Framework;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 栏目及下属文章数据提供者
    /// </summary>
    public partial class ChannelArticlesProvider : ChannelDataProvider
    {
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
        /// 取得数据列表
        /// </summary>
        /// <returns>文章列表</returns>
        List<Article> GetArticleList(Channel ch)
        {            
            ArticleQuery query = new ArticleQuery();
            List<Article> list = new List<Article>();

            if (DesignHelper.IsDesigning)
            {
                RecordCount = DesignHelper.FillItems(out list, PageSize);
            }
            else
            {
                query.ChannelID = ch.ID;
                query.ChannelFullUrl = ch.FullUrl;
                query.IncludeAllSons = true;

                query.State = ArticleStates.Started;
                query.IsShowHome = ShowAtHome ? "1" : "";

                if (!String.IsNullOrEmpty(ArticleOrderFields))
                    query.OrderKeys = ArticleOrderFields;
                else
                    query.OrderKeys = "Updated|Desc";

                query.Overdue = true;
                if (ArticleTag != null && ArticleTag.Length > 0)
                {
                    query.Tag = ArticleTag;
                }

                list = ArticleHelper.QueryArtilcesByAll(query, 0, PageSize, null);
            }
            return list;
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
                if (!String.IsNullOrEmpty(a.Thumbnail))
                {
                    a.TagThumbnail = GetTagThumbnail(a, ThumbnailTag);
                }

                if (!String.IsNullOrEmpty(ShowChannel))
                {
                    Channel ch = ChannelHelper.GetChannel(a.OwnerID, null);
                    string channelName = ch.Name;
                    if (ChannelHasLink)
                        channelName = string.Format("<a href='{1}' target='{2}'>{0}</a>", ch.Name, a.ChannelFullUrl, LinkTarget);

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

                if (ArticleTitleMaxLength > 0 && a.Title.Length > ArticleTitleMaxLength)
                {
                    a.Title = a.Title.Substring(0, ArticleTitleMaxLength) + "...";
                }

                if (Show("Description"))
                {
                    if (SummaryMaxLength > 0 && a.Description != null &&
                        a.Description.Length > SummaryMaxLength)
                    {
                        a.Description = a.Description.Substring(0, SummaryMaxLength) + "...";
                    }

                    if (string.IsNullOrEmpty(a.Description))
                    {
                        string content = We7Helper.RemoveHtml(a.Content);
                        if (content.Length > summaryMaxLength)
                            a.Description = content.Substring(0, SummaryMaxLength) + "...";
                        else
                            a.Description = content;
                    }
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
        /// 取得附件名称
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <param name="type">附件类型</param>
        /// <returns>附件名称</returns>
        public string GetAttachmentName(string id, string type)
        {
            string attachmentName = "";
            string attachmentType = "";
            int index = 0;
            if (type == "0" && AttachmentOneName != null)
            {
                attachmentType = AttachmentOneType;
            }
            if (type == "1" && AttachmentTwoName != null)
            {
                attachmentType = AttachmentTwoType;
                index = 1;
            }

            List<Attachment> list = AttachmentHelper.GetAttachments(id);
            {
                if (list.Count > 0)
                {
                    if (attachmentType == "" || attachmentType == ".*" || attachmentType == "*.*")
                    {
                        if (list.Count > index)
                            attachmentName = Context.Server.UrlEncode(list[index].FileName);
                    }
                    else
                    {
                        foreach (Attachment attachment in list)
                        {
                            if (attachment.FileType == attachmentType)
                                attachmentName = Context.Server.UrlEncode(attachment.FileName);
                            break;
                        }
                    }
                }
            }
            return attachmentName;
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
        /// 取得栏目及其下属文章
        /// </summary>
        public void LoadArticlesOfChannels()
        {
            Channels = GetChannels();
            ShowFields = ArticleShowFields;
            ConstructFelds();
            while (Channels.Count % ColumnCount != 0)
            {
                Channels.Add(new Channel());
            }
            foreach (Channel ch in Channels)
            {
                if (ch.ID != null)
                {
                    if ((TypeOfChannel)int.Parse(ch.Type) == TypeOfChannel.RssOriginal)
                    {
                        RssDataProvider rssProvider = new RssDataProvider();
                        rssProvider.PageSize = PageSize;
                        ch.Articles = rssProvider.ProcessRSSItem(ch.ReturnUrl);
                    }
                    else
                    {
                        ch.Articles = FormatArticlesData(GetArticleList(ch));
                    }
                }
                else
                    ch.Articles = new List<Article>();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            //base.OnLoad(e);
            LoadArticlesOfChannels();
        }
    }
}
