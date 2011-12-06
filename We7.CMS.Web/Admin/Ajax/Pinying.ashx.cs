using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.Ajax
{
    /// <summary>
    /// 获取简体中文拼音首字母
    /// </summary>
    public class Pinying : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string result=string.Empty;
            try
            {
                string stringGB = context.Request["str"];
                if (!string.IsNullOrEmpty(stringGB))
                {
                    CNspellTranslator ct = new CNspellTranslator();
                    result = ct.GetSpells(stringGB);
                }
            }
            catch (Exception ex)
            {
            }
            context.Response.Write(result);
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