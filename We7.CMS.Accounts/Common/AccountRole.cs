using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.PF
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Serializable]
    public class AccountRole
    {
        string id;
        string accountID;
        string roleID;
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;
        string roleTitle;

        /// <summary>
        /// 信息更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// 用户角色
        /// </summary>
        public AccountRole()
        {
            created = DateTime.Now;
        }

        /// <summary>
        /// 信息创建时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 外键用户ID
        /// </summary>
        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        /// <summary>
        /// 外键角色ID
        /// </summary>
        public string RoleID
        {
            get { return roleID; }
            set { roleID = value; }
        }

        /// <summary>
        /// 角色名字
        /// </summary>
        public string RoleTitle
        {
            get { return roleTitle; }
            set { roleTitle = value; }
        }
    }
}
