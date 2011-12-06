using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Thinkment.Data;
using System.Xml;
using We7.CMS.Common;
using We7.Framework.Config;
using System.Text;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateCreate : BasePage
    {
        protected override We7.CMS.Common.Enum.MasterPageMode MasterPageIs
        {
            get
            {
                return We7.CMS.Common.Enum.MasterPageMode.NoMenu;
            }
        }

        string FileName
        {
            get { return Request["tfile"]; }
        }

        string TemplateGroupFileName
        {
            get { return Request["tgfile"]; }
        }

        TemplateBindConfig BindConfig
        {
            get
            {
                TemplateBindConfig bc = new TemplateBindConfig();
                bc.Handler = Request["handler"];
                bc.Mode = Request["mode"];
                bc.Model = Request["model"];
                return bc;
            }
        }

        /// <summary>
        /// 正在编辑的模板对象
        /// </summary>
        Template CurrentTemplate
        {
            get
            {
                if (ViewState["$VS_TEMPLATE"] == null)
                {
                    Template t = new Template();
                    if (!string.IsNullOrEmpty(FileName))
                        t.FileName = FileName;
                    if (!string.IsNullOrEmpty(TemplateGroupFileName))
                    {
                        if (TemplateGroupFileName.ToLower().EndsWith(".xml"))
                            t.SkinFolder = TemplateGroupFileName.Remove(TemplateGroupFileName.LastIndexOf("."));
                        else
                            t.SkinFolder = TemplateGroupFileName;
                    }
                    else
                    {
                        string tmpfolder = GeneralConfigs.GetConfig().DefaultTemplateGroupFileName;
                        tmpfolder = tmpfolder.Remove(tmpfolder.IndexOf("."));
                        t.SkinFolder = tmpfolder;
                    }

                    if (string.IsNullOrEmpty(t.FileName))
                    {
                        TemplateBindConfig tbc = TemplateHelper.GetTemplateConfigSentence(BindConfig);
                        if (!string.IsNullOrEmpty(tbc.Handler))
                        {
                            t.Name = tbc.Description;
                            if (string.IsNullOrEmpty(tbc.Mode)) tbc.Mode = "default";
                            string model = tbc.Model;
                            if (!string.IsNullOrEmpty(model)) model += ".";
                            t.FileName = tbc.Handler + "." + model + tbc.Mode + ".ascx";
                        }
                        else
                        {
                            t.Name = "自定义普通模板1";
                            t.FileName = "mytemplate1";//.ascx";
                        }

                        t.Created = DateTime.Now;
                        t.TemplateType = t.IsSubTemplate ? "0" : (t.IsMasterPage ? "9" : "1");
                        t.IsNew = true;
                    }

                    ViewState["$VS_TEMPLATE"] = t;
                }
                return (Template)ViewState["$VS_TEMPLATE"];
            }
            set
            {
                ViewState["$VS_TEMPLATE"] = value;
            }
        }

        /// <summary>
        /// 初始化数据信息
        /// </summary>
        protected override void Initialize()
        {
            if (CurrentTemplate.IsNew)
            {
                FileNameTextBox.Text = String.Format("{0}", CurrentTemplate.FileName);
                NameTextBox.Text = CurrentTemplate.Name;
            }
        }

        /// <summary>
        /// 信息保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {

            string content = "";
            string logtitle = "";
            Template t = CurrentTemplate;

            if (FileNameTextBox.Text == "请输入文件名" || FileNameTextBox.Text.Trim() == "")
            {
                FileNameTextBox.Text = NameTextBox.Text;// +".ascx";
            }

            //if (!FileNameTextBox.Text.EndsWith(".ascx", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    FileNameTextBox.Text += ".ascx";
            //}

            if (File.Exists(Path.Combine(TemplateHelper.DefaultTemplateGroupPath, FileNameTextBox.Text + ".xml")))
            {
                Messages.ShowError("模板文件【" + FileNameTextBox.Text + "】已存在，请重新命名！");
                return;
            }
            if (FileNameTextBox.Text.EndsWith(".ascx"))//如果是.ascx结尾，则不添加
            {
                t.FileName = FileNameTextBox.Text;
            }
            else
            {
                t.FileName = FileNameTextBox.Text + ".ascx";
            }
           
            t.Created = DateTime.Now;
            t.IsSubTemplate = false;
            t.IsVisualTemplate = true;
            t.Name = NameTextBox.Text;
            t.Description = DescriptionTextBox.Text;
            TemplateHelper.SaveTemplate(t, TemplateGroupFileName);

            t.FilePath = TemplateHelper.GetTemplatePath(String.Format("{0}/{1}", t.SkinFolder, t.FileName));

            File.WriteAllText(Server.MapPath(t.FilePath), "", Encoding.UTF8);
            if (BindConfig.Enough)
                TemplateHelper.SaveTemplateBind(BindConfig, CurrentTemplate.SkinFolder, CurrentTemplate.FileName);


            //记录日志
            content = string.Format("新建模板“{0}”", NameTextBox.Text);
            logtitle = "新建模板";
            AddLog(logtitle, content);

            string url = "SelectTemplate.aspx?folder=" + Path.GetFileNameWithoutExtension(GeneralConfigs.GetConfig().DefaultTemplateGroupFileName);
            url = We7Helper.AddParamToUrl(url, "file", t.FileName);
            Response.Redirect(url);
        }

    }
}
