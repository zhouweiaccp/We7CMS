using System;
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
using We7.CMS.Config;
using We7.Framework.Config;
using System.Text.RegularExpressions;
using We7.Framework;
using System.IO;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
	public partial class system : BasePage
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			bool fileexist = System.IO.File.Exists(Server.MapPath("/Plugins/IPStrategyPlugin/Plugin.xml"));
			if (!fileexist) ipset.Visible = false;
		}

		protected override void Initialize()
		{
			SiteConfigInfo ci = SiteConfigs.GetConfig();
			GeneralConfigInfo si = GeneralConfigs.GetConfig();

			SiteNameTextBox.Text = si.SiteTitle;
			txtSiteFullName.Text = si.SiteFullName;
			ImageValue.Text = si.SiteLogo;
			txtIcpInfo.Text = si.IcpInfo;
			RootUrlTextBox.Text = ci.RootUrl;
			IsHashedPasswordCheckBox.Checked = ci.IsPasswordHashed;
			txtSN.Text = ci.PluginSN;

			AllowSignupCheckBox.Checked = si.AllowSignup == "True";
			//DefaultTemplateGroupTextBox.Text = si.DefaultTemplateGroup;
			DefaultTemplateGroupFileNameTextBox.Text = si.DefaultTemplateGroupFileName;

			HomePageTitleTextBox.Text = si.DefaultHomePageTitle;
			ChannelPageTitleTextBox.Text = si.DefaultChannelPageTitle;
			ContentPageTitleTextBox.Text = si.DefaultContentPageTitle;


			SySMailUserTextBox.Text = si.SysMailUser;
			SysMailPassTextBox.Text = si.SysMailPassword;
			SysMailServerTextBox.Text = si.SysMailServer;
			SystemMailTextBox.Text = si.SystemMail;
			NotifyMailTextBox.Text = si.NotifyMail;
			GenericUserManageTextBox.Text = si.GenericUserManageType;
			SysPopServerTextBox.Text = si.SysPopServer;

			//IsHashedPasswordCheckBox.Checked = si.IsPasswordHashed;
			IsAddLogCheckBox.Checked = si.IsAddLog;
			IsAuditCommentCheckBox.Checked = si.IsAuditComment;

			EnableLoginAuhenCodeCheckBox.Checked = (si.EnableLoginAuhenCode == "true");

			AshxRadioButton.Checked = (si.UrlFormat == "aspx");
			HtmlRadioButton.Checked = (si.UrlFormat == "html");
			IISRadioButton.Checked = (si.UrlRewriterProvider == "iis");
			ASPNETRadioButton.Checked = (si.UrlRewriterProvider == "asp.net");

			ArticleUrlGeneratorTextBox.Text = si.ArticleUrlGenerator;
			ArticleUrlGeneratorDropDownList.SelectedValue = si.ArticleUrlGenerator;
			if (ArticleUrlGeneratorDropDownList.SelectedIndex == -1)
				ArticleUrlGeneratorDropDownList.SelectedIndex = 3;

			ArticleSourceDefaultTextBox.Text = si.ArticleSourceDefault;

			ArticleAutoPublish.Checked = (si.ArticleAutoPublish == "true");
			ArticleAutoShare.Checked = (si.ArticleAutoShare == "true");

			KeywordPageMetaTextBox.Text = si.KeywordPageMeta;
			DescriptionPageMetaTextBox.Text = si.DescriptionPageMeta;


			ADUrlTextBox.Text = ci.ADUrl;

			EnableCache.Checked = (si.EnableCache == "true");
			CacheTimeSpanTextBox.Text = si.CacheTimeSpan.ToString();
			OnlyLoginUserCanVisitCheckBox.Checked = si.OnlyLoginUserCanVisit;
			StartTemplateMapCheckbox.Checked = si.StartTemplateMap;
			SiteStateDropDownList.SelectedValue = si.SiteBuildState.ToLower();
			AllowParentArticleCheckBox.Checked = si.AllowParentArticle;
			EnableSingleTable.Checked = si.EnableSingleTable;
			UseVisualTemplateCheckBox.Checked = si.DefaultTemplateEditor == "1";
			EnableHtmlTemplate.Checked = si.EnableHtmlTemplate;

			hddnIPStrategy.Value = si.IPStrategy;
			ipstrategy.Attributes["src"] = "SystemStrategy.aspx?ipstrategy=" + si.IPStrategy;
			txtSSOUrls.Text = si.SSOSiteUrls;
			txtLinks.Text = si.Links;
			txtCopyright.Text = si.Copyright;

			//注册验证模式
			List<ListItem> listItems = new List<ListItem>();
			listItems.Add(new ListItem("不审核", "none"));
			listItems.Add(new ListItem("邮箱验证", "email"));
			listItems.Add(new ListItem("人工审核", "manual"));

			drpUserRegiseterMode.DataSource = listItems;
			drpUserRegiseterMode.SelectedValue = si.UserRegisterMode;
			drpUserRegiseterMode.DataBind();
		}
		protected void SaveButton_Click(object sender, EventArgs e)
		{
			if (AppCtx.IsDemoSite)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "aler", "alert('演示站点，不能修改选项！');", true);
				return;
			}

			SiteConfigInfo ci = SiteConfigs.GetConfig();
			GeneralConfigInfo si = GeneralConfigs.GetConfig();

			ci.SiteName = SiteNameTextBox.Text;
			si.SiteTitle = SiteNameTextBox.Text;
			ci.RootUrl = RootUrlTextBox.Text;
			ci.IsPasswordHashed = IsHashedPasswordCheckBox.Checked;
			ci.ADUrl = ADUrlTextBox.Text.Trim();
			ci.PluginSN = txtSN.Text.Trim();
			si.SiteFullName = txtSiteFullName.Text;
			si.SiteLogo = ImageValue.Text;
			si.IcpInfo = txtIcpInfo.Text;

			si.AllowSignup = AllowSignupCheckBox.Checked ? "True" : "False";
			//si.DefaultTemplateGroup = DefaultTemplateGroupTextBox.Text;
			si.DefaultTemplateGroupFileName = DefaultTemplateGroupFileNameTextBox.Text;
			si.DefaultHomePageTitle = HomePageTitleTextBox.Text;
			si.DefaultChannelPageTitle = ChannelPageTitleTextBox.Text;
			si.DefaultContentPageTitle = ContentPageTitleTextBox.Text;

			si.DescriptionPageMeta = DescriptionPageMetaTextBox.Text;
			si.KeywordPageMeta = KeywordPageMetaTextBox.Text;

			si.NotifyMail = NotifyMailTextBox.Text;

			si.SysMailUser = SySMailUserTextBox.Text;
			si.SysMailPassword = SysMailPassTextBox.Text;
			si.SysMailServer = SysMailServerTextBox.Text;
			si.SystemMail = SystemMailTextBox.Text;
			si.GenericUserManageType = GenericUserManageTextBox.Text;
			si.SysPopServer = SysPopServerTextBox.Text;

			si.IsAddLog = IsAddLogCheckBox.Checked;
			si.IsAuditComment = IsAuditCommentCheckBox.Checked;

			si.UrlFormat = (AshxRadioButton.Checked) ? "aspx" : "html";
			si.UrlRewriterProvider = (IISRadioButton.Checked) ? "iis" : "asp.net";
			if (ArticleUrlGeneratorDropDownList.SelectedIndex == 3)
				si.ArticleUrlGenerator = ArticleUrlGeneratorTextBox.Text;
			else
				si.ArticleUrlGenerator = ArticleUrlGeneratorDropDownList.SelectedValue;

			si.ArticleAutoShare = (ArticleAutoShare.Checked) ? "true" : "false";
			si.EnableLoginAuhenCode = (EnableLoginAuhenCodeCheckBox.Checked) ? "true" : "false";

			si.ArticleAutoPublish = (ArticleAutoPublish.Checked) ? "true" : "false";

			si.ArticleSourceDefault = ArticleSourceDefaultTextBox.Text;
			si.EnableCache = (EnableCache.Checked) ? "true" : "false";
			if (EnableCache.Checked)
			{
				string tmpStr = CacheTimeSpanTextBox.Text;
				Int16 tmpInt = 60;
				if (Int16.TryParse(tmpStr, out tmpInt))
				{
					si.CacheTimeSpan = tmpInt;
				}
			}

			si.OnlyLoginUserCanVisit = OnlyLoginUserCanVisitCheckBox.Checked;
			si.StartTemplateMap = StartTemplateMapCheckbox.Checked;
			si.SiteBuildState = SiteStateDropDownList.SelectedValue;
			si.IPStrategy = hddnIPStrategy.Value.Trim();
			si.AllowParentArticle = AllowParentArticleCheckBox.Checked;
			si.EnableSingleTable = EnableSingleTable.Checked;
			si.DefaultTemplateEditor = UseVisualTemplateCheckBox.Checked ? "1" : "0";
			si.EnableHtmlTemplate = EnableHtmlTemplate.Checked;
			txtSSOUrls.Text = si.SSOSiteUrls = Regex.Replace(txtSSOUrls.Text, @"\s", "");
			si.Links = txtLinks.Text.Trim();
			si.Copyright = txtCopyright.Text.Trim();

			//用户注册验证模式
			si.UserRegisterMode = drpUserRegiseterMode.SelectedValue;

			ipstrategy.Attributes["src"] = "SystemStrategy.aspx?ipstrategy=" + si.IPStrategy;

			SiteConfigs.SaveConfig(ci);
			GeneralConfigs.SaveConfig(si);

			//记录日志
			string content = string.Format("更改站点信息。");
			AddLog("站点概要", content);

			Messages.ShowMessage("系统设置保存成功！");
		}

		protected void bttnUpload_Click(object sender, EventArgs e)
		{
			if (!ValidateUpload())
			{
				Messages.ShowError("<br><font color='red'>文件为空或文件格式不对</font>");
				return;
			}
			UploadFile();
		}

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
	}
}