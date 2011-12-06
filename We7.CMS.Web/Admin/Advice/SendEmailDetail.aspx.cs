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
using System.Collections.Generic;
using System.IO;
using System.Xml;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
namespace We7.CMS.Web.Admin
{
    public partial class SendEmailDetail : BasePage
    {
        public string FileName
        {
            get
            {
                if (Request["fileName"] != null)
                    return We7Helper.Base64Decode(Request["fileName"]);
                return string.Empty;
            }
        }
        public bool IsFromDepartment
        {
            get
            {
                return Request["from"] != null && Request["from"].ToString() == "depart";
            }
        }

        public bool HavePermission
        {
            get
            {
                if (We7Helper.IsEmptyID(AccountID))
                    return true;
                else
                {
                    Account a = AccountHelper.GetAccount(AccountID, null);
                    if (a != null)
                        return true;
                    else
                        return false;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PagePathLiteral.Text = BuildPagePath();
                LoadErrorEmailInfo();
                BindReplayList();
            }
            catch (Exception ex)
            {
                //ReplayContent.InnerHtml = "邮件异常。";
            }
        }

        /// <summary>
        /// 页面加载信息
        /// </summary>
        void LoadErrorEmailInfo()
        {
            //TitleTextBox.Text = Title;

                string root = Server.MapPath("/_Data/SendEmail/" + FileName);
                if (File.Exists(root))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(root);
                    XmlNode node = doc.SelectSingleNode("/root/infoBody");
                    if (node != null)
                        ReplayContent.InnerHtml = We7Helper.Base64Decode(node.InnerText);
                    node = doc.SelectSingleNode("/root/infoUser");
                    if (node != null)
                        UserLabel.Text = We7Helper.Base64Decode(node.InnerText);
                    node = doc.SelectSingleNode("/root/infoFormUser");
                    if (node != null)
                        FormUserLabel.Text = We7Helper.Base64Decode(node.InnerText);
                    node = doc.SelectSingleNode("/root/infoTime");
                    if(node != null)
                        EmailTimeLabel.Text = node.InnerText;
                    node = doc.SelectSingleNode("/root/infoSubject");
                    if (node != null)
                        TitleTextBox.Text = EmailTitleLabel.Text = We7Helper.Base64Decode(node.InnerText);
                }
        }

        /// <summary>
        /// 构建当前位置导航
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "开始 > <a >设置</a> >  <a href=\"../Advice/AdviceProcessManage.aspx\" >反馈监控管理</a> >  <a>【"
                        + FileName + "】错误邮件详细信息</a>";
            return pos;
        }

        void BindReplayList()
        {
            if (!HavePermission)
            {
                Response.Write("您没有权限访问此信息。");
                Response.End();
            }
        }
    }
}
