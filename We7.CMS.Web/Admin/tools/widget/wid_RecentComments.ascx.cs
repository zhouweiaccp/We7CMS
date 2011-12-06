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
using System.Collections.Generic;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.tools.widget
{
    public partial class wid_RecentComments : BaseUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindingData();
            }
        }
        public void BindingData()
        {
            bool enableCache = (CDHelper.Config.EnableCache == "true");
            List<Comments> result = null;
            List<Comments> list = CommentsHelper.GetAllComments(AccountID, 0, 5);
            if (list != null)
            {
                foreach (Comments comments in list)
                {
                    if (comments.Content.Length > 25)
                    {
                        comments.Content = comments.Content.Substring(0, 25) + "...";
                    }
                }
            }
            result = list;

            DataGridView.DataSource = result;
            DataGridView.DataBind();
        }
    }
}