using System;
using System.Collections.Generic;
using System.Text;
//2007-9-3 日志表
namespace We7.CMS.Common
{
    [Serializable]
    public class Log
    {
        string id;
        string userID;
        string content;
        DateTime created=DateTime.Now;
        string page;
        private string remark;
        DateTime updated=DateTime.Now;

        public Log()
        {
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
        /// 日志ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 操作描述
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        /// <summary>
        /// 操作用户ID
        /// </summary>
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        /// <summary>
        /// 操作页面
        /// </summary>
        public string Page
        {
            get { return page; }
            set { page = value; }
        }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }



    }
}
