using System;
using We7.CMS.Install;
using We7.CMS.Config;
using We7.Framework.Config;

namespace We7.CMS.Install
{
    public class succeed : SetupPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Init();
        }
        
        protected System.Web.UI.WebControls.Literal SummaryLiteral;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               string summary=@" 恭喜! 您已经成功安装{0}<br /><br />
                                        请您牢记以下您的个人信息<br /><br />
                                      用户名：{1}<br />
                                       密码：{2}<br />";
               if (Request["from"] != null && Request["from"].ToString() == "install")
                   SummaryLiteral.Text = string.Format(summary, SetupPage.producename, Session["SystemAdminName"], Session["SystemAdminPws"]);
               else
                   SummaryLiteral.Text = "恭喜! 您的更新操作已成功完成。";

               ApplicationHelper.ResetApplication();
               SetupPage.DeleteLockFile();
            }
        }
    }
}
