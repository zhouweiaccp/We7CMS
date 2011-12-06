using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 审核动作
    /// </summary>
    public enum ProcessAction
    {
        /// <summary>
        /// 送审
        /// </summary>
        Next=1,
        /// <summary>
        /// 退回
        /// </summary>
        Previous=0,

        /// <summary>
        /// 退回初稿
        /// </summary>
        Restart=2,

        /// <summary>
        /// 提交站间审核
        /// </summary>
        SubmitSite = 3
    }
}