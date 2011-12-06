using System;
using System.Runtime;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;
using We7.Framework;

namespace We7.CMS.Common
{
    /// <summary>
    /// CDø‚¥ÌŒÛ–≈œ¢¿‡
    /// </summary>
    [Serializable]
    public class CDException : We7Exception
    {
        public CDException()
            : base()
        {
        }

        public CDException(string message)
            : base(message)
        {
        }

        public CDException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CDException(SerializationInfo si, StreamingContext context)
            : base(si, context)
        {
        }
    }
}