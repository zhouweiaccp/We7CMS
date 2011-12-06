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
    public partial class AccountDetails : UserBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init();
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

        void Init()
        {
            LoadAccount();
            if (CurrentAccount != null)
            {
                imgPhoto.ImageUrl = CurrentAccount.Photo;

                lblLoginName.Text = CurrentAccount.LoginName;

                lblFirstName.Text = CurrentAccount.FirstName;
                lblMiddleName.Text = CurrentAccount.MiddleName;
                lblLastName.Text = CurrentAccount.LastName;

                lblEmail.Text = CurrentAccount.Email;
                lblType.Text = CurrentAccount.TypeText;
                lblModel.Text = String.IsNullOrEmpty(CurrentAccount.UserModelName) ? "个人会员" : CurrentAccount.UserModelName;
                lblState.Text = CurrentAccount.StateText;
            }
            //LoadXmlContent();
        }

        //private void LoadXmlContent()
        //{
        //    try
        //    {
        //        string modelXml = "";

        //        if (!We7Helper.IsEmptyID(CurrentAccountID))
        //        {
        //            Account account = AccountHelper.GetAccount(CurrentAccountID, null);
        //            if (account.UserModelID != null && !string.IsNullOrEmpty(account.ModelXml))
        //                modelXml = account.ModelXml;
        //            else if (account.UserModelID != null)
        //                modelXml = AccountHelper.GetXmlTemplateByUserModelID(account.UserModelID.ToString());
        //            if (!string.IsNullOrEmpty(modelXml))
        //                UserContentModelControl.InputXml = modelXml;
        //            else
        //                Messages.ShowError("无法初始化用户模型，没有找到合适的内容模型！");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Messages.ShowMessage("内容信息初始化出错，错误原因：\r\n" + ex.Message);
        //    }
        //}

        protected void lnkbttnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("AccountEdit.aspx");
        }
    }
}
