using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common.Enum;
using We7.Framework.Config;
using System.IO;
using System.Text;

namespace We7.CMS.Web.Admin.VisualTemplate
{
    public partial class DataControlCssEditorEx : BasePage
    {
        string controlfile;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string ControlFile
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

        /// <summary>
        /// 
        /// </summary>
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

        string s = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Style
        {
            get
            {
                s = Request["style"];
                if (String.IsNullOrEmpty(s))
                    return "";//throw new Exception("当前样式为空");
                else
                    return s.Trim().Replace(".", "_");
            }
            set { s = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Group
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

        public string Template
        {
            get
            {
                string template=Request["template"];
                template = Path.GetFileNameWithoutExtension(template);
                return template;
            }
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            BindContent();
        }

        /// <summary>
        /// 
        /// </summary>
        void BindContent()
        {
            if (!string.IsNullOrEmpty(Style))
            {
                string control = Path.GetFileNameWithoutExtension(ControlFile);
                string css = CssTxt(control);

                TemplateStyleHelper styleHelper = new TemplateStyleHelper();
                CssTextBox.Text = styleHelper.LoadCss(control, Style, css);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private string CssTxt(string control)
        {
            using (StreamReader reader = new StreamReader(GetStyleFile(control), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="Css"></param>
        /// <returns></returns>
        private bool Write(string control, string Css)
        {
            using (StreamWriter write = new StreamWriter(GetStyleFile(control), false, Encoding.UTF8))
            {
                write.Write(Css);
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private string GetStyleFile(string control)
        {
            FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath(ControlFile));
            DirectoryInfo[] dis = fi.Directory.GetDirectories("Style");
            DirectoryInfo di = dis != null && dis.Length > 0 ? dis[0] : null;
            if (di != null)
            {
                FileInfo[] fs = di.GetFiles("*.css");
                if (fs != null && fs.Length > 0)
                {
                    FileInfo f = fs[0];
                    return f.FullName;
                }
            }
            else
            {
                string stylePath = Path.Combine(fi.DirectoryName, "Style");
                string styleFilePath = Path.Combine(stylePath, string.Format("{0}.css", Path.GetFileNameWithoutExtension(fi.FullName)));
                Directory.CreateDirectory(stylePath);
                File.AppendAllText(styleFilePath, "", Encoding.UTF8);
                return styleFilePath;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newStyle"></param>
        void doJs(string newStyle)
        {
            string js = @"(function(){
            alert('保存成功！');
            window.returnValue={key:'{$KEY}',value:'{$VALUE}'};
            window.close();})()";

            js = js.Replace("{$VALUE}", newStyle.Replace("_", ".")).Replace("{$KEY}", newStyle.Replace("_", "."));

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reutrnValue", js, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void edtSave_Click(object sender, EventArgs e)
        {
            string newcss = this.CssTextBox.Text;
            string control = Path.GetFileNameWithoutExtension(ControlFile);
            string css = CssTxt(control);

            TemplateStyleHelper styleHelper = new TemplateStyleHelper();
            string finishStyle = styleHelper.ReplaceAppendCss(control, string.IsNullOrEmpty(Style) ? control.Replace(".", "_") : Style, Template, css, newcss);
            Write(control, finishStyle);

            doJs(string.IsNullOrEmpty(Style) ? control.Replace(".", "_") : Style);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void edtSaveAs_Click(object sender, EventArgs e)
        {
            string newStyle = Request.Form["NewStyleName"];
            if (!string.IsNullOrEmpty(newStyle))
            {
                newStyle = newStyle.Replace(".", "_");

                string newcss = this.CssTextBox.Text.Replace(Style, newStyle);
                string control = Path.GetFileNameWithoutExtension(ControlFile);
                string css = CssTxt(control);

                TemplateStyleHelper styleHelper = new TemplateStyleHelper();
                css = css + styleHelper.CreateAppendCss(control, newStyle, Group, newcss);
                Write(control, css);

                doJs(newStyle);
            }
        }
    }
}
