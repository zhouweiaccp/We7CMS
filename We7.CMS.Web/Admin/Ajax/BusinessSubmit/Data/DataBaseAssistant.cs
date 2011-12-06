using System;
using System.Collections.Generic;
using System.Web;
using Thinkment.Data;
using System.Data;
using We7.Framework;

namespace We7.CMS.Web.Admin.Ajax.BusinessSubmit.Data
{
    /// <summary>
    /// 数据库处理类
    /// 功能:增、删、改
    ///      基于DataTable的查询
    ///      分页查询
    /// author:丁乐
    /// </summary>
    public class DataBaseAssistant : IDataBase
    {
        ObjectAssistant assistant;
        /// <summary>
        /// 当前Helper的数据访问对象
        /// </summary>
        public ObjectAssistant Assistant
        {
            get
            {
                if (assistant == null)
                {
                    assistant = HelperFactory.Instance.Assistant;
                }
                return assistant;
            }
            set { assistant = value; }
        }
        #region IDataBase接口实现
        /// <summary>
        /// 总数据库数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int Total(Criteria condition)
        {
            return Assistant.Count<TableInfo>(condition);
        }

        #region 查询List
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public List<T> GetDtByCondition<T>(Criteria condition)
        {
            object obj = Activator.CreateInstance(typeof(T));
            obj = Assistant.List<T>(condition, null);
            return (obj as List<T>);
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <param name="o">排序字段</param>
        /// <returns></returns>
        public List<T> GetDtByCondition<T>(Criteria condition, Order[] o)
        {
            object obj = Activator.CreateInstance(typeof(T));
            obj = Assistant.List<T>(condition, o);
            return (obj as List<T>);
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <param name="fildes">要返回的字段</param>
        /// <returns></returns>
        public List<T> GetDtByCondition<T>(Criteria condition, string[] fildes)
        {
            object obj = Activator.CreateInstance(typeof(T));
            obj = Assistant.List<T>(condition, null, 0, 0, fildes);
            return (obj as List<T>);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <param name="o">排序字段</param>
        /// <param name="fildes">要返回的字段</param>
        /// <returns></returns>
        public List<T> GetDtByCondition<T>(Criteria condition, Order[] o, string[] fildes)
        {
            object obj = Activator.CreateInstance(typeof(T));
            obj = Assistant.List<T>(condition, o, 0, 0, fildes);
            return (obj as List<T>);
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <param name="o">排序字段</param>
        /// <param name="form">页码</param>
        /// <param name="count">查询个数</param>
        /// <returns></returns>
        public List<T> GetDtByCondition<T>(Criteria condition, Order[] o, int form, int count)
        {
            object obj = Activator.CreateInstance(typeof(T));
            obj = Assistant.List<T>(condition, o, form, count);
            return (obj as List<T>);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">条件</param>
        /// <param name="o">排序数组</param>
        /// <param name="form">起始</param>
        /// <param name="count">要查询的个数</param>
        /// <param name="fildes">要显示的字段</param>
        /// <returns></returns>
        public List<T> GetDtByCondition<T>(Criteria condition, Order[] o, int form, int count, string[] fildes)
        {
            object obj = Activator.CreateInstance(typeof(T));
            obj = Assistant.List<T>(condition, o, form, count, fildes);
            return (obj as List<T>);
        } 
        #endregion

        #region 增删改
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int DeleteList<T>(Criteria condition)
        {
            return Assistant.DeleteList<T>(condition);
        }

        public int Update(object obj, string[] fields, Criteria condition)
        {
            return Assistant.Update(obj, fields, condition);
        }

        public int Update(object obj, string[] fields)
        {
            return Assistant.Update(obj, fields);
        }
        public int Update(object obj)
        {
            return Assistant.Update(obj);
        }
        public bool Delete(object obj)
        {
            return Assistant.Delete(obj);
        } 
        #endregion
        #endregion



       
    }
}