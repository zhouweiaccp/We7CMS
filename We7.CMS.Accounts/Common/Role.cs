using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml.Serialization;
using We7.CMS.Common.Enum;
using We7.Framework.Config;

namespace We7.CMS.Common.PF
{
    /// <summary>
    /// 角色对象类
    /// </summary>
    [Serializable]
    public class Role
    {
        /// <summary>
        /// 类属性
        /// </summary>
        public Role()
        {
            State = "1";
            RoleType = (int)OwnerRank.Normal;
            FromSiteID = SiteConfigs.GetConfig().SiteID;
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }

        public Role(string id, string name, string description,string roletype)
        {
            ID = id; Name = name; Description = description; RoleType =int.Parse(roletype);
            FromSiteID = SiteConfigs.GetConfig().SiteID;
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 来源站点ID
        /// </summary>
        public string FromSiteID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色备注
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 角色创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated { get; set; }
        /// <summary>
        /// 角色状态：启用、禁用
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 角色状态描述
        /// </summary>
        public string StateText
        {
            get
            {
                switch (State)
                {
                    case "1":
                        return "启用";
                    default:
                        return "禁用";
                }
            }
        }

        /// <summary>
        /// 角色类别： 0- 管理员角色 ；1-普通用户角色；
        /// </summary>
        public int RoleType { get; set; }

        /// <summary>
        /// 角色类别描述
        /// </summary>
        public string TypeText
        {
            get
            {
                string type = "";
                switch ((OwnerRank)RoleType)
                {
                    case OwnerRank.Normal:
                        type= "普通用户角色";
                        break;
                    case OwnerRank.Admin:
                        type= "管理员角色";
                        break;
                    default:
                        type= "";
                        break;
                }
                return type;
            }
        }

        /// <summary>
        /// 站群角色
        /// </summary>
        public int GroupRole { get; set; }

        /// <summary>
        /// 是否站群角色
        /// </summary>
        public bool IsGroupRole
        {
            get { return GroupRole > 0; }
            set
            {
                if (value)
                    GroupRole = 1;
                else
                    GroupRole = 0;
            }
        }
    }

}
