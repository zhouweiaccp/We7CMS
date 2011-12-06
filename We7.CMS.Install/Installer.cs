using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections;

using We7.CMS.Config;
using Thinkment.Data;
using We7.Framework;
using We7.Framework.Zip;
using We7.Model.UI.Data;

namespace We7.CMS.Install
{
    public static class Installer
    {
        static Installer()
        {
        }

        /// <summary>
        /// 根据数据库类型创建驱动对象
        /// </summary>
        /// <param name="dbType">类型字符串</param>
        /// <returns></returns>
        public static IDbDriver CreateDbDriver(string dbType)
        {
            IDbDriver driver = null;
            switch (dbType.ToLower())
            {
                case "sqlite":
                    driver = new SQLiteDriver();
                    break;
                case "access":
                    driver = new OleDbDriver();
                    break;
                case "sqlserver":
                    driver = new SqlDbDriver();
                    break;
                case "mysql":
                    driver = new MySqlDriver();
                    break;
                case "oracle":
                    driver = new OracleDriver();
                    break;
            }
            return driver;
        }

        public static string GetDBTypeFromDriver(string driver)
        {
            string dbType = "";
            switch (driver)
            {
                case "Thinkment.Data.SQLiteDriver":
                    dbType = "SQLite"; ;
                    break;
                case "Thinkment.Data.OleDbDriver":
                    dbType = "Access";
                    break;
                case "Thinkment.Data.SqlDbDriver":
                    dbType = "SqlServer";
                    break;
                case "Thinkment.Data.MySqlDriver":
                    dbType = "MySql";
                    break;
                case "Thinkment.Data.OracleDriver":
                    dbType = "Oracle";
                    break;
            }
            return dbType;
        }
        /// <summary>
        /// 检查数据库连接是否正常
        /// </summary>
        /// <param name="bci"></param>
        /// <returns></returns>
        public static bool CheckConnection(BaseConfigInfo bci,out string msg)
        {
            msg = "";
            try
            {
                string connectionString = bci.DBConnectionString;
                string selectDbType = bci.DBType;

                connectionString = connectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);

                IDbDriver driver = CreateDbDriver(selectDbType);
                using (IConnection conn = driver.CreateConnection(connectionString))
                {
                    SqlStatement st = new SqlStatement("SELECT 1");
                    conn.QueryScalar(st);
                }
                return true;
            }
            catch(Exception ex)
            {
                if (ex.Message.Trim().ToUpper().StartsWith("ORA-00923"))
                    return true;
                else
                {
                    //throw ex;
                    msg = ex.Message;
                    return false;
                }
            }
        }

        /// <summary>
        /// 新建数据库
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public static int CreateDatabase(BaseConfigInfo bci,out Exception resultException)
        {
            int result = 0;
            resultException = null;
            DatabaseInfo dbi = GetDatabaseInfo(bci);
            string dbFile="";
            if (dbi.DBFile != null && dbi.DBFile != "")
            {
                dbFile = dbi.DBFile.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
                dbFile = dbFile.Replace('\\', System.IO.Path.DirectorySeparatorChar);
            }

            try
            {
                switch (bci.DBType)
                {
                    case "SqlServer":
                        string masterString = String.Format("Server={0};Database={1};User={2};Password={3}", dbi.Server, "master", dbi.User, dbi.Password);
                        string sql = String.Format("IF NOT EXISTS ( SELECT * FROM SYSDATABASES WHERE NAME=N'{0}') CREATE DATABASE {0}", dbi.Database);

                        //创建数据库
                        IDbDriver driver = new SqlDbDriver();
                        using (IConnection conn = driver.CreateConnection(masterString))
                        {
                            SqlStatement st0 = new SqlStatement(string.Format("SELECT count(*) FROM SYSDATABASES WHERE NAME=N'{0}'",dbi.Database));
                            int count = (int)conn.QueryScalar(st0);
                            if (count == 0)
                            {
                                SqlStatement st = new SqlStatement(sql);
                                driver.FormatSQL(st);
                                driver.FormatSQL(st0);
                                conn.Update(st);
                                if ((int)conn.QueryScalar(st0) > 0) result = 1;
                            }
                            else
                                result = -1;
                        }

                        break;

                    case "MySql":
                        result = -1;
                        break;

                    case "Oracle":

                        result = -1;
                        break;

                    case "SQLite":
                        if (File.Exists(dbFile))
                            result = -1;
                        else
                        {
                            string dbpath = Path.GetDirectoryName(dbFile);
                            if (!Directory.Exists(dbpath))
                                Directory.CreateDirectory(dbpath);
                            System.Data.SQLite.SQLiteConnection.CreateFile(dbFile);
                            if (File.Exists(dbFile)) result = 1;
                        }

                        break;

                    case "Access":
                        if (File.Exists(dbFile))
                            result = -1;
                        else
                        {
                            string dbpath = Path.GetDirectoryName(dbFile);
                            if (!Directory.Exists(dbpath))
                                Directory.CreateDirectory(dbpath);

                            ADOX.Catalog catlog = new ADOX.Catalog();
                            catlog.Create(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Engine Type=5", dbFile));
                            if (File.Exists(dbFile)) result = 1;
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                resultException = ex;
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 生成数据库连接串
        /// </summary>
        /// <param name="selectDbType"></param>
        /// <param name="dbi"></param>
        /// <returns></returns>
        public static BaseConfigInfo GenerateConnectionString(string selectDbType, DatabaseInfo dbi)
        {
            string dbDriver = string.Empty;
            string connectionString = string.Empty;
            //判断数据库类型，填充数据库字符串
            if (dbi.DBFile.IndexOf("\\") > -1)
                dbi.DBFile = dbi.DBFile.Substring(dbi.DBFile.LastIndexOf("\\") + 1);
            string path = "{$App}\\App_data\\DB\\" + dbi.DBFile;
            switch (selectDbType)
            {
                case "SqlServer":
                    connectionString =
                        string.Format(@"Server={0};Database={1};User={2};Password={3};Pooling=True;Min Pool Size=3;Max Pool Size=10;Connect Timeout=30;",
                                      dbi.Server,dbi.Database,dbi.User,dbi.Password);
                    dbDriver = "Thinkment.Data.SqlDbDriver";
                    break;

                case "MySql":
                    connectionString =
                        string.Format(@"server={0};database={1};uid={2};Pwd={3};charset=utf8;Pooling=true;Min Pool Size=3;Max Pool Size=10;Connection Timeout=30;",
                                      dbi.Server, dbi.Database, dbi.User, dbi.Password);
                    dbDriver = "Thinkment.Data.MySqlDriver";
                    break;

                case "Oracle":
                    connectionString =
                        string.Format(@"Data Source={0};User ID={1};Password={2};Pooling=True;Min Pool Size=3;Max Pool Size=10;",
                                      dbi.Server, dbi.User, dbi.Password);
                    dbDriver = "Thinkment.Data.OracleDriver";
                    break;
                    
                case "SQLite":
                    connectionString =
                        string.Format(@"New=False;Compress=True;Synchronous=Off;UTF8Encoding=True;Version=3;Data Source={0};Pooling=True;Min Pool Size=3;Max Pool Size=10;Connect Timeout=30;", path);
                    dbDriver = "Thinkment.Data.SQLiteDriver";
                    break;

                case "Access":
                    connectionString =
                        string.Format(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source={0};Persist Security Info=True;", path);
                    dbDriver = "Thinkment.Data.OleDbDriver";
                    break;
            }

            BaseConfigInfo baseConfig = new BaseConfigInfo();
            baseConfig.DBConnectionString = connectionString;
            baseConfig.DBType = selectDbType;
            baseConfig.DBDriver = dbDriver;

            return baseConfig;
        }
        /// <summary>
        /// 从连接串解析获取数据库信息
        /// </summary>
        /// <param name="bci"></param>
        /// <returns></returns>
        public static DatabaseInfo GetDatabaseInfo(BaseConfigInfo bci)
        {
            DatabaseInfo dbi = new DatabaseInfo();
            string connectionstring = bci.DBConnectionString;
            string selectDbType = bci.DBType;

            if (selectDbType.ToLower() == "sqlserver" || selectDbType.ToLower() == "mysql"||
               selectDbType.ToLower() == "oracle")
            {
                foreach (string info in connectionstring.Split(';'))
                {
                    if (info.ToLower().IndexOf("server") >= 0 || info.ToLower().IndexOf("data source") >= 0)
                    {
                        dbi.Server= info.Split('=')[1].Trim();
                        continue;
                    }
                    if (info.ToLower().IndexOf("database") >= 0)
                    {
                        dbi.Database = info.Split('=')[1].Trim();
                        continue;
                    }
                    if (info.ToLower().IndexOf("user") >= 0 || info.ToLower().IndexOf("uid") >= 0 ||
                        info.ToLower().IndexOf("user id") >= 0)
                    {
                        dbi.User = info.Split('=')[1].Trim();
                        continue;
                    }
                    if (info.ToLower().IndexOf("password") >= 0 || info.ToLower().IndexOf("pwd") >= 0)
                    {
                        dbi.Password= info.Split('=')[1].Trim();
                        continue;
                    }
                }
            }
            else
            {
                foreach (string info in connectionstring.Split(';'))
                {
                    if (info.ToLower().IndexOf("data source") >= 0)
                    {
                        dbi.DBFile= info.Split('=')[1].Trim();
                        continue;
                    }
                }
            }

            return dbi;
        }

        /// <summary>
        /// 执行SQL，进行数据库初始化
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="updateFile"></param>
        public static void ExcuteSQL(BaseConfigInfo bci, string updateFile)
        {
            if (updateFile != "" && File.Exists(updateFile))
            {
                string connectionString = bci.DBConnectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
                XmlDocument doc = new XmlDocument();
                doc.Load(updateFile);

                foreach (XmlNode node in doc.SelectNodes("/Update/Database"))
                {
                    IDbDriver dbDriver = CreateDbDriver(bci.DBType);
                    if (dbDriver == null) continue;

                    //开始处理
                   
                    int success = 0;
                    int errors = 0;
                    using (IConnection conn = dbDriver.CreateConnection(connectionString))
                    {
                        foreach (XmlNode sub in node.SelectNodes("Sql"))
                        {
                            //读取SQL语句，逐一执行
                            SqlStatement sql = new SqlStatement();
                            sql.CommandType = System.Data.CommandType.Text;
                            sql.SqlClause = sub.InnerText;
                            dbDriver.FormatSQL(sql);
                            try
                            {
                                conn.Update(sql);
                                success++;
                            }
                            catch (Exception ex)
                            {
                                //出现了错误，我们继续执行
                                We7.Framework.LogHelper.WriteFileLog(We7.Framework.LogHelper.sql_update,
                                                                     "执行SQL：" + sql.SqlClause + "\n\t 出现错误：", ex.Message);
                                errors++;
                                continue;
                            }
                        }
                    }

                    We7.Framework.LogHelper.WriteFileLog(We7.Framework.LogHelper.sql_update.ToString(),
                                                                        "执行完毕：",
                                                                        string.Format("{3}执行完毕！共执行语句{0}条，成功{1}，失败{2} 。", success + errors, success, errors, updateFile));
                }
            }
        }

        /// <summary>
        /// 执行install/SQL所有文件：指定文件名
        /// </summary>
        /// <param name="bci"></param>
        /// <param name="files"></param>
        public static void ExcuteSQLGroup(BaseConfigInfo bci,List<string> files)
        {
            //设置数据库脚本路径
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "install/SQL");
            string corePath = Path.Combine(basePath, "core");
            foreach (string file in files)
            {
                ExcuteSQL(bci, Path.Combine(corePath, file));
            }
            DirectoryInfo di = new DirectoryInfo(basePath);
            DirectoryInfo[] ds = di.GetDirectories();
            foreach (DirectoryInfo d in ds)
            {
                if (d.Name.ToLower() != "core")
                {
                    string otherPath = Path.Combine(basePath,d.Name);
                    foreach (string file in files)
                    {
                        ExcuteSQL(bci, Path.Combine(otherPath, file));
                    }
                }
            }
        }

        /// <summary>
        /// 执行install/SQL所有文件
        /// </summary>
        /// <param name="bci"></param>
        /// <param name="log"></param>
        public static void ExcuteSQLGroup(BaseConfigInfo bci)
        {
            List<string> files=new List<string>();
            files.Add("create.xml");
            files.Add("install.xml");
            files.Add("update.xml");
            files.Add("data.xml");
            ExcuteSQLGroup(bci, files);
        }

        /// <summary>
        /// 获取指定远程网页内容
        /// </summary>
        /// <param name='strUrl'>所要查找的远程网页地址</param>
        /// <param name='timeout'>超时时长设置，一般设置为8000</param>
        /// <param name='enterType'>是否输出换行符，0不输出，1输出文本框换行</param>
        /// <param name='EnCodeType'>编码方式</param>
        /// <returns></returns>
        public static string GetRemoteWebString(string strUrl, int timeout, int enterType, Encoding EnCodeType)
        {
            string strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Timeout = timeout;
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, EnCodeType);
                StringBuilder strBuilder = new StringBuilder();

                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                    if (enterType == 1)
                    {
                        strBuilder.Append("\r\n");
                    }
                }
                strResult = strBuilder.ToString();
            }
            catch (Exception err)
            {
                strResult = "请求错误：" + err.Message;
            }
            return strResult;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="URL">要下载文件网址</param>
        public static string DownloadFileFromUrl(string url,string savePath)
        {
            WebClient client = new WebClient();
            int n = url.LastIndexOf('/');
            string URLAddress = url.Substring(0, n);  //取得网址
            string fileName = url.Substring(n + 1, url.Length - n - 1);  //取得文件名
            string path = savePath + '\\' + fileName; //下载文件存放完整路径
            client.DownloadFile(url, path);
            return path;
        }

        /// <summary>
        /// 判断后面的版本号是否比前面的更大
        /// </summary>
        /// <param name="oldversion"></param>
        /// <param name="newversion"></param>
        /// <returns></returns>
        public static bool VersionLater(string oldversion, string newversion)
        {
            string[] oldNum = oldversion.Split('.');
            string[] newNum = newversion.Split('.');
            int min = oldNum.Length;
            if (newNum.Length < min) min = newNum.Length;
            bool isLater=false;
            for (int i = 0; i < min; i++)
            {
                if(We7Helper.IsNumber(oldNum[i]) && We7Helper.IsNumber(newNum[i]) )
                {
                    if (int.Parse(oldNum[i]) < int.Parse(newNum[i]))
                    {
                        isLater = true;
                        break;
                    }
                }
            }

            if (!isLater && newNum.Length > oldNum.Length)
                isLater = true;

            return isLater;
        }

        /// <summary>
        /// 升级前备份旧系统文件
        /// </summary>
        /// <param name="rootPath">系统根目录</param>
        /// <param name="backupPath">备份目录</param>
        public static string BackupOldFiles(string rootPath,string backupPath)
        {
            //检查文件夹是否存在
            string tempPath = Path.Combine(rootPath, "_temp\\backupBeforeUpdate");
            if (Directory.Exists(tempPath))
            {
                DirectoryInfo d = new DirectoryInfo(tempPath);
                We7Helper.DeleteFileTree(d);
            }

            //不需要复制的文件夹
            string[] folderList = new string[] { "_data", "_skins", 
                    "_temp", "_backup", "Plugins","log"};

            ArrayList folders=ArrayList.Adapter(folderList);
            DirectoryInfo di = new DirectoryInfo(rootPath);
            DirectoryInfo[] ds = di.GetDirectories();

            //复制当前网站目录结构,到tempPath
            foreach (DirectoryInfo d in ds)
            {
                if (!folders.Contains(d.Name.ToLower()))
                    We7Helper.CopyDirectory(d.FullName,Path.Combine(tempPath,d.Name));
            }

            FileInfo[] files = di.GetFiles();
            foreach (FileInfo f in files)
            {
                File.Copy(f.FullName, Path.Combine(tempPath, f.Name));
            }

            if (!Directory.Exists(backupPath))
                Directory.CreateDirectory(backupPath);

            string[] FileProperties = new string[2];
            FileProperties[0] = tempPath;//临时目录，将被压缩
            FileProperties[1] = Path.Combine(backupPath, "backup-" + DateTime.Today.ToString("yyyy-MM-dd-") + DateTime.Now.GetHashCode() + ".zip");//压缩后的目录

            //压缩文件
            try
            {
                ZipClass.ZipFileMain(FileProperties);

                //压缩之后删除临时文件
                DirectoryInfo d = new DirectoryInfo(tempPath);
                We7Helper.DeleteFileTree(d);

                return FileProperties[1];
            }
            catch
            {

            }
            return "";
        }

        /// <summary>
        /// 升级后清理旧系统文件
        /// </summary>
        public static void ClearOldFiles(string rootPath)
        {
            string[] folderList = new string[] { "_data", "_skins", 
                    "_temp", "_backup", "install","web.config" ,"global.asax","app_data","config","bin","cgi-bin"};

            ArrayList folders = ArrayList.Adapter(folderList);
            DirectoryInfo di = new DirectoryInfo(rootPath);
            DirectoryInfo[] ds = di.GetDirectories();
            foreach (DirectoryInfo d in ds)
            {
                if (!folders.Contains(d.Name.ToLower()))
                    We7Helper.DeleteFileTree(d);
            }

            FileInfo[] files = di.GetFiles();
            foreach (FileInfo f in files)
            {
                if (!folders.Contains(f.Name.ToLower()))
                {
                    File.SetAttributes(f.FullName, FileAttributes.Normal);
                    File.Delete(f.FullName);
                }
            }
        }

        public static bool BackupDatabase(BaseConfigInfo bci, string file)
        {
            string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_temp/backupDB");
            if (Directory.Exists(tempPath))
            {
                DirectoryInfo d = new DirectoryInfo(tempPath);
                We7Helper.DeleteFileTree(d);
            }

            DatabaseInfo dbi = GetDatabaseInfo(bci);
            string dbFile = dbi.DBFile.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
            bool success = false;
            switch (bci.DBType.ToLower())
            {
                case "sqlite":
                case "access":
                    File.Copy(dbFile, Path.Combine(tempPath, Path.GetFileName(file)), true);
                    success = true;
                    break;

                case "sqlserver":
                    string masterString = String.Format("Server={0};Database={1};User={2};Password={3}", dbi.Server, "master", dbi.User, dbi.Password);
                    string sql = String.Format("backup database {0} to disk='{1}' with init", dbi.Database, Path.Combine(tempPath, Path.GetFileName(file)));

                    //创建数据库
                    IDbDriver driver = new SqlDbDriver();
                    using (IConnection conn = driver.CreateConnection(masterString))
                    {
                        SqlStatement st = new SqlStatement(sql);
                        driver.FormatSQL(st);
                        conn.Update(st);
                    }
                    success = true;
                    break;
                case "mysql":
                    
                    break;
            }

            if (success)
            {
                string[] FileProperties = new string[2];
                FileProperties[0] = tempPath;//压缩目录
                FileProperties[1] = file;//压缩后的目录
                if (File.Exists(FileProperties[1]))
                    File.Delete(FileProperties[1]);

                //压缩文件
                try
                {
                    ZipClass.ZipFileMain(FileProperties);
                }
                catch
                {

                }
            }

            return success;
        }

        /// <summary>
        /// 创建所有的模型表
        /// </summary>
        /// <returns></returns>
        public static void CreateModelTables()
        {
            new DataBaseHelper().CreateModelTables();
        }
    }
}
