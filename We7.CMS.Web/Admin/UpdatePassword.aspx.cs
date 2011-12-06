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
using We7.CMS.Config;
using We7.CMS.Common.PF;
using We7.Framework.Config;
using We7.CMS.Accounts;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class UpdatePassword : BasePage
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
            if (!IsPostBack)
            {
                if (!We7Helper.IsEmptyID(AccountID))
                {
                    LoginNameTextBox.Text = AccountHelper.GetAccount(AccountID, new string[] { "LoginName" }).LoginName;
                }
                else
                    LoginNameTextBox.Text = SiteConfigs.GetConfig().AdministratorName;
            }
        }

        void changePassword(string loginName, string password,string newPassword)
        {

            if (AccountID==We7Helper.EmptyGUID && String.Compare(loginName, SiteConfigs.GetConfig().AdministratorName, true) == 0)
            {
                if (CDHelper.AdminPasswordIsValid(password))
                {
                   SiteConfigInfo si = SiteConfigs.GetConfig();
                    bool isHashed =si.IsPasswordHashed ;
                    if (isHashed != IsHashedPasswordCheckBox.Checked)
                        si.IsPasswordHashed = IsHashedPasswordCheckBox.Checked;
                    if(IsHashedPasswordCheckBox.Checked)
                        si.AdministratorKey=Security.Encrypt(newPassword);
                    else
                         si.AdministratorKey=newPassword;

                    SiteConfigs.SaveConfig(si);
                     //CDHelper.UpdateSystemInformation(si);

                    ShowMessage("您的密码已修改成功。");
                }
                else
                {
                    ShowMessage("对不起，您输入的旧密码不正确！");
                }
            }
            else
            {
                Account act = AccountHelper.GetAccountByLoginName(loginName);
                if (act == null )
                {
                    ShowMessage("指定的用户不存在。");
                }
                else if (!AccountHelper.IsValidPassword(act,password))
                {
                    ShowMessage("对不起，您输入的旧密码不正确！");
                }
                else if (act.State != 1)
                {
                    ShowMessage("该帐户不可用。");
                }
                else
                {
                    act.IsPasswordHashed = IsHashedPasswordCheckBox.Checked;
                    AccountHelper.UpdatePassword(act, newPassword);

                    //记录日志
                    string content = string.Format("修改了“{0}”的密码", act.LoginName);
                    AddLog("修改密码", content);

                    ShowMessage("您的密码已修改成功。");
                }
            }
        }


        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//是否是演示站点
            changePassword(LoginNameTextBox.Text, PasswordTextBox.Text, NewPasswordTextBox.Text);
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            LoginNameTextBox.Text="";
            PasswordTextBox.Text="";
            NewPasswordTextBox.Text="";
            AgainPasswordTextBox.Text="";
        }
    }
}
