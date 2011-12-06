using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using Thinkment.Data;

using System.Data;
using System.Data.OleDb;

namespace We7.CMS
{
    /// <summary>
    /// 数据导入实例
    /// </summary>
    [Serializable]
    public class Import3rdDataHelper
    {
        private volatile static Import3rdDataHelper instance = null;
        private static readonly object lockHelper = new object();
        private volatile static string instanceID;
        private volatile static string instanceAccountID;
        //private volatile static DateTime createTime;
        private volatile static string createTime;

        /// <summary>
        /// 导入时间
        /// </summary>
        public static DateTime CreateTime
        {
            get { return Convert.ToDateTime(Import3rdDataHelper.createTime); }
        }

        /// <summary>
        /// 当前实例应用用户唯一标识
        /// </summary>
        public static string InstanceAccountID
        {
            get { return Import3rdDataHelper.instanceAccountID; }
        }

        //public volatile static string InstanceAccountID = "";

        /// <summary>
        /// 实例唯一标识
        /// </summary>
        public static string InstanceID
        {
            get { return instanceID; }
        }

        private Import3rdDataHelper() { }

        /// <summary>
        /// 创建实例，如果存在实例则返回现有实例，否则创建。
        /// </summary>
        /// <returns></returns>
        public static Import3rdDataHelper CreateInstance(string accountID)
        {
            lock (lockHelper)
            {
                if (instance != null)
                {
                    instance = null;
                }

                instance = new Import3rdDataHelper();
                instanceAccountID = accountID;
                createTime = Convert.ToString(DateTime.Now);
                instanceID = HttpContext.Current.Session.SessionID;
            }
            return instance;
        }

        /// <summary>
        /// 获取已存在实例
        /// </summary>
        /// <returns></returns>
        public static Import3rdDataHelper GetInstance()
        {
            if (CreateTime.AddMinutes(5).CompareTo(DateTime.Now) <= 0)
            {
                lock (lockHelper)
                {
                    instance = null;
                }
            }
            return instance;
        }

        /// <summary>
        /// 读取源数据文件
        /// </summary>
        /// <returns></returns>
        public static DataTable ImportSourceData(string path, string name, bool hasTitle)
        {
            OleDbConnection csvCon = new OleDbConnection();
            OleDbCommand csvCmd = new OleDbCommand();
            OleDbDataAdapter csvDa = new OleDbDataAdapter();
            DataTable table = new DataTable();
            try
            {
                csvCon.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
                csvCon.ConnectionString += path;
                if (hasTitle)
                {
                    csvCon.ConnectionString += ";Extended Properties=\"text;HDR=Yes;FMT=Delimited\"";
                }
                else
                {
                    csvCon.ConnectionString += ";Extended Properties=\"text;HDR=No;FMT=Delimited\"";
                }
                csvCon.Open();
                csvCmd.CommandText = "select * from " + name;
                csvCmd.Connection = csvCon;
                csvCmd.CommandType = CommandType.Text;
                csvDa.SelectCommand = csvCmd;
                csvDa.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                csvCon.Close();
                csvCmd.Dispose();
                csvDa.Dispose();
                csvCon.Dispose();
            }
        }


    }
}