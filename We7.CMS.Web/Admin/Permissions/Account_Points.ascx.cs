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
using We7.CMS.Common.Enum;
using We7.CMS.Helpers;

namespace We7.CMS.Web.Admin.Permissions
{
    public partial class Account_Points : BaseUserControl
    {
        public string CurrentAccountID
        {
            get { return Request["id"]; }
        }

         public Account ThisAccount
         {
             get
             {
                 Account a = new Account();
                 if (!string.IsNullOrEmpty(CurrentAccountID))
                     a = AccountHelper.GetAccount(CurrentAccountID, null);
                 return a;
             }
         }

        PointHelper PointHelper
        {
            get { return HelperFactory.GetHelper<PointHelper>(); }
        }

        void DataBind()
        {
            gvList.DataSource = PointHelper.ListAllPointByAccount(CurrentAccountID);
            gvList.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBind();
            }
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex >= 0 ? e.NewPageIndex : 0;
            DataBind();
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = gvList.DataKeys[e.RowIndex]["ID"] as string;
            PointHelper.DelPoint(id);
            DataBind();
        }
    }
}
