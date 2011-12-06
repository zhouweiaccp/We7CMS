using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Thinkment.Data;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.Framework.Config;
using System.IO;
using System.Xml;
using We7.Model.Core.UI;
using We7.CMS.Common.Enum;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class NewSiteWizard : BasePage
    {
        protected override We7.CMS.Common.Enum.MasterPageMode MasterPageIs
        {
            get
            {
                if (Request["nomenu"] != null)
                {
                    return MasterPageMode.NoMenu;
                }
                else
                {
                    return MasterPageMode.FullMenu;
                }
            }
        }
        public string OrignPath
        {
            get
            {
                return ViewState["_OrignPath"] as string;
            }
            set
            {
                ViewState["_OrignPath"] = value;
            }
        }
        #region

        protected override void Initialize()
        {
            if (Request["nomenu"] != null)
            {
                TitleLabel.Text = "新建站点向导";
                SummaryLabel.Text = "分三步创建新站点";
            }
            else
            {
                TitleLabel.Text = "站点设置向导";
                SummaryLabel.Text = "分三步设置站点";
            }
            CheckDisplay();
            BindSiteConfig();
            BindTemplate();
        }

        /// <summary>
        /// 绑定站点配置第一步
        /// </summary>
        private void BindSiteConfig()
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            txtSiteName.Text = si.SiteTitle;
            txtCopyright.Text = si.Copyright;
            txtSiteFullName.Text = si.SiteFullName.Trim();
            txtIcpInfo.Text = si.IcpInfo.Trim();
            ImageValue.Text = si.SiteLogo;
            lblSiteName.Text = si.SiteTitle;
            BindViewState();
        }
        /// <summary>
        /// 绑定模板配置第二步
        /// </summary>
        private void BindTemplate()
        {
            string msg = @"您尚未指定当前模板组，您可以：<br>" +
           "（1）在下面可选模板组中，选择一个，点击”应用“；<br>" +
           "（2）创建一个新的模板组（上面工具条中点击”创建模板组“）;<br>" +
           "（3）上传一个模板组包（工具条中点击”上传模板“）。";
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si.DefaultTemplateGroupFileName != "")
            {
                List<TemplateGroup> tg = new List<TemplateGroup>();
                try
                {
                    tg.Add(TemplateHelper.GetTemplateGroup(si.DefaultTemplateGroupFileName));
                    if (tg.Count < 1)
                    {
                        UploadHyperLink.Visible = true;
                    }
                    UseTemplateGroupsDataList.DataSource = tg;
                    UseTemplateGroupsDataList.DataBind();
                }
                catch
                {
                    UploadHyperLink.Visible = true;
                    //UseTemplateGroupsLabel.Text = msg;
                }
            }
            else
            {
                UploadHyperLink.Visible = true;
                //UseTemplateGroupsLabel.Text = msg;
            }
            TemplateGroupsDataList.DataSource = TemplateHelper.GetTemplateGroups(null);
            TemplateGroupsDataList.DataBind();
        }
        /// <summary>
        /// 得到模板组对应的效果图 2007-07-19 zn
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        protected string GetImageUrl(string FileName)
        {
            string PreviewFileName = FileName + ".jpg";
            string path = "/" + Path.Combine(Constants.TemplateGroupBasePath, PreviewFileName);
            if (!File.Exists(Server.MapPath(path)))
                path = "images/template_default.jpg";
            string phyPath = path.Replace("\\", "/");

            return phyPath;
        }

        public string GetTemplateGroupUrl(string templateGroupName, string str)
        {
            return GetTemplateGroupUrl(templateGroupName, "", str);
        }

        /// <summary>
        /// 配置按钮地址 2007-07-19 zn
        /// </summary>
        /// <param name="TemplateGroupName"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetTemplateGroupUrl(string templateGroupName, string groupName, string action)
        {
            switch (action)
            {
                case "编辑":
                    string url = "";
                    string name = templateGroupName;
                    if (!name.ToLower().EndsWith(".xml"))
                    {
                        name = name + ".xml";
                    }
                    string file = Path.Combine(TemplateHelper.TemplateGroupPath, name);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    XmlNode node = doc.SelectSingleNode("TempateGroup");
                    XmlAttribute attr = node.Attributes["ver"];
                    url = String.Format("/admin/TemplateGroupDetail.aspx?file={0}", templateGroupName);
                    if (attr != null)
                    {
                        string version = attr.Value;
                        if (!String.IsNullOrEmpty(version))
                        {
                            if (version.StartsWith("V")) version = version.Remove(0, 1);
                            if (String.CompareOrdinal(version, "2.1") >= 0)
                            {
                                url = String.Format("/admin/Template/TemplateGroupEdit.aspx?file={0}", templateGroupName);
                            }
                        }
                    }
                    return url;
                case "删除":
                    return string.Format("javascript:deleteGroup('{0}','{1}') ", groupName, templateGroupName);
                case "应用":
                    return string.Format("javascript:applyGroup('{0}','{1}') ", groupName, templateGroupName);
                case "打包":
                    return String.Format("TemplateGroupDonwload.aspx?file={0}", templateGroupName);
                default:
                    return "";
            }
        }

        void ApplyDefaultTemplate(string fileName)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            si.DefaultTemplateGroupFileName = fileName;
            GeneralConfigs.SaveConfig(si);
        }

        /// <summary>
        /// 截取长度 2007-07-19 zn
        /// </summary>
        /// <param name="content"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        protected string CheckLength(string content, int len)
        {
            if (content.Length > len)
            {
                return string.Format("{0}...", content.Substring(0, len));
            }
            else
            {
                return content;
            }
        }

        protected void deleteGroupButton_Click(object sender, EventArgs e)
        {
            Save();
            TemplateHelper.DeleteTemplateGroup(currentGroup.Text);
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            if (config.DefaultTemplateGroupFileName.ToLower() == currentGroup.Text.ToLower())
            {
                config.DefaultTemplateGroupFileName = "";
                GeneralConfigs.SaveConfig(config);
                GeneralConfigs.ResetConfig();
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void applyGroupButton_Click(object sender, EventArgs e)
        {
            Save();
            ApplyDefaultTemplate(currentGroup.Text);
            Response.Write("<script>document.location.reload();</script>");
            Response.Redirect(Request.RawUrl);
        }
        #endregion

        /// <summary>
        /// 控制显示隐藏
        /// </summary>
        private void CheckDisplay()
        {
            btnNext.Visible = true;
            btnPrevious.Visible = true;
            if (Session["VisibleIndex"] == null)
            {
                pnlSiteConfig.Visible = true;
                pnlSiteTemplate.Visible = false;
                pnlSuccess.Visible = false;
            }
            else if (Session["VisibleIndex"].ToString() == "1")
            {
                pnlSiteConfig.Visible = true;
                pnlSiteTemplate.Visible = false;
                pnlSuccess.Visible = false;
            }
            else if (Session["VisibleIndex"].ToString() == "2")
            {
                pnlSiteConfig.Visible = false;
                pnlSiteTemplate.Visible = true;
                pnlSuccess.Visible = false;
            }
            else if (Session["VisibleIndex"].ToString() == "3")
            {
                Session["VisibleIndex"] = null;
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        private void Save()
        {
            if (AppCtx.IsDemoSite)
                return;
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            si.SiteTitle = ViewState["SiteTitle"].ToString();
            si.Copyright = ViewState["Copyright"].ToString();
            si.SiteFullName = ViewState["SiteFullName"].ToString();
            si.IcpInfo = ViewState["IcpInfo"].ToString();
            si.SiteLogo = ViewState["SiteLogo"].ToString();
            lblSiteName.Text = ViewState["SiteTitle"].ToString();
            GeneralConfigs.SaveConfig(si);
            //for (int i = 0; i < TemplateGroupsDataList.Items.Count; i++)
            //{
            //    RadioButton rad_selected = (RadioButton)TemplateGroupsDataList.Items[i].FindControl("rblTemplate").Controls[0];
            //    if (rad_selected.Checked)
            //    {
            //        //DoSomething
            //    }
            //}
        }

        /// <summary>
        /// 上一步事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PreviousPanel(object sender, EventArgs e)
        {
            if (pnlSiteTemplate.Visible)
            {
                Session["VisibleIndex"] = "1";
                pnlSiteTemplate.Visible = false;
                pnlSiteConfig.Visible = true;
            }
        }
        /// <summary>
        /// 下一步事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNextPanel(object sender, EventArgs e)
        {
            if (pnlSiteConfig.Visible)
            {
                Session["VisibleIndex"] = "2";
                pnlSiteConfig.Visible = false;
                pnlSiteTemplate.Visible = true;
                pnlSuccess.Visible = false;
                BindViewState();
            }
            else if (pnlSiteTemplate.Visible)
            {
                Save();
                pnlSiteConfig.Visible = false;
                pnlSiteTemplate.Visible = false;
                pnlSuccess.Visible = true;
                btnNext.Visible = false;
                btnPrevious.Visible = false;
                Session["VisibleIndex"] = "3";
            }
        }


        protected void bttnUpload_Click(object sender, EventArgs e)
        {
            if (!ValidateUpload())
            {
                ltlMsg.Text = "<br><font color='red'>文件为空或文件格式不对</font>";
                return;
            }
            UploadFile();
        }

        string CreateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random((int)DateTime.Now.Ticks).Next();
        }

        /// <summary>
        /// 创建文件路径
        /// </summary>
        /// <param name="ext">文件扩展名</param>
        /// <returns>文件的绝地路径</returns>
        string GetFileFolder()
        {
            Article article = new Article();
            article.ID = We7Helper.CreateNewID();
            return article.AttachmentUrlPath.TrimEnd("/".ToCharArray()) + "/Thumbnail";
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        void UploadFile()
        {
            string fileName = fuImage.FileName;
            string ext = Path.GetExtension(fileName);
            string folder = GetFileFolder();
            string newFileName = CreateFileName();

            OrignPath = folder.TrimEnd('/') + "/" + newFileName + ext;
            string physicalpath = Server.MapPath(folder);
            if (!Directory.Exists(physicalpath))
            {
                Directory.CreateDirectory(physicalpath);
            }
            string physicalfilepath = Server.MapPath(OrignPath);
            fuImage.SaveAs(physicalfilepath);
            ImageValue.Text = OrignPath;
        }

        bool ValidateUpload()
        {
            if (String.IsNullOrEmpty(fuImage.FileName))
                return false;
            string ext = Path.GetExtension(fuImage.FileName).Trim('.');
            string[] list = new string[] { "jpg", "jpeg", "gif", "png", "bmp" };
            return We7.Framework.Util.Utils.InArray(ext.ToLower(), list);
        }

        private void BindViewState()
        {
            ViewState["SiteTitle"] = txtSiteName.Text;
            ViewState["Copyright"] = txtCopyright.Text;
            ViewState["SiteFullName"] = txtSiteFullName.Text;
            ViewState["IcpInfo"] = txtIcpInfo.Text;
            ViewState["SiteLogo"] = ImageValue.Text;
        }

        public bool AppContext { get; set; }
    }
}
