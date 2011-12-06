using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;
using System;
using System.Web;
using We7.Framework;
using We7.CMS.WebControls.Core;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 文章数据提供类属性集合
    /// </summary>
    public partial class ArticleDataProvider
    {
        string bindColumnID;
        /// <summary>
        /// 控件绑定ID
        /// </summary>
        [Option("We7Control", "ArticleList")]
        [Desc("文章列表", "文章列表控件")]
        public string BindColumnID
        {
            get { return bindColumnID; }
            set { bindColumnID = value; }
        }

        /// <summary>
        /// 栏目ID
        /// </summary>
        public string ChannelID
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

        string cssClass;
        /// <summary>
        /// 本控件应用样式
        /// </summary>
        [Option("We7Control", "ArticleList")]
        [Desc("Css", "Css样式")]
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        bool allowPager = false;
        /// <summary>
        /// 是否允许分页（默认不允许）
        /// </summary>
        [Option("We7Control", "ArticleList")]
        [Desc("分页", "是否允许分页")]
        public bool AllowPager
        {
            get { return allowPager; }
            set { allowPager = value; }
        }

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

        string dateFormat = "yyyy-MM-dd";
        /// <summary>
        /// 日期显示格式（不填为“十五分钟前”样式，可以填写“yyyy-MM-dd”格式）
        /// </summary>
        public string DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
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

        bool showBySecurityLevel = false;
        /// <summary>
        /// 是否按栏目权限级别进行文章显示（默认不）
        /// </summary>
        public bool ShowBySecurityLevel
        {
            get { return showBySecurityLevel; }
            set { showBySecurityLevel = value; }
        }

        /// <summary>
        /// 显示附件数量
        /// </summary>
        public int AttachmentNum { get; set; }

        /// <summary>
        /// 附件保护：仅登录用户可以下载
        /// </summary>
        public bool AttachmentProtect { get; set; }

        /// <summary>
        /// 是否可下载
        /// </summary>
        public bool CanDownload
        {
            get
            {
                if (AttachmentProtect)
                    return Request.IsAuthenticated;
                else
                    return true;
            }
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

        int yearFiltrate = 0;
        /// <summary>
        /// 是否显示年份筛选热链（0为不显示年度，填写需要列举的年度数）
        /// </summary>
        public int YearFiltrate
        {
            get { return yearFiltrate; }
            set { yearFiltrate = value; }
        }

        string informationType = "00";
        /// <summary>
        /// 内容信息类型（默认为文章信息）
        /// </summary>
        public string InformationType
        {
            get { return informationType; }
            set
            {
                int temp = 0;
                if (value != "" && value != null)
                {
                    int.TryParse(value, out temp);
                    if (temp != 0)
                    {
                        informationType = value.PadLeft(2, '0');
                    }
                }
            }
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

        HtmlGenericControl pagerDiv;
        /// <summary>
        /// 分页控件所处节点对象
        /// </summary>
        public HtmlGenericControl PagerDiv
        {
            get { return pagerDiv; }
            set { pagerDiv = value; }
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

        /// <summary>
        /// 栏目ID
        /// </summary>
        public string OwnerID
        {
            get { return ChannelHelper.GetChannelIDFromURL(); }
        }

        bool isAllInfomation = false;
        /// <summary>
        /// 是否查询所有(文章和内容模型)
        /// </summary>
        public bool IsAllInfomation
        {
            get { return isAllInfomation; }
            set { isAllInfomation = value; }
        }

        /// <summary>
        /// 通过Url获取查询关键字
        /// </summary>
        public string KeyWord
        {
            get
            {
                if (Request["keyword"] == null || Request["keyword"].ToString() == "")
                    return null;
                else
                    return We7Helper.RemoveHtml(Request["keyword"].ToString());
            }
        }

        private int countCount = 1;
        /// <summary>
        /// 显示多少列
        /// </summary>
        public int ColumnCount
        {
            get { return countCount; }
            set 
            {
                if (value != 0)
                    countCount = value;
                else
                    countCount = 1;
            }
        }

        private int contentMaxLength=50;
        /// <summary>
        /// 内容最大长度
        /// </summary>
        public int ContentMaxLength
        {
            get { return contentMaxLength; }
            set { contentMaxLength = value; }
        }

        private Article thisArticle;
        /// <summary>
        /// 当前的文章
        /// </summary>
        public Article ThisArticle
        {
            get
            {
                if (thisArticle == null)
                {
                    thisArticle = GetThisArticle();
                }
                return thisArticle;
            }
            set { thisArticle = value; }
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
                    thisChannel = new Channel();
                }
                return thisChannel;
            }
            set
            {
                thisChannel = value;
            }
        }

        private List<Article> articles;
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<Article> Articles
        {
            get
            {
                if (articles == null)
                {                    
                    articles = GetArticleListFromDB();
                }
                return articles;
            }
            set { articles = value; }
        }

        /// <summary>
        /// 当前文章的附件列表
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        private Channel currentChannel;
        /// <summary>
        /// 当前栏目
        /// </summary>
        public Channel CurrentChannel
        {
            get
            {
                if (currentChannel == null)
                {

                    string ownerID = String.IsNullOrEmpty(BindColumnID) ? ChannelID : BindColumnID;
                    if (!String.IsNullOrEmpty(ownerID))
                    {
                        if (currentChannel == null)
                            currentChannel = ChannelHelper.GetChannel(ownerID, null);
                    }
                }
                return currentChannel;
            }
        }

        /// <summary>
        /// 栏目名称
        /// </summary>
        public string ChannelName
        {
            get
            {
                return CurrentChannel != null ? CurrentChannel.Name : String.Empty;
            }
        }

        /// <summary>
        /// 当前栏目的Url
        /// </summary>
        public string ChannelUrl
        {
            get { return CurrentChannel != null ? CurrentChannel.RealUrl : String.Empty; }
        }

        /// <summary>
        /// 上下文中的栏目集合
        /// </summary>
        private IDictionary<string, string> ChannelMap
        {
            get
            {                
                if (HttpContext.Current.Items["______ChannelMap___"] == null)
                {
                    HttpContext.Current.Items["______ChannelMap___"] = new Dictionary<string, string>();
                }
                return HttpContext.Current.Items["______ChannelMap___"] as IDictionary<string, string>;
            }
        }
    }
}
