using System;
using System.Collections.Generic;
using System.Text;

namespace We7
{
    /// <summary>
    /// 系统错误
    /// </summary>
    public class SysException:Exception
    {
        public SysException(string message) : base(message) { }
    }
}
