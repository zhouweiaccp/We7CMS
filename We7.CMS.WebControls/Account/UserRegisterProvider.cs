using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.PF;
using We7.Model.UI.Panel.system;
using System.IO;
using We7.Framework.Util;
using We7.Framework.Config;
using System.Web.UI.WebControls;

namespace We7.CMS.WebControls
{
    public class UserRegisterProvider : BaseWebControl
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
        /// <summary>
        /// 是否进行邮箱验证
        /// </summary>
        public bool EmailValidate { get; set; }

        /// <summary>
        /// 是否使用内容模型
        /// </summary>
        public bool UseModel { get; set; }

        /// <summary>
        /// 是否显示注册协议
        /// </summary>
        public bool ShowProtocol { get; set; }

        /// <summary>
        /// 内容模型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 控件样式
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string AccountID
        {
            get { return ViewState["AccountID"] != null ? ViewState["AccountID"].ToString() : String.Empty; }
            set { ViewState["AccountID"] = value; }
        }

        public bool IsValidate
        {
            get { return !String.IsNullOrEmpty(Request["v"]) && !String.IsNullOrEmpty(Request["v"].Trim()); }
        }

        /// <summary>
        /// 协议
        /// </summary>
        public string Protocol
        {
            get
            {
                if (ViewState["$Protocol"] == null)
                {

                    string path = Server.MapPath(Path.Combine(TemplateSourceDirectory, "Protocol.txt"));
                    if (File.Exists(path))
                    {
                        ViewState["$Protocol"] = File.ReadAllText(path, Encoding.UTF8);
                    }
                    else
                    {
                        ViewState["$Protocol"] = String.Empty;
                    }
                }
                return ViewState["$Protocol"].ToString();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            lblErrorAgreee.Visible = false;
            lblError.Text = "";
            base.OnLoad(e);
            RegisterJs();
            if (!IsPostBack)
            {
                txtProtocol.Text = Protocol;
                if (IsValidate)
                {
                    Validate();
                }
                else
                {
                    mv1.ActiveViewIndex = ShowProtocol ? 0 : 1;
                }
                txtRePassword.Attributes.Add("to", txtPassword.UniqueID);
                txtUser.Attributes.Add("url", this.TemplateSourceDirectory + "/Validate.ashx?action=user&n=" + this.txtUser.UniqueID);
                txtEmail.Attributes.Add("url", this.TemplateSourceDirectory + "/Validate.ashx?action=email&n=" + this.txtEmail.UniqueID);
            }
            LoadModel();
            btnNext.Click += new EventHandler(btnNext_Click);
            bttnRegister.Click += new EventHandler(bttnRegister_Click);
        }

        void RegisterJs()
        {
            IncludeJavaScript("common.js", "validator.js");
        }

        void bttnRegister_Click(object sender, EventArgs e)
        {
            if (mv1.ActiveViewIndex == 1 && ValidateInput())
            {
                try
                {
                    string name = txtUser.Text.Trim();
                    string pwd = txtPassword.Text.Trim();
                    string email = txtEmail.Text.Trim();

                    Account act = new Account();
                    act.Created = DateTime.Now;
                    act.Email = email;
                    act.LoginName = name;
                    act.Password = pwd;
                    act.ModelName = ModelName;
                    act.IsPasswordHashed = false;
                    AccountHelper.AddAccount(act);
                    AccountID = act.ID;
                    if (SendEmail()) //不要模型，需要验证
                    {
                        mv1.ActiveViewIndex = 3;
                        ShowMessage(plValidate);
                    }
                    else if (UseModel) //需要模型
                    {
                        mv1.ActiveViewIndex = 2;
                        LoadModel();
                    }
                    else //不要验证，与不要模型
                    {
                        mv1.ActiveViewIndex = 3;
                        ShowMessage(plSuccess);
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                }
            }
        }

        void LoadModel()
        {
            if (phModelEditor != null && mv1.ActiveViewIndex == 2)
            {
                if (String.IsNullOrEmpty(ModelName) || String.IsNullOrEmpty(ModelName.Trim()))
                {
                    Response.Write("内容模型为空");
                    return;
                }

                SimpleEditorPanel ep = LoadControl("~/ModelUI/Panel/system/SimpleEditorPanel.ascx") as SimpleEditorPanel;
                ep.ModelName = ModelName;
                ep.PanelName = "fedit";
                phModelEditor.Controls.Clear();
                phModelEditor.Controls.Add(ep);
                ep.OnSuccess += new EventHandler(ep_OnSuccess);
            }
        }

        void ep_OnSuccess(object sender, EventArgs e)
        {
            try
            {
                if (SendEmail())
                {
                    ShowMessage(plValidate);
                }
                else
                {
                    ShowMessage(plSuccess);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        bool SendEmail()
        {
            if (EmailValidate &&
               (mv1.ActiveViewIndex == 1 && !UseModel ||
               mv1.ActiveViewIndex == 2))
            {
                MailHelper mailHelper = GetMailHelper();
                Account account = AccountHelper.GetAccount(AccountID, null);
                string subject = "注册系统邮件验证";
                string message = String.Format("亲爱的" + account.LoginName + "，您好！<p/>感谢您在我们网站注册成为会员，故系统自动为你发送了这封邮件。请点击下面链接进行验证：<br /><a href='{0}'>{0}</a>", Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + We7Helper.AddParamToUrl(Request.RawUrl, "v", AccountID));
                mailHelper.Send(account.Email, mailHelper.AdminEmail, subject, message, "Low");
                return true;
            }
            return false;
        }

        bool ValidateInput()
        {
            if (Request.Cookies["CheckCode"] == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('您的浏览器设置已被禁用 Cookies，您必须设置浏览器允许使用 Cookies 选项后才能使用本系统。');</script>");
                return false;
            }
            string name = txtUser.Text.Trim();
            string pwd = txtPassword.Text.Trim();
            string email = txtEmail.Text.Trim();
            string vcode = txtValCode.Text.Trim();
            lblError.Text = "";

            if (String.IsNullOrEmpty(name))
            {
                lblError.Text = "用户名为空";
                return false;
            }
            if (String.IsNullOrEmpty(pwd))
            {
                lblError.Text = "密码为空";
                return false;
            }
            if (String.IsNullOrEmpty(email))
            {
                lblError.Text = "邮件不能为空";
                return false;
            }
            if (String.IsNullOrEmpty(vcode))
            {
                lblError.Text = "验证码不能为空";
                return false;
            }
            if (String.Compare(vcode, Request.Cookies["CheckCode"].Value, true) != 0)
            {
                lblError.Text = "验证码不对";
                return false;
            }
            return true;
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            if (chkAgreen.Checked)
            {
                mv1.ActiveViewIndex = 1;
            }
            else
            {
                lblErrorAgreee.Visible=true;
            }
        }

        void Validate()
        {
            Account act = AccountHelper.GetAccount(Request["v"].Trim(), null);
            if (act != null)
            {
                act.EmailValidate = 1;
                AccountHelper.UpdateAccount(act, new string[] { "EmailValidate" });
                ShowMessage(plValidateSuccess);
            }
            else
            {
                ShowMessage(plValidateError);
            }
        }

        MailHelper GetMailHelper()
        {
            MailHelper mailHelper = new MailHelper();

            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            if (ci != null)
            {
                mailHelper.SmtpServer = ci.SysMailServer;
                mailHelper.AdminEmail = ci.SystemMail;
                mailHelper.UserName = ci.SysMailUser;
                mailHelper.Password = ci.SysMailPassword;
                mailHelper.PopServer = ci.SysPopServer;
            }
            return mailHelper;
        }

        void ShowMessage(System.Web.UI.WebControls.PlaceHolder panel)
        {
            mv1.ActiveViewIndex = 3;
            plSuccess.Visible = plValidate.Visible = plValidateError.Visible = plValidateSuccess.Visible = plMessage.Visible = false;
            panel.Visible = true;
        }

        void ShowMessage(string message)
        {
            mv1.ActiveViewIndex = 3;
            plSuccess.Visible = plValidate.Visible = plValidateError.Visible = plValidateSuccess.Visible = false;
            plMessage.Visible = true;
            plMessage.Controls.Clear();
            plMessage.Controls.Add(new Label() { Text = message });
        }
        #region 控件
        protected global::System.Web.UI.WebControls.MultiView mv1;
        protected global::System.Web.UI.WebControls.MultiView mvMessage;
        protected global::System.Web.UI.WebControls.CheckBox chkAgreen;

        protected global::System.Web.UI.WebControls.Button btnNext;
        protected global::System.Web.UI.WebControls.Button bttnRegister;

        protected global::System.Web.UI.WebControls.PlaceHolder phModelEditor;
        protected global::System.Web.UI.HtmlControls.HtmlInputHidden ID;

        protected global::System.Web.UI.WebControls.PlaceHolder plSuccess;
        protected global::System.Web.UI.WebControls.PlaceHolder plValidate;
        protected global::System.Web.UI.WebControls.PlaceHolder plValidateSuccess;
        protected global::System.Web.UI.WebControls.PlaceHolder plValidateError;
        protected global::System.Web.UI.WebControls.PlaceHolder plMessage;

        protected global::System.Web.UI.WebControls.TextBox txtProtocol;
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.TextBox txtUser;
        protected global::System.Web.UI.WebControls.TextBox txtPassword;
        protected global::System.Web.UI.WebControls.TextBox txtRePassword;
        protected global::System.Web.UI.WebControls.TextBox txtValCode;
        protected global::System.Web.UI.WebControls.Label lblError;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl lblErrorAgreee;

        #endregion
    }
}
