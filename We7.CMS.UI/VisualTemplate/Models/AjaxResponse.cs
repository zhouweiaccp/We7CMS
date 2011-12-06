using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace We7.CMS.Module.VisualTemplate
{
    public class AjaxResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 返回信息提示
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回的数据
        /// </summary>
        public object Data { get; set; }

        public AjaxResponse(bool success, string message, object data)
        {
            this.Success = Success;
            this.Data = data;
            this.Message = Message;
        }
        public AjaxResponse(bool sucess, string message)
        {
            this.Message = message;
            this.Success = sucess;
        }
        public AjaxResponse():this(true,string.Empty) { }

        /// <summary>
        /// 转换成JSON字符串
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {

           return JavaScriptConvert.SerializeObject(this).Replace("null", "\"\"");
        }
    }
}
