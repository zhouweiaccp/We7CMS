using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Model.Core
{
    [Serializable]
    public enum Extend 
    { 
        /// <summary>
        /// 没有扩展
        /// </summary>
        NONE, 
        /// <summary>
        /// 扩展添加
        /// </summary>
        ADD, 
        /// <summary>
        /// 扩展列表
        /// </summary>
        LIST, 
        /// <summary>
        /// 引用
        /// </summary>
        RELATED
    }
}
