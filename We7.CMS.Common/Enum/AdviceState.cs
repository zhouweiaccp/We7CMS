using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 反馈状态
    /// </summary>
    public enum AdviceState
    {
        /// <summary>
        /// 全部
        /// </summary>
        All = 0,

        /// <summary>
        /// 待受理
        /// </summary>
        WaitAccept = -1,

        /// <summary>
        /// 待办理
        /// </summary>
        WaitHandle = -3,

        /// <summary>
        /// 审核中
        /// </summary>
        Checking = -4,

        /// <summary>
        /// 办结
        /// </summary>
        Finished = 100
    }
}
