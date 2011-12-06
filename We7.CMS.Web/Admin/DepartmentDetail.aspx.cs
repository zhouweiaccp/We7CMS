using System;
using System.Text;
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
using We7.CMS.Common;
using We7.Framework.Cache;
using We7.CMS.Accounts;

namespace We7.CMS.Web.Admin
{
    public partial class DepartmentDetail : BasePage
    {

        string DepartmentID
        {
            get { return Request["id"]; }
        }

        string ParentID
        {
            get { return Request["pid"]; }
        }

        protected override void Initialize()
        {
            if (!string.IsNullOrEmpty(DepartmentID))
            {
                if(We7Helper.IsEmptyID(DepartmentID))
                {
                    Messages.ShowError("不能修改。");
                }
                else
                {
                    DepartmentNameLabel.Text = "修改部门信息";
                    ShowDepartment(AccountHelper.GetDepartment(DepartmentID, null));
                }
            }
            else
            {
                if (ParentID != null)
                {
                    ParentTextBox.Text = ParentID;
                    DepartmentNameLabel.Text = "新建一个部门。";
                    ReturnHyperLink.NavigateUrl = String.Format("Departments.aspx?id={0}", ParentID);
                    if (We7Helper.IsEmptyID(ParentID))
                    {
                        FullPathLabel.Text = "/";
                    }
                    else
                    {
                        Department dpt = AccountHelper.GetDepartment(ParentID, new string[] { "FullName" });
                        FullPathLabel.Text = dpt.FullName;
                    }
                }
                else
                {
                    Messages.ShowError("缺少必要的参数信息。");
                }
            }
        }

        protected override void HandleException(Exception e)
        {
            ContentPanel.Visible = false;
            Messages.ShowError(e.Message);
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                SaveDepartment();
                CacheRecord.Create(typeof(AccountLocalHelper)).Release();
            }
            catch (CDException ce)
            {
                ContentPanel.Visible = false;
                Messages.ShowError(ce.Message);
            }
        }

        void ShowDepartment(Department dpt)
        {
            NameTextBox.Text = dpt.Name;
            NumberTextBox.Text = dpt.Number;
            IndexTextBox.Text = dpt.Index.ToString();
            DescriptionTextBox.Text = dpt.Description;
            IDLabel.Text = dpt.ID.ToString();
            ParentTextBox.Text = dpt.ParentID.ToString();
            CreatedLabel.Text = dpt.Created.ToString();
            AddressTextBox.Text = dpt.Address;
            FaxTextBox.Text = dpt.Fax;
            EmailTextBox.Text = dpt.Email;
            PhoneTextBox.Text = dpt.Phone;
            MapScriptTextBox.Text = dpt.MapScript;
            SiteUrlTextBox.Text = dpt.SiteUrl;
            ContentTextBox.Value = dpt.Text;

            DepartmentNameLabel.Text = string.Format("编辑部门 {0} 信息",dpt.Name);
            ReturnHyperLink.NavigateUrl = String.Format("Departments.aspx?id={0}", dpt.ParentID);
            if (!We7Helper.IsEmptyID(dpt.ParentID))
            {
                Department p = AccountHelper.GetDepartment(dpt.ParentID, new string[] { "FullName" });
                FullPathLabel.Text = p.FullName;
            }
            else
            {
                FullPathLabel.Text = "/";
            }
        }


        void SaveDepartment()
        {
            Department dpt = new Department();
            dpt.ID = IDLabel.Text;
            dpt.Name = NameTextBox.Text;
            dpt.Description =DescriptionTextBox.Text;
            dpt.Index = Convert.ToInt32(IndexTextBox.Text);

            dpt.Number = NumberTextBox.Text.Trim();
            dpt.Address=AddressTextBox.Text ;
            dpt.Fax = FaxTextBox.Text;
            dpt.Email = EmailTextBox.Text;
            dpt.Phone = PhoneTextBox.Text;
            dpt.MapScript = MapScriptTextBox.Text;
            dpt.SiteUrl = SiteUrlTextBox.Text;
            dpt.Text = ContentTextBox.Value;
            
            if (dpt.ID != String.Empty)
            {
                if (We7Helper.IsEmptyID(dpt.ParentID))
                    dpt.FullName = dpt.Name;
                else
                {
                    Department dptParent = AccountHelper.GetDepartment(dpt.ParentID,null);
                    dpt.FullName = dptParent.FullName + "<" + dpt.Name;
                }

                List<string> fields = new List<string>();
                fields.Add("Name");
                fields.Add("Description");
                fields.Add("Index");
                fields.Add("FullName");
                fields.Add("Address");
                fields.Add("Fax");
                fields.Add("Email");
                fields.Add("Phone");
                fields.Add("MapScript");
                fields.Add("SiteUrl");
                fields.Add("Text");
                fields.Add("Number");

                AccountHelper.UpdateDepartment(dpt, fields.ToArray());
                Messages.ShowMessage("部门信息已经被更新。");

                //记录日志
                string content = string.Format("修改了部门“{0}”的信息", dpt.Name);
                AddLog("编辑部门", content);
            }
            else
            {
                dpt.ParentID = ParentTextBox.Text;
                if (We7Helper.IsEmptyID(dpt.ParentID))
                    dpt.FullName = dpt.Name;
                else
                {
                    Department dptParent = AccountHelper.GetDepartment(dpt.ParentID,null);
                    dpt.FullName = dptParent.FullName + "/" + dpt.Name;
                }

                dpt = AccountHelper.AddDepartment(dpt);
                ShowDepartment(dpt);
                Messages.ShowMessage("新的部门信息已经被保存。");
                //记录日志
                string content = string.Format("新建部门“{0}”", dpt.Name);
                AddLog( "新建部门", content);
            }
        }
    }
}
