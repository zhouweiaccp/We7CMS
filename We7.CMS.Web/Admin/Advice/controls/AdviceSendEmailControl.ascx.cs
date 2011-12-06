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

namespace We7.CMS.Web.Admin.controls
{
    public partial class AdviceSendEmailControl : BaseUserControl
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
                //HttpUtility.HtmlEncode(
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
            if (email != "")
            {
                Account account = AccountHelper.GetAccountByEmail(email);
                if (account != null)
                {
                    return account.LastName;
                }
            }
             return email;
        }

        /// <summary>
        /// 页面加载xml数据
        /// </summary>
        void LoadAdvices()
        {
            List<AdviceReply> adviceReply = new List<AdviceReply>();//借用此对象属性便于绑定前台数据
            string root = Server.MapPath("/_Data/SendEmail/");
            if (Directory.Exists(root))
            {
                DirectoryInfo dir = new DirectoryInfo(root);

                FileInfo[] files = dir.GetFiles("*.xml");
                foreach (FileInfo file in files)
                {
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
                    node = xmlDoc.SelectSingleNode("/root/infoFormUser");
                    ar.FormEmail = We7Helper.Base64Decode(node.InnerText);
                    ar.MailFile = We7Helper.Base64Encode(file.Name);
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

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {

            string xmlTitle = We7Helper.Base64Decode(MailFileText.Text);
            string title = TitleText.Text;
            string root = Server.MapPath("/_Data/SendEmail/");
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
                foreach (string titleTM in list)
                {
                    File.Delete(Path.Combine(root,  We7Helper.Base64Decode(titleTM)));
                }
                Messages.ShowMessage(string.Format("您已经成功删除了{0}封邮件", list.Count.ToString()));
            }
            LoadAdvices();
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
