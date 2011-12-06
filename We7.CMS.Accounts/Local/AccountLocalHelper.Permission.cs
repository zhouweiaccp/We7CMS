using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.PF;
using We7.Framework;
using We7.Framework.Common;
using System.Web;
using We7.CMS.Common;
using Thinkment.Data;
using System.Web.Caching;

namespace We7.CMS.Accounts
{
    public partial class AccountLocalHelper : BaseHelper, IAccountHelper
    {

        #region permmission  权限

        /// <summary>
        /// 获取用户所具有的所有权限内容列表（包含了所属角色的权限列表）
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <param name="objectID">菜单ID或栏目ID</param>
        /// <returns>权限列表</returns>
        public List<string> GetPermissionContents(string accountID, string objectID)
        {
            List<string> contents = new List<string>();
            string key = "$AccountAllPermissionContents" + accountID + objectID;
            if (HttpContext.Current.Items[key] == null)
            {
                Criteria c = new Criteria(CriteriaType.Equals, "ObjectID", objectID);
                Criteria subc = new Criteria(CriteriaType.None);
                subc.Mode = CriteriaMode.Or;
                subc.Add(CriteriaType.Equals, "OwnerID", accountID);

                //获取用户所拥有的角色，如果拥有则继续获取每个角色的权限
                IAccountHelper ah = AccountFactory.CreateInstance();
                List<string> roles = ah.GetRolesOfAccount(accountID);
                if (roles != null)
                {
                    foreach (string ar in roles)
                    {
                        subc.Add(CriteriaType.Equals, "OwnerID", ar);
                    }
                    c.Criterias.Add(subc);
                }

                List<Permission> plist = Assistant.List<Permission>(c, null);
                if (plist != null)
                {
                    foreach (Permission p in plist)
                    {
                        contents.Add(p.Content);
                    }
                }

                HttpContext.Current.Items[key] = contents;
            }
            else
                contents = HttpContext.Current.Items[key] as List<string>;

            return contents;
        }

        /// <summary>
        /// 某一所有者对对象具有的操作权限内容列表
        /// </summary>
        /// <param name="ownerType">用户？角色？</param>
        /// <param name="ownerID">所有者</param>
        /// <param name="objectID">栏目或对象</param>
        /// <returns>权限内容列表</returns>
        public List<string> GetPermissionContents(string ownerType, string ownerID, string objectID)
        {
            List<string> contents = new List<string>();
            string key = "$OwnerPermissionContents" + ownerID + objectID;
            if (HttpContext.Current.Items[key] == null)
            {
                Criteria c = new Criteria(CriteriaType.Equals, "OwnerType", ownerType);
                c.Add(CriteriaType.Equals, "OwnerID", ownerID);
                c.Add(CriteriaType.Equals, "ObjectID", objectID);

                List<Permission> plist = Assistant.List<Permission>(c, null);
                if (plist != null)
                {
                    foreach (Permission p in plist)
                    {
                        contents.Add(p.Content);
                    }
                }
                HttpContext.Current.Items[key] = contents;
            }
            else
                contents = HttpContext.Current.Items[key] as List<string>;

            return contents;
        }

        /// <summary>
        /// 获得所有者列表
        /// </summary>
        /// <param name="typeID">所有者类型</param>
        /// <param name="objectID">栏目ID或反馈类型ID</param>
        /// <returns>所有者列表</returns>
        public List<string> GetPermissionOwners(int typeID, string objectID)
        {
            List<string> ownerIds = new List<string>();
            Criteria c = new Criteria(CriteriaType.Equals, "ObjectID", objectID);
            c.Add(CriteriaType.Equals, "OwnerType", typeID);
            List<Permission> plist = Assistant.List<Permission>(c, null);
            foreach (Permission p in plist)
            {
                if (!ownerIds.Contains(p.OwnerID))
                    ownerIds.Add(p.OwnerID);
            }
            return ownerIds;
        }

        /// <summary>
        /// 获取用户或角色权限
        /// </summary>
        /// <param name="ownerType">所有者类型</param>
        /// <param name="ownerID">用户ID</param>
        /// <param name="objectID">菜单ID或栏目ID</param>
        /// <returns>权限列表</returns>
        public List<Permission> GetPermissions(int ownerType, string ownerID, string objectID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ObjectID", objectID);
            c.Add(CriteriaType.Equals, "OwnerType", ownerType);
            c.Add(CriteriaType.Equals, "OwnerID", ownerID);
            return Assistant.List<Permission>(c, null);
        }

        /// <summary>
        /// 获取权限集：一组所有者，对一组权限类型的权限列表
        /// </summary>
        /// <param name="allOwners">所有者列表</param>
        /// <param name="levels">权限内容列表</param>
        /// <returns>权限集</returns>
        public List<Permission> GetPermissions(List<string> allOwners, List<string> levels)
        {
            List<string> list = new List<string>();
            Criteria c = new Criteria(CriteriaType.None);
            if (allOwners.Count != 0)
            {
                Criteria subc = new Criteria(CriteriaType.None);
                subc.Mode = CriteriaMode.Or;
                foreach (string ownerID in allOwners)
                {
                    subc.AddOr(CriteriaType.Equals, "OwnerID", ownerID);
                }
                c.Criterias.Add(subc);
            }
            if (levels != null)
            {
                Criteria subLevel = new Criteria(CriteriaType.None);
                foreach (string level in levels)
                {
                    subLevel.AddOr(CriteriaType.Equals, "Content", level);
                }
                c.Criterias.Add(subLevel);
            }

            List<Permission> ps = Assistant.List<Permission>(c, null);
            return ps;
        }

        /// <summary>
        ///  获取权限集：授权对象+授权内容
        /// </summary>
        /// <param name="objectID">授权对象</param>
        /// <param name="content">授权内容</param>
        /// <returns></returns>
        public List<Permission> GetPermissions(string objectID, string content)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (!string.IsNullOrEmpty(objectID))
            {
                c.Add(CriteriaType.Equals, "ObjectID", objectID);
            }
            if (!string.IsNullOrEmpty(content))
            {
                c.Add(CriteriaType.Equals, "Content", content);
            }
            List<Permission> ps = Assistant.List<Permission>(c, null);
            return ps;
        }

        /// <summary>
        /// 添加一个权限记录
        /// </summary>
        /// <param name="ownerType">所有者类型</param>
        /// <param name="ownerID">用户ID</param>
        /// <param name="objectID">菜单ID或栏目ID</param>
        /// <param name="contents">权限列表</param>
        public void AddPermission(int ownerType, string ownerID, string objectID, string[] contents)
        {
            if (contents == null || contents.Length == 0)
            {
                return;
            }
            foreach (string c in contents)
            {
                Permission p = new Permission();
                p.Content = c;
                p.ObjectID = objectID;
                p.OwnerID = ownerID;
                p.OwnerType = ownerType;
                p.ID = We7Helper.CreateNewID();
                Assistant.Insert(p);
            }
        }

        /// <summary>
        /// 删除一个权限记录
        /// </summary>
        /// <param name="ownerType">类型所属类型：0—帐户；1—角色</param>
        /// <param name="ownerID">用户ID或角色ID</param>
        /// <param name="objectID">对象ID</param>
        /// <param name="content">具体权限</param>
        public void DeletePermission(int ownerType, string ownerID, string objectID, string[] contents)
        {

            Criteria c = new Criteria(CriteriaType.Equals, "OwnerType", ownerType.ToString());
            c.Add(CriteriaType.Equals, "OwnerID", ownerID);
            c.Add(CriteriaType.Equals, "ObjectID", objectID);

            if (contents != null && contents.Length > 0)
            {
                Criteria cr = new Criteria(CriteriaType.None);
                cr.Mode = CriteriaMode.Or;
                foreach (string content in contents)
                {
                    cr.Criterias.Add(new Criteria(CriteriaType.Equals, "Content", content));
                }
                c.Criterias.Add(cr);
            }
            Assistant.DeleteList<Permission>(c);
        }

        /// <summary>
        /// 删除一组权限记录
        /// </summary>
        /// <param name="ownerType">用户?角色？</param>
        /// <param name="ownerID">所有者ID</param>
        /// <param name="objectID">栏目ID</param>
        public void DeletePermission(int ownerType, string ownerID, string objectID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "OwnerType", ownerType.ToString());
            c.Add(CriteriaType.Equals, "OwnerID", ownerID);
            c.Add(CriteriaType.Equals, "ObjectID", objectID);
            Assistant.DeleteList<Permission>(c);
        }

        /// <summary>
        /// 获取当前用户拥有权限permission的栏目ID列表
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <param name="permission">权限名，如“Channel.Article”</param>
        /// <returns>栏目ID列表</returns>
        public List<string> GetObjectsByPermission(string accountID, string permission)
        {
            List<string> channels = new List<string>();
            object tmpObj = HttpContext.Current.Session[accountID + "MyPermissionChannelList" + permission];
            if (tmpObj != null)
            {
                channels = (List<string>)tmpObj;
            }
            else
            {
                IAccountHelper ah = AccountFactory.CreateInstance();
                List<string> allOwners = ah.GetRolesOfAccount(accountID);
                allOwners.Add(accountID);
                channels = GetObjectID(allOwners, permission);
                HttpContext.Current.Session[accountID + "MyPermissionChannelList" + permission] = channels;
            }

            return channels;
        }

        /// <summary>
        ///  通过权限获取一组栏目（或其他对象）
        /// </summary>
        /// <param name="ownerIDs">角色或用户ID</param>
        /// <param name="level">权限等级集合</param>
        /// <returns>栏目列表</returns>
        List<string> GetObjectID(List<string> ownerIDs, string level)
        {
            List<string> levels = new List<string>();
            levels.Add(level);
            return GetObjectID(ownerIDs, levels);
        }

  
        /// <summary>
        /// 通过条件获取一组栏目（或其他对象）
        /// </summary>
        /// <param name="ownerIDs">角色或用户ID集合</param>
        /// <param name="levels">权限等级集合</param>
        /// <returns>栏目列表</returns>
        List<string> GetObjectID(List<string> ownerIDs, List<string> levels)
        {
            List<string> list = new List<string>();
            List<Permission> ps = GetPermissions(ownerIDs, levels);
            foreach (Permission p in ps)
            {
                list.Add(p.ObjectID);
            }
            return list;
        }

      /*
       
        /// <summary>
        /// 通过用户ID把具有三级审核的栏目ID组合起来
        /// thehim:2009-3-1修改：合并条件，一次调用
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <returns>用户能访问的所有栏目ID</returns>
        public List<string> GetAllChannelID(string accountID)
        {
            List<string> list = new List<string>();
            if (!We7Helper.IsEmptyID(accountID))
            {
                List<string> allAudit = new List<string>();
                List<string> allOwners = new List<string>();

                allAudit.Add("Channel.FirstAudit");
                allAudit.Add("Channel.SecondAudit");
                allAudit.Add("Channel.ThirdAudit");
                allOwners.Add(accountID);
                AccountRole[] accountRoles = GetAccountRoles(accountID);
                if (accountRoles != null)
                {
                    foreach (AccountRole ar in accountRoles)
                    {
                        allOwners.Add(ar.RoleID);
                    }
                }
                list = GetChannelID(allOwners, allAudit);
            }
            return list;
        }
  */
        #endregion

    }
}
