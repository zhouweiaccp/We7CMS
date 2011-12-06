using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace We7.Model.Core
{
    public class ListResult
    {
        /// <summary>
        /// 数据表
        /// </summary>
        public DataTable DataTable;

        /// <summary>
        /// 总记录条数
        /// </summary>
        public int RecoredCount;

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex;
    }
}
