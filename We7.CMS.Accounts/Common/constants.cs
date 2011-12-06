using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS
{
    /// <summary>
    /// 关键字
    /// </summary>
    public sealed class Keys
    {
        private Keys() { }
        /// <summary>
        /// 页码关键字
        /// </summary>
        public const string QRYSTR_PAGEINDEX = "pg";

        /// <summary>
        /// Session关键字
        /// </summary>
        internal const string SESSION_COOKIETEST = "CookieTest";

        /// <summary>
        /// 默认访问者
        /// </summary>
        public static int OwnerAccount = 0;

        /// <summary>
        /// 默认角色
        /// </summary>
        public static int OwnerRole = 1;
    }
}
