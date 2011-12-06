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

using We7.CMS.Config;
using We7.CMS;
using We7.Framework.Config;
using We7.Plugin.DataSharing;

namespace We7.Plugin.SiteGroupPlugin
{
    public partial class GroupConfiguration :BasePage
    {

        protected override void Initialize()
        {
            SiteConfigInfo ci = SiteConfigs.GetConfig();

            SiteIDTextBox.Text = ci.SiteID;
            WDUrlTextBox.Text = ci.WebGroupServiceUrl;
            IDUrlTextBox.Text = ci.InformationServiceUrl;
            PassportServiceUrlTextBox.Text = ci.PassportServiceUrl;
            SiteGroupEnabledCheckBox.Checked = ci.SiteGroupEnabled;
            WDWebUrlTextBox.Text = ci.WDWebUrl;
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            SiteConfigInfo ci = SiteConfigs.GetConfig();
            ci.SiteID = SiteIDTextBox.Text;
            ci.WebGroupServiceUrl = WDUrlTextBox.Text;
            ci.InformationServiceUrl = IDUrlTextBox.Text;
            ci.PassportServiceUrl = PassportServiceUrlTextBox.Text;
            ci.SiteGroupEnabled = SiteGroupEnabledCheckBox.Checked;
            if (WDWebUrlTextBox.Text.EndsWith("/"))
                WDWebUrlTextBox.Text = WDWebUrlTextBox.Text.Remove(WDWebUrlTextBox.Text.Length - 1);
            ci.WDWebUrl = WDWebUrlTextBox.Text;
            SiteConfigs.SaveConfig(ci);

            //记录日志
            string content = string.Format("更改站群配置信息。");
            AddLog("设置-站群参数", content);

            Messages.ShowMessage("站群设置保存成功！");
        }

    }
}
