using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Reflection;

namespace We7.Framework.Util
{
    public class JavascriptHelper
    {
        /// <summary>
        /// Jquery文件的路径
        /// </summary>
        public const string JqueryPath = "~/Admin/Ajax/jquery/jquery-1.3.2.min.js";

        /// <summary>
        /// 当正在访问的页面
        /// </summary>
        public static Page Page
        {
            get
            {
                Page page=HttpContext.Current.Handler as Page;
                if (page == null)
                {
                    throw new Exception("当前页不是System.Web.UI.Page类型，或当前页为空");
                }
                return page;
            }
        }

        /// <summary>
        /// 客户端脚本管理
        /// </summary>
        public static ClientScriptManager ClientScript
        {
            get { return Page.ClientScript; }
        }

        /// <summary>
        /// 注册Jquery
        /// </summary>
        public static void RegisterJquery()
        {
            if (!ClientScript.IsClientScriptIncludeRegistered(Page.GetType(), "Jquery"))
            {
                ClientScript.RegisterClientScriptInclude(Page.GetType(), "Jquery", Page.ResolveUrl(JqueryPath));
            }
        }

        /// <summary>
        /// 为当前页注册form控件
        /// </summary>
        public static void RegisterForm()
        {
        }

        static void Page_PreRenderComplete(object sender, EventArgs e)
        {
            if (Page.Form == null)
            {

                Control body = GetBody();
                if (body == null)
                {
                    throw new Exception("当前面不存在Body");
                }
                HtmlForm form = new HtmlForm();
                foreach (Control c in body.Controls)
                {
                    form.Controls.Add(c);
                }
                body.Controls.Add(form);                
            }
        }

        /// <summary>
        /// 取得Body
        /// </summary>
        /// <returns></returns>
        public static Control GetBody()
        {
            return GetBody(Page); ;
        }

        /// <summary>
        /// 取得Body
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static Control GetBody(Control c)
        {
            PropertyInfo prop=c.GetType().GetProperty("TagName");
            if (prop != null)
            {
                string tagName = prop.GetValue(c, null) as string;
                if (String.Compare(tagName, "body", true) == 0)
                {
                    return c;
                }                    
            }

            foreach (Control child in c.Controls)
            {
                Control ctr = GetBody(child);
                if (ctr != null)
                    return ctr;
            }
            return null;
        }
    }
}
