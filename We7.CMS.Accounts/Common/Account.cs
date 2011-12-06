using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;
using We7.Framework;
using We7.Framework.Config;
using System.Xml.Serialization;

namespace We7.CMS.Common.PF
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    [Serializable]
    public class Account
    {
        /// <summary>
        /// 类属性
        /// </summary>
        public Account()
        {
            DepartmentID = We7Helper.EmptyGUID;
            UserType = (int)OwnerRank.Admin;
            State = 1;
            PasswordHashed = 1;
            Password = "111111";
            Created = DateTime.Now;
            ModelState = 0;
            FromSiteID = SiteConfigs.GetConfig().SiteID;
            Created = DateTime.Now;
            Updated = DateTime.Now;
            Overdue = DateTime.Today.AddYears(10);
        }

        /// <summary>
        /// 信息主键ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 来源站点ID
        /// </summary>
        public string FromSiteID { get; set; }

        /// <summary>
        /// 外键部门ID
        /// </summary>
        public string DepartmentID { get; set; }

        /// <summary>
        /// 缓存冗余字段：Department -> Name
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 会员登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 备用名称
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 备用名称
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// 用户类别：0—管理员；1—普通用户
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 用户类别显示内容转化
        /// </summary>
        [XmlIgnoreAttribute]
        public string TypeText
        {
            get
            {
                string type = "";
                switch ((OwnerRank)UserType)
                {
                    case OwnerRank.Admin:
                        type = "管理员";
                        break;

                    case OwnerRank.Normal:
                        type = "普通用户";
                        break;

                    default:
                        type = "未分类";
                        break;
                }

                return type;
            }
        }

        /// <summary>
        /// 会员姓名
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 会员邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// QQ号
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 会员序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 会员状态：0-禁用，1-启用，2-保留
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Email验证
        /// </summary>
        public int EmailValidate { get; set; }

        /// <summary>
        /// 信息创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 格式化后的注册时间
        /// </summary>
        [XmlIgnoreAttribute]
        public string CreatedNoteTime
        {
            get
            {
                return We7Helper.FormatTimeNote(Created);
            }
        }

        /// <summary>
        /// 信息描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 状态显示内容转化
        /// </summary>
        [XmlIgnoreAttribute]
        public string StateText
        {
            get { return State == 0 ? "禁用" : "可用"; }
        }

        /// <summary>
        /// 通行证ID
        /// </summary>
        public string Home { get; set; }

        /// <summary>
        /// 密码加密
        /// </summary>
        public int PasswordHashed { get; set; }

        /// <summary>
        /// 设置加密
        /// </summary>
        [XmlIgnoreAttribute]
        public bool IsPasswordHashed
        {
            get { return PasswordHashed > 0; }
            set
            {
                if (value)
                    PasswordHashed = 1;
                else
                    PasswordHashed = 0;
            }
        }


        /// <summary>
        ///会员过期时间
        /// </summary>
        public DateTime Overdue { get; set; }

        /// <summary>
        ///信息更新时间 
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// 用户模型名称，如：个人会员，企业会员
        /// </summary>
        public string UserModelName { get; set; }

        /// <summary>
        /// 用户模型数据
        /// </summary>
        public string ModelXml { get; set; }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 模型配置
        /// </summary>
        public string ModelConfig { get; set; }

        /// <summary>
        /// 模型数据架构
        /// </summary>
        public string ModelSchema { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public int Point { get; set; }
        /// <summary>
        /// 威望
        /// </summary>
        public int Prestige { get; set; }
        /// <summary>
        /// 金钱
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 发表信息总数
        /// </summary>
        public int PublishCount { get; set; }

        /// <summary>
        /// 头像信息
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 模型状态：分为 0-未设置，1-在申请，2-通过
        /// </summary>
        public int ModelState { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

    }
}
