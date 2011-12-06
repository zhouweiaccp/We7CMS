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
using We7.CMS.Common;
using System.Collections.Generic;
using We7.Framework;
using We7.CMS.WebControls;
using Thinkment.Data;
using We7.CMS.WebControls.Core;

namespace We7.CMS.Web.Widgets
{
    /// <summary>
    /// 文章列表数据提供者
    /// </summary>
    [ControlGroupDescription(Label = "文章详细", Icon = "文章详细", Description = "文章详细", DefaultType = "ArticleView.Default")]
    [ControlDescription(Name = "文章详细", Desc = "文章详细控件")]
    public partial class ArticleView_Default : ThinkmentDataControl
    {
        /// <summary>
        /// 文章业务助手
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        /// <summary>
        /// 栏目业务助手
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// 当前文章
        /// </summary>
        private Article thisArticle;
        /// <summary>
        /// 相关文章
        /// </summary>
        private List<Article> relevantArticles;

        /// <summary>
        /// 上一篇
        /// </summary>
        private Article previousArticle;

        /// <summary>
        ///下一篇 
        /// </summary>
        private Article nextArticle;

        /// <summary>
        /// 当前文章的附件
        /// </summary>
        private List<Attachment> attachments = new List<Attachment>();

        /// <summary>
        /// 相关文章条数
        /// </summary>
        [Parameter(Title = "相关文章条数", Type = "Number", DefaultValue = "3")]
        public int PageSize = 3;

        /// <summary>
        /// 标题长度
        /// </summary>
        [Parameter(Title = "标题长度", Type = "Number", DefaultValue = "30")]
        public int TitleLength = 30;

        /// <summary>
        /// 日期格式
        /// </summary>
        [Parameter(Title = "日期格式", Type = "String", DefaultValue = "[MM-dd]")]
        public string DateFormat = "[MM-dd]";

        /// <summary>
        /// 
        /// </summary>
        [Parameter(Title = "自定义边框样式", Type = "ColorSelector", DefaultValue = "")]
        public string BorderColor;

        /// <summary>
        /// 
        /// </summary>
        [Parameter(Title = "Tags标签", Type = "Tags", DefaultValue = "")]
        public string Tags;

        /// <summary>
        /// 是否显示附件
        /// </summary>
        [Parameter(Title = "是否显示附件", Type = "Boolean", DefaultValue = "1")]
        public bool IsShowAtta;

        protected virtual string BoxBorderColor
        {
            get
            {
                return BorderColor;
            }
        }
        protected string SetBoxBorderColor()
        {
            if (!string.IsNullOrEmpty(BoxBorderColor))
            {
                return string.Format("style=\"border-color:{0};\"", BoxBorderColor);
            }
            return string.Empty;
        }
        /// <summary>
        /// 自定义Css类名称
        /// </summary>
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "ArticleView_Default")]
        public string CssClass;

        /// <summary>
        /// 自定义的css样式
        /// </summary>
        protected virtual string Css
        {
            get
            {
                return CssClass;
            }
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
        /// 获得当前栏目下的第一篇文章
        /// </summary>
        /// <returns></returns>
        protected Article GetThisArticle()
        {
            string id = ChannelHelper.GetChannelIDFromURL();
            Channel ch = ChannelHelper.GetChannel(id, null);

            Criteria c = new Criteria(CriteriaType.Equals, "ChannelFullUrl", ch.FullUrl);
            c.Add(CriteriaType.Equals, "State", 1);
            Order[] os = new Order[] { new Order("Updated", OrderMode.Desc) };
            List<Article> aList = Assistant.List<Article>(c, os, 0, 1);
            if (aList != null && aList.Count > 0)
            {
                return aList[0];
            }
            else
            {
                return new Article();
            }
        }

        /// <summary>
        /// 当前文章
        /// </summary>
        protected Article ThisArticle
        {
            get
            {
                if (thisArticle == null)
                {
                    if (!We7Helper.IsEmptyID(ArticleID))
                    {
                        Criteria c = new Criteria(CriteriaType.Equals, "ID", ArticleID);
                        c.Add(CriteriaType.Equals, "State", 1);
                        Order[] os = new Order[] { new Order("Updated", OrderMode.Desc) };
                        List<Article> aList = Assistant.List<Article>(c, os, 0, 1);
                        if (aList != null && aList.Count > 0)
                        {
                            thisArticle = aList[0];
                        }
                    }
                }
                return thisArticle ?? new Article(); ;
            }
            set
            {
                thisArticle = value;
            }
        }

        /// <summary>
        /// 上一篇
        /// </summary>
        protected Article PreviousArticle
        {
            get
            {
                if (previousArticle == null)
                {
                    Criteria c = new Criteria(CriteriaType.None);
                    c.Add(CriteriaType.Equals, "OwnerID", ThisArticle.OwnerID);
                    c.Add(CriteriaType.MoreThan, "Updated", ThisArticle.Updated);
                    c.Add(CriteriaType.Equals, "State", 1);
                    Order[] os = new Order[] { new Order("Updated", OrderMode.Asc) };
                    List<Article> aList = Assistant.List<Article>(c, os, 0, 1);
                    if (aList != null && aList.Count > 0)
                    {
                        previousArticle = aList[0];
                    }
                }
                return previousArticle;
            }
        }

        /// <summary>
        /// 下一篇
        /// </summary>
        protected Article NextArticle
        {
            get
            {
                if (nextArticle == null)
                {
                    Criteria c = new Criteria(CriteriaType.None);
                    c.Add(CriteriaType.Equals, "OwnerID", ThisArticle.OwnerID);
                    c.Add(CriteriaType.LessThan, "Updated", ThisArticle.Updated);
                    c.Add(CriteriaType.Equals, "State", 1);
                    Order[] os = new Order[] { new Order("Updated", OrderMode.Desc) };
                    List<Article> aList = Assistant.List<Article>(c, os, 0, 1);
                    if (aList != null && aList.Count > 0)
                    {
                        nextArticle = aList[0];
                    }
                }
                return nextArticle;
            }
        }

        /// <summary>
        /// 相关文章
        /// </summary>
        protected List<Article> RelevantArticles
        {
            get
            {
                if (relevantArticles == null)
                {
                    if (!We7Helper.IsEmptyID(ArticleID))
                    {
                        Criteria c = new Criteria(CriteriaType.None);
                        c.Add(CriteriaType.Equals, "OwnerID", ThisArticle.OwnerID);
                        c.Add(CriteriaType.Equals, "State", 1);
                        Order[] os = new Order[] { new Order("Updated", OrderMode.Desc) };
                        List<Article> aList = Assistant.List<Article>(c, os, 0, PageSize);
                        if (aList != null && aList.Count > 0)
                        {
                            relevantArticles = aList;
                        }
                    }
                }
                return relevantArticles;
            }
        }


        /// <summary>
        /// 例子数据
        /// </summary>
        /// <returns></returns>
        private Article GetExampleData()
        {
            Article temp = new Article();
            temp.ID = We7Helper.CreateNewID();
            temp.Title = "测试新闻详细标题";
            temp.SubTitle = "测试新闻详细副标题";
            temp.Content = "核心提示：国家发改委昨天宣布，《商品房销售明码标价规定》已于日前发布，规定要求，从今年5月1日起，商品房销售实行“一套一标价”，并要明确公示代收代办收费和物业服务收费，商品房经营者不得在标价之外加收任何未标明的费用。";
            return temp;
        }

        protected override void OnDesigning()
        {
            ThisArticle = GetExampleData();
        }

        /// <summary>
        /// 获取附件列表
        /// </summary>
        protected List<Attachment> Attachments
        {
            get
            {
                return HelperFactory.GetHelper<AttachmentHelper>().GetAttachments(ThisArticle.ID);
            }
        }
    }
}