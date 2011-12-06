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
using We7.CMS.Common.PF;
using System.Text;
using System.Collections.Generic;
using We7.CMS.Accounts;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin.Ajax
{
    public partial class UserSel : System.Web.UI.Page
    {

        public List<Account> AccountItems=new List<Account>();
        /// <summary>
        /// 权限业务对象
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        protected string El
        {
            get
            {
                return Request["el"];
            }
        }

        protected string[] vls;
        protected bool ContainVal(string v)
        {
            if(vls!=null)
            {
                foreach(string s in vls)
                {
                    if(s==v)return true;
                }
            }
            return false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            vls = !String.IsNullOrEmpty(Request["v"]) ? Request["v"].Split(';') : null;
            if (!IsPostBack)
            {
                BindData();               
            }
            
        }

        void BindData()
        {
            string siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
            List<Account> list=AccountHelper.GetAccounts(siteID, null,string.Empty,OwnerRank.All);
            AccountItems =list;
            //dlUser.DataSource = list;
            //dlUser.DataBind();
        }
        public int Columns = 5;
        public int Rows
        {
            get
            {
                int c=AccountItems.Count % Columns;
                return c == 0 && AccountItems.Count > 0 ? AccountItems.Count / Columns : AccountItems.Count / Columns + 1;
            }
        }

        public int LastCount
        {
            get
            {
                int c = AccountItems.Count % Columns;
                return c == 0 && AccountItems.Count > 0 ?Columns : AccountItems.Count%Columns;
            }
        }

        //protected void dlUser_ItemDataBound(object sender, DataListItemEventArgs e)
        //{
        //    CheckBox chkUser=e.Item.FindControl("chkUser") as CheckBox;
        //    if (chkUser != null)
        //    {
        //        chkUser.Text = ((Account)e.Item.DataItem).LastName;
        //        chkUser.Checked = ContainVal(chkUser.Text);
        //    }
        //}

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    StringBuilder sb=new StringBuilder();
        //    foreach (DataListItem item in dlUser.Items)
        //    {
        //        CheckBox chkUser = item.FindControl("chkUser") as CheckBox;
        //        if (chkUser != null)
        //        {
        //            if (chkUser.Checked)
        //                sb.Append(chkUser.Text).Append(";");
        //        }                
        //    }
        //    if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
        //    ClientScript.RegisterClientScriptBlock(GetType(), "returnvalue", "updateValue('" + El + "','" + sb.ToString() + "');window.close();", true);
        //}
    }
}
