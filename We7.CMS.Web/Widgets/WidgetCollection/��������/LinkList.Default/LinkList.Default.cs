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
using We7.CMS.WebControls.Core;
using We7.CMS.WebControls;
using System.Collections.Generic;
using We7.CMS.Common;
using Thinkment.Data;

namespace We7.CMS.Web.Widgets
{
    [ControlGroupDescription(Label = "友情链接", Icon = "友情链接", Description = "友情链接", DefaultType = "LinkList.Default")]
    [ControlDescription(Desc = "友情链接", Author = "系统")]
    public partial class LinkList_Default : ThinkmentDataControl
    {
        /// <summary>
        /// 显示记录条数
        /// </summary>
        [Parameter(Title = "控件每页记录", Type = "Number",DefaultValue="10")]
        public int PageSize = 10;
        /// <summary>
        /// 自定义Css类名称
        /// </summary>
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "LinkList_Default")]
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
        private List<Link> items;

        [Parameter(Title = "标签", Type = "Tags", DefaultValue = "")]
        public string Tag = "";

        /// </summary>
        [Parameter(Title = "自定义图标样式", Type = "CustomImage", DefaultValue = "")]
        public string Icon;

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
        /// 友情链接数据
        /// </summary>
        protected  List<Link> Items
        {
            get
            {
                if (items == null)
                {
                    Criteria c = new Criteria(CriteriaType.None);
                    if (!String.IsNullOrEmpty(Tag))
                    {
                        c.Add(CriteriaType.Equals, "Tag", Tag);
                    }
                    Order[] os = new Order[] { new Order("OrderNumber"), new Order("Updated", OrderMode.Desc) };
                    items = Assistant.List<Link>(c, os, 0, PageSize);
                }
                return items;
            }
            set { items = value; }
        }

        protected override void OnDesigning()
        {
            Items=GetExampleData();
        }
        
        /// <summary>
        /// 例子数据
        /// </summary>
        /// <returns></returns>
        private List<Link> GetExampleData()
        {
            List<Link> lsResult = new List<Link>();
            for (int i = 0; i < 8;i++ )
            {
                Link model = new Link();
                model.ID = We7Helper.CreateNewID();
                model.OrderNumber = i;
                model.Title = "测试友情链接" + i + 1;
                model.Url = "#";
                lsResult.Add(model);
            }
            return lsResult;

        }

    }
}