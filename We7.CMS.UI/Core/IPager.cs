using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public interface IPager
    {
        /// <summary>
        /// 禁止分页
        /// </summary>
        bool Disable { get; set; }
        /// <summary>
        /// 当前的页码
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 记录总条数
        /// </summary>
        int RecordCount { get; set; }

        /// <summary>
        /// 总共页数
        /// </summary>
        int PageCount { get; set; }

        /// <summary>
        /// 每一页显示的记录数
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 开始记录
        /// </summary>
        int StartItem { get; }

        /// <summary>
        /// 分页结束记录
        /// </summary>
        int EndItem { get; }

        /// <summary>
        /// 当前页面记录
        /// </summary>
        int PageItemsCount { get; }
    }


    public class Pager : IPager
    {
        private int pageCount;
        private int pageIndex = 1;
        private int pageSize = 10;

        public Pager()
        {
        }

        public Pager(int recordCount, int pageIndex)
        {
            RecordCount = recordCount;
            this.pageIndex = pageIndex;
        }

        public Pager(int recordCount, int pageIndex, int pageSize)
            : this(recordCount, pageIndex)
        {
            this.pageSize = pageSize;
        }

        /// <summary>
        /// 禁止分页
        /// </summary>
        public bool Disable { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (!Disable)
                {
                    try
                    {
                        string sp = HttpContext.Current.Request != null ? HttpContext.Current.Request["PageIndex"] : "1";
                        pageIndex = String.IsNullOrEmpty(sp) ? 1 : Convert.ToInt32(sp);
                    }
                    catch { pageIndex = 1; }
                }
                else
                {
                    pageIndex = 1;
                }
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        /// <summary>
        /// 记录总条数
        /// </summary>
        public virtual int RecordCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                pageCount = RecordCount / PageSize;
                pageCount = RecordCount % PageSize == 0 ? pageCount : (pageCount + 1);
                return pageCount;
            }
            set
            {
                pageCount = value;
            }
        }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        /// <summary>
        ///　分页开始的条目
        /// </summary>
        public int StartItem
        {
            get
            {

                int start = (PageIndex - 1) * PageSize;
                return start < 0 ? 0 : start;
            }
        }

        /// <summary>
        ///  分页结束的条目
        /// </summary>
        public int EndItem
        {
            get
            {
                int end = PageIndex * PageSize - 1;
                return end >= RecordCount ? (RecordCount - 1) : end;
            }
        }

        /// <summary>
        /// 当前页面的记录数
        /// </summary>
        public int PageItemsCount
        {
            get
            {
                int count = EndItem - StartItem + 1;
                return count > RecordCount ? 0 : count;
            }
        }
    }

    /// <summary>
    /// 数据源控件
    /// </summary>
    public interface IDataSource : IPager
    {
        /// <summary>
        /// 数据
        /// </summary>
        Object Data { get; set; }
    }

}
