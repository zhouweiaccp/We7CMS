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
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;
using We7.CMS.Web.Admin.Permissions;
using We7.CMS.Web.Admin;
using We7.CMS.Accounts;

namespace We7.CMS.Web.User
{
    public partial class AccountEdit : UserBasePage
    {
        protected override bool NeedAnAccount
        {
            get { return false; }
        }

        public string TabID
        {
            get { return Request["tab"]; }
        }

        public string ActID
        {
            //get { return Request["id"]; }
            get { return Security.CurrentAccountID; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            MenuTabLabel.Text = BuildNavString();
            //PagePathLiteral.Text = BuildPagePath();
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

            if (tab == 1)
            {
                tabString += string.Format(strActive, 1, "基本信息", dispay);
                Control ctl = this.LoadControl("~/User/Controls/Account_Basic.ascx");
                ContentHolder.Controls.Add(ctl);
            }
            else
                tabString += string.Format(strLink, 1, "基本信息", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));

            if (ActID != null)
            {
                Account acc = AccountHelper.GetAccount(ActID, new string[] { "LoginName", "UserType" });
                string actName = acc.LoginName;

                if (acc != null)
                    dispay = "";
                else
                    dispay = "none";

                if (tab == 4)
                {
                    tabString += string.Format(strActive, 4, "选项", dispay);
                    Control ctl = this.LoadControl("~/User/Controls/Account_Extent.ascx");
                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 4, "选项", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "4"));
                if (tab == 5)
                {
                    tabString += string.Format(strActive, 5, "密码设置", dispay);
                    Control ctl = this.LoadControl("~/User/Controls/Account_Password.ascx");
                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 5, "密码设置", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "5"));
            }

            return tabString;
        }

        /// <summary>
        /// 构建当前位置导航
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "";

            if (ActID != null)
            {
                string actName = AccountHelper.GetAccount(ActID, new string[] { "LoginName" }).LoginName;

                if (actName != string.Empty)
                {
                    pos = "<a href='/admin/'>控制台</a> > <a >用户</a> >  <a href='../Departments.aspx'>用户与部门管理</a> >  <a>编辑用户<b>"
                        + actName + "</b> </a>";
                }
            }
            else
            {
                pos = "<a href='/admin/'>控制台</a> > <a >用户</a> >  <a href='../Departments.aspx'>用户与部门管理</a> >  <a>创建新用户</a>";
            }

            return pos;

        }
    }
}