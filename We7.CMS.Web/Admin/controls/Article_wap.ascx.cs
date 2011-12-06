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
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin.controls
{
    /// <summary>
    /// ·¢²¼µ½wap
    /// </summary>
    public partial class Article_wap : BaseUserControl
    {
        public string OwnerID
        {
            get
            {
                if (Request["oid"] != null)
                    return Request["oid"];
                else
                {
                    if (ViewState["$VS_OwnerID"] == null)
                    {
                        if (ArticleID != null)
                        {
                            Article a = ArticleHelper.GetArticle(ArticleID, null);
                            ViewState["$VS_OwnerID"] = a.OwnerID;
                        }
                    }
                    return (string)ViewState["$VS_OwnerID"];
                }
            }
        }

        public string ArticleID
        {
            get { return Request["id"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWapArticle();
            }
        }

        Article AddWapArticle()
        {
            Article sourceArticle = ArticleHelper.GetArticle(ArticleID, null);
            Article wap = ArticleHelper.CopyToWapArticle(sourceArticle);
            wap.OwnerID = WapOidTextBox.Text.Trim();
            wap.AccountID = AccountID;
            ArticleHelper.AddArticle(wap);
            return wap;
        }

        protected void SaveWapButton_Click(object sender, EventArgs e)
        {
            if (!We7Helper.IsEmptyID(WapOidTextBox.Text.Trim()))
            {
                Article wap = ArticleHelper.GetArticleBySource(WapOidTextBox.Text.Trim(), ArticleID);
                if (wap == null)
                {
                    wap = AddWapArticle();
                }
                EditWapHyperLink.NavigateUrl = string.Format("/addins/ArticleEdit.aspx?wap=1&id={0}", wap.ID);
                EditWapHyperLink.Target = "_blank";
                EditWapHyperLink.Visible = true;
                PublishToWap.Visible = false;
            }
        }

        void LoadWapArticle()
        {
            if (ArticleID != null)
            {
                Article wap = ArticleHelper.GetArticleBySource(null, ArticleID);
                if (wap != null)
                {
                    EditWapHyperLink.NavigateUrl = string.Format("/addins/ArticleEdit.aspx?wap=1&id={0}", wap.ID);
                    EditWapHyperLink.Target = "_blank";
                    EditWapHyperLink.Visible = true;
                    PublishToWap.Visible = false;
                }
                else
                {
                    EditWapHyperLink.Visible = false;
                    PublishToWap.Visible = true;
                }
            }
            else
            {
                EditWapHyperLink.Visible = false;
                PublishToWap.Visible = false;
            }
        }


    }

}