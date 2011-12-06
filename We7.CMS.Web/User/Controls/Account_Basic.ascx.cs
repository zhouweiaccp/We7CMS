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
using System.IO;
using We7.CMS.Accounts;

namespace We7.CMS.Web.User.Controls
{
    public partial class Account_Basic : BaseUserControl
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                //Init();
                InitEdit();
            }
        }
        private Account CurrentAccount;

        public string CurrentAccountID
        {
            get
            {
                return Security.CurrentAccountID;
            }
        }

        void LoadAccount()
        {
            if(!We7Helper.IsEmptyID(CurrentAccountID))
                CurrentAccount = AccountHelper.GetAccount(CurrentAccountID, null);
        }

        /// <summary>
        /// 检查头像
        /// </summary>
        /// <param name="currUser"></param>
        void CheckPhoto(ref Account currUser)
        {
            if (string.IsNullOrEmpty(currUser.Photo))
            {
                currUser.Photo = "/user/style/images/face_big.gif";
                AccountHelper.UpdateAccount(currUser, new string[] { "Photo" });
            }
        }

        void Init()
        {
            LoadAccount();
            if (CurrentAccount != null)
            {
                lblLoginName.Text = CurrentAccount.LoginName;

                lblFirstName.Text = CurrentAccount.FirstName;
                lblMiddleName.Text = CurrentAccount.MiddleName;
                lblLastName.Text = CurrentAccount.LastName;

                lblEmail.Text = CurrentAccount.Email;
                lblType.Text = CurrentAccount.TypeText;
                lblModel.Text =String.IsNullOrEmpty(CurrentAccount.UserModelName)?"个人会员":CurrentAccount.UserModelName;
                lblState.Text = CurrentAccount.StateText;
            }
        }

        void InitEdit()
        {
            LoadAccount();
            if (CurrentAccount != null)
            {
                CheckPhoto(ref CurrentAccount);

                lblLoginName2.Text = CurrentAccount.LoginName;

                txtFirstName.Text = CurrentAccount.FirstName;
                txtMiddleName.Text = CurrentAccount.MiddleName;
                txtLastName.Text = CurrentAccount.LastName;

                txtEmail.Text = CurrentAccount.Email;
                lblType2.Text = CurrentAccount.TypeText;
                lblModel2.Text = String.IsNullOrEmpty(CurrentAccount.UserModelName) ? "个人会员" : CurrentAccount.UserModelName;
                lblState2.Text = CurrentAccount.StateText;
                imgHead.ImageUrl = CurrentAccount.Photo;
                imgHeader2.ImageUrl = CurrentAccount.Photo;
            }
        }

        protected void bttnBack_Click(object sender, EventArgs e)
        {
            //tbDetails.Visible = true;
            //tbEdit.Visible = false;
            Response.Redirect("AccountDetails.aspx");
        }

        protected void lnkbttnEdit_Click(object sender, EventArgs e)
        {
            tbDetails.Visible = false;
            tbEdit.Visible = true;
            InitEdit();
        }

        protected void bttnUpload_Click(object sender, EventArgs e)
        {
            if (upload.HasFile)
            {
                string fileName=String.Format("/_data/Uploads/{0}{1}",Guid.NewGuid(),Path.GetExtension(upload.FileName));
                upload.SaveAs(Server.MapPath("~"+fileName));
                imgHead.ImageUrl = fileName;
                imgHeader2.ImageUrl = fileName;
            }
        }

        protected void bttnEdit_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(CurrentAccountID))
            {
                Account account = new Account();
                account.ID = CurrentAccountID;
                account.Photo = imgHeader2.ImageUrl;
                account.LastName = txtLastName.Text.Trim();
                account.Email = txtEmail.Text.Trim();
                AccountHelper.UpdateAccount(account, new string[] {"LastName","Email","Photo"});

                Response.Redirect("/User/AccountEdit.aspx?t="+DateTime.Now.Ticks.ToString());
                //tbDetails.Visible = true;
                //tbEdit.Visible = false;
                //Init();
            }
        }
    }
}