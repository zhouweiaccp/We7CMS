using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace We7.Framework
{
    internal class SimpleScheduler : Scheduler
    {
        public override void Queue(WaitCallback callBack, object state)
        {
            ThreadPool.QueueUserWorkItem(callBack, state);
        }

        public override void Queue(WaitCallback callBack)
        {
            ThreadPool.QueueUserWorkItem(callBack);
        }

        public override void Paiallel(WaitCallback callBack, object state)
        {
            ThreadPool.QueueUserWorkItem(callBack, state);
        }

        public override void Paiallel(WaitCallback callBack)
        {
            ThreadPool.QueueUserWorkItem(callBack);
        }
    }
}
