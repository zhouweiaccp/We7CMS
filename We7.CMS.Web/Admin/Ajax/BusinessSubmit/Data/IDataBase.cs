using System;
using System.Collections.Generic;
using System.Web;
using Thinkment.Data;
using System.Data;

namespace We7.CMS.Web.Admin.Ajax.BusinessSubmit.Data
{
    /// <summary>
    /// 数据库处理接口
    /// 功能:增、删、改
    ///      基于DataTable的查询
    ///      分页查询
    /// author:丁乐
    /// </summary>
    public interface IDataBase
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        int Total(Criteria condition);
        /// <summary>
        /// 根据条件取得记录
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        List<T> GetDtByCondition<T>(Criteria condition);
        /// <summary>
        /// 根据条件取得记录
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="condition">条件</param>
        /// <param name="o">排序数组</param>
        /// <returns></returns>
        List<T> GetDtByCondition<T>(Criteria condition,Order[] o);
        /// <summary>
        /// 根据条件取得记录
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="fildes">定制字段信息(默认所有)</param>
        /// <returns></returns>
        List<T> GetDtByCondition<T>(Criteria condition, string[] fildes);
        /// <summary>
        /// 根据条件取得记录
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="o">排序</param>
        /// <param name="fildes">定制字段信息(默认所有)</param>
        /// <returns></returns>
        List<T> GetDtByCondition<T>(Criteria condition,Order[] o ,string[] fildes);
        /// <summary>
        /// 根据条件取得记录(分页)
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="o">排序</param>
        /// <param name="form">开始</param>
        /// <param name="count">记录数</param>
        /// <returns></returns>
        List<T> GetDtByCondition<T>(Criteria condition, Order[] o,int form, int count);
        /// <summary>
        /// 根据条件取得记录(分页)
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="o">排序</param>
        /// <param name="form">开始</param>
        /// <param name="count">记录数</param>
        /// <param name="fildes">定制字段信息(默认所有)</param>
        /// <returns></returns>
        List<T> GetDtByCondition<T>(Criteria condition,Order[] o, int form ,int count, string[] fildes);

        /// <summary>
        /// 根据条件删除一组数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        int DeleteList<T>(Criteria condition);

        /// <summary>
        /// 根据条件删除一组数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fields">需要更新的字段</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        int Update(object obj, string[] fields, Criteria condition);
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fields">有效字段</param>
        /// <returns></returns>
        int Update(object obj, string[] fields);
        /// <summary>
        /// 更新一条记录
        /// </summary>
        int Update(object obj);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Delete(object obj);
    }
}