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
using We7.Framework;
using System.Collections.Generic;
using We7.CMS.Common;
using We7.Framework.Config;
using System.IO;

namespace We7.CMS.Web.Admin.VisualTemplate
{
    public partial class VisualDesign : BasePage
    {
        protected override We7.CMS.Common.Enum.MasterPageMode MasterPageIs
        {
            get
            {
                return We7.CMS.Common.Enum.MasterPageMode.None;
            }
        }

        protected string IsDemoSite
        {
            get { return GeneralConfigs.GetConfig().IsDemoSite?"true":"false"; }
        }

       
          /// <summary>
        /// 模板皮肤名称
        /// </summary>
        public string SkinName
        {
            get { 
                
                var folder=Request["folder"];
                

                return folder;
            }
        }

        string DesignFolder
        {
            get
            {
                var folder = SkinName;
                if (!folder.StartsWith("~"))
                {
                    folder = string.Format("~{0}", folder);
                }

                return folder;
            }
        }
        string FileName
        {
            get { return Request["file"]; }
        }

        TemplateHelper TemplateHelper
        {
            get { return HelperFactory.Instance.GetHelper<TemplateHelper>(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterCommonScript();
            CreateThemeIndex();
            string editFile = TemplateHelper.GetTemplatePath(String.Format("~{0}/{1}", SkinName, FileName));
            InitControls(editFile);
            Template t = new Template();
            t.FromFile(Server.MapPath(String.Format("\\{0}\\{1}", Constants.TemplateBasePath, SkinName)), FileName + ".xml");
            NameLabel.Text = t.Name;
            this.Page.Title = "正在设计" + NameLabel.Text;
            SummaryLabel.Text = String.Format("{0}", t.FileName);
         
        }

        /// <summary>
        /// 初始化相关控件
        /// </summary>
        private void InitControls(string editFileFullPath)
        {
            PrevewDropDownList.Items.Clear();
            PrevewDropDownList.Items.Add(new ListItem("--预览地址--", ""));
            string editUrl = editFileFullPath.Replace(Server.MapPath("~/"), "").Replace("\\", "/");
            if (!editUrl.StartsWith("/")) editUrl = "/" + editUrl;
            List<string> prevews = TemplateHelper.GetPrevewUrls(FileName, SkinName);
            foreach (string p in prevews)
            {
                PrevewDropDownList.Items.Add(new ListItem(p, string.Format("{0}?template={1}", p, editUrl)));
            }
        }

        /// <summary>
        /// 注册公共脚本
        /// </summary>
        private void RegisterCommonScript()
        {
            HtmlGenericControl script = new HtmlGenericControl("script");
            script.Attributes["type"] = "text/javascript";
            script.InnerText = "var Common={};Common.Theme='" + GeneralConfigs.GetConfig().Theme + "';";
            this.Header.Controls.AddAt(0,script);
        }

        /// <summary>
        /// 生成主题索引文件
        /// </summary>
        private void CreateThemeIndex()
        {
            string theme = HttpContext.Current.Server.MapPath("~/Widgets/Themes/Themes.xml");
            if (!File.Exists(theme))
            {
                try
                {
                    We7.CMS.WebControls.Core.BaseControlHelper Helper = new We7.CMS.WebControls.Core.BaseControlHelper();
                    Helper.CreateThemeIndex();
                }
                catch (Exception ex)
                {
                    //log
                }
            }
        }
    }
}
