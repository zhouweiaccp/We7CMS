using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Web.UI;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 列表控件基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class ListUserControl<T> : FrontUserControl
        where T : class, new()
    {
        protected virtual List<T> Items
        {
            get
            {
                Criteria c = CreateCriteria();
                if (c == null)
                    throw new ArgumentNullException("查询条件不能为空");

                RecordCount = Assistant.Count<T>(c);
                return Assistant.List<T>(c, CreateOrders(), Pager.StartItem,Pager.PageItemsCount);
            }
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <returns></returns>
        protected abstract Criteria CreateCriteria();

        /// <summary>
        /// 创建排序列表
        /// </summary>
        /// <returns></returns>
        protected abstract Order[] CreateOrders();
    }
}
