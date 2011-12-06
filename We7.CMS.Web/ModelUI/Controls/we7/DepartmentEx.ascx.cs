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
using We7.Model.UI.Controls;
using We7.Framework;
using We7.CMS.Common;
using System.Xml;
using System.Collections.Generic;
using We7.CMS.Common.PF;
using We7.CMS.Accounts;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin.ContentModel.Controls
{
    public partial class DepartmentEx : We7FieldControl
    {
        private IAccountHelper helper = AccountFactory.CreateInstance();
        private Account act;
        private Account CurrentAccount
        {
            get
            {
                if (act == null)
                {
                    act = helper.GetAccount(Security.CurrentAccountID, null);
                }
                return act;
            }
        }

        public override void InitControl()
        {
            string keyword = Control.Params["keyword"];
            string format = Control.Params["format"];
            string parentId = Control.Params["parentId"];
            string role = Control.Params["role"];

            List<Department> departments;
            string siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
            if (String.Compare("true", format, true) == 0)
            {
                departments = helper.GetDepartmentTreeWithFormat(siteID, parentId);
            }
            else
            {
                departments = helper.GetDepartmentTree(siteID, parentId);
            }

            ddlDepartment.DataSource = departments;
            ddlDepartment.DataTextField = "Name";
            ddlDepartment.DataValueField = "ID";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("请选择", ""));

            string val = Value as string;
            if (String.IsNullOrEmpty(val))
            {
                if (CurrentAccount != null)
                {
                    foreach (ListItem item in ddlDepartment.Items)
                    {
                        item.Selected = item.Value == CurrentAccount.DepartmentID;
                    }
                }
            }
            else
            {
                ddlDepartment.SelectedValue = val;
            }

            if (!String.IsNullOrEmpty(role) && Security.CurrentAccountID!=We7Helper.EmptyGUID )
            {
                List<string> actids = helper.GetRolesOfAccount(Security.CurrentAccountID);
                bool flag = false;
                if (actids != null)
                {
                    foreach (string s in actids)
                    {
                        if (s ==role.Trim('{','}'))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                ddlDepartment.Enabled = flag;
            }

            if (!String.IsNullOrEmpty(Control.Width))
            {
                ddlDepartment.Width = Unit.Parse(Control.Width);
            }
            if (!String.IsNullOrEmpty(Control.Height))
            {
                ddlDepartment.Height = Unit.Parse(Control.Height);
            }

            ddlDepartment.CssClass = Control.CssClass;
            if (Control.Required && !ddlDepartment.CssClass.Contains("required"))
            {
                ddlDepartment.CssClass += " required";
            }
        }

        public override object GetValue()
        {
            return ddlDepartment.SelectedValue;
        }
    }
}