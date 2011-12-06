using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Xml;

using We7.CMS.Config;
using Thinkment.Data;

namespace We7.CMS.Install
{
    /// <summary>
    /// setup4 的摘要说明. 
    /// </summary>
    public class upgrade_db : SetupPage
    {
        protected System.Web.UI.WebControls.Button ResetDBInfo;
        protected System.Web.UI.WebControls.Button PrevPage;
        protected System.Web.UI.WebControls.Panel SummaryPanel;
        protected System.Web.UI.WebControls.CheckBox GenerateConfigCheckbox;
        protected System.Web.UI.WebControls.Literal SummaryLiteral;

        BaseConfigInfo DBConfig
        {
            get { return (BaseConfigInfo)ViewState["We7$DBConfig"]; }
            set { ViewState["We7$DBConfig"] = value; }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (Request["from"] == null || Request["from"].ToString() != "upgrade.aspx")
            {
                if (BaseConfigs.ConfigFileExist())
                    Response.Redirect("upgrade.aspx", true);
            }
            Init();
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                 DisableSubmitBotton(this, this.ResetDBInfo);
                 if (NoConfig())
                 {
                     SummaryPanel.Visible = true;
                     DBConfig = LoadOldXMLConfig();
                     string summ = "<span style='color:red'>没有找到新版系统的数据库配置文件。</span>";
                     if (DBConfig!=null && DBConfig.DBConnectionString != "" && DBConfig.DBType != "")
                     {
                         summ += " 但发现旧版系统中的配置信息如下：<br>数据库类型{0}，数据库连接串 {1} <br>";
                         summ = string.Format(summ, DBConfig.DBType, DBConfig.DBConnectionString);
                         summ += "<br><span style='color:red'>您是否要使用原有数据库配置创建新的数据库配置文件？</span>";
                         GenerateConfigCheckbox.Text = "是的，我要用上面信息建立新的配置文件";
                         GenerateConfigCheckbox.Visible = true;
                     }
                     SummaryLiteral.Text = summ;
                 }
                
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
            this.ResetDBInfo.Click += new EventHandler(this.ResetDBInfo_Click);
            this.PrevPage.Click += new EventHandler(this.PrevPage_Click);
            this.Load += new EventHandler(this.Page_Load);

            HttpContext context = HttpContext.Current;
            if (context.Request["isforceload"] == "1")
            {
                ResetDBInfo.Enabled = false;
            }
        }
        #endregion

        protected void PrevPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("upgrade.aspx");
        }

        /// <summary>
        /// 确保_Data目录存在
        /// </summary>
        private void CreateDataLogPath()
        {
            string _dataLogPath = Server.MapPath("~/_data");
            if (!Directory.Exists(_dataLogPath))
                Directory.CreateDirectory(_dataLogPath);
        }

        private void ResetDBInfo_Click(object sender, EventArgs e)
        {
            CreateDataLogPath();

            if (GenerateConfigCheckbox.Checked)
            {
                string configPath = Server.MapPath("~/config/db.config");
                BaseConfigs.SaveConfigTo(DBConfig, configPath);
                BaseConfigs.ResetConfig();
            }

            //读取默认db.config文件内容
            BaseConfigInfo bci = BaseConfigs.GetBaseConfig();
            if (bci!=null && bci.DBType != "" && bci.DBConnectionString != "")
            { 
                
                
                Installer.ExcuteSQLGroup(bci);
                this.Response.Redirect("succeed.aspx");
            }
            else
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('无法读取数据库配置文件db.config，请检查重试。');</script>");

        }

        bool NoConfig()
        {
            string configPath = Server.MapPath("~/config/db.config");
            return (!File.Exists(configPath));
        }

        BaseConfigInfo LoadOldXMLConfig()
        {
            string dbPath = Server.MapPath("~/app_data/db/");
            ObjectAssistant oa = new ObjectAssistant();
            if (File.Exists(Path.Combine(dbPath, "cd.xml")))
            {
                oa.LoadOldDBConfig(Path.Combine(dbPath, "cd.xml"));
                Dictionary<string, IDatabase> databases = oa.GetDatabases();
                BaseConfigInfo bci = new BaseConfigInfo();
                bci.DBConnectionString = databases["We7.CMS.Common"].ConnectionString;
                bci.DBConnectionString = bci.DBConnectionString.ToLower().Replace(AppDomain.CurrentDomain.BaseDirectory.ToLower(), "{$App}\\");
                bci.DBDriver = databases["We7.CMS.Common"].DbDriver.ToString();
                bci.DBType = Installer.GetDBTypeFromDriver(databases["We7.CMS.Common"].DbDriver.ToString());
                return bci;
            }
            return null; 
        }
      
    }
}