using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Thinkment.Data;
using We7.CMS.Common;
using We7.CMS.Common.PF;

namespace We7.CMS.Web.Admin
{
    public partial class LogDetail : BasePage
    {
        string LogID
        {
            get { return Request["id"]; }
        }
        protected override void Initialize()
        {
            Log log = LogHelper.GetLog(LogID, new string[] { "ID", "Page", "UserID", "Content", "Created" });
            
            NameLabel.Text = log.Content;
            SummaryLabel.Text = log.Created.ToString();

            //用户名
            string userName;
            if (AccountID == We7Helper.EmptyGUID)
            {
                userName = "管理员（内置帐户）";
            }
            else
            {
                Account act = AccountHelper.GetAccount(AccountID, new string[] { "LoginName", "Email" });
                userName = String.Format("{0}({1})", act.LoginName, act.Email);
            }
            //详细描述
            string content = string.Format("{0}于{1}，在{2}页面，{3}。",
                userName,SummaryLabel.Text,log.Page,NameLabel.Text);

            ContentLabel.Text = content;
        }
    }
}
