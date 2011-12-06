using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 访问统计类
    /// </summary>
    [Serializable]
    public class Statistics
    {
        private string id;
        //private int typeCode;
        private string visitorID;
        private string userName;
        private string articleID;
        private string channelID;
        private string articleName;
        private DateTime visitDate;
        private string url;
        private string visitorIP;
        string timeNote;
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Statistics()
        { }

        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }


        /// <summary>
        /// 用户ID
        /// </summary>
        public string VisitorID
        {
            get { return visitorID; }
            set { visitorID = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleID
        {
            get { return articleID; }
            set { articleID = value; }
        }

        /// <summary>
        /// 栏目ID
        /// </summary>
        public string ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }

        /// <summary>
        /// 文章名
        /// </summary>
        public string ArticleName
        {
            get { return articleName; }
            set { articleName = value; }
        }

        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime VisitDate
        {
            get { return visitDate; }
            set { visitDate = value; }
        }

        /// <summary>
        /// 地址信息
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        /// <summary>
        /// 用户IP
        /// </summary>
        public string VisitorIP
        {
            get { return visitorIP; }
            set { visitorIP = value; }
        }

        /// <summary>
        /// 节点时间
        /// </summary>
        public string TimeNote
        {
            get { return timeNote; }
            set { timeNote = value; }
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
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

    }

    /// <summary>
    /// 访问统计历史类
    /// </summary>
    public class StatisticsHistory : Statistics
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatisticsHistory() { }
    }
}
