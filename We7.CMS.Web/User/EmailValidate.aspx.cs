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
    public partial class EmailValidate : UserBasePage
    {
        protected bool IsEmailValidate;

        protected string Email = "";

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init();
            }
        }

        public int _emailValidateStep = 1;
        public int EmailValidateStep
        {
            get
            {
                return _emailValidateStep;
            }
            set
            {
                _emailValidateStep = value;
            }
        }

        protected void Init()
        {
            if (Security.CurrentAccountID == null)
            {
                Response.Redirect("/user/login.aspx?returnURL=" + HttpUtility.UrlEncode(Request.RawUrl));
            }
            Account currUser
               = AccountHelper.GetAccount(Security.CurrentAccountID, new string[] { "Email", "FirstName", "MiddleName", "LastName", "Photo", "EmailValidate" });
            IsEmailValidate = currUser.EmailValidate == 0 ? false : true;
            Email = currUser.Email;
            txtEmail.Text = currUser.Email;
            if (IsEmailValidate)
            {
                EmailValidateStep = 3;
            }
        }


        protected string GetStep1Str()
        {
            switch (EmailValidateStep)
            {
                case 1:
                    return "line_frist";
                case 2:
                    return "line_last";
                case 3:
                    return "line";
                default:
                    return "";
            }
        }

        protected string GetStep2Str()
        {
            switch (EmailValidateStep)
            {
                case 1:
                    return "line";
                case 2:
                    return "line_frist";
                case 3:
                    return "line_last";
                default:
                    return "";
            }
        }

        protected string GetStep3Str()
        {
            switch (EmailValidateStep)
            {
                case 1:
                    return "line_end";
                case 2:
                    return "line_end";
                case 3:
                    return "line_over";
                default:
                    return "";
            }
        }
        protected void lbtnChange_Click(Object sender, EventArgs e)
        {
            EmailValidateStep = 1;
        }

        protected void lbtnNext_Click(Object sender, EventArgs e)
        {
            Account currUser
              = AccountHelper.GetAccount(Security.CurrentAccountID, new string[] { "Email", "FirstName", "MiddleName", "LastName", "Photo", "EmailValidate" });
            SendMail(Security.CurrentAccountID, "user");
            EmailValidateStep = 2;
        }

        void SendMail(string accountId, string type)
        {
            try
            {
                if (type == "user")
                {
                    AccountMails.SendMailOfValidate(AccountHelper.GetAccount(accountId, null), We7.Model.Core.UI.Constants.FEID);
                }
                else
                {
                    AccountMails.SendMailOfHandle(AccountHelper.GetAccount(accountId, null), "商城高级用户");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('电子邮件发送失败，请检查邮件设置。原因：" + ex.Message + "')</script>");
            }
        }

        //Protected Sub lbtnChange_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        //End Sub

        //string GetPrefix(int step)
        //{
        //    switch(step)
        //    {
        //        case 1:
        //    }
        //}

    }
}
