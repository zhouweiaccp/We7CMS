using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;

namespace We7.CMS.Common
{
    /// <summary>
    /// 反馈模型
    /// </summary>
    [Serializable]
    public class AdviceType
    {
        private string id;
        private string title;
        private string description;
        private DateTime createDate=DateTime.Now;
        DateTime updated=DateTime.Now;
        string modelXml;
        string modelXmlName;

        string accountID;
        string enumState;
        int toWhichDepartment;
        int flowSeries;
        int flowInnerDepart;
        string mailMode;
        int useSystemMail;
        string mailSMTPServer;
        string pOPServer;
        string mailUser;
        string mailPassword;
        string sMSUser;
        int remindDays;
        string mailAddress;
        int participateMode;
        string stateText;

        public AdviceType()
        {}

        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get{return id;}
            set{id=value;}
        }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// 存放扩展信息XML数据类型
        /// </summary>
        public string ModelXml
        {
            get { return modelXml; }
            set { modelXml = value; }
        }

        /// <summary>
        /// 数据类型名字
        /// </summary>
        public string ModelXmlName
        {
            get { return modelXmlName; }
            set { modelXmlName = value; }
        }

        /// <summary>
        /// 模型创建人
        /// </summary>
        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        /// <summary>
        /// 模式状态（直接办理、转交办理、上报办理）
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        /// <summary>
        /// 转交到办理部门过滤，提供以下一组部门供选择：1、同级部门、2、下级部门；3、所有部门；
        /// </summary>
        public int ToWhichDepartment
        {
            get { return toWhichDepartment; }
            set { toWhichDepartment = value; }
        }

        /// <summary>
        /// 上报审核级数
        /// </summary>
        public int FlowSeries
        {
            get { return flowSeries; }
            set { flowSeries = value; }
        }

        /// <summary>
        /// 审核完毕动作：0-审结，进入禁用；1-审结后直接启用；2-送跨站审核
        /// </summary>
        public string ProcessEnd { get; set; }

        /// <summary>
        /// 是否在部门内审核  0 否 ；1 是
        /// </summary>
        public int FlowInnerDepart
        {
            get { return flowInnerDepart; }
            set { flowInnerDepart = value; }
        }

        /// <summary>
        /// 邮件参与形式
        /// </summary>
        public string MailMode
        {
            get { return mailMode; }
            set { mailMode = value; }
        }

        /// <summary>
        /// 是否使用默认邮件地址 0 默认； 1 专用
        /// </summary>
        public int UseSystemMail
        {
            get { return useSystemMail; }
            set { useSystemMail = value; }
        }

        /// <summary>
        /// 邮件SMTP服务器
        /// </summary>
        public string MailSMTPServer
        {
            get { return mailSMTPServer; }
            set { mailSMTPServer = value; }
        }

        /// <summary>
        /// POP服务器
        /// </summary>
        public string POPServer
        {
            get { return pOPServer; }
            set { pOPServer = value; }
        }

        /// <summary>
        /// 邮箱用户名
        /// </summary>
        public string MailUser
        {
            get { return mailUser; }
            set { mailUser = value; }
        }

        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string MailPassword
        {
            get { return mailPassword; }
            set { mailPassword = value; }
        }

        /// <summary>
        /// 短信通知者
        /// </summary>
        public string SMSUser
        {
            get { return sMSUser; }
            set { sMSUser = value; }
        }

        /// <summary>
        /// 过时自动催办
        /// </summary>
        public int RemindDays
        {
            get { return remindDays; }
            set { remindDays = value; }
        }

        /// <summary>
        /// 此类型绑定邮箱
        /// </summary>
        public string MailAddress
        {
            get { return mailAddress; }
            set { mailAddress = value; }
        }

        /// <summary>
        /// 参与形式：0 是邮件参与；1 是短信通知
        /// </summary>
        public int ParticipateMode
        {
            get { return participateMode; }
            set { participateMode = value; }
        }

        /// <summary>
        ///模型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// 模式状态
        /// </summary>
        public string StateText
        {
            get
            {
                switch (EnumState)
                {
                    case "00000000000000000000": return "直接办理";
                    case "00010000000000000000": return "转交办理";
                    case "00020000000000000000": return "上报办理";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 转交部门
        /// </summary>
        public string ToWhichDepartmentText
        {
            get
            {
                switch ((AdviceToWhichDepartment)ToWhichDepartment)
                {
                    case AdviceToWhichDepartment.LowLevel: return "下级部门";
                    case AdviceToWhichDepartment.Samelevel: return "同级部门";
                    case AdviceToWhichDepartment.All: return "所有部门";
                    default:
                        return "所有部门";
                }
            }
        }
    }
}
