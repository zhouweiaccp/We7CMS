using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using We7.CMS.WebControls.Core;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 呈现参数
    /// </summary>
    public class RenderArguments
    {
        /// <summary>
        /// 当前控件
        /// </summary>
        public BaseControl Control { get; set; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool IsFinished { get; set; }

        /// <summary>
        /// 是否出错
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 扩展参数
        /// </summary>
        public Dictionary<string, object> Params { get; set; }

        /// <summary>
        /// Request
        /// </summary>
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        /// <summary>
        /// Response
        /// </summary>
        public HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        /// <summary>
        /// 当前页面上下文
        /// </summary>
        public HttpContext Current
        {
            get { return HttpContext.Current; }
        }

        public RenderArguments(BaseControl uc)
        {
            Control = uc;
        }

        public RenderArguments(BaseControl uc, Dictionary<string, object> uxParam)
            : this(uc)
        {
            Params = uxParam;
        }

        public RenderArguments(BaseControl uc, Dictionary<string, object> uxParams, bool isError, Exception ex)
            : this(uc, uxParams)
        {
            IsError = isError;
            Exception = ex;
        }
    }

}
