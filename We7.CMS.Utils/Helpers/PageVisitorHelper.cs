using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Web.Caching;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using Thinkment.Data;
using System.Threading;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// 访问业务类
    /// </summary>
    [Serializable]
    [Helper("We7.PageVisitorHelper")]
    public class PageVisitorHelper:BaseHelper
    {
        public static readonly string PageVisitorSessionKey = "We7.Session.PageVisitor.Key";
        public static readonly string OnlinePeopleApplicationKey = "We7.Application.OnlinePeople.Key";
        public const string VisiteCountCacheKey = "$WE7_VISITECOUNT";//访问缓存Key

        /// <summary>
        /// （类型）登录信息
        /// </summary>
        public const int TypeCode_User = 0;

        /// <summary>
        /// （类型）文章访问
        /// </summary>
        public const int TypeCode_Article = 1;

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PageVisitorHistory GetPageVisitor(string id)
        {
            PageVisitorHistory s = new PageVisitorHistory();
            Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", id);
            List<PageVisitorHistory> cList = Assistant.List<PageVisitorHistory>(cTmp, null);
            if (cList != null && cList.Count > 0)
            {
                s = cList[0];
            }
            return s;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public PageVisitorHistory GetPageVisitor(string id, string[] fields)
        {
            PageVisitorHistory s = new PageVisitorHistory();
            Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", id);
            List<PageVisitorHistory> cList = Assistant.List<PageVisitorHistory>(cTmp, null, 0, 0, fields);
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
        public List<PageVisitorHistory> GetPageVisitores()
        {
            return Assistant.List<PageVisitorHistory>(null, null);
        }

       
        /// <summary>
        /// 获取统计信息列表
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<PageVisitorHistory> GetPageVisitores(Criteria c)
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<PageVisitorHistory>(c,o);
        }

        /// <summary>
        /// 获取统计信息列表，分页
        /// </summary>
        /// <param name="c">查询条件</param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<PageVisitorHistory> GetPageVisitores(Criteria c, int from, int count)
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<PageVisitorHistory>(c, o, from, count);
        }

        /// <summary>
        /// 取得所有访问者：分页
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<PageVisitorHistory> GetPageVisitores(int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeCode", StatisticsHelper.TypeCode_User);
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<PageVisitorHistory>(c, o, from, count);
        }

        /// <summary>
        /// 取得访问者
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<PageVisitorHistory> GetPageVisitores(string key, DateTime begin, DateTime end, int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeCode", StatisticsHelper.TypeCode_User);
            if (key.Trim() != "") c.Add(CriteriaType.Equals, "Keyword", key.Trim());
            if (begin <= end)
            {
                if (begin != DateTime.MinValue)
                {
                    Criteria s1 = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", begin);
                    c.Criterias.Add(s1);
                }
                if (end != DateTime.MaxValue)
                {
                    Criteria s2 = new Criteria(CriteriaType.LessThanEquals, "VisitDate", end.AddDays(1));
                    c.Criterias.Add(s2);
                }
            }
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<PageVisitorHistory>(c, o, from, count);
        }
        
        /// <summary>
        /// 取得访问者数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int GetPageVisitorCount(string key, DateTime begin, DateTime end)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeCode", StatisticsHelper.TypeCode_User);
            if (key.Trim() != "") c.Add(CriteriaType.Equals, "Keyword", key.Trim());
            if (begin <= end)
            {
                if (begin != DateTime.MinValue)
                {
                    Criteria s1 = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", begin);
                    c.Criterias.Add(s1);
                }
                if (end != DateTime.MaxValue)
                {
                    Criteria s2 = new Criteria(CriteriaType.LessThanEquals, "VisitDate", end.AddDays(1));
                    c.Criterias.Add(s2);
                }
            }

            return Assistant.Count<PageVisitorHistory>(c);
        }

        /// <summary>
        /// 获取指定字段统计信息列表，分页
        /// </summary>
        /// <param name="c"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<PageVisitorHistory> GetPageVisitores(Criteria c, int from, int count,string[] fields)
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<PageVisitorHistory>(c, o, from, count,fields);
        }

        /// <summary>
        /// 获取统计信息数量
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int GetPageVisitorCount(Criteria c)
        {
            return Assistant.Count<PageVisitorHistory>(c);
        }

        /// <summary>
        /// 取得访问历史
        /// </summary>
        /// <returns></returns>
        public int GetPageVisitorCount()
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeCode", StatisticsHelper.TypeCode_User);
            return Assistant.Count<PageVisitorHistory>(c);
        }

        /// <summary>
        /// 更新访问信息
        /// </summary>
        /// <param name="pv"></param>
        /// <param name="fields"></param>
        public void UpdatePageVisitor(PageVisitorHistory pv, string[] fields)
        {
            Assistant.Update(pv, fields);
        }

        public void UpdatePageVisitor(PageVisitor pv, string[] fields)
        {
            Assistant.Update(pv, fields);
        }

        /// <summary>
        /// 访客离开，在session结束时调用
        /// </summary>
        public void PageVisitorLeave()
        {
            if (HttpContext.Current != null)
            {
                HttpSessionState Session = HttpContext.Current.Session;
                if (Session[PageVisitorHelper.PageVisitorSessionKey] != null)
                {
                    PageVisitor pv = (PageVisitor)Session[PageVisitorHelper.PageVisitorSessionKey];
                    pv.OnlineTime = DateTime.Now;
                    Assistant.Update(pv, new string[] { "OnlineTime" });
                }
            }
        }

        /// <summary>
        /// 新增一条统计信息
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        public void AddPageVisitor(PageVisitor s)
        {
            s.ID = We7Helper.CreateNewID();
            s.VisitDate = DateTime.Now;
            s.LogoutDate = DateTime.Now;
            s.OnlineTime = DateTime.Now;
            s.Updated = DateTime.Now;
            s.Created = DateTime.Now;
            Assistant.Insert(s);
        }

        /// <summary>
        /// 添加访问信息
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public PageVisitor AddPageVisitor(string AccountID)
        {
            HttpContext Context = HttpContext.Current;
            string ip = Context.Request.ServerVariables["REMOTE_ADDR"];
            PageVisitor s = GetPageVisitorByIP(ip);
            if (s != null)
            {
                s.OnlineTime = DateTime.Now;
                Assistant.Update(s, new string[] { "OnlineTime" });
                return s;
            }
            else
            {
                s = new PageVisitor();
                HttpBrowserCapabilities bc = Context.Request.Browser;
                s.TypeCode = StatisticsHelper.TypeCode_User;
                s.VisitorIP = ip;
                s.Url = Context.Request.RawUrl;
                s.HttpReferer = Context.Request.ServerVariables["HTTP_REFERER"];
                SearchEngine se = new SearchEngine();
                s.Keyword = se.SearchKey(s.HttpReferer);
                s.SearchEngine = se.EngineName;
                s.Platform = GetPlatformInfo(Context);
                s.Screen = bc.ScreenPixelsWidth.ToString() + "×" + bc.ScreenPixelsHeight.ToString();
                s.Browser = bc.Browser + bc.Version;
                s.FromSite = We7Helper.GetDomainFromUrl(s.HttpReferer);
                s.Clicks = 1;
                AddPageVisitor(s);
                return s;
            }
        }

        /// <summary>
        /// 独立IP:24小时内同一IP只保存一条记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        PageVisitor GetPageVisitorByIP(string ip)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "VisitorIP", ip);
            c.Add(CriteriaType.MoreThanEquals, "VisitDate", DateTime.Today);
            c.Add(CriteriaType.LessThanEquals, "VisitDate", DateTime.Today.AddDays(1));
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            List<PageVisitor> visitors= Assistant.List<PageVisitor>(c, o);
            if (visitors.Count > 0)
                return visitors[0];
            else
                return null;
        }

        string GetCityFromIP()
        {
            return "";
        }

        string GetPlatformInfo(HttpContext Context)
        {
            string strSysVersion = "其他";
            string strAgentInfo = Context.Request.ServerVariables["HTTP_USER_AGENT"];

            if (strAgentInfo.Contains("NT 5.2"))
            {
                strSysVersion = "Windows 2003";
            }
            else if (strAgentInfo.Contains("NT 5.1"))
            {
                strSysVersion = "Windows XP";
            }
            else if (strAgentInfo.Contains("NT 5"))
            {
                strSysVersion = "Windows 2000";
            }
            else if (strAgentInfo.Contains("NT 4.9"))
            {
                strSysVersion = "Windows ME";
            }
            else if (strAgentInfo.Contains("NT 4"))
            {
                strSysVersion = "Windows NT4";
            }
            else if (strAgentInfo.Contains("NT 98"))
            {
                strSysVersion = "Windows 98";
            }
            else if (strAgentInfo.Contains("NT 95"))
            {
                strSysVersion = "Windows 95";
            }
            else if (strSysVersion.ToLower().Contains("Mac"))
            {
                strSysVersion = "Mac";
            }
            else if (strSysVersion.ToLower().Contains("unix"))
            {
                strSysVersion = "UNIX";
            }
            else if (strSysVersion.ToLower().Contains("linux"))
            {
                strSysVersion = "Linux";
            }
            else if (strSysVersion.Contains("SunOS"))
            {
                strSysVersion = "SunOS";
            }

            return strSysVersion;
        }

        /// <summary>
        /// 删除一条统计信息
        /// </summary>
        /// <param name="id"></param>
        public void DeletePageVisitor(string id)
        {
            PageVisitor s = GetPageVisitor(id);
            if (s != null)
            {
                
                Assistant.Delete(s);
            }
        }

        /// <summary>
        /// 删除一组统计信息
        /// </summary>
        /// <param name="ids"></param>
        public void DeletePageVisitor(List<string> ids)
        {
            foreach (string id in ids)
            {
                DeletePageVisitor(id);
            }
        }

        /// <summary>
        /// 更新退出时间
        /// </summary>
        public void UpdateLogoutTime()
        {
            HttpContext Context = HttpContext.Current;
            if (Context.Session[PageVisitorHelper.PageVisitorSessionKey] != null)
            {
                PageVisitor pv = (PageVisitor)Context.Session[PageVisitorHelper.PageVisitorSessionKey];
                pv.LogoutDate = DateTime.Now;
                Assistant.Update(pv, new string[] { "LogoutDate" });
            }
        }

        /// <summary>
        /// 获取总体统计数据
        /// </summary>
        /// <returns></returns>
        public VisiteCount GetCurrentVisiteCount()
        {
            VisiteCount vc = AppCtx.Cache.RetrieveObject<VisiteCount>(VisiteCountCacheKey);
            if (vc != null && vc.CreateDate.Day != DateTime.Now.Day) //如果是第二天了就把前一天的记录给清除掉
            {
                vc = null;
                AppCtx.Cache.RemoveObject(VisiteCountCacheKey);                
            }
            if (vc == null)
            {
                DateTime oldest = GetOldestTime();
                if (oldest < DateTime.Today)
                {
                    //MigrateToHistory();
                    //FreshSumData(oldest);
                }
                vc = new VisiteCount();
                //在线人数
                HttpContext Context = HttpContext.Current;
                //vc.OnlineVisitors = (int)Context.Application[PageVisitorHelper.OnlinePeopleApplicationKey];
                Criteria c = new Criteria(CriteriaType.MoreThanEquals, "OnlineTime", DateTime.Now.AddMinutes(-15));
                vc.OnlineVisitors = Assistant.Count<PageVisitor>(c);

                //今天访问量
                vc.DayVisitors = Assistant.Count<PageVisitor>(null);
                vc.DayPageview = Assistant.Count<Statistics>(null);

                //总访问数
                vc.TotalVisitors = Assistant.Count<PageVisitorHistory>(null) + vc.DayVisitors;
                //总浏览量
                vc.TotalPageView = Assistant.Count<StatisticsHistory>(null) + vc.DayPageview;
                //今年访问量
                int year = DateTime.Now.Year;
                DateTime thisYear = Convert.ToDateTime(year.ToString() + "-01-01");
                c = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", thisYear);
                vc.YearVisitors = Assistant.Count<PageVisitorHistory>(c) + vc.DayVisitors;
                vc.YearPageview = Assistant.Count<StatisticsHistory>(c) + vc.DayPageview;
                //本月访问量
                int month = DateTime.Now.Month;
                DateTime thisMonth = Convert.ToDateTime(year.ToString() + "-" + month.ToString() + "-01");
                c = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", thisMonth);
                vc.MonthVisitors = Assistant.Count<PageVisitorHistory>(c) + vc.DayVisitors;
                vc.MonthPageview = Assistant.Count<StatisticsHistory>(c) + vc.DayPageview;

                //昨天访问量
                c = new Criteria(CriteriaType.LessThan, "VisitDate", DateTime.Today);
                Criteria subc = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", DateTime.Today.AddDays(-1));
                c.Criterias.Add(subc);
                vc.YestodayVisitors = Assistant.Count<PageVisitorHistory>(c);
                vc.YestodayPageview = Assistant.Count<StatisticsHistory>(c);

                //平均每天访问量
                Order[] o = new Order[] { new Order("VisitDate", OrderMode.Asc) };
                List<PageVisitorHistory> list = Assistant.List<PageVisitorHistory>(null, o, 0, 1);
                DateTime firstDay =list.Count>0?list[0].VisitDate:DateTime.Now;
                vc.StartDate = firstDay;
                int days = ((TimeSpan)(DateTime.Today - firstDay.Date)).Days + 1;
                
                if (days > 0)
                {
                    vc.AverageDayVisitors = vc.TotalVisitors / days;
                    vc.AverageDayPageview = vc.TotalPageView / days;
                }
                AppCtx.Cache.AddObject(VisiteCountCacheKey, vc,((int)CacheTime.Short)*1000);
            }
            return vc;
        }

        /// <summary>
        /// 取得某段时间内的访问信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public int GetVisitorCountByTime(DateTime startTime,DateTime endTime)
        {
            Criteria c = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", startTime);
            Criteria subc = new Criteria(CriteriaType.LessThanEquals, "VisitDate", endTime);
            c.Criterias.Add(subc);
            return  Assistant.Count<PageVisitorHistory>(c);
        }

        /// <summary>
        /// 取得某段时间内的点击信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public int GetPVCountByTime(DateTime startTime, DateTime endTime)
        {
            Criteria c = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", startTime);
            Criteria subc = new Criteria(CriteriaType.LessThanEquals, "VisitDate", endTime);
            c.Criterias.Add(subc);
            return Assistant.Count<StatisticsHistory>(c);
        }

        /// <summary>
        /// 获取最旧的记录时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetOldestTime()
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Asc) };
            List<PageVisitor> list = Assistant.List<PageVisitor>(null, o, 0, 1);
            if (list != null && list.Count > 0)
                return list[0].VisitDate;
            else
                return DateTime.Today;
        }

        /// <summary>
        /// 取得访问者的使用次数
        /// </summary>
        /// <param name="visitorId">访问者ID</param>
        /// <returns></returns>
         public int GetStatisticsCount(string visitorId)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "VisitorID", visitorId);
            return Assistant.Count<StatisticsHistory>(c);
        }

        /// <summary>
        /// 取得退出时间
        /// </summary>
        /// <param name="visitorId">访问者ID</param>
        /// <returns></returns>
         DateTime GetLogoutDate(string visitorId)
         {
             Criteria c = new Criteria(CriteriaType.Equals, "VisitorID", visitorId);
             Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
             List<StatisticsHistory> sh = Assistant.List<StatisticsHistory>(c, o);
             if (sh.Count > 0)
                 return sh[0].VisitDate;
             else
                 return DateTime.MinValue;
         }

        /// <summary>
        /// 更新冗余字段
        /// </summary>
         public int FreshSumData(DateTime startTime)
         {
             Criteria c = new Criteria(CriteriaType.MoreThanEquals, "VisitDate", startTime);
             List<PageVisitorHistory> list = GetPageVisitores(c);
             int count = 0;
             foreach (PageVisitorHistory s in list)
             {
                 bool needUpdate = false;
                 if (s.Clicks <= 1)
                 {
                     s.Clicks = GetStatisticsCount(s.ID);
                     if (s.Clicks > 1) needUpdate = true;
                 }
                 s.PageView = s.Clicks;

                 if (s.City == "" || s.City == null)
                 {
                     string[] parts = We7Helper.GetCityNameByIP(s.VisitorIP).Split(' ');
                     if (parts.Length > 0)
                         s.Province = parts[0];
                     if (parts.Length > 1)
                         s.City = parts[1];
                     needUpdate = true;
                 }
                 if (s.FromSite == "" || s.FromSite == null)
                 {
                     s.FromSite = We7Helper.GetDomainFromUrl(s.HttpReferer);
                     needUpdate = true;
                 }
                 if (s.VisitDate == s.LogoutDate)
                 {
                     s.LogoutDate = GetLogoutDate(s.ID);
                     s.OnlineTime = s.LogoutDate;
                     needUpdate = true;
                 }

                 if (needUpdate)
                 {
                     try
                     {
                         UpdatePageVisitor(s, new string[] { "City", "Province", "FromSite", "Clicks", "LogoutDate", "OnlineTime" });
                     }
                     finally
                     {
                         count += 1;
                     }
                 }
             }
             return count;
         }

        /// <summary>
        /// 将PageVisitor表的数据迁移到PageVisitorHistory
        /// </summary>
        public void MigrateToHistory()
        {
            Criteria c = new Criteria(CriteriaType.LessThan, "VisitDate", DateTime.Today);
            List<PageVisitor> list = Assistant.List<PageVisitor>(c, null);
            foreach (PageVisitor o in list)
            {

                PageVisitorHistory pvh = new PageVisitorHistory();
                pvh = CopyObjectPropertiesTo( (Object)o,(Object)pvh) as PageVisitorHistory;
                try
                {
                    Assistant.Insert(pvh, null);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            List<Statistics> list2 = Assistant.List<Statistics>(c, null);
            foreach (Statistics o in list2)
            {
                StatisticsHistory sh = new StatisticsHistory();
                sh = CopyObjectPropertiesTo((Object)o, (Object)sh) as StatisticsHistory;
                try
                {
                    Assistant.Insert(sh, null);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            Assistant.DeleteList<Statistics>(c);
            Assistant.DeleteList<PageVisitor>(c);
        }

        /// <summary>
        /// 复制对象的所有属性到另一个对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public Object CopyObjectPropertiesTo(Object source, Object target)
        {
            PropertyInfo[] pis = target.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if(pi.CanWrite)
                    pi.SetValue(target, pi.GetValue(source,null), null);
            }

            return target;
        }
    }
}
