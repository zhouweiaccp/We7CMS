using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common;
using System.Globalization;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin
{
    public partial class CategoryEdit : BasePage
    {
        protected string TypeID
        {
            get { return Request["typeID"].Trim(); }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        protected void SaveLinkbutton_Click(object sender, EventArgs args)
        {
            Category cat = new Category();
            cat.ParentID = String.IsNullOrEmpty(ddlParentCategory.SelectedValue) ? TypeID : ddlParentCategory.SelectedValue.Trim();
            cat.Name = txtName.Text.Trim();
            cat.KeyWord = txtKey.Text.Trim();
            cat.Description = txtDesc.Text.Trim();
            int index;
            cat.Index = Int32.TryParse(txtIndex.Text.Trim(), out index) ? index : 0;

            int op = 0;
            foreach (ListItem item in chkOptions.Items)
            {
                int temp;
                if (item.Selected && Int32.TryParse(item.Value,NumberStyles.AllowHexSpecifier,null,out temp))
                {
                    op |= temp;
                }
            }
            cat.Options = op.ToString();

            string id = Request["id"];
            if (String.IsNullOrEmpty(id))
            {
                try
                {
                    CategoryHelper.AddCategory(cat);
                    Messages.ShowMessage("添加成功!<a href='CategoryEdit.aspx?typeId="+TypeID+"'>继续添加</a>");
                    //Response.Redirect("CategoryList.aspx?typeId=" + TypeID);
                }
                catch (Exception ex)
                {
                    Messages.ShowError("添加信息出错:" + ex.Message);
                }
            }
            else
            {
                try
                {
                    cat.ID = id;
                    CategoryHelper.UpdateCategory(cat);
                    Messages.ShowMessage("修改成功!");
                    //Response.Redirect("CategoryList.aspx?typeId=" + TypeID);
                }
                catch (Exception ex)
                {
                    Messages.ShowError("修改信息出错:" + ex.Message);
                }
            }
        }

        private void BindData()
        {
            string id = Request["id"];
            BindType();

            Category cat = CategoryHelper.GetCategory(id);
            if (cat != null)
            {
                ddlParentCategory.SelectedValue = cat.ParentID;
                ddlParentCategory.Enabled = false;
                txtName.Text = cat.Name;
                txtKey.Text = cat.KeyWord;
                txtDesc.Text = cat.Description;
                txtIndex.Text = cat.Index.ToString();

                foreach (ListItem item in chkOptions.Items)
                {
                    int v;
                    item.Selected=CategoryOptionHelper.Check(item.Value,cat.IntOption);
                }
            }
        }

        void BindType()
        {
            ddlParentCategory.DataSource = CategoryHelper.GetFmtChildren(TypeID);
            ddlParentCategory.DataTextField = "Name";
            ddlParentCategory.DataValueField = "ID";
            ddlParentCategory.DataBind();
            ddlParentCategory.Items.Insert(0, new ListItem("根目录", ""));

            Category cat = CategoryHelper.GetCategory(TypeID);
            if (cat != null)
            {
                chkOptions.DataSource = CategoryOptionHelper.GetOptions(cat.Options);
                chkOptions.DataTextField = "Name";
                chkOptions.DataValueField = "Value";
                chkOptions.DataBind();
            }
        }

        protected CategoryHelper CategoryHelper
        {
            get { return HelperFactory.GetHelper<CategoryHelper>(); }
        }

        protected string TypeName
        {
            get
            {
                return CategoryHelper.GetCategory(TypeID).Name;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                BindData();
            }
        }

    }
}