using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using We7.CMS.Common;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 详细信息控件基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DetailUserControl<T> : FrontUserControl where T : class, new()
    {
        /// <summary>
        /// 当前数据信息
        /// </summary>
        protected virtual T Item
        {
            get
            {
                Criteria c = CreateCriteria();
                if (c == null)
                    throw new ArgumentNullException("查询条件不能为空");

                List<T> list = Assistant.List<T>(c, null, 0, 0);
                T item=list != null && list.Count > 0 ? list[0] : new T();
                FormatItem(item);

                return item;
            }
        }

        protected virtual void FormatItem(T item)
        {
        }

        protected virtual Criteria CreateCriteria()
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", UrlHelper.GetID());
            return c;
        }
    }
}
