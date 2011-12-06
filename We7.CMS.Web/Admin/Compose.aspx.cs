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
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class Compose : BasePage
    {

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        protected string FileName
        {
            get { return (string)ViewState["$VS_FILENAME"]; }
            set { ViewState["$VS_FILENAME"] = value; }
        }

        protected string BodyText
        {
            get { return (string)ViewState["$VS_BODYTEXT"]; }
            set { ViewState["$VS_BODYTEXT"] = value; }
        }

        bool IsSubTemplate
        {
            get
            {
                if (Request["templateSub"] != null)
                    return true;
                else
                {
                    return false;
                }
            }
        }

        string FileFolder
        {
            
            get 
            {
                if (Request["folder"] != null)
                    return Request["folder"];
                else
                {
                    string tmpfolder = CDHelper.Config.DefaultTemplateGroupFileName;
                    tmpfolder = tmpfolder.Remove(tmpfolder.IndexOf("."));
                    return tmpfolder;
                }
            }
        }

        string CssPath
        {
            get { return Server.MapPath("\\" + Constants.TemplateBasePath + "\\styles"); }
        }

        public string MyPageTitle
        {
            get
            {
                return NameLabel.Text + "：" + SummaryLabel.Text;
            }
        }

        public static bool EnableSiteSkins
        {
            get
            {
                string _default = SiteSettingHelper.Instance.Config.EnableSiteSkins;
                if (_default.ToLower() == "true")
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 信息提示关键字
        /// </summary>
        string InfoKey
        {
            get
            {
                if (IsSubTemplate)
                {
                    return "子模板";
                }
                else
                {
                    return "模板";
                }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //传递给CuteEditor计算文件夹路径所需参数(thhim-2007-10-31)
            Session["COMPOSE_URL"] = Constants.TemplateUrlPath;
            Session["WorkPlace"] = "compose";
        }

        protected override void Initialize()
        {
            string file = FileTextBox.Text= Request["file"];
            We7Helper.Assert(file != null, " 非法的参数。");

            Template tp = new Template();

            if (EnableSiteSkins)
            {
                tp.FromFile(Server.MapPath(String.Format("\\{0}\\{1}", Constants.TemplateBasePath, FileFolder)), file + ".xml");
                FileName = TemplateHelper.GetTemplatePath(String.Format("{0}/{1}", FileFolder, tp.FileName));
            }
            else
            {
                tp.FromFile(Server.MapPath(Constants.TemplateBasePath), file + ".xml");
                FileName = TemplateHelper.GetTemplatePath(tp.FileName);
            }

            NameLabel.Text = "编辑模板";
            SummaryLabel.Text = String.Format("正在编辑" + InfoKey + "文件 {0}。", tp.FileName);
           
            string fn = Server.MapPath(FileName);
            if (File.Exists(fn))
            {
                LoadTemplateFromFile(fn);
            }
            else
            {
                SummaryLabel.Text=String.Format("新建的" + InfoKey + "文件，将被保存到文件 {0}", FileName);
            }
            TemplatePathTextBox.Text = Constants.TemplateUrlPath;
            string path = string.Format("/{0}/{1}", Constants.TemplateBasePath, FileFolder);
           
        }


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

            //TODO: Get the content inside <head> by using Regx

            if (!IsSubTemplate)
            {
                string pat = @"\<head[^>]*\>([\w\W]*?)\</head\>";
                Regex r = new Regex(pat, RegexOptions.IgnoreCase);
                Match match = r.Match(hf.Output);

                if (!match.Success)
                {
                    throw new Exception("head标记不正确，无法解析，请检查模版文件的正确性！");
                }
                else
                    HeaderTextBox.Text = match.Groups[1].Value;
            }

            Messages.ShowMessage("文件" + fn + "导入成功！");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            Templator tp = new Templator();
            tp.Input = TemplateContentTextBox.Value;
            tp.IsSubTemplate = IsSubTemplate;

            tp.FromVisualBoxText();
            tp.BodyText = BodyText;
            tp.HeadContent = HeaderTextBox.Text;
            tp.FileName = Server.MapPath(FileName);
            tp.Save();

            Messages.ShowMessage("" + InfoKey + "文件成功保存！" + DateTime.Now.ToLongTimeString());

            //记录日志
            string content = string.Format("编辑{0}的内容", NameLabel.Text.Replace("编辑" + InfoKey + "", ""));
            AddLog("编辑" + InfoKey + "内容", content);
        }

        void LoadTemplateFromFile(string fn)
        {
            Templator pa = new Templator();
            pa.FileName = fn;
            pa.IsSubTemplate = IsSubTemplate;

            pa.Load();
            TemplateContentTextBox.Value = We7Helper.FilterXMLChars(pa.BodyContent);
            HeaderTextBox.Text = pa.HeadContent;
            BodyText = pa.BodyText;
        }

        protected void CopyButton_Click(object sender, EventArgs e)
        {
            //string fn = TemplateHelper.GetTemplatePath(CopyIDTextBox.Text);原路径错误
            string fn = TemplatePathTextBox.Text;
            fn = Path.Combine(fn, CopyIDTextBox.Text);
            LoadTemplateFromFile(Server.MapPath(fn));
            Messages.ShowMessage("模板" + fn + "载入成功！");
        }
    }
}