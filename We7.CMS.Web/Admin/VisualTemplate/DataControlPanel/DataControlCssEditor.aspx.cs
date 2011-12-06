using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common.Enum;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin.VisualTemplate
{
    public partial class DataControlCssEditor : BasePage
    {
        string controlfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        string ControlFile
        {
            get
            {
                if (String.IsNullOrEmpty(controlfile))
                {
                    controlfile = Request["ctr"];
                    if (String.IsNullOrEmpty(controlfile))
                        throw new Exception("控件参数不能为空");
                }
                return controlfile;
            }
        }

        bool IsEdit
        {
            get
            {
                if (ViewState["WE$IsEditor"] == null)
                {
                    string cmd = Request["cmd"];
                    return String.IsNullOrEmpty(cmd) ? false : cmd.ToLower() == "edit";
                }
                return (bool)ViewState["WE$IsEditor"];
            }
            set
            {
                ViewState["WE$IsEditor"] = value;
            }
        }

        public string Style
        {
            get
            {
                string s = Request["style"];
                if (String.IsNullOrEmpty(s))
                    return "";//throw new Exception("当前样式为空");
                else
                    return s.Trim();
            }
        }
        string Group
        {
            get
            {
                string tmpfolder = String.Empty;
                if (!String.IsNullOrEmpty(Request["gp"]))
                    tmpfolder = Request["gp"];// We7Helper.Base64Decode(Request["gp"]);
                else
                {
                    tmpfolder = CDHelper.Config.DefaultTemplateGroupFileName;
                    tmpfolder = tmpfolder.Remove(tmpfolder.IndexOf("."));
                }
                return tmpfolder.TrimStart('~');
            }
        }

        string GroupCopy
        {
            get
            {
             //   return String.Format("~{0}", Group);
                GeneralConfigInfo config = GeneralConfigs.GetConfig();
                if (config.SiteBuildState == "run")
                {
                    return String.Format("~{0}", Group);
                }
                else
                {
                    return Group;
                }
            }
        }

        private void BindData()
        {
            BindContent();
        }

        void BindContent()
        {
            if (!string.IsNullOrEmpty(Style))
            {
                TemplateProcessor proccessor = new TemplateProcessor(GroupCopy);
                CssTextBox.Text = proccessor.LoadAppendCss(ControlFile, Style);
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            TemplateProcessor proccor = new TemplateProcessor(GroupCopy);
            proccor.OverrideCss(ControlFile, Style, CssTextBox.Text);

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "window.close()", true);
        }

        protected void bttnStore_Click(object sender, EventArgs e)
        {
            string style = Style;
            if (hdKey.Value == "1")
            {
                try
                {
                    string[] s = hdValue.Value.Split('|');
                    string sv = DataControlHelper.SaveCss(ControlFile, s[0], s[1], style, CssTextBox.Text);
                    doJs(sv);
                    style = sv.Split('.')[0];
                }
                catch (Exception ex)
                {
                    msgLabel.Text = ex.Message;
                }
            }
            else
            {
                try
                {
                    DataControlHelper.EditCss(ControlFile, Style, CssTextBox.Text);
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "window.close()", true);
                    TemplateProcessor proccor = new TemplateProcessor(GroupCopy);
                    proccor.OverrideCss(ControlFile, style, CssTextBox.Text);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    Response.End();
                }
            }
        }
        void doJs(string ctr)
        {
            string js = @"(function(){
            window.returnValue={key:'{$KEY}',value:'{$VALUE}'};
            window.close();})()";

            string[] s = ctr.Split('.');
            js = js.Replace("{$KEY}", s[0]).Replace("{$VALUE}", s[1]);

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reutrnValue", js, true);
        }
    }
}
