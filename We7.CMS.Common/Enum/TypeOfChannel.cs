using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 栏目类型
    /// </summary>
    public enum TypeOfChannel : int
    {
        /// <summary>
        /// 原创型栏目
        /// </summary>
        NormalChannel = 0,

        /// <summary>
        /// 引用型栏目
        /// </summary>
        QuoteChannel = 1,

        /// <summary>
        /// 共享栏目
        /// </summary>
        ShareChannel = 2,

        /// <summary>
        /// 跳转型栏目
        /// </summary>
        ReturnChannel = 3,

        /// <summary>
        /// Rss源
        /// </summary>
        RssOriginal=4,

        /// <summary>
        /// 空节点栏目
        /// </summary>
        BlankChannel=5

    }
}
