using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using We7.Framework.Zip;
using System.Web;
using System.Threading;
using We7.Framework;
using We7.CMS.Config;
using System.Xml;
using We7.Framework.Util;
using Thinkment.Data;

namespace We7.CMS.Install.Plugin
{
    public class ResourceManager
    {
        private const string  PluginDirectory = "~/Plugins";
        private static string SkinPath = HttpContext.Current.Server.MapPath("~/_skin");
        private static string ModelPath = HttpContext.Current.Server.MapPath("~/Models");
        private static string We7ControlsPath = HttpContext.Current.Server.MapPath("~/We7Controls");
        private static string BinPath = HttpContext.Current.Server.MapPath("~/Bin"); 
        private static string DateMappingPath=HttpContext.Current.Server.MapPath("~/App_Data/XML");
        private string PluginDirectoryPath;
        private string PluginBinPath;
        private string PluginPath;
        private static object InstallLock = new object();

        public void InstallLocalResource(Stream stream)
        {
            lock (InstallLock)
            {
                CheckResourceTempDirectory();
                PluginPath=ZipUtils.ExtractZipWithRoot(stream, PluginDirectory);
                InstallSkin();
                InstallModels();
                InstallWebControls();
                InstallBin();
            }            
        }

        void CheckResourceTempDirectory()
        {
            PluginDirectoryPath = HttpContext.Current.Server.MapPath(PluginDirectory+"/temp"+DateTime.Now.Ticks);
            if (!Directory.Exists(PluginDirectoryPath))
                Directory.CreateDirectory(PluginDirectoryPath);
            PluginBinPath = Path.Combine(PluginDirectoryPath, "Bin");
            if (!Directory.Exists(PluginBinPath))
                Directory.CreateDirectory(PluginBinPath);
        }

        /// <summary>
        /// 安装模板
        /// </summary>
        void InstallSkin()
        {
            string path = Path.Combine(PluginPath, "Skin");
            if (!Directory.Exists(path)) return;
            CopyFile(path, SkinPath);
        }

        void InstallModels()
        {
            string path = Path.Combine(PluginPath, "Models");
            if (!Directory.Exists(path)) return;
            CopyFile(path, ModelPath);
        }

        void InstallWebControls()
        {
            string path = Path.Combine(PluginPath, "Control");
            if (!Directory.Exists(path)) return;
            CopyFile(path, We7ControlsPath);
        }

        void InstallData()
        {
            string dataPath = Path.Combine(PluginPath, "Data");
            if (!Directory.Exists(dataPath)) return;
            string mappingPath = Path.Combine(dataPath, "Relation");
            DirectoryInfo di = new DirectoryInfo(mappingPath);
            if (di.Exists)
            {
                CopyFile(di.FullName,DateMappingPath);
            }

                   
            string installPath=Path.Combine(dataPath,"Install.xml");
            if(File.Exists(installPath))
            {
                ExecuteDBFile(dataPath, new List<string>() { "Install.xml" });
            }

            string updatePath=Path.Combine(dataPath,"Update.xml");
            if(File.Exists(updatePath))
            {
                ExecuteDBFile(dataPath, new List<string>() { "Update.xml" });
            }
        }



        void InstallBin()
        {
            string path = Path.Combine(PluginPath, "Bin");
            if (!Directory.Exists(path)) return;
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo f in di.GetFiles())
            {
                f.CopyTo(BinPath,true);
            }
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void CopyFile(string source,string target)
        {
            DirectoryInfo di = new DirectoryInfo(source);
            DirectoryInfo tdi=new DirectoryInfo(target);
            

            if (di.Exists)
            {
                if (!tdi.Exists)
                    tdi.Create();

                FileInfo[] fis=di.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    if (String.Compare(fi.Extension, ".dll", true) == 0)
                    {
                        fi.CopyTo(PluginBinPath);
                    }
                    else
                    {
                        string filePath = Path.Combine(tdi.FullName, fi.Name);
                        fi.CopyTo(filePath, true);
                    }
                }

                DirectoryInfo[] dis = di.GetDirectories();
                foreach (DirectoryInfo d in dis)
                {
                    CopyFile(d.FullName, Path.Combine(tdi.FullName, d.Name));
                }
            }
        }

                /// <summary>
        /// 执行指定的数据文件
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataDirectory"></param>
        /// <param name="filePath"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        private void ExecuteDBFile(string dataDir,List<string> DBFileList)
        {
            foreach (string file in DBFileList)
            {
                string filePath = Path.Combine(dataDir, file);
                if (!File.Exists(filePath))
                    throw new Exception(String.Format("数据文件{0}不存在!", file));
                ExcuteSQL(BaseConfigs.GetBaseConfig(), filePath);
            }
        }

        public void ExcuteSQL(BaseConfigInfo bci, string updateFile)
        {
            if (updateFile != "")
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
                            if (sub == null || String.IsNullOrEmpty(sub.InnerText) || String.IsNullOrEmpty(sub.InnerText.Trim()))
                                continue;
                            //读取SQL语句，逐一执行
                            SqlStatement sql = new SqlStatement();
                            sql.CommandType = System.Data.CommandType.Text;
                            sql.SqlClause = sub.InnerText.Trim();
                            dbDriver.FormatSQL(sql);
                            try
                            {
                                conn.Update(sql);
                                success++;
                            }
                            catch (Exception ex)
                            {
                                //出现了错误，我们继续执行
                    
                                We7.Framework.LogHelper.WriteFileLog(We7.Framework.LogHelper.sql_plugin_update,
                                                                      "执行SQL：" + sql.SqlClause, ex.Message);
                                errors++;
                                continue;
                            }
                        }
                    }

                   
                    We7.Framework.LogHelper.WriteFileLog(We7.Framework.LogHelper.sql_plugin_update.ToString(),
                                                                       "执行完毕：", 
                                                                       string.Format("{3}执行完毕！共执行语句{0}条，成功{1}，失败{2} 。", success + errors, success, errors, updateFile));
                }
            }
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
    }
}
