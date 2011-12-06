using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using log4net;

namespace We7.Framework
{
    /// <summary>
    /// Log Helper
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// Shre Error
        /// </summary>
        public static string shareError =  @"shareError\shareError.log";

        /// <summary>
        /// //IPluginCommand
        /// </summary>
        public static string sql_plugin_update =  @"plugins\sql_plugin_update_log_" + DateTime.Today.ToString("yyyyMM") + ".txt";

        /// <summary>
        /// //InstalWebService
        /// </summary>
        public static string sql_update =  @"Install\sql_update_log_" + DateTime.Today.ToString("yyyyMM") + ".txt";


        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        #region static void WriteLog(Type t, Exception ex)
        public static void WriteLog(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error("Error", ex);
        }
        #endregion

        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        #region static void WriteLog(Type t, string msg)
        public static void WriteLog(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error(msg);
        }
        #endregion


        /// <summary>
        /// 为特定用途，写文本文件做日志
        ///     \Log\
        /// </summary>
        /// <param name="fileName">xxx.txt</param>
        /// <param name="strTitle">错误信息</param>
        /// <param name="strContent">错误提示</param>
        #region static void WriteFileLog(String fileName, String strTitle, String strContent)
        public static void WriteFileLog(String fileName, String strTitle, String strContent)
        {
            String strFile, strDir;
            StreamWriter sw;
            DateTime now = DateTime.Now;

            strDir = AppDomain.CurrentDomain.BaseDirectory + @"\Log\";

            //创建多级目录
            string[] parts = fileName.Replace("/", "\\").Split('\\');
            if (parts.Length > 1)
            {
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    string part = parts[i];
                    strDir += part + @"\";
                    if (!Directory.Exists(strDir))
                        Directory.CreateDirectory(strDir);
                }
                strFile = strDir + parts[parts.Length - 1];
            }
            else
                strFile = strDir + fileName;

            //文件
            if (File.Exists(strFile))
            {
                sw = File.AppendText(strFile);
            }
            else
            {
                sw = File.CreateText(strFile);
            }
            sw.WriteLine("<----------------{0}------------------->", now.ToLongDateString() + " " + now.ToLongTimeString());
            sw.WriteLine("Title:");
            sw.WriteLine("\t" + strTitle);
            sw.WriteLine("Content");
            sw.WriteLine("\t" + strContent);
            sw.WriteLine("");
            sw.Close();
        }
        #endregion
    }
}
