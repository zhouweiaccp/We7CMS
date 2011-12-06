using System;
using System.Collections.Generic;
using System.Web;

namespace We7.CMS.Web.Admin.ContentModel.ajax
{
	/// <summary>
	/// ModelSort 的摘要说明
	/// </summary>
	public class ModelSort : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			string model = We7Helper.GetParamValueFromUrl(context.Request.RawUrl, "model");

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