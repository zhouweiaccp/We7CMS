using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 用户或角色类别
    /// </summary>
    public enum OwnerRank : int
    {
        /// <summary>
        /// 管理员
        /// </summary>
        Admin =0,
        /// <summary>
        /// 普通用户
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 站群通行证或角色
        /// </summary>
        Group=8,
        /// <summary>
        /// 全部
        /// </summary>
        All=100
    }
}
