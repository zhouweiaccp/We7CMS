using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 投票
    /// </summary>
    [Serializable]
    public class Vote
    {
        public string ID { get; set; }
        /// <summary>
        /// 投票标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 选项类型
        /// 1 单选
        /// 2 多选
        /// </summary>
        public int OptionType { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 投票人数
        /// </summary>
        public int VotePeoples { get; set; }
        /// <summary>
        /// 是否可重复投
        /// </summary>
        public bool IsCanRepeat { get; set; }

        /// <summary>
        /// 选项类型 文字表示
        /// </summary>
        public string OptionTypeString
        {
            get
            {
                return (OptionType == 1) ? "单选" : "多选";
            }
        }

        /// <summary>
        /// 投票选项
        /// </summary>
        public List<VoteEntry> ListVoteEntrys { get; set; }

        /// <summary>
        /// 投票答案统计
        /// </summary>
        public List<VoteAnswerStat> ListVoteAnswerStats { get; set; }       
    }
}
