using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Model.Core.UI
{
    public class PagerContainer : CommandContainer
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public virtual int RecordCount { get; set; }

        /// <summary>
        /// 当前记录数
        /// </summary>
        public virtual int CurrentPageIndex { get; set; }
    }
}
