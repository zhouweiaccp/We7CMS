using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;

namespace We7.CMS.Accounts
{
    public class AccountQuery
    {
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 所属站点ID
        /// </summary>
        public string SiteID { get; set; }

        /// <summary>
        /// 所属部门ID
        /// </summary>
        public string DepartmentID { get; set; }

        /// <summary>
        /// 类型：管理员-0？会员-1？
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 用户模型
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 状态：0-禁用，1-启用，2-保留，100-所有
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Email验证:0-待验证；1-验证通过；100-全部
        /// </summary>
        public int EmailValidate { get; set; }

        /// <summary>
        /// 模型状态：分为 0-未设置，1-在申请，2-通过,100-全部
        /// </summary>
        public int ModelState { get; set; }

         /// <summary>
        /// 排序字段请按“Created|Asc,Clicks|Desc”模式传入
        /// </summary>
        public string OrderKeys { get; set; }

        public AccountQuery()
        {
            State = 100;
            EmailValidate = 100;
            ModelState = 100;
            UserType = 100;
        }
    }
}
