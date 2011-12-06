using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS
{
    /// <summary>
    /// 统计键值对象
    /// </summary>
    [Serializable]
    public class PVKeyCountView
    {
        string keyword;
        int count;
        double percent;

        public double Percent
        {
            get { return percent; }
            set { percent = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public string KeyValue
        {
            get { return keyword; }
            set { keyword = value; }
        }
    }
}
