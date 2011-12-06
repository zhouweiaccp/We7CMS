using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.IO;
using System.Text;

using OpenFlashChart;
using AreaLine=OpenFlashChart.AreaLine;
using Legend=OpenFlashChart.Legend;

namespace We7.CMS.Web.Admin
{
    public partial class publish_data : BasePage
    {
        protected override bool NeedAnAccount
        {
            get
            {
                return false;
            }
        }

        public DateTime BeginDate
        {
            get
            {
                string begin = Request["begin"];
                if (begin == null || begin == "")
                    return DateTime.Today.AddYears(-1);
                else
                    return DateTime.Parse(begin);
            }
        }

        public DateTime EndDate
        {
            get
            {
                string end = Request["end"];
                if (end == null || end == "")
                    return DateTime.Today;
                else
                    return DateTime.Parse(end);
            }
        }

        int Days
        {
            get
            {
                TimeSpan ts = EndDate - BeginDate;
                return ts.Days;
            }
        }

        int Steps
        {
            get
            {
                int num = Days / 5;
                if (num < 1)
                    return Days;
                else if (num < 2)
                    return 2;
                else
                    return num;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.CacheControl = "no-cache";
            Response.Write(BuildChart().ToPrettyString());
            Response.End();
        }

        OpenFlashChart.OpenFlashChart BuildChart()
        {
            OpenFlashChart.OpenFlashChart chart = new OpenFlashChart.OpenFlashChart();
            List<double> data1 = new List<double>();

            int maxDot = 0;
            for (double i = 0; i < Days; i++)
            {
                //data1.Add(rand.Next(30));
                DateTime startTime = BeginDate.AddDays(i);
                DateTime endTime = startTime.AddDays(1);
                int pv = ArticleHelper.GetArticleCountByTime(startTime, endTime);
                if (pv > maxDot) maxDot = pv;
                data1.Add(pv);
            }

            OpenFlashChart.Area area = new Area();
            area.Values = data1;
            area.HaloSize = 0;
            area.Width = 4;
            //area.DotSize = 2;
            area.FontSize = 12;
            area.DotStyleType.Tip = "#x_label#<br>发表数：#val#";
            //area.DotStyleType.Type = DotType.ANCHOR;
            area.DotStyleType.Type = DotType.DOT;
            area.DotStyleType.Colour = "#0077CC";

            area.Tooltip = "提示：#val#";
            area.Colour = "#0077CC";
            area.FillColor = "#E6F2FA";
            area.FillAlpha = .5;
            Animation animation = new Animation();
            animation.Cascade = 1;
            animation.Delay = 0.5;
            animation.Type = "pop-up";
            area.OnShowAnimation = animation;
            chart.AddElement(area);
            chart.Y_Legend = new Legend("");
            chart.Title = new Title("");
            chart.Tooltip = new ToolTip("#x_label#<br>发表数：#val#");
            chart.Tooltip.MouseStyle = ToolTipStyle.FOLLOW;
            chart.Tooltip.Shadow = false;
            chart.Tooltip.BackgroundColor = "#ffffff";
            chart.Tooltip.Rounded = 3;
            chart.Tooltip.Stroke = 2;
            chart.Tooltip.Colour = "#000000";
            chart.Tooltip.BodyStyle = "color: #000000; font-weight: normal; font-size: 11;";

            maxDot = (int)(maxDot * 1.3);
            chart.Y_Axis.SetRange(0, maxDot, maxDot/3);
            chart.X_Axis.GridColour = "#eeeeee";
            chart.Y_Axis.GridColour = "#eeeeee";
            chart.X_Axis.Colour = "#333333";
            chart.Y_Axis.Colour = "#333333";
            chart.Bgcolor = "#fbfbfb";

            List<string> data2 = new List<string>();
            for (int i = 0; i < Days; i++)
            {
                data2.Add(BeginDate.AddDays(i).ToString("yyyy年MM月dd日"));
            }

            XAxis x = new XAxis();
            x.GridColour = "#eeeeee";
            x.Stroke = 1;
            x.Steps = Steps;
            XAxisLabels xlabels = new XAxisLabels();
            xlabels.Steps = Steps;
            xlabels.Vertical = false;
            xlabels.SetLabels(data2);
            x.Labels = xlabels;
            chart.X_Axis = x;

            chart.Tooltip = new ToolTip("全局提示：#val#");
            chart.Tooltip.Shadow = true;
            chart.Tooltip.Colour = "#e43456";
            chart.Tooltip.MouseStyle = ToolTipStyle.CLOSEST;
            return chart;
        }
    }
}