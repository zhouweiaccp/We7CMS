using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Web.Caching;
using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web.UI;
using System.Collections.Specialized;
using System.Collections;
using System.Web.SessionState;
using We7.Framework;

namespace We7.Framework
{
    /// <summary>
    /// 执行Action的基础类
    /// </summary>
    public abstract class BaseAction :IHttpHandler, IRequiresSessionState  
    {
        #region 初始化基础类型

        /// <summary>
        /// Action对象的实例的关键字
        /// </summary>
        public const string ActionInstance = "_________Redirect.Action.Instance";

        static readonly Dictionary<Type, IRequestParser> Parser = new Dictionary<Type, IRequestParser>();

        static BaseAction()
        {
            Parser.Add(typeof(string), new RequestParser<string>());
            Parser.Add(typeof(bool), new RequestParser<bool>());
            Parser.Add(typeof(Int16), new RequestParser<Int16>());
            Parser.Add(typeof(Int32), new RequestParser<Int32>());
            Parser.Add(typeof(Int64), new RequestParser<Int64>());
            Parser.Add(typeof(Single), new RequestParser<Single>());
            Parser.Add(typeof(Double), new RequestParser<Double>());
            Parser.Add(typeof(Decimal), new RequestParser<Decimal>());
        }

        #endregion

        #region IHttpHandler 成员

        protected HttpRequest Request;
        protected HttpContext Context;
        protected HttpSessionState Session;
        protected HttpResponse Response;

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                InitFields(context);
                InitProperties();
                Execute();
                Success = true;
            }
            catch (Exception ex)
            {
                Message = String.IsNullOrEmpty(Message) ? ex.Message : Message;
            }
            Dispatcher();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 当前的ActionID
        /// </summary>
        public string _ActionID { get; set; }

        /// <summary>
        /// 当前Action的信息
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        public string ShowMessage 
        {
            get
            {
                return String.IsNullOrEmpty(Message) ? "display:none" : "";
            }
        }

        /// <summary>
        /// 业务助手工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID]; }
        }

        #endregion

        #region 抽象方法

        /// <summary>
        /// 执行Action
        /// </summary>
        public abstract void Execute();

        #endregion

        #region 静态方法

        /// <summary>
        /// 取得当前的Action
        /// </summary>
        /// <returns></returns>
        public static object GetAction(string actionID)
        {
            string key = ActionInstance + actionID;
            object o=HttpContext.Current.Items[key];
            if (o == null)
            {
                o = HttpContext.Current.Session[key];
                HttpContext.Current.Items[key] = o;
                HttpContext.Current.Session.Remove(key);
            }
            return o;
        }

        /// <summary>
        /// 取得当前的Action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAction<T>(string actionID)
        {
            T t = default(T);
            try
            {
                object o = GetAction(actionID);
                if (o != null)
                {
                    t = (T)o;
                }
            }
            catch
            {
            }
            return t;
        }

        /// <summary>
        /// 取得当前值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key,string actionID)
        {
            object o = GetAction(actionID);
            if (o != null)
            {
                Type t = o.GetType();
                PropertyInfo pt = t.GetProperty(key);
                if (pt != null)
                {
                    return pt.GetValue(o, null);
                }
            }
            return null;
        }

        #endregion

        #region 私有方法

        void Dispatcher()
        {
            if (Session["$_ActionID"] != null && _ActionID == null)
                _ActionID = Session["$_ActionID"].ToString();
            string key=ActionInstance + _ActionID;
            //Cache[key] = this;
            Session[key] = this;
            if (Session["$ActionFrom"] != null)
                Response.Redirect(Session["$ActionFrom"].ToString());
            else
                Response.Redirect(Request.UrlReferrer.PathAndQuery);
        }

        /// <summary>
        /// 根据Request中的数据初始化类中的属性
        /// </summary>
        void InitProperties()
        {
            Type type = this.GetType();
            PropertyInfo[] info = type.GetProperties();
            foreach (PropertyInfo p in info)
            {
                if (Request[p.Name] != null)
                {
                    p.SetValue(this, GetValue(Request, p), null);
                }
            }
        }

        void InitFields(HttpContext context)
        {
            Request = context.Request;
            Context = context;
            Session = context.Session;
            Response = context.Response;
        }

        /// <summary>
        /// 取得数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        object GetValue(HttpRequest request, PropertyInfo p)
        {
            object o = null;

            Type t = p.PropertyType;
            string name = p.Name;
            if (t.IsGenericType)
            {
                t = t.GetGenericArguments()[0];
                o = Parser[t].GetValue(GetStringValues(request, name));
            }
            else
            {
                o = Parser[t].GetValue(request[name]);
            }
            return o;
        }

        /// <summary>
        /// 取得集合数据
        /// </summary>
        /// <param name="r"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        List<string> GetStringValues(HttpRequest r, string name)
        {
            List<string> list = new List<string>();
            if (r.Form.GetValues(name) != null)
            {
                list.AddRange(r.Form.GetValues(name));
            }
            if (r.QueryString.GetValues(name) != null)
            {
                list.AddRange(r.QueryString.GetValues(name));
            }
            if (r.Headers.GetValues(name) != null)
            {
                list.AddRange(r.Headers.GetValues(name));
            }
            return list;
        }

        #endregion


        #region 数据解析

        interface IRequestParser
        {
            object GetValue(string s);

            object GetValue(List<string> s);
        }

        class RequestParser<T> : IRequestParser
        {
            /// <summary>
            /// 取得单个值
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            object IRequestParser.GetValue(string s)
            {
                if (typeof(T).Equals(typeof(string)))
                    return s;
                return TryParse(s);
            }

            /// <summary>
            /// 取得列表数据
            /// </summary>
            /// <param name="l"></param>
            /// <returns></returns>
            object IRequestParser.GetValue(List<string> l)
            {
                if (typeof(T).Equals(typeof(string)))
                    return l;
                return TryParse(l);
            }

            /// <summary>
            /// 解析字符串为基础数据
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="s"></param>
            /// <returns></returns>
            T TryParse(string s)
            {
                Type t = typeof(T);
                MethodInfo m = t.GetMethod("TryParse", new Type[] { typeof(string), typeof(T).MakeByRefType() });
                if (m != null)
                {
                    object[] args = new object[] { s, default(T) };
                    m.Invoke(null, args);
                    return (T)args[1];
                }
                return default(T);
            }

            /// <summary>
            /// 解析字符串列表为基础数据列表
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="l"></param>
            /// <returns></returns>
            List<T> TryParse(List<string> l)
            {
                List<T> result = new List<T>();
                foreach (string s in l)
                {
                    result.Add(TryParse(s));
                }
                return result;
            }
        }

        #endregion
}

    public interface IListAction
    {
        int RecordCount { get; }

        int PageIndex { get; set; }

        int StartIndex { get; }

        int EndIndex { get; }

        int ItemsCount { get;}

        int PageCount { get; }

        IList Records { get; set; }
    }

    /// <summary>
    /// 针对列表数据的Action
    /// </summary>
    public abstract class ListAction<T> : BaseAction,IListAction
    {
        /// <summary>
        /// 记录的条数
        /// </summary>
        public abstract int RecordCount { get;}

        /// <summary>
        /// 当前的页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 开始页
        /// </summary>
        public virtual int StartIndex
        {
            get
            {
               PageIndex = PageIndex < 1 ? 1:PageIndex;
               return (PageIndex-1)* PageSize;
            }
            set { StartIndex = value; }
        }

        /// <summary>
        /// 结束页
        /// </summary>
        public int EndIndex 
        {
            get
            {
                return StartIndex + ItemsCount;
            }
        }

        /// <summary>
        /// 每一页的记录数
        /// </summary>
        public int ItemsCount
        {
            get
            {
                int endIndex = StartIndex + PageSize;
                int pageCount = 0;
                if (endIndex > RecordCount)
                    pageCount = RecordCount - StartIndex;
                else
                    pageCount = pageSize;
                return pageCount;
            }
        }

        /// <summary>
        /// 总共有多少页
        /// </summary>
        public int PageCount
        {
            get
            {
                int count = RecordCount / PageSize;
                if (RecordCount % PageSize != 0)
                    count++;
                return count;
            }
        }

        private int pageSize=10;
        /// <summary>
        /// 页记录
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private IList records = new List<T>();
        /// <summary>
        /// 取得所有记录
        /// </summary>
        /// <returns></returns>
        public IList Records
        {
            get { return records; }
            set { records = value; }
        }
    }
}
