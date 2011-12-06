using System;
using System.Text;
using System.Web;
using System.Data;
using System.Collections.Generic;

using We7.Framework;
using Thinkment.Data;

namespace We7.CMS
{
    /// <summary>
    ///  多数据源helper
    /// </summary>
    [Serializable]
    [Helper("We7.MultiDataSourceHelper")]
    public class MultiDataSourceHelper : BaseHelper
    {
        HelperFactory HelperFactory
        {
            get
            {
                return (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
            }
        }


        /// <summary>
        /// 查询数据库并返回查询结果
        /// </summary>
        /// <param name="dbKey">数据库连接池中的key</param>
        /// <param name="sqlStr">要之行的SQL语句</param>
        /// <returns></returns>
        public DataTable Query(string dbKey, string sqlStr)
        {
            DataTable table = new DataTable();
            if (string.IsNullOrEmpty(dbKey))
            {
                return table;
            }

            HttpContext.Current.Application["isMulti"] = true;

            IDatabase db = Assistant.GetDatabases()[dbKey];
            SqlStatement sql = new SqlStatement(sqlStr);

            db.DbDriver.FormatSQL(sql);
            try
            {
                using (IConnection conn = db.CreateConnection())
                {
                    table = conn.Query(sql);
                    HttpContext.Current.Application["isMulti"] = null;
                    HttpContext.Current.Application.UnLock();
                    return table;
                }
            }
            catch
            {
                HttpContext.Current.Application["isMulti"] = null;
                HttpContext.Current.Application.UnLock();
                return table;
            }
        }
    }
}
