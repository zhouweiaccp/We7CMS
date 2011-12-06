using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 终审后下一步操作动作
    /// </summary>
    public enum ProcessEnding
    {
        /// <summary>
        /// 进入禁用
        /// </summary>
        Stop = 0,

        /// <summary>
        /// 启用
        /// </summary>
        Start=1,

        /// <summary>
        /// 送跨站审核
        /// </summary>
        SubmitSite=2

    }
}