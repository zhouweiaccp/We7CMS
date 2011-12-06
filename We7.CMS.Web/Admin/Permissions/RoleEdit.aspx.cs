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
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin
{
    public partial class RoleEdit : BasePage
    {
        protected override bool NeedAnAccount
        {
            get { return false; }
        }

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
                tabString += string.Format(strActive, 1, "基本信息", dispay);
                Control ctl = this.LoadControl("../Permissions/Role_Basic.ascx");
                ContentHolder.Controls.Add(ctl);
            }
            else
                tabString += string.Format(strLink, 1, "基本信息", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));

            if (RoleID != null)
            {
                if (r != null)
                    dispay = "";
                else
                    dispay = "none";

                if (tab == 2)
                {
                    tabString += string.Format(strActive, 2, "所属用户", dispay);
                    Control ctl = this.LoadControl("../Permissions/Role_Accounts.ascx");
                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 2, "所属用户", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "2"));

                if (tab == 3)
                {
                    tabString += string.Format(strActive, 3, "模块权限", dispay);

                    //向动态控件传参
                    Permission_Func ctl = this.LoadControl("../Permissions/Permission_Func.ascx") as Permission_Func;
                    ctl.OwnerType = "role";
                    ctl.OwnerID = r.ID;

                    if ((OwnerRank)r.RoleType == OwnerRank.Admin)
                    {
                        ctl.ObjectID = "System.Administration";
                        ctl.EntityID = "System.Administration";
                    }
                    else
                    {
                        ctl.ObjectID = "System.User";
                        ctl.EntityID = "System.User";
                    }

                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 3, "模块权限", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "3"));


            //    dispay = "";

                //if (tab == 4)
                //{
                //    tabString += string.Format(strActive, 4, "功能权限", dispay);

                //    //向动态控件传参
                //    Permission_Func ctl = this.LoadControl("../Permissions/Permission_Func.ascx") as Permission_Func;
                //    ctl.OwnerType = "role";
                //    ctl.OwnerID = r.ID;
                //    ctl.ObjectID = Helper.EmptyGUID;
                //    ctl.EntityID = "System.Function";
                //    ContentHolder.Controls.Add(ctl);
                //}
                //else
                //    tabString += string.Format(strLink, 4, "功能权限", dispay, Helper.AddParamToUrl(rawurl, "tab", "4"));

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

            if (RoleID != null)
            {
                Role r = AccountHelper.GetRole(RoleID);

                if (r != null)
                {
                    pos = "开始 > <a >站点管理</a> >  <a href=\"../Roles.aspx\" >角色管理</a> >  <a>编辑角色【"
                        + r.Name + "】</a>";
                }
            }
            else
            {
                pos = "开始 > <a >站点管理</a> >  <a href=\"../Roles.aspx\" >角色管理</a> >  <a>创建新角色</a>";
            }

            return pos;

        }
    }
}