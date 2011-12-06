using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Threading;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Reflection;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 声明创建ActionID的委托
    /// </summary>
    /// <returns></returns>
    public delegate string CreateID();

    /// <summary>
    /// Html帮助类
    /// </summary>
    public class HtmlHelper
    {
        private CreateID creater;
        private string actionID;

        /// <summary>
        /// HtmlHelper构造函数
        /// </summary>
        /// <param name="actionID">当前的ActionID（引用类型的变量）</param>
        /// <param name="creater">生成ActionID的委托</param>
        public HtmlHelper(ref string actionID, CreateID creater)
        {
            if (creater == null)
            {
                throw new Exception("不存在ID创建者");
            }
            this.creater = creater;
            this.actionID = actionID;
        }
        /// <summary>
        /// 生成From开始标签
        /// </summary>
        /// <returns>返回Form对象</returns>
        public CMForm BeginForm()
        {
            return new CMForm(ref actionID, creater);
        }

        /// <summary>
        /// 生成From开始标签
        /// </summary>
        /// <param name="action">form的action</param>
        /// <returns>返回Form对象</returns>
        public CMForm BeginForm(string action)
        {
            return new CMForm(ref actionID, creater, action);
        }

        /// <summary>
        /// 生成From开始标签
        /// </summary>
        /// <param name="action">form的action</param>
        /// <param name="id">form的id</param>
        /// <returns>返回Form对象</returns>
        public CMForm BeginForm(string action, string id)
        {
            return new CMForm(ref actionID, creater, action, id);
        }

        /// <summary>
        /// 生成From开始标签
        /// </summary>
        /// <param name="action">form的action</param>
        /// <param name="id">form的id</param>
        /// <param name="method">form的method</param>
        /// <returns>返回Form对象</returns>
        public CMForm BeginForm(string action, string id, string method, string htmlAttribute)
        {
            return new CMForm(ref actionID, creater, action, id, method, htmlAttribute);
        }

        /// <summary>
        /// 生成From结束的标签
        /// </summary>
        public void EndForm()
        {
            CMForm.WriteEnd();
            actionID = creater();
        }

        /// <summary>
        /// 创建From标签的对象
        /// </summary>
        public class CMForm : IDisposable
        {
            public const string BeginFormPattern = "<form id='{0}' action='{1}' method='{2}' {4}>\r\n<input type='hidden' name='_ActionID' value='{3}' />\r\n";
            public const string EndFormPattern = "</form>";
            private CreateID creater;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="actionID">当前的ActionID</param>
            /// <param name="creater">ActionID创建委托</param>
            public CMForm(ref string actionID, CreateID creater) : this(ref actionID, creater, null) { }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="actionID">当前的ActionID</param>
            /// <param name="creater">ActionID创建委托</param>
            /// <param name="action">form的Action</param>
            public CMForm(ref string actionID, CreateID creater, string action) : this(ref actionID, creater, action, null) { }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="actionID">当前的ActionID</param>
            /// <param name="creater">ActionID创建委托</param>
            /// <param name="action">form的Action</param>
            /// <param name="id">form的ID</param>
            public CMForm(ref string actionID, CreateID creater, string action, string id) : this(ref actionID, creater, action, id, null, null) { }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="actionID">当前的ActionID</param>
            /// <param name="creater">ActionID创建委托</param>
            /// <param name="action">form的Action</param>
            /// <param name="id">form的ID</param>
            /// <param name="method">form的Mehtod</param>
            public CMForm(ref string actionID, CreateID creater, string action, string id, string method, string htmlAttribute)
            {
                if (String.IsNullOrEmpty(actionID))
                {
                    throw new Exception("ActionID不能为空");
                }
                id = String.IsNullOrEmpty(id) ? actionID : id;
                method = String.IsNullOrEmpty(method) ? "post" : method;
                this.creater = creater;
                HttpContext.Current.Response.Write(String.Format(BeginFormPattern, id, action, method, actionID, htmlAttribute));
            }

            /// <summary>
            /// 输出</from>
            /// </summary>
            public static void WriteEnd()
            {
                HttpContext.Current.Response.Write(EndFormPattern);
            }

            /// <summary>
            /// 销毁对象时调用
            /// </summary>
            public void Dispose()
            {
                WriteEnd();
                if (creater != null)
                    creater();
            }
        }
    }

    public class HtmlHelper2
    {
        private Control container;
        private InternalForm form;
        public HtmlTextWriter Writer { get; set; }

        public HtmlHelper2(Control container)
        {
            this.container = container;
        }

        public bool IsPostBack
        {
            get { return container.Page.Request[InternalForm.ActionFlag] == container.UniqueID; }
        }

        public T Request<T>(string key)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                return (T)converter.ConvertFromString(container.Page.Request[key]);
            }
            return default(T);
        }

        public InternalForm BeginFrom()
        {
            form = new InternalForm(container, Writer);
            form.ResponseBegin(null);
            return form;
        }

        public InternalForm BeginFrom(HtmlTextWriter writer)
        {
            form = new InternalForm(container,writer);
            form.ResponseBegin(null);
            return form;
        }

        public InternalForm BeginFrom(IDictionary<string, string> args)
        {
            form = new InternalForm(container, Writer);
            form.ResponseBegin(args);
            return form;
        }

        public InternalForm BeginFrom(HtmlTextWriter writer,IDictionary<string, string> args)
        {
            form = new InternalForm(container,writer);
            form.ResponseBegin(args);
            return form;
        }

        public void EndForm()
        {
            if (form != null)
            {
                form.ResponseEnd();
            }
        }

        public class InternalForm : IDisposable
        {
            internal const string BeginFormHtml = "<form id='{0}' name='{0}' action='{1}' target='{2}' method='{3}' {4}>";
            internal const string FlagHtml = "<input type='hidden' name='{0}' value='{1}' />";
            internal const string EndFromHtml = "</form>";
            internal const string SetForm = "<script>var {0}=document.forms['{0}']||docuemnt.{0};</script>";
            public const string ActionFlag = "_ActionFlag";
            private int locknum = 0;
            private int lockhtmlwriter = 0;

            private Control container;
            internal InternalForm(Control container)
            {
                this.container = container;
            }

            internal InternalForm(Control container,HtmlTextWriter writer):this(container)
            {
                HtmlWriter = writer;
            }

            internal void ResponseBegin(IDictionary<string, string> args)
            {
                string action = container.Page.Request.RawUrl,
                       target = "_self",
                       method = "post";
                StringBuilder sb = new StringBuilder();
                if (args != null)
                {
                    foreach (KeyValuePair<string, string> kvp in args)
                    {
                        if ("action".Equals(kvp.Key, StringComparison.CurrentCultureIgnoreCase))
                        {
                            action = kvp.Value;
                            continue;
                        }
                        if ("target".Equals(kvp.Key, StringComparison.CurrentCultureIgnoreCase))
                        {
                            target = kvp.Value;
                            continue;
                        }
                        if ("method".Equals(kvp.Key, StringComparison.CurrentCultureIgnoreCase))
                        {
                            method = kvp.Value;
                            continue;
                        }
                        sb.AppendFormat(" {0}='{1}'", kvp.Key, kvp.Value);
                    }
                }

                Write(String.Format(BeginFormHtml, container.ClientID, action, target, method, sb));
                Write(String.Format(FlagHtml, ActionFlag, container.UniqueID));
                Write(String.Format(SetForm, container.ClientID));
            }

            internal void ResponseEnd()
            {
                Write(EndFromHtml);
            }

            internal HttpResponse Response
            {
                get { return container.Page.Response; }
            }

            public HtmlTextWriter HtmlWriter { get; set; }

            void Write(string content)
            {
                if (HtmlWriter != null)
                {
                    HtmlWriter.Write(content);
                }
                else
                {
                    Response.Write(content);
                }
            }


            #region IDisposable 成员

            public void Dispose()
            {
                if (Interlocked.CompareExchange(ref locknum, 1, 0) == 0)
                {
                    ResponseEnd();
                }
            }

            #endregion
        }
    }

    public class HtmlForm : PlaceHolder
    {
        protected override void Render(HtmlTextWriter writer)
        {

            FrontUserControl uc = Parent as FrontUserControl;
            if (uc != null)
            {
                using (uc.HtmlHelper.BeginFrom(writer))
                {
                    base.Render(writer);
                }
            }
            else
            {
                base.Render(writer);
            }
        }
    }


}
