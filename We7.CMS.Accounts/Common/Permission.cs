using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 授权信息实体类
    /// </summary>
    [Serializable]
    public class Permission
    {
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }

        /// 用户ID或RoleID
        /// </summary>
        public string OwnerID { get; set; }

        /// <summary>
        /// 所属类型：0—帐户；1—角色
        /// </summary>
        public int OwnerType { get; set; }

        /// <summary>
        /// 对象ID
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 栏目Url或菜单Url
        /// </summary>
        public string Url { get; set; }

    }
}
