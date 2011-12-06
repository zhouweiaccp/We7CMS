using System;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Web;
using We7.CMS.Config;

using Thinkment.Data;

namespace We7.CMS.Install
{
    /// <summary>
    /// 数据库迁移动作集
    /// </summary>
    public class DBMigrator
    {
        static DBMigrator()
        {
        }

        public static void DoMigrate(string xmlPath, BaseConfigInfo oldConfig, BaseConfigInfo newConfig)
        {
            DoMigrate(xmlPath, oldConfig, newConfig, null);
        }

        /// <summary>
        /// 执行数据迁移
        /// </summary>
        /// <param name="xmlPath">数据结构映射XML文件存放路径（需要确保两个库的结构相同）</param>
        /// <param name="oldConfig">源数据库</param>
        /// <param name="newConfig">目标数据库</param>
        public static void DoMigrate(string xmlPath, BaseConfigInfo oldConfig, BaseConfigInfo newConfig, List<string> tables)
        {
            //导入数据
            string newConnString = newConfig.DBConnectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory); 
            IDbDriver driver = Installer.CreateDbDriver(newConfig.DBType);
            using (IConnection newConn = driver.CreateConnection(newConnString))
            {
                if (Directory.Exists(xmlPath))
                {
                    DirectoryInfo dir = new DirectoryInfo(xmlPath);
                    FileInfo[] files = dir.GetFiles("*.xml");
                    foreach (FileInfo file in files)
                    {
                        string xmlFile = file.FullName;
                        ObjectAssistant oa = new ObjectAssistant();

                        if (oldConfig.DBConnectionString != "")
                        {
                            oldConfig.DBConnectionString = oldConfig.DBConnectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
                            oa.LoadDBConnectionString(oldConfig.DBConnectionString, oldConfig.DBDriver);
                        }
                        oa.LoadFromFile(xmlFile);

                        //过滤：找出两个数据库都有的表对象
                        IDbDriver oldDriver = Installer.CreateDbDriver(oldConfig.DBType);
                        IConnection oldConn = oldDriver.CreateConnection(oldConfig.DBConnectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory));
                        List<string> objects = GetUpdateObjects(xmlFile, newConn, oldConn, tables);
                        if (objects.Count > 0)
                        {
                            //进行数据转换
                            foreach (string tpName in objects)
                            {
                                if (tpName == "")
                                {
                                    continue;
                                }
                                Type tp = Type.GetType(tpName);
                                Type mt = typeof(MigrateObject<>);
                                Type gt = mt.MakeGenericType(new Type[] { tp });
                                IMigrateObject mo = Activator.CreateInstance(gt) as IMigrateObject;
                                mo.Connection = newConn;
                                mo.Assistant = oa;
                                mo.Update();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 备份数据库配置文件
        /// </summary>
        /// <param name="bci"></param>
        /// <returns></returns>
        public static bool BackupBaseConfig(BaseConfigInfo bci)
        {
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("/_Temp")))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/_Temp"));
                BaseConfigs.SaveConfigTo(bci, HttpContext.Current.Server.MapPath("/_Temp/db.config"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取可以更新的数据库表，条件：筛选XML表，在两个库中都存在
        /// </summary>
        /// <param name="file"></param>
        /// <param name="newConn"></param>
        /// <param name="oldConn"></param>
        /// <returns></returns>
        static List<string> GetUpdateObjects(string file, IConnection newConn, IConnection oldConn, List<string> tables)
        {
            List<string> objects = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            foreach (XmlNode nd in doc.SelectNodes("Objects/Database"))
            {
                foreach (XmlNode n in nd.SelectNodes("Object"))
                {
                    string table = n.Attributes["table"].Value;
                    string newtable = newConn.Driver.FormatTable(table);
                    string oldtable = oldConn.Driver.FormatTable(table);
                    if (tables == null || tables != null && tables.Contains(table))
                    {
                        if (table != null && table.Length > 0 && newConn.TableExist(newtable) && oldConn.TableExist(oldtable))
                        {
                            SqlStatement sql = new SqlStatement("delete from " + newtable + "");
                            newConn.Update(sql);
                            objects.Add(n.Attributes["type"].Value);
                        }
                    }
                }
            }
            return objects;
        }

        interface IMigrateObject
        {
            ObjectAssistant Assistant { get; set; }
            IConnection Connection { get; set; }
            void Update();
        }

        /// <summary>
        /// 这是负责进行数据转换的辅助类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        class MigrateObject<T> : IMigrateObject
        {
            ObjectAssistant assistant;
            IConnection connection;

            public MigrateObject()
            {
            }

            public ObjectAssistant Assistant
            {
                get { return assistant; }
                set { assistant = value; }
            }

            public IConnection Connection
            {
                get { return connection; }
                set { connection = value; }
            }

            public void Update()
            {
                // 从数据持久层获取全部数据
                List<T> list = assistant.List<T>(null, null);
                foreach (T o in list)
                {
                    //用数据持久层把数据更新到另外一个连接指向的数据库中。
                    assistant.Insert(connection, o, null);
                }
            }
        }
    }
}
