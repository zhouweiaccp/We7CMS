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
    public partial class Account_Password : UserBasePage
    {
        protected void bttnBack_Click(object sender, EventArgs e)
        {
            //返回
            Response.Redirect("AccountDetails.aspx");
        }

        protected void bttnEdit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                bool check = CheckPritivePassword(this.txtPritivePassword.Text.Trim());
                if (!check)
                {
                    this.lblErrorMessage.Text = "原始密码错误！";
                    this.lblErrorMessage.Visible = true;
                    return;
                }

                bool edit = EditAccountPassword(this.txtNewPasswordConfirm.Text.Trim());
                if (!edit)
                {
                    this.lblErrorMessage.Text = "修改密码失败！";
                }
                else
                {
                    Response.Redirect("~/user/logout.aspx?returnurl=/user/index.aspx");
                }
            }
        }

        #region private Method

        /// <summary>
        /// 检查原始密码是否正确
        /// </summary>
        /// <param name="password">输入的原始密码</param>
        /// <returns></returns>
        private bool CheckPritivePassword(string password)
        {
            bool check = false;

            Account account = AccountHelper.GetAccount(CurrentAccountID, null);

            if (account != null)
            {
                if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.noneEncrypt)
                {
                    check = account.Password.Trim() == password;
                }
                else if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt)
                {
                    check = account.Password.Trim() == Security.Encrypt(password);
                }
                else if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.bbsEncrypt)
                {
                    check = account.Password.Trim() == Security.BbsEncrypt(password);
                }
            }

            return check;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password">新密码</param>
        /// <returns></returns>
        private bool EditAccountPassword(string password)
        {
            bool edit = false;

            Account account = AccountHelper.GetAccount(CurrentAccountID, null);
            if (account != null)
            {
                try
                {
                    //UpdatePassword
                    AccountHelper.UpdatePassword(account, password);
                    edit = true;
                }
                catch { }
            }

            return edit;
        }
        #endregion

        #region Property

        public string CurrentAccountID
        {
            get
            {
                return Security.CurrentAccountID;
            }
        }

        #endregion
    }
}
