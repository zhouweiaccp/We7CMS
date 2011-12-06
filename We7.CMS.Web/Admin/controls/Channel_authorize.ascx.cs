using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

using We7.CMS;
using We7.CMS.Controls;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.Framework.Config;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin.controls
{
    public partial class Channel_authorize : BaseUserControl
    {

        public string ChannelID
        {
            get { return Request["id"]; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                InitControls();
                LoadRoles();
                LoadUsers();
            }
        }

        protected void SaveButton_ServerClick(object sender, EventArgs e)
        {
            SaveObjectPermissions(RolesGridView,"role");
            SaveObjectPermissions(UsersGridView, "user");

            Messages.ShowMessage("栏目权限已更新。");
        }

        void InitControls()
        {
            //SaveButton2.Attributes["onclick"] = "return channelBasicCheck('" + this.ClientID + "');";
        }

        /// <summary>
        /// 在行数据绑定事件中，判断需要显示的字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RolesGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            BindPermisstionsData(e,"role");
        }

        void LoadRoles()
        {
            string siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
            RolesGridView.DataSource = AccountHelper.GetRoles(siteID, OwnerRank.All, string.Empty);
            RolesGridView.DataBind();
        }

        void LoadUsers()
        {
            string ownerType = "user";
            int typeID = ownerType == "role" ? Constants.OwnerRole : Constants.OwnerAccount;
            List<string> ownerIds = AccountHelper.GetPermissionOwners(typeID, ChannelID);
            List<Account> users = AccountHelper.GetAccountList(ownerIds);

            UsersGridView.DataSource = users;
            UsersGridView.DataBind();
        }

        /// <summary>
        /// 绑定权限数据
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ownerType"></param>
        void BindPermisstionsData(GridViewRowEventArgs e, string ownerType)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                Channel ch = ChannelHelper.GetChannel(ChannelID, null);
                if (ch != null)
                {
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    
                    if (ch.Process != null && ch.Process == "1" && ch.ProcessLayerNO != null)
                    {
                        switch (ch.ProcessLayerNO)
                        {
                            case "1":
                                e.Row.Cells[5].Visible = true;
                                e.Row.Cells[6].Visible = false;
                                e.Row.Cells[7].Visible = false;
                                break;
                            case "2":
                                e.Row.Cells[5].Visible = true;
                                e.Row.Cells[6].Visible = true;
                                e.Row.Cells[7].Visible = false;
                                if (e.Row.RowType == DataControlRowType.Header)
                                {
                                    e.Row.Cells[5].Text = "一审";
                                }
                                break;
                            case "3":
                                e.Row.Cells[5].Visible = true;
                                e.Row.Cells[6].Visible = true;
                                e.Row.Cells[7].Visible = true;
                                if (e.Row.RowType == DataControlRowType.Header)
                                {
                                    e.Row.Cells[5].Text = "一审";
                                }
                                break;
                        }
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        System.Web.UI.HtmlControls.HtmlInputHidden roleIDHidden = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("IDHidden");
                        string roleID = roleIDHidden.Value;
                        CheckBox ChannelInputCheckbox = (CheckBox)e.Row.FindControl("ChannelInputCheckbox");
                        if (ChannelInputCheckbox != null)
                        {
                            if (PermissionIsChecked(ownerType, roleID, ChannelID, "Channel.Input"))
                                ChannelInputCheckbox.Checked = true;
                            else
                                ChannelInputCheckbox.Checked = false;
                        }

                        CheckBox ChannelReadCheckbox = (CheckBox)e.Row.FindControl("ChannelReadCheckbox");
                        if (ChannelReadCheckbox != null)
                        {
                            if (PermissionIsChecked(ownerType, roleID, ChannelID, "Channel.Read"))
                                ChannelReadCheckbox.Checked = true;
                            else
                                ChannelReadCheckbox.Checked = false;
                        }

                        CheckBox ChannelArticleCheckbox = (CheckBox)e.Row.FindControl("ChannelArticleCheckbox");
                        if (ChannelArticleCheckbox != null)
                        {
                            if (PermissionIsChecked(ownerType, roleID, ChannelID, "Channel.Article"))
                                ChannelArticleCheckbox.Checked = true;
                            else
                                ChannelArticleCheckbox.Checked = false;
                        }

                        CheckBox ChannelFirstAuditCheckbox = (CheckBox)e.Row.FindControl("ChannelFirstAuditCheckbox");
                        if (ChannelFirstAuditCheckbox != null)
                        {
                            if (PermissionIsChecked(ownerType, roleID, ChannelID, "Channel.FirstAudit"))
                                ChannelFirstAuditCheckbox.Checked = true;
                            else
                                ChannelFirstAuditCheckbox.Checked = false;
                        }

                        CheckBox ChannelSecondAuditCheckbox = (CheckBox)e.Row.FindControl("ChannelSecondAuditCheckbox");
                        if (ChannelFirstAuditCheckbox != null)
                        {
                            if (PermissionIsChecked(ownerType, roleID, ChannelID, "Channel.SecondAudit"))
                                ChannelSecondAuditCheckbox.Checked = true;
                            else
                                ChannelSecondAuditCheckbox.Checked = false;
                        }
                        CheckBox ChannelThirdAuditCheckbox = (CheckBox)e.Row.FindControl("ChannelThirdAuditCheckbox");
                        if (ChannelThirdAuditCheckbox != null)
                        {
                            if (PermissionIsChecked(ownerType, roleID, ChannelID, "Channel.ThirdAudit"))
                                ChannelThirdAuditCheckbox.Checked = true;
                            else
                                ChannelThirdAuditCheckbox.Checked = false;
                        }

                        CheckBox ChannelAdminCheckbox = (CheckBox)e.Row.FindControl("ChannelAdminCheckbox");
                        if (ChannelAdminCheckbox != null)
                        {
                            if (PermissionIsChecked(ownerType, roleID, ChannelID, "Channel.Admin"))
                                ChannelAdminCheckbox.Checked = true;
                            else
                                ChannelAdminCheckbox.Checked = false;
                        }
                    }
                }
            }
        }

        bool PermissionIsChecked(string ownerType,string ownerID, string objectID, string entityID)
        {
            int typeID = ownerType == "role" ? Constants.OwnerRole : Constants.OwnerAccount;

            List<string> ps = AccountHelper.GetPermissionContents(typeID.ToString(), ownerID, objectID);
            if (ps.Contains(entityID))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 保存角色/用户的权限
        /// </summary>
        /// <param name="objectGridView"></param>
        /// <param name="ownerType"></param>
        void SaveObjectPermissions(GridView objectGridView,string ownerType)
        {
            if (DemoSiteMessage)
            {
                return;
            }

            int typeID = ownerType == "role" ? Constants.OwnerRole : Constants.OwnerAccount;
            for (int i = 0; i < objectGridView.Rows.Count; i++)
            {
                System.Web.UI.HtmlControls.HtmlInputHidden objIDHidden = (System.Web.UI.HtmlControls.HtmlInputHidden)objectGridView.Rows[i].FindControl("IDHidden");
                string ownerID = objIDHidden.Value;

                AccountHelper.DeletePermission(typeID, ownerID, ChannelID);
                ArrayList al = new ArrayList();
                if (((CheckBox)objectGridView.Rows[i].FindControl("ChannelReadCheckbox")).Checked)
                {
                    al.Add("Channel.Read");
                }
                if (((CheckBox)objectGridView.Rows[i].FindControl("ChannelInputCheckbox")).Checked)
                {
                    al.Add("Channel.Input");
                }
                if (((CheckBox)objectGridView.Rows[i].FindControl("ChannelArticleCheckbox")).Checked)
                {
                    al.Add("Channel.Article");
                }
                if (((CheckBox)objectGridView.Rows[i].FindControl("ChannelFirstAuditCheckbox")).Checked)
                {
                    al.Add("Channel.FirstAudit");
                }
                if (((CheckBox)objectGridView.Rows[i].FindControl("ChannelSecondAuditCheckbox")).Checked)
                {
                    al.Add("Channel.SecondAudit");
                }
                if (((CheckBox)objectGridView.Rows[i].FindControl("ChannelThirdAuditCheckbox")).Checked)
                {
                    al.Add("Channel.ThirdAudit");
                }
                if (((CheckBox)objectGridView.Rows[i].FindControl("ChannelAdminCheckbox")).Checked)
                {
                    al.Add("Channel.Admin");
                }

                string[] adds = (string[])al.ToArray(typeof(string));
                AccountHelper.DeletePermission(typeID, ownerID, ChannelID);
                AccountHelper.AddPermission(typeID, ownerID, ChannelID, adds);

                //处理子栏目的权限信息
                ChannelHelper.DeleteChildrenPermission(typeID, ownerID, ChannelID);
                ChannelHelper.AddChildrenPermission(typeID, ownerID, ChannelID, adds);
            }
        }

        protected void UsersGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            BindPermisstionsData(e, "user");
        }

        protected void userAddSubmit_ServerClick(object sender, EventArgs e)
        {
            Account acc = AccountHelper.GetAccountByLoginName(userNameInput.Value);
            if (acc == null)
                Messages.ShowError(string.Format("没有找到用户“{0}”，请输入正确的用户登录名再试。", userNameInput.Value));
            else
            {
                AccountHelper.DeletePermission(Constants.OwnerAccount, acc.ID, ChannelID, new string[] { "Channel.Read" });
                AccountHelper.AddPermission(Constants.OwnerAccount, acc.ID, ChannelID, new string[] { "Channel.Read" });

                //处理子栏目的权限信息
                ChannelHelper.DeleteChildrenPermission(Constants.OwnerAccount, acc.ID, ChannelID, new string[] { "Channel.Read" });
                ChannelHelper.AddChildrenPermission(Constants.OwnerAccount, acc.ID, ChannelID, new string[] { "Channel.Read" });
                LoadUsers();
            }
        }
    }
}