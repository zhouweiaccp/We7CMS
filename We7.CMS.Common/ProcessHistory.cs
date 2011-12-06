using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;
using We7.CMS.Config;
using We7.Framework.Config;

namespace We7.CMS.Common
{
    /// <summary>
    /// 审批历史记录项
    /// </summary>
    [Serializable]
    public class ProcessHistory
    {
        public ProcessHistory()
        {
            CreateDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 当前栏目、文章或反馈的ID
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// 当前审批进程层数，等于上一环节
        /// </summary>
        public string FromProcessState { get; set; }

        /// <summary>
        /// 审核方向/动作
        /// </summary>
        public string ProcessDirection { get; set; }

        /// <summary>
        /// 下一审批环节层级
        /// </summary>
        public string ToProcessState { get; set; }

        /// <summary>
        /// 跨站审核目标站点
        /// </summary>
        public string TargetSites { get; set; }

        /// <summary>
        /// 操作用户ID
        /// </summary>
        public string ProcessAccountID { get; set; }

        /// <summary>
        /// 签字项
        /// </summary>
        public string ApproveTitle { get; set; }

        /// <summary>
        /// 签字人名称
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
        /// 下一环节审核文字描述
        /// </summary>
        public string ProcessText
        {
            get
            {
                string site = "";
                if (!string.IsNullOrEmpty(TargetSites) && SiteConfigs.GetConfig().SiteGroupEnabled)
                    site = "站点[" + TargetSites + "]";

                switch ((ProcessStates)int.Parse(ToProcessState))
                {
                    case ProcessStates.FirstAudit: return site + "一审";
                    case ProcessStates.SecondAudit: return site + "二审";
                    case ProcessStates.ThirdAudit: return site + "三审";
                    case ProcessStates.EndAudit: return site + "终审完毕";
                    case ProcessStates.WaitAccept: return site + "受理";
                    case ProcessStates.WaitHandle: return site + "办理";

                    default: return site + "编辑";
                }
            }
        }

        /// <summary>
        /// 当前环节文字描述，等于上一步骤的任务
        /// </summary>
        public string ProcessingText 
        {
            get
            {
                return GetProcessingText(FromProcessState);
            }
        }

        /// <summary>
        /// 状态转换为描述
        /// </summary>
        /// <param name="layno"></param>
        /// <returns></returns>
        public string GetProcessingText(string layno)
        {
            string site = "";
            if (!string.IsNullOrEmpty(SiteName) && SiteConfigs.GetConfig().SiteGroupEnabled)
                site = "站点[" + SiteName + "]";
            switch (layno)
            {
                case "-1": return site + "受理中";
                case "-3": return site + "办理中";
                case "0": return site + "稿件处理中";
                case "1": return site + "一审中";
                case "2": return site + "二审中";
                case "3": return site + "三审中";
                default: return site + "稿件处理中";
            }
        }

        /// <summary>
        /// 权限级别
        /// </summary>
        public string CurLayerNOToChannel
        {
            get
            {
                switch (ToProcessState)
                {
                    case "1": return "Channel.FirstAudit";
                    case "2": return "Channel.SecondAudit";
                    case "3": return "Channel.ThirdAudit";
                    default: return "";
                }
            }
        }

        /// <summary>
        /// 操作方向字符转换
        /// </summary>
        public string ProcessDirectionText
        {
            get
            {
                string ret = "";
                if (string.IsNullOrEmpty(ToProcessState)) ToProcessState = "0";
                ProcessAction pa = (ProcessAction)int.Parse(ProcessDirection);
                if (((ProcessStates)int.Parse(ToProcessState)) != ProcessStates.EndAudit)
                {
                    switch (pa)
                    {
                        case ProcessAction.Previous:
                        case ProcessAction.Restart:
                            ret= "退回";
                            break;
                        case ProcessAction.Next:
                        case ProcessAction.SubmitSite:
                            ret= "交";
                            break;
                        default:
                            break;
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// 当前审批环节起点站点名称
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 当前审批环节起点站点ID
        /// </summary>
        public string SiteID { get; set; }

        /// <summary>
        /// 当前审批环节起点站点应用接口地址
        /// </summary>
        public string SiteApiUrl { get; set; }

        /// <summary>
        /// 文章所属栏目ID
        /// </summary>
        public string ChannelID { get; set; }

        /// <summary>
        /// 文章所属栏目名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 步骤序号
        /// </summary>
        public int ItemNum { get; set; }

    }
}

