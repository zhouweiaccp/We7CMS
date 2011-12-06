using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Install
{
    /// <summary>
    /// 数据库连接信息类
    /// </summary>
    public class DatabaseInfo
    {
        public DatabaseInfo() { }

        string initialCatalog;
        /// <summary>
        /// 服务器
        /// </summary>
        public string Server
        {
            get { return initialCatalog; }
            set { initialCatalog = value; }
        }
        string datasource;
        /// <summary>
        /// 数据库
        /// </summary>
        public string Database
        {
            get 
            {
                if (datasource != null && datasource != "")
                    return datasource;
                else if(dataFile!=null && dataFile!="")
                {
                    if (dataFile.IndexOf("\\") > -1)
                        return dataFile.Substring(dataFile.LastIndexOf("\\") + 1);
                }
                return datasource;
            }
            set { datasource = value; }
        }
        string userID;
        /// <summary>
        /// 登录用户
        /// </summary>
        public string User
        {
            get { return userID; }
            set { userID = value; }
        }
        string password;
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        string dataFile;
        /// <summary>
        /// 文件数据的数据库文件路径
        /// </summary>
        public string DBFile
        {
            get { return dataFile; }
            set { dataFile = value; }
        }

        /// <summary>
        /// 是否创建数据库
        /// </summary>
        public bool CreateDB { get; set; }

    }
}
