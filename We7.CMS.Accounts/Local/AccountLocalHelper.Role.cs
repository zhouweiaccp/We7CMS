using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.PF;
using We7.Framework;
using We7.Framework.Common;
using We7.CMS.Common.Enum;
using Thinkment.Data;
using System.Web;
using We7.CMS.Common;

namespace We7.CMS.Accounts
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public partial class AccountLocalHelper : BaseHelper, IAccountHelper
    {

        #region 角色列表

        /// <summary>
        /// 获取一组角色
        /// </summary>
        /// <param name="type">角色的类型，管理员、普通用户等</param>
        /// <param name="key">角色名称搜索</param>
        /// <returns>角色数组</returns>
        public List<Role> GetRoles(string siteID, OwnerRank type, string key)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (type != OwnerRank.All)
                c.Add(CriteriaType.Equals, "RoleType", (int)type);
            if (!string.IsNullOrEmpty(siteID))
                c.Add(CriteriaType.Equals, "FromSiteID", siteID);
            if (!string.IsNullOrEmpty(key))
                c.Add(CriteriaType.Like, "Name", "%" + key + "%");

            return Assistant.List<Role>(c, null);
        }


        /// <summary>
        /// 获取一个类型的角色数目
        /// </summary>
        /// <param name="type">角色的类型</param>
        /// <returns>角色数目</returns>
        public int GetRoleCount(string siteID, OwnerRank type)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (type != OwnerRank.All)
                c.Add(CriteriaType.Equals, "RoleType", (int)type);
            if (!string.IsNullOrEmpty(siteID))
                c.Add(CriteriaType.Equals, "FromSiteID", siteID);
            return Assistant.Count<Role>(c);
        }

        /// <summary>
        /// 获取一组角色
        /// </summary>
        /// <param name="from">记录开始值</param>
        /// <param name="count">返回的记录数</param>
        /// <returns>角色列表</returns>
        public List<Role> GetRoles(string siteID, int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (!string.IsNullOrEmpty(siteID))
                c.Add(CriteriaType.Equals, "FromSiteID", siteID);

            Order[] o = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Role>(c, o, from, count);
        }

        /// <summary>
        /// 通过角色ID获取一个角色
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>角色对象</returns>
        public Role GetRole(string roleID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", roleID);
            List<Role> roles = Assistant.List<Role>(c, null);
            if (roles.Count > 0)
            {
                return roles[0];
            }
            return null;
        }

        /// <summary>
        /// 通过角色名称获取一个角色
        /// </summary>
        /// <param name="title">角色名字</param>
        /// <returns>角色对象</returns>
        public Role GetRoleBytitle(string title)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Name", title);
            List<Role> roles = Assistant.List<Role>(c, null);
            if (roles.Count > 0)
            {
                return roles[0];
            }
            return null;
        }

        #endregion

        #region 添加角色

        /// <summary>
        /// 添加一个角色
        /// </summary>
        /// <param name="roleID">指定的角色ID</param>
        /// <param name="name">角色名称</param>
        /// <param name="desc">角色描述</param>
        /// <param name="type">角色类型</param>
        /// <returns>一个角色信息</returns>
        public Role AddRole(Role role)
        {
            if(string.IsNullOrEmpty(role.ID))
                role.ID = Guid.NewGuid().ToString(); ;
            role.Created = DateTime.Now;
            Assistant.Insert(role);
            return role;
        }

        #endregion

        #region 更新删除角色
        /// <summary>
        /// 更新一个角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="name">角色名称</param>
        /// <param name="desc">角色描述</param>
        /// <param name="type">角色类型</param>
        public void UpdateRole(Role role)
        {
            Assistant.Update(role, new string[] { "Name", "Description", "RoleType" });
        }

        /// <summary>
        /// 删除角色（同时删除权限）
        /// </summary>
        /// <param name="roleID">角色ID</param>
        public void DeleteRole(string roleID)
        {
            We7Helper.AssertNotNull(Assistant, "AccountHelper.Assistant");
            We7Helper.AssertNotNull(roleID, "DeleteRole.roleID");

            //利用事务进行帐户的相关权限删除
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            //IConnection ic = Assistant.GetConnections()[db];请注意别用此获去IC
            IConnection ic = db.DbDriver.CreateConnection(db.ConnectionString);
            ic.IsTransaction = true;

            try
            {
                //删除Permissions
                Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", roleID);
                try
                {
                    Assistant.DeleteList<Permission>(ic, c);
                }
                catch (Exception)
                {
                }

                //删除AccountRole
                Criteria cr = new Criteria(CriteriaType.Equals, "RoleID", roleID);
                try
                {
                    Assistant.DeleteList<AccountRole>(ic, cr);
                }
                catch (Exception)
                {
                }

                //最后删除当前角色
                Role r = new Role();
                r.ID = roleID;
                Assistant.Delete(ic, r);

                ic.Commit();
            }
            catch (Exception ex)
            {
                ic.Rollback();
                throw ex;
            }
            finally { ic.Dispose(); }
        }

        #endregion

        #region 用户角色关联关系
        /// <summary>
        /// 新建一个角色和用户关系的对象
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <param name="roleID">角色ID</param>
        public bool AssignAccountRole(string accountID, string roleID)
        {
            List<AccountRole> roles = this.GetAccountRoles(accountID);
            if (roles != null)
            {
                foreach (AccountRole role in roles)
                {
                    if (role.RoleID == roleID)
                    {
                        return false;
                    }
                }
            }
            AccountRole ar = new AccountRole();
            ar.AccountID = accountID;
            ar.Created = DateTime.Now;
            ar.RoleID = roleID;
            ar.ID = We7Helper.CreateNewID();
            Assistant.Insert(ar);
            return true;
        }
        /// <summary>
        /// 删除一个用户的角色关联
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <param name="roleID">角色ID</param>
        public void UnassignAccountRole(string accountID, string roleID)
        {
            List<AccountRole> roles = this.GetAccountRoles(accountID);
            foreach (AccountRole role in roles)
            {
                if (role.RoleID == roleID)
                {
                    Assistant.Delete(role);
                    return;
                }
            }
        }

        /// <summary>
        /// 删除用户角色关联表记录
        /// </summary>
        /// <param name="id"></param>
        public void DeleteAccountRole(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            Assistant.DeleteList<AccountRole>(c);
        }

        /// <summary>
        /// 通过用户ID查找此用户的用户加入的角色
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <returns>用户角色关联对象组</returns>
        List<AccountRole> GetAccountRoles(string accountID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AccountID", accountID);
            return Assistant.List<AccountRole>(c, null);
        }

        /// <summary>
        /// 通过用户ID插入一组用户角色关联对象
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <param name="roles">角色对象组</param>
        public void UpdateAccountRoles(string accountID, string[] roles)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AccountID", accountID);
            Assistant.DeleteList<AccountRole>(c);

            foreach (string r in roles)
            {
                if (r != String.Empty)
                {
                    AccountRole ar = new AccountRole();
                    ar.AccountID = accountID;
                    ar.Created = DateTime.Now;
                    ar.ID = We7Helper.CreateNewID();
                    ar.RoleID = r;
                    Assistant.Insert(ar);
                }
            }
        }

        /// <summary>
        /// 获取用户的角色与用户集合
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <returns>角色与用户集合</returns>
        public List<string> GetRolesOfAccount(string accountID)
        {
            List<string> allOwners = new List<string>();
            List<AccountRole> accountRoles = GetAccountRoles(accountID);
            foreach (AccountRole ar in accountRoles)
            {
                allOwners.Add(ar.RoleID);
            }
            return allOwners;
        }

        /// <summary>
        /// 获取属于某一角色的用户集合（ID）
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<string> GetAccountsOfRole(string roleID)
        {
            List<string> accountIDs = new List<string>();
            Criteria c = new Criteria(CriteriaType.Equals, "RoleID", roleID);
            List<AccountRole> accountRole = Assistant.List<AccountRole>(c, null);
            foreach (AccountRole ar in accountRole)
            {
                if (!accountIDs.Contains(ar.AccountID)) accountIDs.Add(ar.AccountID);
            }
            return accountIDs;
        }

        /// <summary>
        /// 获取属于某一角色的用户集合（ID）
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="from">开始位置</param>
        /// <param name="count">总共返回的条数</param>
        /// <returns>一组用户角色关联对象</returns>
        public List<string> GetAccountsOfRole(string roleID, int from, int count)
        {
            List<string> accountIDs = new List<string>();
            Criteria c = new Criteria(CriteriaType.Equals, "RoleID", roleID);
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            List<AccountRole> ars = Assistant.List<AccountRole>(c, orders, from, count);
            foreach (AccountRole ar in ars)
            {
                accountIDs.Add(ar.AccountID);
            }
            return accountIDs;
        }

        /// <summary>
        /// 获取属于某一角色的用户总数
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public int GetAccountCountOfRole(string roleID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "RoleID", roleID);
            return Assistant.Count<AccountRole>(c);
        }

        #endregion

    }
}
