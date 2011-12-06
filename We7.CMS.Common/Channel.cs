using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.CMS.Config;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Common
{
    /// <summary>
    /// 栏目信息类
    /// </summary>
    [Serializable]
    public class Channel:IComparable<Channel>
    {
        public static int MaxLevels = 8;

        string id;
        string parentID;
        string alias;
        string name;
        string description;
        string templateName;
        string detailTemplate;
        int state;
        int index;
        int securityLevel;
        string referenceID;
        string parameter;
        DateTime created=DateTime.Now;
        string fullPath;
        string templateText;
        string detailTemplateText;
        List<Channel> channels;
        string defaultContentID;
        string channelFolder;
        string titleImage;
        string process;
        string type;
        string channelName;
        string refAreaID;
        int isComment;
        string fullUrl;
        string returnUrl;
        private string processLayerNO;
        DateTime updated=DateTime.Now;
        string enumState;
        int articlesCount;
        string tags;
        string keyWord;
        string descriptionKey;
        string ipstrategy;
        bool isOldFullUrl;

        /// <summary>
        /// 栏目类构造函数
        /// </summary>
        public Channel()
        {
            created = DateTime.Now;
            updated = DateTime.Now;
            channels = new List<Channel>();
            securityLevel = 0;
            state = 1;
            type = "0";
            isComment = 0;
            enumState = StateMgr.StateInitialize();
            int enumValue = (int)EnumLibrary.ChannelContentType.Article;
            enumState = StateMgr.StateProcess(enumState, EnumLibrary.Business.ChannelContentType, enumValue);
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 栏目别名
        /// </summary>
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        /// <summary>
        /// 父级栏目ID
        /// </summary>
        public string ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        /// <summary>
        /// 栏目标题，对外显示名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }

        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// 显示全路径，如：/新闻/图片新闻
        /// </summary>
        public string FullPath
        {
            get { return fullPath; }
            set { fullPath = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 详细页模板
        /// </summary>
        public string DetailTemplate
        {
            get { return detailTemplate; }
            set { detailTemplate = value; }
        }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 状态转化字符串
        /// </summary>
        public string StateText
        {
            get
            {
                return State == 0 ? "不可用" : "可用";
            }
        }

        /// <summary>
        /// 安全级别
        /// </summary>
        public int SecurityLevel
        {
            get { return securityLevel; }
            set { securityLevel = value; }
        }

        /// <summary>
        /// 安全级别转化字符串
        /// </summary>
        public string SecurityLevelText
        {
            get
            {
                switch (SecurityLevel)
                {
                    case 1: return "中";
                    case 2: return "高";

                    default:
                    case 0: return "低";
                }
            }
        }

        /// <summary>
        /// 参考信息ID
        /// </summary>
        public string ReferenceID
        {
            get { return referenceID; }
            set { referenceID = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// 模板信息
        /// </summary>
        public string TemplateText
        {
            get { return templateText; }
            set { templateText = value; }
        }

        /// <summary>
        /// 详细模板信息
        /// </summary>
        public string DetailTemplateText
        {
            get { return detailTemplateText; }
            set { detailTemplateText = value; }
        }

        public List<Channel> Channels
        {
            get { return channels; }
            set { channels = value; }
        }

        /// <summary>
        /// 默认详细内容ID
        /// </summary>
        public string DefaultContentID
        {
            get { return defaultContentID; }
            set { defaultContentID = value; }
        }

        /// <summary>
        /// 栏目文件夹
        /// </summary>
        public string ChannelFolder
        {
            get
            {
                return channelFolder;
            }
            set { channelFolder = value; }
        }

        /// <summary>
        /// 标题图片
        /// 例如：/_data/Channels/zwgk_first.jpg
        /// </summary>
        /// </summary>
        public string TitleImage
        {
            get { return titleImage; }
            set { titleImage = value; }
        }

        /// <summary>
        /// 是否走审批流程：1-审批，其他-不审批
        /// </summary>
        public string Process
        {
            get { return process; }
            set { process = value; }
        }

        /// <summary>
        /// 栏目类型
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// IP策略
        /// </summary>
        public string IPStrategy
        {
            get { return ipstrategy; }
            set { ipstrategy = value; }
        }
        /// <summary>
        /// 栏目类型转化字符串
        /// </summary>
        public string TypeText
        {
            get
            {
                switch ((TypeOfChannel)int.Parse(Type))
                {
                    case TypeOfChannel.QuoteChannel:
                        return "专题型";
                    case TypeOfChannel.RssOriginal:
                        return "RSS源";
                    case TypeOfChannel.BlankChannel:
                        return "空节点";
                    case TypeOfChannel.ReturnChannel:
                        return "跳转型";
                    default:
                    case TypeOfChannel.NormalChannel:
                        return "原创型";
                }
            }
        }

        /// <summary>
        /// 频道唯一名称，用于URL
        /// </summary>
        public string ChannelName
        {
            get { return channelName; }
            set { channelName = value; }
        }

        /// <summary>
        /// 栏目来源
        /// </summary>
        public string RefAreaID
        {
            get { return refAreaID; }
            set { refAreaID = value; }
        }

        /// <summary>
        /// 是否评论
        /// </summary>
        public int IsComment
        {
            get { return isComment; }
            set { isComment = value; }
        }


        /// <summary>
        /// 是否返回原来的FullUrl
        /// 默认我false 
        /// 修改WJZ 2010年8月3日
        /// </summary>
        public bool IsOldFullUrl
        {
            get { return isOldFullUrl; }
            set { isOldFullUrl = value; }
        }

        /// <summary>
        /// 内容模型名称
        /// </summary>
        public string ModelName
        { get; set; }

        /// <summary>
        /// 是否评论转化字符串
        /// </summary>

        public string IsCommentText
        {
            get
            {
                switch (IsComment)
                {
                    case 1: return "允许登录用户评论";
                    case 2: return "允许匿名评论";

                    default:
                    case 0: return "不允许评论";
                }
            }
        }

        /// <summary>
        /// thehim 2009-2-26日修改：
        /// 1、FullUrl 为原本栏目的channelname的组合
        /// 2、增加属性 RealUrl 来解决跳转问题，前台菜单控件请使用RealUrl
        /// </summary>
        public string FullUrl
        {
            get
            {
                if (IsOldFullUrl)
                {
                    return fullUrl;
                }
                else
                {

                    if (fullUrl != null)
                    {
                        string cleanUrl = fullUrl;

                        while (cleanUrl.StartsWith("/"))
                            cleanUrl = cleanUrl.Remove(0, 1);

                        if (cleanUrl.EndsWith("/"))
                            cleanUrl = cleanUrl.Remove(cleanUrl.Length - 1);

                        cleanUrl = "/" + cleanUrl + "/";

                        return cleanUrl;
                    }
                    else
                        return fullUrl;
                }
            }
            set { fullUrl = value; }
                
                
        }

        string realUrl;
        /// <summary>
        /// 增加属性 RealUrl 来解决跳转问题，前台菜单控件请使用RealUrl
        /// 1、如是跳转型，返回 ReturnUrl;
        /// 2、如不是，返回 FullUrl
        /// </summary>
        public string RealUrl
        {
            get
            {
                if (FullUrl != null)
                {
                    string cleanUrl = FullUrl;

                    if (Type != null && (TypeOfChannel)int.Parse(Type) == TypeOfChannel.ReturnChannel)
                    {
                        //判断地址中是否有“.”;处理有问题，暂时注释
                        //if (ReturnUrl != null && ReturnUrl.IndexOf(".") != -1)
                        //{
                        //    //判断地址中是否有“http://”
                        //    if (!ReturnUrl.ToLower().StartsWith("http://"))
                        //    {
                        //        ReturnUrl = "http://" + ReturnUrl;
                        //    }
                        //}
                        cleanUrl = ReturnUrl;
                    }
                    else
                    {
                        GeneralConfigInfo si = GeneralConfigs.GetConfig();
                        string ext = si.UrlFormat;
                        if (ext == "aspx") cleanUrl = cleanUrl + "default.aspx";
                    }
                    return cleanUrl;
                }
                else
                    return FullUrl;
            }
            set { realUrl = value; }
        }

        /// <summary>
        /// 完整的栏目文件地址
        /// </summary>
        public string FullFolderPath
        {
            get
            {
                return string.Format("{0}/{1}", ChannelUrlPath, ChannelFolder);
            }
        }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public string ReturnUrl
        {
            get { return returnUrl; }
            set { returnUrl = value; }
        }

        /// <summary>
        /// 保存XML信息
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("Channel");
            xe.SetAttribute("id", ID);
            xe.SetAttribute("parentID", ParentID);
            xe.SetAttribute("alias", Alias);
            xe.SetAttribute("name", Name);
            xe.SetAttribute("description", Description);
            xe.SetAttribute("fullPath", FullPath);
            xe.SetAttribute("template", TemplateName);
            xe.SetAttribute("detailTemplate", DetailTemplate);
            xe.SetAttribute("securityLevel", SecurityLevel.ToString());
            xe.SetAttribute("state", State.ToString());
            xe.SetAttribute("reference", ReferenceID);
            xe.SetAttribute("defaultContentID", DefaultContentID);
            xe.SetAttribute("index", Index.ToString());
            xe.SetAttribute("parameter", Parameter);
            xe.SetAttribute("channelFolder", ChannelFolder);
            xe.SetAttribute("titleImage", TitleImage);
            xe.SetAttribute("process", Process);
            xe.SetAttribute("type", Type);
            xe.SetAttribute("channelName", ChannelName);
            xe.SetAttribute("refAreaID", RefAreaID);
            xe.SetAttribute("isComment", IsComment.ToString());
            xe.SetAttribute("fullUrl", FullUrl);
            xe.SetAttribute("returnUrl", ReturnUrl);
            xe.SetAttribute("processLayerNO", ProcessLayerNO);
            xe.SetAttribute("enumState", EnumState);

            foreach (Channel ch in Channels)
            {
                xe.AppendChild(ch.ToXml(doc));
            }
            return xe;
        }

        /// <summary>
        /// 获取XML数据信息
        /// </summary>
        /// <param name="xe"></param>
        public void FromXml(XmlElement xe)
        {
            Channels.Clear();
            ID = xe.GetAttribute("id");
            ParentID = xe.GetAttribute("parentID");
            Alias = xe.GetAttribute("alias");
            Name = xe.GetAttribute("name");
            Description = xe.GetAttribute("description");
            FullPath = xe.GetAttribute("fullPath");
            TemplateName = xe.GetAttribute("template");
            DetailTemplate = xe.GetAttribute("detailTemplate");
            SecurityLevel = Convert.ToInt16(xe.GetAttribute("securityLevel"));
            State = Convert.ToInt16(xe.GetAttribute("state"));
            ReferenceID = xe.GetAttribute("reference");
            DefaultContentID = xe.GetAttribute("defaultContentID");
            Index = Convert.ToInt32(xe.GetAttribute("index"));
            Parameter = xe.GetAttribute("parameter");
            ChannelFolder = xe.GetAttribute("channelFolder");
            TitleImage = xe.GetAttribute("titleImage");
            Process = xe.GetAttribute("process");
            Type = xe.GetAttribute("type");
            ChannelName = xe.GetAttribute("channelName");
            RefAreaID = xe.GetAttribute("refAreaID");
            IsComment = Convert.ToInt16(xe.GetAttribute("isComment"));
            FullUrl = xe.GetAttribute("fullUrl");
            ReturnUrl = xe.GetAttribute("returnUrl");
            ProcessLayerNO = xe.GetAttribute("processLayerNO");
            EnumState = xe.GetAttribute("enumState");

            foreach (XmlNode node in xe.SelectNodes("Channel"))
            {
                XmlElement el = node as XmlElement;
                if (el != null)
                {
                    Channel ch = new Channel();
                    ch.FromXml(el);
                    Channels.Add(ch);
                }
            }
        }

        //此代码原位于We7.CMS.Common.Utils下的Constants.cs中
        public static string ChannelPath = "_data\\Channels";

        //此代码原位于We7.CMS.Common.Utils下的Constants.cs中
        public static string ChannelUrlPath
        {
            get
            {
                string temp = ChannelPath.Replace("\\", "/");
                if (!temp.StartsWith("/")) temp = "/" + temp;
                //temp = "~" + temp;
                return temp;
            }
        }
        /// <summary>
        /// SEO优化关键字
        /// </summary>
        public string KeyWord
        {
            get { return keyWord; }
            set { keyWord = value; }
        }

        /// <summary>
        /// SEO优化
        /// </summary>
        public string DescriptionKey
        {
            get { return descriptionKey; }
            set { descriptionKey = value; }
        }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tags
        {
            get 
            {
                if (tags == null)
                    return string.Empty;
                else
                    return tags;
            }
            set { tags = value; }
        }

        /// <summary>
        /// 统计该栏目下的文章数
        /// </summary>
        public int ArticlesCount
        {
            get { return articlesCount; }
            set { articlesCount = value; }
        }

        /// <summary>
        /// 栏目状态
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// 审核步骤：1、2、3类，字符串
        /// </summary>
        public string ProcessLayerNO
        {
            get { return processLayerNO; }
            set { processLayerNO = value; }
        }

        /// <summary>
        /// 审核级数
        /// </summary>
        public string ProcessLayerNOText
        {
            get
            {
                switch (ProcessLayerNO)
                {
                    case "1": return "一审";
                    case "2": return "二审";
                    case "3": return "三审";
                    default: return "";
                }
            }
        }

        /// <summary>
        /// 审核完毕动作：0-审结，进入禁用；1-审结后直接启用；2-送跨站审核
        /// </summary>
        public string ProcessEnd { get; set; }

        /// <summary>
        /// 栏目列表页URL
        /// </summary>
        public string ListUrl
        {
            get
            {
                string ext = "aspx";
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si != null) ext = si.UrlFormat;
                if (type == ((int)TypeOfChannel.BlankChannel).ToString())
                {
                    return FullUrl;
                }
                else
                {
                    return FullUrl + "list." + ext;
                }
            }
        }

        /// <summary>
        /// 栏目搜索页URL
        /// </summary>
        public string SearchUrl
        {
            get
            {
                string ext = "aspx";
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si != null) ext = si.UrlFormat;
                return FullUrl + "search." + ext;
            }
        }

        /// <summary>
        ///显示标题（临时属性）
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 本栏目是否被选中（用于判断菜单状态）
        /// </summary>
        public bool MenuIsSelected { get; set; }
        /// <summary>
        /// 是否有子栏目（用于生成菜单状态）
        /// </summary>
        public bool HaveSon { get; set; }
        /// <summary>
        /// 子栏目列表（用于生成菜单树）
        /// </summary>
        public List<Channel> SubChannels { get; set; }
        /// <summary>
        /// 下属文章列表（排在前面的部分）
        /// </summary>
        public List<Article> Articles { get; set; }
        /// <summary>
        /// 生成多级栏目链接
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string BuildLinkHtml(string separator)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(FullPath) && !string.IsNullOrEmpty(FullUrl))
            {
                string[] urls = FullUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string[] names = FullPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string longUrl = "/";
                for (int i = 0; i < names.Length; i++)
                {
                    if (i < urls.Length)
                    {
                        sb.Append(string.Format("<a href='{0}'>{1}</a>", longUrl + urls[i] + "/", names[i]));
                        sb.Append(separator);
                        longUrl += urls[i] + "/";
                    }
                }
                sb.Remove(sb.Length - separator.Length, separator.Length);
            }
            return sb.ToString();
        }

        #region IComparable<Channel> 成员

        /// <summary>
        /// 对栏目排序
        /// </summary>
        public int CompareTo(Channel other)
        {
            return string.Compare(this.FullPath.Replace("//", "/").Replace("/", " 》"), other.FullPath.Replace("//", "/").Replace("/", " 》"), false);
        }

        #endregion
    }
}