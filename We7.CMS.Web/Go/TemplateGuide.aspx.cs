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
using We7.Framework.Config;
using We7.Framework;
using System.IO;
using We7.CMS.Common;
using System.Collections.Generic;
using We7.Model.Core;


namespace We7.CMS.Web
{
    public partial class TemplateGuide : System.Web.UI.Page
    {
        TemplateBindConfig BindConfig
        {
            get
            {
                if (ViewState["$BINDCONFIG"] != null)
                    return ViewState["$BINDCONFIG"] as TemplateBindConfig;
                else
                {
                    TemplateBindConfig bc = new TemplateBindConfig();
                    bc.Handler = Request["handler"];
                    if (bc.Handler == "site" && string.IsNullOrEmpty(Request["mode"]))
                        bc.Mode = "home";
                    else if (string.IsNullOrEmpty(Request["mode"]))
                        bc.Mode = "default";
                    else
                        bc.Mode = Request["Mode"];

                    bc.Model = Request["model"];
                    if (!string.IsNullOrEmpty(bc.Model))
                    {
                        ModelInfo mi = ModelHelper.GetModelInfoByName(bc.Model);
                        if (mi != null) bc.ModelText = mi.Label;
                    }
                    TemplateHelper TemplateHelper = HelperFactory.Instance.GetHelper<TemplateHelper>();
                    bc = TemplateHelper.GetTemplateConfigSentence(bc);
                    ViewState["$BINDCONFIG"] = bc;
                    return bc;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HelperFactory helperFactory = HelperFactory.Instance;
            TemplateHelper TemplateHelper = helperFactory.GetHelper<TemplateHelper>();
            string editorUrl = "/admin/DataControlUI/Compose.aspx";
            if (GeneralConfigs.GetConfig().DefaultTemplateEditor == "1")
                editorUrl = "/admin/VisualTemplate/TemplateCreate.aspx";
            string bindName = string.Format("handler={0}&mode={1}&model={2}", BindConfig.Handler, BindConfig.Mode, BindConfig.Model);
            string urlCompose = string.Format("{0}?{1}", editorUrl, bindName);

            TitleLabel.Text = "没有匹配的" + BindConfig.ModelText + BindConfig.Description + "模板";
            ActionLiteral.Text = string.Format("<a href='{0}' target='_blank'>创建{1}（模板）</a>", urlCompose, BindConfig.ModelText + BindConfig.Description);

            if (string.IsNullOrEmpty(GeneralConfigs.GetConfig().DefaultTemplateGroupFileName) ||
                    !Directory.Exists(TemplateHelper.DefaultTemplateGroupPath))
            {
                HelpLiteral.Text = "您也可以到微七插件商场下载一套完整模板。<a href='http://m.we7.cn'  target='_blank'>去下载</a>";
                TemplateGroup[] tg = TemplateHelper.GetTemplateGroups(null);
                int count = tg.Length;
                if (count > 0)
                {
                    string urlSelectTemplate = "/admin/TemplateGroups.aspx";
                    HelpLiteral.Text += string.Format("<br>本地有{0}套可用模板组，您没有选用，要选用一个吗？<a href='{1}' target='_blank'>选一个</a>", count.ToString(), urlSelectTemplate);
                }
            }
            else
            {
                Template[] tps = TemplateHelper.GetTemplates(null, Path.GetFileNameWithoutExtension(GeneralConfigs.GetConfig().DefaultTemplateGroupFileName));
                if (tps.Length > 0)
                {
                    string urlBindTemplate = string.Format("/admin/Template/TemplateGroupEdit.aspx?file={0}&tab=3", GeneralConfigs.GetConfig().DefaultTemplateGroupFileName);
                    HelpLiteral.Text = string.Format("您已经创建了{0}个模板页了，是否已经创建本页模板但尚未指定？<a href='{1}' target='_blank'>去看一下</a>", tps.Length, urlBindTemplate);
                }
            }
        }
    }
}
