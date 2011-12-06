using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.WebControls.WD;
using We7.CMS.Config;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using System.Web.UI.HtmlControls;

namespace We7.CMS.WebControls
{
    public class SiteTreeProvider : BaseWebControl
    {
        public WebSite WebSite;
        protected string TreeHtml;

        string cssClass;
        /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        private string siteID = String.Empty;
        /// <summary>
        /// 上级根目录
        /// </summary>
        public string SiteID
        {
            get { return siteID; }
            set { siteID = value; }
        }

        private bool showChildren = true;
        /// <summary>
        /// 是否显示子站点
        /// </summary>
        public bool ShowChildren
        {
            get { return showChildren; }
            set { showChildren = true; }
        }

        private string treeCss = "filetree";
        /// <summary>
        /// 树的Css
        /// </summary>
        public string TreeCss
        {
            get { return treeCss; }
            set { treeCss = value; }
        }

        private string rootCss = "folder";
        /// <summary>
        /// 根节点Css
        /// </summary>
        public string RootCss
        {
            get { return rootCss; }
            set { rootCss = value; }
        }

        private string nodeCss = "folder";
        /// <summary>
        /// 节点Css
        /// </summary>
        public string NodeCss
        {
            get { return nodeCss; }
            set { nodeCss = value; }
        }

        private string leafCss="file";
        /// <summary>
        /// 叶子节点Css
        /// </summary>
        public string LeafCss
        {
            get { return leafCss; }
            set { leafCss = value; }
        }

        private string rootText = "站点地图";
        /// <summary>
        /// 根目录的文字
        /// </summary>
        public string RootText
        {
            get { return rootText; }
            set { rootText = value; }
        }

        private string target = "_self";
        /// <summary>
        /// 网页打开的目标
        /// </summary>
        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IncludeJavaScript();
            JavaScriptManager.Include("/Admin/Ajax/jquery/jquery.cookie.js",
                                      "/Admin/Ajax/jquery/jquery.treeview.min.js");
            HtmlGenericControl ctr = new HtmlGenericControl("link");
            ctr.Attributes["href"] = "/Admin/Ajax/jquery/css/jquery.treeview.css";
            ctr.Attributes["type"] = "text/css";
            ctr.Attributes["rel"] = "Stylesheet";
            Page.Header.Controls.Add(ctr);
            LoadData();
            BuildHtml();
        }

        protected void LoadData()
        {
            WDWebService service=new WDWebService();
            service.Url = SiteConfigs.GetConfig().WebGroupServiceUrl;
            WebSite = service.GetWebSiteTree(SiteID, ShowChildren);            
        }

        protected void BuildHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<ul class='{0}'>",TreeCss);
            if (WebSite.ID == We7Helper.EmptyGUID)
            {
                sb.AppendFormat("<li><span class='{0}'>{1}</span>", RootCss, RootText);
            }
            else
            {
                sb.AppendFormat("<li><span class='{0}'><a href='http://{2}' target='{3}'>{1}</a></span>", RootCss, WebSite.Name, WebSite.Url, Target);
            }
            if (WebSite.Children.Length > 0)
            {
                sb.AppendFormat("<ul id='{0}'>", this.ClientID);
                foreach (WebSite site in WebSite.Children)
                {
                    BuildChildren(site, sb);
                }
                sb.Append("</ul>");
            }
            sb.Append("</ul>");
            TreeHtml = sb.Append("<script>$(function(){$('#" + this.ClientID + "').treeview({collapsed: true});});</script>").ToString();
        }

        protected void BuildChildren(WebSite site, StringBuilder sb)
        {
            //if (site.ID == Helper.EmptyGUID)
            //{
            //    sb.AppendFormat("<li><span class='{0}'>{1}</span>", RootCss, RootText);
            //}
            //else
            //{
            sb.AppendFormat("<li><span class='{0}'><a href='http://{2}' target='{3}'>{1}</a></span>", site.Children.Length > 0 ? NodeCss : LeafCss, site.Name, site.Url, Target);
            //}
            if (site.Children.Length > 0)
            {
                sb.Append("<ul>");
                foreach (WebSite cw in site.Children)
                {
                    BuildChildren(cw, sb);
                }
                sb.Append("</ul>");
            }
            sb.Append("</li>");
        }
        
    }
}
