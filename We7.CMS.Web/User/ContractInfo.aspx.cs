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
    public partial class ContractInfo : UserBasePage
    {
        private Account act;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtAddress.Text = Account.Department;//这儿用部门来代替地址
                txtMobile.Text = Account.Mobile;
                if (!String.IsNullOrEmpty(Account.Tel))
                {
                    string[] ss = Account.Tel.Split('-');
                    if (ss.Length > 1)
                    {
                        txtPrePhone.Text = ss[0];
                        txtPhone.Text = ss[1];
                    }
                    else
                    {
                        txtPhone.Text = ss[0];
                    }
                }
                txtQQ.Text = Account.QQ;
                ltlEmail.Text = Account.Email;
            }
            bttnSubmit.Click += new EventHandler(bttnSubmit_Click);
        }

        void bttnSubmit_Click(object sender, EventArgs e)
        {
            Account.Department = txtAddress.Text.Trim();
            Account.Mobile = txtMobile.Text.Trim();
            if (!String.IsNullOrEmpty(txtPrePhone.Text) && !String.IsNullOrEmpty(txtPrePhone.Text.Trim()))
            {
                Account.Tel = txtPrePhone.Text.Trim() + "-" + txtPhone.Text.Trim();
            }
            else
            {
                Account.Tel = txtPhone.Text.Trim();
            }
            Account.QQ = txtQQ.Text.Trim();
            AccountFactory.CreateInstance().UpdateAccount(Account, new string[] { "Department", "Mobile", "Tel", "QQ" });
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "success", "alert('修改成功！')", true);
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
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
    }
}
