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
using Thinkment.Data;


namespace We7.CMS.WebControls
{
    /// <summary>
    /// 文章最新动态
    /// </summary>
    public class ArticleNewsUpdatesProvider : BaseWebControl
    {
        private ArticleQuery articleHotQuery;
        private Criteria articleNewestCriteria;
        private int hotRecordcount=-1,newsRecordCount = -1;

        /// <summary>
        /// 文章业务助手
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }
        /// <summary>
        /// 附件业务助手
        /// </summary>
        protected AttachmentHelper AttachmentHelper
        {
            get { return HelperFactory.GetHelper<AttachmentHelper>(); }
        }
        ObjectAssistant assistant;
        /// <summary>
        /// 当前Helper的数据访问对象
        /// </summary>
        public ObjectAssistant Assistant
        {
            get
            {
                if (assistant == null)
                {
                    assistant = HelperFactory.Instance.Assistant;
                }
                return assistant;
            }
            set { assistant = value; }
        }

        #region 配置参数
        /// <summary>
        /// 取的文章总条数
        /// </summary>
        public int PageSize;

        /// <summary>
        /// 最热的文章条数
        /// </summary>
        public int HotSize;

        /// <summary>
        /// 最新的天数定义（显示小图标）
        /// </summary>
        public int NewestDays;

        int titleMaxLength;
        /// <summary>
        /// 标题显示最大长度（默认30个字符）
        /// </summary>
        public int TitleMaxLength
        {
            get { return titleMaxLength; }
            set { titleMaxLength = value; }
        }
        string tag = "";
        /// <summary>
        /// 文章标签参数（默认为""）
        /// </summary>
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        #endregion

        #region 其他参数
        /// <summary>
        /// 等于 PageSize - HotSize
        /// </summary>
        private int NewestSize
        {
            get { return PageSize - HotSize; }
        }
        /// <summary>
        /// 时间显示格式
        /// </summary>
        public string DateFormat;
        /// <summary>
        /// 最热记录数
        /// </summary>
        public int HotRecordCount
        {
            get
            {
                if (hotRecordcount == -1)
                {
                    hotRecordcount = ArticleHelper.QueryArtilceCountByAll(CreateHotQuery());                   
                }
                return hotRecordcount;
            }
            set { hotRecordcount = value; }
        }
        /// <summary>
        /// 最新记录数
        /// </summary>
        public int NewsRecordCount
        {
            get
            {
                if (newsRecordCount == -1)
                {
                    newsRecordCount = QueryArtilcesByCriteria(CreateNewestCriteria(HotArticles));
                }
                return newsRecordCount;
            }
            set { newsRecordCount = value; }
        }
        #endregion

        #region 输出
        /// <summary>
        /// 最热文章列表
        /// </summary>
        public List<Article> HotArticles
        {
            get
            {
                if (hotArticles == null)
                {
                    hotArticles = GetHotArticles();
                }
                return hotArticles;
            }
          
        }
        private List<Article> hotArticles;

        /// <summary>
        /// 最新文章列表
        /// </summary>
        public List<Article> NewsArticles
        {
            get
            {
                if (newsArticles == null)
                {
                    newsArticles = GetNewestArticles(HotArticles);
                }
                return newsArticles;
            }

        }
        private List<Article> newsArticles;
        #endregion

        /*------------------------------- 方法 ------------------------------------*/

        #region 最热查询
        /// <summary>
        /// 最热查询
        /// </summary>
        /// <returns></returns>
        private ArticleQuery CreateHotQuery()
        {
            if (articleHotQuery == null)
            {
                articleHotQuery = new ArticleQuery();

                articleHotQuery.State = ArticleStates.Started;
                articleHotQuery.OrderKeys = "Clicks|Desc,Updated|Desc,ID|Asc";
                articleHotQuery.Overdue = true;
                if (!string.IsNullOrEmpty(Tag))
                    articleHotQuery.Tag = Tag;
            }
            return articleHotQuery;
        }

        /// <summary>
        /// 获取最热的文章列表
        /// </summary>
        /// <returns></returns>
        protected List<Article> GetHotArticles()
        {
            List<Article> list = null;
            ArticleQuery query = CreateHotQuery();

            if (HotSize <= 0)
            {
                list = ArticleHelper.QueryArtilcesByAll(query, 0, HotRecordCount, null);
            }
            else
            {
                if (HotRecordCount > HotSize)
                {
                    list = ArticleHelper.QueryArtilcesByAll(query, 0, HotSize, null);
                }
                else
                {
                    list = ArticleHelper.QueryArtilcesByAll(query, 0, HotRecordCount, null);
                }
            }
            FormatArticlesData(list);
            return list;
        }
        #endregion

        #region 最新查询
        /// <summary>
        /// 最新查询
        /// </summary>
        /// <returns></returns>
        private Criteria CreateNewestCriteria(List<Article> listHots)
        {
            if (articleNewestCriteria == null)
            {
                articleNewestCriteria = new Criteria(CriteriaType.Equals, "State", (int)ArticleStates.Started);                

                //tag
                if (!string.IsNullOrEmpty(Tag))
                    articleNewestCriteria.Add(CriteriaType.Like, "Tags", "%"+Tag+"%");

                //排除掉最热的。。。
                //列表必须有元素
                if (listHots.Count > 0)
                {
                    Criteria subA = new Criteria(CriteriaType.None);
                    subA.Mode = CriteriaMode.And;
                    foreach (Article at in listHots)
                        subA.Add(CriteriaType.NotEquals, "ID", at.ID);
                    articleNewestCriteria.Criterias.Add(subA);
                }


                //overdue
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;
                Criteria subChildC1 = new Criteria(CriteriaType.MoreThanEquals, "Overdue", DateTime.Now);
                subC.Criterias.Add(subChildC1);
                Criteria subChildC3 = new Criteria(CriteriaType.LessThanEquals, "Overdue", DateTime.Today.AddYears(-30));
                subC.Criterias.Add(subChildC3);
                articleNewestCriteria.Criterias.Add(subC);
                
            }
            return articleNewestCriteria;
        }

        /// <summary>
        /// 获取最新的文章列表
        /// </summary>
        /// <param name="listHots">要排除的最热查询</param>
        /// <returns></returns>
        protected List<Article> GetNewestArticles(List<Article> listHots)
        {
            List<Article> list = null;
            Criteria query = CreateNewestCriteria(listHots);

            List<Order> orders = new List<Order>();
            Order order = new Order("Updated",OrderMode.Desc);
            Order order2 = new Order("ID", OrderMode.Asc);
            orders.Add(order);
            orders.Add(order2);

            if (NewestSize <= 0)
            {
                list = QueryArtilcesByCriteria(query, orders, 0, NewsRecordCount, null);
            }
            else
            {
                if (NewsRecordCount > NewestSize)
                {
                    list = QueryArtilcesByCriteria(query, orders, 0, NewestSize, null);
                }
                else
                {
                    list = QueryArtilcesByCriteria(query, orders, 0, NewsRecordCount, null);
                }
            }
            FormatArticlesData(list);
            return list;
        }
        #endregion

        #region 输出格式化
        /// <summary>
        /// 格式化列表中的数据
        /// </summary>
        /// <param name="list">文章列表</param>        
        protected List<Article> FormatArticlesData(List<Article> list)
        {
            DateTime now = DateTime.Now;

            foreach (Article a in list)
            {
                //连接地址
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
                a.TimeNote = a.Updated.ToString(DateFormat);
            

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
            }
            return list;
        }

        /// <summary>
        /// 判断某篇文章是否满足最新文章的定义
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool IsNewsArticle(Article article)
        {
            return DateTime.Now < article.Updated.AddDays(NewestDays);
        }
        #endregion

        #region 查询分页方法
        /// <summary>
        /// 根据查询类获取文章列表（分页）
        /// </summary>
        /// <param name="query">查询类</param>
        /// <param name="from">第几条开始</param>
        /// <param name="count">获取条数</param>
        /// <param name="fields">string[]字段列表，null为全部</param>
        /// <returns></returns>
        public List<Article> QueryArtilcesByCriteria(Criteria criteria,List<Order> orders, int from, int count, string[] fields)
        {
            try
            {
                Order[] o = null;
                if (orders != null) o = orders.ToArray();

                return Assistant.List<Article>(criteria, o, from, count, fields);
            }
            catch (Exception ex)
            {
            }
            return new List<Article>();
        }

        /// <summary>
        /// 根据查询类获得文章数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int QueryArtilcesByCriteria(Criteria c)
        {
            return Assistant.Count<Article>(c);
        }
        #endregion
    }
}
