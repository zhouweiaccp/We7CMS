using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using We7.CMS;
using Thinkment.Data;
using We7.Framework;
using We7.Framework.Util;
using We7.Framework.Config;

using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web
{
    public partial class RSS : System.Web.UI.Page
    {
        private string PageSize="20", ShowAtHome, Tag, ColoumnID, ChannelUrl;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/xml";

            if (!IsPostBack)
            {
                if (!setParam())
                    return;
                ResponseXml();
            }
        }
        /// <summary>
        /// 输出Xml文件
        /// </summary>
        private void ResponseXml()
        {
            Channel ch = new Channel();
            if (!string.IsNullOrEmpty(ChannelUrl))
                ColoumnID = myChannelHelper.GetChannelIDByFullUrl(ChannelUrl);
            
            if (!string.IsNullOrEmpty(ColoumnID))
                ch = myChannelHelper.GetChannel(ColoumnID, null);

            List<Article> list = GetArticles();
            RssChannel channel = new RssChannel();
            channel.Title = ch.Name;
            channel.Link = SiteConfigs.GetConfig().RootUrl + ch.FullUrl;
            channel.Description = ch.Description;
            channel.Ttl = PageSize;
            channel.Language = "zh-cn";
            channel.LastBuildDate = DateTime.Now.ToString("yyyy-MM-dd");
            channel.PubDate = DateTime.Now.ToString("yyyy-MM-dd");
            SetRssItems(list, channel);

            Response.Write(channel.GetXml().OuterXml);
        }

        /// <summary>
        /// 设置RssItem
        /// </summary>
        /// <param name="list"></param>
        /// <param name="channel"></param>
        private void SetRssItems(List<Article> list, RssChannel channel)
        {
            List<RssItem> l = new List<RssItem>();
            foreach (Article article in list)
            {
                FormatArticleUrl(article);

                RssItem item = new RssItem();
                item.Author = article.Author;
                item.Link = article.LinkUrl;
                item.Title = article.Title;
                if (!string.IsNullOrEmpty(article.Description))
                    item.Description = article.Description;
                else
                {
                    string content = We7Helper.RemoveHtml(article.Content);
                    content = content.Replace("&nbsp; ", "");
                    content = We7Helper.ConvertTextToHtml(content);
                    if (content.Length > 200)
                    {
                        content = content.Substring(0, 200) + "...";
                    }
                    item.Description = content;
                }

                item.PubDate = article.Created.ToString("yyyy-MM-dd");
                l.Add(item);
            }
            channel.Rssitem = l;
        }

        /// <summary>
        /// 格式化地址
        /// </summary>
        /// <param name="article"></param>
        private void FormatArticleUrl(Article article)
        {
            Channel ch = myChannelHelper.GetChannel(article.OwnerID, null);
            if (article.ContentType == (int)TypeOfArticle.LinkArticle)
            {
                article.LinkUrl = article.ContentUrl;
            }
            else
            {
                article.LinkUrl = String.Format("{0}{1}", ch.FullUrl, article.FullUrl);
            }
            article.LinkUrl = String.Format("http://{0}:{1}{2}", Request.Url.Host, Request.Url.Port, article.LinkUrl);
        }

        /// <summary>
        /// 按条件取得文章
        /// </summary>
        /// <returns></returns>
        private List<Article> GetArticles()
        {
            int state = 1;
            if (ShowAtHome != "0")
                state = 100 + state;

            ArticleQuery query = new ArticleQuery();
            query.ChannelID = ColoumnID;
            if (!string.IsNullOrEmpty(ChannelUrl))
            {
                query.ChannelFullUrl = ChannelUrl;
                query.IncludeAllSons = true;
            }

            return ArticleHelper.QueryArtilcesByAll(query, 0, Convert.ToInt32(PageSize), null);
        }

        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        protected ChannelHelper myChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <returns></returns>
        private bool setParam()
        {
            if (Request["ColoumnID"] != null || Request["ChannelUrl"] != null)
            {
                ColoumnID = Request.QueryString["ColoumnID"];
                ChannelUrl = Request.QueryString["ChannelUrl"];
            }
            else
                return false;

            if (Request["PageSize"] != null)
                PageSize = Request.QueryString["PageSize"];

            if (Request["Tag"] != null)
                Tag = Request.QueryString["Tag"];

            if (Request["ShowAtHome"] != null)
                ShowAtHome = Request.QueryString["ShowAtHome"];

            return true;
        }
    }
}
