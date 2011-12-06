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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using We7.CMS.Common;
using We7.Framework;
using We7.CMS.Accounts;
using We7.CMS.Common.PF;


namespace We7.CMS.Web.Admin
{
    public partial class AdviceTypes :BasePage
    {
        MenuHelper MenuHelper
        {
            get { return HelperFactory.GetHelper<MenuHelper>(); }
        }

        private string[] GetIDs(string ids)
        {
            if (ids.Length > 0)
            {
                char[] sp ={ ',' };
                return ids.Split(sp);
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                Bind();
            }
        }
        /// <summary>
        /// 当前过滤条件，反馈模型状态
        /// </summary>
        protected  int CurrentState
        {
            get
            {
                if (Request["state"] != null)
                {
                    if (We7Helper.IsNumber(Request["state"].ToString()))
                     return Convert.ToInt32(StateMgr.StateProcess(Request["state"].ToString(), EnumLibrary.Business.AdviceMode, int.Parse(Request["state"].ToString())));
                }
                return 0;
            }
        }

        protected void Pager_Fired(object sender, EventArgs e)
        {
            Bind();
        }
        private void Bind()
        {
            string selectMemberName = SearchTextBox.Text.Trim();
            string selectAccountName = AccountTextBox.Text.Trim();
            bool accountName = false;
            List<string> ids = new List<string>();
            if (selectAccountName != null && selectAccountName != "")
            {
                Account a = AccountHelper.GetAccountByLoginName(selectAccountName);
                if (a != null) ids.Add(a.ID);
                int i = selectAccountName.IndexOf("管理员");
                if (i >= 0)
                {
                    ids.Add("{00000000-0000-0000-0000-000000000000}");
                }
                accountName = true;
            }

            if (AdvicePager.Count < 0)
            {
                AdvicePager.PageIndex = 0;
            }
            AdvicePager.FreshMyself();
            List<AdviceType> adviceTypeList = new List<AdviceType>();
            AdvicePager.RecorderCount = AdviceTypeHelper.GetAdviceTypeCountByName(selectMemberName, ids, accountName);
            if (AdvicePager.RecorderCount < 1)
            {
                Messages.ShowMessage("您还没有添加任何反馈模型");
                DataGridView.DataSource = adviceTypeList;
                DataGridView.DataBind();
                return;
            }

            adviceTypeList = AdviceTypeHelper.SearchAdviceTypeByName(selectMemberName, ids, accountName, AdvicePager.Begin, AdvicePager.PageSize);
            if (adviceTypeList.Count < 1)
            {
                Messages.ShowMessage("您还没有添加任何反馈模型");
                return;
            }
            DataGridView.DataSource = adviceTypeList;
            DataGridView.DataBind();
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//是否是演示站点
            List<string> list = GetIDs();

            if (list.Count < 1)
            {
                Messages.ShowMessage("没有选择任何一条记录!");
                return;
            }
            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (string id in list)
            {
                sb.Append(id);
                count++;
                if (count != list.Count)
                {
                    sb.Append(",");
                }
            }

            string[] ids = GetIDs(sb.ToString());
            DeleteTypeList(ids);
            Bind();
            Messages.ShowMessage("您已经成功删除" + ids.Length+ "条反馈模型，同时对应的菜单已经删除，你需要退出系统重新登陆");
        }

        void DeleteTypeList(string[] ids)
        {
            List<Advice> list = new List<Advice>();
            string aTitle = "";
            foreach (string id in ids)
            {
                list = AdviceHelper.GetAdvices(id);
                foreach (Advice a in list)
                {
                    AdviceHelper.DeleteAdvice(a.ID);
                    aTitle += String.Format("{0};", a.Title);
                }
            }
            //记录日志
            string content = string.Format("删除了{0}个反馈模型:“{1}”", ids.Length.ToString(), aTitle);
            AddLog("反馈模型管理", content);
            string titles = "";
            List<string> listString = new List<string>();
            foreach (string id in ids)
            {
                AdviceType at = AdviceTypeHelper.GetAdviceType(id);
                AdviceTypeHelper.DeleteAdviceType(id);
                titles += String.Format("{0};", at.Title);
                string menuName = at.Title + "管理";
                We7.CMS.Common.MenuItem menuItem = MenuHelper.GetMenuItemByTitle(menuName);
                if(menuItem !=null)
                {
                    listString.Add(menuItem.ID);
                }
            }
            if (listString != null)
            {
                MenuHelper.DeleteMenuItem(listString);
            }
            //记录日志
            string atContent = string.Format("删除了{0}个反馈模型:“{1}”", ids.Length.ToString(), titles);
            AddLog("反馈模型管理", atContent);

        }

        private List<string> GetIDs()
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
        /// <summary>
        /// 根据模型ID查询用户名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetAccountNameByAdviceTypeID(string id)
        {
            if (id == null || id == "") return "";

            AdviceType adviceType = AdviceTypeHelper.GetAdviceType(id);
            if (adviceType == null || adviceType.AccountID == null || adviceType.AccountID == "")
                return "匿名用户";
            else if (adviceType.AccountID == "{00000000-0000-0000-0000-000000000000}")
            {
                return "管理员";
            }
            else
            {
                return AccountHelper.GetAccount(adviceType.AccountID, new string[] { "LoginName" }).LoginName;
            }
        }

        List<MenuEx> GetMenuEx()
        {
            List<MenuEx> menuExList = new List<MenuEx>();
            for (int i = 0; i < DataGridView.Rows.Count; i++)
            {
                MenuEx menuEx = new MenuEx();
                if (((CheckBox)DataGridView.Rows[i].FindControl("chkItem")).Checked)
                {
                    string id = ((Label)(DataGridView.Rows[i].FindControl("lblID"))).Text;
                    string name = ((TextBox)(DataGridView.Rows[i].FindControl("MenuNameTextBox"))).Text;
                    menuEx.ID = id;
                    menuEx.MenuName = name;
                    menuExList.Add(menuEx);
                }
            }
            return menuExList;
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                List<MenuEx> emnuString = new List<MenuEx>();
                emnuString = GetMenuEx();
                if (emnuString == null)
                {
                    Messages.ShowMessage("您没选择任何记录");
                    return;
                }
                else
                {
                    foreach (MenuEx menuEx in emnuString)
                    {
                        if (MenuHelper.ExistMenuItem(menuEx.MenuName, null))
                        { }
                        else
                        {

                            MenuHelper.AddAdivcieMenu(menuEx.MenuName, menuEx.ID, "{00000000-0000-0004-0000-000000000000}");
                            count++;
                        }
                    }
                    Messages.ShowMessage(string.Format("您已经成功创建{0个菜单项，位于主菜单<反馈>下，请重新登录后查看。", count));
                }
            }
            catch (Exception)
            {
                Messages.ShowError("无法创建菜单，请到<菜单维护>中进行管理与维护。");
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void QueryButton_ServerClick(object sender, EventArgs e)
        {
            Bind();
        }

        [Serializable]
        public class MenuEx
        {
            private string id = "";
            public string ID
            {
                get { return id; }
                set { id = value; }
            }
            private string menuName = "";
            public string MenuName
            {
                get { return menuName; }
                set { menuName = value; }
            }
        }
    }
}
