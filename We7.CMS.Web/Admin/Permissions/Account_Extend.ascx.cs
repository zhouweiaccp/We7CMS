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

namespace We7.CMS.Web.Admin.Permissions
{
    public partial class Account_Extend : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Account account = AccountHelper.GetAccount(Request["id"], null);
            if (string.IsNullOrEmpty(account.ModelName))
            {
                this.Controls.Clear();
                //msgLiteral.Text="当前用户未设置用户模型。";
                //msgLiteral.Visible = true;
                //EditorPanel1.Visible = false;
            }
            else
            {
                EditorPanel1.ModelName = account.ModelName;
                EditorPanel1.PanelName = "edit";
            }

        }
    }
}