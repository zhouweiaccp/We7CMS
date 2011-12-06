using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 母版类型：本页采用哪一种母版
    /// </summary>
    public enum MasterPageMode
    {
        /// <summary>
        /// 没有母版
        /// </summary>
        None, 
        /// <summary>
        /// 全菜单母版
        /// </summary>
        FullMenu,
        /// <summary>
        /// 无菜单母版
        /// </summary>
        NoMenu,
        /// <summary>
        /// 用户或会员专用母版
        /// </summary>
        User,
        /// <summary>
        /// 自定义
        /// </summary>
        Diy
    }
}
