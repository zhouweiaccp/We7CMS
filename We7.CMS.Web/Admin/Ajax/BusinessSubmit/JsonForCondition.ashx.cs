using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using We7.CMS.Common;
using We7.Framework;
using System.Reflection;
using We7.CMS.Web.Admin.Ajax.BusinessSubmit.Entity;
using Thinkment.Data;

namespace We7.CMS.Web.Admin.Ajax.BusinessSubmit
{
    /// <summary>
    /// JsonForCondition 的摘要说明
    /// </summary>
    public class JsonForCondition : IHttpHandler//, IRequiresSessionState
    {
        /// <summary>
        /// SQL构造接口
        /// </summary>
        private IQueryCondition condiction;
        public JsonForCondition()
        {
            if (!string.IsNullOrEmpty(We7.CMS.Accounts.Security.CurrentAccountID))  //判断是否登录
            {
                condiction = new QueryCondition(HttpContext.Current.Request.Form); //初始化查询对象
                condiction.ResponseJsonEvent += JsonResult.ToJson;
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            if (condiction!=null)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(condiction.ToJson(condiction));
            }
        }

        private IJsonResult jsonResult;
        /// <summary>
        /// Json操作
        /// </summary>
        public IJsonResult JsonResult
        {
            get
            {
                if (jsonResult == null)
                {
                    jsonResult = new JsonResult();
                }
                return jsonResult;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}