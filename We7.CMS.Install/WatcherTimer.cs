using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace We7.CMS
{
    public class WatcherTimer
    {
        private int TimeoutMillis = 2000;

        System.IO.FileSystemWatcher fsw = new System.IO.FileSystemWatcher();
        System.Threading.Timer m_timer = null;
        List<String> files = new List<string>();
        FileSystemEventHandler fswHandler = null;
        public static System.Web.HttpContext context = null;

        public WatcherTimer(FileSystemEventHandler watchHandler)
        {
            m_timer = new System.Threading.Timer(new TimerCallback(OnTimer), null, Timeout.Infinite, Timeout.Infinite);
            fswHandler = watchHandler;

        }


        public WatcherTimer(FileSystemEventHandler watchHandler, int timerInterval)
        {
            m_timer = new System.Threading.Timer(new TimerCallback(OnTimer), null, Timeout.Infinite, Timeout.Infinite);
            TimeoutMillis = timerInterval;
            fswHandler = watchHandler;
            context = System.Web.HttpContext.Current;
        }

        public void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Mutex mutex = new Mutex(false, "FSW");
            mutex.WaitOne();
            if (!files.Contains(e.Name))
            {
                files.Add(e.Name);
            }
            mutex.ReleaseMutex();

            m_timer.Change(TimeoutMillis, Timeout.Infinite);
        }

        private void OnTimer(object state)
        {
            List<String> backup = new List<string>();

            Mutex mutex = new Mutex(false, "FSW");
            mutex.WaitOne();
            backup.AddRange(files);
            files.Clear();
            
            string changePath = string.Empty;
            foreach (var item in backup)
            {
                changePath += item+"|";
            }
            fswHandler(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, string.Empty, changePath));
            mutex.ReleaseMutex();
            //foreach (string file in backup)
            //{
            //    fswHandler(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, string.Empty, file));
            //}

        }
    }
}
