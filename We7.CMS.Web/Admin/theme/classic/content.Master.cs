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
using System.IO;

using We7.CMS;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Config;
using We7.CMS.Common.PF;
using We7.CMS.Accounts;
using We7.CMS.WebControls.Core;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.theme.classic
{
    public partial class content : System.Web.UI.MasterPage
    {
        protected virtual string AccountID
        {
            get
            {
                return Security.CurrentAccountID;
            }
        }

        public string SiteLoginUserTitle
        {
            get
            {
                string accountName = "";
                if (!We7Helper.IsEmptyID(AccountID))
                {
                    IAccountHelper AccountHelper = AccountFactory.CreateInstance();
                    Account act = AccountHelper.GetAccount(AccountID, new string[] { "LoginName", "LastName" });
                    if (act != null && act.LoginName != null)
                    {
                        accountName = act.LastName;
                    }
                    else
                    {
                        accountName = "管理员";
                    }
                }
                else
                {
                    accountName = "管理员";
                }

                return accountName;
            }
        }

        protected string SiteHeadTitle
        {
            get
            {
                return SiteConfigs.GetConfig().SiteName;
            }
        }

        protected string ProductBrand
        {
            get
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si != null)
                {
                    return si.ProductName;
                }
                else
                    return "We7";
            }
        }

        private bool isSiteGroup;
        protected string IsSiteGroup
        {
            get
            {
                return isSiteGroup ? "" : "display:none;";
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request["reboot"] != null)
            {
                ApplicationHelper.ResetApplication();
                string url = We7Helper.RemoveParamFromUrl(this.Request.RawUrl, "reboot");
                Response.Redirect(url, true);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (AccountID == null || AccountID == "")
                Response.Redirect(
                    "/admin/Signin.aspx?returnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl), true);
            else if (AccountID == We7Helper.EmptyGUID)
            {
                footer_upgrade.Visible = true;
                RebootHyperLink.NavigateUrl = We7Helper.AddParamToUrl(this.Request.RawUrl, "reboot", "true");
            }

            //GroupLinksLiteral.Text = CreateGroupLinks();
            //QuickLinksLiteral.Text = CreateQuickLinks();
            if (SiteConfigs.GetConfig().SiteGroupEnabled)
            {
                GroupHolder.Text = string.Format("<input type=\"hidden\" id=\"wdUrl\" value=\"{0}\" />", SiteConfigs.GetConfig().WDWebUrl + "/group/GetSites.aspx?json=t&AccountID=" + AccountID);
            }
            InitCopyrightLinks();
        }

        string CreateGroupLinks()
        {
            string jsString = @"<link media=""screen"" rel=""stylesheet"" href=""/admin/ajax/jquery/colorbox/colorbox.css"" />
                <script src=""/admin/ajax/jquery/colorbox/jquery.colorbox-min.js"" type=""text/javascript""></script>
                <script type=""text/javascript"">
                  $(document).ready(function(){
                  $(""#swichAction"").colorbox({width:""70%"", height:""80%"", iframe:true});
                  });
                </script>";
            string links = "<a href=\"{0}/group/GetSites.aspx?AccountID={1}\" id=\"swichAction\" >站点切换</a> | <a href='{0}'  target='_blank'>站群管理</a> |  ";
            SiteConfigInfo si = SiteConfigs.GetConfig();
            isSiteGroup = si.SiteGroupEnabled;
            if (si != null && si.SiteGroupEnabled && si.WDWebUrl != null && si.WDWebUrl != "")
            {
                links = string.Format(links, si.WDWebUrl, AccountID);
                return jsString + links;
            }
            else
                return "";
        }

        string CreateQuickLinks()
        {
            return "";
        }

        void InitCopyrightLinks()
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si != null)
            {
                if (si.IsOEM)
                {
                    CopyrightLiteral.Text = si.Copyright;
                    AuthorLinksLiteral.Text = si.Links;
                }
                else
                {
                    CopyrightLiteral.Text = si.CopyrightOfWe7;
                    AuthorLinksLiteral.Text = si.LinksOfWe7;
                }
            }
        }

        /// <summary>
        /// 重建所有索引
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReIndexHyperLink_Click(object sender, EventArgs e)
        {
            try
            {
                //部件，主题索引
                BaseControlHelper baseCtrlHelper = new BaseControlHelper();
                baseCtrlHelper.CreateThemeIndex();
                baseCtrlHelper.CreateWidegetsIndex();
                //模板图索引
                SiteSettingHelper siteSettingHelper = new SiteSettingHelper();
                TemplateHelper TemplateHelper = new TemplateHelper();
                string defaultTempGroupFile = siteSettingHelper.Config.DefaultTemplateGroupFileName;
                if (!string.IsNullOrEmpty(defaultTempGroupFile) && 
                    File.Exists(Path.Combine(TemplateHelper.TemplateGroupPath, defaultTempGroupFile)))
                {
                    SkinInfo data = TemplateHelper.GetSkinInfo(defaultTempGroupFile);
                    TemplateHelper.CreateMapFileFromSkinInfo(data);
                    TemplateHelper.RefreshTemplateDefaultBindText(data);
                }            
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(content), ex);
            }
        }
    }
}
