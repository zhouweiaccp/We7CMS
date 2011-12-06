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
using System.Collections.Generic;
using System.Data;

namespace Thinkment.Data
{
    /// <summary>
    /// SQL对象类
    /// </summary>
	[Serializable]
	public class SqlStatement 
	{
		string _s1;
		CommandType _c1;
		List<DataParameter> _p1;

		public SqlStatement()
		{
			_c1 = CommandType.Text;
			_p1 = new List<DataParameter>();
		}

        public SqlStatement(string sql) : this()
        {
            SqlClause = sql;
        }
        /// <summary>
        /// SQL对象类型：存储过程？表？
        /// </summary>
		public CommandType CommandType
		{
			get { return _c1; }
			set { _c1 = value; }
		}
        /// <summary>
        /// SQL语句
        /// </summary>
		public string SqlClause
		{
			get { return _s1; }
			set { _s1 = value; }
		}
        /// <summary>
        /// 参数列表
        /// </summary>
		public List<DataParameter> Parameters
		{
			get { return _p1; }
		}
	}
}
