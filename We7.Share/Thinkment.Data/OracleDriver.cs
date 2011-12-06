// Author:
//   thehim,2009-6-16
//For Oracle database

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Text.RegularExpressions;
using System.Data.OracleClient;

namespace Thinkment.Data
{
    /// <summary>
    /// Oracle数据库驱动
    /// </summary>
    public class OracleDriver : BaseDriver
    {
        public OracleDriver()
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

        public override string Prefix
        {
            get { return ":"; }
        }

        public override string FormatTable(string table)
        {
            return String.Format("\"{0}\" ", table);
        }

        public override string FormatField(Adorns ad, string field)
        {
            switch (ad)
            {
                case Adorns.Average:
                    return String.Format("AVE(\"{0}\") AS \"{0}\"", field);

                case Adorns.Distinct:
                    return String.Format("DISTINCT(\"{0}\") AS \"{0}\"", field);

                case Adorns.Maximum:
                    return String.Format("MAX(\"{0}\") AS \"{0}\"", field);

                case Adorns.Minimum:
                    return String.Format("MIN(\"{0}\") AS \"{0}\"", field);

                case Adorns.None:
                case Adorns.Substring:
                    return String.Format("\"{0}\"", field);

                case Adorns.Sum:
                    return String.Format("SUM(\"{0}\") AS \"{0}\"", field);

                case Adorns.Total:
                    return String.Format("TOTAL(\"{0}\") AS \"{0}\"", field);

                default:
                    return String.Format("\"{0}\"", field);
            }
        }

        public override string FormatField(Adorns ad, string field, int start, int length)
        {
            switch (ad)
            {
                case Adorns.Substring:
                    return string.Format("SUBSTR(\"{0}\"," + (start + 1) + "," + length + ")", field);

                case Adorns.Average:
                case Adorns.Distinct:
                case Adorns.Maximum:
                case Adorns.Minimum:
                case Adorns.Sum:
                case Adorns.Total:
                case Adorns.None:
                default:
                    return String.Format("\"{0}\"", field);

            }
        }

        public override string FormatField(ConListField field)
        {
            switch (field.Adorn)
            {
                case Adorns.Average:
                    return String.Format("AVE(\"{0}\") AS \"{1}\"", field.FieldName, field.AliasName);

                case Adorns.Distinct:
                    return String.Format("DISTINCT(\"{0}\") AS \"{1}\"", field.FieldName, field.AliasName);

                case Adorns.Maximum:
                    return String.Format("MAX(\"{0}\") AS \"{1}\"", field.FieldName, field.AliasName);

                case Adorns.Minimum:
                    return String.Format("MIN(\"{0}\") AS \"{1}\"", field.FieldName, field.AliasName);

                case Adorns.None:
                    if (field.FieldName == field.AliasName)
                    {
                        return String.Format("\"{0}\"", field.FieldName);
                    }
                    else
                    {
                        return String.Format("\"{0}\" AS \"{1}\"", field.FieldName, field.AliasName);
                    }
                    break;

                case Adorns.Substring:
                    return String.Format("SUBSTR(\"{0}\"," + (field.Start + 1)
                        + "," + field.Length + ") AS \"{1}\"", field.FieldName, field.AliasName);
                    break;

                case Adorns.Sum:
                    return String.Format("SUM(\"{0}\") AS \"{1}\"", field.FieldName, field.AliasName);
                    break;

                case Adorns.Total:
                default:
                    return String.Format("TOTAL(\"{0}\") AS \"{1}\"", field.FieldName, field.AliasName);

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
                string fmt = @"SELECT {2}  FROM 
                                        ( 
                                        SELECT ROWNUM RECNO,{2}  FROM  
                                        (
                                        SELECT {2}   FROM {3}  {4}  ORDER BY {5}
                                        ) 
                                        WHERE ROWNUM <= ({1} +{0} ) 
                                        ORDER BY ROWNUM ASC 
                                         ) 
                                        WHERE RECNO BETWEEN {1} +1 AND ({1} + {0})";
                return String.Format(fmt, count, from, fields, table, ws, ods, rods);
            }
            else if (count > 0)
            {
                string fmt = "select {1} from (SELECT * FROM {2} {3} ORDER BY {4}) where rownum<={0}";
                return String.Format(fmt, count, fields, table, ws, ods);
            }
            else
            {
                string fmt = "SELECT {0} FROM {1} {2} ORDER BY {3}";
                return String.Format(fmt, fields, table, ws, ods);
            }
        }

        public override SqlStatement FormatSQL(SqlStatement sql)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            //TODO::检查正则表达式出错
            //Regex alterSql = new Regex(@"(alter\s+table|create\s+table|insert\s+into|delete\s+from|select\s+(.*)+\s+from)\s", options);
            Regex alterSql = new Regex(@"(alter\s+table|create\s+table|insert\s+into|delete\s+from|select\s+.+\s+from)\s", options);
            if (alterSql.IsMatch(sql.SqlClause))
            {
                sql.SqlClause = sql.SqlClause.Replace("(", " (");
                sql.SqlClause = new Regex(@"\s+varchar[^(\w]+", options).Replace(sql.SqlClause, " VARCHAR2 ");
                sql.SqlClause = new Regex(@"\s+nvarchar[^(\w]+", options).Replace(sql.SqlClause, " VARCHAR2 ");
                sql.SqlClause = new Regex(@"\s+datetime[\W]+", options).Replace(sql.SqlClause, " DATE");
                sql.SqlClause = new Regex(@"\s+text[\W]+", options).Replace(sql.SqlClause, " CLOB");
                sql.SqlClause = new Regex(@"\s+ntext[\W]+", options).Replace(sql.SqlClause, "  NCLOB");
                sql.SqlClause = new Regex(@"\s+int[\W]+", options).Replace(sql.SqlClause, " NUMBER ");
                sql.SqlClause = new Regex(@"\s+decimal[^(\w]+", options).Replace(sql.SqlClause, " NUMBER");
                sql.SqlClause = new Regex(@"\s+bigint[\W]+", options).Replace(sql.SqlClause, " NUMBER");
                sql.SqlClause = new Regex(@"\s+money[\W]", options).Replace(sql.SqlClause, " NUMBER(8,2)");
                //sql.SqlClause = new Regex(@"\s+month\(+[^\(|\)]+\)+", options).Replace(sql.SqlClause, new MatchEvaluator(ReplaceMonth));
            }

            sql.SqlClause = new Regex(@"\s+month\(+[^\(|\)]+\)+", options).Replace(sql.SqlClause, new MatchEvaluator(ReplaceMonth));
            sql.SqlClause = sql.SqlClause.Replace("[", "\"").Replace("]", "\"");
            return sql;
        }

        string ReplaceMonth(Match m)
        {
            string result = m.Value;
            result = result.Replace("MONTH", "to_char");
            result = result.Replace(")", ", \'MM\')");
            return result;
        }

        class OracleDbConnection : IConnectionEx
        {
            bool _t1;
            string _c1;
            OracleTransaction _t2;
            OracleConnection _c2;
            IDbDriver _d1;

            public OracleDbConnection()
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
                get { return _d1; }
                set { _d1 = value; }
            }

            public string ConnectionString
            {
                get { return _c1; }
                set { _c1 = value; }
            }

            public bool IsTransaction
            {
                get { return _t1; }
                set { _t1 = value; }
            }

            public OracleConnection Connection
            {
                get { return _c2; }
            }

            OracleCommand CreateCommand(SqlStatement sql)
            {
                OracleCommand _c = new OracleCommand(sql.SqlClause);
                if (_c2 == null)
                {
                    _c2 = new OracleConnection(_c1);
                    _c2.Open();
                    if (IsTransaction && _t2 == null)
                    {
                        _t2 = _c2.BeginTransaction();
                    }
                }
                if (IsTransaction)
                {
                    if (_t2 == null)
                    {
                        _t2 = _c2.BeginTransaction();
                    }
                    _c.Transaction = _t2;
                }
                _c.Connection = _c2;
                _c.CommandTimeout = 300;
                _c.CommandType = sql.CommandType;
                foreach (DataParameter dp in sql.Parameters)
                {
                    OracleParameter p = new OracleParameter();
                    p.ParameterName = String.Format("{0}", dp.ParameterName);
                    p.Size = dp.Size;
                    p.Direction = dp.Direction;

                    if (dp.Value == null)
                        p.Value = DBNull.Value;
                    else
                        p.Value = dp.Value;

                    if (dp.DbType == DbType.DateTime)
                    {
                        if (dp.Value.ToString() == DateTime.MinValue.ToString())
                        {
                            //p.Value = OracleDateTime.MinValue;
                            p.Value = OracleDateTime.Null;
                        }
                        else if (dp.Value == null)
                        {
                            p.Value = OracleDateTime.Null;
                        }
                        else
                        {
                            p.Value = ((DateTime)dp.Value).ToString("yyyy-MM-dd HH:mm:ss"); ;
                        }
                        p.Size = 64;
                        _c.CommandText = _c.CommandText.Replace(p.ParameterName, string.Format("to_date({0},'yyyy-mm-dd hh24:mi:ss')", p.ParameterName));
                        //if( _c.CommandText.IndexOf(string.Format("to_char({0},'yyyy-mm-dd hh24:mi:ss')", p.SourceColumn))<0)
                        //    _c.CommandText = _c.CommandText.Replace(p.SourceColumn, string.Format("to_char({0},'yyyy-mm-dd hh24:mi:ss')", p.SourceColumn));
                    }
                    else if (dp.DbType == DbType.String)
                    {
                        if (dp.Size > 65500)
                        {
                            p.OracleType = OracleType.Clob;
                            if (dp.Value == null || dp.Value.ToString().Length == 0)
                                p.Value = DBNull.Value;
                        }
                        else
                        {
                            if (dp.Value != null)
                                p.Value = getSubStringByBytes(dp.Value.ToString(), p.Size);
                        }
                    }

                    _c.Parameters.Add(p);
                }
                return _c;
            }


            public DataTable Query(SqlStatement sql)
            {
                using (OracleCommand _c = this.CreateCommand(sql))
                {
                    OracleDataAdapter _s = new OracleDataAdapter(_c);
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
                //using 使用误区？？
                using (OracleCommand _c = this.CreateCommand(sql))
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
                using (OracleCommand cmd = this.CreateCommand(sql))
                {
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

            void PopuloateCommand(OracleCommand cmd, SqlStatement st)
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
                if (_t2 != null)
                {
                    _t2.Commit();
                    _t2.Dispose();
                    _t2 = null;
                }
            }

            public void Rollback()
            {
                if (_t2 != null)
                {
                    _t2.Rollback();
                    _t2.Dispose();
                    _t2 = null;
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
                    if (_t2 != null)
                    {
                        Commit();
                    }
                    if (_c2 != null)
                    {
                        _c2.Close();
                        _c2.Dispose();
                        _c2 = null;
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
                    using (OracleCommand cmd = this.CreateCommand(sql))
                    {
                        cmd.Connection = Connection;
                        int ret = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            private string getSubStringByBytes(string str, int count)
            {
                byte[] bwrite = Encoding.UTF8.GetBytes(str.ToCharArray());
                if (bwrite.Length >= count)
                    return Encoding.UTF8.GetString(bwrite, 0, count);
                else return Encoding.UTF8.GetString(bwrite);
            }
        }

        class FrontPageOracleDbConnection : IConnectionEx
        {
            bool _t1;
            string _c1;
            OracleTransaction _t2;
            OracleConnection _c2;
            IDbDriver _d1;

            public FrontPageOracleDbConnection()
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
                get { return _d1; }
                set { _d1 = value; }
            }

            public string ConnectionString
            {
                get { return _c1; }
                set { _c1 = value; }
            }

            public bool IsTransaction
            {
                get { return _t1; }
                set { _t1 = value; }
            }

            public OracleConnection Connection
            {
                get 
                {
                    if (_c2 == null)
                    {
                        _c2 = DBAccessHelper.CreateConnection<OracleConnection>(ConnectionString);
                    }
                    return _c2; 
                }
            }

            OracleCommand CreateCommand(SqlStatement sql)
            {
                OracleCommand _c = new OracleCommand(sql.SqlClause);
                _c.Connection = Connection;
                _c.CommandTimeout = 300;
                _c.CommandType = sql.CommandType;

                create = false;
                if (IsTransaction)
                {
                    if (_t2 == null)
                    {
                        _t2 = Connection.BeginTransaction();
                    }
                    _c.Transaction = _t2;
                }                
                foreach (DataParameter dp in sql.Parameters)
                {
                    OracleParameter p = new OracleParameter();
                    p.ParameterName = String.Format("{0}", dp.ParameterName);
                    p.Size = dp.Size;
                    p.Direction = dp.Direction;

                    if (dp.Value == null)
                        p.Value = DBNull.Value;
                    else
                        p.Value = dp.Value;

                    if (dp.DbType == DbType.DateTime)
                    {
                        if (dp.Value.ToString() == DateTime.MinValue.ToString())
                        {
                            //p.Value = OracleDateTime.MinValue;
                            p.Value = OracleDateTime.Null;
                        }
                        else if (dp.Value == null)
                        {
                            p.Value = OracleDateTime.Null;
                        }
                        else
                        {
                            p.Value = ((DateTime)dp.Value).ToString("yyyy-MM-dd HH:mm:ss"); ;
                        }
                        p.Size = 64;
                        _c.CommandText = _c.CommandText.Replace(p.ParameterName, string.Format("to_date({0},'yyyy-mm-dd hh24:mi:ss')", p.ParameterName));
                        //if( _c.CommandText.IndexOf(string.Format("to_char({0},'yyyy-mm-dd hh24:mi:ss')", p.SourceColumn))<0)
                        //    _c.CommandText = _c.CommandText.Replace(p.SourceColumn, string.Format("to_char({0},'yyyy-mm-dd hh24:mi:ss')", p.SourceColumn));
                    }
                    else if (dp.DbType == DbType.String)
                    {
                        if (dp.Size > 65500)
                        {
                            p.OracleType = OracleType.Clob;
                            if (dp.Value == null || dp.Value.ToString().Length == 0)
                                p.Value = DBNull.Value;
                        }
                        else
                        {
                            if (dp.Value != null)
                                p.Value = getSubStringByBytes(dp.Value.ToString(), p.Size);
                        }
                    }

                    _c.Parameters.Add(p);
                }
                return _c;
            }


            public DataTable Query(SqlStatement sql)
            {
                using (OracleCommand _c = this.CreateCommand(sql))
                {
                    OracleDataAdapter _s = new OracleDataAdapter(_c);
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
                //using 使用误区？？
                using (OracleCommand _c = this.CreateCommand(sql))
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
                using (OracleCommand cmd = this.CreateCommand(sql))
                {
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

            void PopuloateCommand(OracleCommand cmd, SqlStatement st)
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
                if (_t2 != null)
                {
                    _t2.Commit();
                    _t2.Dispose();
                    _t2 = null;
                }
            }

            public void Rollback()
            {
                if (_t2 != null)
                {
                    _t2.Rollback();
                    _t2.Dispose();
                    _t2 = null;
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
                    if (_t2 != null)
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
                    using (OracleCommand cmd = this.CreateCommand(sql))
                    {
                        cmd.Connection = Connection;
                        int ret = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            private string getSubStringByBytes(string str, int count)
            {
                byte[] bwrite = Encoding.UTF8.GetBytes(str.ToCharArray());
                if (bwrite.Length >= count)
                    return Encoding.UTF8.GetString(bwrite, 0, count);
                else return Encoding.UTF8.GetString(bwrite);
            }
        }

        private IConnectionEx CreateConnection()
        {
            if (DBAccessHelper.IsDataAccessPage)
            {
                return new FrontPageOracleDbConnection();
            }
            else
            {
                return new OracleDbConnection();
            }
        }
    }
}