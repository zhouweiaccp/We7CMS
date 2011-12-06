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
using We7.CMS.Common.PF;
using We7.CMS.Accounts;

namespace We7.CMS.Web.User.Controls
{
    public partial class Account_Extent : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Request[We7.Model.Core.UI.Constants.FEID]))
            {
                Response.Redirect(We7Helper.AddParamToUrl(Request.RawUrl,We7.Model.Core.UI.Constants.FEID,Security.CurrentAccountID));
            }
            Account account = AccountHelper.GetAccount(Security.CurrentAccountID, null);
            
            if (string.IsNullOrEmpty(account.ModelName))
            {               
                Response.Write("<script>alert('该用户没有使用内容模型！');location.href='/User/AccountEdit.aspx';</script>");
                Response.End();
                return;
            }
            EditorPanel1.ModelName = account.ModelName;
            EditorPanel1.PanelName = "fedit";
        }
    }
}