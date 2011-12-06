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
using We7.CMS.Helpers;
using We7.CMS.Common.Enum;
using System.Text;
using We7.CMS.Accounts;

namespace We7.CMS.Web.User
{
    public partial class FavoriteList : UserBasePage
    {
        string Tag
        {
            get
            {
                return HttpUtility.UrlDecode(Request["tag"], Encoding.Default);
            }
        }
        FavoriteHelper FavoriteHelper
        {
            get { return HelperFactory.GetHelper<FavoriteHelper>(); }
        }

        DataTable dtTagList = new DataTable();
        protected bool definedTag = false;
        void DataBind()
        {
            if (string.IsNullOrEmpty(Tag))
            {
                gvList.DataSource = FavoriteHelper.ListAllFavoriteByAccount(Security.CurrentAccountID);
            }
            else
            {
                gvList.DataSource = FavoriteHelper.ListCurrentAccountFavoriteByTag(Security.CurrentAccountID, Tag);
            }
            gvList.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBind();
                DataBindTag();
            }
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex >= 0 ? e.NewPageIndex : 0;
            DataBind();
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = gvList.DataKeys[e.RowIndex]["FavoriteID"] as string;
            FavoriteHelper.DelFavorite(id);
            DataBind();
        }

        protected void gvList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string id = gvList.DataKeys[e.NewEditIndex]["FavoriteID"] as string;
            Response.Redirect("FavoriteEdit.aspx?id=" + id);
        }
        private void DataBindTag()
        {
            if (Security.CurrentAccountID != null)
            {
                DataTable dtTagDefaultList = FavoriteHelper.ListDefaultTag(ref definedTag);
                if (definedTag)
                {
                    DataTable dtTag = FavoriteHelper.ListAllTagByAccount(Security.CurrentAccountID);
                    dtTagList.Merge(dtTagDefaultList);
                    dtTagList.Merge(dtTag);
                    dtTagList = FavoriteHelper.SelectDistinct(dtTagList);
                    dlTagList.DataSource = dtTagList;
                    dlTagList.DataBind();
                }
                else
                {
                    dlTagList.DataSource = dtTagDefaultList;
                    dlTagList.DataBind();
                }
            }
        }
    }
}
