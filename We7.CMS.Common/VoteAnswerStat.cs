using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 用户投票统计
    /// </summary>
    [Serializable]
    public class VoteAnswerStat
    {
        public string ID { get; set; }
        /// <summary>
        /// 投票ID
        /// </summary>
        public string VoteID { get; set; }
        /// <summary>
        /// 投票选项文字
        /// </summary>
        public string VoteEntryText { get; set; }
        /// <summary>
        /// 投票选项ID
        /// </summary>
        public string VoteEntryID { get; set; }
        /// <summary>
        /// 投票数量
        /// </summary>
        public int VoteEntrySum { get; set; }
        /// <summary>
        /// 排序用ID
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 投票数量百分比
        /// </summary>
        public int VoteEntrySumPercent { get; set; }
    }
}
