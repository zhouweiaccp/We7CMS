using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework.Log
{
    public interface ILogFormatter
    {
        string Format(String message,Level level,Exception ex);
    }
}
