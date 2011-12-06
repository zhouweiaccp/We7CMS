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

namespace We7.CMS.Web.User
{
    public partial class UserInfo : UserBasePage
    {
        private Account act;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ltlLoginName.Text = Account.LoginName;
                txtBirthday.Text = Account.Birthday.ToString("yyyy-MM-dd");
                txtIntro.Text = Account.Description;
                txtRealName.Text = Account.LastName;
                if (Account.Sex == "0")
                {
                    rdFemale.Checked = true;
                }
                else
                {
                    rdMale.Checked = true;
                }
            }
            bttnSubmit.Click += new EventHandler(bttnSubmit_Click);
        }

        void bttnSubmit_Click(object sender, EventArgs e)
        {
            Account.LastName = txtRealName.Text.Trim();
            Account.Description = txtIntro.Text.Trim();
            Account.Sex = rdMale.Checked ? "1" : "0";
            DateTime dt;
            DateTime.TryParse(txtBirthday.Text.Trim(), out dt);
            Account.Birthday = dt;
            AccountFactory.CreateInstance().UpdateAccount(Account, new string[] { "LastName", "Description", "Sex", "Birthday" });
            Page.RegisterStartupScript("success", "<script>alert('修改成功!');</script>");
        }

        protected Account Account
        {
            get
            {
                if (act == null)
                {
                    act = AccountFactory.CreateInstance().GetAccount(Security.CurrentAccountID, null) ?? new Account();
                }
                return act;
            }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }
    }
}
