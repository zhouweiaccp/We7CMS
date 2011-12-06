using System;
using We7.Framework.Config;

namespace We7.CMS.Config
{
	/// <summary>
	/// 基本设置描述类, 加[Serializable]标记为可序列化
	/// </summary>
	[Serializable]
	public class BaseConfigInfo : IConfigInfo
    {
        #region 私有字段

        private string dbConnectionString = "New=False;Compress=True;Synchronous=Off;UTF8Encoding=True;Version=3;Data Source={$Current}\\CD.DB3;";		// 数据库连接串-格式(中文为用户修改的内容)：Data Source=数据库服务器地址;User ID=您的数据库用户名;Password=您的数据库用户密码;Initial Catalog=数据库名称;Pooling=true
        private string dbDriver = "Thinkment.Data.SQLiteDriver";
        private string dbType = "SQLite";

        #endregion

        #region 属性

        /// <summary>
		/// 数据库连接串
		/// 格式(中文为用户修改的内容)：
		///    Data Source=数据库服务器地址;
		///    User ID=您的数据库用户名;
		///    Password=您的数据库用户密码;
		///    Initial Catalog=数据库名称;Pooling=true
		/// </summary>
        public string DBConnectionString
        {
            get { return dbConnectionString; }
            set { dbConnectionString = value; }
        }

        /// <summary>
        /// 数据库驱动
        /// </summary>
        public string DBDriver
        {
            get { return dbDriver; }
            set { dbDriver = value; }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DBType
        {
            get { return dbType; }
            set { dbType = value; }
        }

        #endregion
    }
}
