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
using We7.Framework.Util;
using System.Text;

namespace We7.CMS.Web.Widgets
{
    [ControlGroupDescription(Label = "推荐新闻", Icon = "推荐新闻", Description = "推荐新闻", DefaultType = "Recommand.Articles")]
    [ControlDescription("推荐文章")]
    public partial class Recommand_Newest : ThinkmentDataControl
    {
        private List<Article> articles, pictureNews;
        private Channel channel;
        private Article picArticle;

        /// <summary>
        /// 栏目ID
        /// </summary>
        [Parameter(Title = "栏目", Type = "Channel", Required = true)]
        public string OwnerID = String.Empty;

        /// <summary>
        /// 显示记录条数
        /// </summary>
        [Parameter(Title = "控件每页记录", Type = "Number", DefaultValue = "10")]
        public int PageSize = 10;

        /// <summary>
        /// 显示Flash图片记录数
        /// </summary>
        [Parameter(Title = "显示Flash图片记录数", Type = "Number", DefaultValue = "5")]
        public int SliderSize = 5;

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
        /// 是否包含子栏目
        /// </summary>
        [Parameter(Title = "包含子栏目", Type = "Boolean", DefaultValue = "1")]
        public bool IncludeChildren;

        /// <summary>
        /// 上边距10像素
        /// </summary>
        [Parameter(Title = "上边距10像素", Type = "Boolean", DefaultValue = "1")]
        public bool MarginTop10;

        /// <summary>
        /// Tags标签
        /// </summary>
        [Parameter(Title = "Tags标签", Type = "Tags", DefaultValue = "")]
        public string Tags;

        /// <summary>
        /// 自定义图标样式
        /// </summary>
        [Parameter(Title = "自定义图标样式", Type = "CustomImage", DefaultValue = "")]
        public string Icon;

        /// <summary>
        /// 自定义边框样式
        /// </summary>
        [Parameter(Title = "自定义边框样式", Type = "ColorSelector", DefaultValue = "")]
        public string BorderColor;

        /// <summary>
        /// 是否按置顶排序
        /// </summary>
        [Parameter(Title = "是否显示置顶", Type = "Boolean", DefaultValue = "0", Required = true)]
        public bool IsShow;

        /// <summary>
        /// 自定义Css类名称
        /// </summary>
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "Recommand_Newest")]
        public string CssClass;

        /// <summary>
        /// 当前栏目信息
        /// </summary>
        protected Channel Channel
        {
            get
            {
                if (channel == null)
                {
                    ChannelHelper helper = HelperFactory.GetHelper<ChannelHelper>();
                    if (string.IsNullOrEmpty(OwnerID))
                    {
                        OwnerID = helper.GetChannelIDFromURL();
                    }
                    channel = helper.GetChannel(OwnerID, null) ?? new Channel();
                }
                return channel;
            }
        }

        /// <summary>
        /// 自定义图标
        /// </summary>
        protected virtual string CustomIcon
        {
            get
            {
                return Icon;
            }
        }
        protected string BackgroundIcon()
        {
            if (!string.IsNullOrEmpty(CustomIcon))
            {
                return string.Format("style=\"background:url({0}) no-repeat;\"", CustomIcon);
            }
            return string.Empty;
        }


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
        /// 文章列表
        /// </summary>
        protected List<Article> Articles
        {
            get
            {
                if (articles == null)
                {
                    Criteria c = new Criteria(CriteriaType.None);
                    if (IncludeChildren)
                    {
                        c.Add(CriteriaType.Like, "ChannelFullUrl", Channel.FullUrl + "%");
                    }
                    else
                    {
                        c.Add(CriteriaType.Equals, "OwnerID", Channel.ID);
                    }
					c.Add(CriteriaType.Equals,"ModelName","System.Article");
                    c.Add(CriteriaType.Equals, "State", 1);
                    if (!String.IsNullOrEmpty(Tags))
                    {
                        
                        //'1''2''3''12'
                        c.Add(CriteriaType.Like, "Tags", "%'" + Tags + "'%");
                    }
                    Order[] os = IsShow ? new Order[] { new Order("IsShow", OrderMode.Desc), new Order("Updated", OrderMode.Desc) } : new Order[] { new Order("Updated", OrderMode.Desc) };
                    articles = Assistant.List<Article>(c, os, 0, PageSize) ?? new List<Article>();
                }
                return articles;
            }
            set { articles = value; }
        }

        /// <summary>
        /// 图片新闻
        /// </summary>
        protected List<Article> Pictures
        {
            get
            {
                if (pictureNews == null)
                {
                    Criteria c = new Criteria(CriteriaType.Equals, "ModelName", "System.Article");
                    c.Add(CriteriaType.Equals, "IsImage", 1);
                    c.Add(CriteriaType.Equals, "State", 1);
                    if (IncludeChildren)
                    {
                        c.Add(CriteriaType.Like, "ChannelFullUrl", Channel.FullUrl + "%");
                    }
                    else
                    {
                        c.Add(CriteriaType.Equals, "OwnerID", Channel.ID);
                    }
                    if (!String.IsNullOrEmpty(Tags))
                    {
                        c.Add(CriteriaType.Like, "Tags", "%" + Tags + "%");
                    }
                    Order[] os = IsShow ? new Order[] { new Order("IsShow", OrderMode.Desc), new Order("Updated", OrderMode.Desc) } : new Order[] { new Order("Updated", OrderMode.Desc) };
                    int count = HelperFactory.Instance.Assistant.Count<Article>(c);
                    int pageIndex = 0, start, pageitemcount;
                    Utils.BuidlPagerParam(count, PageSize, ref pageIndex, out start, out pageitemcount);
                    pictureNews = HelperFactory.Instance.Assistant.List<Article>(c, os, 0, pageitemcount) ?? new List<Article>();
                }
                return pictureNews;
            }
        }

        /// <summary>
        /// 图片flash
        /// </summary>
        protected string FlashSlideData
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                StringBuilder sb3 = new StringBuilder();
                for (int i = 0; i < Pictures.Count && i < SliderSize; i++)
                {
                    sb.AppendFormat("{0}|", Pictures[i].Thumbnail);
                    sb2.AppendFormat("{0}|", Pictures[i].Url);
                    sb3.AppendFormat("{0}|", Pictures[i].Title);
                }
                Utils.TrimEndStringBuilder(sb, "|");
                Utils.TrimEndStringBuilder(sb2, "|");
                Utils.TrimEndStringBuilder(sb3, "|");
                return "pics=" + sb + "&amp;links=" + sb2 + "&amp;texts=" + sb3;
            }
        }

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

        protected override void OnDesigning()
        {
            Articles = GetExampleData();
        }

        private List<Article> GetExampleData()
        {
            if (Articles.Count > 0)
            {
                return articles;
            }
            else
            {
                List<Article> lsResult = new List<Article>();
                for (int i = 0; i < 6; i++)
                {
                    Article temp = new Article();
                    temp.ID = We7Helper.CreateNewID();
                    temp.Title = "测试新闻" + (i + 1);
                    lsResult.Add(temp);
                }
                return lsResult;
            }
        }
    }
}