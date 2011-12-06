using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.Data;
using We7.Framework;
using System.Web;
using We7.CMS;
using Thinkment.Data;
using We7.Model.Core;
using System.Data;
using We7.CMS.Config;
using System.Text.RegularExpressions;

namespace We7.Model.UI.Data
{
    public class DataBaseHelper : IDataBaseHelper
    {

        private List<string> ignoreUpdateFields;

        #region properties
        /// <summary>
        /// 业务工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID]; }
        }

        /// <summary>
        /// 文章业务助手
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        /// <summary>
        /// 数据助手
        /// </summary>
        protected ObjectAssistant Assistant
        {
            get { return ArticleHelper.Assistant; }
        }

        protected List<string> IgnoreUpdateFields
        {
            get
            {
                if (ignoreUpdateFields == null)
                {
                    ignoreUpdateFields = new List<string>(new string[] { "ID", "AccountID", "OwnerID", "Source", "ContentType", "ContentUrl", "State", "IsShow", "Tags", "Updated", "Created", "ProcessState", "ProcessDirection", "ProcessSiteID", "FullPath" });
                }
                return ignoreUpdateFields;
            }
        }

        #endregion

        #region const
        const string SqlCreate = @" CREATE TABLE [{0}] (
                                    [ID] nvarchar (40) primary key
                                    )";
        const string SqlAddColumn = @"ALTER TABLE [{0}] add [{1}] {2} ";

        const string SqlAlterColumn = @"ALTER TABLE [{0}] ALTER COLUMN [{1}] {2} ";

        const string SqlDropTable = @"DROP TABLE [{0}] ";

        const string SqlSelectTable = @"SELECT * FROM [{0}] ";


        #endregion

        #region IDataBaseHelper 成员

        /// <summary>
        /// 添加表
        /// </summary>
        /// <param name="model"></param>
        public void CreateTable(We7.Model.Core.ModelInfo model)
        {
            if (model.Type != ModelType.ARTICLE)
                return;
            string table = model.DataSet.Tables[0].Name;
            string sql = String.Format(SqlCreate, table);
            try
            {
                try
                {
                    ModelDBHelper DbHelper = ModelDBHelper.Create(model.ModelName);
                    //TODO：不知道以下代码作用是什么，看起来像是创建完表结构之后检测一下
                    DbHelper.Query(String.Format(SqlSelectTable, table));
                }
                catch
                {
                    ExecuteSql(sql);
                }
            }
            catch { }

            UpdateTable(model);
        }

        /// <summary>
        /// 更新表
        /// </summary>
        /// <param name="model"></param>
        public void UpdateTable(We7.Model.Core.ModelInfo model)
        {
            ModelDBHelper DbHelper = ModelDBHelper.Create(model.ModelName);

            string table = model.DataSet.Tables[0].Name;
            DataTable dt = DbHelper.Query(String.Format(SqlSelectTable, table));
            We7DataColumnCollection columns = model.DataSet.Tables[0].Columns;
            if (!columns.Contains("Updated", true))
            {
                columns.Add(new We7DataColumn() { DataType = TypeCode.DateTime, Name = "Updated" });
            }
            if (!columns.Contains("AccountID", true))
            {
                columns.Add(new We7DataColumn() { DataType = TypeCode.String, Name = "AccountID", MaxLength = 40 });
            }
            foreach (We7DataColumn c in columns)
            {
                try
                {
                    if (c.Direction == ParameterDirection.ReturnValue)
                        continue;

                    string sql = String.Empty;
                    if (ContainsColumn(dt, c.Name))
                    {
                        if (IgnoreUpdateFields.Contains(c.Name))
                            continue;

                        sql = String.Format(SqlAlterColumn, table, c.Name, GetDbType(c));
                    }
                    else
                    {
                        sql = String.Format(SqlAddColumn, table, c.Name, GetDbType(c));
                    }
                    //DbHelper.Execute(sql);
                    ExecuteSql(sql);
                }
                catch (Exception ex) { }
            }
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="model"></param>
        public void DeleteTable(We7.Model.Core.ModelInfo model)
        {
            string table = model.DataSet.Tables[0].Name;
            string sql = String.Format(SqlDropTable, table);
            ExecuteSql(sql);
        }

        /// <summary>
        /// 根据Sql条件查询数据行
        /// </summary>
        /// <param name="table">条件sql</param>
        /// <param name="sqlCondition">Condition</param>
        /// <returns></returns>
        public DataRow GetDataRow(string table, string sqlCondition)
        {
            string sql;
            sqlCondition = sqlCondition.Trim();
            if (String.IsNullOrEmpty(sqlCondition))
            {
                sql = String.Format("SELECT * FROM [{0}]", table);
            }
            else
            {
                sql = String.Format("SELECT * FROM [{0}] WHERE {1}", table, sqlCondition);
            }
            DataTable dt = Query(sql);
            if (dt == null)
                throw new Exception("不存在当前数据表:" + table);

            return dt.Rows.Count > 0 ? dt.Rows[0] : dt.NewRow();
        }

        /// <summary>
        /// Get Count
        /// </summary>
        /// <param name="table">Table Name </param>
        /// <param name="sqlCondition"> Condition </param>
        /// <returns>Count</returns>
        public int Count(string table, string sqlCondition)
        {
            string sql;
            sqlCondition = sqlCondition.Trim();
            if (String.IsNullOrEmpty(sqlCondition))
            {
                sql = String.Format("SELECT COUNT(*) FROM [{0}]", table);
            }
            else
            {
                sql = String.Format("SELECT COUNT(*) FROM [{0}] WHERE {1}", table, sqlCondition);
            }
            object o = Get(sql);
            return (int)o;
        }

        /// <summary>
        /// 对当前数据进行查询
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="sqlCondition">查询条件</param>
        /// <param name="start">记录开始序号</param>
        /// <param name="querycount">查询条数</param>
        /// <param name="orders">排序</param>
        /// <returns></returns>
        public DataTable Query(string table, string sqlCondition, int start, int querycount, string orders)
        {
            string sql = GetSqlFormat(table, sqlCondition, orders, start, querycount);
            DataTable dt = Query(sql);
            return dt;
        }

        /// <summary>
        /// 创建所有的文章模型表
        /// </summary>
        public void CreateModelTables()
        {
            ModelHelper.ReCreateModelIndex();

            ContentModelCollection collection = ModelHelper.GetAllContentModel();

            foreach(ContentModel cm in collection)
            {
                ModelInfo mi=ModelHelper.GetModelInfo(cm.Name);
                if (mi.Type == ModelType.ARTICLE)
                {
                    CreateTable(mi);
                }
            }
        }

        #endregion
        /// <summary>
        /// 取得数据类型
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        string GetDbType(We7DataColumn dc)
        {
            switch (dc.DataType)
            {
                case TypeCode.Char:
                    return String.Format("nchar ({0})", dc.MaxLength > 0 ? dc.MaxLength : 50);
                case TypeCode.DateTime:
                    return "datetime";
                case TypeCode.Decimal:
                case TypeCode.Double:
                    return "decimal";
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.SByte:
                case TypeCode.UInt32:
                case TypeCode.UInt16:
                case TypeCode.Single:
                case TypeCode.Byte:
                case TypeCode.Boolean:
                    return "int";
                case TypeCode.UInt64:
                case TypeCode.Int64:
                    return "bigint";
                case TypeCode.String:
                    string dbtype = BaseConfigs.GetBaseConfig().DBType;
                    if (String.Compare(dbtype, "Access", true) == 0)
                    {
                        return dc.MaxLength >= 255 ? "text" : String.Format("nvarchar ({0})", dc.MaxLength > 0 ? dc.MaxLength : 50);
                    }
                    else if (String.Compare(dbtype, "SqlServer", true) == 0)
                    {
                        return dc.MaxLength >= 4000 ? "text" : String.Format("nvarchar ({0})", dc.MaxLength > 0 ? dc.MaxLength : 50);
                    }
                    else if (String.Compare(dbtype, "MySql", true) == 0)
                    {
                        return dc.MaxLength >= 65535 ? "text" : String.Format("nvarchar ({0})", dc.MaxLength > 0 ? dc.MaxLength : 50);
                    }
                    else if (String.Compare(dbtype, "Oracle", true) == 0)
                    {
                        return dc.MaxLength >= 4000 ? "text" : String.Format("nvarchar ({0})", dc.MaxLength > 0 ? dc.MaxLength : 50);
                    }
                    else if (String.Compare(dbtype, "SQLLite", true) == 0)
                    {
                        return dc.MaxLength >= 4000 ? "text" : String.Format("nvarchar ({0})", dc.MaxLength > 0 ? dc.MaxLength : 50);
                    }
                    else
                    {
                        return dc.MaxLength >= 4000 ? "text" : String.Format("nvarchar ({0})", dc.MaxLength > 0 ? dc.MaxLength : 50);
                    }
                default:
                    return String.Format("nvarchar ({0})", 50);
            }
            return "nvarchar (50)";
        }


        /// <summary>
        /// 取得Sql格式化语式
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="sqlwhere">查询条件</param>
        /// <param name="orders">排序</param>
        /// <param name="startindex">开始记录序号</param>
        /// <param name="itemcount">查询记录条数</param>
        /// <returns></returns>
        string GetSqlFormat(string table, string sqlwhere, string orders, int startindex, int itemcount)
        {
            string dbtype = BaseConfigs.GetDbType;
            string sql = "";

            if (String.Compare(dbtype, "Access", true) == 0)
            {
                sql = GetAccessSqlFormat(table, sqlwhere, orders, startindex, itemcount);
            }
            else if (String.Compare(dbtype, "SqlServer", true) == 0)
            {
                sql = GetSqlServerSqlFormat(table, sqlwhere, orders, startindex, itemcount);
            }
            else if (String.Compare(dbtype, "Oracle", true) == 0)
            {
            }
            else if (String.Compare(dbtype, "SQLite", true) == 0)
            {
            }
            else if (String.Compare(dbtype, "MySql", true) == 0)
            {
            }
            else
            {
                throw new Exception("内容模型不支持数据库类型" + dbtype);
            }
            return sql;
        }

        string GetAccessSqlFormat(string table, string sqlwhere, string orders, int startindex, int itemcount)
        {
            string sql = "";
            orders = String.IsNullOrEmpty(orders) ? "ORDER BY [ID] ASC" : "ORDER BY " + orders;
            sqlwhere = !String.IsNullOrEmpty(sqlwhere) ? String.Format("WHERE {0}", sqlwhere) : "";
            table = string.Format("[{0}]", table);
            string revorders = GetRevertOrders(orders);

            if (startindex > 0)
            {
                int firstcount = itemcount + startindex;
                sql = String.Format(@"SELECT * FROM (SELECT TOP {4} * FROM (SELECT TOP {0} * FROM {1} {2} {3}) AS TB_1 {5}) AS TB_2 {3}", firstcount, table, sqlwhere, orders, itemcount, revorders, orders);
            }
            else if (itemcount > 0)
            {
                sql = String.Format(@"SELECT TOP {0} * FROM {1} {2} {3}", itemcount, table, sqlwhere, orders);
            }
            else
            {

                sql = String.Format("SELECT * FROM {0} {1} {2}", table, sqlwhere, orders);
            }
            return sql;
        }

        string GetSqlServerSqlFormat(string table, string sqlwhere, string orders, int startindex, int itemcount)
        {
            string sql = "";
            orders = String.IsNullOrEmpty(orders) ? "ORDER BY [ID] ASC" : "ORDER BY " + orders;
            sqlwhere = !String.IsNullOrEmpty(sqlwhere) ? String.Format("WHERE {0}", sqlwhere) : "";
            table = string.Format("[{0}]", table);
            string revorders = GetRevertOrders(orders);

            if (startindex > 0)
            {
                int firstcount = itemcount + startindex;
                sql = String.Format(@"SELECT * FROM (SELECT TOP {4} * FROM (SELECT TOP {0} * FROM {1} {2} {3}) AS TB_1 {5}) AS TB_2 {3}", firstcount, table, sqlwhere, orders, itemcount, revorders, orders);
            }
            else if (itemcount > 0)
            {
                sql = String.Format(@"SELECT TOP {0} * FROM {1} {2} {3}", itemcount, table, sqlwhere, orders);
            }
            else
            {

                sql = String.Format("SELECT * FROM {0} {1} {2}", table, sqlwhere, orders);
            }
            return sql;
        }

        string GetRevertOrders(string orders)
        {
            Regex regex = new Regex(@"\bDESC\b|\bASC\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex.Replace(orders, delegate(Match match)
            {
                if (match.Success)
                {
                    if (match.Value.ToLower() == "desc")
                        return match.Result("ASC");
                    else if (match.Value.ToLower() == "asc")
                        return match.Result("DESC");
                }
                return string.Empty;
            });
        }

        bool ContainsColumn(DataTable dt, string columnName)
        {
            return dt != null && dt.Columns.Contains(columnName);
        }


        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sql"></param>
        public void ExecuteSql(string sql)
        {
            try
            {
                IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
                SqlStatement sqlstatement = new SqlStatement();
                sqlstatement.SqlClause = sql;
                db.DbDriver.FormatSQL(sqlstatement);
                using (IConnection conn = db.CreateConnection())
                {
                    conn.Update(sqlstatement);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(sql + "\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable Query(string sql)
        {
            try
            {
                IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
                SqlStatement sqlstatement = new SqlStatement();
                sqlstatement.SqlClause = sql;
                db.DbDriver.FormatSQL(sqlstatement);
                using (IConnection conn = db.CreateConnection())
                {
                    return conn.Query(sqlstatement);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(sql + "\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 取得单个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        object Get(string sql)
        {
            try
            {
                IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
                SqlStatement sqlstatement = new SqlStatement();
                sqlstatement.SqlClause = sql;
                db.DbDriver.FormatSQL(sqlstatement);
                using (IConnection conn = db.CreateConnection())
                {
                    return conn.QueryScalar(sqlstatement);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(sql + "\r\n" + ex.Message);
            }
        }
    }
}
