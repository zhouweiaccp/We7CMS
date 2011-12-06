using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum ProcessStates
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Unaudit = 0,

        /// <summary>
        /// 受理
        /// </summary>
        WaitAccept = -1,

        /// <summary>
        /// 待办理
        /// </summary>
        WaitHandle = -3,

        /// <summary>
        /// 一审
        /// </summary>
        FirstAudit = 1,

        /// <summary>
        /// 二审
        /// </summary>
        SecondAudit = 2,

        /// <summary>
        /// 三审
        /// </summary>
        ThirdAudit = 3,

        /// <summary>
        /// 站间审核
        /// </summary>
        SiteAudit = 8,

        /// <summary>
        /// 审毕，办结
        /// </summary>
        EndAudit = 99,

        /// <summary>
        /// 办结
        /// </summary>
        Finished=100,
        /// <summary>
        /// 未知
        /// </summary>
        Unknown=-100

    }
}