using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 邮件参与类型
    /// </summary>
    public enum MailMode
    {
        /// <summary>
        /// 新反馈邮件通知受理人
        /// </summary>
        MailNotify = 01,

        /// <summary>
        ///以邮件形式直接转交办理人
        /// </summary>
        HandleByMail = 02,

        /// <summary>
        /// 邮件通知办理人，带链接进入后台办理
        /// </summary>
        MailHyperLink = 03,

        ///// <summary>
        ///// 邮件催办提醒
        ///// </summary>
        //MailRemind = 04,
    }
}