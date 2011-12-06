using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using System.Web;
using We7.Framework;

namespace We7.CMS.WebControls
{
    public class VisiteCountProvider : BaseWebControl
    {
        private string cssClass;
        /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        /// <summary>
        /// 页面访问业务对象
        /// </summary>
        protected PageVisitorHelper PageVisitorHelper
        {
            get { return HelperFactory.GetHelper<PageVisitorHelper>(); }
        }

        protected VisiteCount Counter { get; set; }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Counter = new VisiteCount();
            VisiteCount conter = AppCtx.Cache.RetrieveObject<VisiteCount>(PageVisitorHelper.VisiteCountCacheKey);
            if (conter == null)
            {
                conter = PageVisitorHelper.GetCurrentVisiteCount();
            }
            if (conter != null)
            {
                Counter.TotalPageView = conter.TotalPageView + InitTotalPageView;
                Counter.TotalVisitors = conter.TotalVisitors + InitTotalVisitors;
                Counter.YearVisitors = conter.YearVisitors + InitYearVisitors;
                Counter.MonthVisitors = conter.MonthVisitors + InitMonthVisitors;
                Counter.DayVisitors = conter.DayVisitors + InitDayVisitors;
                Counter.YestodayVisitors = conter.YestodayVisitors + InitYestodayVisitors;
                Counter.AverageDayVisitors = conter.AverageDayVisitors + InitAverageDayVisitors;
                Counter.YearPageview = conter.YearPageview + InitYearPageview;
                Counter.MonthPageview = conter.MonthPageview + InitMonthPageview;
                Counter.DayPageview = conter.DayPageview + InitDayPageview;
                Counter.YestodayPageview = conter.YestodayPageview + InitYestodayPageview;
                Counter.AverageDayPageview = conter.AverageDayPageview + InitAverageDayPageview;
                Counter.OnlineVisitors = conter.OnlineVisitors + InitOnlineVisitors;
            }
        }

        public int InitTotalPageView { get; set; }

        public int InitTotalVisitors { get; set; }

        public int InitYearVisitors { get; set; }

        public int InitMonthVisitors { get; set; }

        public int InitDayVisitors { get; set; }

        public int InitYestodayVisitors { get; set; }

        public int InitAverageDayVisitors { get; set; }

        public int InitYearPageview { get; set; }

        public int InitMonthPageview { get; set; }

        public int InitDayPageview { get; set; }

        public int InitYestodayPageview { get; set; }

        public int InitAverageDayPageview { get; set; }

        public int InitOnlineVisitors { get; set; }

    }
}
