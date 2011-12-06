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

namespace We7.CMS.Web.Admin
{
    public partial class iArticles : BasePage
    {
        public string ColumnID
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

        public string QuoteOwnerID
        {
            get { return Request["oid"]; }
        }

        public bool IsQuote
        {
            get
            {
                string isQuote = Request["type"];
                if (isQuote == "quote")
                {
                    return true;
                }
                return false;
            }
        }

        protected override void Initialize()
        {
            ArticleQuery query = new ArticleQuery();
            query.ChannelID = ColumnID;
            List<Article> data = ArticleHelper.QueryArtilcesByAll(query, 0, 100, null);
            DetailGridView.DataSource = data;
            DetailGridView.DataBind();
            BuildRows();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DetailGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void DetailGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (IsQuote)
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[4].Visible = false;
                }
                else
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[4].Visible = true;
                }
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
            Response.Write("<script   language   =javascript>alert('添加成功!');</script>");

        }

        void QuoteArticles(string id)
        {
            Article a = ArticleHelper.GetArticle(id, null);
            a.Content = "";
            a.ContentType = (int)TypeOfArticle.LinkArticle;
     
            Channel ch=ChannelHelper.GetChannel(a.OwnerID, null);
            a.ContentUrl = ch.FullUrl + a.FullUrl;
            a.OwnerID = QuoteOwnerID;
            a.EnumState = ch.EnumState;
            a.ChannelName = ch.FullPath;

            ArticleHelper.AddArticle(a);
        }

        void BuildRows()
        {
            foreach (GridViewRow dt in DetailGridView.Rows)
            {
                Label lbSelect = (Label)dt.FindControl("SelectLabel");
                Label lbUrl = (Label)dt.FindControl("UrlLabel");
                if (lbSelect != null && lbUrl != null)
                {
                    string url = lbUrl.Text;
                    if (url != null && url.Length > 0)
                        lbSelect.Text = string.Format("<a href=\"javascript:returnUrl('{0}')\" >选</a>", url);
                }
            }
        }
    }
}
