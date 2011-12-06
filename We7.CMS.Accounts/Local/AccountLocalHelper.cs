using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.IO;

using Thinkment.Data;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using System.Web.Caching;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Accounts
{
    /// <summary>
    /// 用户权限方法集（本地存储）；
    /// 包括用户、部门、角色、权限；
    /// </summary>
    [Serializable]
    [Helper("We7.AccountHelper")]
    public partial class AccountLocalHelper : BaseHelper, IAccountHelper
    {

        #region 预定义变量

        /// <summary>
        /// 权限的Session关键字
        /// </summary>
        public static readonly string AccountSessionKey = "We7.Session.Account.Key";

        /// <summary>
        /// 当前的http上下文
        /// </summary>
        HttpContext context { get { return HttpContext.Current; } }

        /// <summary>
        ///　业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)context.Application[HelperFactory.ApplicationID]; }
        }

        #endregion

        #region 注册账号

        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="act">用户对象</param>
        /// <returns>用户对象</returns>
        public Account AddAccount(Account act)
        {
            if (act.LoginName.Length < 3)
                throw new Exception("用户名不能小于3位。");

            if (act.Password.Length < 6)
                throw new Exception("密码不能小于6位。");

            if (GetAccountByLoginName(act.LoginName) != null)
                throw new Exception(string.Format("登录名 {0} 已存在。", act.LoginName));

            if (GetAccountByEmail(act.Email) != null)
                throw new Exception(string.Format("邮件地址 {0} 已被使用。", act.Email));

            We7Helper.AssertNotNull(Assistant, "AccountHelper.Assistant");
            We7Helper.AssertNotNull(act, "AddAccount.act");

                        //利用事务进行帐户的相关权限删除
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            IConnection ic = Assistant.GetConnections()[db];
            ic.IsTransaction = true;
            try
            {
                act.ID = We7Helper.CreateNewID();
                act.Created = DateTime.Now;
                if (GeneralConfigs.GetConfig().UserRegisterMode == "none")
                    act.State = 1;

                AccountRole ar = new AccountRole();
                ar.AccountID = act.ID;
                ar.RoleID = "1";
                ar.RoleTitle = "注册用户";
                ar.ID = We7Helper.CreateNewID();

                OnUserAdded(act);
                UpdateUserPassword(act);

                Assistant.Insert(ic, act, null);
                Assistant.Insert(ic, ar, null);

                ic.Commit();
                ic.Dispose();
                return act;
            }
            catch (Exception ex)
            {
                ic.Rollback();
                ic.Dispose();
                throw ex;
             }
        }

        /// <summary>
        /// 判断会员登录名称是否已经存在
        /// </summary>
        /// <param name="userName">会员登录名称</param>
        /// <returns>是否存在</returns>
        public bool ExistUserName(string userName)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "LoginName", userName);
            List<Account> Account = Assistant.List<Account>(c, null);
            if (Account.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断会员邮箱地址是否已经存在
        /// </summary>
        /// <param name="email">会员邮箱地址</param>
        /// <returns></returns>
        public bool ExistEmail(string email)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Email", email);
            List<Account> Account = Assistant.List<Account>(c, null);
            if (Account.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 获取用户

        /// <summary>
        /// 通过登录名获取一个用户
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>用户对象</returns>
        public Account GetAccountByLoginName(string loginName)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "LoginName", loginName);
            if (Assistant.Count<Account>(c) > 0)
            {
                List<Account> act = Assistant.List<Account>(c, null);
                return act[0];
            }
            return null;
        }

        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="accountID">用户ID</param>
        /// <param name="fields">返回的字段集合</param>
        /// <returns>用户对象</returns>
        public Account GetAccount(string accountID, string[] fields)
        {
            if (accountID == We7Helper.EmptyGUID)
            {
                Account sa = new Account();
                sa.ID = accountID;
                sa.LastName = "系统管理员";
                sa.LoginName = SiteConfigs.GetConfig().AdministratorName;
                return sa;
            }
            else if (!string.IsNullOrEmpty(accountID))
            {
                We7Helper.AssertNotNull(accountID, "GetAccount.accountID");
                Criteria c = new Criteria(CriteriaType.Equals, "ID", accountID);
                List<Account> act = Assistant.List<Account>(c, null, 0, 1, fields);
                if (act.Count > 0)
                {
                    return act[0];
                }
            }

            Account a = new Account();
            a.ID = accountID;
            a.LoginName = "";
            a.LastName = "未知用户";
            return a;
        }

        /// <summary>
        /// 根据用户邮箱获取用户信息
        /// </summary>
        /// <param name="email">E-mail</param>
        /// <returns>用户信息</returns>
        public Account GetAccountByEmail(string email)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Email", email);
            List<Account> account = Assistant.List<Account>(c, null);
            if (account.Count > 0)
            {
                return account[0];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 删除账户

        /// <summary>
        /// 删除帐户（同时删除权限以及联盟和邮箱帐号等）
        /// </summary>
        /// <param name="accountID">用户ID</param>
        public void DeleteAccont(string accountID)
        {
            We7Helper.AssertNotNull(Assistant, "AccountHelper.Assistant");
            We7Helper.AssertNotNull(accountID, "DeleteAccont.accountID");

            //利用事务进行帐户的相关权限删除
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            IConnection ic = Assistant.GetConnections()[db];
            ic.IsTransaction = true;
            try
            {
                //删除Permissions
                Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", accountID);
                Assistant.DeleteList<Permission>(ic, c);
                //删除AccountRole
                Criteria ca = new Criteria(CriteriaType.Equals, "AccountID", accountID);
                Assistant.DeleteList<AccountRole>(ic, ca);
                //最后删除当前帐户
                Account act = new Account();
                act.ID = accountID;
                Assistant.Delete(ic, act);
                OnUserDeleted(act);

                ic.Commit();
            }
            catch (Exception)
            {
                try { ic.Rollback(); }
                catch (Exception)
                { }
            }
            finally { ic.Dispose(); }
        }

        /// <summary>
        /// 删除帐户（同时删除权限以及联盟和邮箱帐号等）
        /// </summary>
        /// <param name="ic">连接</param>
        /// <param name="accountID">用户ID</param>
        public void DeleteAccont(IConnection ic, string accountID)
        {
            We7Helper.AssertNotNull(Assistant, "AccountHelper.Assistant");
            We7Helper.AssertNotNull(accountID, "DeleteAccont.accountID");

            //利用事务进行帐户的相关权限删除
            try
            {
                //删除Permissions
                Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", accountID);
                try
                {
                    Assistant.DeleteList<Permission>(ic, c);
                }
                catch (Exception)
                {
                }

                //删除AccountRole
                Criteria ca = new Criteria(CriteriaType.Equals, "AccountID", accountID);
                try
                {
                    Assistant.DeleteList<AccountRole>(ic, ca);
                }
                catch (Exception)
                {
                }

                //最后删除当前帐户
                Account act = new Account();
                act.ID = accountID;
                Assistant.Delete(ic, act);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region 更新账户

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="account">用户对象</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>修改过后的密码</returns>
        public string UpdatePassword(Account account, string newPassword)
        {
            account.Password = newPassword;
            account.PasswordHashed = (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.noneEncrypt;
            Assistant.Update(account, new string[] { "Password", "PasswordHashed" });
            OnPasswordUpdated(account);
            UpdateUserPassword(account);
            return "";
        }

        /// <summary>
        /// 将用户密码明码改为加密码
        /// </summary>
        /// <param name="account"></param>
        public void UpdateUserPassword(Account account)
        {
            if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.noneEncrypt)
            {
                string oldPassword = account.Password;
                account.Password = Security.Encrypt(oldPassword);
                account.PasswordHashed = (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt;
                Assistant.Update(account, new string[] { "Password", "PasswordHashed" });
            }
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="act">用户对象</param>
        /// <param name="fields">需要更新的字段集合</param>
        public void UpdateAccount(Account act, string[] fields)
        {
            Assistant.Update(act, fields);
        }

        /// <summary>
        /// 初始化用户角色，每个用户都赋予系统角色：注册用户
        /// </summary>
        /// <returns></returns>
        public int InitAllUserRole()
        {
            int total = 0;
            List<Account> allUser = Assistant.List<Account>(null, null);
            foreach (Account a in allUser)
            {
                if (AssignAccountRole(a.ID, "1"))
                    total++;
            }
            return total;
        }

        #endregion

        #region 获取用户列表

        /// <summary>
        /// 根据条件获取一组用户
        /// </summary>
        /// <param name="departmentID">部门ID</param>
        /// <param name="selectName">登陆名</param>
        /// <param name="state">用户状态</param>
        /// <returns>一组用户信息</returns>
        public List<Account> GetAccounts(string siteID, string departmentID, string searchKey, OwnerRank type)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (!string.IsNullOrEmpty(siteID))
                c.Add(CriteriaType.Equals, "FromSiteID", siteID);
            if (departmentID != null && departmentID != "")
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;
                subC.AddOr(CriteriaType.Equals, "DepartmentID", departmentID);

                List<Department> list=GetDepartmentTree(siteID, departmentID);
                foreach (Department depart in list)
                {
                    subC.AddOr(CriteriaType.Equals, "DepartmentID", depart.ID);
                }
                c.Criterias.Add(subC);
            }
            if (type != OwnerRank.All)
                c.Add(CriteriaType.Equals, "UserType", (int)type);
            if (!string.IsNullOrEmpty(searchKey))
                c.Add(CriteriaType.Like, "LoginName", "%" + searchKey + "%");
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            return c.Criterias.Count > 0 ? Assistant.List<Account>(c, o) : new List<Account>();
        }

        /// <summary>
        /// 根据用户ID列表,查询用户信息
        /// </summary>
        /// <param name="ownerIds">用户ID列表</param>
        /// <returns>用户信息列表</returns>
        public List<Account> GetAccountList(List<string> ownerIds)
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Mode = CriteriaMode.Or;
            if (ownerIds != null && ownerIds.Count > 0)
            {
                foreach (string id in ownerIds)
                {
                    c.AddOr(CriteriaType.Equals, "ID", id);
                }
                return Assistant.List<Account>(c, null);
            }
            else
                return null;
        }

        /// <summary>
        /// 通过登录名获取用户用户名
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>登陆名</returns>
        public List<string> GetAccountIDSByLoginName(string username)
        {
            Criteria c = new Criteria(CriteriaType.Like, "LoginName", "%" + username + "%");
            List<Account> ars = Assistant.List<Account>(c, null);
            List<string> ids = new List<string>();
            if (ars != null)
            {
                foreach (Account account in ars)
                {
                    ids.Add(account.ID);
                }
            }
            return ids;
        }

        public List<Account> GetAccountList(Criteria c, Order[] o, int begin, int count)
        {
            return Assistant.List<Account>(c, o, begin, count);
        }

        public int GetAccountCountByCriteria(Criteria c)
        {
            return Assistant.Count<Account>(c);
        }

        public List<Account> GetAccounts(Criteria c, Order[] o)
        {
            return Assistant.List<Account>(c, o);
        }

        #endregion

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name">登录名</param>
        /// <param name="password">密码</param>
        /// <returns>成败消息</returns>
        public string[] Login(string name, string password)
        {
            string[] result = { "false", "" };
            Account act = GetAccountByLoginName(name);
            if (act == null)
            {
                result[0] = "false";
                result[1] = "该用户不存在!";
                return result;
            }
            if (!IsValidPassword(act, password))
            {
                result[0] = "false";
                result[1] = "密码不正确!";
                return result;
            }
            if (GeneralConfigs.GetConfig().UserRegisterMode == "email" && act.EmailValidate != 1)
            {
                result[0] = "false";
                result[1] = "该用户还未通过Email验证!";
                return result;
            }
            if (GeneralConfigs.GetConfig().UserRegisterMode == "manual" && act.State != 1)
            {
                result[0] = "false";
                result[1] = "该用户还未通过人工审核!";
                return result;
            }
            if (act.Overdue < DateTime.Today)
            {
                result[0] = "false";
                result[1] = "您的会员使用日期已终止！";
                return result;
            }

            result[0] = "true";
            result[1] = act.ID;
            Security.SetAccountID(act.ID);
            OnUserLogined(act);
            return result;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns>成败消息</returns>
        public string SignOut()
        {
            string result = "";
            try
            {
                Security.SignOut();
            }
            catch (Exception ex)
            {
                result = "退出失败：" + ex.Message;
                return result;
            }
            try
            {
                OnUserSignOut();
            }
            catch (Exception ex)
            {
                result = "同步退出失败：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="account">用户对象</param>
        /// <param name="password">密码</param>
        /// <returns>密码正确返回true，错误返回false</returns>
        public bool IsValidPassword(Account account, string password)
        {
            if (account == null)
            {
                return false;
            }

            if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt)
            {
                password = Security.Encrypt(password);
            }
            else if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.bbsEncrypt)
            {
                password = Security.BbsEncrypt(password);
            }
            return string.Compare(password, account.Password, false) == 0;
        }

        /// <summary>
        /// 获取当前登录账户
        /// </summary>
        /// <returns></returns>
        public Account GetAuthenticatedAccount()
        {
            if (Security.IsAuthenticated())
                return GetAccount(Security.CurrentAccountID, null);
            else
                return null;
        }
        #endregion

        #region 用户查询

        Criteria CreateCriteriaByQuey(AccountQuery query)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (query.State != 100)
                c.Add(CriteriaType.Equals, "State", query.State);
            if (query.EmailValidate != 100)
                c.Add(CriteriaType.Equals, "EmailValidate", query.EmailValidate);
            if (query.ModelState != 100)
                c.Add(CriteriaType.Equals, "ModelState", query.ModelState);

            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                Criteria keyCriteria = new Criteria(CriteriaType.None);
                keyCriteria.Mode = CriteriaMode.Or;
                keyCriteria.AddOr(CriteriaType.Like, "LastName", "%" + query.KeyWord + "%");
                keyCriteria.AddOr(CriteriaType.Like, "LoginName", "%" + query.KeyWord + "%");
                keyCriteria.AddOr(CriteriaType.Like, "Email", "%" + query.KeyWord + "%");
                c.Criterias.Add(keyCriteria);
            }

            if (!string.IsNullOrEmpty(query.SiteID))
            {
                c.Add(CriteriaType.Equals, "FromSiteID", query.SiteID);
            }

            if (!string.IsNullOrEmpty(query.ModelName ))
            {
                c.Add(CriteriaType.Equals, "ModelName", query.ModelName );
            }
            if (!string.IsNullOrEmpty(query.DepartmentID))
            {
                c.Add(CriteriaType.Equals, "DepartmentID", query.DepartmentID);
            }

            if (query.UserType!=100)
            {
                c.Add(CriteriaType.Equals, "UserType", query.UserType);
            }
            if (c.Criterias.Count == 0)
                return null;
            else 
                return c;
        }

        /// <summary>
        /// 根据查询类获得用户数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int QueryAccountCountByQuery(AccountQuery query)
        {
            Criteria c = CreateCriteriaByQuey(query);
            return Assistant.Count<Account>(c);
        }

        /// <summary>
        /// 按照查询类提供的条件查询用户
        /// </summary>
        /// <param name="query"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<Account> QueryAccountsByQuery(AccountQuery query, int from, int count, string[] fields)
        {
            try
            {
                Criteria c = CreateCriteriaByQuey(query);
                List<Order> orders = CreateOrdersByAll(query.OrderKeys);
                Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
                if (orders != null) o = orders.ToArray();

                return Assistant.List<Account>(c, o, from, count, fields);
            }
            catch (Exception ex)
            {
                return new List<Account>();
            }
        }

        #endregion

    }
}