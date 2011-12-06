using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using We7.CMS.Controls;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class Articles : BasePage
    {

        public string OwnerID
        {
            get
            {
                string oid = Request["oid"];
                if (oid == null)
                {
                    return We7Helper.EmptyGUID;
                }
                return oid;
            }
        }
        public string ChannelTreeVisble
        {
            get
            {
                //TODO::2010-12-6.以前用的是不等null,现在改成了等于null
                if (AccountHelper.GetAccount(AccountID, new string[] { "ID" }) == null)
                {
                    return "none";
                }
                else
                { return ""; }
                return "";
            }

        }
        bool IsWap
        {
            get { return Request["wap"] != null; }
        }

        protected override void Initialize()
        {
            ClientScriptManager cs = this.Page.ClientScript;
            string url = "../ajax/Ext2.0/adapter/ext/ext-base.js";
            if (!cs.IsClientScriptIncludeRegistered("ext-base_js"))
                cs.RegisterClientScriptInclude(this.GetType(), "ext-base_js", url);
            url = "../ajax/Ext2.0/ext-all.js";
            if (!cs.IsClientScriptIncludeRegistered("ext-all_js"))
                cs.RegisterClientScriptInclude(this.GetType(), "ext-all_js", url);

            url = "../ajax/ArticleTree.js";
            if (!cs.IsClientScriptIncludeRegistered("ArticleTree_js"))
                cs.RegisterClientScriptInclude(this.GetType(), "ArticleTree_js", url);

            HtmlGenericControl cssLink = new HtmlGenericControl("link");
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            cssLink.Attributes["href"] = "../ajax/Ext2.0/resources/css/ext-all.css";
            this.Header.Controls.Add(cssLink);

            if (IsWap) NameLabel.Text = "WAP文章管理";

            url = "articlelist.aspx" + Request.Url.Query;
            ListTypeHyperLink.NavigateUrl = We7Helper.AddParamToUrl(url, "notiframe", "1");
            TreeTypeHyperLink.NavigateUrl = "articles.aspx" + Request.Url.Query;
        }
    }
}
