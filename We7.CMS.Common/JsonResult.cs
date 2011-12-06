using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace We7.CMS.Common
{
    /// <summary>
    /// 返回Json类据
    /// </summary>
    public interface IJsonResult
    {
        string ToJson();
    }

    /// <summary>
    /// 用于返回Json数据
    /// </summary>
    public class JsonResult:IJsonResult
    {
        /// <summary>
        /// 返回数据是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public IJsonResult Data { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public JsonResult(bool success)
            :this(success,String.Empty)
        {
        }

        public JsonResult(bool success, IJsonResult data)
            : this(success, data, String.Empty)
        {
        }

        public JsonResult(bool success, string desc)
            : this(success, null,desc)
        {
        }

        public JsonResult(bool success, IJsonResult data, string desc)
        {
            Success = success;
            Data = data;
            Description = desc;
        }

        /// <summary>
        /// 返回的数据
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder("{");
            sb.AppendFormat("{0}:{1},", "success", Success.ToString().ToLower());
            if (Data != null)
            {
                sb.AppendFormat("{0}:{1},", "data", Data.ToJson());
            }
            sb.AppendFormat("{0}:'{1}'", "desc", Description);
            sb.Append("}");
            return sb.ToString();
        }

        public void Response()
        {
            HttpResponse ctx = HttpContext.Current.Response;
            ctx.Clear();
            ctx.ContentEncoding = System.Text.Encoding.UTF8;
            ctx.Write(this.ToJson());
            ctx.Flush();
            ctx.End();
        }
    }
}
