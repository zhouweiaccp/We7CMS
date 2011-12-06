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
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace Thinkment.Data
{
    /// <summary>
    /// 数据库驱动基类（抽象类）
    /// </summary>
    public abstract class BaseDriver : IDbDriver
    {

        public BaseDriver()
        {
        }

        /// <summary>
        /// 变量名前缀
        /// </summary>
        public virtual string Prefix
        {
            get { return "@"; }
        }

        /// <summary>
        /// 构建用于分页的sql语句
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fields"></param>
        /// <param name="where"></param>
        /// <param name="orders"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual string BuildPaging(string table, string fields, string where,
            List<Order> orders, int from, int count)
        {
            if (orders == null || orders.Count == 0)
            {
                throw new Exception("Order information is required by paging function (OleDbDriver).");
            }
            string ods = BuildOrderString(orders, false);
            string ws = "";
            if (where != null && where.Length > 0)
            {
                ws = " WHERE " + where;
            }
            if (from > 0)
            {
                string rods = BuildOrderString(orders, true, true);
                string rrods = BuildOrderString(orders, false, true);
                string fmt = "SELECT TOP {0} * FROM ( SELECT TOP {0} * FROM ( SELECT TOP {1} {2} FROM "
                    + "{3} AS TB__1 {4} ORDER BY {5}) AS TB__2 ORDER BY {6} ) AS TB__3 ORDER BY {7} ";
                return String.Format(fmt, count, from + count, fields, table, ws, ods, rods, rrods);
            }
            else if (count > 0)
            {
                string fmt = "SELECT TOP {0} {1} FROM {2} {3} ORDER BY {4}";
                return String.Format(fmt, count, fields, table, ws, ods);
            }
            else
            {
                string fmt = "SELECT {0} FROM {1} {2} ORDER BY {3}";
                return String.Format(fmt, fields, table, ws, ods);
            }
        }
        /// <summary>
        /// 根据表达式范式类型，返回字符串
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual string GetCriteria(CriteriaType type)
        {
            switch (type)
            {
                case CriteriaType.NotEquals:
                    return "<>";

                case CriteriaType.LessThan:
                    return "<";

                case CriteriaType.LessThanEquals:
                    return "<=";

                case CriteriaType.MoreThan:
                    return ">";

                case CriteriaType.MoreThanEquals:
                    return ">=";

                case CriteriaType.Like:
                    return "LIKE";

                case CriteriaType.NotLike:
                    return "NOT LIKE";

                case CriteriaType.Desc:
                    return "DESC";

                case CriteriaType.Asc:
                    return "ASC";

                case CriteriaType.Equals:
                    return "=";

                default:
                    throw new DataException(ErrorCodes.UnkownCriteria);
            }
        }

        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <param name="connectionString">连接串</param>
        /// <returns></returns>
        public abstract IConnection CreateConnection(string connectionString);
        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public abstract IConnection CreateConnection(string connectionString, bool create);

        /// <summary>
        /// 构建排序语句
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        protected string BuildOrderString(List<Order> orders)
        {
            return BuildOrderString(orders, false);
        }
        /// <summary>
        /// 构建排序语句
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        protected string BuildOrderString(List<Order> orders, bool reverse)
        {
            return BuildOrderString(orders, reverse, false);
        }
        /// <summary>
        /// 构建排序语句
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="reverse"></param>
        /// <param name="isalias"></param>
        /// <returns></returns>
        protected string BuildOrderString(List<Order> orders, bool reverse, bool isalias)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Order order in orders)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                if (order.Adorn == Adorns.None)
                {
                    sb.Append(FormatField(Adorns.None, order.Name));
                }
                else
                {
                    if (isalias)
                    {
                        string name = FormatField(order.Adorn, order.AliasName, order.Start, order.Length);
                        sb.Append(name);
                    }
                    else
                    {
                        string name = FormatField(order.Adorn, order.Name, order.Start, order.Length);
                        sb.Append(name);
                    }
                }
                if (order.Mode == OrderMode.Asc && !reverse ||
                    order.Mode == OrderMode.Desc && reverse)
                {
                    sb.Append(" " + GetCriteria(CriteriaType.Asc) + " ");
                }
                else
                {
                    sb.Append(" " + GetCriteria(CriteriaType.Desc) + " ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化数据库表名
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public virtual string FormatTable(string table)
        {
            return String.Format("[{0}]", table);
        }
        /// <summary>
        /// 格式化字段名（包括函数）
        /// </summary>
        /// <param name="ad"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public abstract string FormatField(Adorns ad, string field);
        /// <summary>
        /// 格式化字段名（包括函数）
        /// </summary>
        /// <param name="ad"></param>
        /// <param name="field"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public abstract string FormatField(Adorns ad, string field, int start, int length);
        /// <summary>
        /// 格式化字段名（包括函数）,用于扩展字段类
        /// </summary>
        /// <param name="conListField"></param>
        /// <returns></returns>
        public abstract string FormatField(ConListField conListField);

        /// <summary>
        /// 格式化SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual SqlStatement FormatSQL(SqlStatement sql)
        {
            return sql;
        }

        /// <summary>
        /// 选择标识字段的语句
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public virtual string GetIdentityExpression(string table)
        {
            return "SELECT @@IDENTITY";
        }
    }
}
