// Author:
//   thehim,2009-5-21
//For SQLite database


using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Thinkment.Data
{
    /// <summary>
    /// SQLiteÊý¾Ý¿âÇý¶¯
    /// </summary>
    public class SQLiteDriver : BaseDriver
    {
        public SQLiteDriver()
        {

        }

        public override IConnection CreateConnection(string connectionString)
        {
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
                    return string.Format("SUBSTR([{0}]," + (start + 1) + "," + length + ")", field);

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
                    return String.Format("SUBSTR([{0}]," + (field.Start + 1)
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

        public override string BuildPaging(string table, string fields, string where,
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
                string rods = BuildOrderString(orders, true);
                string fmt = "SELECT  * FROM ( SELECT  * FROM ( SELECT  {2} FROM "
                    + "{3} AS TB__1 {4} ORDER BY {5}  LIMIT 0,{1}) AS TB__2 ORDER BY {6}   LIMIT 0,{0} ) AS TB__3 ORDER BY {5}  LIMIT 0,{0}";
                return String.Format(fmt, count, from + count, fields, table, ws, ods, rods);
            }
            else if (count > 0)
            {

                string fmt = "SELECT {1} FROM {2} {3} ORDER BY {4}  LIMIT 0,{0}";
                return String.Format(fmt, count, fields, table, ws, ods);
            }
            else
            {
                string fmt = "SELECT {0} FROM {1} {2} ORDER BY {3}";
                return String.Format(fmt, fields, table, ws, ods);
            }
        }

        //public override SqlStatement FormatSQL(SqlStatement sql)
        //{
        //    return base.FormatSQL(sql);
        //}

        class SQLiteDBConnection : IConnectionEx
        {
            bool isTransaction;
            string connectionString;
            SQLiteTransaction tran;
            SQLiteConnection connection;
            IDbDriver driver;

            public SQLiteDBConnection()
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

            public SQLiteConnection Connection
            {
                get { return connection; }
            }


            SQLiteCommand CreateCommand(SqlStatement sql)
            {
                SQLiteCommand cmd = new SQLiteCommand(sql.SqlClause);
                if (connection == null)
                {
                    try
                    {
                        connection = new SQLiteConnection(connectionString);
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
                    SQLiteParameter p = new SQLiteParameter();
                    //if (dp.DbType == DbType.DateTime)
                    //{
                    //    p.DbType = OleDbType.Date;
                    //}
                    //else
                    //{
                    //    p.DbType = dp.DbType;
                    //}
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
                using (SQLiteCommand cmd = this.CreateCommand(sql))
                {
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
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
                using (SQLiteCommand cmd = this.CreateCommand(sql))
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
                using (SQLiteCommand cmd = this.CreateCommand(sql))
                {
                    cmd.Connection = connection;
                    int ret = cmd.ExecuteNonQuery();
                    PopuloateCommand(cmd, sql);
                    if (!create)
                    {
                        Dispose(true);
                    }
                    return ret;
                }
            }

            void PopuloateCommand(SQLiteCommand cmd, SqlStatement st)
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
                    using (SQLiteCommand cmd = this.CreateCommand(sql))
                    {
                        cmd.Connection = connection;
                        int ret = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        class FrontPageSQLiteDBConnection : IConnectionEx
        {
            bool isTransaction;
            string connectionString;
            SQLiteTransaction tran;
            SQLiteConnection connection;
            IDbDriver driver;

            public FrontPageSQLiteDBConnection()
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

            public SQLiteConnection Connection
            {
                get
                {
                    if (connection == null)
                    {
                        connection = DBAccessHelper.CreateConnection<SQLiteConnection>(ConnectionString);
                    }
                    return connection;
                }
            }


            SQLiteCommand CreateCommand(SqlStatement sql)
            {
                SQLiteCommand cmd = new SQLiteCommand(sql.SqlClause);
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
                    SQLiteParameter p = new SQLiteParameter();
                    //if (dp.DbType == DbType.DateTime)
                    //{
                    //    p.DbType = OleDbType.Date;
                    //}
                    //else
                    //{
                    //    p.DbType = dp.DbType;
                    //}
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
                using (SQLiteCommand cmd = this.CreateCommand(sql))
                {
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
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
                using (SQLiteCommand cmd = this.CreateCommand(sql))
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
                using (SQLiteCommand cmd = this.CreateCommand(sql))
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

            void PopuloateCommand(SQLiteCommand cmd, SqlStatement st)
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
                    using (SQLiteCommand cmd = this.CreateCommand(sql))
                    {
                        int ret = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public override SqlStatement FormatSQL(SqlStatement sql)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            sql.SqlClause = new Regex(@"\s+month\(+[^\(|\)]+\)+", options).Replace(sql.SqlClause, new MatchEvaluator(ReplaceMonth));
            return sql;
        }

        string ReplaceMonth(Match m)
        {
            string result = m.Value.ToLower();
            result = result.Replace("month(", "strftime(\'%m\',");
            return result;
        }

        protected IConnectionEx CreateConnection()
        {
            if (DBAccessHelper.IsDataAccessPage)
            {
                return new FrontPageSQLiteDBConnection();
            }
            else
            {
                return new SQLiteDBConnection();
            }
        }
    }
}