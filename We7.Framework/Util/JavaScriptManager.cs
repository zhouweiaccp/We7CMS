using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace We7.Framework.Util
{
    /// <summary>
    /// Javascript管理器
    /// </summary>
    public static class JavaScriptManager
    {
        /// <summary>
        /// 包含JS引用。
        /// </summary>
        /// <param name="filePaths"></param>
        public static void Include(params string[] filePaths)
        {
            HttpContext context = HttpContext.Current;
            if (null == context)
            {
                throw new Exception("HttpContext为空。");
            }

            System.Web.UI.Page p = context.CurrentHandler as System.Web.UI.Page;
            if (null == p)
            {
                throw new Exception("HttpContext.CurrentHandler不是Page。");
            }

            IList<string> jss = GetIncludedJavaScript();
            string resolveUrl;
            foreach (string filePath in filePaths)
            {
                resolveUrl = p.ResolveUrl(filePath);
                if (!jss.Contains(resolveUrl))
                {
                    jss.Add(p.ResolveUrl(resolveUrl));
                    System.Web.UI.HtmlControls.HtmlGenericControl script = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
                    script.Attributes.Add("type", "text/javascript");
                    script.Attributes.Add("src", filePath);
                    if(p.Header!=null)
                        p.Header.Controls.Add(script);
                    else
                        throw new Exception("页面中没有 runat=server 的header对象存在。");
                }
            }
            HttpContext.Current.Items["IncludedJavaScript"] = jss;
        }

        /// <summary>
        /// 获取已经包含的JS列表
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetIncludedJavaScript()
        {
            HttpContext context = HttpContext.Current;
            if (null == context)
            {
                throw new Exception("HttpContext为空。");
            }

            IList<string> jss = HttpContext.Current.Items["IncludedJavaScript"] as IList<string>;
            if (null == jss)
            {
                jss = new List<string>();
                HttpContext.Current.Items["IncludedJavaScript"] = jss;
            }
            return jss;
        }
    }
}
