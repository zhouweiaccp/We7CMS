using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.tools.widget
{
    public partial class SiteProfile : BaseUserControl
    {
        PageViewReportHelper PageViewReportHelper
        {
            get { return HelperFactory.GetHelper<PageViewReportHelper>(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCount();
            }
        }

        private void BindCount()
        {
            VisiteCount vc = PageVisitorHelper.GetCurrentVisiteCount();
            LabelTotalVisitors.Text = vc.TotalPageView.ToString();
            TodayPVLabel.Text = vc.DayPageview.ToString();

            StatisticsArticle sa = PageViewReportHelper.GetStatisticsArticleCount();
            LabelTotalArticles.Text = sa.TotalArticles.ToString();
            LabelTotalComments.Text = sa.TotalComments.ToString();
            LabelMonthArticles.Text = sa.MonthArticles.ToString();
            LabelMonthComments.Text = sa.MonthComments.ToString();
            LabelWeekArticles.Text = sa.WeekArticles.ToString();
            LabelWeekComments.Text = sa.WeekComments.ToString();
        }
    }
}