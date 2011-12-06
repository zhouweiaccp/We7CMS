using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.WebControls.Core;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 列表控件分页信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class ListUserControl<T>
    {
        private Pager pager;

        public virtual IPager Pager
        {
            get
            {
                return pager = pager ?? new Pager();
            }
        }

        /// <summary>
        /// 禁止分页
        /// </summary>
        [Option("Boolean")]
        [Desc("禁止分页")]
        public virtual bool DisablePager
        {
            get { return Pager.Disable; }
            set { Pager.Disable = value; }
        }

        /// <summary>
        /// 记录总条数
        /// </summary>
        public virtual int RecordCount
        {
            get { return Pager.RecordCount; }
            set { Pager.RecordCount = value; }
        }

        /// <summary>
        /// 每页条数
        /// </summary>
        [Option("Number")]
        [Desc("页数")]
        [Default(10)]
        [Required]
        public int PageSize
        {
            get { return Pager.PageSize; }
            set { Pager.PageSize = value; }
        }
    }
}
