using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 投票选项
    /// </summary>
    [Serializable]
    public class VoteEntry
    {
        public string ID { get; set; }
        /// <summary>
        /// 投票ID号
        /// </summary>
        public string VoteID { get; set; }
        /// <summary>
        /// 选项内容
        /// </summary>
        public string EntryText { get; set; }
        /// <summary>
        /// 排序用ID
        /// </summary>
        public int OrderID { get; set; }
    }
}
