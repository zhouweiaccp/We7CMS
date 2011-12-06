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

namespace We7.CMS.Web.Admin
{
    public partial class GetPassword : BasePage
    {
        protected override bool NeedAnAccount
        {
            get { return false; }
        }

        void ShowMessage(string m)
        {
            MessageLabel.Text = m;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            ShowMessage(CDHelper.GetMyPassword(LoginNameTextBox.Text, MailTextBox.Text,AccountHelper));
        }
    }
}
