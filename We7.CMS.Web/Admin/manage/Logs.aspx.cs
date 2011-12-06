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

using Thinkment.Data;
using We7.CMS.Common;
using We7.CMS.Common.PF;

namespace We7.CMS.Web.Admin
{
    public partial class Logs : BasePage
    {
        public DateTime QueryBeginDate
        {
            get
            {
                if (starttime2.Value == "")
                {
                    return DateTime.MinValue;
                }

                DateTime dt = new DateTime();
                try
                {
                    dt = DateTime.Parse(starttime2.Value);
                }
                catch
                {
                    dt = DateTime.MinValue;
                }

                return dt;
            }
        }

        public DateTime QueryEndDate
        {
            get
            {
                if (endtime2.Value == "")
                {
                    return DateTime.MaxValue;
                }

                DateTime dt = new DateTime();
                try
                {
                    dt = DateTime.Parse(endtime2.Value);
                }
                catch
                {
                    dt = DateTime.MaxValue;
                }

                return dt;
            }
        }

        protected override void Initialize()
        {
            LoadLogs();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Initialize();
        }

        protected void Pager_Fired(object sender, EventArgs e)
        {
            LoadLogs();
        }

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//是否是演示站点
            List<string> ids = GetIDs();

            if (ids.Count < 1)
            {
                ShowMessage(true, "你没有选择任何一条记录");
                return;
            }
            foreach (string id in ids)
            {
                LogHelper.DeleteLog(id);
            }

            //记录日志
            string content = string.Format("删除了{0}条日志", ids.Count.ToString());
            AddLog( "日志管理", content);

            ShowMessage(true, string.Format("您已经成功删除{0}条记录", ids.Count.ToString()));
            LoadLogs();
        }

        protected void QueryButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//是否是演示站点
            LoadLogs();
        }

        List<string> GetIDs()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < DataGridView.Rows.Count; i++)
            {
                if (((CheckBox)DataGridView.Rows[i].FindControl("chkItem")).Checked)
                {
                    list.Add(((Label)(DataGridView.Rows[i].FindControl("lblID"))).Text);
                }
            }
            return list;
        }

        void LoadLogs()
        {
            ShowMessage(false, "");
            LogListPanel.Visible = true;

            string actID = GetAccountID(UserTextBox.Text.Trim());

            LogPager.RecorderCount = LogHelper.QueryLogCountByAll(actID, QueryBeginDate, QueryEndDate, "");

            LogPager.FreshMyself();

            string[] fields = new string[] { "ID", "UserID", "Content", "Created", "Page","Remark" };
            List<Log> list = LogHelper.QueryLogsByAll(actID, QueryBeginDate, QueryEndDate, "", LogPager.Begin, LogPager.Count, fields, "Created", false);

            foreach (Log l in list)
            {
                if (l.UserID == We7Helper.EmptyGUID)
                {
                    l.UserID = "管理员（内置帐户）";
                }
                else
                {
                    Account act = AccountHelper.GetAccount(l.UserID, new string[] { "LoginName", "Email" });
                    if (act != null)
                        l.UserID = String.Format("{0}({1})", act.LoginName, act.Email);
                    else
                        l.UserID = "用户已删除";
                }
            }
            if (list.Count <= 0)
            {
                ShowMessage(true, "没有任何记录");
            }
            DataGridView.DataSource = list;
            DataGridView.DataBind();
        }

        string GetAccountID(string name)
        {
            string actID = "";
            if (name != "")
            {    
                if (name == "administrator" || name == "管理员")
                {
                    actID = We7Helper.EmptyGUID;
                }
                else
                {
                    Account act = AccountHelper.GetAccountByLoginName(name);
                    if (act != null)
                    {
                        actID = act.ID;
                    }
                    else
                    {
                        ShowMessage(true, "没有这个用户");
                        LogListPanel.Visible = false;
                    }
                }
            }
            return actID;
        }

        void ShowMessage(bool d, string m)
        {
            MessagePanel.Visible = d;
            MessageLabel.Text = m;
        }

        protected void OutPutButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//是否是演示站点
            Response.Redirect(string.Format("LogDonwload.aspx?aid={0}&begintime={1}&endtime={2}", GetAccountID(UserTextBox.Text.Trim()), QueryBeginDate.ToString(), QueryEndDate.ToString()));
        }


    }
}
