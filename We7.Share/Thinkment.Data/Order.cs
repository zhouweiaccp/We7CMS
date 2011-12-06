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
using System.Data;

namespace Thinkment.Data
{
    /// <summary>
    /// 排序构建类
    /// </summary>
    [Serializable]
	public class Order
	{
        Adorns adorn;
        OrderMode mode;
		string name;
        int start;
        int length;
        string aliasName;

		public Order()
		{
            adorn = Adorns.None;
        }

        public Order(string n)
        {
            adorn = Adorns.None;
            name = n;
            mode = OrderMode.Asc;
        }

		public Order(string n, OrderMode m)
		{
            adorn = Adorns.None;
            name = n;
			mode = m;
		}
        /// <summary>
        /// 升降模式
        /// </summary>
		public OrderMode Mode
		{
			get { return mode; }
			set { mode = value; }
		}
        /// <summary>
        /// 别名
        /// </summary>
        public string AliasName
        {
            get { return aliasName; }
            set { aliasName = value; }
        }
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name
		{
			get { return name; }
			set { name = value; }
		}

        /// <summary>
        /// 排序项函数目前仅支持SubString
        /// </summary>
        public Adorns Adorn
        {
            get { return adorn; }
            set { adorn = value; }
        }
        /// <summary>
        /// 开始
        /// </summary>
        public int Start
        {
            get { return start; }
            set { start = value; }
        }
        /// <summary>
        /// 长度
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        }

    }
}