using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;

using Thinkment.Data;
using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin
{
    public partial class DataControlBuilder :BasePage
    {

        string ControlFile
        {
            get { return Request["file"]; }
        }
        string Template
        {
            get { return Request["template"]; }
        }

        protected override We7.CMS.Common.Enum.MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        protected override void Initialize()
        {
            Response.Expires = -1;

            if (ControlFile != null)
            {
                DataControl dc = TemplateHelper.GetDataControl(ControlFile, true);
                NameLabel.Text = dc.Description;
                SummaryLabel.Text = dc.Name;

                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                doc.AppendChild(dc.ToXml(doc));
                FieldsTextBox.Text = doc.OuterXml;

                IsFirstTextBox.Text = Request["isFirst"];
                if (ControlFile != null && Template != null)
                {
                    InitializeCssFile();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ControlFile != null)
            {
                string path = Constants.ControlUrlPath + "/" + ControlFile;
                if (!File.Exists(Server.MapPath(path)))
                {
                    Response.Write("<p style='font-size:14px;color:red'>找不到控件的配置文件，请确认控件是否存在或控件标签格式是否正确！</p>");
                    Response.End();
                }
            }
        }

        void InitializeCssFile()
        {
            string cfn = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(ControlFile));
            string tn = Path.GetFileNameWithoutExtension(Template);

            Templator tp = new Templator();
            tp.FileName = Server.MapPath(TemplateHelper.GetTemplatePath(Template));

            string cssFile = "";
            WeControl c = new WeControl();
            c.TagName = cfn;
            if (tp.CopyStyleSheet(c, ref cssFile))
            {
                EditCssHyperLink.NavigateUrl = string.Format("manage/CssDetail.aspx?file={0}_{1}.css&folder=controls", tn, cfn);
                CssFileTextBox.Text = string.Format("{0}_{1}.css", tn, cfn);
            }
            StylePathTextBox.Text = Constants.TemplateUrlPath.Remove(0,1) + "/styles";
        }
    }
}
