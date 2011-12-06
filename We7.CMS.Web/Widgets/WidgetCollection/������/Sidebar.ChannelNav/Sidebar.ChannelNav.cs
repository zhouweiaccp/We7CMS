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
using We7.Framework;
using We7.CMS.Common;
using System.Collections.Generic;
using Thinkment.Data;
using We7.CMS.WebControls.Core;
using We7.CMS.WebControls;

namespace We7.CMS.Web.Widgets
{
    [ControlGroupDescription(Label = "侧栏导航", Icon = "侧栏导航", Description = "侧栏导航", DefaultType = "Sidebar.ChannelNav")]
    [ControlDescription(Desc = "侧栏导航控件")]
    public partial class Sidebar_ChannelNav : ThinkmentDataControl
    {
        private Channel channel;
        private List<Channel> listChildren;

        /// <summary>
        /// 栏目ID
        /// </summary>
        [Parameter(Title = "栏目", Type = "Channel", Required = true)]
        public string OwnerID = String.Empty;

        /// <summary>
        /// 上边距10像素
        /// </summary>
        [Parameter(Title = "上边距10像素", Type = "Boolean", DefaultValue = "1")]
        public bool MarginTop10;

        /// <summary>
        /// 自定义Css类名称
        /// </summary>
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "Sidebar_ChannelNav")]
        public string CssClass;

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
        /// <summary>
        /// 
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

        /// <summary>
        /// 当前栏目信息
        /// </summary>
        protected Channel Channel
        {
            get
            {
                if (channel == null)
                {
                    ChannelHelper helper = HelperFactory.Instance.GetHelper<ChannelHelper>();
                    if (string.IsNullOrEmpty(OwnerID))
                    {
                        OwnerID = helper.GetChannelIDFromURL();
                    }
                    channel = helper.GetChannel(OwnerID, null);
                    if (GetChildren(channel.ID).Count > 0)
                        listChildren = GetChildren(channel.ID);
                    else
                    {
                        if (channel.ParentID != We7Helper.EmptyGUID)
                        {
                            listChildren = GetChildren(channel.ParentID);
                            channel = helper.GetChannel(channel.ParentID, null);
                        }
                    }

                    return channel;
                }
                return new Channel();
            }
        }

        /// <summary>
        /// 子栏目
        /// </summary>
        protected List<Channel> ChannelChildren
        {
            get
            {
                if (Channel != null)
                    return listChildren;
                return null;
            }
        }

        private List<Channel> GetChildren(string ID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", ID);
            c.Add(CriteriaType.Equals, "State", 1);
            //c.Add(CriteriaType.NotEquals, "ID", Channel.ID);
            return Assistant.List<Channel>(c, new Order[] { new Order("ID") });
        }

        protected string BackgroundIcon()
        {
            if (!string.IsNullOrEmpty(CustomIcon))
            {
                return string.Format("style=\"background:url({0}) no-repeat 8px 8px;\"", CustomIcon);
            }
            return string.Empty;
        }

    }
}