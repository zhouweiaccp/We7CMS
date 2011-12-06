using System;
using System.Runtime;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;

namespace We7
{
    /// <summary>
    /// We7异常处理基础类
    /// </summary>
	[Serializable]
	public class We7Exception : Exception
	{
		public We7Exception()
			: base()
		{
		}

		public We7Exception(string message)
			: base(message)
		{
		}

		public We7Exception(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected We7Exception(SerializationInfo si, StreamingContext context)
			: base(si, context)
		{
		}
	}
}