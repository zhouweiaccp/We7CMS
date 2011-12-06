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
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;
using We7.CMS.Accounts;


namespace We7.CMS.Web.Admin.manage.controls
{
    public partial class ProcessHistoryList : System.Web.UI.UserControl
    {
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }
        protected ProcessingHelper ProcessingHelper
        {
            get { return HelperFactory.GetHelper<ProcessingHelper>(); }
        }
        protected ProcessHistoryHelper ProcessHistoryHelper
        {
            get { return HelperFactory.GetHelper<ProcessHistoryHelper>(); }
        }
        public string OwnerID
        {
            get
            { return Security.CurrentAccountID; }
        }

        public string ArticleID
        {
            get
            {
                if (Request["id"] != null && Request["id"] != "")
                {
                    return Request["id"].ToString();
                }
                else
                    return "";
            }
        }

        public string AdviceID
        {
            get
            {
                if (Request["id"] != null && Request["id"] != "")
                {
                    return Request["id"].ToString();
                }
                else
                    return "";
            }
        }

        string StartState
        {
            get
            {
                if (Request["from"] != null && Request["from"] == "advice")
                {
                    return "-1";
                }
                else
                    return "0";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Binding();
        }

        public void Binding()
        {
            ProcessHistory[] aph = null;
            if (StartState=="-1")
            {
                aph = ProcessHistoryHelper.GetAdviceProcessHistorys(AdviceID);
            }
            else
            {
                aph = ProcessHistoryHelper.GetArticleProcessHistorys(ArticleID);
            }
            int count = aph.Length;
            if (count > 0)
            {
                ViewDataList.DataSource = aph;
                ViewDataList.DataBind();
            }
        }

        public string GetAccountName(string id)
        {
            string name;
            string accountID = "";
            if (StartState=="-1")
            {
                accountID = ProcessHistoryHelper.GetAdviceProcessHistory(ArticleID,id).ProcessAccountID;
            }
            else
            {
                accountID = ProcessHistoryHelper.GetArticleProcessHistory(ArticleID, id).ProcessAccountID;
            }
            if (We7Helper.IsEmptyID(accountID))
                name = "管理员";
            else
                name = AccountHelper.GetAccount(accountID, new string[] { "LastName" }).LastName;
            if (name == null || name == "") name = "未知用户";
            return name;
        }

        public string GetRemark(string id)
        {
            string remark = ""; 
            if (StartState == "-1")
            {
                remark = ProcessHistoryHelper.GetAdviceProcessHistory(AdviceID, id).Remark;
            }
            else
            {
                remark = ProcessHistoryHelper.GetArticleProcessHistory(ArticleID, id).Remark;
            }

            if (remark != "") remark = "，并发表意见：<p>" + remark + "</p>";
            return remark;
        }
    }
}