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
using Thinkment.Data;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class ArticleRelatesAdd : BasePage
    {
        /// <summary>
        /// 弹出窗口暂不过滤权限
        /// </summary>
        protected override bool NeedAnPermission
        {
            get { return false; }
        }

        public string OwnerID
        {
            get
            {
                string id = Request["id"];
                if (id == null)
                {
                    return We7Helper.EmptyGUID;
                }
                return id;
            }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

   
        public List<string> Tags
        {
            get
            {
                if (ViewState["ARTICLE_TAGS"] != null)
                {
                    return (List<string>)ViewState["ARTICLE_TAGS"];
                }
                else
                {
                    List<string> taglist = ArticleHelper.GetTags(OwnerID);
                    ViewState["ARTICLE_TAGS"] = taglist;
                    return taglist;
                }
            }
        }

        public bool IsFilter
        {
            get { return (bool)ViewState["$VS_CD_ISFILTER"]; }
            set { ViewState["$VS_CD_ISFILTER"] = value; }

        }

        public int PageSize
        {
            get
            {
                return 10;
            }
        }

        void BindListByFilter()
        {
            ArticlePager.RecorderCount = ArticleHelper.QueryArticlesCountByTags(Tags);
            ArticlePager.PageSize = PageSize;
            string[] fields = new string[] { "ID", "Title", "OwnerID", "Description", "Permission" };
            List<Article> list = ArticleHelper.QueryArticlesByTags(Tags, ArticlePager.Begin, ArticlePager.Count, fields);
            DetailGridView.DataSource = list;
            DetailGridView.DataBind();
            MessageLabel.Text ="直接匹配标签“"+ TagsLabel.Text+"”的结果：";
        }

        void BindListByKeyword()
        {
            string KeyWord = KeyTextBox.Text;
            DateTime BeginDate = DateTime.MinValue;
            DateTime EndDate = DateTime.MaxValue;
            ArticleQuery q = new ArticleQuery();
            q.KeyWord = KeyWord;
            q.BeginDate = BeginDate;
            q.EndDate = EndDate;

            ArticlePager.PageSize = PageSize;
            ArticlePager.RecorderCount = ArticleHelper.QueryArtilceCountByAll(q);

            if (PageSize > 0)
            {
                PagerDiv.Visible = (ArticlePager.RecorderCount <= PageSize) ? false : true;
            }

            List<Article> list = ArticleHelper.QueryArtilcesByAll(q, ArticlePager.Begin, ArticlePager.Count, null);
            DetailGridView.DataSource = list;
            DetailGridView.DataBind();
            MessageLabel.Text = "查询关键字“" + KeyTextBox.Text + "”的结果：";
        }

        protected void Pager_Fired(object sender, EventArgs e)
        {
            if (IsFilter)
                BindListByFilter();
            else
                BindListByKeyword();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                for (int i = 0; i < Tags.Count; i++)
                {
                    TagsLabel.Text = TagsLabel.Text + "/" + Tags[i];
                }
            }
        }


        protected void DetailGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void AddArticlesButton_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < DetailGridView.Rows.Count; i++)
            {
                if (((CheckBox)DetailGridView.Rows[i].FindControl("chkItem")).Checked)
                {
                    list.Add(((Label)(DetailGridView.Rows[i].FindControl("lblID"))).Text);
                }
            }
            foreach (string id in list)
            {
                QuoteArticles(id);
            }
        }

        void QuoteArticles(string id)
        {
            if (id != OwnerID)
            {
                RelatedArticle a = new RelatedArticle();
                a.ArticleID = OwnerID;
                a.RelatedID = id;
                ArticleHelper.AddRelatedArticle(a);
            }
        }

        protected void FilterButton_Click(object sender, EventArgs e)
        {
            IsFilter = true;
            BindListByFilter();
        }

        protected void QueryButton_Click(object sender, EventArgs e)
        {
            IsFilter = false;
            BindListByKeyword();
        }
    }
}
