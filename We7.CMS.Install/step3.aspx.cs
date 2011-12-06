using System;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

using We7.CMS.Config;
using Thinkment.Data;
using We7.Framework.Config;

namespace We7.CMS.Install
{
    /// <summary>
    /// setup 的摘要说明. 
    /// </summary>
    public class Step3 : SetupPage
    {
        protected TextBox DatabaseTextBox;
        protected TextBox ServerTextBox;
        protected TextBox UserTextBox;
        protected TextBox PasswordTextBox;

        protected TextBox WebsiteNameTextBox;
        protected Button ResetDBInfo;
        protected Literal msg;

        protected TextBox AdminPasswordTextBox;
        protected TextBox AdminNameTextBox;
        protected TextBox DbFileNameTextBox;
        protected TextBox AdminMailTextBox;
        protected DropDownList DbTypeDropDownList;
        protected CheckBox CreateNewDBCheckBox;
        protected TextBox txtMsg;
        protected Panel ConfigMsgPanel;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (BaseConfigs.ConfigFileExist() && !SetupPage.LockFileExist())
                Response.Redirect("upgrade.aspx", true);
            Init();
        }
        

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //读取默认db.config文件内容
                BaseConfigInfo bci = BaseConfigs.GetBaseConfig();
                if (bci != null)
                {
                    //填充界面
                    DatabaseInfo dbi = new DatabaseInfo();
                    dbi = Installer.GetDatabaseInfo(bci);
                    ServerTextBox.Text = dbi.Server;
                    DatabaseTextBox.Text = dbi.Database;
                    UserTextBox.Text = dbi.User;
                    PasswordTextBox.Text = dbi.Password;
                    DbFileNameTextBox.Text = dbi.DBFile;
                    if (DbFileNameTextBox.Text.IndexOf("\\") > -1)
                        DbFileNameTextBox.Text = DbFileNameTextBox.Text.Substring(DbFileNameTextBox.Text.LastIndexOf("\\") + 1);

                    SelectDB = bci.DBType;
                    CreateNewDBCheckBox.Checked = false;

                    if (!CheckWebConfig())
                    {
                        msg.Visible = true;
                    }

                    SiteConfigInfo si = SiteConfigs.GetConfig();
                    if (si != null)
                    {
                        WebsiteNameTextBox.Text = si.SiteName;
                        AdminNameTextBox.Text = si.AdministratorName;
                    }
                }

                AdminPasswordTextBox.Attributes.Add("onkeyup", "return loadinputcontext(this);");
            }
        }

 
        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.ResetDBInfo.Click += new System.EventHandler(this.ResetDBInfo_Click);
            //this.upgrade.Click += new System.EventHandler(this.upgrade_Click);
            this.Load += new System.EventHandler(this.Page_Load);

            //this.upgrade.Text = "从" + upgradeproductname + "升级";

        }
        #endregion

        #region web.config检查
        //判断web.config文件中的设置是否正确
        public bool CheckWebConfig()
        {
            string webconfigpath = Path.Combine(Request.PhysicalApplicationPath, "web.config");

            //如果文件不存在退出
            if (!File.Exists(webconfigpath) && (!File.Exists(Server.MapPath("../web.config"))))
            {
                return false;
            }

            return true;
        }

        #endregion

        string Encrypt(string password)
        {
            // Force the string to lower case
            //
            password = password.ToLower();

            Byte[] clearBytes = new UnicodeEncoding().GetBytes(password);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

            return BitConverter.ToString(hashedBytes);
        }

        protected void ResetDBInfo_Click(object sender, EventArgs e)
        {

            #region 验证输入
            //验证密码长度
            if (AdminPasswordTextBox.Text.Length < 6)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('系统管理员密码长度不能少于6位');</script>");
                return;
            }
            //验证数据库名为空
            if (DatabaseTextBox.Text.Length == 0 && DbTypeDropDownList.SelectedValue == "SqlServer")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('数据库名不能为空');</script>");
                return;
            }

            //验证必须选择数据库类型
            if (DbTypeDropDownList.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('请选择数据库类型');</script>");
                return;
            }

            #endregion

            #region 写Site.config文件
            try
            {
                SiteConfigInfo __configinfo;
                //记录存储到SiteConfig.config
                try
                {
                    __configinfo = SiteConfigs.GetConfig();
                }
                catch
                {
                    __configinfo = new SiteConfigInfo();
                }
                __configinfo.AdministratorKey = Encrypt(AdminPasswordTextBox.Text); 
                __configinfo.IsPasswordHashed = true;
                __configinfo.SiteName = WebsiteNameTextBox.Text;
                __configinfo.AdministratorName = AdminNameTextBox.Text;
                SiteConfigs.Serialiaze(__configinfo,Server.MapPath("~/config/site.config"));
                Session["SystemAdminName"] = AdminNameTextBox.Text;
                Session["SystemAdminPws"] = AdminPasswordTextBox.Text;
            }
            catch { ;}
            #endregion

             string setupDbType =SelectDB= DbTypeDropDownList.SelectedValue;

            DatabaseInfo dbi=new DatabaseInfo();
            dbi.Server=ServerTextBox.Text;
            dbi.Database=DatabaseTextBox.Text;
            dbi.User=UserTextBox.Text;
            dbi.Password=PasswordTextBox.Text;
            dbi.DBFile=DbFileNameTextBox.Text;

            BaseConfigInfo baseConfig = Installer.GenerateConnectionString(setupDbType, dbi);

            //验证链接

            if (!SaveDBConfig(baseConfig))
            {
                ResetDBInfo.Enabled = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>if(confirm('无法把设置写入\"db.config\"文件, 系统将把文件内容显示出来, 您可以将内容保存为\"db.config\", 然后通过FTP软件上传到网站根目录下.. \\r\\n*注意: db.config位于网站Config目录。\\r\\n\\r\\n如果要继续运行安装, 请点击\"确定\"按钮. ')) {window.location.href='step4.aspx?isforceload=1';}else{window.location.href='step3.aspx';}</script>");
                return;
            }

            //下面数据库需要手工创建
            if (baseConfig.DBType == "Oracle" || baseConfig.DBType == "MySql")
                CreateNewDBCheckBox.Checked = false;

            if (CreateNewDBCheckBox.Checked)//创建数据库
            {
                Exception ex = null;
                int ret = Installer.CreateDatabase(baseConfig,out ex);
                if (ret == -1)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('数据库已存在，请重新命名或去掉重新“创建新数据库”前面的勾，使用已有数据库。');</script>");
                    return;
                }
                else if (ret == 0)
                {
                    string exceptionMsgs = We7Helper.ConvertTextToHtml(ex.Message);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('创建数据库发生错误。错误原因：" + exceptionMsgs+ "');</script>");
                    return;
                }
            }

            string msg = "";
            if (!Installer.CheckConnection(baseConfig,out msg))
            {
                msg = We7Helper.ConvertTextToHtml(msg);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('连接数据库失败,请检查您填写的数据库信息。" + msg+ "');</script>");
                return;
            }
            else
            {
                Response.Redirect(Server.HtmlEncode(String.Format("step4.aspx?db={0}", setupDbType)));
            }
        }

        private bool SaveDBConfig(BaseConfigInfo bci)
        {
            try
            {
                string file = Server.MapPath("~/config/db.config");
                BaseConfigs.SaveConfigTo(bci,file);
                BaseConfigs.ResetConfig();
                SetupPage.CreateLockFile();
                return true;
            }
            catch
            {
                ErrProcess(bci);
            }
            return false;
        }

        private void ErrProcess(BaseConfigInfo config)
        {
            string MyDBConfig = "<?xml version=\"1.0\"?>\r\n";
            MyDBConfig += "<BaseConfigInfo xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n";
            MyDBConfig += "<DBConnectionString>" + config.DBConnectionString + "</DBConnectionString>\r\n";
            MyDBConfig += "<DBDriver>" + config.DBDriver + "</DBDriver>\r\n";
            MyDBConfig += "<DBType>" + config.DBType + "</DBType>\r\n";
            MyDBConfig += "</BaseConfigInfo>\r\n";
            txtMsg.Text = MyDBConfig;
            ConfigMsgPanel.Visible = true;
        }
    }
}