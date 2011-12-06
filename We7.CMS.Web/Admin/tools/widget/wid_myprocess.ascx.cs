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
using System.Collections.Generic;
using We7.CMS.Common.Enum;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.tools.widget
{
    public partial class wid_myprocess : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindingData();
            }
        }
        public void BindingData()
        {
            //取出所有待审批文章，逐一判断是否具有权限
            ArticleQuery query = new ArticleQuery();
            query.State = ArticleStates.Checking;
            List<Article> GetAllArticles = ArticleHelper.QueryArtilcesByAll(query);
            if (GetAllArticles==null)
            {
                return;
            }
            List<Article> articles = new List<Article>();

            foreach (Article article in GetAllArticles)
            {
                if (article.Title.Length > 25)
                {
                    article.Title = article.Title.Substring(0, 25) + "...";
                }
                try
                {
                    string curLayerNOText = ArticleProcessHelper.GetCurLayerNOText(article.ID);
                    if (curLayerNOText != "") //文章当前审批进程：类似 Channel.FirstAudit
                    {
                        string channelID = ArticleHelper.GetArticle(article.ID).OwnerID;
                        List<string> contents = AccountHelper.GetPermissionContents(AccountID, channelID);
                        if (contents.Contains(curLayerNOText))
                        {
                            articles.Add(article);
                        }
                    }
                }
                catch
                { }
            }
            if (articles.Count > 5)
            {
                DataGridView.DataSource = articles.GetRange(0, 5);
            }
            else
            {
                DataGridView.DataSource = articles;
            }
            DataGridView.DataBind();
        }
        public string GetUrl(string ID)
        {

            string url = "/addins/ArticleEdit.aspx?id={0}";
            Article a = ArticleHelper.GetArticle(ID, new string[] { "Title", "State" });
            string returnurl = "";
            if (a.State != (int)ArticleStates.Checking)
            {
                returnurl = String.Format(url, ID);
            }
            else
                returnurl = "javascript:alert('文章正在审核流程，不能编辑。');";

            return returnurl;

        }
    }
}