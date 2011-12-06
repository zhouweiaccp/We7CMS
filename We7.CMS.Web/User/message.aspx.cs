using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.CMS.Config;
using We7.Framework.Config;

namespace We7.CMS.Web.User
{
    public partial class message : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.User;
            }
        }

        string SenderID { get; set; }
        string ReceiverID { get; set; }
        MessageState MsgState
        {
            get
            {
                SenderID = ReceiverID = string.Empty;
                MessageState state = MessageState.Unknown;
                switch (Request["state"])
                {
                    case "inbox":
                        state = MessageState.Inbox;
                        ReceiverID = AccountID;
                        break;
                    case "outbox":
                        state = MessageState.Outbox;
                        SenderID = AccountID;
                        break;
                    case "sms":
                        state = MessageState.AllSMS;
                        SenderID = AccountID;
                        break;
                    case "draft":
                        state = MessageState.Draft;
                        SenderID = AccountID;
                        break;
                    default:
                        state = MessageState.Unknown;
                        break;
                }
                return state;
            }
        }
        private int _resultsPageNumber = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        protected int PageNumber
        {
            get
            {
                if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
                    _resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
                return _resultsPageNumber;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadMessages();
            //??
            //   Response.Write(ReceiverID + "|" + MsgState);
        }

        void LoadMessages()
        {
            ArticleUPager.PageIndex = PageNumber;
            ArticleUPager.ItemCount = MessageHelper.QueryMessageCount(MsgState, SenderID, ReceiverID);
            ArticleUPager.UrlFormat = We7Helper.AddParamToUrl(Request.RawUrl.Replace("{", "{{").Replace("}", "}}"), Keys.QRYSTR_PAGEINDEX, "{0}");
            ArticleUPager.PrefixText = "共 " + ArticleUPager.MaxPages + "  页 ·   第 " + ArticleUPager.PageIndex + "  页 · ";
            List<ShortMessage> myMessages = MessageHelper.GetPagedMessages(MsgState, SenderID, ReceiverID, ArticleUPager.Begin - 1, ArticleUPager.Count);
            foreach (ShortMessage sm in myMessages)
            {
                if (AccountID == We7Helper.EmptyGUID)
                    sm.AccountName = SiteConfigs.GetConfig().AdministratorName;
            }

            DataGridView.DataSource = myMessages;
            DataGridView.DataBind();
        }

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();
            ids = GetIDs();

            if (ids.Count < 1)
            {
                Messages.ShowError("你没有选择任何一条记录");
                return;
            }
            int delCount = 0;
            int unDelCount = 0;
            string aTitle = "";
            string message = "";
            foreach (string id in ids)
            {
                MessageHelper.DeleteMessage(id);
                delCount += 1;
            }
            //记录日志
            if (delCount > 0)
            {
                string content = string.Format(" 删除了{0}篇短信。", delCount );
                AddLog("短消息管理", content);
            }

            message = string.Format("您已经成功删除{0}条记录,{1}条未删除！", delCount, unDelCount) + message;
            Messages.ShowMessage(message);

            LoadMessages();
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
    }
}
