using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;

using Thinkment.Data;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.CMS.Module.VisualTemplate.Helpers;
using We7.CMS.WebControls.Core;

namespace We7.CMS.Web.Admin.VisualTemplate
{
    public partial class WidgetBuilder : BasePage
    {
        private WidgetHelper Widgethelper = new WidgetHelper();

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
        //protected bool IsModel
        //{
        //    get
        //    {
        //        DataControlInfo dcinfo = Widgethelper.GetDataControlInfoByPath(ControlFile);                
        //        return String.IsNullOrEmpty(dcinfo.Model);
        //    }
        //}

        string ControlFile
        {
            get { return Request["file"]; }
        }
        protected string Template
        {
            get { return Request["template"]; }
        }

        protected string CssClass
        {
            get
            {
                string s = Request["cssclass"];
                if (s.Equals("undefined"))
                    return Path.GetFileNameWithoutExtension(ControlFile).Replace(".", "_");
                return s.Replace(".", "_");
            }
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
                    if (!String.IsNullOrEmpty(tmpfolder) && tmpfolder.IndexOf(".") > -1)
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
                //得到部件的信息
                FieldsTextBox.Text = new BaseControlHelper().PickUp(ControlFile).ToJson();
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
                String.IsNullOrEmpty(ControlFile.Split('.')[0]) ||
                !File.Exists(Widgethelper.GetDataControlPath(ControlFile)))
            {

                Response.Write("<p style='font-size:14px;color:red'>找不到控件的配置文件，请确认控件是否存在或控件标签格式是否正确！</p>");
                Response.End();
            }
        }

        void InitializeCssFile()
        {
        }
    }
}
