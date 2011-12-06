using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{

    /// <summary>
    /// 反馈回复信息
    /// </summary>
    [Serializable]
    public class AdviceReplyInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 反馈ID
        /// </summary>
        public string AdviceID { get; set; }

        /// <summary>
        /// 处理的用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 处理的标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string MailBody { get; set; }

    }


    #region 以前的数据

    /// <summary>
    /// 反馈回复信息
    /// </summary>
    [Serializable]
    public class AdviceReply
    {
        private string id;
        private string adviceID;
        private string userID;
        private string title;
        private string content;
        private DateTime createDate = DateTime.Now;
        DateTime updated = DateTime.Now;

        string mailBody;
        string enumState;
        string comment;
        string suggest;
        string mailFile;
        string userEmail;

        /// <summary>
        /// 本地邮件文件名称
        /// </summary>
        public string MailFile
        {
            get { return mailFile; }
            set { mailFile = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }
        public AdviceReply()
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
        /// 咨询投诉编号
        /// </summary>
        public string AdviceID
        {
            get { return adviceID; }
            set { adviceID = value; }
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// 回复标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// 备份回复邮件内容
        /// </summary>
        public string MailBody
        {
            get { return mailBody; }
            set { mailBody = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        /// <summary>
        /// 客户满意度评价
        /// </summary>
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        /// <summary>
        /// 反馈意见
        /// </summary>
        public string Suggest
        {
            get { return suggest; }
            set { suggest = value; }
        }

        /// <summary>
        /// 回复邮件接收地址
        /// </summary>
        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }

        string formEmail;
        /// <summary>
        /// 发送邮件地址
        /// </summary>
        public string FormEmail
        {
            get { return formEmail; }
            set { formEmail = value; }
        }
    }

    #endregion
}
