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

using Thinkment.Data;
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class Roles : BasePage
    {
        /// <summary>
        /// 当前过滤条件
        /// </summary>
        protected OwnerRank CurrentState
        {
            get
            {
                OwnerRank s = OwnerRank.All;
                if (Request["state"] != null)
                {
                    if (We7Helper.IsNumber(Request["state"].ToString()))
                        s =(OwnerRank)int.Parse(Request["state"].ToString());
                }
                return s;
            }
        }

        string Keyword
        {
            get
            {
                return Request["keyword"];
            }
        }

        protected override void Initialize()
        {
            DataBinds(CurrentState);
            StateLiteral.Text = BuildStateLinks();
        }

        private string siteID;
        protected string SiteID
        {
            get
            {
                if (string.IsNullOrEmpty(siteID))
                {
                   siteID= SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
                }
                return siteID;
            }
        }

        protected void DataBinds(OwnerRank state)
        {
            string siteID = SiteID;
            List<Role> roles = AccountHelper.GetRoles(siteID, state,Keyword);
            DataGridView.DataSource = roles;
            DataGridView.DataBind();
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//是否是演示站点

            string name = NameTextBox.Text;
            string id = IDTextBox.Text;

            if (We7Helper.IsNumber(id))
            {
                Messages.ShowError(name + "为系统角色，不允许删除！");
            }
            else
            {
                try
                {
                    AccountHelper.DeleteRole(id);

                    //记录日志
                    string content = string.Format("删除了角色:“{0}”", name);
                    AddLog("角色管理", content);
                    Initialize();
                }
                catch (Exception ex)
                {
                    string messages = "删除角色【" + name + "】出错！出错原因：" + ex.Message;
                    Messages.ShowError(messages);
                    Initialize();
                }
            }
        }

        /// <summary>
        /// 构建按类型/状态过滤的超级链接字符串
        /// </summary>
        /// <returns></returns>
        string BuildStateLinks()
        {
            string links = @"<li> <a href='Roles.aspx'   {0} >全部<span class=""count"">({1})</span></a> |</li>
            <li><a href='Roles.aspx?state=0'  {2} >管理员角色<span class=""count"">({3})</span></a> |</li>
            <li><a href='Roles.aspx?state=1'  {4} >普通用户角色<span class=""count"">({5})</span></a> </li>";

            string siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
            string css100, css0, css1, css2;
            css100 = css0 = css1 = css2 = "";
            if (CurrentState == OwnerRank.All) css100 = "class=\"current\"";
            if (CurrentState == OwnerRank.Admin) css0 = "class=\"current\"";
            if (CurrentState == OwnerRank.Normal) css1 = "class=\"current\"";
            links = string.Format(links, css100, AccountHelper.GetRoleCount(siteID,OwnerRank.All),
                css0, AccountHelper.GetRoleCount(siteID, OwnerRank.Admin), css1, AccountHelper.GetRoleCount(siteID,OwnerRank.Normal));

            return links;
        }

        /// <summary>
        /// 构建当前位置导航
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos ="开始 > <a >站点管理</a> >  <a href=\"Roles.aspx\" >角色管理</a>";
            return pos;
        }
    }
}
