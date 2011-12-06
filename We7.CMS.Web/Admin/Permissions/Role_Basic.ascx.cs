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

namespace We7.CMS.Web.Admin
{
    public partial class Role_Basic : BaseUserControl
    {
        string RoleID
        {
            get { return Request["id"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["saved"] != null && Request["saved"].ToString() == "1")
                {
                    Messages.ShowMessage("角色信息已经成功更新。");
                }
                Initialize();
            }
        }

        protected void Initialize()
        {
            if (RoleID != null)
            {
                Role r = AccountHelper.GetRole(RoleID);
                ShowRole(r);
            }
        }

        void ShowMessage(string m)
        {
            Messages.ShowMessage(m);
        }

        void ShowRole(Role r)
        {
            IDLabel.Text = r.ID;
            NameTextBox.Value = r.Name;
            DescriptionTextBox.Value = r.Description;
            CreatedLabel.Text = r.Created.ToString();
            RoleIDTextBox.Text = r.ID;
            TypeDropDownList1.SelectedValue = r.RoleType.ToString();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            string id = IDLabel.Text;
            string name = NameTextBox.Value;
            string description = DescriptionTextBox.Value;
            string roletype = "";
            if (TypeDropDownList1.Visible==true)
            {
                roletype = TypeDropDownList1.SelectedValue;
            }
            else
            {
                roletype = TypeDropDownList2.SelectedValue;
            }
            if (We7Helper.IsEmptyID(id))
            {
                if (AccountHelper.GetRoleBytitle(name) != null)
                    Messages.ShowError(name + " 的角色已经存在。");
                else
                {
                    string idNew = Guid.NewGuid().ToString();
                    Role r = new Role(idNew, name, description, roletype);
                    AccountHelper.AddRole(r);
                    ShowRole(r);

                    //记录日志
                    string content = string.Format("新建角色“{0}”", name);
                    AddLog("新建角色", content);

                    string rawurl = We7Helper.AddParamToUrl(Request.RawUrl, "saved", "1");
                    rawurl = We7Helper.AddParamToUrl(rawurl, "id", r.ID);
                    Response.Redirect(rawurl);
                }
            }
            else
            {
                Role r = new Role(id, name, description, roletype);
                AccountHelper.UpdateRole(r);
                ShowMessage("角色信息已经更新。");

                //记录日志
                string content = string.Format("修改了角色“{0}”的信息", name);
                AddLog("编辑角色", content);
            }
        }
    }
}