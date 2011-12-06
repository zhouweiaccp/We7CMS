using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using We7.CMS.Common;
using We7.Framework.Config;
using System.Collections.Generic;
using We7.CMS.Common.PF;
using We7.Framework;
using Thinkment.Data;
using We7.CMS.Common.Enum;

namespace We7.CMS.Accounts
{
    /// <summary>
    /// insall 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://westengine.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [ToolboxItem(false)]
    public class AccountWebService : System.Web.Services.WebService
    {
        AccountLocalHelper AccountLocalHelper
        {
            get
            {
                return HelperFactory.Instance.GetHelper<AccountLocalHelper>();
            }
        }

        [WebMethod]
        public void hello()
        {
        }

        #region 账户
        [WebMethod]
        public Account AddAccount(Account act)
        {
            return AccountLocalHelper.AddAccount(act);
        }
        [WebMethod]
        public bool ExistEmail(string email)
        {
            return AccountLocalHelper.ExistEmail(email);
        }
        [WebMethod]
        public bool ExistUserName(string userName)
        {
            return AccountLocalHelper.ExistUserName(userName);
        }
        [WebMethod]
        public void DeleteAccont(string accountID)
        {
            AccountLocalHelper.DeleteAccont(accountID);
        }
        [WebMethod]
        public void UpdateAccount(Account act, string[] fields)
        {
            AccountLocalHelper.UpdateAccount(act, fields);
        }
        [WebMethod]
        public Account GetAccount(string accountID, string[] fields)
        {
            return AccountLocalHelper.GetAccount(accountID, fields);
        }
        [WebMethod]
        public Account GetAccountByLoginName(string loginName)
        {
            return AccountLocalHelper.GetAccountByLoginName(loginName);
        }
        [WebMethod]
        public Account GetAccountByEmail(string email)
        {
            return AccountLocalHelper.GetAccountByEmail(email);
        }
        [WebMethod]
        public byte[] GetAccounts(string siteID, string departmentID, string selectName, OwnerRank type)
        {
            List<Account> list= AccountLocalHelper.GetAccounts(siteID, departmentID,  selectName, type);
            return We7Helper.ObjectToBytes(list);
        }

        [WebMethod(MessageName = "GetAccountList-1")]
        public byte[] GetAccountList(List<string> ownerIds)
        {
             List<Account> list= AccountLocalHelper.GetAccountList(ownerIds);
             return We7Helper.ObjectToBytes(list); 
        }
        [WebMethod(MessageName = "GetAccountList-4")]
        public byte[] GetAccountList(Criteria c, Order[] o, int begin, int count)
        {
            List<Account> list = AccountLocalHelper.GetAccountList(c, o, begin, count);
            return We7Helper.ObjectToBytes(list);
        }
        [WebMethod]
        public int GetAccountCountByCriteria(Criteria c)
        {
            return AccountLocalHelper.GetAccountCountByCriteria(c);
        }

        [WebMethod]
        public int QueryAccountCountByQuery(AccountQuery query)
        {
            return AccountLocalHelper.QueryAccountCountByQuery(query);
        }

        [WebMethod]
        public byte[] QueryAccountsByQuery(AccountQuery query, int from, int count, string[] fields)
        {
            List<Account> list= AccountLocalHelper.QueryAccountsByQuery(query, from, count, fields);
            return We7Helper.ObjectToBytes(list);
        }

        [WebMethod(EnableSession = true)]
        public string[] Login(string name, string password)
        {
            return AccountLocalHelper.Login(name, password);
        }

        [WebMethod(EnableSession=true)]
        public string SignOut()
        {
            return AccountLocalHelper.SignOut();
        }
        [WebMethod]
        public bool IsValidPassword(Account account, string password)
        {
            return AccountLocalHelper.IsValidPassword(account, password);
        }
        [WebMethod]
        public string UpdatePassword(Account account, string newPassword)
        {
            return AccountLocalHelper.UpdatePassword(account, newPassword);
        }
        [WebMethod(EnableSession = true)]
        public Account GetAuthenticatedAccount()
        {
            return AccountLocalHelper.GetAuthenticatedAccount();
        }

        #endregion

        #region 角色

        [WebMethod]
        public Role AddRole(Role role)
        {
            return AccountLocalHelper.AddRole(role);
        }
        [WebMethod]
        public void DeleteRole(string roleID)
        {
            AccountLocalHelper.DeleteRole(roleID);
        }
        [WebMethod]
        public void UpdateRole(Role role)
        {
            AccountLocalHelper.UpdateRole(role);
        }
        [WebMethod]
        public void UpdateAccountRoles(string accountID, string[] roles)
        {
            AccountLocalHelper.UpdateAccountRoles(accountID, roles);
        }
        [WebMethod]
        public bool AssignAccountRole(string accountID, string roleID)
        {
            return AccountLocalHelper.AssignAccountRole(accountID, roleID);
        }

        [WebMethod]
        public Role GetRole(string roleID)
        {
            return AccountLocalHelper.GetRole(roleID);
        }
        [WebMethod]
        public Role GetRoleBytitle(string title)
        {
            return AccountLocalHelper.GetRoleBytitle(title);
        }
        [WebMethod]
        public int GetRoleCount(string siteID, OwnerRank type)
        {
            return AccountLocalHelper.GetRoleCount(siteID, type);
        }
        [WebMethod(MessageName = "GetRoles-2")]
        public byte[] GetRoles(string siteID, int from, int count)
        {
            List<Role> list = AccountLocalHelper.GetRoles(siteID, from, count);
            return We7Helper.ObjectToBytes(list);
        }

        [WebMethod(MessageName = "GetRoles-2-2")]
        public byte[] GetRoles(string siteID, OwnerRank type, string key)
        {
            List<Role> list = AccountLocalHelper.GetRoles(siteID, type, key);
            return We7Helper.ObjectToBytes(list);
        }

        [WebMethod]
        public void DeleteAccountRole(string id)
        {
            AccountLocalHelper.DeleteAccountRole(id);
        }
        [WebMethod]
        public byte[] GetRolesOfAccount(string accountID)
        {
            List<string> list = AccountLocalHelper.GetRolesOfAccount(accountID);
            return We7Helper.ObjectToBytes(list);
        }
        [WebMethod(MessageName = "GetAccountsOfRole-1")]
        public byte[]  GetAccountsOfRole(string roleID)
        {
            List<string> list= AccountLocalHelper.GetAccountsOfRole(roleID);
            return We7Helper.ObjectToBytes(list);
        }
        [WebMethod(MessageName = "GetAccountsOfRole-3")]
        public byte[] GetAccountsOfRole(string roleID, int from, int count)
        {
            List<string> list = AccountLocalHelper.GetAccountsOfRole(roleID, from, count);
            return We7Helper.ObjectToBytes(list);
        }
        [WebMethod]
        public int GetAccountCountOfRole(string roleID)
        {
            return AccountLocalHelper.GetAccountCountOfRole(roleID);
        }

        #endregion

        #region 部门
        [WebMethod]
        public Department AddDepartment(Department dpt)
        {
            return AccountLocalHelper.AddDepartment(dpt);
        }
        [WebMethod]
        public void DeleteDepartment(string departmentID)
        {
            AccountLocalHelper.DeleteDepartment(departmentID);
        }
        [WebMethod]
        public void UpdateDepartment(Department dpt, string[] fields)
        {
            AccountLocalHelper.UpdateDepartment(dpt, fields);
        }
        [WebMethod]
        public Department GetDepartment(string departmentID, string[] fields)
        {
            return AccountLocalHelper.GetDepartment(departmentID, fields);
        }
        [WebMethod]
        public byte[] GetDepartments(string siteID, string parentID, string selectName, string[] fields)
        {
            List<Department> list= AccountLocalHelper.GetDepartments(siteID, parentID, selectName, fields);
            return We7Helper.ObjectToBytes(list);
        }
        [WebMethod]
        public byte[] GetOrderDepartments(string siteID, string parentID)
        {
            List<Department> list=  AccountLocalHelper.GetOrderDepartments(siteID, parentID);
            return We7Helper.ObjectToBytes(list);
        }
        [WebMethod]
        public byte[] GetDepartmentTreeWithFormat(string siteID, string parentId)
        {
            List<Department> list=  AccountLocalHelper.GetDepartmentTreeWithFormat(siteID, parentId);
            return We7Helper.ObjectToBytes(list);
        }
        [WebMethod]
        public byte[] GetDepartmentTree(string siteID, string parentId)
        {
            List<Department> list=  AccountLocalHelper.GetDepartmentTree(siteID, parentId);
            return We7Helper.ObjectToBytes(list);
        }
        #endregion
    }
}
