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
    /// SQL ServerÊý¾Ý¿âÇý¶¯
    /// </summary>
    public class SqlDbDriver : BaseDriver
    {
        public SqlDbDriver()
        {
        }

        public override IConnection CreateConnection(string connectionString)
        {
            IConnectionEx cc = CreateConnection();
            string cs = connectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
            cc.ConnectionString = cs;
            cc.Driver = this;
            cc.Create = true;
            return cc;
        }

        public override IConnection CreateConnection(string connectionString, bool create)
        {
            IConnectionEx cc = CreateConnection();
            string cs = connectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
            cc.ConnectionString = cs;
            cc.Driver = this;
            cc.Create = create;
            return cc;
        }

        class SqlDbConnection : IConnectionEx
        {
            bool isTransaction;
            string connectionStr;
            SqlTransaction mytransaction;
            SqlConnection myconnection;
            IDbDriver idriver;

            public SqlDbConnection()
            {
            }

            bool create;

            public bool Create
            {
                get { return create; }
                set { create = value; }
            }

            public IDbDriver Driver
            {
                get { return idriver; }
                set { idriver = value; }
            }

            public string ConnectionString
            {
                get { return connectionStr; }
                set { connectionStr = value; }
            }

            public bool IsTransaction
            {
                get { return isTransaction; }
                set { isTransaction = value; }
            }

            public SqlConnection Connection
            {
                get { return myconnection; }
            }

            SqlCommand CreateCommand(SqlStatement sql)
            {
                SqlCommand _c = new SqlCommand(sql.SqlClause);
                if (myconnection == null)
                {
                    myconnection = new SqlConnection(connectionStr);
                    myconnection.Open();
                    if (IsTransaction && mytransaction == null)
                    {
                        mytransaction = myconnection.BeginTransaction();
                    }
                }
                if (IsTransaction)
                {
                    if (mytransaction == null)
                    {
                        mytransaction = myconnection.BeginTransaction();
                    }
                    _c.Transaction = mytransaction;
                }
                _c.Connection = myconnection;
                _c.CommandTimeout = 300;
                _c.CommandType = sql.CommandType;
                foreach (DataParameter dp in sql.Parameters)
                {
                    SqlParameter p = new SqlParameter();
                    if (dp.Value == null)
                    {
                        p.Value = DBNull.Value;
                    }
                    else if (dp.Value.ToString() == DateTime.MinValue.ToString())
                    {
                        p.Value = DateTime.Now;
                    }
                    else
                    {
                        p.Value = dp.Value;
                    }
                    if (dp.DbType == DbType.DateTime && dp.Value != null)
                    {
						//if ((DateTime)dp.Value <= DateTime.Parse("1/1/1753 12:00:00") ||
						//    (DateTime)dp.Value >= DateTime.Parse("12/31/9999 11:59:59"))
						//{
						//    p.Value = DBNull.Value;
						//}
						if (Convert.ToDateTime(dp.Value) <= DateTime.Parse("1/1/1753 12:00:00") ||
						Convert.ToDateTime(dp.Value) >= DateTime.Parse("12/31/9999 11:59:59"))
						{
							p.Value = DBNull.Value;
						}
                    }

                    p.ParameterName = String.Format("{0}", dp.ParameterName);
                    p.Size = dp.Size;
                    p.Direction = dp.Direction;
                    _c.Parameters.Add(p);
                }
                return _c;
            }

            public DataTable Query(SqlStatement sql)
            {
                using (SqlCommand _c = this.CreateCommand(sql))
                {
                    SqlDataAdapter _s = new SqlDataAdapter(_c);
                    DataTable _d = new DataTable();
                    _s.Fill(_d);
                    PopuloateCommand(_c, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return _d;
                }
            }

            public object QueryScalar(SqlStatement sql)
            {
                using (SqlCommand _c = this.CreateCommand(sql))
                {
                    object _o = _c.ExecuteScalar();
                    PopuloateCommand(_c, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return _o;
                }
            }

            public int Update(SqlStatement sql)
            {
                using (SqlCommand cmd = this.CreateCommand(sql))
                {
                    //_c.Connection = _c2;
                    cmd.Connection = Connection;
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    int ret = cmd.ExecuteNonQuery();
                    PopuloateCommand(cmd, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return ret;
                }
            }

            void PopuloateCommand(SqlCommand cmd, SqlStatement st)
            {
                for (int i = 0; i < st.Parameters.Count; i++)
                {
                    DataParameter dp = st.Parameters[i];
                    if (dp.Direction != ParameterDirection.Input)
                    {
                        dp.Value = cmd.Parameters[i].Value;
                    }
                }
            }

            public void Commit()
            {
                if (mytransaction != null)
                {
                    mytransaction.Commit();
                    mytransaction.Dispose();
                    mytransaction = null;
                }
            }

            public void Rollback()
            {
                if (mytransaction != null)
                {
                    mytransaction.Rollback();
                    mytransaction.Dispose();
                    mytransaction = null;
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }

            protected void Dispose(bool isDisposing)
            {
                if (isDisposing)
                {
                    if (mytransaction != null)
                    {
                        Commit();
                    }
                    if (myconnection != null)
                    {
                        myconnection.Close();
                        myconnection.Dispose();
                        myconnection = null;
                    }

                    GC.SuppressFinalize(this);
                }
            }

            public bool TableExist(string table)
            {
                try
                {
                    SqlStatement sql = new SqlStatement();
                    sql.SqlClause = "select COUNT(*) from " + table;
                    using (SqlCommand _c = this.CreateCommand(sql))
                    {
                        _c.Connection = myconnection;
                        if (myconnection.State != ConnectionState.Open)
                            myconnection.Open();
                        int ret = _c.ExecuteNonQuery();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }


        class FrontPageSqlDbConnection : IConnectionEx
        {
            bool isTransaction;
            string connectionStr;
            SqlTransaction mytransaction;
            SqlConnection myconnection;
            IDbDriver idriver;

            public FrontPageSqlDbConnection()
            {
            }

            bool create;

            public bool Create
            {
                get { return create; }
                set { create = value; }
            }

            public IDbDriver Driver
            {
                get { return idriver; }
                set { idriver = value; }
            }

            public string ConnectionString
            {
                get { return connectionStr; }
                set { connectionStr = value; }
            }

            public bool IsTransaction
            {
                get { return isTransaction; }
                set { isTransaction = value; }
            }

            public SqlConnection Connection
            {
                get
                {
                    if (myconnection == null)
                    {
                        myconnection = DBAccessHelper.CreateConnection<SqlConnection>(ConnectionString);
                    }
                    return myconnection;
                }
            }

            SqlCommand CreateCommand(SqlStatement sql)
            {
                SqlCommand _c = new SqlCommand(sql.SqlClause);
                _c.Connection = Connection;
                _c.CommandTimeout = 300;
                _c.CommandType = sql.CommandType;

                if (IsTransaction)
                {
                    if (mytransaction == null)
                    {
                        mytransaction = Connection.BeginTransaction();
                    }
                    _c.Transaction = mytransaction;
                }
                foreach (DataParameter dp in sql.Parameters)
                {
                    SqlParameter p = new SqlParameter();
                    if (dp.Value == null)
                    {
                        p.Value = DBNull.Value;
                    }
                    else if (dp.Value.ToString() == DateTime.MinValue.ToString())
                    {
                        p.Value = DateTime.Now;
                    }
                    else
                    {
                        p.Value = dp.Value;
                    }
                    if (dp.DbType == DbType.DateTime || dp.Value != null && dp.Value.GetType() == typeof(DateTime))
                    {
                        if ((DateTime)dp.Value <= DateTime.Parse("1/1/1753 12:00:00") ||
                            (DateTime)dp.Value >= DateTime.Parse("12/31/9999 11:59:59"))
                        {
                            p.Value = DBNull.Value;
                        }
                    }

                    p.ParameterName = String.Format("{0}", dp.ParameterName);
                    p.Size = dp.Size;
                    p.Direction = dp.Direction;
                    _c.Parameters.Add(p);
                }
                return _c;
            }

            public DataTable Query(SqlStatement sql)
            {
                using (SqlCommand _c = this.CreateCommand(sql))
                {
                    SqlDataAdapter _s = new SqlDataAdapter(_c);
                    DataTable _d = new DataTable();
                    _s.Fill(_d);
                    PopuloateCommand(_c, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return _d;
                }
            }

            public object QueryScalar(SqlStatement sql)
            {
                using (SqlCommand _c = this.CreateCommand(sql))
                {
                    object _o = _c.ExecuteScalar();
                    PopuloateCommand(_c, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return _o;
                }
            }

            public int Update(SqlStatement sql)
            {
                using (SqlCommand cmd = this.CreateCommand(sql))
                {
                    //_c.Connection = _c2;
                    cmd.Connection = Connection;
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();
                    int ret = cmd.ExecuteNonQuery();
                    PopuloateCommand(cmd, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return ret;
                }
            }

            void PopuloateCommand(SqlCommand cmd, SqlStatement st)
            {
                for (int i = 0; i < st.Parameters.Count; i++)
                {
                    DataParameter dp = st.Parameters[i];
                    if (dp.Direction != ParameterDirection.Input)
                    {
                        dp.Value = cmd.Parameters[i].Value;
                    }
                }
            }

            public void Commit()
            {
                if (mytransaction != null)
                {
                    mytransaction.Commit();
                    mytransaction.Dispose();
                    mytransaction = null;
                }
            }

            public void Rollback()
            {
                if (mytransaction != null)
                {
                    mytransaction.Rollback();
                    mytransaction.Dispose();
                    mytransaction = null;
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }

            protected void Dispose(bool isDisposing)
            {
                if (isDisposing)
                {
                    if (mytransaction != null)
                    {
                        Commit();
                    }
                    GC.SuppressFinalize(this);
                }
            }

            public bool TableExist(string table)
            {
                try
                {
                    SqlStatement sql = new SqlStatement();
                    sql.SqlClause = "select COUNT(*) from " + table;
                    using (SqlCommand _c = this.CreateCommand(sql))
                    {
                        int ret = _c.ExecuteNonQuery();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        public override string FormatTable(string table)
        {
            return String.Format("[{0}] ", table);
        }

        public override string FormatField(Adorns ad, string field)
        {
            switch (ad)
            {
                case Adorns.Average:
                    return String.Format("AVE([{0}]) AS [{0}]", field);

                case Adorns.Distinct:
                    return String.Format("DISTINCT([{0}]) AS [{0}]", field);

                case Adorns.Maximum:
                    return String.Format("MAX([{0}]) AS [{0}]", field);

                case Adorns.Minimum:
                    return String.Format("MIN([{0}]) AS [{0}]", field);

                case Adorns.None:
                case Adorns.Substring:
                    return String.Format("[{0}]", field);

                case Adorns.Sum:
                    return String.Format("SUM([{0}]) AS [{0}]", field);

                case Adorns.Total:
                    return String.Format("TOTAL([{0}]) AS [{0}]", field);

                default:
                    return String.Format("[{0}]", field);
            }
        }

        public override string FormatField(Adorns ad, string field, int start, int length)
        {
            switch (ad)
            {
                case Adorns.Substring:
                    return string.Format("SUBSTRING([{0}]," + (start + 1) + "," + length + ")", field);

                case Adorns.Average:
                case Adorns.Distinct:
                case Adorns.Maximum:
                case Adorns.Minimum:
                case Adorns.Sum:
                case Adorns.Total:
                case Adorns.None:
                default:
                    return String.Format("[{0}]", field);

            }
        }

        public override string FormatField(ConListField field)
        {
            switch (field.Adorn)
            {
                case Adorns.Average:
                    return String.Format("AVE([{0}]) AS [{1}]", field.FieldName, field.AliasName);

                case Adorns.Distinct:
                    return String.Format("DISTINCT([{0}]) AS [{1}]", field.FieldName, field.AliasName);

                case Adorns.Maximum:
                    return String.Format("MAX([{0}]) AS [{1}]", field.FieldName, field.AliasName);

                case Adorns.Minimum:
                    return String.Format("MIN([{0}]) AS [{1}]", field.FieldName, field.AliasName);

                case Adorns.None:
                    if (field.FieldName == field.AliasName)
                    {
                        return String.Format("[{0}]", field.FieldName);
                    }
                    else
                    {
                        return String.Format("[{0}] AS [{1}]", field.FieldName, field.AliasName);
                    }
                    break;

                case Adorns.Substring:
                    return String.Format("SUBSTRING([{0}]," + (field.Start + 1)
                        + "," + field.Length + ") AS [{1}]", field.FieldName, field.AliasName);
                    break;

                case Adorns.Sum:
                    return String.Format("SUM([{0}]) AS [{1}]", field.FieldName, field.AliasName);
                    break;

                case Adorns.Total:
                default:
                    return String.Format("TOTAL([{0}]) AS [{1}]", field.FieldName, field.AliasName);

            }
        }

        public override SqlStatement FormatSQL(SqlStatement sql)
        {
            return base.FormatSQL(sql);
        }

        public IConnectionEx CreateConnection()
        {
            if (DBAccessHelper.IsDataAccessPage)
            {
                return new FrontPageSqlDbConnection();
            }
            else
            {
                return new SqlDbConnection();
            }
        }

    }
}