using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using System.Net;
using We7.Framework;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// RSS数据提供者
    /// </summary>
    public class RssDataProvider : BaseWebControl
    {
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<Article> Articles { get; set; }

        /// <summary>
        /// rss地址
        /// </summary>
        public string RssUrl { get; set; }

        /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass { get; set; }




        /// <summary>
        /// 将RSS列表加载到Article对象列表中
        /// </summary>
        /// <param name="rssURL"></param>
        /// <returns></returns>
        public List<Article> ProcessRSSItem(string rssURL)
        {

            WebRequest myRequest = null;
            WebResponse myResponse = null;
            //注意超时问题，有待优化
            try
            {
                myRequest = System.Net.WebRequest.Create(rssURL);
                myRequest.Timeout = 2 * 1000;
                myResponse = myRequest.GetResponse();
            }
            catch (WebException ex)
            {
                throw ex;
                Logger.Error(ex.Message);
                return new List<Article>();
            }
            System.IO.Stream rssStream = myResponse.GetResponseStream();
            System.Xml.XmlDocument rssDoc = new System.Xml.XmlDocument();
            rssDoc.Load(rssStream);

            System.Xml.XmlNodeList rssItems = rssDoc.SelectNodes("rss/channel/item");

            List<Article> articles = new List<Article>();
            int showRssCounts = rssItems.Count;
            if (PageSize < rssItems.Count && PageSize != 0)
            {
                showRssCounts = PageSize;
            }
            for (int i = 0; i < showRssCounts; i++)
            {
                System.Xml.XmlNode rssDetail;
                Article a = new Article();

                rssDetail = rssItems.Item(i).SelectSingleNode("title");
                if (rssDetail != null)
                {
                    a.Title = rssDetail.InnerText;
                }
                else
                {
                    a.Title = "";
                }

                rssDetail = rssItems.Item(i).SelectSingleNode("link");
                if (rssDetail != null)
                {
                    a.LinkUrl = rssDetail.InnerText;
                }
                else
                {
                    a.LinkUrl = "";
                }

                rssDetail = rssItems.Item(i).SelectSingleNode("description");
                if (rssDetail != null)
                {
                    a.Description = rssDetail.InnerText;
                }
                else
                {
                    a.Description = "";
                }

                rssDetail = rssItems.Item(i).SelectSingleNode("pubDate");
                if (rssDetail != null)
                {
                    a.TimeNote = rssDetail.InnerText;
                }
                else
                {
                    a.TimeNote = "";
                }

                rssDetail = rssItems.Item(i).SelectSingleNode("author");
                if (rssDetail != null)
                {
                    a.Author = rssDetail.InnerText;
                }
                else
                {
                    a.Author = "";
                }

                articles.Add(a);
            }
            myResponse.Close();
            rssStream.Close();
            return articles;

        }

        protected override void OnLoad(EventArgs e)
        {
            //0为显示全部
            base.OnLoad(e);
            Articles = new List<Article>();
            if (DesignHelper.IsDesigning)
            {
                List<Article> result;
                RecordCount = DesignHelper.FillItems(out result, PageSize);
                Articles = result;
            } //if (DesignHelper.IsDesigning) true
            else
            {
                if (!string.IsNullOrEmpty(RssUrl))
                {
                    Articles = ProcessRSSItem(RssUrl);
                }
            } //if (DesignHelper.IsDesigning) false
        }
    }
}