using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.CMS.Common;
using We7.Framework.Config;
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;
using Thinkment.Data;
using System.Web;
using System.Net;

namespace We7.CMS.Accounts
{
    /// <summary>
    /// 调用远程服务器数据
    /// </summary>
    public class AccountRemoteHelper:IAccountHelper
    {
        AccountLocalHelper LocalHelper
        {
            get
            {
                return HelperFactory.Instance.GetHelper<AccountLocalHelper>();
            }
        }

        WD.AccountWebService RemoteHelper
        {
            get
            {
                WD.AccountWebService client = new WD.AccountWebService();
                client.Url = SiteConfigs.GetConfig().PassportServiceUrl;
                return client;
            }
        }
        
        CookieContainer MyCookieContainer
        {
            get
            {
                if (HttpContext.Current.Session["$AcountCookieContainer"] != null)
                    return HttpContext.Current.Session["$AcountCookieContainer"] as CookieContainer;
                else
                {
                    System.Net.CookieContainer cookieContainer = new System.Net.CookieContainer();
                    HttpContext.Current.Session["$AcountCookieContainer"] = cookieContainer;
                    return cookieContainer;
                }

            }
        }


        #region 账户

        public Account AddAccount(Account act)
        {
            return RemoteHelper.AddAccount(act);
        }

        public bool ExistEmail(string email)
        {
            return RemoteHelper.ExistEmail(email);
        }

        public bool ExistUserName(string userName)
        {
            return RemoteHelper.ExistUserName(userName);
        }

        public void DeleteAccont(string accountID)
        {
            RemoteHelper.DeleteAccont(accountID);
        }

        public void UpdateAccount(Account act, string[] fields)
        {
            RemoteHelper.UpdateAccount(act, fields);
        }

        public Account GetAccount(string accountID, string[] fields)
        {
            if (accountID == We7Helper.EmptyGUID)
            {
                Account a = new Account();
                a.LoginName = SiteConfigs.GetConfig().AdministratorName;
                a.LastName = "管理员";
                a.ID = We7Helper.EmptyGUID;
                return a;
            }
            else if (accountID == Security.CurrentAccountID)
            {
                if(HttpContext.Current.Session["$We7CurrentAccount"] == null)
                    HttpContext.Current.Session["$We7CurrentAccount"]=RemoteHelper.GetAccount(accountID, null);
                return HttpContext.Current.Session["$We7CurrentAccount"] as Account;
            }
            else
                return RemoteHelper.GetAccount(accountID, fields);
        }

        public Account GetAccountByLoginName(string loginName)
        {
            return RemoteHelper.GetAccountByLoginName(loginName);
        }

        public Account GetAccountByEmail(string email)
        {
            return RemoteHelper.GetAccountByEmail(email);
        }

        public List<Account> GetAccounts(string siteID, string departmentID, string selectName, OwnerRank type)
        {
            byte[]  list = RemoteHelper.GetAccounts(siteID,departmentID, selectName, type);
            return We7Helper.BytesToObject(list) as List<Account>;
        }

        public List<Account> GetAccountList(List<string> ownerIds)
        {
            if (ownerIds == null || ownerIds.Count == 0)
                return null;
            else
            {
                byte[] list = RemoteHelper.GetAccountList(ownerIds.ToArray());
                return We7Helper.BytesToObject(list) as List<Account>;
            }
        }

        public List<Account> GetAccountList(Criteria c, Order[] o, int begin, int count)
        {
            byte[] list = RemoteHelper.GetAccountList(c, o, begin, count);
            return We7Helper.BytesToObject(list) as List<Account>;
        }

        public int GetAccountCountByCriteria(Criteria c)
        {
            return RemoteHelper.GetAccountCountByCriteria(c);
        }


        public int QueryAccountCountByQuery(AccountQuery query)
        {
            return RemoteHelper.QueryAccountCountByQuery(query);
        }


        public List<Account> QueryAccountsByQuery(AccountQuery query, int from, int count, string[] fields)
        {
            byte[] arr = RemoteHelper.QueryAccountsByQuery(query, from, count, fields);
            return (List<Account>)We7Helper.BytesToObject(arr);
        }


        public bool IsValidPassword(Account account, string password)
        {
            return RemoteHelper.IsValidPassword(account, password);
        }

        public string UpdatePassword(Account account, string newPassword)
        {
            return RemoteHelper.UpdatePassword(account, newPassword);
        }

        public string[] Login(string name, string password)
        {
            string[] result = { "", "" };
            if (HttpContext.Current.Request["Authenticator"] == null)
            {
                SSORequest req = new SSORequest();
                req.Action = "signin";
                req.UserName = name;
                req.Password = password;
                req.SiteID = SiteConfigs.GetConfig().SiteID;
                Authentication.CreateAppToken(req);
                Authentication.Post(req, SiteConfigs.GetConfig().PassportAuthPage);
            }
            //else if (Request["Authenticator"] != null && Request["accountID"] != null)
            //{
            //    SSORequest ssoRequest = SSORequest.GetRequest(HttpContext.Current);
            //    string actID = ssoRequest.AccountID;
            //    if (Authentication.ValidateEACToken(ssoRequest) && !string.IsNullOrEmpty(actID) && We7Helper.IsGUID(actID))
            //    {
            //        Security.SetAccountID(actID);
            //        result[0] = "true";
            //        result[1] = actID;
            //    }
            //    else if (Request["message"] != null)
            //    {
            //        result[0] = "false";
            //        result[1] = Request["message"];
            //    }
            //}
            return result;
        }

        public string SignOut()
        {
            string result = "";
            if (HttpContext.Current.Request["Authenticator"] == null)
            {
                Security.SignOut();
                SSORequest req = new SSORequest();
                req.Action = "signout";
                req.SiteID = SiteConfigs.GetConfig().SiteID;
                Authentication.CreateAppToken(req);
                Authentication.Post(req, SiteConfigs.GetConfig().PassportAuthPage);
            }
            return result;
        }

        public Account GetAuthenticatedAccount()
        {
            if (HttpContext.Current.Request["Authenticator"] != null && HttpContext.Current.Request["accountID"] != null)
            {
                SSORequest ssoRequest = SSORequest.GetRequest(HttpContext.Current);
                string actID = ssoRequest.AccountID;
                if (Authentication.ValidateEACToken(ssoRequest) && !string.IsNullOrEmpty(actID) && We7Helper.IsGUID(actID))
                {
                    Security.SetAccountID(actID);
                    return RemoteHelper.GetAccount(actID, null);
                }
                else
                    return null;
            }
            else if (HttpContext.Current.Request["Authenticator"] == null)
            {
                SSORequest req = new SSORequest();
                req.Action = "authenticate";
                req.SiteID = SiteConfigs.GetConfig().SiteID;
                Authentication.CreateAppToken(req);
                Authentication.Post(req, SiteConfigs.GetConfig().PassportAuthPage);
                return null;
            }
            else
                return null;
        }

        #endregion

        #region 角色


        public Role AddRole(Role role)
        {
            return RemoteHelper.AddRole(role);
        }

        public void DeleteRole(string roleID)
        {
            RemoteHelper.DeleteRole(roleID);
        }

        public void UpdateRole(Role role)
        {
            RemoteHelper.UpdateRole(role);
        }

        public void UpdateAccountRoles(string accountID, string[] roles)
        {
            RemoteHelper.UpdateAccountRoles(accountID, roles);
        }

        public bool AssignAccountRole(string accountID, string roleID)
        {
            return RemoteHelper.AssignAccountRole(accountID, roleID);
        }


        public Role GetRole(string roleID)
        {
            return RemoteHelper.GetRole(roleID);
        }

        public Role GetRoleBytitle(string title)
        {
            return RemoteHelper.GetRoleBytitle(title);
        }

        public int GetRoleCount(string siteID, OwnerRank type)
        {
            return RemoteHelper.GetRoleCount(siteID, type);
        }

        public List<Role> GetRoles(string siteID, int from, int count)
        {
            byte[] list = RemoteHelper.GetRoles(siteID, from, count);
            return We7Helper.BytesToObject(list) as List<Role>;
        }


        public List<Role> GetRoles(string siteID, OwnerRank type, string key)
        {
            byte[] list = RemoteHelper.GetRoles(siteID, type, key);
            return We7Helper.BytesToObject(list) as List<Role>;
        }


        public void DeleteAccountRole(string id)
        {
            RemoteHelper.DeleteAccountRole(id);
        }

        public List<string> GetRolesOfAccount(string accountID)
        {
            byte[]  list = RemoteHelper.GetRolesOfAccount(accountID);
            return We7Helper.BytesToObject(list) as List<string>;
        }

        public List<string> GetAccountsOfRole(string roleID)
        {
            byte[] list = RemoteHelper.GetAccountsOfRole(roleID);
            return We7Helper.BytesToObject(list) as List<string>;
        }

        public List<string> GetAccountsOfRole(string roleID, int from, int count)
        {
            byte[]  list = RemoteHelper.GetAccountsOfRole(roleID, from, count);
            return We7Helper.BytesToObject(list) as List<string>;
        }

        public int GetAccountCountOfRole(string roleID)
        {
            return RemoteHelper.GetAccountCountOfRole(roleID);
        }

        #endregion

        #region 部门

        public Department AddDepartment(Department dpt)
        {
            return RemoteHelper.AddDepartment(dpt);
        }

        public void DeleteDepartment(string departmentID)
        {
            RemoteHelper.DeleteDepartment(departmentID);
        }

        public void UpdateDepartment(Department dpt, string[] fields)
        {
            RemoteHelper.UpdateDepartment(dpt, fields);
        }

        public Department GetDepartment(string departmentID, string[] fields)
        {
            return RemoteHelper.GetDepartment(departmentID, fields);
        }

        public List<Department> GetDepartments(string siteID, string parentID, string selectName, string[] fields)
        {
            byte[] list = RemoteHelper.GetDepartments(siteID, parentID, selectName, fields);
            return We7Helper.BytesToObject(list) as List<Department>;
        }

        public List<Department> GetOrderDepartments(string siteID, string parentID)
        {
            byte[] list = RemoteHelper.GetOrderDepartments(siteID, parentID);
            return We7Helper.BytesToObject(list) as List<Department>;
        }

        public List<Department> GetDepartmentTreeWithFormat(string siteID, string parentId)
        {
            byte[] list = RemoteHelper.GetDepartmentTreeWithFormat(siteID, parentId);
            return We7Helper.BytesToObject(list) as List<Department>;
        }

        public List<Department> GetDepartmentTree(string siteID, string parentId)
        {
            byte[]  list = RemoteHelper.GetDepartmentTree(siteID, parentId);
            return We7Helper.BytesToObject(list) as List<Department>;
        }
        #endregion

        #region 权限

        public void AddPermission(int ownerType, string ownerID, string objectID, string[] contents)
        {
            LocalHelper.AddPermission(ownerType, ownerID, objectID, contents);
        }
        public void DeletePermission(int ownerType, string ownerID, string objectID, string[] contents)
        {
            LocalHelper.DeletePermission(ownerType, ownerID, objectID, contents);
        }
        public void DeletePermission(int ownerType, string ownerID, string objectID)
        {
            LocalHelper.DeletePermission( ownerType,  ownerID,  objectID);
        }
        public List<string> GetPermissionContents(string accountID, string objectID)
        {
            return LocalHelper.GetPermissionContents(accountID, objectID);
        }
        public List<string> GetPermissionContents(string ownerType, string ownerID, string objectID)
        {
            return LocalHelper.GetPermissionContents(ownerType, ownerID, objectID);
        }
        public List<string> GetPermissionOwners(int typeID, string objectID)
        {
            return LocalHelper.GetPermissionOwners(typeID, objectID);
        }
        public List<Permission> GetPermissions(int ownerType, string ownerID, string objectID)
        {
            return LocalHelper.GetPermissions(ownerType, ownerID, objectID);
        }
        public List<Permission> GetPermissions(string adviceTypeID, string content)
        {
            return LocalHelper.GetPermissions(adviceTypeID, content);
        }
        public List<Permission> GetPermissions(List<string> allOwners, List<string> objList)
        {
            return LocalHelper.GetPermissions(allOwners, objList);
        }
        public List<string> GetObjectsByPermission(string accountID, string permission)
        {
            return LocalHelper.GetObjectsByPermission(accountID, permission);
        }

        #endregion

    }
}
