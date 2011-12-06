using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 错误代码类
    /// </summary>
    [Serializable]
    public class ErrorCode
    {
        int id;
        string title;
        string description;
        string helpLink;
        DateTime created=DateTime.Now;
        int level;
        DateTime updated=DateTime.Now;

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ErrorCode()
        {
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 层数
        /// </summary>
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 帮助链接
        /// </summary>
        public string HelpLink
        {
            get { return helpLink; }
            set { helpLink = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
    }
}
