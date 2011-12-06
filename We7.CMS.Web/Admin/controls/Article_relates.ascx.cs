using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.controls
{
    public partial class Article_relates : BaseUserControl
    {
        public string ArticleID
        {
            get { return Request["id"]; }
        }

        public string RelatedArticlePath = "";


        protected void DeleteRelatedArticleButton_Click(object sender, EventArgs e)
        {
            string aid = RelatedArticleIDTextBox.Text;
            ArticleHelper.DeleteRelatedArticle(ArticleID, aid);
            BindRelatedArticleList();
        }


        void BindRelatedArticleList()
        {
            List<Article> als = ArticleHelper.GetRelatedArticles(ArticleID);
            foreach (Article a in als)
            {
                if (a.Title.Length > 20)
                {
                    a.Title = a.Title.Substring(0, 20);
                }
                Channel ch = ChannelHelper.GetChannel(a.OwnerID, null);
                a.LinkUrl = String.Format("{0}{1}", ch.FullUrl, a.FullUrl);
                a.FullChannelPath = ch.FullPath;
            }

            DataGridView.DataSource = als;
            DataGridView.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRelatedArticleList();
            }
        }
        protected void RefreshButton_Click(object sender, EventArgs e)
        {
            BindRelatedArticleList();
        }
    }
}