using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Data;

namespace We7.Framework.Helper
{
    /// <summary>
    /// 数据查询业务助手
    /// </summary>
    public static partial class DbHelper
    {
        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sql"></param>
        public static void ExecuteSql(string sql)
        {
            try
            {
                IDatabase db = HelperFactory.Instance.Assistant.GetDatabases()["We7.CMS.Common"];
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
        public static DataTable Query(string sql)
        {
            try
            {
                IDatabase db = HelperFactory.Instance.Assistant.GetDatabases()["We7.CMS.Common"];
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
        public static object Get(string sql)
        {
            try
            {
                IDatabase db = HelperFactory.Instance.Assistant.GetDatabases()["We7.CMS.Common"];
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

        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static bool CheckTableExits(string tableName)
        {
            try
            {
                Query("SELECT * FROM [" + tableName + "] WHERE 1>2");
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查数据表与数据列是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static bool CheckColumnsExits(string tableName, params string[] columns)
        {
            bool retVal = true;
            try
            {
                DataTable dt=Query("SELECT * FROM [" + tableName + "] WHERE 1>2");
                foreach (string col in columns)
                {
                    if (!dt.Columns.Contains(col))
                    {
                        retVal = false;
                        break;
                    }
                }
            }
            catch
            {
                retVal=false;
            }
            return retVal;
        }

        public static string FormatDate(DateTime dt,string type)
        {
            type = type.ToLowerInvariant();
            if (type == "access")
            {
                return dt.ToString("#yyyy-MM-dd HH:mm:ss#");
            }
            else if (type == "sqlserver")
            {
                return dt.ToString("'yyyy-MM-dd HH:mm:ss'");
            }
            throw new Exception("还没有实现"+type+"的格式化时间方法");
        }
    }
}
