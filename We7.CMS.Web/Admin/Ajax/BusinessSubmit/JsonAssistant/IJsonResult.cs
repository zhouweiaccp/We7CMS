using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace We7.CMS.Web.Admin.Ajax.BusinessSubmit
{
    /// <summary>
    /// Json接口类
    /// </summary>
    public interface IJsonResult
    {
        /// <summary>
        /// 输出Json
        /// </summary>
        /// <param name="cmd">sql语句</param>
        /// <returns></returns>
        string ToJson(IQueryCondition condition);
        /// <summary>
        /// 返回代码
        /// 响应正常，状态码为200
        /// 发生错误时，响应为200
        /// </summary>
        string code();
        /// <summary>
        /// 信息
        /// </summary>
        string Message();
    }
}
