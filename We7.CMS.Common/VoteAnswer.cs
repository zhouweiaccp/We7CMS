using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 投票答案
    /// </summary>
    [Serializable]
    public class VoteAnswer
    {
        public string ID { get; set; }
        /// <summary>
        /// 投票ID
        /// </summary>
        public string VoteID { get; set; }
        /// <summary>
        /// 投票选项ID
        /// </summary>
        public string VoteEntryID { get; set; }
        /// <summary>
        /// 投票答案
        /// </summary>
        public string AnswerText { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string AccountID { get; set; }
        /// <summary>
        /// 用户IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 投票时间
        /// </summary>
        public DateTime VoteDate { get; set; }
    }
}
