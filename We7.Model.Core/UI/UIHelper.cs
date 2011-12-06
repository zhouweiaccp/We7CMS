using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using We7.Model.Core;
using We7.Model.Core.UI;
using We7.Model.Core.Config;
using We7.Framework;
using System.Web;
using System.Collections;

namespace We7.Model.Core
{
    /// <summary>
    /// 控件界面业务类
    /// </summary>
    public class UIHelper
    {
        private Page Page;
        private PanelContext panelContext;

        public UIHelper(Page page, PanelContext ctx)
            : this(page)
        {
            panelContext = ctx;
        }

        public UIHelper(Page page)
        {
            Page = page;
        }
        public FieldControl GetControl(We7Control control)
        {
            FieldControl ctr = Page.LoadControl(GetControlPath(control.Type)) as FieldControl;
            if (ctr == null)
            {
                throw new SysException(control.Type + "不存在");
            }
            ctr.ID = control.ID;
            ctr.InitControl(panelContext, control);
            //ctr.SetFieldData(field);
            //ctr.PanelContext = modelData;
            //ctr.ID = field.Info.ID;
            //ctr.InitControl();
            return ctr;
        }

        public ModelContainer GetContainer(string containername)
        {
            return Page.LoadControl(GetContainerPath(containername)) as ModelContainer;
        }

        public LayoutEditor LoadLayoutEditor(string path)
        {
            return Page.LoadControl(path) as LayoutEditor;
        }

        /// <summary>
        /// 从容器中查找ID与id一致的控件
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="container">容器</param>
        /// <returns>查找到的控件</returns>
        public Control GetControl(string id, System.Web.UI.Control container)
        {
            //TODO::这儿的算法不太好，效率不太高。
            Control c = container.ID == id ? container : null;
            if (c == null)
            {
                foreach (Control cc in container.Controls)
                {
                    c = GetControl(id, cc);
                    if (c is FieldControl)
                        break;
                }
            }
            return c;
        }

        /// <summary>
        /// 从容器中查找ID与id一致的控件
        /// </summary>
        /// <typeparam name="T">查找控件类型</typeparam>
        /// <param name="id">控件ID</param>
        /// <param name="container">容器</param>
        /// <returns>查找到的控件</returns>
        public T GetControl<T>(string id, Control container) where T : class
        {
            T t = container.ID == id ? container as T : null;
            if (t == null)
            {
                foreach (Control c in container.Controls)
                {
                    t = GetControl<T>(id, c);
                    if (t != null)
                        break;
                }
            }
            return t;
        }

        /// <summary>
        /// 取得控件路径
        /// </summary>
        /// <returns>控件的相对路径。服务器端路径,带有"~"</returns>
        public string GetControlPath(string theme, string controlname)
        {
            string pathformat = "{0}/{1}/{2}.ascx";
            return String.Format(pathformat, ModelConfig.ControlsDirectory, theme, controlname);
        }

        /// <summary>
        /// 取得控件路径
        /// </summary>
        /// <param name="controlname">控件名称</param>
        /// <returns>控件的相对路径。服务器端路径,带有"~"</returns>
        public string GetControlPath(string controlname)
        {
            string pathformat = "{0}/{1}.ascx";
            return String.Format(pathformat, ModelConfig.ControlsDirectory, controlname.Contains(".") ? controlname.Replace(".", "/") : String.Format("system/{0}", controlname));
        }

        /// <summary>
        /// 取得容器控件路径
        /// </summary>
        /// <param name="container">控件名称</param>
        /// <returns>控件的相对路径。服务器端路径,带有"~"</returns>
        public string GetContainerPath(string container)
        {
            string pathformat = "{0}/{1}.ascx";
            return String.Format(pathformat, ModelConfig.ContainerDirectory, container.Contains(".") ? container.Replace(".", "/") : String.Format("system/{0}", container));
        }

        public const string MESSAGEKEY = "modelmessagekey";
        /// <summary>
        /// 当前页信息
        /// </summary>
        public static Message Message
        {
            get
            {
                if (HttpContext.Current.Items[MESSAGEKEY] == null)
                {
                    HttpContext.Current.Items[MESSAGEKEY] = new Message();
                }
                return HttpContext.Current.Items[MESSAGEKEY] as Message;
            }
        }

        private static MessageHandler ErrorHandler
        {
            get { return HttpContext.Current.Items["$MessageHandler$ErrorHandler"] as MessageHandler; }
            set { HttpContext.Current.Items["$MessageHandler$ErrorHandler"] = value; }
        }

        private static MessageHandler MessageHandler
        {
            get { return HttpContext.Current.Items["$MessageHandler$MessageHandler"] as MessageHandler; }
            set { HttpContext.Current.Items["$MessageHandler$MessageHandler"] = value; }
        }

        public static void SendMessage(string message, Hashtable ext)
        {
            if (MessageHandler != null)
            {
                foreach (Delegate dl in MessageHandler.GetInvocationList())
                {
                    ((MessageHandler)dl).Invoke(message, ext);
                }
            }
        }
        public static void SendMessage(string message)
        {
            SendMessage(message, new Hashtable());
        }

        public static void SendError(string message)
        {
            SendError(message, new Hashtable());
        }

        public static void SendError(string message, Hashtable ext)
        {
            if (ErrorHandler != null)
            {
                foreach (Delegate dl in ErrorHandler.GetInvocationList())
                {
                    ((MessageHandler)dl).Invoke(message, ext);
                }
            }
        }

        public static void RegisterErrorHandler(MessageHandler handler)
        {
            if (ErrorHandler == null)
                ErrorHandler = handler;
            else
                ErrorHandler += handler;
        }

        public static void RegisterMessageHandler(MessageHandler handler)
        {
            if (MessageHandler == null)
                MessageHandler = handler;
            else
                MessageHandler += handler;
        }
    }

    public delegate void MessageHandler(string message, Hashtable ext);

    public enum MessageType
    {
        NONE = 0,
        ADDSUCCESS = 1,
        EDITFAILURE = 2,
        ADDFAILURE = 15,
        EDITSUCCESS = 16,
        ERROR = 99
    }
    public class Message
    {
        public MessageType Type;
        public StringBuilder Info = new StringBuilder();

        public void AppendInfo(MessageType type, string info)
        {
            if (type > Type)
            {
                Type = type;
                Info.Remove(0, Info.Length);
                Info.Append(info);
            }
            else
            {
                Info.Append(info);
            }

        }
    }
}
