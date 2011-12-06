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

using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin.tools.widget
{
    public partial class wid_myarticle : BaseUserControl
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
            bool enableCache = (CDHelper.Config.EnableCache == "true");
            List<Article> result = null;

            string type = StateMgr.ConvertEnumToStr(EnumLibrary.ArticleType.Article);
            ArticleQuery query = new ArticleQuery();
            query.AccountID = AccountID;
            query.EnumState = type;
            query.OrderKeys = "ID";
            List<Article> articleList = ArticleHelper.QueryArtilcesByAll(query, 0, 5, null);
            if (articleList != null)
            {
                foreach (Article article in articleList)
                {
                    if (article.Title.Length > 25)
                    {
                        article.Title = article.Title.Substring(0, 25) + "...";
                    }
                }
            }
            result = articleList;

            DataGridView.DataSource = result;
            DataGridView.DataBind();
        }

        public string GetUrl(string ID)
        {

            string url = "/admin/addins/ArticleEdit.aspx?id={0}";
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