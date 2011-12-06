using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin.manage
{
    public partial class DepartmentList : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DetailGridView.DataSource = GetDepartmentList();
            DetailGridView.DataBind();
        }

        public List<Department> GetDepartmentList()
        {
            string siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
            List<Department> departments = new List<Department>();
            departments = AccountHelper.GetDepartments(siteID, string.Empty, string.Empty, new string[] { "ParentID", "ID", "Name","FullName" });
            //List<Department> departmentList = new List<Department>();
            //if (departments != null)
            //{
            //    foreach (Department depList in departments)
            //    {
            //        if (depList.FullName != "")
            //        {
            //            int c = depList.FullName.Length - depList.FullName.Replace("/", String.Empty).Length;
            //            for (int i = 0; i < c; i++)
            //            {
            //                depList.Name = "&nbsp;" + "&nbsp;" + "&nbsp;" + depList.Name;
            //            }
            //        }
            //        departmentList.Add(depList);
            //    }
            //}
            //return departmentList;
            return departments;
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {


        }

    }
}
 