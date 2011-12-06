using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 反馈参与形式
    /// </summary>
    public enum AdviceParticipateMode : int
    {
        /// <summary>
        /// 邮件参与
        /// </summary>
        Mail = 1,

        /// <summary>
        /// 短信通知
        /// </summary>
        SMS = 2,

        /// <summary>
        /// 邮件参与和短信通知
        /// </summary>
        All = 3,
        /// <summary>
        /// 都不参与
        /// </summary>
        None=-1
    }
}