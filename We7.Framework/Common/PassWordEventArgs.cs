using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework.Common
{
    /// <summary>
    /// 事件参数。提供带参数的实体
    /// </summary>
    public class PassWordEventArgs : EventArgs
    {
        /// <summary>
        /// 传递参数构造
        /// </summary>
        /// <param name="password"></param>
        public PassWordEventArgs(string password)
        {
            this.repassword = password;
        }
        /// <summary>
        /// 
        /// </summary>
        private string repassword;

        /// <summary>
        /// 设置或者得到传递的参数
        /// </summary>
        public string Repassword
        {
            get { return repassword; }
            set { repassword = value; }
        }
    }
}
