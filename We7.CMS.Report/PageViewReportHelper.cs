using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Data;

using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using Thinkment.Data;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// 流量统计报表生成类
    /// </summary>
    [Serializable]
    [Helper("We7.PageViewReportHelper")]
    public class PageViewReportHelper : BaseHelper
    {
        HelperFactory HelperFactory
        {
            get
            {
                return (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
            }
        }

        ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// 按键值获取统计数字列表
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="keyValue"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<PVKeyCountView> GetKeyCountList(string keyName,string keyValue, DateTime begin, DateTime end)
        {
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            SqlStatement sql = new SqlStatement();            
            
            string sqlCommandTxt =
       @"SELECT  DISTINCT  [{1}],COUNT([{1}]) AS [pvcount] FROM [PageVisitorHistory]  {0} 
           GROUP BY [{1}] ORDER BY COUNT([{1}]) DESC ";
            string strWhere = " where 1=1  ";
            if (begin <= end)
            {
                if (begin != DateTime.MinValue)
                {
                    strWhere += "and  [VisitDate] > {0}BEGIN";
                    DataParameter dp = new DataParameter();
                    dp.ParameterName = db.DbDriver.Prefix + "BEGIN";
                    dp.DbType = DbType.DateTime;
                    dp.SourceColumn = "VisitDate";
                    dp.Value = begin;

                    sql.Parameters.Add(dp);
                }
                if (end != DateTime.MaxValue)
                {
                    strWhere += " and [VisitDate] < {0}END";
                    DataParameter dp2 = new DataParameter();
                    dp2.ParameterName = db.DbDriver.Prefix + "END";
                    dp2.Value = end.AddDays(1);
                    dp2.DbType = DbType.DateTime;
                    dp2.SourceColumn = "VisitDate";
                    sql.Parameters.Add(dp2);
                }
            }
            if (keyValue.Trim() != "")
            {
                strWhere += " and [{1}] like {0}KEY";
                DataParameter dp3 = new DataParameter();
                dp3.ParameterName = db.DbDriver.Prefix + "KEY";
                dp3.Value = "%" + keyValue.Trim() + "%";
                dp3.DbType = DbType.String;
                sql.Parameters.Add(dp3);
            }

            strWhere = string.Format(strWhere, db.DbDriver.Prefix, keyName);
            sqlCommandTxt = string.Format(sqlCommandTxt, strWhere, keyName);
            sql.SqlClause = sqlCommandTxt;

            db.DbDriver.FormatSQL(sql);
            List<PVKeyCountView> list = new List<PVKeyCountView>();
            using (IConnection conn = db.CreateConnection())
            {
                DataTable dt = conn.Query(sql);
                int total = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PVKeyCountView kv = new PVKeyCountView();
                        kv.KeyValue = dr[keyName].ToString();
                        kv.Count = Int32.Parse(dr["pvcount"].ToString());
                        total += kv.Count;
                        list.Add(kv);
                    }
                    foreach (PVKeyCountView v in list)
                    {
                        v.Percent = (double)v.Count / (double)total * 100;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取发表文章统计数据对象
        /// </summary>
        /// <returns></returns>
        public StatisticsArticle GetStatisticsArticleCount()
        {
            StatisticsArticle sa = new StatisticsArticle();
            //总文章数
            sa.TotalArticles = Assistant.Count<Article>(null);
            //总评论数
            sa.TotalComments = Assistant.Count<Comments>(null);
            //当前月发表文章数
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            DateTime thisMonth = Convert.ToDateTime(year.ToString() + "-" + month.ToString() + "-01");
            Criteria c = new Criteria(CriteriaType.MoreThanEquals, "Created", thisMonth);
            sa.MonthArticles = Assistant.Count<Article>(c);
            //当前月评论数量
            sa.MonthComments = Assistant.Count<Comments>(c);
            //当前周发表文章数
            DateTime currentTime = DateTime.Now;
            string currentWeek = currentTime.DayOfWeek.ToString();
            int dayCount = DayInWeek(currentWeek);
            DateTime thisWeek = Convert.ToDateTime(currentTime.ToShortDateString()).AddDays(-(dayCount - 1));
            c = new Criteria(CriteriaType.MoreThanEquals, "Created", thisWeek);
            sa.WeekArticles = Assistant.Count<Article>(c);
            //当前周评论总数
            sa.WeekComments = Assistant.Count<Comments>(c);
            return sa;
        }

        //当前是本周的第几天
        public int DayInWeek(string week)
        {
            int daycount = 0;
            switch (week)
            {
                case "Monday":
                    daycount = 1;
                    break;
                case "Tuesday":
                    daycount = 2;
                    break;
                case "Wednesday":
                    daycount = 3;
                    break;
                case "Thursday":
                    daycount = 4;
                    break;
                case "Friday":
                    daycount = 5;
                    break;
                case "Saturday":
                    daycount = 6;
                    break;
                case "Sunday":
                    daycount = 7;
                    break;
            }
            return daycount;

        }

        /// <summary>
        /// 获取月访问统计
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="orderKey"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<PVKeyCountView> GetStatisticsMonthsList
            (string channelID, DateTime begin, DateTime end, string orderKey, bool sort)
        {
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            SqlStatement sql = new SqlStatement();

            string sqlCommandTxt = @"SELECT DISTINCT MONTH([VisitDate]) AS MyMonth, COUNT( MONTH([VisitDate]) ) AS [pvcount] FROM [StatisticsHistory] {0} GROUP BY MONTH([VisitDate]) ORDER BY MONTH([VisitDate])";

            //根据条件组织追加相应的sql语句
            string strWhere = " where 1=1  ";
            if (begin > end)
            {
                begin = new DateTime(DateTime.Now.Year, 01, 01);
                end = new DateTime(DateTime.Now.Year, 01, 31);
            }
            if (begin != DateTime.MinValue)
            {
                strWhere += "and  [VisitDate] >= {0}BEGIN";
                DataParameter dp = new DataParameter();
                dp.ParameterName = db.DbDriver.Prefix + "BEGIN";
                dp.DbType = DbType.DateTime;
                dp.SourceColumn = "VisitDate";
                dp.Value = begin;
                sql.Parameters.Add(dp);
            }
            if (end != DateTime.MaxValue)
            {
                strWhere += " and [VisitDate] < {0}END";
                DataParameter dp2 = new DataParameter();
                dp2.ParameterName = db.DbDriver.Prefix + "END";
                dp2.Value = end.AddDays(1);
                dp2.DbType = DbType.DateTime;
                dp2.SourceColumn = "VisitDate";
                sql.Parameters.Add(dp2);
            }
            if (channelID.Trim() != "")
            {
                HttpContext Context = HttpContext.Current;
                string chFullUrl = ChannelHelper.GetFullUrl(channelID);
                if (chFullUrl != "")
                {
                    strWhere += " and Url like {0}Url";
                    DataParameter dp3 = new DataParameter();
                    dp3.ParameterName = db.DbDriver.Prefix + "Url";
                    dp3.Value = chFullUrl+'%';
                    dp3.DbType = DbType.String;
                    dp3.Size = 255;
                    sql.Parameters.Add(dp3);
                }
            }

            strWhere = string.Format(strWhere, db.DbDriver.Prefix);
            sqlCommandTxt = string.Format(sqlCommandTxt, strWhere);
            sql.SqlClause = sqlCommandTxt;

            sql = db.DbDriver.FormatSQL(sql);
            List<PVKeyCountView> list = new List<PVKeyCountView>();
            using (IConnection conn = db.CreateConnection())
            {
                DataTable dt = conn.Query(sql);
                int total = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    PVKeyCountView kv = new PVKeyCountView();
                    kv.KeyValue = dr["MyMonth"].ToString();
                    kv.Count = Int32.Parse(dr["pvcount"].ToString());
                    total += kv.Count;
                    list.Add(kv);
                }
                //判断月份是不是十二个月份
                if (list.Count != 12)
                {
                    int num = 1;
                    for (int i = 1; i <= 12; i++)
                    {
                        
                        PVKeyCountView pv = list.Find(delegate(PVKeyCountView view)
                        {
                            if (view.KeyValue != "0" + i.ToString() && view.KeyValue.IndexOf("0") == 0)
                                num = 0;
                            return view.KeyValue == i.ToString() || view.KeyValue == "0" + i.ToString();
                        });

                        //如果没有 i 这个月份，则进行追加
                        if (pv == null)
                        {
                            PVKeyCountView kv = new PVKeyCountView();
                            if (num == 0 && i.ToString().Length <2)
                                kv.KeyValue = "0" + i.ToString();
                            else
                                kv.KeyValue = i.ToString();
                            kv.Count = 0;
                            total += kv.Count;
                            list.Add(kv);
                        }
                    }
                }
                //合算比例
                foreach (PVKeyCountView v in list)
                {
                    if (v.Count != 0)
                        v.Percent = (double)v.Count / (double)total * 100;
                }
            }
            list.Sort(new DinoComparer(orderKey, sort));
            return list;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public class DinoComparer : IComparer<PVKeyCountView>
        {
            private bool desc;
            private string order;
            int i1;
            int i2;

            public DinoComparer(string orderKey,bool direct)
            {
                desc = direct;
                order = orderKey;
            }

            public int Compare(PVKeyCountView x, PVKeyCountView y)
            {
                if (order == "Month")
                {
                    i1 = int.Parse(x.KeyValue);
                    i2 = int.Parse(y.KeyValue);
                }
                if (order == "Count")
                {
                    i1 = x.Count;
                    i2 = y.Count;
                }
                if (desc)
                    return i1 - i2;//true 从小到大
                else
                    return i2 - i1;
            }
        }
    }
}
