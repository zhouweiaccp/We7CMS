using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;

namespace We7.CMS.Common
{
    /// <summary>
    /// 审批中信息实体类
    /// </summary>
    [Serializable]
    public class Processing
    {
        public Processing()
        {
            ProcessTotalLayer = 0;
            ProcessEndAction = ProcessEnding.Stop;
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 对应流转历史记录的编号
        /// </summary>
        public int ItemNum { get; set; }

        /// <summary>
        /// 对象ID
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// 审核总级数
        /// </summary>
        public int ProcessTotalLayer { get; set; }

        /// <summary>
        /// 审核结束后动作
        /// </summary>
        public ProcessEnding ProcessEndAction { get; set; }

        /// <summary>
        /// 当前审核进程级数
        /// </summary>
        public string CurLayerNO { get; set; }

        /// <summary>
        /// 办理方向
        /// </summary>
        public string ProcessDirection { get; set; }

        /// <summary>
        /// 办理用户ID
        /// </summary>
        public string ProcessAccountID { get; set; }

        /// <summary>
        /// 签字项
        /// </summary>
        public string ApproveTitle { get; set; }

        /// <summary>
        /// 签字人名
        /// </summary>
        public string ApproveName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 当前审核状态
        /// </summary>
        public ProcessStates ProcessState
        {
            get
            {
                return (ProcessStates)int.Parse(CurLayerNO);
            }
            set
            {
                CurLayerNO = ((int)value).ToString();
            }
        }

        /// <summary>
        /// 当前审批状态文本描述
        /// </summary>
        public string ProcessText
        {
            get
            {
                if (string.IsNullOrEmpty(CurLayerNO))
                    CurLayerNO = "-100";
                string site = "";
                if (FromOtherSite && !string.IsNullOrEmpty(CurrentSiteName))
                    site = "[" + CurrentSiteName + "]";
                switch ((ProcessStates)int.Parse(CurLayerNO))
                {
                    case ProcessStates.Unaudit: return site + "稿件处理中";
                    case ProcessStates.FirstAudit: return site + "一审中";
                    case ProcessStates.SecondAudit: return site + "二审中";
                    case ProcessStates.ThirdAudit: return site + "三审中";
                    case ProcessStates.EndAudit: return site + "终审完毕";
                    case ProcessStates.WaitAccept: return "受理中";
                    case ProcessStates.WaitHandle: return "办理中";
                    case ProcessStates.Finished: return "已办结";

                    default: return site + "";
                }
            }
        }

        /// <summary>
        /// 审批方向文本描述：退回？交？
        /// </summary>
        public string ProcessDirectionText
        {
            get
            {
                string ret = "";
                if (!string.IsNullOrEmpty(ProcessDirection) && !string.IsNullOrEmpty(CurLayerNO))
                {
                    ProcessAction pa = (ProcessAction)int.Parse(ProcessDirection);
                    if (((ProcessStates)int.Parse(CurLayerNO)) != ProcessStates.EndAudit)
                    {
                        switch (pa)
                        {
                            case ProcessAction.Previous:
                            case ProcessAction.Restart:
                                ret = "退回";
                                break;
                            case ProcessAction.Next:
                            case ProcessAction.SubmitSite:
                                ret = "交";
                                break;
                            default:
                                break;
                        }
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// 当前栏目审批级别关键词描述：Channel.FirstAudit 来源于 CurLayerNO=1
        /// </summary>
        public string CurLayerNOToChannel
        {
            get
            {
                switch (CurLayerNO)
                {
                    case "1": return "Channel.FirstAudit";
                    case "2": return "Channel.SecondAudit";
                     case "3": return "Channel.ThirdAudit";
                    default: return "";

                }
            }
        }

        /// <summary>
        /// 当前反馈审批级别关键词描述：Advice.FirstAudit 来源于 CurLayerNO=1
        /// </summary>
        public string CurLayerNOToAdvice
        {
            get{
                switch (CurLayerNO)
                {
                    case "1": return "Advice.FirstAudit";
                    case "2": return "Advice.SecondAudit";
                    case "3": return "Advice.ThirdAudit";
                    default: return "";
                }
            }
        }

        /// <summary>
        /// 文章起草站点
        /// </summary>
        public string SourceSiteID { get; set; }

        /// <summary>
        /// 起草站点名称
        /// </summary>
        public string SourceSiteName { get; set; }

        /// <summary>
        /// 前一级站点
        /// </summary>
        public string PreviousSiteID { get; set; }

        /// <summary>
        /// 是否来自其他站点
        /// </summary>
        public bool FromOtherSite { get; set; }

        /// <summary>
        /// 跨站审核目标站点
        /// </summary>
        public string TargetSites { get; set; }

        /// <summary>
        /// 跨站审批目标站点ID
        /// </summary>
        public string TargetSiteID { get; set; }

        /// <summary>
        /// 当前站点名称
        /// </summary>
        public string CurrentSiteName { get; set; }

        /// <summary>
        /// 当前站点ID
        /// </summary>
        public string CurrentSiteID { get; set; }

        /// <summary>
        /// 文章状态，对不同站点权限进行了判断
        /// </summary>
        public ArticleStates ArticleState { get; set; }

        /// <summary>
        /// 反馈状态
        /// </summary>
        public AdviceState AdviceState { get; set; }

    }
}
