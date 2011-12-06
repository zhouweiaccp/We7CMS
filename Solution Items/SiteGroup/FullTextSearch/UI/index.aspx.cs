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
using WebEngine2007.SE;
using We7.Framework.Config;
using We7.Plugin.FullTextSearch;

namespace We7.CMS.Web.Plugin
{
    public partial class FullTextSearch_UI_index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadConfig();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            SaveToConfigFile();
        }

        protected void InitButton_Click(object sender, EventArgs e)
        {
            int ret = IndexerDataProvider.Instance().CreateArticleIndexData("10000");
            Messages.ShowMessage(string.Format("已将{0}条信息导入索引表。", ret));
        }

        private void SaveToConfigFile()
        {
            SiteConfigInfo si = SiteConfigs.GetConfig();
            if (!string.IsNullOrEmpty(SEUrlTextBox.Text))
            {
                si.FullTextSearchUrl = SEUrlTextBox.Text;
                SiteConfigs.SaveConfig(si);
            }

             string url = "tcp://localhost:11001";
            if (!string.IsNullOrEmpty(si.FullTextSearchUrl))
                url = si.FullTextSearchUrl;

            ISearchService searcher = Activator.GetObject(typeof(ISearchService), url + "/SearchService") as ISearchService;
            if (searcher != null)
            {
                SiteInfo site = new SiteInfo(si.SiteID,si.SiteName,si.RootUrl);
                searcher.RegisterSite(site);
                Messages.ShowMessage("站点已成功注册到搜索服务器！");
            }
        }

        void LoadConfig()
        {
            SiteConfigInfo si = SiteConfigs.GetConfig();
            if (si != null)
                SEUrlTextBox.Text = si.FullTextSearchUrl;
        }
    }
}