using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using We7.CMS.Controls;
using Thinkment.Data;
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;
using We7.Framework.Cache;
using We7.CMS.Accounts;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class Departments : BasePage
    {

        string departmentID;
        string DepartmentID
        {
            get
            {
                string id;

                id = Request["id"];
                if (id == null)
                {
                    id = We7Helper.EmptyGUID;
                }
                return id;
            }
            set
            {
                departmentID = value;
            }
        }

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

        private int _resultsPageNumber = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageNumber
        {
            get 
            {
                if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
                    _resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
                return _resultsPageNumber; 
            }
        }

        protected override void Initialize()
        {
            RefreshHyperLink.NavigateUrl = String.Format("Departments.aspx?id={0}", DepartmentID);
            NewDepartmentHyperLink.NavigateUrl = String.Format("DepartmentDetail.aspx?pid={0}", DepartmentID);
            NewUserHyperLink.NavigateUrl = String.Format("Permissions/AccountEdit.aspx?d={0}", DepartmentID);

            FullPathLabel.Text = BuildPagePath();
            //StateLiteral.Text = BuildStateLinks();
            BindData();
        }

        void BindData()
        {
            List<ViewItem> items = GetItems();
            if (items != null)
            {
                UPager.PageIndex = PageNumber;
                UPager.ItemCount = items.Count;
                UPager.UrlFormat = We7Helper.AddParamToUrl(Request.RawUrl, Keys.QRYSTR_PAGEINDEX, "{0}");
                UPager.PrefixText = "共 " + UPager.MaxPages + "  页 ·   第 " + UPager.PageIndex + "  页 · ";

                if (UPager.ItemCount <=0)
                {
                    DepartmentsGridView.DataSource = null;
                    DepartmentsGridView.DataBind();
                    return;
                }
                else
                {
                    DepartmentsGridView.DataSource = items.GetRange(UPager.Begin - 1, UPager.Count);
                    DepartmentsGridView.DataBind();
                }
            }
        }


        List<ViewItem> GetItems( )
        {
            List<ViewItem> items = new List<ViewItem>();
            List<Department> dts = null;
            List<Account> acts = null;
            string siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
                if (CurrentState == OwnerRank.All)
                    dts = AccountHelper.GetDepartments(siteID, DepartmentID, Keyword, new string[] { "ID", "Name", "Description", "State" });


            if (dts != null)
            {
                foreach (Department dt in dts)
                {
                    ViewItem vi = new ViewItem();
                    vi.Text = dt.Name;
                    vi.Summary = dt.Description;
                    vi.Mode = "Department";
                    vi.ID = dt.ID;
                    vi.Url = String.Format("Departments.aspx?id={0}", dt.ID);
                    vi.DeleteUrl = String.Format("javascript:DeleteConfirm('{0}','{1}','Department');", dt.ID, dt.Name);
                    vi.EditUrl = String.Format("DepartmentDetail.aspx?id={0}", dt.ID);
                    items.Add(vi);
                }
            }

            return items;
        }


        protected void DeleteDepartmentButton_Click(object sender, EventArgs e)
        {           
            try
            {
                if (DemoSiteMessage) return;//是否是演示站点
                string dptID = IDTextBox.Text;
                string dptName = NameTextBox.Text;

                Department dpt = AccountHelper.GetDepartment(dptID, new string[] { "ParentID" });
                AccountHelper.DeleteDepartment(dptID);
                CacheRecord.Create(typeof(AccountLocalHelper)).Release();
                string content = string.Format("删除部门“{0}”", dptName);
                AddLog("删除部门", content);


                Response.Redirect(String.Format("Departments.aspx?id={0}", dpt.ParentID));
            }
            catch (Exception ex)
            {
                Messages.ShowMessage("删除部门出错！出错原因：" + ex.Message);
            }
        }

        protected void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (DemoSiteMessage) return;//是否是演示站点
                string actID = IDTextBox.Text;
                string actName = NameTextBox.Text;

                AccountHelper.DeleteAccont(actID);

                string content = string.Format("删除帐户“{0}”", actName);
                AddLog("删除帐户", content);

                Response.Redirect(String.Format("Departments.aspx?id={0}", DepartmentID));
            }
            catch (Exception ex)
            {
                Messages.ShowMessage("删除用户出错！出错原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 构建按类型/状态过滤的超级链接字符串
        /// </summary>
        /// <returns></returns>
        string BuildStateLinks()
        {
            string links = @"<li> <a href='Departments.aspx?state=100'   {0} >全部用户<span class=""count"">({1})</span></a> |</li>
            <li><a href='Departments.aspx?state=1'  {2} >普通用户<span class=""count"">({3})</span></a> |</li>
            <li><a href='Departments.aspx?state=0'  {4}  >管理员用户<span class=""count"">({5})</span></a></li>
";
            string css100, css0, css1, css2;//, css3 css3 =
            css100 = css0 = css1 = css2 = "";
            if (CurrentState == OwnerRank.All) css100 = "class=\"current\"";
            if (CurrentState == OwnerRank.Normal) css0 = "class=\"current\"";
            if (CurrentState == OwnerRank.Admin) css1 = "class=\"current\"";

            AccountQuery aq = new AccountQuery();
            aq.UserType =(int)OwnerRank.All;
            int count = AccountHelper.QueryAccountCountByQuery(aq);
            aq.UserType = (int)OwnerRank.Normal;
            int count0 = AccountHelper.QueryAccountCountByQuery(aq);
            aq.UserType = (int)OwnerRank.Admin;
            int count1 = AccountHelper.QueryAccountCountByQuery(aq);

            links = string.Format(links, css100, count, css0, count0, css1, count1);

            return links;
        }

        /// <summary>
        /// 构建当前位置导航
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "<a href='/admin/'>控制台</a> > <a >用户</a> >  <a href='Departments.aspx'>部门管理</a>  {0} ";
            return string.Format(pos, DepartMappath(DepartmentID));
        }

        string DepartMappath(string departID)
        {
            if (We7Helper.IsEmptyID(departID))
            {
                return "";
            }
            else
            {
                string navString = "<a href='Departments.aspx?id={0}'>部门 <b>{1}</b></a>";
                Department dpt = AccountHelper.GetDepartment(departID,
                      new string[] { "Name", "Description", "ParentID" });

                navString = string.Format(navString, dpt.ID, dpt.Name);
                return DepartMappath(dpt.ParentID) + " > " + navString;
            }
        }
    }
         
    #region Inner class

    [Serializable]
    public class ViewItem
    {
        string id;
        string text;
        string summary;
        bool selected;
        string mode;
        string state;
        string url;
        string deleteUrl;
        string editUrl;
        string manageLinks;
        string registerDate;

        public ViewItem()
        {
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public string Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }


        public string DeleteUrl
        {
            get { return deleteUrl; }
            set { deleteUrl = value; }
        }

        public string EditUrl
        {
            get { return editUrl; }
            set { editUrl = value; }
        }

        public string ManageLinks
        {
            get { return manageLinks; }
            set { manageLinks = value; }
        }

        public string RegisterDate
        {
            get { return registerDate; }
            set { registerDate = value; }
        }

    }
    #endregion
}