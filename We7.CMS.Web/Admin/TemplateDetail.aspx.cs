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
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateDetail : BasePage
    {
        string fileName = "";
        string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        string TemplateGroupFileName
        {
            get { return Request["tgfile"]; }
        }

        string FileAlias
        {
            get { return Request["fa"]; }
        }

        string IsDetail
        {
            get { return Request["isd"]; }
        }
        string FileNameAlias
        {
            get { return Request["fileName"]; }
        }
        string AscxFile
        {
            get { return Request["file"]; }
        }

        protected override void Initialize()
        {
            ReturnHyperLink.NavigateUrl = String.Format("TemplateGroupDetail.aspx?file={0}", TemplateGroupFileName);
            //通过模板别名和是否详细模板读取
            if (FileAlias != null && IsDetail != null && TemplateGroupFileName != null)
            {
                //读取模板别名
                TemplateGroup tg = TemplateHelper.GetTemplateGroup(TemplateGroupFileName);
                foreach (TemplateGroup.Item it in tg.Items)
                {
                    if (it.Alias == FileAlias && it.IsDetailTemplate.ToString() == IsDetail)
                    {
                        DeleteItemAliasTextBox.Text = AliasTextBox.Text = AliasWordsTextBox.Text = it.Alias;
                        We7Helper.SetDropdownList(IsDetailTemplateDropDownList, it.IsDetailTemplate ? Boolean.TrueString : Boolean.FalseString);
                        DeleteItemIsDetailTextBox.Text = it.IsDetailTemplate ? Boolean.TrueString : Boolean.FalseString;
                        FileName = it.Template;
                        AliasPanel.Visible = true;
                    }
                }
            }
            //通过模板文件名读取
            if (AscxFile != null && TemplateGroupFileName != null)
            {
                FileName = AscxFile;
                AliasPanel.Visible = false;
            }
            if (FileNameAlias != null && FileNameAlias != "")
            {
                FileName = FileNameAlias;
            }
            if (FileName != null && FileName != "" && TemplateGroupFileName != null)
            {
                Template t = TemplateHelper.GetTemplate(FileName + ".xml", Path.GetFileNameWithoutExtension(TemplateGroupFileName));
                if (t != null)
                {
                    NameTextBox.Text = t.Name;
                    DescriptionTextBox.Text = t.Description;
                    CreatedLabel.Text = t.Created.ToString();
                    FileNameTextBox.Text = t.FileName;
                    FileNameTextBox.ReadOnly = true;
                    We7Helper.SetDropdownList(TypeList, t.IsSubTemplateText);
                    CodeDropDownList.SelectedValue = t.IsCode ? Boolean.TrueString : Boolean.FalseString;
                    if (t.IsSubTemplate)
                    {
                        ComposeHyperLink.NavigateUrl = String.Format("Compose.aspx?file={0}&folder={1}&&templateSub=sub", t.FileName, Path.GetFileNameWithoutExtension(TemplateGroupFileName));
                        AliasPanel.Visible = false;
                    }
                    else
                    {
                        ComposeHyperLink.NavigateUrl = String.Format("Compose.aspx?file={0}&folder={1}", t.FileName, Path.GetFileNameWithoutExtension(TemplateGroupFileName));
                        AliasPanel.Visible = true;
                    }
                    TitleLabel.Text = "编辑模板文件";
                    SummaryLabel.Text = String.Format("修改模板文件 {0}", t.FileName);
                    //TypeList.Enabled = false;
                    ComposeHyperLink.Enabled = true;
                }
            }
            else if (FileName == null || FileName == "" && TemplateGroupFileName != null)
            {
                DateTime now = DateTime.Now;

                ComposeHyperLink.NavigateUrl = String.Format("Compose.aspx?file={0}&folder={1}", FileAlias, Path.GetFileNameWithoutExtension(TemplateGroupFileName));
                ComposeHyperLink.Enabled = true;
            }
            else
            {
                Messages.ShowError("Url信息不完整。");
                ComposeHyperLink.NavigateUrl = SaveHyperLink.NavigateUrl = "";
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            bool isSub = TypeList.SelectedValue == Boolean.TrueString;
            FileDetailTextBox.Text = FileNameTextBox.Text.Trim() + ".ascx";

            ////子模板不判断别名为空
            //if (AliasWordsTextBox.Text == "" && !isSub)
            //{
            //    Messages.ShowError("请选择一个别名。");
            //}
            //else
            //{
            Template t;
            //编辑模板
            if (FileNameTextBox.ReadOnly)
            {
                t = TemplateHelper.GetTemplate(FileNameTextBox.Text + ".xml", Path.GetFileNameWithoutExtension(TemplateGroupFileName));

                if (AliasWordsTextBox.Text != "" && !isSub)
                {
                    DeleteTemplateGroupItem();
                    //AddTemplateGroupItem();
                }
                //记录日志
                string content = string.Format("编辑模板信息“{0}”", NameTextBox.Text);
                AddLog("编辑模板信息", content);
            }
            //新建模板
            else
            {
                t = new Template();
                t.FileName = FileNameTextBox.Text;
                if (!t.FileName.EndsWith(".ascx", StringComparison.CurrentCultureIgnoreCase))
                {
                    t.FileName += ".ascx";
                    FileNameTextBox.Text = t.FileName;
                }
                t.Created = DateTime.Now;
                //记录日志
                string content = string.Format("新建模板“{0}”", NameTextBox.Text);
                AddLog("新建模板", content);
            }

            t.Name = NameTextBox.Text;
            t.Description = DescriptionTextBox.Text;
            t.IsSubTemplate = TypeList.SelectedValue == Boolean.TrueString;
            t.IsCode = CodeDropDownList.SelectedValue == Boolean.TrueString;
            TitleLabel.Text = "编辑模板文件";
            SummaryLabel.Text = String.Format("修改模板文件 {0}", t.FileName);

            if (AliasWordsTextBox.Text != "" && !isSub)
            {
                if (!AddTemplateGroupItem()) return;
            }
            TemplateHelper.SaveTemplate(t, Path.GetFileNameWithoutExtension(TemplateGroupFileName));
            string ReturnURL = "";
            if (AliasWordsTextBox.Text != null && AliasWordsTextBox.Text != "")
            { ReturnURL = String.Format("TemplateDetail.aspx?saved=1&tgfile={0}&fa={1}&isd={2}", TemplateGroupFileName, AliasWordsTextBox.Text, IsDetailTemplateDropDownList.SelectedValue == Boolean.TrueString); }
            else
            {
                ReturnURL = String.Format("TemplateDetail.aspx?saved=1&tgfile={0}&fileName={1}&isd={2}", TemplateGroupFileName, FileNameTextBox.Text, IsDetailTemplateDropDownList.SelectedValue == Boolean.TrueString);
            }
            if (isSub)
            {
                ReturnURL = String.Format("TemplateDetail.aspx?saved=1&tgfile={0}&file={1}", TemplateGroupFileName, FileNameTextBox.Text);
            }
            Response.Redirect(ReturnURL);
        }

        protected void TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TypeList.SelectedValue == Boolean.TrueString)
            {
                AliasPanel.Visible = false;
            }
            else
            {
                AliasPanel.Visible = true;
            }
        }

        protected void IsDetailTemplateDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void TemplatesTagDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AliasTextBox.Text = AliasWordsTextBox.Text = TemplatesTagDropDownList.SelectedValue;
        }

        bool AddTemplateGroupItem()
        {
            //子模板不记录到模板组XML
            if (TypeList.SelectedValue != Boolean.TrueString)
            {
                TemplateGroup.Item item = null;

                TemplateGroup tg = TemplateHelper.GetTemplateGroup(TemplateGroupFileName);

                foreach (TemplateGroup.Item it in tg.Items)
                {
                    if (it.Alias == AliasWordsTextBox.Text && it.IsDetailTemplate.ToString() == IsDetailTemplateDropDownList.SelectedValue)
                    {
                        item = it;
                    }
                }
                if (item == null)
                {
                    item = new TemplateGroup.Item();
                    item.Alias = AliasWordsTextBox.Text;
                    item.Template = FileNameTextBox.Text;
                    item.TemplateText = TemplateHelper.GetTemplateName(item.Template);
                    item.IsDetailTemplate = IsDetailTemplateDropDownList.SelectedValue == Boolean.TrueString;

                    tg.Items.Add(item);

                    string fn = TemplateHelper.SaveTemplateGroupAndPreviewFile(tg, Path.GetFileNameWithoutExtension(TemplateGroupFileName));
                    return true;
                }
                else
                    Messages.ShowError("无法增加模板记录到模板组，该别名已被使用；请使用其他别名再试！");
            }
            return false;
        }

        void DeleteTemplateGroupItem()
        {
            //子模板不记录到模板组XML
            if (TypeList.SelectedValue != Boolean.TrueString)
            {
                TemplateGroup.Item del = null;
                TemplateGroup tg = TemplateHelper.GetTemplateGroup(TemplateGroupFileName);
                foreach (TemplateGroup.Item it in tg.Items)
                {
                    if (it.Alias == DeleteItemAliasTextBox.Text && it.IsDetailTemplate.ToString() == DeleteItemIsDetailTextBox.Text)
                    {
                        del = it;
                        break;
                    }
                }
                if (del != null)
                {
                    tg.Items.Remove(del);
                    string fn = TemplateHelper.SaveTemplateGroupAndPreviewFile(tg, Path.GetFileNameWithoutExtension(TemplateGroupFileName));
                }
                DeleteItemAliasTextBox.Text = "";
                DeleteItemIsDetailTextBox.Text = "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["saved"] != null && Request["saved"].ToString() == "1")
                    Messages.ShowMessage("模板信息已成功修改。");
            }
        }

    }
}
