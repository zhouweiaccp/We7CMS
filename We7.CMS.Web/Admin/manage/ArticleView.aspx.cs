using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;
using System.Collections.Generic;

namespace We7.CMS.Web.Admin
{
    public partial class ArticleView : BasePage
    {
        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        protected override We7.CMS.Common.Enum.MasterPageMode MasterPageIs
        {
            get
            {
                return We7.CMS.Common.Enum.MasterPageMode.NoMenu;
            }
        }

        public string ArticleID
        {
            get
            {
                return Request["id"].ToString();
            }
        }
        /// <summary>
        /// 当前文章的附件列表
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadArticle();
        }

        void LoadArticle()
        {
            Attachments = new List<Attachment>();

            if (!We7Helper.IsEmptyID(ArticleID))
            {
                Article a = ArticleHelper.GetArticle(ArticleID);
                if (a != null)
                {
                    TitleLabel.Text = a.Title;
                    ContentLabel.Text = a.Content;
                    Attachments = AttachmentHelper.GetAttachments(a.ID);
                }
            }
        }
    }
}
