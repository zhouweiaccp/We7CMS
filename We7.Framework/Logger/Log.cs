using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;
using System.IO;

namespace We7.Framework
{
    public class Logger
    {
        static readonly ILog log;
        static string logFile;
        static Logger()
        {
            LogManager mgr = new LogManager();
            FileLog fileLog = new FileLog();
            logFile = fileLog.FileUrlName;
            mgr.Register(fileLog);
            log = mgr;
        }

        public static string GetFileName()
        {
            return logFile;
        }

        public static void Write(string msg)
        {
            log.Append(msg);
        }

        public static void Info(string msg)
        {
            WriteLine();
            Write("提示信息");
            WriteDate();
            log.Append(msg);
        }

        public static void Error(string msg)
        {
            WriteLine();
            Write("错误信息");
            WriteDate();
            log.Append(msg);
        }

        public static void Warn(string msg)
        {
            WriteLine();
            Write("警告信息");
            WriteDate();
            log.Append(msg);
        }

        public static void WriteLine()
        {
            WriteLine('-');
        }

        public static void WriteLine(char c)
        {
            log.Append("".PadLeft(50, c));
        }

        public static void WriteDate(string format)
        {
            log.Append(DateTime.Now.ToString(format));
        }

        public static void WriteDate()
        {
            log.Append(DateTime.Now.ToString());
        }
    }

    public interface ILog
    {
        void Append(string msg);
    }

    public class LogManager : ILog
    {
        List<ILog> logs = new List<ILog>();

        public void Register(ILog log)
        {
            logs.Add(log);
        }

        public void Append(string msg)
        {
            foreach (ILog log in logs)
            {
                log.Append(msg);
            }
        }
    }

    public class FileLog : ILog
    {
        static string fileUrl="~/_Log/" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        string LogPath = HttpContext.Current.Server.MapPath(fileUrl);
        Queue<string> queue = new Queue<string>();
        ManualResetEvent mre = new ManualResetEvent(false);

        public FileLog()
        {
            Thread thread = new Thread(delegate()
            {
                while (true)
                {
                    mre.WaitOne();
                    WriteLog();
                    mre.Reset();
                }
            });

            thread.Start();
        }

        public string FileUrlName
        {
            get
            {
                return fileUrl;
            }
        }

        public void WriteLog()
        {
            //try
            //{
                FileInfo file = new FileInfo(LogPath);
                if (!file.Directory.Exists)
                {
                    file.Directory.Create();
                }
                using (StreamWriter writer = !file.Exists ? new StreamWriter(file.Create(), Encoding.UTF8) : file.AppendText())
                {
                    string msg = "";
                    while (!String.IsNullOrEmpty(msg = queue.Dequeue()))
                    {
                        writer.WriteLine();
                        writer.Write(msg);
                        writer.WriteLine();
                        Thread.Sleep(10);
                    }
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                }

            //捕获了异常无处理，不如暴露出来
            //}
            //catch (Exception ex) { }
        }
        public void Append(string msg)
        {
            queue.Enqueue(msg);
            mre.Set();
        }
    }


}
