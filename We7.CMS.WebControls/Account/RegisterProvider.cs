using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using We7.CMS.Config;
using We7.CMS.Common.PF;
using We7.Framework.Util;
using We7.Framework;
using We7.Framework.Config;
using System.Text.RegularExpressions;
using We7.Model.Core;
using We7.CMS.Accounts;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 注册数据提供者
    /// </summary>
    public class RegisterProvider : BaseWebControl
    {

        private bool _isHtml = false;
        /// <summary>
        /// 是否需要静态化
        /// </summary>
        public override bool IsHtml
        {
            get { return false; }
            set { _isHtml = value; }
        }

        string cssClass;
        /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }
        /// <summary>
        /// 注册用户默认角色
        /// </summary>
        public string DefaultRole { get; set; }

        /// <summary>
        /// 是否是商城用户
        /// </summary>
        public bool IsMallUser { get; set; }

        protected global::System.Web.UI.WebControls.MultiView mvRegister;

        protected global::We7.Model.UI.Panel.system.SimpleEditorPanel ucEditor;
        protected global::System.Web.UI.WebControls.Button bttnStep1;
        protected global::System.Web.UI.WebControls.Button bttnRegisger;
        protected global::System.Web.UI.WebControls.Button bttnRegisgerGroup;

        protected global::System.Web.UI.WebControls.CheckBox chkAgree;

        protected global::System.Web.UI.WebControls.TextBox txtProvision;
        protected global::System.Web.UI.WebControls.TextBox txtUserName;
        protected global::System.Web.UI.WebControls.TextBox txtPassword;
        protected global::System.Web.UI.WebControls.TextBox txtRePassword;
        protected global::System.Web.UI.WebControls.TextBox txtValidate;
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.Label lblEmailMsg;
        protected global::System.Web.UI.WebControls.Label lblUserName;
        protected global::System.Web.UI.WebControls.Label lblGroupState;
        protected global::System.Web.UI.WebControls.Label lblGroupMessage;


        protected override void OnLoad(EventArgs e)
        {
         
            
            JavascriptHelper.RegisterForm();
            if (!IsPostBack)
            {
                txtProvision.Text = File.ReadAllText(Server.MapPath("~/We7Controls/Register/Page/Resource/rule.txt"));
            }
            base.OnLoad(e);
            IncludeJavaScript("Register.js");
            ////////////给控件添加客户端事件(start)////////////
            string CheckAllValidate = "";
            //"CheckValidate('" + txtEmail.ClientID + "','" + txtUserName.ClientID + "','" + txtPassword.ClientID + "','" + txtRePassword.ClientID + "','" + bttnRegisger.ClientID + "','" + txtValidate.ClientID + "');";
            txtEmail.Attributes.Add("onBlur", "CheckEmail(this.value,'dvEmail','" + txtEmail.ClientID + "','trEmail');" + CheckAllValidate);
            txtUserName.Attributes.Add("onBlur", "CheckUserName(this.value,'dvUserName','" + txtUserName.ClientID + "','trUserName');" + CheckAllValidate);
            txtPassword.Attributes.Add("onBlur", "CheckPWD(this.value,'dvPassword','" + txtPassword.ClientID + "','trPassword');" + CheckAllValidate);
            txtRePassword.Attributes.Add("onBlur", "CheckRePWD('" + txtPassword.ClientID + "',this.value,'dvRePassword','" + txtRePassword.ClientID + "','trRePassword');" + CheckAllValidate);
            txtValidate.Attributes.Add("onkeyup", "CheckValidateCode(this.value,'dvValidate','" + txtValidate.ClientID + "','trValidate');" + CheckAllValidate);           
            ////////////给控件添加客户端事件(end)//////////// 
            ucEditor.OnSuccess += new EventHandler(ucEditor_OnSuccess);
            ProcessRequest();
        }


        /// <summary>
        /// 处理请求
        /// </summary>
        void ProcessRequest()
        {
            string index = Request["activeIndex"];            
            int intIndex = 0;
            int.TryParse(index, out intIndex);
            string strAccountId = Request[We7.Model.Core.UI.Constants.FEID];
            //请求中用户id是否为空
            if (!string.IsNullOrEmpty(strAccountId))
            {               
                Account accountModel = AccountHelper.GetAccount(strAccountId, new string[] { "LoginName", "ModelState", "EmailValidate", "State" });
                //判断当前用户已经走到了流程中哪一步
                if (accountModel != null)
                {
                    if (accountModel.ModelState == 2)//已申请开源小组成员已通过
                    {
                        intIndex = 5;
                    }
                    else if (accountModel.ModelState == 1)//已申请开源小组成员待审核
                    {
                        intIndex = 4;
                    }
                    else//未申请开源小组成员
                    {
                        if (accountModel.EmailValidate == 1 && accountModel.State == 1)//Email审核已通过
                        {
                            if (intIndex == 3)
                            {
                                intIndex = 3;
                            }
                            else
                            {
                                intIndex = 2;
                                lblUserName.Text = accountModel.LoginName;
                            }
                        }
                        else//Email审核未通过
                        {
                            intIndex = 1;
                            lblEmailMsg.Text = "已经发送邮件到您的邮箱中，请点击其中的链接激活帐号!";
                        }
                    }                   
                }
                else
                {
                    intIndex = 0; 
                }
            }
            else
            {
                intIndex = 0;
            }
            mvRegister.ActiveViewIndex = intIndex;
        }

        /// <summary>
        /// 申请成为开源小组成员完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ucEditor_OnSuccess(object sender, EventArgs e)
        {
            //更新用户升级状态
            
            string strAccountId = Request[We7.Model.Core.UI.Constants.FEID];
            Account account = AccountHelper.GetAccount(strAccountId, null);
            if (account == null)
            {
                mvRegister.ActiveViewIndex = 0;
            }
            else
            {
                account.ModelState = 1;
                account.ModelName = ModelName;
                AccountHelper.UpdateAccount(account, new string[] { "ModelState", "ModelName" });
            }
            SendMail(account, "admin");
            ProcessTransfer("4");
        }

        /// <summary>
        /// 申请成为开源小组成员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        protected void bttnRegisgerGroup_Click(object sender, EventArgs arg)
        {
            ProcessTransfer("3");
        }

        /// <summary>
        /// 处理跳转
        /// </summary>
        /// <param name="index"></param>
        void ProcessTransfer(string index)
        {
            string strAccountId = Request[We7.Model.Core.UI.Constants.FEID];
            string url = We7Helper.AddParamToUrl(Request.RawUrl, We7.Model.Core.UI.Constants.FEID, strAccountId);
            url = We7Helper.AddParamToUrl(url, "activeIndex", index);            
            Response.Redirect(url);      
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

        /// <summary>
        /// 检测用户名是否有效
        /// </summary>
        string CheckUserName()
        {
            string userName = txtUserName.Text.Trim();
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
            //else if (!Regex.IsMatch(userName, @"^[\u4E00-\u9FA5a-zA-Z0-9]+$"))
            //{
            //    return "用户名必须是必须是字母、数字或组合";
            //}
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
        string CheckEmail()
        {
            string email = txtEmail.Text.Trim();
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
                return"该电子邮箱名已被使用";
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
        string CheckPWD()
        {
            string password = txtPassword.Text.Trim();

            if (!(password.Length >= 6 && password.Length <= 16))
            {
                return "密码必须在6-16个字符内";
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 检测确认密码是否有效
        /// </summary>
        /// <returns></returns>
        string CheckRePWD()
        {
            string password = txtPassword.Text.Trim();
            string rePassword = txtRePassword.Text.Trim();
            if (password != rePassword)
            {
                return "密码不匹配";
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 检测验证码是否有效
        /// </summary>
        /// <returns></returns>
        string CheckValidateCode()
        {
            if (String.Compare(txtValidate.Text.Trim(), Request.Cookies["CheckCode"].Value, true) == 0)
            {
                return "";
            }
            else
            {
                return "验证码错误";
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        protected void bttnRegister_Onclick(object sender, EventArgs arg)
        {
            bttnRegisger.Enabled = false;
            StringBuilder checkResult = new StringBuilder("");
            string validateCodeError = CheckValidateCode();
            if (!string.IsNullOrEmpty(validateCodeError))
            {
                checkResult.Append(validateCodeError + "<br/>");
            }
            string emailError = CheckEmail();
            if (!string.IsNullOrEmpty(emailError))
            {
                checkResult.Append(emailError+"<br/>");
            }
            string userNameError = CheckUserName();
            if (!string.IsNullOrEmpty(userNameError))
            {
                checkResult.Append(userNameError + "<br/>");
            }
            string passwordError = CheckPWD();
            if (!string.IsNullOrEmpty(passwordError))
            {
                checkResult.Append(passwordError + "<br/>");
            }
            string rePasswordError = CheckRePWD();
            if (!string.IsNullOrEmpty(rePasswordError))
            {
                checkResult.Append(rePasswordError + "<br/>");
            }

            if (!string.IsNullOrEmpty(checkResult.ToString()))
            {
                lblMsg.Text = "<font color='red'>" + checkResult.ToString() + "</font>";
            }
            else
            {                
                string userName = txtUserName.Text.Trim();
                string password = txtPassword.Text.Trim();
                Account account = new Account();
                account.LoginName = userName;
                account.Password = password;
                account.Email = txtEmail.Text.Trim();
                account.EmailValidate = 0;
                account.Created = DateTime.Now;
                account.Updated = DateTime.Now;
                account.State = 0;
                account.UserType = 1;
                account.PasswordHashed = (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.noneEncrypt;
                try
                {
                    account=AccountHelper.AddAccount(account);
                    if (IsMallUser)
                    {
                        
                    }
                    if (!string.IsNullOrEmpty(DefaultRole))
                    {
                        Role role = AccountHelper.GetRoleBytitle(DefaultRole);
                        if (role != null && !string.IsNullOrEmpty(account.ID))
                            AccountHelper.AssignAccountRole(account.ID, role.ID);
                    }
                    SendMail(account, "user");
                    string strAccountId = Request[We7.Model.Core.UI.Constants.FEID];
                    string url = We7Helper.AddParamToUrl(Request.RawUrl, We7.Model.Core.UI.Constants.FEID, account.ID);
                    url = We7Helper.AddParamToUrl(url, "activeIndex", "1");
                    Response.Redirect(url);        
                }
                catch(Exception ex)
                {
                    lblMsg.Text = "<font color='red'>无法注册用户！原因：" + ex.Message + "</font>";
                }               
            }
            bttnRegisger.Enabled = true;
        }
        void SendMail(Account account, string type)
        {
            try
            {
                if (type == "user")
                {
                    AccountMails.SendMailOfValidate(account, We7.Model.Core.UI.Constants.FEID);
                }
                else
                {
                    ModelInfo model = ModelHelper.GetModelInfoByName(ModelName);
                    AccountMails.SendMailOfHandle(account, model.Label);
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('电子邮件发送失败，请检查邮件设置。原因：" + ex.Message + "')</script>");
            }
        }
        public string ModelName
        {
            get
            {
                return ucEditor != null ? ucEditor.ModelName : String.Empty;
            }
            set
            {
                if (ucEditor != null)
                {
                    ucEditor.ModelName = value;
                }
            }
        }
    }
}