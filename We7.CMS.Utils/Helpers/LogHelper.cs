using System;
using System.Collections.Generic;
using System.Text;

using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using Thinkment.Data;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// 日志业务类
    /// </summary>
    [Serializable]
    [Helper("We7.LogHelper")]
    public class LogHelper:BaseHelper
    {
        /// <summary>
        /// 获取全部日志总数
        /// </summary>
        /// <returns></returns>
        public int QueryAllLogCount()
        {
            Order[] o = new Order[] { new Order("ID") };
            return Assistant.List<Log>(null, o).Count;
        }
        /// <summary>
        /// 根据条件获取日志总数
        /// </summary>
        /// <param name="c">Criteria</param>
        /// <returns></returns>
        public int QueryLogCount(Criteria c)
        {
            return Assistant.Count<Log>(c);
        }
        /// <summary>
        /// 得到日志
        /// </summary>
        /// <param name="logID">日志ID</param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public Log GetLog(string id, string[] fields)
        {
            Log l = new Log();
            Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", id);
            List<Log> cList = Assistant.List<Log>(cTmp, null, 0, 0, fields);
            if (cList != null && cList.Count > 0)
            {
                l = cList[0];
            }
            return l;
        }
        /// <summary>
        /// 根据条件得到日志列表
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<Log> GetLogs(Criteria c)
        {
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            List<Log> ts = Assistant.List<Log>(c, o);
            return ts;
        }
        /// <summary>
        /// 根据条件得到分页日志列表
        /// </summary>
        /// <param name="c"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Log> GetPagedLogs(Criteria c, int from, int count)
        {
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            return Assistant.List<Log>(c, o, from, count);
        }
        /// <summary>
        /// 得到全部日志列表
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Log> GetPagedAllLogs(int from, int count)
        {
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            return Assistant.List<Log>(null, o, from, count);
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="l">日志</param>
        public void AddLog(Log l)
        {
            l.Created = DateTime.Now;
            l.ID = We7Helper.CreateNewID();
            Assistant.Insert(l);
        }
        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="logID">日志ID</param>
        public void DeleteLog(string id)
        {
            Log l = new Log();
            l.ID = id;
            Assistant.Delete(l);
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="accountID">当前用户ID</param>
        /// <param name="page">当前页面</param>
        /// <param name="content">操作描述</param>
        public void WriteLog(string accountID,string page,string content,string remark)
        {
            Log l = new Log();
            l.UserID = accountID;
            l.Page = page;
            l.Content = content;
            l.Remark = remark;
            
            AddLog(l);
        }

        /// <summary>
        /// 查询日志记录数
        /// </summary>
        /// <param name="Author">用户</param>
        /// <param name="BeginDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <param name="Page">什么页面</param>
        /// <returns></returns>
        public int QueryLogCountByAll(string Author, DateTime BeginDate, DateTime EndDate,string Page)
        {
            Criteria c = CreateCriteriaByAll(Author, BeginDate,EndDate,Page);
            return Assistant.Count<Log>(c);
        }

        /// <summary>
        /// 查询日志记录
        /// </summary>
        /// <param name="Author">用户</param>
        /// <param name="BeginDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <param name="Page">什么页面</param>
        /// <param name="from">开始记录</param>
        /// <param name="count">查询条数</param>
        /// <param name="fields">查询字段</param>
        /// <param name="orderKey">排序关键字</param>
        /// <param name="up"></param>
        /// <returns></returns>
        public List<Log> QueryLogsByAll(string Author, DateTime BeginDate, DateTime EndDate,string Page, int from, int count, string[] fields, string orderKey,bool up)
        {
            Criteria c = CreateCriteriaByAll(Author,BeginDate,EndDate,Page);

            OrderMode mode = OrderMode.Asc;
            if (!up) mode = OrderMode.Desc;
            Order[] orders = new Order[] { new Order(orderKey, mode) };

            return Assistant.List<Log>(c, orders, from, count, fields);
        }

        /// <summary>
        /// 构造查询条件对象
        /// </summary>
        /// <param name="Author"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        Criteria CreateCriteriaByAll(string Author, DateTime BeginDate, DateTime EndDate, string Page)
        {
            Criteria c = new Criteria(CriteriaType.NotEquals,"ID","");

            if (Author != null && Author.Length > 0)
            {
                c.Add(CriteriaType.Like, "UserID", Author);
            }

            if (Page != null && Page.Length > 0)
            {
                c.Add(CriteriaType.Like, "Page", "%" + Author + "%");
            }

            if (BeginDate <= EndDate)
            {
                if (BeginDate.ToString() != DateTime.MinValue.ToString())
                    c.Add(CriteriaType.MoreThanEquals, "Created", BeginDate);
                if (EndDate.ToString() != DateTime.MaxValue.ToString())
                    c.Add(CriteriaType.LessThanEquals, "Created", EndDate.AddDays(1));
            }
            else
            {
                if (EndDate.ToString() != DateTime.MinValue.ToString())
                    c.Add(CriteriaType.MoreThanEquals, "Created", EndDate);
                if (BeginDate.ToString() != DateTime.MaxValue.ToString())
                    c.Add(CriteriaType.LessThanEquals, "Created", BeginDate.AddDays(1));
            }

            return c;
        }
    }
}
