using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 评论类
    /// </summary>
    [Serializable]
    public class Comments
    {
        string id;
        string articleID;
        string content;
        string author;
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;
        int state;
        int index;
        string timeNote;
        string ip;
        string accountID;
        string articleName;
        string articleTitle;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Comments()
        {
        }
        /// <summary>
        /// 评论ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 所属文章ID
        /// </summary>
        public string ArticleID
        {
            get { return articleID; }
            set { articleID = value; }
        }
        /// <summary>
        /// 评论作者
        /// </summary>
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        /// <summary>
        /// 评论更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }
        /// <summary>
        /// 评论状态
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// 评论索引
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// 时间节点
        /// </summary>
        public string TimeNote
        {
            get { return timeNote; }
            set { timeNote = value; }
        }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        /// <summary>
        /// 用户ＩＤ
        /// </summary>
        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        /// <summary>
        /// 状态转化字符串
        /// </summary>
        public string AuditText
        {
            get
            {
                switch (State)
                {
                    case 0: return "<font color=red>已屏蔽</font>";
                    default:
                    case 1: return "<font color=green>已启用</font>";
                }
            }
        }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string ArticleTitle
        {
            get { return articleTitle; }
            set { articleTitle = value; }
        }

        /// <summary>
        /// 文章名字
        /// </summary>
        public string ArticleName
        {
            get { return articleName; }
            set { articleName = value; }
        }
    }

}
