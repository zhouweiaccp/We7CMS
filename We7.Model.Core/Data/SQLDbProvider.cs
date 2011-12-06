using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Util;
using System.Configuration;
using System.Data;
using We7.Model.Core.Config;
using We7.CMS.Config;
using We7.Framework.Config;
using We7.CMS;
using We7.CMS.Accounts;

namespace We7.Model.Core.Data
{
    public class SQLDbProvider : IDbProvider
    {
        //ISQLBuilder SqlBuilder = Utils.CreateInstance<ISQLBuilder>(ModelConfig.SQLBuilder);
        //string connstr = ModelConfig.DBConnectionString; //ConfigurationManager.ConnectionStrings["ModelDB"].ConnectionString;

        /// <summary>
        /// 连接字符串
        /// </summary>
        private string connstr = BaseConfigs.GetDBConnectString;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnStr
        {
            get { return connstr; }
        }

        /// <summary>
        /// SQL构造者
        /// </summary>
        public ISQLBuilder SqlBuilder
        {
            get
            {
                string dbtype = BaseConfigs.GetDbType;

                ISQLBuilder builder = null;

                if (String.Compare(dbtype, "Access", true) == 0)
                {
                    builder = new AccessSQLBuilder();
                }
                else if (String.Compare(dbtype, "SqlServer", true) == 0)
                {
                    builder = new MSSqlServerSQLBuilder();
                }
                else if (String.Compare(dbtype, "Oracle", true) == 0)
                {
                    builder = new OracleSQLBuilder();
                }
                else if (String.Compare(dbtype, "SQLite", true) == 0)
                {
                    builder = new SQLLiteSQLBuilder();
                }
                else if (String.Compare(dbtype, "MySql", true) == 0)
                {
                    builder = new MySqlSQLBuilder();
                }
                if (builder == null)
                    throw new Exception("内容模型不支持数据库类型" + dbtype);
                return builder;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data">模型数据</param>
        /// <returns>是否成功</returns>
        public bool Insert(PanelContext data)
        {
            using (IDbConnection conn = SqlBuilder.GetConnection())
            {
                conn.ConnectionString = connstr;
                conn.Open();
                using (IDbCommand comm = SqlBuilder.GetCommand())
                {
                    comm.Connection = conn;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = SqlBuilder.BuildInsertSQL(data, comm.Parameters);
                    comm.ExecuteNonQuery();
                }
            }
            return true;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">模型数据</param>
        /// <returns>是否成功</returns>
        public bool Update(PanelContext data)
        {
            using (IDbConnection conn = SqlBuilder.GetConnection())
            {
                conn.ConnectionString = connstr;
                conn.Open();
                using (IDbCommand comm = SqlBuilder.GetCommand())
                {
                    comm.Connection = conn;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = SqlBuilder.BuildUpdateSQL(data, comm.Parameters);
                    comm.ExecuteNonQuery();
                }
            }
            return true;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="data">模型数据</param>
        /// <returns>是否成功</returns>
        public bool Delete(PanelContext data)
        {
            using (IDbConnection conn = SqlBuilder.GetConnection())
            {
                conn.ConnectionString = connstr;
                conn.Open();
                using (IDbCommand comm = SqlBuilder.GetCommand())
                {
                    comm.Connection = conn;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = SqlBuilder.BuildDeleteSQL(data, comm.Parameters);
                    comm.ExecuteNonQuery();
                }
            }
            return true;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="data">模型数据</param>
        /// <param name="recordcount">返回总的数据记录</param>
        /// <param name="pageindex">当前的页数</param>
        /// <returns>数据集</returns>
        public DataTable Query(PanelContext data, out int recordcount, ref int pageindex)
        {
            int itemscount, startindex = data.PageIndex;
            if (!GeneralConfigs.GetConfig().ShowAllInfo && Security.CurrentAccountID!=We7Helper.EmptyGUID)
            {
                data.QueryFields.Add(new QueryField(data.Table.Columns["AccountID"],Security.CurrentAccountID));
            }
            recordcount = GetCount(data);

            Utils.BuidlPagerParam(recordcount, data.PageSize, ref pageindex, out startindex, out itemscount);

            using (IDbConnection conn = SqlBuilder.GetConnection())
            {
                conn.ConnectionString = connstr;
                conn.Open();
                IDbDataAdapter adapter = SqlBuilder.GetDataAdapter();
                using (IDbCommand comm = SqlBuilder.GetCommand())
                {
                    comm.Connection = conn;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = SqlBuilder.BuildListSQL(data, comm.Parameters, startindex, itemscount);
                    adapter.SelectCommand = comm;
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }

        /// <summary>
        /// 取得当前数据行
        /// </summary>
        /// <param name="data">模型数据</param>
        /// <returns>数据行</returns>
        public DataRow Get(PanelContext data)
        {
            using (IDbConnection conn = SqlBuilder.GetConnection())
            {
                conn.ConnectionString = connstr;
                conn.Open();
                IDbDataAdapter adapter = SqlBuilder.GetDataAdapter();
                using (IDbCommand comm = SqlBuilder.GetCommand())
                {
                    comm.Connection = conn;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = SqlBuilder.BuildGetSQL(data, comm.Parameters);
                    adapter.SelectCommand = comm;
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                        return ds.Tables[0].Rows[0];
                    return null;
                }
            }
        }

        /// <summary>
        /// 取得总记录数
        /// </summary>
        /// <param name="data">模型数据</param>
        /// <returns>记录数</returns>
        public int GetCount(PanelContext data)
        {
            using (IDbConnection conn = SqlBuilder.GetConnection())
            {
                conn.ConnectionString = connstr;
                conn.Open();
                using (IDbCommand comm = SqlBuilder.GetCommand())
                {
                    comm.Connection = conn;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = SqlBuilder.BuildCountSQL(data, comm.Parameters);
                    object o = comm.ExecuteScalar();
                    return (int)o;
                }
            }
        }
    }
}
