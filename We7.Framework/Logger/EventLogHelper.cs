using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Web;
using System.IO;

namespace We7.Framework
{
    /// <summary>
    /// We7各服务错误日志
    /// update{
    /// date:"2011-9-29",
    /// author:"Brian.Gou"
    /// }
    /// 
    /// </summary>
    public class EventLogHelper
    {
        private static readonly string LogName = "We7Group";

        /// <summary>
        /// 写事件日志
        /// </summary>
        /// <param name="source">日志发生源（工程名—类名—方法名）</param>
        /// <param name="ex"></param>
        /// <param name="logType"></param>
        public static void WriteToLog(string source, Exception ex, EventLogEntryType logType)
        {
            try
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, LogName);
                }

                string message = "";
                message = ex.Source;
                if (ex.GetType().Equals(typeof(Thinkment.Data.DataException)))
                {
                    Thinkment.Data.DataException dex = (Thinkment.Data.DataException)ex;
                    message += ("\r\n\r\n" + dex.ErrorCode.ToString());
                }
                message += ("\r\n\r\n" + ex.Message);
                message += ("\r\n\r\n" + ex.StackTrace);

                EventLog.WriteEntry(source, message, logType);
                
            }
            catch (Exception)
            {
            }
            finally
            {
                string message = "";
                message = ex.Source;
                if (ex.GetType().Equals(typeof(Thinkment.Data.DataException)))
                {
                    Thinkment.Data.DataException dex = (Thinkment.Data.DataException)ex;
                    message += ("\r\n\r\n" + dex.ErrorCode.ToString());
                }
                message += ("\r\n\r\n" + ex.Message);
                message += ("\r\n\r\n" + ex.StackTrace);
                SaveLogToFile(source, message);

            }

        }

        /// <summary>
        /// 写事件日志
        /// </summary>
        /// <param name="source">日志发生源（工程名—类名—方法名）</param>
        /// <param name="ex"></param>
        public static void WriteToLog(string source, Exception ex)
        {
            try
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, LogName);
                }

                string message = "";
                message = ex.Source;
                if (ex.GetType().Equals(typeof(Thinkment.Data.DataException)))
                {
                    Thinkment.Data.DataException dex = (Thinkment.Data.DataException)ex;
                    message += ("\r\n\r\n" + dex.ErrorCode.ToString());
                }
                message += ("\r\n\r\n" + ex.Message);
                message += ("\r\n\r\n" + ex.StackTrace);

                EventLog.WriteEntry(source, message, EventLogEntryType.Information);
            }
            catch (Exception)
            {
            }


        }

        /// <summary>
        /// 保存到文件日志中
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void SaveLogToFile(string source, string message)
        {
            string logFileName = LogHelper.shareError;
            HttpContext context = HttpContext.Current;
            //string path = context.Server.MapPath(logFileName);
            string dir1 = Path.GetDirectoryName(logFileName);
            if (!Directory.Exists(dir1))
            {
                Directory.CreateDirectory(dir1);
            }
            string input = "来源：" + source + "\r\n-------------------\r\n";
            input += message;
            SaveLog(input, logFileName);
        }


        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <param name="logFileName">文件名称</param>
        public static void SaveLog(string input, string logFileName)
        {
            string title=new string('-', 30) + DateTime.Now.ToString("M/d H:m") + new string('-', 30);
            string content=input.Replace("\t", "  ");
            We7.Framework.LogHelper.WriteFileLog(logFileName, title, content);

            //HttpContext context = HttpContext.Current;
            //FileStream wri = new FileStream(context.Server.MapPath(logFileName), FileMode.Append);
            //StreamWriter wrs = new StreamWriter(wri);
            //wrs.WriteLine(new string('-', 30) + DateTime.Now.ToString("M/d H:m") + new string('-', 30));
            //wrs.WriteLine(input.Replace("\t", "  "));
            //wrs.Close();
            //wri.Close();
        }
    }
}
