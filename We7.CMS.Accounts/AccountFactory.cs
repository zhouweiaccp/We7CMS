using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.CMS.Accounts;
using We7.Framework.Config;

namespace We7.CMS
{
    /// <summary>
    /// 账户类工厂
    /// </summary>
    public class AccountFactory
    {
        public static AccountLocalHelper Instance
        {
            get
            {
                return HelperFactory.Instance.GetHelper<AccountLocalHelper>();
            }
        }

        /// <summary>
        /// 接口实例
        /// </summary>
        /// <returns></returns>
        public static IAccountHelper CreateInstance()
        {
            if (SiteConfigs.GetConfig().SiteGroupEnabled)
            {
                if (string.IsNullOrEmpty(SiteConfigs.GetConfig().PassportServiceUrl))
                    throw new Exception("您还没有在site.config中设置好身份认证服务地址（PassportServiceUrl）的值！");
                AccountRemoteHelper ar = new AccountRemoteHelper();
                return ar;
            }
            else
            {
                AccountLocalHelper al = HelperFactory.Instance.GetHelper<AccountLocalHelper>();
                return al;
            }
        }
    }

}
