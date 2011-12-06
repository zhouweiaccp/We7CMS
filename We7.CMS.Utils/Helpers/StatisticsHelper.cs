using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Thinkment.Data;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using System.Threading;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// 用户访问统计数据层
    /// </summary>
    [Serializable]
    [Helper("We7.StatisticsHelper")]
    public class StatisticsHelper:BaseHelper
    {
        /*功能：用户访问统计数据层
         *作者：张成明
         *日期：2007-8-27
         */

        /// <summary>
        /// （类型）登录信息
        /// </summary>
        public const int TypeCode_User = 0;

        /// <summary>
        /// （类型）文章访问
        /// </summary>
        public const int TypeCode_Article = 1;

        /// <summary>
        /// 取得统计信息
        /// </summary>
        /// <param name="id">统计ID</param>
        /// <returns></returns>
        public Statistics GetStatistics(string id)
        {
            Statistics s = new Statistics();
            s.ID = id;
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            int i=Assistant.Count<Statistics>(c);
            if (i > 0)
            {
                Assistant.Select(s);
                return s;
            }
            else
                return null;
        }

        /// <summary>
        /// 取得统计信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="fields">查询字段</param>
        /// <returns></returns>
        public Statistics GetStatistics(string id,string[] fields)
        {
            Statistics s = new Statistics();
            Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", id);
            List<Statistics> cList = Assistant.List<Statistics>(cTmp, null, 0, 0, fields);
            if (cList != null && cList.Count > 0)
            {
                s = cList[0];
            }
            return s;
        }

        /// <summary>
        /// 获取所有统计列表
        /// </summary>
        /// <returns></returns>
        public List<Statistics> GetStatisticses()
        {
            return Assistant.List<Statistics>(null, null);
        }

        /// <summary>
        /// 获取统计信息列表
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<StatisticsHistory> GetStatisticses(Criteria c)
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<StatisticsHistory>(c,o);
        }

        /// <summary>
        /// 获取统计信息列表，分页
        /// </summary>
        /// <param name="c"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<StatisticsHistory> GetStatisticses(Criteria c, int from, int count)
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<StatisticsHistory>(c, o, from, count);
        }

        /// <summary>
        /// 取得所有统计信息：分页
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<StatisticsHistory> GetArticleStatisticses(int from, int count)
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            Criteria c = new Criteria(CriteriaType.None);
            c.Criterias.Add(new Criteria(CriteriaType.Like, "ArticleID", "{%"));
            return Assistant.List<StatisticsHistory>(c, o, from, count);
        }

        /// <summary>
        /// 获取指定字段统计信息列表，分页
        /// </summary>
        /// <param name="c"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<StatisticsHistory> GetStatisticses(Criteria c, int from, int count,string[] fields)
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<StatisticsHistory>(c, o, from, count,fields);
        }

        /// <summary>
        /// 取得某一访问者的统计信息
        /// </summary>
        /// <param name="visitorid">访问者ID</param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<StatisticsHistory> GetStatisticses(string visitorid, int from, int count, string[] fields)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "VisitorID", visitorid);
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<StatisticsHistory>(c, o, from, count, fields);
        }

        /// <summary>
        /// 获取统计信息数量
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int GetStatisticsCount(Criteria c)
        {
            return Assistant.Count<Statistics>(c);
        }

        /// <summary>
        /// 取得文章的访问数
        /// </summary>
        /// <returns></returns>
        public int GetArticleStatisticsCount()
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Criterias.Add(new Criteria(CriteriaType.Like, "ArticleID","{%"));
            return Assistant.Count<StatisticsHistory>(c);
        }

        /// <summary>
        /// 取得某一篇文章的访问数
        /// </summary>
        /// <param name="articleid"></param>
        /// <returns></returns>
        public int GetArticleStatisticsCount(string articleid)
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Criterias.Add(new Criteria(CriteriaType.Equals, "ArticleID", articleid));
            return Assistant.Count<StatisticsHistory>(c);
        }

        /// <summary>
        /// 取得某一个访问者的访问数
        /// </summary>
        /// <param name="visitorId">访问者</param>
        /// <returns></returns>
        public int GetStatisticsCount(string visitorId)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "VisitorID", visitorId);
            return Assistant.Count<StatisticsHistory>(c);
        }

        /// <summary>
        /// 取得某一位访问者某段时间以来的访问数
        /// </summary>
        /// <param name="visitorId">访问者</param>
        /// <param name="startTime">开始时间</param>
        /// <returns></returns>
        public int GetStatisticsCount(string visitorId,DateTime startTime)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "VisitorID", visitorId);
            c.Add(CriteriaType.MoreThanEquals, "VisitDate", startTime);
            return Assistant.Count<StatisticsHistory>(c);
        }

        /// <summary>
        /// 新增一条统计信息
        /// </summary>
        /// <param name="s">统计信息</param>
        /// <param name="type"></param>
        public void AddStatistics(Statistics s)
        {
            s.ID = We7Helper.CreateNewID();
            s.VisitDate = DateTime.Now;
            Assistant.Insert(s);
        }

        /// <summary>
        /// 添加统计信息
        /// </summary>
        /// <param name="pv">点击量</param>
        /// <param name="ArticleID">文章ID</param>
        /// <param name="ColumnID">栏目ID</param>
        public void AddStatistics(PageVisitor pv, string ArticleID, string ColumnID)
        {
            HttpContext Context = HttpContext.Current;
            Statistics s = new Statistics();
            s.VisitorID = pv.ID;
            s.ArticleID = ArticleID;
            s.ChannelID = ColumnID;
            s.Url = Context.Request.RawUrl;
            AddStatistics(s);
        }
        /// <summary>
        /// 删除一条统计信息
        /// </summary>
        /// <param name="id"></param>
        public void DeleteStatistics(string id)
        {
            Statistics s = GetStatistics(id);
            if(s!=null)
                Assistant.Delete(s);
        }

        /// <summary>
        /// 删除一组统计信息
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteStatistics(List<string> ids)
        {
            foreach (string id in ids)
            {
                DeleteStatistics(id);
            }
        }

    }
}
