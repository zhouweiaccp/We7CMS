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
using System.Collections.Generic;
using We7.Model.Core;
using We7.CMS.Accounts;
using We7.Framework.Config;
using We7.CMS.Common.Enum;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
    public partial class UserList : BasePage
    {
        private PanelContext panelContext;
        public PanelContext PanelContext
        {
            get
            {
                if (panelContext == null)
                    throw new Exception("当前模型信息为Null");
                return panelContext;
            }
            set
            {
                panelContext = value;
            }
        }

        /// <summary>
        /// 当前过滤条件
        /// </summary>
        public QueryType CurrentState
        {
            get
            {
                QueryType s = QueryType.ALL;
                if (Request["state"] != null)
                {
                    if (We7Helper.IsNumber(Request["state"].ToString()))
                        s = (QueryType)int.Parse(Request["state"].ToString());
                }
                return s;
            }
        }

        string keywords
        {
            get
            {
                return Request["keyword"];
            }
        }

        private string siteID;
        public string SiteID
        {
            get
            {
                if (string.IsNullOrEmpty(siteID))
                {
                    siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
                }
                return siteID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                StateLiteral.Text = BuildStateLinks();
                FullPathLabel.Text = BuildPagePath();
            }
            lnkPass.Click += new EventHandler(lnkPass_Click);
            lnkStop.Click += new EventHandler(lnkStop_Click);
            AccountsGridView.RowCommand += new GridViewCommandEventHandler(AccountsGridView_RowCommand);

            CheckMailPlugin();
        }

        /// <summary>
        /// 检测是否安装了批量发邮件插件
        /// </summary>
        private void CheckMailPlugin()
        {
            bool fileexist = System.IO.File.Exists(Server.MapPath("/Plugins/MailPlugin/Plugin.xml"));
            if (!fileexist)
            {
                sendmails.Title = "使用此功能需安装批量发送邮件插件！";
                sendmails.HRef = string.Format("/admin/Plugin/PluginAdd.aspx?tab=0&qtext={0}&qtype=1&ptype=PLUGIN", HttpUtility.UrlEncode("批量发送邮件"));
            }
            else
            {
                bool isinstalled = new PluginInfo(Server.MapPath("/Plugins/MailPlugin/Plugin.xml")).IsInstalled;
                if (!isinstalled)
                {
                    sendmails.Title = "使用此功能需激活批量发送邮件插件！";
                    sendmails.HRef = "/admin/Plugin/PluginList.aspx";
                }
            }
        }

        string BuildPagePath()
        {
            return "<a href='/admin/'>控制台</a> > <a >用户</a> >  <a href='UserExamine.aspx'>会员审核</a>";
        }


        protected void BindData()
		{
			List<Account> list = new List<Account>();
			AccountQuery aq = new AccountQuery();
			aq.KeyWord = keywords;
            aq.SiteID = SiteID;
			aq.UserType = (int)OwnerRank.Normal;

			switch (CurrentState)
			{
				case QueryType.ALL:
					break;
				case QueryType.WaitExamin:
					aq.ModelState = 0;
					break;
				case QueryType.Passed:
					aq.ModelState = 1;
					break;
				case QueryType.WaitValidate:
					aq.EmailValidate = 0;
					break;
			}

			UPager.PageIndex = PageNumber;
			UPager.ItemCount = AccountHelper.QueryAccountCountByQuery(aq);
			UPager.UrlFormat = We7Helper.AddParamToUrl(Request.RawUrl, Keys.QRYSTR_PAGEINDEX, "{0}");
			UPager.PrefixText = "共 " + UPager.MaxPages + "  页 ·   第 " + UPager.PageIndex + "  页 · ";

			list = AccountHelper.QueryAccountsByQuery(aq, UPager.Begin - 1, UPager.Count,
				new string[] { "ID", "LoginName", "Email", "CreatedNoteTime", "EmailValidate", "ModelState", "ModelName", "State", "Created", "UserType", "Department" });

			AccountsGridView.DataSource = list;
			AccountsGridView.DataBind();
		}

        void AccountsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id = e.CommandArgument as string;
            if (!String.IsNullOrEmpty(id))
            {
                Account account = AccountHelper.GetAccount(id, null);
                if (account != null)
                {
                    account.State = account.State == 0 ? 1 : 0;
                    AccountHelper.UpdateAccount(account, new string[] { "State" });
                }
            }
            BindData();
        }

        private int _resultsPageNumber = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageNumber
        {
            get
            {
                if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
                    _resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
                return _resultsPageNumber;
            }
        }

        public enum QueryType { ALL, WaitExamin, Passed, WaitValidate }


        /// <summary>
        /// 构建按类型/状态过滤的超级链接字符串
        /// </summary>
        /// <returns></returns>
        string BuildStateLinks()
        {
            string links = @"<li> <a href='UserList.aspx'   {0} >全部<span class=""count"">({1})</span></a> |</li>
            <li><a href='UserList.aspx?state=2'  {2} >审核通过用户<span class=""count"">({3})</span></a> |</li>
            <li><a href='UserList.aspx?state=1'  {4} >待审核用户<span class=""count"">({5})</span></a> |</li>
            <li><a href='UserList.aspx?state=3'  {6}  >待验证用户<span class=""count"">({7})</span></a></li>";

            string css100, css0, css1, css2;
            css100 = css0 = css1 = css2 = "";
            if (CurrentState == QueryType.ALL) css100 = "class=\"current\"";
            if (CurrentState == QueryType.Passed) css0 = "class=\"current\"";
            if (CurrentState == QueryType.WaitExamin) css1 = "class=\"current\"";
            if (CurrentState == QueryType.WaitValidate) css2 = "class=\"current\"";
            AccountQuery aq = new AccountQuery();
            aq.SiteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
            aq.UserType = (int)OwnerRank.Normal;
            aq.KeyWord = keywords;
            int count = AccountHelper.QueryAccountCountByQuery(aq);
            aq.ModelState = 1;
            int count0 = AccountHelper.QueryAccountCountByQuery(aq);
            aq.ModelState = 0;
            int count1 = AccountHelper.QueryAccountCountByQuery(aq);
            aq.ModelState = 100;
            aq.EmailValidate = 0;
            int count2 = AccountHelper.QueryAccountCountByQuery(aq);
            links = string.Format(links, css100, count, css0, count0, css1, count1, css2, count2);
            return links;
        }

        protected string GetAllState(object objModelState, object objModelName, object objState)
        {
            int state = int.Parse(objState.ToString());
            string summary = "";
            if (objModelState != null)
            {
                string strModelState = objModelState.ToString();
                switch (strModelState)
                {
                    case "0":
                        summary = "";
                        break;

                    case "1":
                        summary += "申请成为 ";
                        break;

                    case "2":
                        summary += "";
                        break;

                    default:
                        summary = "";
                        break;
                }
            }

            if (objModelName != null && objModelName.ToString() != "")
            {
                string strModelName = objModelName.ToString();
                try
                {
                    ModelInfo tempModel = ModelHelper.GetModelInfo(strModelName);
                    summary += tempModel.Label;
                }
                catch (Exception ex)
                {
                    summary += "未知类型";
                }
            }
            if (summary.IndexOf("申请") > 0)
                summary = string.Format("<font style='color:blue'>{0}</font>", summary);

            return summary;
        }

        /// <summary>
        /// 格式化用户升级状态（0-未设置，1-在申请，2-通过）
        /// </summary>
        /// <param name="objModelState">状态编号</param>
        /// <returns>状态名称</returns>
        protected string GetModelState(object objModelState)
        {
            if (objModelState == null)
            {
                return "未申请";
            }
            else
            {
                string strModelState = objModelState.ToString();
                switch (strModelState)
                {
                    case "0":
                        return "未申请";

                    case "1":
                        return "<font style='color:blue'>等待审核</font>";

                    case "2":
                        return "<font style='color:green'>已通过</font>";
                    default:
                        return "未申请";
                }
            }
        }

        protected string GetModelName(object objModelName)
        {
            if (objModelName != null && objModelName.ToString() != "")
            {
                string strModelName = objModelName.ToString();
                try
                {
                    ModelInfo tempModel = ModelHelper.GetModelInfo(strModelName);
                    return tempModel.Label;
                }
                catch (Exception ex)
                {
                    return "未知类型";
                }
            }
            else
            {
                return "";
            }
        }

        void lnkStop_Click(object sender, EventArgs e)
        {
            string ids = Request["ids"];
            int count = 0;
            if (!String.IsNullOrEmpty(ids))
            {
                string[] list = ids.Split(',');
                foreach (String s in list)
                {
                    Account account = AccountHelper.GetAccount(s, null);
                    if (account != null)
                    {
                        account.State = 0;
                        AccountHelper.UpdateAccount(account, new string[] { "State" });
                        count++;
                    }
                }
                BindData();
                Messages.ShowMessage("成功禁用账户 " + count + " 个。");
            }
        }

        void lnkPass_Click(object sender, EventArgs e)
        {
            string ids = Request["ids"];
            int count = 0;
            if (!String.IsNullOrEmpty(ids))
            {
                string[] list = ids.Split(',');
                foreach (String s in list)
                {
                    Account account = AccountHelper.GetAccount(s, null);
                    if (account != null)
                    {
                        account.State = 1;
                        AccountHelper.UpdateAccount(account, new string[] { "State" });
                        count++;
                    }
                }
                BindData();
                Messages.ShowMessage("成功启用账户 " + count + " 个。");
            }
        }
    }
}
