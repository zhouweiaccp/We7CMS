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
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Thinkment.Data
{
    /// <summary>
    /// Access数据库驱动
    /// </summary>
    public class OleDbDriver : BaseDriver
    {
        public OleDbDriver()
        {

        }

        public override IConnection CreateConnection(string connectionString)
        {
            //OleConnection conn = new OleConnection();
            IConnectionEx conn = CreateConnection();
            conn.ConnectionString = connectionString;
            conn.Driver = this;
            conn.Create = true;
            return conn;
        }

        public override IConnection CreateConnection(string connectionString, bool create)
        {
            IConnectionEx conn = CreateConnection();
            conn.ConnectionString = connectionString;
            conn.Driver = this;
            conn.Create = create;
            return conn;
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
                    return string.Format("MID([{0}]," + (start + 1) + "," + length + ")", field);

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
                    return String.Format("MID([{0}]," + (field.Start + 1)
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
            RegexOptions options = RegexOptions.IgnoreCase;
            Regex alterSql = new Regex(@"(alter\s+table|create\s+table|insert\s+into|delete\s+from)\s", options);

            if (alterSql.IsMatch(sql.SqlClause))
            {
                sql.SqlClause = new Regex(@"\s+[^\[]?nvarchar", options).Replace(sql.SqlClause, " varchar");
                sql.SqlClause = new Regex(@"\s+[^\[]?text", options).Replace(sql.SqlClause, " Memo");
                sql.SqlClause = new Regex(@"\s+[^\[]?decimal", options).Replace(sql.SqlClause, " Double");
                sql.SqlClause = new Regex(@"\s+[^\[]?bigint", options).Replace(sql.SqlClause, " Long");
            }
            return sql;
        }

        private IConnectionEx CreateConnection()
        {
            if (DBAccessHelper.IsDataAccessPage)
            {
                return new FrontPageOleConnection();
            }
            else
            {
                return new OleConnection();
            }
        }

        class OleConnection : IConnectionEx
        {
            bool isTransaction;
            string connectionString;
            OleDbTransaction tran;
            OleDbConnection connection;
            IDbDriver driver;

            public OleConnection()
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
                get { return driver; }
                set { driver = value; }
            }

            public string ConnectionString
            {
                get { return connectionString; }
                set { connectionString = value; }
            }

            public bool IsTransaction
            {
                get { return isTransaction; }
                set { isTransaction = value; }
            }

            public OleDbConnection Connection
            {
                get { return connection; }
            }

            OleDbCommand CreateCommand(SqlStatement sql)
            {
                OleDbCommand cmd = new OleDbCommand(sql.SqlClause);
                if (connection == null)
                {
                    try
                    {
                        connection = new OleDbConnection(connectionString);
                        connection.Open();
                    }
                    catch
                    {
                    }
                    if (IsTransaction && tran == null)
                    {
                        tran = connection.BeginTransaction();
                    }
                }
                if (IsTransaction)
                {
                    if (tran == null)
                    {
                        tran = connection.BeginTransaction();
                    }
                    cmd.Transaction = tran;
                }
                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                cmd.CommandType = sql.CommandType;
                foreach (DataParameter dp in sql.Parameters)
                {
                    OleDbParameter p = new OleDbParameter();
                    if (dp.DbType == DbType.DateTime)
                    {
                        p.OleDbType = OleDbType.Date;
                    }
                    else
                    {
                        p.DbType = dp.DbType;
                    }
                    p.ParameterName = dp.ParameterName;
                    if (dp.Size.ToString().Length >= 65536)  //如果超过最大值。则不截断。让其算出实际大小
                    {
                        p.Size = dp.Size;
                    }
                    p.Direction = dp.Direction;
                    p.IsNullable = dp.IsNullable;
                    p.Value = dp.Value == null ? DBNull.Value : dp.Value;

                    cmd.Parameters.Add(p);
                }
                return cmd;
            }

            public DataTable Query(SqlStatement sql)
            {
                using (OleDbCommand cmd = this.CreateCommand(sql))
                {
                    OleDbDataAdapter sda = new OleDbDataAdapter(cmd);
                    DataTable ds = new DataTable();
                    sda.Fill(ds);
                    PopuloateCommand(cmd, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return ds;
                }
            }

            public object QueryScalar(SqlStatement sql)
            {
                using (OleDbCommand cmd = this.CreateCommand(sql))
                {
                    object obj = cmd.ExecuteScalar();
                    PopuloateCommand(cmd, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return obj;
                }
            }

            public int Update(SqlStatement sql)
            {
                using (OleDbCommand cmd = this.CreateCommand(sql))
                {
                    int ret = cmd.ExecuteNonQuery();
                    PopuloateCommand(cmd, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return ret;
                }
            }

            void PopuloateCommand(OleDbCommand cmd, SqlStatement st)
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
                if (tran != null)
                {
                    tran.Commit();
                    tran.Dispose();
                    tran = null;
                }
            }

            public void Rollback()
            {
                if (tran != null)
                {
                    tran.Rollback();
                    tran.Dispose();
                    tran = null;
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
                    if (tran != null)
                    {
                        Commit();
                    }
                    if (connection != null)
                    {
                        if (connection != null)
                        {
                            connection.Close();
                            connection.Dispose();
                            connection = null;
                        }
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
                    using (OleDbCommand cmd = this.CreateCommand(sql))
                    {
                        int ret = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }

        }

        class FrontPageOleConnection : IConnectionEx
        {
            bool isTransaction;
            string connectionString;
            OleDbTransaction tran;
            OleDbConnection connection;
            IDbDriver driver;

            public FrontPageOleConnection()
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
                get { return driver; }
                set { driver = value; }
            }

            public string ConnectionString
            {
                get { return connectionString; }
                set { connectionString = value; }
            }

            public bool IsTransaction
            {
                get { return isTransaction; }
                set { isTransaction = value; }
            }

            public OleDbConnection Connection
            {
                get
                {
                    if (connection == null)
                    {
                        connection = DBAccessHelper.CreateConnection<OleDbConnection>(ConnectionString);
                    }
                    return connection;
                }
            }

            OleDbCommand CreateCommand(SqlStatement sql)
            {
                OleDbCommand cmd = new OleDbCommand(sql.SqlClause);
                cmd.Connection = Connection;
                cmd.CommandTimeout = 300;
                cmd.CommandType = sql.CommandType;

                if (IsTransaction)
                {
                    if (tran == null)
                    {
                        tran = Connection.BeginTransaction();
                    }
                    cmd.Transaction = tran;
                }

                foreach (DataParameter dp in sql.Parameters)
                {
                    OleDbParameter p = new OleDbParameter();
                    if (dp.DbType == DbType.DateTime)
                    {
                        p.OleDbType = OleDbType.Date;
                    }
                    else
                    {
                        p.DbType = dp.DbType;
                    }
                    p.ParameterName = dp.ParameterName;
                    p.Size = dp.Size;
                    p.Direction = dp.Direction;
                    p.IsNullable = dp.IsNullable;
                    p.Value = dp.Value == null ? DBNull.Value : dp.Value;

                    cmd.Parameters.Add(p);
                }
                return cmd;
            }

            public DataTable Query(SqlStatement sql)
            {
                using (OleDbCommand cmd = this.CreateCommand(sql))
                {
                    OleDbDataAdapter sda = new OleDbDataAdapter(cmd);
                    DataTable ds = new DataTable();
                    sda.Fill(ds);
                    PopuloateCommand(cmd, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return ds;
                }
            }

            public object QueryScalar(SqlStatement sql)
            {
                using (OleDbCommand cmd = this.CreateCommand(sql))
                {
                    object obj = cmd.ExecuteScalar();
                    PopuloateCommand(cmd, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return obj;
                }
            }

            public int Update(SqlStatement sql)
            {
                using (OleDbCommand cmd = this.CreateCommand(sql))
                {
                    int ret = cmd.ExecuteNonQuery();
                    PopuloateCommand(cmd, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return ret;
                }
            }

            void PopuloateCommand(OleDbCommand cmd, SqlStatement st)
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
                if (tran != null)
                {
                    tran.Commit();
                    tran.Dispose();
                    tran = null;
                }
            }

            public void Rollback()
            {
                if (tran != null)
                {
                    tran.Rollback();
                    tran.Dispose();
                    tran = null;
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
                    if (tran != null)
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
                    using (OleDbCommand cmd = this.CreateCommand(sql))
                    {
                        int ret = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}