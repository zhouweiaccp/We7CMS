using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using Thinkment.Data;

namespace We7.CMS.Accounts
{
    public interface IAccountHelper
    {
        //账户
        Account AddAccount(Account act);
        bool ExistEmail(string email);
        bool ExistUserName(string userName);
        void DeleteAccont(string accountID);
        void UpdateAccount(Account act, string[] fields);
        Account GetAccount(string accountID, string[] fields);
        Account GetAuthenticatedAccount();
        Account GetAccountByLoginName(string loginName);
        Account GetAccountByEmail(string email);
        List<Account> GetAccounts(string siteID, string departmentID, string selectName, OwnerRank type);
        List<Account> GetAccountList(List<string> ownerIds);
        List<Account> GetAccountList(Criteria c, Order[] o, int begin, int count);
        int GetAccountCountByCriteria(Criteria c);

        int QueryAccountCountByQuery(AccountQuery query);
        List<Account> QueryAccountsByQuery(AccountQuery query, int from, int count, string[] fields);

        string[] Login(string name, string password);
        string SignOut();
        bool IsValidPassword(Account account, string password);
        string UpdatePassword(Account account, string newPassword);

        //角色
        Role AddRole(Role role);
        void DeleteRole(string roleID);
        void UpdateRole(Role role);
        void UpdateAccountRoles(string accountID, string[] roles);
        bool AssignAccountRole(string accountID, string roleID);

        Role GetRole(string roleID);
        Role GetRoleBytitle(string title);
        int GetRoleCount(string siteID, OwnerRank type);
        List<Role> GetRoles(string siteID, int from, int count);
        List<Role> GetRoles(string siteID, OwnerRank type, string key);

        void DeleteAccountRole(string id);
        List<string> GetRolesOfAccount(string accountID);
        List<string> GetAccountsOfRole(string roleID);
        List<string> GetAccountsOfRole(string roleID, int from, int count);
        int GetAccountCountOfRole(string roleID);

        //部门
        Department AddDepartment(Department dpt);
        void DeleteDepartment(string departmentID);
        void UpdateDepartment(Department dpt, string[] fields);
        Department GetDepartment(string departmentID, string[] fields);
        List<Department> GetDepartments(string siteID, string parentID, string selectName, string[] fields);
        List<Department> GetOrderDepartments(string siteID, string parentID);
        List<Department> GetDepartmentTreeWithFormat(string siteID, string parentId);
        List<Department> GetDepartmentTree(string siteID, string parentId);

        //权限
        void AddPermission(int ownerType, string ownerID, string objectID, string[] contents);
        void DeletePermission(int ownerType, string ownerID, string objectID, string[] contents);
        void DeletePermission(int ownerType, string ownerID, string objectID);
        List<string> GetPermissionContents(string accountID, string objectID);
        List<string> GetPermissionContents(string ownerType, string ownerID, string objectID);
        List<string> GetPermissionOwners(int typeID, string objectID);
        List<Permission> GetPermissions(int ownerType, string ownerID, string objectID);
        List<Permission> GetPermissions(string adviceTypeID, string content);
        List<Permission> GetPermissions(List<string> allOwners, List<string> objList);
        List<string> GetObjectsByPermission(string accountID, string permission);
    }
}
