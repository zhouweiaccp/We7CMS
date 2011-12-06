using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;

using Thinkment.Data;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using System.Web;
using We7.CMS.WebControls.Core;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.DataControlUI
{
    public partial class DataControlBuilder : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        /// <summary>
        /// 是否是模型控件
        /// </summary>
        protected bool IsModel
        {
            get
            {
                DataControlInfo dcinfo=DataControlHelper.GetDataControlInfo(ControlFile);
                return String.IsNullOrEmpty(dcinfo.Model);
            }
        }

        string ControlFile
        {
            get { return Request["file"]; }
        }
        protected string Template
        {
            get { return Request["template"]; }
        }

        string Folder
        {
            get 
            {
                if (!String.IsNullOrEmpty(Request["folder"]))
                    return Request["folder"];
                else
                {
                    string tmpfolder = CDHelper.Config.DefaultTemplateGroupFileName;
                    if (!String.IsNullOrEmpty(tmpfolder)&&tmpfolder.IndexOf(".")>-1)
                        tmpfolder = tmpfolder.Remove(tmpfolder.IndexOf("."));
                    return tmpfolder;
                }
            }
        }


        protected override void Initialize()
        {
            Response.Expires = -1;

            if (ControlFile != null)
            {
                //DataControlInfo  info = DataControlHelper.GetDataControlInfoByPath(ControlFile);
                //DataControlInfo info=DataControlHelper.GetDataControlInfoByPath(ControlFile);

                BaseControlHelper Helper = new BaseControlHelper();
                DataControlInfo info = Helper.GetIntegrationInfoByPath(ControlFile);
                NameLabel.Text = info.Name;
                SummaryLabel.Text = info.Desc;

                FieldsTextBox.Text = info.ToJson();

                IsFirstTextBox.Text = Request["isFirst"];
                GroupHidden.Value = Folder.Trim();
                ControlHidden.Value = ControlFile;
                if (ControlFile != null && Template != null)
                {
                    InitializeCssFile();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ControlFile) ||  
                String.IsNullOrEmpty(ControlFile.Split('.')[0]) ||  (
                !File.Exists(HttpContext.Current.Server.MapPath(ControlFile)) &&
                !File.Exists(Path.Combine(DataControlHelper.GetDataControlPath(ControlFile.Split('.')[0]),Constants.We7ControlConfigFile))))
            {

                Response.Write("<p style='font-size:14px;color:red'>找不到控件的配置文件，请确认控件是否存在或控件标签格式是否正确！</p>");
                Response.End();
            }
        }

        void InitializeCssFile()
        {
            //string cfn = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(ControlFile));
            //string tn = Path.GetFileNameWithoutExtension(Template);

            //Templator tp = new Templator();
            //tp.FileName = Server.MapPath(TemplateHelper.GetTemplatePath(Template));

            //string cssFile = "";
            //WeControl c = new WeControl();
            //c.TagName = cfn;
            //if (tp.CopyStyleSheet(c, ref cssFile))
            //{
            //    //EditCssHyperLink.NavigateUrl = string.Format("manage/CssDetail.aspx?file={0}_{1}.css&folder=controls", tn, cfn);
            //    CssFileTextBox.Text = string.Format("{0}_{1}.css", tn, cfn);
            //}
            //StylePathTextBox.Text = Constants.TemplateUrlPath.Remove(0, 1) + "/styles";
        }

        protected bool IsControl()
        {
            BaseControlHelper Helper = new BaseControlHelper();
            return Helper.IsControl(ControlFile);
        }

        protected bool IsWidget()
        {
            string physicalPath = HttpContext.Current.Server.MapPath("/Widgets/WidgetsIndex.xml");
            FileInfo fi = new FileInfo(physicalPath);
            if(fi.Exists){
                XmlNodeList listXmlNodes  =  XmlHelper.GetXmlNodeList(physicalPath, "//group//widget//type");
                foreach (XmlNode node in listXmlNodes)
                {
                    if (node.Attributes["file"].Value == ControlFile)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
