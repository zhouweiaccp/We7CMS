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
using System.Xml;

namespace We7.CMS.Web.Admin
{
    public partial class Channels : BasePage
    {
        static string TopTitle = "根栏目";
        static string TopSummary = "此栏目下的所有栏目，将作为第一级栏目。";

        string ChannelID
        {
            get { return Request["id"]; }
        }

        public bool IsWap
        {
            get { return Request["wap"] != null; }
        }

        protected override void Initialize()
        {           
            ClientScriptManager cs = this.Page.ClientScript;
            string url = "/admin/ajax/Ext2.0/adapter/ext/ext-base.js";
            if (!cs.IsClientScriptIncludeRegistered("ext-base_js"))
                cs.RegisterClientScriptInclude(this.GetType(), "ext-base_js", url);
            url = "/admin/ajax/Ext2.0/ext-all.js";
            if (!cs.IsClientScriptIncludeRegistered("ext-all_js"))
                cs.RegisterClientScriptInclude(this.GetType(), "ext-all_js", url);
            //url = "/Ajax/Ext2.0/SessionProvider.js";
            //if (!cs.IsClientScriptIncludeRegistered("SessionProvider_js"))
            //    cs.RegisterClientScriptInclude(this.GetType(), "SessionProvider_js", url);

            url = "/admin/ajax/ChannelTree.js";
            if (!cs.IsClientScriptIncludeRegistered("ChannelTree_js"))
                cs.RegisterClientScriptInclude(this.GetType(), "ChannelTree_js", url);

            HtmlGenericControl cssLink = new HtmlGenericControl("link");
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            cssLink.Attributes["href"] = "/admin/ajax/Ext2.0/resources/css/ext-all.css";
            this.Header.Controls.Add(cssLink);

            if (IsWap) NameLabel.Text = "WAP栏目管理";

        }
    }
}