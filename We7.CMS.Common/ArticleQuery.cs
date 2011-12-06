using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;

namespace We7.CMS.Common
{
    /// <summary>
    /// 文章查询类，作为查询时的参数传递
    /// </summary>
    public class ArticleQuery
    {
        public ArticleQuery() { }
        string accountID;
        string channelID;
        bool excludeThisChannel = false;
        string keyWord;
        DateTime beginDate;
        DateTime endDate;
        int articleType = 0;
        ArticleStates currentState = ArticleStates.All;
        bool includeAllSons;
        bool includeAdministrable = false;
        string enumState;
        string author;
        string orderKeys = "Updated|Desc";
        bool orderDesc;
        string isShowHome;
        string listKeys;
        string listKeys2;
        string modelName;

        /// <summary>
        /// 用户ID
        /// </summary>
        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        /// <summary>
        /// 所属栏目ID
        /// </summary>
        public string ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }

        /// <summary>
        /// 所属栏目Url地址
        /// </summary>
        public string ChannelFullUrl { get; set; }

        /// <summary>
        /// 是否排除当前所设Channel
        /// </summary>
        public bool ExcludeThisChannel
        {
            get { return excludeThisChannel; }
            set { excludeThisChannel = value; }
        }

        /// <summary>
        /// 关键词
        /// </summary>
        public string KeyWord
        {
            get { return keyWord; }
            set { keyWord = value; }
        }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        /// <summary>
        /// 文章类型：文本、图片等
        /// </summary>
        public int ArticleType
        {
            get { return articleType; }
            set { articleType = value; }
        }

        /// <summary>
        /// 文章状态：启用、禁用
        /// </summary>
        public ArticleStates State
        {
            get { return currentState; }
            set { currentState = value; }
        }

        /// <summary>
        /// 是否包含子栏目（所有下级子栏目）
        /// </summary>
        public bool IncludeAllSons
        {
            get { return includeAllSons; }
            set { includeAllSons = value; }
        }

        /// <summary>
        /// 是否包含拥有文章管理权限的列表，Channel.Articles
        /// </summary>
        public bool IncludeAdministrable
        {
            get { return includeAdministrable; }
            set { includeAdministrable = value; }
        }

        /// <summary>
        /// 状态位
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        /// <summary>
        /// 排序字段请按“Created|Asc,Clicks|Desc”模式传入
        /// </summary>
        public string OrderKeys
        {
            get { return orderKeys; }
            set { orderKeys = value; }
        }

        /// <summary>
        /// 是否在首页显示，1则在首页显示
        /// </summary>
        public string IsShowHome
        {
            get { return isShowHome; }
            set { isShowHome = value; }
        }
        string tag = "";
        /// <summary>
        /// 过滤标签
        /// </summary>
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        bool overdue = false;
        /// <summary>
        /// 是否验证过期时间
        /// </summary>
        public bool Overdue
        {
            get { return overdue; }
            set { overdue = value; }
        }

        //string searcherKey = "";
        ///// <summary>
        ///// 查询
        ///// </summary>
        //public string SearcherKey
        //{
        //    get { return searcherKey; }
        //    set { searcherKey = value; }
        //}

        string articleID;
        /// <summary>
        /// 内容模型关链外键ID
        /// </summary>
        public string ArticleID
        {
            get { return articleID; }
            set { articleID = value; }
        }

        string isImage;
        /// <summary>
        /// 是否为图片类文章
        /// </summary>
        public string IsImage
        {
            get { return isImage; }
            set { isImage = value; }
        }

        /// <summary>
        /// 省份
        /// </summary>
        public string ListKeys
        {
            get { return listKeys; }
            set { listKeys = value; }
        }
        /// <summary>
        /// 市
        /// </summary>
        public string ListKeys2
        {
            get { return listKeys2; }
            set { listKeys2 = value; }
        }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        /// <summary>
        /// 按模型查询
        /// </summary>
        public bool UseModel { get; set; }

        /// <summary>
        /// 文章父ID
        /// </summary>
        public string ArticleParentID { get; set; }

    }
}
