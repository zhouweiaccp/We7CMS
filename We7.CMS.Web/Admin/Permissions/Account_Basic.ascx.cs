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
using System.Xml;
using System.IO;
using System.Collections.Generic;

using Thinkment.Data;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.CMS.Config;
using We7.Model.Core;
using We7.Framework.Config;
using We7.Framework;
using We7.Framework.Util;
using System.Text.RegularExpressions;
using We7.CMS.Accounts;

namespace We7.CMS.Web.Admin.Permissions
{
    public partial class Account_Basic : BaseUserControl
    {
        string CurrentAccountID
        {
            get
            {
                return Request["id"];
            }
        }

        string DepartmentID
        {
            get { return Request["d"]; }
        }

        protected void Initialize()
        {
            ResetPasswordCheckBox.Checked = false;
            ResetPasswordSpan.Visible = false;
            if (We7Helper.IsEmptyID(Security.CurrentAccountID))
            {
                UserTypeDropDownList.Items.Add(new ListItem("管理员", "0"));
            }
            UserTypeDropDownList.Items.Add(new ListItem("普通用户","1"));

            if (We7Helper.IsEmptyID(CurrentAccountID))//新建
            {
                PassWordText.Visible = true;
                We7Helper.AssertNotNull(DepartmentID, "AccountDetail.p");
                if (!We7Helper.IsEmptyID(DepartmentID))
                {
                    Department dpt = AccountHelper.GetDepartment(DepartmentID, new string[] { "FullName" });
                    FullPathLabel.Text = dpt.FullName;
                    ParentTextBox.Text = DepartmentID;
                }
                else
                {
                    ParentTextBox.Text = We7Helper.EmptyGUID;
                }
                MailMessageTemplate mt = new MailMessageTemplate("UserEmailConfig.xml", "新建用户通知");
                MailBodyTextBox.Text = mt.Body;
                SaveButton.Value = "创建账户";
                DeleteButtun.Visible = false;
            }
            else
            {
                ShowAccount(AccountHelper.GetAccount(CurrentAccountID, null));
                ResetPasswordSpan.Visible = true;
                MailMessageTemplate mt = new MailMessageTemplate("UserEmailConfig.xml", "账号审核通过通知");
                MailBodyTextBox.Text = mt.Body;
            }
        }

        /// <summary>
        /// 检测用户名是否有效
        /// </summary>
        string CheckUserName(string userName)
        {
            HttpContext.Current.Response.Clear();
            int length = GetStrLen(userName);
            if (userName == "")
            {
                return "用户名不能为空";
            }
            else if (length < 5 || length > 20)
            {
                return "用户名必须是5-20位";
            }
            else if (AccountHelper.ExistUserName(userName))
            {
                return "该会员名已被使用";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 检测邮箱是否有效
        /// </summary>
        string CheckEmail(string email)
        {
            HttpContext.Current.Response.Clear();
            if (email == "")
            {
                return "Email不能为空";
            }
            else if (!Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                return "Email格式不正确";
            }
            else if (AccountHelper.ExistEmail(email))
            {
                return "该电子邮件名已被使用";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 检测密码是否有效
        /// </summary>
        /// <returns></returns>
        string CheckPWD(string password)
        {
            if (!(password.Length >= 6 && password.Length <= 16))
            {
                return "密码必须在6-16个字符内";
            }
            else
            {
                return "";
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (NameTextBox.Text == SiteConfigs.GetConfig().AdministratorName)
                {
                    Messages.ShowError("无法创建用户！原因：用户名“" + NameTextBox.Text + "”为系统关键字，请换一个名字试试。");
                }
                else
                    SaveAccount();
            }
            catch (Exception ce)
            {
                string msg = ce.Message.Replace(AppDomain.CurrentDomain.BaseDirectory, "/").Replace("\\", "/");
                Messages.ShowError("用户信息保存时出错！出错原因：" + msg);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//是否是演示站点
            AccountHelper.DeleteAccont(CurrentAccountID);
            Response.Redirect("../Departments.aspx");
        }

        /// <summary>
        /// 根据部门ID获取部门名称
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public string GetDepartmentNameByID(string departmentID)
        {
            if (departmentID != null && departmentID != "")
            {
                Department dpt = AccountHelper.GetDepartment(departmentID, new string[] { "FullName" });
                if (dpt != null && dpt.FullName != "")
                {
                    return dpt.FullName;
                }
            }
            return "";
            
        }

        void ShowAccount(Account act)
        {
            if (!We7Helper.IsEmptyID(act.DepartmentID))
            {
                FullPathLabel.Text = GetDepartmentNameByID(act.DepartmentID);
            }
            CreatedLabel.Text = act.Created.ToString();
            DescriptionTextBox.Text = act.Description;
            EmailTextBox.Text = act.Email;
            IDLabel.Text = act.ID.ToString();
            IndexTextBox.Text = act.Index.ToString();
            LastNameTextBox.Text = act.LastName;
            NameTextBox.Text = act.LoginName;

            ParentTextBox.Text = act.DepartmentID.ToString();
            SetDropdownList(UserTypeDropDownList, act.UserType.ToString());
            SetDropdownList(UserStateDropDownList, act.State.ToString());      
            if(!string.IsNullOrEmpty(act.ModelName))
                SetDropdownList(UserModelDropDownList, act.ModelName.ToLower());

            SetDropdownList(ModelStateDropDownList, act.ModelState.ToString());

            AccountIDTextBox.Text = act.ID;

            IsHashedCheckBox.Checked = act.IsPasswordHashed;
            OverdueTextBox.Text = act.Overdue.ToLongDateString();

            NameTextBox.Enabled = false;
            ResetPasswordCheckBox.Visible = true;
        }

        protected void SetDropdownList(DropDownList list, string value)
        {
            int i = 0;
            foreach (ListItem item in list.Items)
            {
                if (item.Value == value)
                {
                    list.SelectedIndex = i;
                    return;
                }
                i++;
            }
        }

        protected void SetRadioButtonList(RadioButtonList list, string value)
        {
            int i = 0;
            foreach (ListItem item in list.Items)
            {
                if (item.Value == value)
                {
                    list.SelectedIndex = i;
                    return;
                }
                i++;
            }
        }

        void SaveAccount()
        {
            bool addModelRole = false;
            Account act = new Account();
            act.ID = IDLabel.Text;
            act.LoginName = NameTextBox.Text;
            act.LastName = LastNameTextBox.Text;
            act.Index = Convert.ToInt32(IndexTextBox.Text);
            act.State = Convert.ToInt32(UserStateDropDownList.SelectedValue);
            act.UserType = Convert.ToInt32(UserTypeDropDownList.SelectedValue);
            act.Description = DescriptionTextBox.Text;
            act.Email = EmailTextBox.Text;
            if (!String.IsNullOrEmpty(UserModelDropDownList.SelectedValue))
            {
                if (File.Exists(ModelHelper.GetModelPath(UserModelDropDownList.SelectedValue)))
                {
                    act.ModelName = UserModelDropDownList.SelectedValue;
                    act.ModelConfig = ModelHelper.GetModelConfigXml(act.ModelName);
                    act.ModelSchema = ModelHelper.GetModelSchema(act.ModelName);
                }
                else
                    throw new Exception(UserModelDropDownList.SelectedValue + " 模型配置文件没有找到！");
            }

            if (act.ModelState != 2 && ModelStateDropDownList.SelectedValue == "2"
                && !String.IsNullOrEmpty(UserModelDropDownList.SelectedValue))
            {
                string moldelStr = UserModelDropDownList.SelectedValue;
                act.ModelState = Int32.Parse(ModelStateDropDownList.SelectedValue);
                addModelRole = true;
                if (SendMailCheckBox.Checked)
                    AccountMails.SendMailOfPassNotify(act, UserModelDropDownList.SelectedItem.Text, MailBodyTextBox.Text);
            }
           
            if (DepartmentIDTextBox.Text != null && DepartmentIDTextBox.Text.Trim() != "")
            {
                act.DepartmentID = DepartmentIDTextBox.Text;
                act.Department= AccountHelper.GetDepartment(act.DepartmentID, new string[] { "Name" }).Name;
            }


            string chkEmail = CheckEmail(this.EmailTextBox.Text.Trim());
            if (act.ID == String.Empty)
            {
                string checkName = CheckUserName(NameTextBox.Text);
                if (checkName == "" && chkEmail == "")
                {
                    if (String.IsNullOrEmpty(PassWordText.Text) || String.IsNullOrEmpty(PassWordText.Text.Trim()))
                    {
                        Messages.ShowError("密码不能为空");
                        return;
                    }
                    act.DepartmentID = ParentTextBox.Text;
                    act.IsPasswordHashed = IsHashedCheckBox.Checked;
                    if (act.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt)
                    {
                        act.Password = Security.Encrypt(PassWordText.Text);
                    }
                    else if (act.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.bbsEncrypt)
                    {
                        act.Password = Security.BbsEncrypt(PassWordText.Text);
                    }
                    else
                    {
                        act.Password = PassWordText.Text;
                    }

                    if (SendMailCheckBox.Checked)
                    {
                        MailHelper mailHelper = AccountMails.GetMailHelper();
                        if (String.IsNullOrEmpty(mailHelper.AdminEmail))
                        {
                            Messages.ShowError("没有配置管理员邮箱。如不需要发送邮箱，请去掉发送邮件选项。");
                            return;
                        }
                        if (String.IsNullOrEmpty(act.Email))
                        {
                            Messages.ShowError("用户邮箱不能为空");
                            return;
                        }
                    }

                    act = AccountHelper.AddAccount(act);
                    ShowAccount(act);

                    if (SendMailCheckBox.Checked)
                        AccountMails.SendMailOfRegister(act, PassWordText.Text, MailBodyTextBox.Text);

                    //记录日志
                    string content = string.Format("创建了帐户“{0}”", act.LoginName);
                    AddLog("新建帐户", content);

                    string rawurl = We7Helper.AddParamToUrl(Request.RawUrl, "saved", "1");
                    rawurl = We7Helper.AddParamToUrl(rawurl, "id", act.ID);
                    Response.Redirect(rawurl);
                }
                else
                {
                    Messages.ShowError("无法注册用户。原因：" + checkName + chkEmail);
                }
            }
            else
            {
                List<string> fields = new List<string>();
                fields.Add("LoginName");
                fields.Add("LastName");
                fields.Add("MiddleName");
                fields.Add("FirstName");
                fields.Add("Index");
                fields.Add("State");
                fields.Add("UserType");
                fields.Add("Description");
                fields.Add("Email");
                fields.Add("ModelName");
                fields.Add("ModelState");
                fields.Add("ModelConfig");
                fields.Add("ModelSchema");
                fields.Add("Overdue");
                if (We7Utils.IsDateString(OverdueTextBox.Text))
                    act.Overdue = DateTime.Parse(OverdueTextBox.Text);
                if (DepartmentIDTextBox.Text != null && DepartmentIDTextBox.Text.Trim() != "")
                {
                    fields.Add("DepartmentID");
                    fields.Add("Department");
                }
                string repassword = "";
                if (ResetPasswordCheckBox.Checked)
                {

                    if (String.IsNullOrEmpty(PasswordTextBox.Text) || String.IsNullOrEmpty(PasswordTextBox.Text.Trim()))
                    {
                        Messages.ShowError("密码不能为空");
                        return;
                    }
                    fields.Add("PasswordHashed");
                    act.IsPasswordHashed = IsHashedCheckBox.Checked;
                    if (act.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt)
                    {
                        act.Password = Security.Encrypt(PasswordTextBox.Text);
                    }
                    else if (act.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.bbsEncrypt)
                    {
                        act.Password = Security.BbsEncrypt(PasswordTextBox.Text);

                    }
                    else
                    {
                        act.Password = PasswordTextBox.Text;
                    }
                    repassword = PasswordTextBox.Text.Trim();//将密码临时保存在session中以便修改BBS数据库使用。

                    fields.Add("Password");
                }
                AccountHelper.UpdateAccount(act, fields.ToArray());
                if (addModelRole)
                    AddAccountModelRole(act);

                Messages.ShowMessage("帐户信息已更新。");
                //记录日志
                string content = string.Format("更新了帐户“{0}”的信息", act.LoginName);
                AddLog("更新帐户", content);

                ResetPasswordCheckBox.Checked = false;
            }
        }

        private void AddAccountModelRole(Account act)
        {
            ModelInfo mi = ModelHelper.GetModelInfoByName(act.ModelName);
            string roleName = "";
            if (!string.IsNullOrEmpty(mi.Parameters))
            {
                string[] p = mi.Parameters.Split(':');
                if (p[0] == "role" && p.Length > 1)
                    roleName = p[1];
            }
            if (!string.IsNullOrEmpty(roleName))
            {
                Role r = AccountHelper.GetRoleBytitle(roleName);
                AccountHelper.AssignAccountRole(act.ID, r.ID);
            }
        }

        void InitUserModelDropDownList()
        {
            UserModelDropDownList.DataSource = ModelHelper.GetContentModel(ModelType.ACCOUNT);
            UserModelDropDownList.DataTextField = "Label";
            UserModelDropDownList.DataValueField = "name";
            UserModelDropDownList.DataBind();
            UserModelDropDownList.Items.Insert(0, new ListItem("----", ""));
            for (int i = 0; i < UserModelDropDownList.Items.Count; i++)
                UserModelDropDownList.Items[i].Value = UserModelDropDownList.Items[i].Value.ToLower();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetPasswordCheckBox.Checked = false;
                if (Request["saved"] != null && Request["saved"].ToString() == "1")
                {
                    Messages.ShowMessage("用户信息已经成功更新。");
                }
                InitUserModelDropDownList();
                Initialize();
            }
        }


        //得到字符长度（一个汉字占两个字符）
        int GetStrLen(String ss)
        {
            Char[] cc = ss.ToCharArray();
            int intLen = ss.Length;
            int i;
            if ("中文".Length == 4)
            {
                //是非 中文 的 平台
                return intLen;
            }
            for (i = 0; i < cc.Length; i++)
            {
                if (Convert.ToInt32(cc[i]) > 255)
                {
                    intLen++;
                }
            }
            return intLen;
        }

    }
}