using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using We7.CMS.Common;
using We7.CMS.Config;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateGroup_Info : BaseUserControl
    {
        string FileName
        {
            get
            {
                if (Request["file"] != null && Request["file"].Trim().ToLower() == "default")
                    return CDHelper.Config.DefaultTemplateGroupFileName;
                else
                    return Request["file"];
            }
        }

        protected SkinInfo Data
        {
            get { return ViewState["$VS_SKIN_DATA"] as SkinInfo; }
            set { ViewState["$VS_SKIN_DATA"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetSkinInfo();
            ShowSkinInfo();

            string folderName = NameTextBox.Text.ToLower().Trim();
            if (!string.IsNullOrEmpty(folderName) && ExistFolder(folderName))
                NameTextBox.Enabled = false;
        }

        /// <summary>
        /// 获取模板组信息
        /// </summary>
        void GetSkinInfo()
        {
            if (FileName != null)
            {
                string file = FileName;
                if (file != null && file != "" && File.Exists(Path.Combine(TemplateHelper.TemplateGroupPath, file)))
                {
                    Data = TemplateHelper.GetSkinInfo(file);
                    foreach (SkinInfo.SkinItem it in Data.Items)
                    {
                        try
                        {
                            Template t = TemplateHelper.GetTemplate(it.Template + ".xml");
                            if (t != null)
                                it.TemplateText = t.Name;
                        }
                        catch
                        {
                            it.TemplateText = "";
                            Messages.ShowError("没有找到模板组" + file);
                        }
                    }
                    ShowSkinInfo();
                }                   
            }
        }

        /// <summary>
        /// 获取绑定模板皮肤信息
        /// </summary>
        void ShowSkinInfo()
        {
            if (Data!=null)
            {
                NameTextBox.Text = Data.Name;
                DescriptionTextBox.Text = Data.Description;
                CreatedLabel.Text = Data.Created.ToString();
                FileTextBox.Text = Data.FileName;
            }
        }

        /// <summary>
        /// 模板信息保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//是否是演示站点
            if (Data == null)
            {
                Data = new SkinInfo();
            }
            Data.Name = NameTextBox.Text;
            Data.Description = DescriptionTextBox.Text;
            Data.Ver = GeneralConfigs.GetConfig().ProductVersion;

            string folderName = NameTextBox.Text.ToLower().Trim();
            string fileName = "";
            if (CreateFolder(folderName))
            {
                fileName = TemplateHelper.SaveSkinInfoAndPreviewFile(Data, folderName);

                string content = "";
                string title = "";
                content = string.Format("编辑模板组“{0}”", Data.Name);
                title = "编辑模板组";
                if (FileName == null || FileName.Trim() == "")
                {
                    content = string.Format("新建模板组“{0}”", Data.Name);
                    title = "新建模板组";
                    GeneralConfigInfo config = GeneralConfigs.GetConfig();
                    if (string.IsNullOrEmpty(config.DefaultTemplateGroupFileName))
                    {
                        config.DefaultTemplateGroupFileName = folderName + ".xml";
                        GeneralConfigs.SaveConfig(config);
                        GeneralConfigs.ResetConfig();
                        //Response.Redirect(String.Format("{0}?file={1}.xml", "/admin/Template/TemplateGroupEdit.aspx", folderName),true);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "parent.location='" + String.Format("{0}?file={1}.xml", "/admin/Template/TemplateGroupEdit.aspx", folderName)+ "'", true);
                    }
                }
                AddLog(title, content);
                UploadPreviewFile(fileName);
                Messages.ShowMessage("成功保存模板组信息");
                //Response.Redirect(String.Format("{0}?file={1}.xml", "/admin/Template/TemplateGroupEdit.aspx", folderName));
            }
            else
            {
                //if (FileName != null && FileName.Trim() != "")
                //{
                //    Messages.ShowError("已有此模板组,请重新填写名称！");
                //}
                //else
                //{
                    fileName = TemplateHelper.SaveSkinInfoAndPreviewFile(Data, folderName);
                    UploadPreviewFile(fileName);
                    Messages.ShowMessage("成功保存模板组信息");
                    //Response.Redirect(String.Format("{0}?file={1}.xml", "/admin/Template/TemplateGroupEdit.aspx", folderName));
                //}
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Register();", true);
        }

        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        bool ExistFolder(string folderName)
        {
            string folderPath = Server.MapPath("/"+string.Format("{0}\\{1}", Constants.SiteSkinsBasePath, folderName));
            return Directory.Exists(folderPath);
        }

        /// <summary>
        /// 创建存放文件夹
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        bool CreateFolder(string folderName)
        {
            string folderPath = Server.MapPath("/" + string.Format("{0}\\{1}", Constants.SiteSkinsBasePath, folderName));
            bool isCreate = false;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                isCreate = true;
            }
            return isCreate;
        }

        /// <summary>
        /// 上传预览图片
        /// </summary>
        /// <param name="PreviewFileName"></param>
        void UploadPreviewFile(string PreviewFileName)
        {
            if (PreviewFileUploador.PostedFile.FileName != "")
            {
                if (String.Compare(Path.GetExtension(PreviewFileUploador.FileName), ".jpg", true) == 0)
                {
                    string path = PreviewFileName + Path.GetExtension(PreviewFileUploador.FileName);
                    path = "~/" + Path.Combine(Constants.TemplateGroupBasePath, path);
                    string phyPath = Server.MapPath(path);
                    PreviewFileUploador.SaveAs(phyPath);
                }
                else
                {
                    Messages.ShowMessage("图片必须是jpg文件。");
                }
            }
            else
            {
            }
        }
    }
}