using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 文章树型结构数据提供者
    /// </summary>
    public class ArticleTreeProvider : BaseWebControl
    {
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<Article> Articles { get; set; }

        /// <summary>
        /// 父文章层级路径
        /// </summary>
        public List<Article> ParentArticlePath { get; set; }

        /// <summary>
        /// 当前栏目
        /// </summary>
        public Channel ThisChannel { get; set; }

         /// <summary>
        /// 控件绑定ID
        /// </summary>
        public string BindColumnID { get; set; }

        /// <summary>
        /// 栏目ID
        /// </summary>
        public string OwnerID
        {
            get { return ChannelHelper.GetChannelIDFromURL(); }
        }

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

         /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

          /// <summary>
        /// 标题最大字数
        /// </summary>
        public int TitleMaxLength { get; set; }

         /// <summary>
        /// 鼠标提示
        /// </summary>
        public string ShowToolTip { get; set; }

        public bool ShowParentName { get; set; }

         /// <summary>
        /// 客户已选择的排序字段项
        /// </summary>
        public string OrderFields { get; set; }

         /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass { get; set; }

        private bool noChildsDisplay = false;
        /// <summary>
        /// 子栏目显示：子栏目为空时，显示上级
        /// </summary>
        public bool NoChildsDisplay 
        {
            get { return noChildsDisplay; }
            set { noChildsDisplay = value; }
        }


        /// <summary>
        /// 文章业务助手
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Articles = GetArticleListFromDB();
            ParentArticlePath = GetParentArticlePath();
        }

        /// <summary>
        /// 构造多级文章当前位置超链接
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string BuildMapPathLinkHtml(string separator)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Article a in ParentArticlePath)
            {
                sb.Append(string.Format("<a href='{0}'>{1}</a>", a.LinkUrl, a.Title));
                sb.Append(separator);
            }
            if(sb.Length>0)
                sb.Remove(sb.Length - separator.Length, separator.Length);
            return sb.ToString();
        }

        /// <summary>
        /// 取得数据列表
        /// </summary>
        /// <returns>文章列表</returns>
        List<Article> GetArticleListFromDB()
        {
            ArticleQuery query = new ArticleQuery();
            List<Article> list;
            if (BindColumnID == string.Empty)
            {
                if (Tag != null && Tag.Length > 0)
                {
                    query.Tag = Tag;
                }
                else
                    query.ChannelID = OwnerID;
            }
            else
            {
                query.ChannelID = BindColumnID;
            }
            Channel ch = ChannelHelper.GetChannel(query.ChannelID, new string[] { "FullFolderPath", "FullUrl" });
            ThisChannel = ch;
            query.ChannelFullUrl = ch != null ? ch.FullUrl : "";
            query.State = ArticleStates.Started;

            if (!String.IsNullOrEmpty(OrderFields))
                query.OrderKeys = OrderFields;
            else
                query.OrderKeys = "Updated|Desc";

            if (!string.IsNullOrEmpty(Tag))
                query.Tag = Tag;

            if (!NoChildsDisplay)
            {
                if (!string.IsNullOrEmpty(ArticleID))
                    query.ArticleParentID = ArticleID;
                else
                    query.ArticleParentID = We7Helper.EmptyGUID;
            }

            RecordCount = ArticleHelper.QueryArtilceCountByAll(query);
            list = ArticleHelper.QueryArtilcesByAll(query, 0, RecordCount, null);
            if(list==null) 
                list= new List<Article>();
            else
                FormatArticlesData(list);
            return list;
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
                if (a.ContentType == (int)TypeOfArticle.LinkArticle)
                {
                    a.LinkUrl = a.ContentUrl;
                }
                else
                {
                    a.LinkUrl = String.Format("{0}{1}", a.ChannelFullUrl, a.FullUrl);
                }

                a.FullTitle = a.Title;

                //if (TitleMaxLength > 0 && a.Title.Length > TitleMaxLength)
                //{
                //    a.Title = a.Title.Substring(0, TitleMaxLength) + "...";
                //}
            }
            return list;
        }

        /// <summary>
        /// 获取父文章路径
        /// </summary>
        /// <returns></returns>
        public List<Article> GetParentArticlePath()
        {
            List<Article> list = new List<Article>();
            if (!string.IsNullOrEmpty(ArticleID))
            {
                Article a=ArticleHelper.GetArticle(ArticleID,null);
                while (a != null )
                {
                    list.Insert(0, a);
                    if (!We7Helper.IsEmptyID(a.ParentID))
                        a = ArticleHelper.GetArticle(a.ParentID, null);
                    else
                        break;
                }
            }
            if(list!=null && list.Count>0 )
                FormatArticlesData(list);
            return list;
        }
    }
}
