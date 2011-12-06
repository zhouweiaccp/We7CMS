// Author:
//   Marek Sieradzki (marek.sieradzki@gmail.com)
//
// (C) 2005 Marek Sieradzki
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections;
using System.Text;

namespace Thinkment.Data
{
    /// <summary>
    /// 数据驱动层异常处理
    /// </summary>
    [Serializable]
    public class DataException : Exception
    {
        ErrorCodes errorCode = ErrorCodes.Success;

        protected DataException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }

        public DataException()
            : base()
        {
        }

        public DataException(ErrorCodes code)
            : base()
        {
            errorCode = code;
        }

        public DataException(string message)
            : base(message)
        {
        }

        public DataException(string message, ErrorCodes code)
            : base(message)
        {
            errorCode = code;
        }

        public DataException(string message, Exception ie)
            : base(message, ie)
        {
        }

        public DataException(string message, Exception ie, ErrorCodes code)
            : base(message, ie)
        {
            errorCode = code;
        }

        public ErrorCodes ErrorCode
        {
            get { return errorCode; }
            private set { errorCode = value; }
        }
    }
}