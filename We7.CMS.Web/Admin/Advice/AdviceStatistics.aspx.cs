using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.PF;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceStatistics : BasePage
    {

        public string TabID
        {
            get { return Request["tab"]; }
        }

        public string RoleID
        {
            get { return Request["id"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MenuTabLabel.Text = BuildNavString();
            PagePathLiteral.Text = BuildPagePath();
        }

        /// <summary>
        /// 构建标签项，完全可以通过控制每步骤的display属性
        /// 进行功能项的控制，此属性完全可以进行如同模块式的管理
        /// 设计页面、Tab名称、Tab显示属性、Tab所加载控件
        /// 三者以控制其相应的显示
        /// </summary>
        /// <returns></returns>
        string BuildNavString()
        {
            string strActive = @"<LI class=TabIn id=tab{0} style='display:{2}'><A>{1}</A> </LI>";
            string strLink = @"<LI class=TabOut id=tab{0}  style='display:{2}'><A  href={3}>{1}</A> </LI>";
            int tab = 1;
            string tabString = "";
            string dispay = "";
            string rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "tab");
            rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "saved");

            if (TabID != null && We7Helper.IsNumber(TabID))
                tab = int.Parse(TabID);

            Role r = AccountHelper.GetRole(RoleID);

            if (tab == 1)
            {
                tabString += string.Format(strActive, 1, "回复统计", dispay);
                Control ctl = this.LoadControl("../Advice/controls/AdviceReplyStatisticsControl.ascx");
                ContentHolder.Controls.Add(ctl);
            }
            else
                tabString += string.Format(strLink, 1, "回复统计", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));
            return tabString;
        }

        /// <summary>
        /// 构建当前位置导航
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "开始 > <a >设置</a> >  <a href=\"../Advice/AdviceStatistics.aspx\" >反馈信息统计</a>";
            return pos;
        }
    }
}
