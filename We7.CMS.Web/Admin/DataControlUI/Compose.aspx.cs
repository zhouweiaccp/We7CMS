using System;
using System.Xml;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Config;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Framework.Config;
using We7.CMS.UI.Template;

namespace We7.CMS.Web.Admin.DataControlUI
{
    public partial class Compose : BasePage
    {
        /// <summary>
        /// 母版模式
        /// </summary>
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
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
                    if(!string.IsNullOrEmpty(Request["file"]))
                        t.FileName = We7Helper.Base64Decode(Request["file"]);
                    t.IsSubTemplate = (Request["templateSub"] != null && Request["templateSub"].ToString().ToLower() == "sub");
                    t.IsMasterPage = (Request["templateSub"] != null && Request["templateSub"].ToString().ToLower() == "master");
                    if (!string.IsNullOrEmpty(Request["folder"]))
                        t.SkinFolder = We7Helper.Base64Decode(Request["folder"]);
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
                        else if(t.IsSubTemplate)
                        {
                            string i = CreateIndex("subtemplate");
                            t.Name = "自定义子模板"+i;
                            t.FileName = "subtemplate"+i+".ascx";
                        }
                        else if (t.IsMasterPage)
                        {
                            t.Name = "会员中心母版";
                            t.FileName = "content.Master";
                        }
                        else
                        {
                            string i = CreateIndex("mytemplate");
                            t.Name = "自定义普通模板" +i;
                            t.FileName = "mytemplate" + i + ".ascx";
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

        string CreateIndex(string fileName)
        {
            fileName = fileName.ToLower();
            string root = Server.MapPath("~/_skins/" +Path.GetFileNameWithoutExtension(GeneralConfigs.GetConfig().DefaultTemplateGroupFileName));
            string[] fs=Directory.GetFiles(root, "*.ascx");
            if (fs != null && fs.Length > 0)
            {
                List<string> list = new List<string>();
                Regex regex = new Regex(fileName + @"\d?\.ascx", RegexOptions.IgnoreCase);
                foreach (string s in fs)
                {
                    Match m=regex.Match(s);
                    if (m.Success)
                    {
                        list.Add(m.Value);
                    }
                }
                if (list.Count > 0)
                {
                    list.Sort();
                    string i = Path.GetFileNameWithoutExtension(list[list.Count - 1].ToLower()).ToLower().Replace(fileName, "");
                    return (Convert.ToInt32(i) + 1).ToString();
                }
            }
            return "1";
        }

        TemplateBindConfig BindConfig
        {
            get
            {
                TemplateBindConfig bc = new TemplateBindConfig();
                bc .Handler= Request["handler"];
                bc.Mode = Request["mode"];
                bc.Model = Request["model"];
                return bc;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //传递给CuteEditor计算文件夹路径所需参数
            Session["COMPOSE_URL"] = Constants.TemplateUrlPath;
            Session["WorkPlace"] = "compose";

            if (Request["create"] != null && Request["create"].ToLower() == "success")
                Messages.ShowMessage("模板创建成功！");
        }

        /// <summary>
        /// 页面初始化
        /// </summary>
        protected override void Initialize()
        {
            string currentTemplateGroupName = TemplateHelper.GetCurrentExistTemplateGroup();
            if (string.IsNullOrEmpty(currentTemplateGroupName))
                TemplateHelper.CreateDefaultTemplateGroup();

            try
            {
                We7Helper.Assert(CurrentTemplate.FileName != null, " 非法的参数。");
                if(!CurrentTemplate.IsNew)
                    CurrentTemplate.FromFile(Server.MapPath(String.Format("\\{0}\\{1}", Constants.TemplateBasePath, CurrentTemplate.SkinFolder)), CurrentTemplate.FileName + ".xml");
                CurrentTemplate.FilePath = TemplateHelper.GetTemplatePath(String.Format("{0}/{1}", CurrentTemplate.SkinFolder, CurrentTemplate.FileName));
                FilenameTextBox.Text = CurrentTemplate.FileName;
                if(CurrentTemplate.IsNew)
                    ActionLiteral.Text = "新建";
                else
                    ActionLiteral.Text = "编辑";
                ActionLiteral.Text += CurrentTemplate.TemplateTypeText;

                NameTextbox.Text = CurrentTemplate.Name;

                this.Page.Title = ActionLiteral.Text + NameLabel.Text;
                if (CurrentTemplate.IsNew)
                {
                    FilenameTextBox.Text = String.Format("{0}", CurrentTemplate.FileName);
                    FilenameTextBox.Visible = true;
                }
                else
                {
                    SummaryLabel.Text = String.Format("{0}", CurrentTemplate.FileName);
                    FilenameTextBox.Visible = false;
                }

                string fn = Server.MapPath(CurrentTemplate.FilePath);
                if (CDHelper.Config.SiteBuildState == "run") //处于运行状态，启用副本
                {
                    PublishSpan.Visible = true;
                    CurrentTemplate.EditFileFullPath = TemplateCopies.GetThisTemplateCopy(CurrentTemplate, FilenameTextBox.Text);
                    if (File.Exists(fn) || File.Exists(CurrentTemplate.EditFileFullPath))
                    {
                        LoadTemplateCopy(CurrentTemplate);
                    }

                    InitCopyControls(CurrentTemplate.EditFileFullPath);
                    SummaryLabel.Text += "编辑后的内容将保存到副本中，发布后才会正式启用。";
                }
                else
                {
                    PublishSpan.Visible = false;
                    if (File.Exists(fn) )
                    {
                        LoadTemplateFromFile(fn);
                    }
                    else
                    {
                        //SummaryLabel.Text = String.Format("文件将保存到{0}，", CurrentTemplate.FileName);
                        TemplateContentTextBox.Value = @"<html>
    <head>
        <title></title>
   </head>
    <body>
    </body>
</html>";
                    }
                    InitCopyControls(fn);
                }

                TemplatePathTextBox.Text = Constants.TemplateUrlPath;
                string path = string.Format("/{0}/{1}", Constants.TemplateBasePath, CurrentTemplate.SkinFolder);
                
                PrevewDropDownList.Visible = !CurrentTemplate.IsMasterPage;
            }
            catch (Exception ex)
            {
                Messages.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 导入静态页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImportButton_Click(object sender, EventArgs e)
        {
            string fn = ImportFileTextBox.Text;
            //string fullName = Path.Combine(Server.MapPath("~"), fn);
            string fullName = Server.MapPath(fn);
            string root = Constants.TemplateUrlPath + "/";
            HTFormatter hf = new HTFormatter();
            hf.Root = root;
            hf.FileName = fullName;
            hf.Process();
            TemplateContentTextBox.Value = hf.Output;
            Messages.ShowMessage("文件" + fn + "导入成功！");
        }

        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                SaveTemplateInfo();

                if (CDHelper.Config.SiteBuildState == "run") //处于运行状态，启用副本
                {
                    SaveTemplate("~" + CurrentTemplate.SkinFolder, CurrentTemplate.EditFileFullPath);//保相模版到副本
                }
                else
                {
                    SaveTemplate(CurrentTemplate.SkinFolder, Server.MapPath(CurrentTemplate.FilePath));//保存到正本
                }
                OriginalInput = TemplateContentTextBox.Value;

                if(BindConfig.Enough)
                    TemplateHelper.SaveTemplateBind(BindConfig, CurrentTemplate.SkinFolder, CurrentTemplate.FileName);

                //记录日志
                string content = string.Format("编辑{0}的内容", NameLabel.Text.Replace("编辑" + CurrentTemplate.Name + "", ""));
                AddLog("编辑" + CurrentTemplate.Name + "内容", content);

                if (CurrentTemplate.IsNew)
                {
                    string fileName = We7Helper.Base64Encode(CurrentTemplate.FileName);
                    string path = We7Helper.Base64Encode(CurrentTemplate.SkinFolder);
                    string url = String.Format("/admin/DataControlUI/Compose.aspx?file={0}&folder={1}&create=success", fileName, path);
                    Response.Redirect(url, true);
                }
                else
                {
                    Messages.ShowMessage("" + CurrentTemplate.FileName + "文件成功保存！" + DateTime.Now.ToLongTimeString());
                }
            }
            catch (Exception ex)
            {
                Messages.ShowError("无法保存模板，错误为：" + ex.Message);
            }
        }

        /// <summary>
        /// 保存模板信息文件.ascx.xml文件
        /// </summary>
        void SaveTemplateInfo()
        {
            if (CurrentTemplate.IsNew|| CurrentTemplate.Name!=NameTextbox.Text)
            {
                CurrentTemplate.FilePath = Regex.Replace(CurrentTemplate.FilePath, CurrentTemplate.FileName, FilenameTextBox.Text,RegexOptions.IgnoreCase);
                CurrentTemplate.FileName = FilenameTextBox.Text;
                if (!CurrentTemplate.FileName.EndsWith(".ascx"))
                    CurrentTemplate.FileName += ".ascx";
                CurrentTemplate.Name = NameTextbox.Text;

                if (CurrentTemplate.IsNew && File.Exists(Server.MapPath(CurrentTemplate.FilePath)))
                {
                    throw new Exception("当前模板已存在，请选用其它模板文件名称");
                }

                TemplateHelper.SaveTemplate(CurrentTemplate, CurrentTemplate.SkinFolder);
            }
        }

        /// <summary>
        /// 发布到正本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PublishButton_Click(object sender, EventArgs e)
        {
            PublishTemplateCopy();
        }
        /// <summary>
        /// 复制模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CopyButton_Click(object sender, EventArgs e)
        {
            string fn = TemplatePathTextBox.Text;
            fn = Path.Combine(fn, CopyIDTextBox.Text);
            LoadTemplateFromFile(Server.MapPath(fn));
            Messages.ShowMessage("模板" + fn + "载入成功！");
        }

        /// <summary>
        /// 上一版本内容
        /// </summary>
        string OriginalInput
        {
            get
            {
                if (ViewState["WE$OriginalInput"] == null)
                {
                    ViewState["WE$OriginalInput"]=String.Empty;
                }
                return ViewState["WE$OriginalInput"] as string;
            }
            set
            {
                ViewState["WE$OriginalInput"] = value;
            }
        }

        /// <summary>
        /// 保存模版
        /// </summary>
        /// <param name="template">模版组名称</param>
        /// <param name="filePath">当前模版文件路径</param>
        void SaveTemplate(string template,string filePath)
        {
            TemplateProcessor tp = new TemplateProcessor(template,new WidgetTemplateProcessorHelper());
            tp.OrignInput = OriginalInput;

            tp.Input = TemplateContentTextBox.Value;
            tp.IsSubTemplate = CurrentTemplate.IsSubTemplate;
            tp.IsMasterPage = CurrentTemplate.IsMasterPage;
            tp.FromVisualBoxText();
            tp.FileName = filePath;
            tp.Save();
        }

        /// <summary>
        /// 加载模板内容
        /// </summary>
        /// <param name="fn"></param>
        void LoadTemplateFromFile(string fn)
        {
            TemplateProcessor pa = new TemplateProcessor(CurrentTemplate.SkinFolder.TrimStart('~'),new WidgetTemplateProcessorHelper());
            pa.FileName = fn;
            pa.IsSubTemplate = CurrentTemplate.IsSubTemplate;
            pa.IsMasterPage = CurrentTemplate.IsMasterPage;

            pa.Load();
            TemplateContentTextBox.Value = pa.HtmlContent;
            OriginalInput = TemplateContentTextBox.Value;
        }

        /// <summary>
        /// 初始化副本相关控件
        /// </summary>
        private void InitCopyControls(string editFileFullPath)
        {
            PrevewDropDownList.Items.Clear();
            PrevewDropDownList.Items.Add(new ListItem("--预览地址--", ""));
            string editUrl = editFileFullPath.Replace(Server.MapPath("~/"), "").Replace("\\", "/");
            if (!editUrl.StartsWith("/"))
                editUrl = "/" + editUrl;
            //BUG修正:无法取到预览地址
            string[] parts = editUrl.Split('/');
            FileTextBox.Text = parts[parts.Length - 1];
            List<string> prevews = TemplateHelper.GetPrevewUrls(FileTextBox.Text, CurrentTemplate.SkinFolder);
            foreach (string p in prevews)
            {
                PrevewDropDownList.Items.Add(new ListItem(p, string.Format("{0}?template={1}", p, editUrl)));
            }
        }

        /// <summary>
        /// 将本副本发布到正本
        /// </summary>
        /// <returns></returns>
        bool PublishTemplateCopy()
        {
            string fn = Server.MapPath(CurrentTemplate.FilePath);
            try
            {
                TemplateCopies.PublistCopy(CurrentTemplate,FilenameTextBox.Text,TemplateContentTextBox.Value);
                TemplateProcessor.PublicTemplate(CurrentTemplate.EditFileFullPath, fn);
                //File.Copy(EditFileName, fn, true);
                Messages.ShowMessage("副本已成功发布！");
                return true;
            }
            catch (Exception ex)
            {
                Messages.ShowError("发布模板副本出错。错误：" + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 装载模板副本内容
        /// </summary>
        /// <param name="tp"></param>
         void LoadTemplateCopy(Template tp)
        {
            string copyFile = TemplateCopies.GetThisTemplateCopy(tp, FilenameTextBox.Text);
            FileInfo cfi = new FileInfo(copyFile);
            FileInfo fi = new FileInfo(tp.FileName);

            if (fi.Exists && (!cfi.Exists || cfi.LastWriteTime < fi.LastWriteTime))
                TemplateCopies.MergeToTemplateCopy(tp.FileName,tp);
            LoadTemplateFromFile(copyFile);
        }
    }
}