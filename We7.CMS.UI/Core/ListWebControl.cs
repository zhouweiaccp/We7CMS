using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 列表类数据提供者抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListWebControl<T> : BaseWebControl
    {
        /// <summary>
        /// 记录的总条数
        /// </summary>
        public override int RecordCount
        {
            get
            {
                object o = Get("RecordCount") ?? GetListAction().RecordCount;
                return (int)o;
            }
        }

        /// <summary>
        /// 返回列表记录
        /// </summary>
        public List<T> Items
        {
            get
            {
                return GetListAction().Records as List<T>;
            }
        }
         
        /// <summary>
        /// 取得列表的Action
        /// </summary>
        /// <returns>列表Action</returns>
        public virtual IListAction GetListAction()
        {
            throw new Exception("请选实现GetListAction方法");
        }
    }
}
