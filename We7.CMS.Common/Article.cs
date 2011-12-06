using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web;
using System.Collections.Specialized;
using We7.CMS.Config;
using We7.CMS.Common.Enum;
using System.Data;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Common
{
    /// <summary>
    /// 文章信息类
    /// </summary>
    [Serializable]
    public class Article : ProcessObject
    {
        /// <summary>
        /// 构建Article对象
        /// </summary>
        public Article()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
            AccountID = We7Helper.EmptyGUID;
            Overdue = DateTime.Now.AddMonths(12);
            ContentType = 2;
            IsDeleted = 0;
            IsImage = 0;
            IsShow = 0;
            State = 1;
            Clicks = 0;
            CommentCount = 0;
            EnumState = StateMgr.StateInitialize();
            int enumValue = (int)EnumLibrary.ArticleType.Article;
            EnumState = StateMgr.StateProcess(EnumState, EnumLibrary.Business.ArticleType, enumValue);
            ProcessState = "0";
            ProcessSiteID = SiteConfigs.GetConfig().SiteID;
            ParentID = We7Helper.EmptyGUID;
            Attachments = new List<Attachment>();
        }

        /// <summary>
        /// 存放modelXml数据（不包含结构）
        /// </summary>
        public string ListKeys { get; set; }

        /// <summary>
        /// 存放modelXml数据（不包含结构）
        /// </summary>
        public string ListKeys1 { get; set; }

        /// <summary>
        /// 存放modelXml数据（不包含结构）
        /// </summary>
        public string ListKeys2 { get; set; }

        /// <summary>
        /// 存放modelXml数据（不包含结构）
        /// </summary>
        public string ListKeys3 { get; set; }

        /// <summary>
        /// 存放modelXml数据（不包含结构）
        /// </summary>
        public string ListKeys4 { get; set; }

        /// <summary>
        /// 存放modelXml数据（不包含结构）
        /// </summary>
        public string ListKeys5 { get; set; }

        /// <summary>
        /// 存放视频代码
        /// </summary>
        public string VideoCode { get; set; }

        /// <summary>
        /// SEO 优化文章标题
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// SEOul优化文章简介
        /// </summary>
        public string DescriptionKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UptoTime { get; set; }

        string tags;
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
        /// 栏目地址
        /// </summary>
        public string ChannelFullUrl { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// 评论总数
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 存放扩展信息XML数据
        /// </summary>
        public string ModelXml { get; set; }

        ///// <summary>
        ///// 状态信息,本属性已过时。请不要再使用
        ///// </summary>
        [Obsolete]
        public string EnumState { get; set; }

        /// <summary>
        /// 共享来源ID
        /// </summary>
        public string FromRowID { get; set; }

        /// <summary>
        /// 共享来源文章地址
        /// </summary>
        public string FromSiteUrl { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Overdue { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 1—为允许评论；0—为不允许评论
        /// </summary>
        public int AllowComments { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 文章父ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 摘要：来自Description，Content
        /// </summary>
        public string Summary
        {
            get
            {
                string s = "";
                if (string.IsNullOrEmpty(Description))
                {
                    string content = We7Helper.RemoveHtml(Content);
                    if (content.Length > 50)
                        s = content.Substring(0, 50) + "...";
                    else
                        s = content;
                }
                else
                    s = Description;

                return s;
            }
        }

        /// <summary>
        /// 文章作者
        /// </summary>
        public string Author { get; set; }

        private string content;
        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content
        {
            get
            {
                return We7Helper.ConvertPageBreakToHtml(content);
            }
            set { this.content = value; }
        }

        /// <summary>
        /// 文章创建时间
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated { get; set; }

        public int state;
        /// <summary>
        /// 启用与禁用：1—启用；0—禁用（停用）；2—审核中；3—过期；4—回收站
        /// </summary>
        public int State
        {
            get
            {
                //if (state == 3)
                //{
                //    if (Overdue > DateTime.Now)
                //    {
                //        return 1;
                //    }
                //}
                return state;
            }
            set { state = value; }
        }

        /// <summary>
        /// 栏目ID
        /// </summary>
        public string OwnerID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string AccountID { get; set; }

        /// <summary>
        /// 连接地址
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 时间节点
        /// </summary>
        public string TimeNote { get; set; }

        /// <summary>
        /// 状态转化名称字符串
        /// </summary>
        public string AuditText
        {
            get
            {
                switch ((ArticleStates)State)
                {
                    case ArticleStates.Started: return "<font color=green>已发布</font>";
                    case ArticleStates.Checking: return "<font color=#aa0>审核中</font>";
                    case ArticleStates.Overdued: return "<font color=#888>已过期</font>";
                    case ArticleStates.Recycled: return "<font color=#009>已删除</font>";
                    default:
                    case ArticleStates.Stopped: return "<font color=red>已停用</font>";
                }
            }
        }

        /// <summary>
        /// 是否有缩略图
        /// </summary>
        public int IsImage { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public int IsShow { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 缩略图存放地址（小缩略图）
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 通过ID生成的url，如e6b4ed25_263c_4dc6_81f8_f7e06c214099.shtml或1008.html
        /// </summary>
        public string FullUrl
        {
            get
            {
                return GenerateArticleUrl(SN, Created, ID);
            }
        }

        /// <summary>
        /// 按照变量生成文章URL
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="create"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GenerateArticleUrl(long sn, DateTime create, string id)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            string ext = si.UrlFormat;
            string snRex = si.ArticleUrlGenerator;
            if (snRex != null && snRex.Trim().Length > 0)
            {
                if (snRex == "0")
                    return sn.ToString() + "." + ext;
                else
                    return create.ToString(snRex) + "-" + sn.ToString() + "." + ext;
            }
            else
                return We7Helper.GUIDToFormatString(id) + "." + ext;
        }

        /// <summary>
        /// 按照变量生成文章URL
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="create"></param>
        /// <param name="id"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public string GenerateArticleUrl(long sn, DateTime create, string id, string modelName)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            string ext = si.UrlFormat;
            if (!String.IsNullOrEmpty(modelName))
            {
                string snRex = si.ArticleUrlGenerator;
                if (snRex != null && snRex.Trim().Length > 0)
                {
                    if (snRex == "0")
                        return sn.ToString() + "." + ext;
                    else
                        return create.ToString(snRex) + "-" + sn.ToString() + "." + ext;
                }
                else
                    return We7Helper.GUIDToFormatString(id) + "." + ext;
            }
            else
            {
                return We7Helper.GUIDToFormatString(id) + "." + ext;
            }

        }

        /// <summary>
        /// 参考TypeOfArticle枚举
        /// </summary>
        public int ContentType { get; set; }

        /// <summary>
        /// 是否引用类型
        /// </summary>
        public bool IsLinkArticle { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public string TypeText
        {
            get
            {
                switch ((TypeOfArticle)ContentType)
                {
                    case TypeOfArticle.LinkArticle:
                        return "引用文章";
                    case TypeOfArticle.ShareArticle:
                        return "共享文章";
                    case TypeOfArticle.WapArticle:
                        return "WAP文章";
                    default:
                    case TypeOfArticle.NormalArticle:
                        return "原创文章";
                }
            }
        }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 模型配置
        /// </summary>
        public string ModelConfig { get; set; }

        /// <summary>
        /// 模型数据架构
        /// </summary>
        public string ModelSchema { get; set; }

        /// <summary>
        /// 保密级别0,公开;0|公开,1|内部,2|秘密,3|机密,4|绝密
        /// </summary>
        public int PrivacyLevel { get; set; }

        /// <summary>
        /// 不同类型图片
        /// </summary>
        public string TypeIcon
        {
            get
            {
                switch ((TypeOfArticle)ContentType)
                {
                    case TypeOfArticle.LinkArticle:
                        return "/admin/images/filetype/link.gif";
                    case TypeOfArticle.ShareArticle:
                        return "/admin/images/filetype/mpg.gif";
                    default:
                    case TypeOfArticle.NormalArticle:
                        return "";
                }
            }
        }

        public string IsShowText
        {
            get
            {
                return IsShow == 1 ? "是" : "否";
            }
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 内容URL
        /// </summary>
        public string ContentUrl { get; set; }

        /// <summary>
        /// 点击数
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// 完整的标题
        /// </summary>
        public string FullTitle { get; set; }


        /// <summary>
        /// 失生前端可用的Url;
        /// </summary>
        public string Url
        {
            get
            {
                return ChannelFullUrl + FullUrl;
            }
        }

        /// <summary>
        /// 大缩略图
        /// </summary>
        public string WapImage
        {
            get
            {
                return GetImageName(Thumbnail, "_W");
            }
        }

        /// <summary>
        /// 文章原始图片
        /// </summary>
        public string OriginalImage
        {
            get
            {
                return GetImageName(Thumbnail, "");
            }
        }

        /// <summary>
        /// 根据标签tag属性取得对应缩略图
        /// 如，Thumbnail["wap"] 为 mysource_wap.jpg
        /// </summary>
        public string TagThumbnail { get; set; }

        /// <summary>
        /// 获取标签图片名称
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string GetTagThumbnail(string tag)
        {
            if (string.IsNullOrEmpty(Thumbnail))
                return string.Empty;
            else
            {
                string ext = Path.GetExtension(Thumbnail);
                string imgName = Path.GetFileNameWithoutExtension(Thumbnail);
                int nameLength = ext.Length + imgName.Length;
                string url = Thumbnail.Substring(0, Thumbnail.Length - nameLength);
                return String.Format("{3}{0}_{1}{2}", imgName, tag, ext, url);
            }
        }

        /// <summary>
        /// 获取图片名称
        /// </summary>
        /// <param name="thumbnailName"></param>
        /// <param name="imgType"></param>
        /// <returns></returns>
        public string GetImageName(string thumbnailName, string imgType)
        {
            string ext = Path.GetExtension(thumbnailName);
            string imgName = Path.GetFileNameWithoutExtension(thumbnailName);

            imgName = imgName.Substring(0, imgName.Length - 2);
            int nameLength = 2 + ext.Length + imgName.Length;
            string url = thumbnailName.Substring(0, thumbnailName.Length - nameLength);

            return String.Format("{3}{0}{1}{2}", imgName, imgType, ext, url);
        }

        /// <summary>
        /// 获取文章的完整url，前台调用请使用此属性，而不是FullUrl
        /// </summary>
        /// <param name="channelUrl">所属栏目url</param>
        /// <returns></returns>
        public string GetFullUrlWithChannel(string channelUrl)
        {
            if (ContentType == (int)TypeOfArticle.LinkArticle)
                return ContentUrl;
            else
                return String.Format("{0}{1}", channelUrl, FullUrl);
        }

        /// <summary>
        /// 相关文章
        /// </summary>
        public List<Article> RelatedArticles { get; set; }

        /// <summary>
        /// 所属栏目的全路径名称显示：如，/新闻/图片新闻
        /// </summary>
        public string FullChannelPath { get; set; }

        /// <summary>
        /// 引用/wap类型的原文章ID
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// 文章流水号
        /// </summary>
        public long SN { get; set; }

        /// <summary>
        /// IP策略
        /// </summary>
        public string IPStrategy { get; set; }

        /// <summary>
        /// 文章附件所在路径：如 /_data/2010/02/25/64a55027_062d_4f78_8c51_aeb6500fdacb/
        /// </summary>
        public string AttachmentUrlPath
        {
            get
            {
                string year = Created.ToString("yyyy");
                string month = Created.ToString("MM");
                string day = Created.ToString("dd");
                string sn = We7Helper.GUIDToFormatString(ID);
                return string.Format("/_data/{0}/{1}/{2}/{3}", year, month, day, sn);
            }
        }

        /// <summary>
        /// 当前文章的附件列表
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// 本篇文章所属站点；（用于站群搜索结果）
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 本篇文章所属站点URL；（用于站群搜索结果）
        /// </summary>
        public string SiteUrl { get; set; }

        public string Photos { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 字体重度
        /// </summary>
        public string FontWeight { get; set; }
        /// <summary>
        /// 字体样式
        /// </summary>
        public string FontStyle { get; set; }

        private string titleStyle;
        public string TitleStyle
        {
            get
            {
                if (String.IsNullOrEmpty(titleStyle))
                {
                    StringBuilder sb = new StringBuilder();
                    if (!String.IsNullOrEmpty(Color))
                    {
                        sb.AppendFormat("color:{0};", Color);
                    }
                    if (!String.IsNullOrEmpty(FontWeight))
                    {
                        sb.AppendFormat("font-weight:{0};", FontWeight);
                    }
                    if (!String.IsNullOrEmpty(FontStyle))
                    {
                        sb.AppendFormat("font-style:{0};", FontStyle);
                    }
                    titleStyle = sb.ToString();
                }
                return titleStyle;
            }
        }

        /// <summary>
        /// 日点击量
        /// </summary>
        public int DayClicks { get; set; }

        /// <summary>
        /// 昨日点击量
        /// </summary>
        public int YesterdayClicks { get; set; }

        /// <summary>
        /// 周点击量
        /// </summary>
        public int WeekClicks { get; set; }

        /// <summary>
        /// 月点击量
        /// </summary>
        public int MonthClicks { get; set; }

        /// <summary>
        /// 季点击量
        /// </summary>
        public int QuarterClicks { get; set; }

        /// <summary>
        /// 年点击量
        /// </summary>
        public int YearClicks { get; set; }

        #region
        [NonSerialized]
        private DataSet dataSet;
        [NonSerialized]
        private DataRow row;

        /// <summary>
        /// 访问模型中的信息
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns>模型数据</returns>
        public object this[string name]
        {
            get
            {
                return Row != null && Row.Table.Columns.Contains(name) ? Row[name] : null;
            }
        }

        private DataRow Row
        {
            get
            {
                if (row == null)
                {
                    row = DataSet != null && DataSet.Tables.Count > 0 && DataSet.Tables[0].Rows.Count > 0 ? DataSet.Tables[0].Rows[0] : null;
                }
                return row;
            }
        }

        private DataSet DataSet
        {
            get
            {
                if (dataSet == null)
                {
                    dataSet = CreateDataSet();
                    if (dataSet != null)
                    {
                        using (StringReader reader = new StringReader(ModelXml))
                        {
                            dataSet.ReadXml(reader);
                        }
                    }
                }
                return dataSet;
            }
        }

        DataSet CreateDataSet()
        {
            if (!String.IsNullOrEmpty(ModelSchema))
            {
                DataSet ds = new DataSet();
                using (StringReader reader = new StringReader(ModelSchema))
                {
                    ds.ReadXmlSchema(reader);
                }
                return ds;
            }
            return null;
        }
        #endregion

    }

    /// <summary>
    /// 相关文章类
    /// </summary>
    [Serializable]
    public class RelatedArticle
    {

        public RelatedArticle() { }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleID { get; set; }
        /// <summary>
        /// 关联文章ID
        /// </summary>
        public string RelatedID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated { get; set; }

    }
    /// <summary>
    /// 文章统计类
    /// </summary>
    public class StatisticsArticle
    {
        /// <summary>
        /// 总文章数
        /// </summary>
        public int TotalArticles { get; set; }
        /// <summary>
        /// 总评论数
        /// </summary>
        public int TotalComments { get; set; }
        /// <summary>
        /// 本月发表文章数
        /// </summary>
        public int MonthArticles { get; set; }

        /// <summary>
        /// //本月评论
        /// </summary>
        public int MonthComments { get; set; }
        /// <summary>
        /// 本周文章数
        /// </summary>
        public int WeekArticles { get; set; }
        /// <summary>
        /// 本周评论
        /// </summary>
        public int WeekComments { get; set; }

    }

    /// <summary>
    /// 缩略图配置信息类
    /// </summary>
    public class ThumbnailConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 标签-标识，用于文件名的构造
        /// </summary>
        public string Tag { get; set; }

    }
}
