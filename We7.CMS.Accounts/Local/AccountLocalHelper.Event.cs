using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.PF;
using We7.Framework;
using We7.Framework.Common;

namespace We7.CMS.Accounts
{
    public partial class AccountLocalHelper : BaseHelper, IAccountHelper
    {

        public static event EventHandler<EventArgs> UserAdded;
        /// <summary>
        /// 触发用户已添加事件
        /// </summary>
        /// <param name="account"></param>
        protected virtual void OnUserAdded(Account account)
        {
            if (UserAdded != null)//&& UserAdded.Target != null
            {
                UserAdded(account, new EventArgs());
            }
        }

        public static event EventHandler<EventArgs> UserUpdated;
        /// <summary>
        /// 触发用户已更新事件
        /// </summary>
        /// <param name="account"></param>
        protected virtual void OnUserUpdated(Account account)
        {
            
            if (UserUpdated != null)
            {
                UserUpdated(account, new EventArgs());
            }
        }

        public static event EventHandler<EventArgs> UserUpgraded;
        /// <summary>
        /// 触发用户已升级事件
        /// </summary>
        /// <param name="account"></param>
        protected virtual void OnUserUpgraded(Account account)
        {

            if (UserUpgraded != null)
            {
                UserUpgraded(account, new EventArgs());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler<EventArgs> UserUpdatedByPwd;
        /// <summary>
        /// 触发用户已更新事件 wjz 2010年7月23
        /// </summary>
        /// <param name="account"></param>
        /// <param name="repassword"></param>
        protected virtual void OnUserUpdated(Account account,string repassword)
        {

            if (UserUpdatedByPwd != null)
            {
                UserUpdatedByPwd(account,new PassWordEventArgs(repassword) );//自定义wventArges继承类。带参数过去
            }
        }

   
        public static event EventHandler<EventArgs> UserDeleted;
        /// <summary>
        /// 触发用户已删除事件
        /// </summary>
        /// <param name="account"></param>
        protected virtual void OnUserDeleted(Account account)
        {
            if (UserDeleted != null)
            {
                UserDeleted(account, new EventArgs());
            }
        }

        public static event EventHandler<EventArgs> PasswordUpdated;
        /// <summary>
        /// 触发密码已修改事件
        /// </summary>
        /// <param name="account"></param>
        protected virtual void OnPasswordUpdated(Account account)
        {
            if (PasswordUpdated != null)
            {
                PasswordUpdated(account, new EventArgs());
            }
        }

        public static event EventHandler<EventArgs> UserLogined;
        /// <summary>
        /// 触发用户登录事件
        /// </summary>
        /// <param name="account"></param>
        protected virtual void OnUserLogined(Account account)
        {
            if (UserLogined != null)//&& UserAdded.Target != null
            {
                UserLogined(account, new EventArgs());
            }
        }

        public static event EventHandler<EventArgs> UserSignOut;
        /// <summary>
        /// 触发用户退出事件
        /// </summary>
        protected virtual void OnUserSignOut()
        {
            if (UserSignOut != null)//&& UserAdded.Target != null
            {
                UserSignOut("", new EventArgs());
            }
        }

    }
}
