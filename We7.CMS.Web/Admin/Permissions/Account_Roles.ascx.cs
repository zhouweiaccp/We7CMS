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
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;
using System.Collections.Generic;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin.Permissions
{
    public partial class Account_Roles : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        string CurrentAccountID
        {
            get { return Request["id"]; }
        }

        /// <summary>
        /// 角色类型
        /// </summary>
        public OwnerRank RoleType { get; set; }

        protected void Initialize()
        {
            string siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
            personalForm.DataSource = AccountHelper.GetRoles(siteID, RoleType,string.Empty);
            personalForm.DataBind();

            //AccountRole[] ars = AccountHelper.GetAccountRoles(CurrentAccountID);
            List<string> ars = AccountHelper.GetRolesOfAccount(CurrentAccountID);
            if (ars != null)
            {
                foreach (string ar in ars)
                {
                    if (ValuesTextBox.Text.Length > 0)
                    {
                        ValuesTextBox.Text += ";";
                    }
                    ValuesTextBox.Text += ar;
                }
            }
            if (!Page.ClientScript.IsStartupScriptRegistered("onload"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", "<script type=\"text/javascript\">onBodyLoad();</script>");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] newValues = ValuesTextBox.Text.Split(';');
                AccountHelper.UpdateAccountRoles(CurrentAccountID, newValues);
                Messages.ShowMessage("用户角色信息保存成功！");
                Initialize();
            }
            catch (Exception ex)
            {
                Messages.ShowMessage("用户角色信息保存出错！出错原因：" + ex.Message);
            }
        }
    }
}
