using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS;
using We7;
using We7.CMS.Controls;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;
using We7.Framework.Cache;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 栏目数据提供类属性集合
    /// </summary>
    public partial class ChannelDataProvider : BaseWebControl
    {
        /// <summary>
        /// 当前的栏目ID
        /// </summary>
        public string ChannelID
        {
            get
            {
                ChannelHelper.GetChannel(ParentID, null);
                return ChannelHelper.GetChannelIDFromURL();
            }
        }

        string parentID;
        /// <summary>
        /// 上级栏目ID
        /// </summary>
        public string ParentID
        {
            get { return ChannelHelper.FormatChannelGUID(parentID); }
            set { parentID = value; }
        }

        string displayParentChannel;
        /// <summary>
        /// 如果本级栏目不存在子栏目是否本级栏目结构
        /// </summary>
        public string DisplayParentChannel
        {
            get { return displayParentChannel; }
            set { displayParentChannel = value; }
        }

        string level;
        /// <summary>
        /// 分级
        /// </summary>
        public string Level
        {
            get { return level; }
            set { level = value; }
        }

        string tag;
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }


        string orderIndex;
        /// <summary>
        /// 排序
        /// </summary>
        public string OrderIndex
        {
            get { return orderIndex; }
            set { orderIndex = value; }
        }

        string imageMenu;
        /// <summary>
        /// 图片菜单
        /// </summary>
        public string ImageMenu
        {
            get { return imageMenu; }
            set { imageMenu = value; }
        }

        string imageFormat;
        /// <summary>
        /// 图片格式
        /// </summary>
        public string ImageFormat
        {
            get { return imageFormat; }
            set { imageFormat = value; }
        }

        int titleMaxLength = 0;
        /// <summary>
        /// 标题最大字数
        /// </summary>
        public int TitleMaxLength
        {
            get { return titleMaxLength; }
            set { titleMaxLength = value; }
        }

        string showToolTip;
        /// <summary>
        /// 鼠标提示
        /// </summary>
        public string ShowToolTip
        {
            get { return showToolTip; }
            set { showToolTip = value; }
        }

        string orderFields;
        /// <summary>
        /// 客户已选择的排序字段项
        /// </summary>
        public string OrderFields
        {
            get { return orderFields; }
            set { orderFields = value; }
        }

        string cssClass;
        /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        int maxTreeClass = 2;
        /// <summary>
        /// 最大菜单层数
        /// </summary>
        public int MaxTreeClass
        {
            get { return maxTreeClass; }
            set { maxTreeClass = value; }
        }


        bool noChildsDisplay;
        /// <summary>
        /// 子栏目附属品
        /// </summary>
        public bool NoChildsDisplay
        {
            get { return noChildsDisplay; }
            set { noChildsDisplay = value; }
        }

        int multiColumn;
        /// <summary>
        /// 多列菜单个数
        /// </summary>
        public int MultiColumn
        {
            get { return multiColumn; }
            set 
            {
                if (value == 0)
                    multiColumn = 1;
                else
                    multiColumn = value; 
            }
        }

        private Channel thisChannel;
        /// <summary>
        /// 当前栏目
        /// </summary>
        public Channel ThisChannel
        {
            get
            {                
                if (thisChannel == null)
                {
                    thisChannel = GetThisChannel() ?? new Channel();
                }
                return thisChannel;
            }
            set { thisChannel = value; }
        }

        /// <summary>
        /// 栏目列表
        /// </summary>
        public List<Channel> Channels { get; set; }

    }
}
