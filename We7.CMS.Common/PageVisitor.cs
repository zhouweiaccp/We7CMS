using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 功能：页面访问用户统计实体 theheim 2007-11-08
    /// </summary>
    [Serializable]
    public class PageVisitor
    {
        private string id;
        private int typeCode;
        private string userID;
        private string userName;
        private DateTime visitDate;
        private DateTime logoutDate;
        private string visitorIP;
        private string url;
        private string http_referer;
        private string searchEngine;
        private string keyword;
        private int clicks;
        private DateTime onlineTime;
        private string platform;
        private string browser;
        private string screen;
        private string city;
        private int pageView;
        private DateTime updated=DateTime.Now;
        private DateTime created=DateTime.Now;
        string fromSite;
        string province;

        public PageVisitor()
        { }

        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 类别：0为登录统计，1为文章统计
        /// </summary>
        public int TypeCode
        {
            get { return typeCode; }
            set { typeCode = value; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }


        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime VisitDate
        {
            get { return visitDate; }
            set { visitDate = value; }
        }

        /// <summary>
        /// 退出时间
        /// </summary>
        public DateTime LogoutDate
        {
            get { return logoutDate; }
            set { logoutDate = value; }
        }

        /// <summary>
        /// 访问者IP
        /// </summary>
        public string VisitorIP
        {
            get { return visitorIP; }
            set { visitorIP = value; }
        }

        /// <summary>
        /// 入口页面
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        /// <summary>
        /// 来源应用网址
        /// </summary>
        public string HttpReferer
        {
            get { return http_referer; }
            set { http_referer = value; }
        }

        /// <summary>
        /// 搜索引擎
        /// </summary>
        public string SearchEngine
        {
            get { return searchEngine; }
            set { searchEngine = value; }
        }

        /// <summary>
        /// 搜索引擎关键词
        /// </summary>
        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        /// <summary>
        /// 点击数
        /// </summary>
        public int Clicks
        {
            get { return clicks; }
            set { clicks = value; }
        }

        /// <summary>
        /// 在线时间
        /// </summary>
        public DateTime OnlineTime
        {
            get { return onlineTime; }
            set { onlineTime = value; }
        }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string Platform
        {
            get { return platform; }
            set { platform = value; }
        }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser
        {
            get { return browser; }
            set { browser = value; }
        }

        /// <summary>
        /// 屏幕分辨率
        /// </summary>
        public string Screen
        {
            get { return screen; }
            set { screen = value; }
        }
        /// <summary>
        /// 省份
        /// </summary>
        /// <summary>
        /// 省份
        /// </summary>
        public string Province
        {
            get { return province; }
            set { province = value; }
        }

        /// <summary>
        /// 城市
        /// </summary>
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        /// <summary>
        /// 浏览页数
        /// </summary>
        public int PageView
        {
            get { return pageView; }
            set { pageView = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// 来源主域名
        /// </summary>
        public string FromSite
        {
            get { return fromSite; }
            set { fromSite = value; }
        }

    }

    public class VisiteCount:ICloneable
    {
        private int totalPageView;
        private int totalVisitors;
        private int yearVisitors;
        private int monthVisitors;
        private int dayVisitors;
        private int yestodayVisitors;
        private int averageDayVisitors;
        private int yearPageview;
        private int monthPageview;
        private int dayPageview;
        private int yestodayPageview;
        private int averageDayPageview;
        private int onlineVisitors;
        private DateTime startDate = DateTime.Now;

        public VisiteCount() { }

        /// <summary>
        /// 
        /// </summary>
        public int TotalPageView
        {
            get { return totalPageView; }
            set { totalPageView = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TotalVisitors
        {
            get { return totalVisitors; }
            set { totalVisitors = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int YearVisitors
        {
            get { return yearVisitors; }
            set { yearVisitors = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MonthVisitors
        {
            get { return monthVisitors; }
            set { monthVisitors = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int DayVisitors
        {
            get { return dayVisitors; }
            set { dayVisitors = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int YestodayVisitors
        {
            get { return yestodayVisitors; }
            set { yestodayVisitors = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AverageDayVisitors
        {
            get { return averageDayVisitors; }
            set { averageDayVisitors = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int YearPageview
        {
            get { return yearPageview; }
            set { yearPageview = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MonthPageview
        {
            get { return monthPageview; }
            set { monthPageview = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DayPageview
        {
            get { return dayPageview; }
            set { dayPageview = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int YestodayPageview
        {
            get { return yestodayPageview; }
            set { yestodayPageview = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AverageDayPageview
        {
            get { return averageDayPageview; }
            set { averageDayPageview = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OnlineVisitors
        {
            get { return onlineVisitors; }
            set { onlineVisitors = value; }
        }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        /// <summary>
        /// 当前数据创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }


        #region ICloneable 成员

        object ICloneable.Clone() //所有字段皆为值类型，所以进行浅复制
        {
            return this.MemberwiseClone();
        }

        public VisiteCount Clone() //所有字段皆为值类型，所以进行浅复制
        {
            return this.MemberwiseClone() as VisiteCount;
        }

        #endregion
    }

    public class PageVisitorHistory : PageVisitor
    {
        public PageVisitorHistory() { }
    }
}
