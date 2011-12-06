using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 加密类型 
    /// </summary>
    public enum TypeOfPasswordHashed
    {
        /// <summary>
        /// 没有加密，明码存放。
        /// </summary>
        [Description("不用加密")]
        noneEncrypt = 0,

        /// <summary>
        /// 主站加密方式
        /// </summary>
        [Description("cms加密方式")]
        webEncrypt = 1,

        /// <summary>
        /// 论坛加密方式
        /// </summary>
        [Description("BBS加密方式")]
        bbsEncrypt = 2
    }
}
