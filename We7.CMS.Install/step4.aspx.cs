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
    public class Step4 : SetupPage
    {
        protected System.Web.UI.WebControls.Button ResetDBInfo;
        protected System.Web.UI.WebControls.Button PrevPage;
       
        string LogFile = "";

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (SetupPage.LockFileExist() && BaseConfigs.ConfigFileExist())
                Init();
            else
                Response.Redirect("upgrade.aspx", true);
        }
        

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                 DisableSubmitBotton(this, this.ResetDBInfo);
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
            Response.Redirect("step3.aspx");
        }

        private void ResetDBInfo_Click(object sender, EventArgs e)
        {
            //读取默认db.config文件内容
            BaseConfigInfo bci = BaseConfigs.GetBaseConfig();
            if (bci.DBType != "" && bci.DBConnectionString !="")
            { 
                Installer.ExcuteSQLGroup(bci);
                this.Response.Redirect("succeed.aspx?from=install");
            }
        }

      
    }
}