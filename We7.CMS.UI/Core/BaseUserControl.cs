using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Web.UI;
using We7.Framework;
using We7.CMS.Module.VisualTemplate;
using We7.Framework.TemplateEnginer;
using System.IO;
using HtmlAgilityPack;
using We7.Framework.Util;
using System.Collections;
using System.Reflection;
using We7.CMS.WebControls.Core;
using System.Web;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 用户控件基类
    /// </summary>
    public abstract partial class FrontUserControl : UserControl
    {
        private HtmlHelper2 htmlHelper;
        private DesignHelper designHelper;
        private const string IDIndexKey = "$We7$Sytstem$IDIndex";

        protected Dictionary<string, object> UxParams = new Dictionary<string, object>();

        /// <summary>
        /// 数据查询助手
        /// </summary>
        protected ObjectAssistant Assistant
        {
            get
            {
                return HelperFactory.Instance.Assistant;
            }
        }

        /// <summary>
        /// 表单提交辅助类
        /// </summary>
        public HtmlHelper2 HtmlHelper
        {
            get
            {
                if (htmlHelper == null)
                {
                    htmlHelper = new HtmlHelper2(this);
                }
                return htmlHelper;
            }
        }

        /// <summary>
        /// 设计时辅助类
        /// </summary>
        protected DesignHelper DesignHelper
        {
            get
            {
                if (designHelper == null)
                {
                    designHelper = new DesignHelper(this);
                }
                return designHelper;
            }
        }

        /// <summary>
        /// 业务工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return HelperFactory.Instance; }
        }

        /// <summary>
        /// 当前控件的Css样式
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// 包含样式
        /// </summary>
        /// <param name="includeJquery">是否包含Jquery</param>
        /// <param name="files"></param>
        protected void IncludeJavaScript(bool includeJquery, params string[] files)
        {
            List<string> paths = new List<string>();
            if (includeJquery)
            {
                paths.Add("/Scripts/jquery/jquery-1.4.2.js");
            }
            foreach (string file in files)
            {
                string f=file.Trim();
                if (f.StartsWith("/"))
                {
                    paths.Add(file);
                }
                else
                {
                    paths.Add(this.TemplateSourceDirectory + "/js/" + f);
                }
            }
            JavaScriptManager.Include(paths.ToArray());
        }

        /// <summary>
        /// 包含样式
        /// </summary>
        /// <param name="files"></param>
        protected void IncludeJavaScript(params string[] files)
        {
            IncludeJavaScript(false, files);
        }

        protected T LoadControl<T>(string virtualPath) where T : Control
        {
            T t = LoadControl(virtualPath) as T;
            if (t != null)
            {
                t.ID = typeof(T).Name.Replace(".", "_") + CreateIDIndex();
            }
            return t;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            OnPreLoadCompositeControl(e);
            LoadCompositeControl();
        }

        /// <summary>
        /// 加载组合控件
        /// </summary>
        protected virtual void LoadCompositeControl()
        {
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                if (Attribute.IsDefined(prop, typeof(ChildrenAttribute), true))
                {
                    Control ctr = FindControl("_" + prop.Name) as Control;

                    if (ctr == null)
                        throw new Exception("不存在"+prop.Name+"控件的容器");

                    ctr.Controls.Add(prop.GetValue(this, null) as Control);
                }
            }
        }

        protected virtual void OnPreLoadCompositeControl(EventArgs e)
        {            
        }

        protected int CreateIDIndex()
        {
            int index = HttpContext.Current.Items[IDIndexKey] != null ? (int)HttpContext.Current.Items[IDIndexKey] : 1;
            HttpContext.Current.Items[IDIndexKey] = ++index;
            return index;
        }
    }
}
