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

    [ControlGroupDescription(Label = "图片新闻", Icon = "图片新闻", Description = "图片新闻", DefaultType = "PictureArticleList.Default")]
    [ControlDescription(Desc = "推荐图片文章列表")]
    public partial class PictureArticleList_Default : ThinkmentDataControl
    {
        private List<Article> articles;
        private Channel channel;
        private Article picArticle;

        /// <summary>
        /// 栏目ID
        /// </summary>
        [Parameter(Title = "栏目", Type = "Channel", Required = true)]
        public string OwnerID = String.Empty;

        /// <summary>
        /// 缩略图标签
        /// </summary>
        [Parameter(Title = "缩略图标签", Type = "KeyValueSelector", Data = "thumbnail", DefaultValue = "flash")]
        public string ThumbnailTag = "flash";

        /// <summary>
        /// 显示记录条数
        /// </summary>
        [Parameter(Title = "控件每页记录", Type = "Number", DefaultValue = "4")]
        public int PageSize = 4;

        /// <summary>
        /// 标题长度
        /// </summary>
        [Parameter(Title = "标题长度", Type = "Number", DefaultValue = "30")]
        public int TitleLength = 30;

        /// <summary>
        /// 是否包含子栏目
        /// </summary>
        [Parameter(Title = "包含子栏目", Type = "Boolean", DefaultValue = "1")]
        public bool IncludeChildren;

        /// <summary>
        /// 自定义图标样式
        /// </summary>
        [Parameter(Title = "自定义图标样式", Type = "CustomImage", DefaultValue = "")]
        public string Icon;

        /// <summary>
        /// Tags标签
        /// </summary>
        [Parameter(Title = "Tags标签", Type = "Tags", DefaultValue = "")]
        public string Tags;

        /// <summary>
        /// 自定义Css类名称
        /// </summary>
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "PictureArticleList_Default")]
        public string CssClass;

        /// <summary>
        /// 是否按置顶排序
        /// </summary>
        [Parameter(Title = "是否显示置顶", Type = "Boolean", DefaultValue = "0", Required = true)]
        public bool IsShow;


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
        /// </summary>
        [Parameter(Title = "自定义边框样式", Type = "ColorSelector", DefaultValue = "")]
        public string BorderColor;

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
        /// 自定义的css样式
        /// </summary>
        protected virtual string Css
        {
            get
            {
                return CssClass;
            }
        }
        public List<Article> Articles
        {
            get
            {
                if (articles != null)
                {
                    return articles;
                }

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

                Order[] os = IsShow ? new Order[] { new Order("IsShow", OrderMode.Desc), new Order("Updated", OrderMode.Desc), new Order("ID", OrderMode.Desc) } : new Order[] { new Order("Updated", OrderMode.Desc), new Order("ID", OrderMode.Desc) };                
                int count = HelperFactory.Instance.Assistant.Count<Article>(c);
                int pageIndex = 0, start;
                Utils.BuidlPagerParam(count, PageSize, ref pageIndex, out start, out PageSize);
                articles = HelperFactory.Instance.Assistant.List<Article>(c, os, 0, PageSize) ?? new List<Article>();
                return articles;
            }
        }

        public Channel Channel
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


    }
}