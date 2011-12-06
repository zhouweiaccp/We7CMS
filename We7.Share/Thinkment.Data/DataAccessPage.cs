using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Data;
using System.Web;

namespace Thinkment.Data
{
    /// <summary>
    /// 使用数据访问页
    /// </summary>
    public interface IDataAccessPage
    {
    }

    internal sealed class DBAccessHelper
    {
        public const string ConnectionKey = "$Thinkment$Data$PageDBConnection";

        public static T CreateConnection<T>(string connectionString) where T : class, IDbConnection, new()
        {

            T connection = HttpContext.Current.Items[ConnectionKey] as T;
            if (connection == null)
            {
                connection = new T();
                connection.ConnectionString = connectionString;
                connection.Open();
                HttpContext.Current.Items[ConnectionKey] = connection;
                Page page = HttpContext.Current.Handler as Page;
                page.Unload += new EventHandler(page_Unload);
                page.Error += new EventHandler(page_Unload);
            }
            return connection;
        }

        static void page_Unload(object sender, EventArgs e)
        {
            IDbConnection conn = HttpContext.Current.Items[ConnectionKey] as IDbConnection;
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                HttpContext.Current.Items[ConnectionKey] = null;
            }
        }

        /// <summary>
        /// 是否是数据访问页面
        /// </summary>
        public static bool IsDataAccessPage
        {
            get
            {
                return HttpContext.Current != null && HttpContext.Current.Items != null &&
                    HttpContext.Current.Handler != null && HttpContext.Current.Handler is IDataAccessPage &&
                    HttpContext.Current.Application["isMulti"] == null;
            }
        }
    }
}
