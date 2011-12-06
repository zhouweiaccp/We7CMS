using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace We7.Framework
{
    public abstract class Scheduler
    {
        private static Scheduler instance;
        private static Dictionary<string, Scheduler> instances = new Dictionary<string, Scheduler>();

        public static Scheduler Instance()
        {
            if (instance == null)
            {
                instance = new SimpleScheduler();
            }
            return instance;
        }

        public static Scheduler Instance(string key)
        {
            if (!instances.ContainsKey(key))
            {
            }
            return instances.ContainsKey(key) ? instances[key] : null;
        }

        /// <summary>
        /// 执行队列
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="state"></param>
        public abstract void Queue(WaitCallback callBack, object state);

        /// <summary>
        /// 执行队列
        /// </summary>
        /// <param name="callBack"></param>
        public abstract void Queue(WaitCallback callBack);

        /// <summary>
        /// 执行并行任务
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="state"></param>
        public abstract void Paiallel(WaitCallback callBack, object state);

        /// <summary>
        /// 执行并行任务
        /// </summary>
        /// <param name="callBack"></param>
        public abstract void Paiallel(WaitCallback callBack);
    }

    
}
