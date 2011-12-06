using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;
using System;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 栏目及下属文章数据提供类属性集合
    /// </summary>
    public partial class ChannelArticlesProvider
    {

        int titleMaxLength;
        /// <summary>
        /// 文章标题显示最大长度（默认30个字符）
        /// </summary>
        public int ArticleTitleMaxLength
        {
            get { return titleMaxLength; }
            set { titleMaxLength = value; }
        }

        string tag = "";
        /// <summary>
        /// 热点文章标签参数（默认为""）
        /// </summary>
        public string HotTag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// 文章列表标签
        /// </summary>
        public string ArticleTag { get; set; }

        int summaryMaxLength;
        /// <summary>
        /// 摘要显示的最大长度（默认200个字符）
        /// </summary>
        public int SummaryMaxLength
        {
            get { return summaryMaxLength; }
            set { summaryMaxLength = value; }
        }

        bool showAtHome = false;
        /// <summary>
        /// 是否显示置顶文章（默认不显示）
        /// </summary>
        public bool ShowAtHome
        {
            get { return showAtHome; }
            set { showAtHome = value; }
        }

        string dateFormat = "yyyy-MM-dd HH:mm";
        /// <summary>
        /// 日期显示格式（不填为“十五分钟前”样式，可以填写“yyyy-MM-dd”格式）
        /// </summary>
        public string DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        string attachmentOneType = "";
        /// <summary>
        /// 文章附件一显示类型（“.doc,.pdf,.txt”，不填写则为不显示附件一）
        /// </summary>
        public string AttachmentOneType
        {
            get { return attachmentOneType; }
            set { attachmentOneType = value; }
        }

        string attachmentTwoType;
        /// <summary>
        /// 文章附件二显示类型（“.doc,.pdf,.txt”，不填写则为不显示附件二）
        /// </summary>
        public string AttachmentTwoType
        {
            get { return attachmentTwoType; }
            set { attachmentTwoType = value; }
        }

        bool showToolTip = false;
        /// <summary>
        /// 鼠标置于链接之上显示完整的标题（默认不）
        /// </summary>
        public bool ShowToolTip
        {
            get { return showToolTip; }
            set { showToolTip = value; }
        }


        bool showIcon = false;
        /// <summary>
        /// 是否显示文章小图标（默认不）
        /// </summary>
        public bool ShowIcon
        {
            get { return showIcon; }
            set { showIcon = value; }
        }

        bool showSubArticle = false;
        /// <summary>
        /// 是否显示子栏目文章（默认不）
        /// </summary>
        public bool ShowSubArticle
        {
            get { return showSubArticle; }
            set { showSubArticle = value; }
        }

        bool noLink = false;
        /// <summary>
        /// 是否只需显示内容，不需要进入详细页链接
        /// </summary>
        public bool NoLink
        {
            get { return noLink; }
            set { noLink = value; }
        }

        string showChannel = "";
        /// <summary>
        /// 是否显示文章所在栏目（默认不，显示则设置为类似于“[{0}]”格式）
        /// </summary>
        public string ShowChannel
        {
            get { return showChannel; }
            set { showChannel = value; }
        }

        bool channelHasLink = false;
        /// <summary>
        /// 频道是否有链接（默认否）
        /// </summary>
        public bool ChannelHasLink
        {
            get { return channelHasLink; }
            set { channelHasLink = value; }
        }

        string linkTarget;
        /// <summary>
        /// 新页面的打开模式（默认弹出新窗口）
        /// </summary>
        public string LinkTarget
        {
            get
            {
                if (linkTarget != null && linkTarget != "")
                    return linkTarget;
                else
                    return "_blank";
            }
            set { linkTarget = value; }
        }

        bool isImage = false;
        /// <summary>
        /// 是否是图片文章
        /// </summary>
        public bool IsImage
        {
            get { return isImage; }
            set { isImage = value; }
        }


        string thumbnailTag;
        /// <summary>
        /// 缩略图标签
        /// </summary>
        public string ThumbnailTag
        {
            get { return thumbnailTag; }
            set { thumbnailTag = value; }
        }

        string attachmentOneName;
        /// <summary>
        /// 附件1名称
        /// </summary>
        public string AttachmentOneName
        {
            get { return attachmentOneName; }
            set { attachmentOneName = value; }
        }


        string attachmentTwoName;
        /// <summary>
        /// 附件2名称
        /// </summary>
        public string AttachmentTwoName
        {
            get { return attachmentTwoName; }
            set { attachmentTwoName = value; }
        }

        private int countCount = 2;
        /// <summary>
        /// 显示多少列
        /// </summary>
        public int ColumnCount
        {
            get { return countCount; }
            set { countCount = value; }
        }
        /// <summary>
        /// 文章显示字段
        /// </summary>
        public string ArticleShowFields { get; set; }
        /// <summary>
        /// 文章排序字段
        /// </summary>
        public string ArticleOrderFields { get; set; }

    }
}
