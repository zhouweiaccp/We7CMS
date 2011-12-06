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
using We7.CMS.Helpers;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
namespace We7.CMS.Web.Admin.manage.controls
{
    public partial class TemplateStaticize : System.Web.UI.UserControl
    {
        TemplateStaticizeHelper helper = TemplateStaticizeHelper.Instance();

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (helper.ProcessState)
            {
                case TemplateStaticizeHelper.State.None:
                    bttnGenerate.Enabled = true;
                    bttnGenerate.Text = "　　生成静态页　　";
                    break;
                case TemplateStaticizeHelper.State.Running:
                    bttnGenerate.Enabled = false;
                    bttnGenerate.Text = "正在生成静态页．．．";
                    break;
                case TemplateStaticizeHelper.State.Complated:
                    bttnGenerate.Enabled = true;
                    bttnGenerate.Text = "重新生成静态页";
                    break;
            }

        }

        protected void bttnGenerate_Click(object sender, EventArgs e)
        {
            if (AppCtx.IsDemoSite)
            {
                lblMsg.Text = "<b>演示站点不能生成静态页！</b>";
                return;
            }
            TemplateStaticizeHelper.Instance().Start();
            ShowMsg();
        }

        protected void bttnQuery_Click(object sender, EventArgs e)
        {
            if (AppCtx.IsDemoSite)
            {
                lblMsg.Text = "<b>演示站点不能查询生成进度!</b>";
                return;
            }

            ShowMsg();
        }

        protected void ShowMsg()
        {
            string state = "0%";
            List<TemplateStaticizeHelper.Message> msg = helper.GetMessage(out state);
            StringBuilder sb = new StringBuilder();
            if (helper.ProcessState == TemplateStaticizeHelper.State.Complated)
            {
                sb.Append("<font style='font-weight:bold;font-size:14px;color:#FF5400'>当前进度100%</font><br>");
            }
            else
            {
                sb.AppendFormat("<font style='font-weight:bold;font-size:14px;color:#FF5400'>当前进度{0}</font><br>", state);
            }

            foreach (TemplateStaticizeHelper.Message m in msg)
            {
                sb.AppendFormat("<font class='{0}'>{1}</font><br>", m.Success ? "success" : "error", m.Msg);
            }
            lblMsg.Text = sb.ToString();
        }
    }
}