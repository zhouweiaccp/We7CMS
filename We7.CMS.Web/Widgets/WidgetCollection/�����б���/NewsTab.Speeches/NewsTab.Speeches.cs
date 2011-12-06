using System;
using System.Collections.Generic;
using System.Web;
using We7.CMS.WebControls;
using We7.CMS.WebControls.Core;
using We7.CMS.Common;
using Thinkment.Data;
using System.Text.RegularExpressions;


namespace We7.CMS.Web.Widgets
{
    [ControlGroupDescription(Label = "新闻Tab选项卡", Icon = "新闻Tab选项卡", Description = "新闻Tab选项卡", DefaultType = "ArticleList.Default")]
    [ControlDescription(Name = "新闻Tab选项卡控件", Desc = "新闻Tab选项卡控件")]
    public partial class NewsTab_Speeches : ThinkmentDataControl
    {
        protected ArticleHelper ArticleHelper
        {
            get
            {
                return HelperFactory.GetHelper<ArticleHelper>();
            }
        }

        #region 栏目
        /// <summary>
        /// 栏目ID1
        /// </summary>
        [Parameter(Title = "栏目一", Type = "Channel", Required = true, Weight = 10)]
        public string OwnerID1 = String.Empty;

        /// <summary>
        /// 栏目ID2
        /// </summary>
        [Parameter(Title = "栏目二", Type = "Channel", Required = true, Weight = 9)]
        public string OwnerID2 = String.Empty;

        /// <summary>
        /// 栏目ID3
        /// </summary>
        [Parameter(Title = "栏目三", Type = "Channel", Required = true, Weight = 8)]
        public string OwnerID3 = String.Empty;

        /// <summary>
        /// 栏目ID4
        /// </summary>
        [Parameter(Title = "栏目四", Type = "Channel", Required = false, Weight = 7)]
        public string OwnerID4 = String.Empty;

        /// <summary>
        /// 栏目ID5
        /// </summary>
        [Parameter(Title = "栏目五", Type = "Channel", Required = false, Weight = 6)]
        public string OwnerID5 = String.Empty;

        /// <summary>
        /// 栏目ID6
        /// </summary>
        [Parameter(Title = "栏目六", Type = "Channel", Required = false, Weight = 5)]
        public string OwnerID6 = String.Empty;

        /// <summary>
        /// 栏目ID7
        /// </summary>
        [Parameter(Title = "栏目七", Type = "Channel", Required = false, Weight = 4)]
        public string OwnerID7 = String.Empty;

        /// <summary>
        /// 栏目ID8
        /// </summary>
        [Parameter(Title = "栏目八", Type = "Channel", Required = false, Weight = 3)]
        public string OwnerID8 = String.Empty;

        /// <summary>
        /// 栏目ID9
        /// </summary>
        [Parameter(Title = "栏目九", Type = "Channel", Required = false, Weight = 2)]
        public string OwnerID9 = String.Empty;

        /// <summary>
        /// 栏目ID10
        /// </summary>
        [Parameter(Title = "栏目十", Type = "Channel", Required = false, Weight = 1)]
        public string OwnerID10 = String.Empty;
        #endregion

        #region 页面信息
        /// <summary>
        /// 显示记录条数
        /// </summary>
        [Parameter(Title = "控件每页记录", Type = "Number", DefaultValue = "10", Required = false)]
        public int PageSize = 10;

        /// <summary>
        /// 标题长度
        /// </summary>
        [Parameter(Title = "标题长度", Type = "Number", DefaultValue = "30")]
        public int TitleLength = 30;

        /// <summary>
        /// 日期格式
        /// </summary>
        [Parameter(Title = "日期格式", Type = "String", DefaultValue = "[MM-dd]")]
        public new string DateFormat = "[MM-dd]";

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
        /// 下边距10像素
        /// </summary>
        [Parameter(Title = "左边距10像素", Type = "Boolean", DefaultValue = "1")]
        public bool MarginLeft10;

        /// <summary>
        /// 自定义Css类名称
        /// </summary>
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "NewsTab_Speeches")]
        public string CssClass;

        /// <summary>
        /// 是否按置顶排序
        /// </summary>
        [Parameter(Title = "是否显示置顶", Type = "Boolean", DefaultValue = "0", Required = true)]
        public bool IsShow;

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
        /// 附加的Css样式
        /// </summary>
        protected string MarginCss
        {
            get { return (MarginTop10 ? " mtop10" : "") + (MarginLeft10 ? " mleft10" : ""); }
        }

        #endregion

        private List<Channel> channellist;

        #region 取得栏目信息
        /// <summary>
        /// 当前栏目信息
        /// </summary>
        protected List<Channel> ChannelList
        {
            get
            {
                if (channellist == null)
                {
                    #region 将非空的栏目ID添加到 ListOwnerID
                    List<string> ListOwnerID = new List<string>(); ;
                    if (!string.IsNullOrEmpty(OwnerID1))
                        ListOwnerID.Add(OwnerID1);
                    if (!string.IsNullOrEmpty(OwnerID2))
                        ListOwnerID.Add(OwnerID2);
                    if (!string.IsNullOrEmpty(OwnerID3))
                        ListOwnerID.Add(OwnerID3);
                    if (!string.IsNullOrEmpty(OwnerID4))
                        ListOwnerID.Add(OwnerID4);
                    if (!string.IsNullOrEmpty(OwnerID5))
                        ListOwnerID.Add(OwnerID5);
                    if (!string.IsNullOrEmpty(OwnerID6))
                        ListOwnerID.Add(OwnerID6);
                    if (!string.IsNullOrEmpty(OwnerID7))
                        ListOwnerID.Add(OwnerID7);
                    if (!string.IsNullOrEmpty(OwnerID8))
                        ListOwnerID.Add(OwnerID8);
                    if (!string.IsNullOrEmpty(OwnerID9))
                        ListOwnerID.Add(OwnerID9);
                    if (!string.IsNullOrEmpty(OwnerID10))
                        ListOwnerID.Add(OwnerID10);
                    #endregion

                    channellist = new List<Channel>();
                    for (int i = 0; i < ListOwnerID.Count; i++)
                    {
                        Criteria c = new Criteria();
                        c.Mode = CriteriaMode.And;
                        c.Add(CriteriaType.Equals, "State", 1);
                        c.Add(CriteriaType.Equals, "ID", ListOwnerID[i]);
                        List<Channel> templist = Assistant.List<Channel>(c, null);
                        if (templist != null || templist.Count > 0)
                        {
                            channellist.Add(templist[0]);
                        }
                    }
                    for (int i = 0; i < channellist.Count; i++)
                    {
                        channellist[i].Articles = QueryArticlesByChannel(channellist[i], true, 0, PageSize);
                    }
                }
                return channellist;
            }
        }

        /// <summary>
        /// 根据栏目获取文章
        /// </summary>
        /// <param name="ch">栏目</param>
        /// <param name="includechildren">包含子栏目</param>
        /// <param name="from"></param>
        /// <param name="PageSize">页大小</param>
        List<Article> QueryArticlesByChannel(Channel ch, bool includechildren, int from, int PageSize)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (includechildren)
            {
                c.Add(CriteriaType.Like, "ChannelFullUrl", ch.FullUrl + "%");
                c.Add(CriteriaType.Equals, "State", 1);
            }
            else
            {
                c.Add(CriteriaType.Equals, "OwnerID", ch.ID);
                c.Add(CriteriaType.Equals, "State", 1);
            }
            if (!String.IsNullOrEmpty(Tags))
            {
                c.Add(CriteriaType.Like, "Tags", "%'" + Tags + "'%");
            }
            Order[] os = IsShow ? new Order[] { new Order("IsShow", OrderMode.Desc), new Order("Updated", OrderMode.Desc) } : new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Article>(c, os, from, PageSize);
        }
        #endregion
    }
}