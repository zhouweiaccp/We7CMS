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

using We7.CMS.Controls;
using System.IO;
using System.Xml;
using System.Reflection;
using We7.CMS.Common;
using We7.CMS.Common.PF;
using We7.Framework;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.controls
{
    public partial class AdviceReplyEmailControl : BaseUserControl
    {
        public string AdviceTypeID
        {
            get
            {
                return Request["adviceTypeID"];
            }
        }

        long sn;
        AdviceQuery query = null;
        AdviceQuery CurrentQuery
        {
            get
            {
                if (query == null)
                {
                    query = new AdviceQuery();
                    query.AccountID = AccountID;
                    query.SN = sn;
                }
                return query;
            }
        }

        public string GetAdviceIDBySN(long sn)
        {
            int from = 0;
            int count = 1;
            string adviceID = "";
            List<Advice> advice = AdviceHelper.GetAdviceByQuery(CurrentQuery, from, count);
            for (int i = 0; i < advice.Count; i++)
            {
                if (advice[i] != null)
                {
                    adviceID = advice[i].TypeID;
                }
            }
            return adviceID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAdvices();
            }
        }

        protected void Pager_Fired(object sender, EventArgs e)
        {
            LoadAdvices();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //this.Master.SiteHeadTitle = SiteHeadTitle;
            //this.Master.TitleName = "文章流转管理";
        }

        /// <summary>
        /// 根据回复邮箱获取回复用户名称
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetAdviceReplyUser(string email)
        {
            Account account = AccountHelper.GetAccountByEmail(email);
            if (account != null)
            {
                return account.LastName;
            }
            else
            {
                return email;
            }
        }

        /// <summary>
        /// 页面加载xml数据
        /// </summary>
        void LoadAdvices()
        {
            List<AdviceReply> adviceReply = new List<AdviceReply>();
            string root = Server.MapPath("/_Data/ErrorEmail/");
            if (Directory.Exists(root))
            {
                DirectoryInfo dir = new DirectoryInfo(root);

                FileInfo[] files = dir.GetFiles("*.xml");

                for (int i = files.Length-1; i >= 0; i--)
                {
                    FileInfo file = files[i];
                    AdviceReply ar = new AdviceReply();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(file.FullName);
                    XmlNode node = xmlDoc.SelectSingleNode("/root/infoSubject");
                    ar.Title = We7Helper.Base64Decode(node.InnerText);
                    node = xmlDoc.SelectSingleNode("/root/infoBody");
                    ar.Content = We7Helper.Base64Decode(node.InnerText);
                    node = xmlDoc.SelectSingleNode("/root/infoUser");
                    ar.UserEmail = We7Helper.Base64Decode(node.InnerText);
                    node = xmlDoc.SelectSingleNode("/root/infoTime");
                    ar.CreateDate = Convert.ToDateTime(node.InnerText);
                    ar.MailFile = file.Name;
                    adviceReply.Add(ar);
                }
            }
            if (adviceReply != null)
            { Pager.RecorderCount = adviceReply.Count; }
            else
            {
                Pager.RecorderCount = 0;
            }
            if (Pager.Count < 0)
                Pager.PageIndex = 0;
            Pager.FreshMyself();
            if (Pager.Count <= 0)
            {
                DataGridView.DataSource = null;
                DataGridView.DataBind();
                return;
            }
            DataGridView.DataSource = adviceReply.GetRange(Pager.Begin, Pager.Count);
            DataGridView.DataBind();
        }

        void InsertArticleProcessHistory(string id)
        {
            Processing ap = ArticleProcessHelper.GetArticleProcess(id);
            ProcessHistory aph = new ProcessHistory();
            //aph.ID = ap.ID;
            aph.ObjectID = ap.ObjectID;
            aph.ToProcessState = "-1";
            aph.ProcessDirection = ap.ProcessDirection;
            aph.ProcessAccountID = ap.ProcessAccountID;
            aph.Remark = ap.Remark;
            aph.CreateDate = DateTime.Now;
            aph.UpdateDate = DateTime.Now;
            ArticleProcessHistoryHelper.InsertAdviceProcessHistory(aph);
        }

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            string xmlTitle = xmlTitleText.Text;
            string root = Server.MapPath("/_Data/ErrorEmail/");
            if (xmlTitle.Trim() != "")
            {
                
                File.Delete(Path.Combine(root, xmlTitle));
                Messages.ShowMessage("您已经成功删除【" + xmlTitle + "】邮件。");
            }
            else
            {
                List<string> list = GetIDs();
                if (list.Count < 1)
                {
                    Messages.ShowMessage("您没有选择任何一条记录！");
                    return;
                }
                foreach(string title in list)
                {
                    File.Delete(Path.Combine(root, title));
                }
                Messages.ShowMessage(string.Format("您已经成功删除了{0}封邮件", list.Count.ToString()));
            }
            LoadAdvices();
        }

        /// <summary>
        /// 取消流转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UntreadBtn_Click(object sender, EventArgs e)
        {

            string message = string.Format("您已经成功将{0}条记录取消流转");
            Messages.ShowMessage(message);
            LoadAdvices();
        }

        protected void ReceiveBtn_Click(object sender, EventArgs e)
        {
            if (AdviceTypeID != null && AdviceTypeID != "")
            {
                AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
                try
                {
                    MailHelper mailHelper = AdviceHelper.GetMailHelper(adviceType);
                    string typeName = typeof(AdviceHelper).ToString();
                    bool delete = false;
                    if (DeleteEmailTextBox.Text == "1")
                    {
                        delete = true;
                    }
                    MailResult result = mailHelper.ReceiveMail("We7.CMS.Utils.dll", typeName, "HandleReceiveMail", delete);
                    LoadAdvices();
                    string errorRoot = "<a href=\"/admin/Advice/AdviceProcessManage.aspx\" >反馈监控管理</a>";
                    string message = "";
                    if (result.Count > 0)
                    {
                        message = "您共获取到了" + result.Count + "封邮件";
                    }
                    else
                    { 
                        message = "Sorry,没有邮件可以获取...";
                    }
                    int errorCount = result.Count - result.Success;
                    if (errorCount > 0)
                    {
                        message += ",有" + errorCount + "封回复邮件存在错误信息，不可直接对应回复到反馈信息，请到" + errorRoot + " <邮件回复>下进行处理。";
                    }
                    else if (result.Count > 0 && result.Success > 0)
                    {
                        message += ",并成功回复" + result.Success + "条反馈信息。";
                    }
                    Messages.ShowMessage(message);
                }
                catch (Exception)
                {
                    Messages.ShowMessage("系统繁忙，请稍后在试！");
                }
            }
            else
            { 
                Messages.ShowMessage("请先选择反馈模型后再获取回复邮件！");
            }
        }

        /// <summary>
        /// 获取所选择的所有数据的ID
        /// </summary>
        /// <returns></returns>
        private List<string> GetIDs()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < DataGridView.Rows.Count; i++)
            {
                if (((CheckBox)DataGridView.Rows[i].FindControl("chkItem")).Checked)
                {
                    list.Add(((Label)(DataGridView.Rows[i].FindControl("lblID"))).Text);
                }
            }
            return list;
        }

    }
}
