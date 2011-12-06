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
using System.IO;
using System.Text;
using We7.CMS.Common;
using We7.CMS.Common.PF;

namespace We7.CMS.Web.Admin
{
    public partial class LogDonwload : BasePage
    {

        string AID
        {
            get { return Request["aid"]; }
        }

        string QueryBeginDate
        {
            get { return Request["begintime"]; }
        }

        string QueryEndDate
        {
            get { return Request["endtime"]; }
        }


        protected override void Initialize()
        {
            ReturnHyperLink.NavigateUrl = Request.Path;
            LoadLogs();
        }

        void LoadLogs()
        {
            if (AID != "")
            {
                int count = LogHelper.QueryLogCountByAll(AID, DateTime.Parse(QueryBeginDate), DateTime.Parse(QueryEndDate), "");
                string[] fields = new string[] { "ID", "UserID", "Content", "Created", "Page" };

                List<Log> list = LogHelper.QueryLogsByAll(AID, DateTime.Parse(QueryBeginDate), DateTime.Parse(QueryEndDate), "", 1, count, fields, "Created", false);

                foreach (Log l in list)
                {
                    if (l.UserID == We7Helper.EmptyGUID)
                    {
                        l.UserID = "管理员（内置帐户）";
                    }
                    else
                    {
                        Account act = AccountHelper.GetAccount(l.UserID, new string[] { "LoginName", "Email" });
                        l.UserID = String.Format("{0}({1})", act.LoginName, act.Email);
                    }
                }
                if (list.Count <= 0)
                {
                    LogsTextBox.Text = "没有任何记录";
                    return;
                }

                string content = "";
                foreach (Log log in list)
                {
                    string userID = string.Format("<font color=red>{0}</font>", log.UserID);
                    string created = string.Format("<font color=#777777>{0}</font>", log.Created.ToString());

                    string logContent = string.Format("<font color=green>{0}于{1}\r\n在{2}页面，{3}。</font><br>",
                    userID, created, log.Page, log.Content);
                    content += logContent;
                }
                LogsTextBox.Text = content;
            }
        }
    }
}
