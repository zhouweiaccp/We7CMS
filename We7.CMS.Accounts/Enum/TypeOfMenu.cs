using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum TypeOfMenu : int
    {
        /// <summary>
        /// 普通菜单
        /// </summary>
        NormalMenu = 0,

        /// <summary>
        /// 顶部一级菜单
        /// </summary>
        TopMenu = 1,

        /// <summary>
        /// 分组类型
        /// </summary>
        GroupMenu = 2,

        /// <summary>
        /// 引用类型
        /// </summary>
        ReferenceMenu = 3        
    }
}
