using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS;

namespace We7.Plugin.SiteGroupPlugin.InfoSharing
{
    public partial class InfoSharingConfig : BasePage
    {
        protected override bool NeedAnAccount
        {
            get { return false; }
        }

        public string TabID
        {
            get { return Request["tab"]; }
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
            string strActive = @"<li class='TabIn' id='tab{0}' style='display:{2}'><a>{1}</a> </li>";
            string strLink = @"<li class='TabOut' id='tab{0}'  style='display:{2}'><a  href='{3}'>{1}</a> </li>";

            int tab = 1;
            string tabString = "";
            string dispay = "";
            string rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "tab");
            rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "saved");

            if (TabID != null && We7Helper.IsNumber(TabID))
                tab = int.Parse(TabID);

            if (tab == 1)
            {
                tabString += string.Format(strActive, 1, "站点关联", dispay);
                Control ctl = this.LoadControl("Config_SiteSetting.ascx");
                ContentHolder.Controls.Add(ctl);
            }
            else
                tabString += string.Format(strLink, 1, "站点关联", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));


            return tabString;
        }

        /// <summary>
        /// 构建当前位置导航
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "<a href='/admin/' target='_parent' >控制台</a> > <a>共享</a> > <a href='/admin/InfoSharing/InfoSharingConfig.aspx/' target='_parent'>共享参数配置</a>";
            return pos;
        }
    }
}