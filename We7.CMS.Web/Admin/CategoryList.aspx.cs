using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace We7.CMS.Web.Admin
{
    public partial class CategoryList : BasePage
    {
        protected string TypeID
        {
            get { return Request["typeId"]; }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                DataBind();
            }
            DataGridView.RowCommand += new GridViewCommandEventHandler(DataGridView_RowCommand);
        }

        void DataGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName;
            if (String.Compare(cmd, "ed", true) == 0)
            {
                Response.Redirect("CategoryEdit.aspx?typeId=" + TypeID + "&id=" + e.CommandArgument);
            }
            else if(String.Compare(cmd,"del",true)==0)
            {
                try
                {
                    HelperFactory.GetHelper<CategoryHelper>().DeleteCategory(e.CommandArgument as string);
                    DataBind();
                    Messages.ShowMessage("操作成功!");
                }
                catch (Exception ex)
                {
                    Messages.ShowError("删除失败：" + ex.Message);
                }
            }
        }

        protected string TypeName
        {
            get
            {
                return HelperFactory.GetHelper<CategoryHelper>().GetCategory(TypeID).Name;
            }
        }

        protected void DataBind()
        {
            DataGridView.DataSource = HelperFactory.GetHelper<CategoryHelper>().GetFmtChildren(TypeID);
            DataGridView.DataBind();
        }
    }
}