using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Web.Admin.Ajax.BusinessSubmit
{
    /// <summary>
    /// 构造SQL接口
    /// </summary>
    public interface IQueryCondition
    {
        /// <summary>
        /// 注册输出Json
        /// </summary>
        event ResponseDelegate ResponseJsonEvent;

        /// <summary>
        /// 输出Json
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        string ToJson(IQueryCondition condition);
    }
    public delegate string ResponseDelegate(IQueryCondition condition);
}
