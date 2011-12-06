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
using We7.Framework;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
    public partial class CatTypeAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NameCheckLable.Visible = false;
            KeyCheckLable.Visible = false;
            if (!IsPostBack)
            {
                InitControl();
                BindData();
            }
        }

        string CatId
        {
            get { return Request["id"]; }
        }

        protected void BindData()
        {
            if (!String.IsNullOrEmpty(CatId))
            {
               CategoryHelper helper = HelperFactory.Instance.GetHelper<CategoryHelper>();
               Category cat=helper.GetCategory(CatId);
               if (cat != null)
               {
                   txtName.Text = cat.Name;
                   txtKey.Text = cat.KeyWord;
                   txtDesc.Text = cat.Description;
                   ddlOptions.SelectedValue = cat.Options;
               }
            }
        }

        protected void InitControl()
        {
            ddlOptions.DataSource = CategoryOptionHelper.GetOptionTypes();
            ddlOptions.DataBind();
        }

        protected void SaveLinkbutton_Click(object sender, EventArgs e)
        {
            CategoryHelper helper = HelperFactory.Instance.GetHelper<CategoryHelper>();

            if (String.IsNullOrEmpty(CatId))
            {
                if (helper.CheckNameRepeat(txtName.Text.Trim()))
                {
                    NameCheckLable.Visible = true;
                    return;
                }

                if (helper.CheckKeywordRepeat(txtKey.Text.Trim()))
                {
                    KeyCheckLable.Visible = true;
                    return;
                }
            }

            Category cat = new Category();
            cat.Name = txtName.Text.Trim();
            cat.KeyWord = txtKey.Text.Trim();
            cat.Description = txtDesc.Text.Trim();
            cat.ParentID = We7Helper.EmptyGUID;
            cat.Options = ddlOptions.SelectedValue;
            if (String.IsNullOrEmpty(CatId))
            {
                try
                {
                    helper.AddCategory(cat);
                    Response.Redirect("CatTypeMgr.aspx");
                }
                catch (Exception ex)
                {
                    Messages.ShowError("添加类型出错:" + ex.Message);
                }
            }
            else
            {
                try
                {
                    cat.ID = CatId;
                    helper.UpdateCategory(cat);
                    Response.Redirect("CatTypeMgr.aspx");
                }
                catch (Exception ex)
                {
                    Messages.ShowError("更新类型出错:" + ex.Message);
                }
            }            
        }

        
    }
}
