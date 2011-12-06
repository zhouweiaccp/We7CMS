using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 文章检索类
    /// </summary>
    [Serializable]
    public class ArticleIndex
    {
        string articleID;
        int operation;
        int isLock;

        /// <summary>
        /// 检索的内容的ID（包括文章ID，产品ID等）
        /// </summary>
        public string ArticleID
        {
            get { return articleID; }
            set { articleID = value; }
        }

        /// <summary>
        /// 对检索内容的操作（添加，删除，更新操作）
        /// </summary>
        public int Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        /// <summary>
        /// 是否上锁,0-上锁，1-解锁
        /// </summary>
        public int IsLock
        {
            get { return isLock; }
            set { isLock = value; }
        }
    }
}
