using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.Framework;
using We7.CMS.Accounts;

namespace We7.CMS.Web
{
    public partial class Errors : Page
    {

        string ErrorCodeID
        {
            get { return Request["id"]; }
        }

        string ErrorType
        {
            get { return Request["t"]; }
        }

        protected string BackUrl
        {
            get
            {                
                return Context.Request.ServerVariables["HTTP_REFERER"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (ErrorType != null && ErrorType.ToLower() == "permission")
                {
                    TitleLabel.Text = "没有访问权限！";
                    MessageLabel.Text = "您没有被授权访问本页面，如果想获得更多的访问权限，您需要向网站管理员申请。";
                    HelpHyperLink.Visible = false;
                }
                else if (ErrorType != null && ErrorType.ToLower() == "notemplate")
                {
                    TitleLabel.Text = "没有找到对应模板！";
                    MessageLabel.Text = "您可能还没有为本页面指定相应的模板，请到后台模板组编辑也中进行相应设置。";
                    HelpHyperLink.Visible = false;
                }
                else
                {
                    TitleLabel.Text = "对不起，系统运行出现点小问题！";
                    MessageLabel.Text = GetErrorsMessage();
                    HelpHyperLink.Visible = false;
                }
                if (Request.IsAuthenticated && Security.CurrentAccountID == We7Helper.EmptyGUID)
                    DetailInfo.Visible = true;
                else
                    DetailInfo.Visible = false;
            }
        }

        string GetErrorsMessage()
        {
            string noExceptionFound = "系统没有提供更多错误信息。";
            StringBuilder exceptionMsgs = new StringBuilder();
            string logContent = "";
            
            if (Server.GetLastError() != null)
            {
                Exception ex = Server.GetLastError().GetBaseException();
                while (null != ex) // this is obsolete since we're grabbing base...
                {
                    if (ex is System.IO.FileNotFoundException)
                    {
                        exceptionMsgs.Append("<p>您请求的资源没有找到。</p>");
                    }
                    else
                    {
                        exceptionMsgs.AppendFormat("<p>{0}</p>", We7Helper.ConvertTextToHtml(ex.Message.Trim()));
                        logContent += "错误源：" + ex.Source + "\r\n";
                        logContent += "错误信息：" + ex.Message + "\r\n";
                        logContent += "堆栈信息：" + "\r\n" + ex.StackTrace + "\r\n";
                    }
                    ex = ex.InnerException;
                }
            }
            else
                exceptionMsgs.Append("<p>未知错误。如反复出现，请及时与产品开发商联系。非常感谢！</p>");

            Server.ClearError();

            if (exceptionMsgs.Length == 0)
            {
                exceptionMsgs.Append(noExceptionFound);
            }

            if (logContent.Length > 0)
            {
                logContent = "访问页面：" + Request.RawUrl + "\r\n" + logContent;
                logContent = "站点IP：" + Request.ServerVariables.Get("Local_Addr").ToString() + "\r\n" + logContent;
                Logger.Error(logContent);
                ViewLogHyperlink.Text = "查看详细错误日志";
                ViewLogHyperlink.NavigateUrl = Logger.GetFileName();
                ViewLogHyperlink.Target = "_blank";
            }

            return exceptionMsgs.ToString();
        }
    }
}
