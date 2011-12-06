using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 短信对象表（短信发送历史记录）
    /// </summary>
    [Serializable]
    public class ShortMessage
    {
        string id;
        string accountID;
        string content;
        string receivers;
        string description;
        DateTime created=DateTime.Now;
        DateTime sendTime;
        int state;
        string success;
        string accountName;
        string passWord;
        DateTime updated=DateTime.Now;


        public ShortMessage()
        {
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 发送人ID
        /// </summary>
        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }
        /// <summary>
        /// 发送人
        /// </summary>
        public string AccountName
        {
            get { return accountName; }
            set { accountName = value; }
        }

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }
        /// <summary>
        /// 接收人
        /// </summary>
        public string Receivers
        {
            get { return receivers; }
            set { receivers = value; }
        }

        /// <summary>
        /// 接收人ID
        /// </summary>
        public string ReceiverID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public DateTime SendTime
        {
            get { return sendTime; }
            set { sendTime = value; }
        }

        public int State
        {
            get { return state; }
            set { state = value; }
        }

        public string Success
        {
            get { return success; }
            set { success = value; }
        }

        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

    }

    public enum MessageState
    {
        /// <summary>
        /// 草稿箱
        /// </summary>
        Draft,
        /// <summary>
        /// 新消息
        /// </summary>
        NewMessage,
        /// <summary>
        /// 收件箱中已读
        /// </summary>
        Inbox,
        /// <summary>
        /// 发件箱存档
        /// </summary>
        Outbox,
        /// <summary>
        /// 手机短信发送失败
        /// </summary>
        Failure,
        /// <summary>
        /// 手机短信发送成功
        /// </summary>
        Success,
        /// <summary>
        /// 所有短信
        /// </summary>
        AllSMS,
        /// <summary>
        /// 未知
        /// </summary>
        Unknown
    }

}
