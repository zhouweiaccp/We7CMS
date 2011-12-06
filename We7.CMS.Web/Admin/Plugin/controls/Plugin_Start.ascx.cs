using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using We7.CMS.Plugin;
using System.Threading;
using We7.CMS.Web.Admin.Modules.Plugin;
using We7.CMS;
using We7.CMS.Common;
using System.Xml;

namespace We7.CMS.Web.Admin.Modules
{
	public partial class Plugin_Start : System.Web.UI.UserControl
	{
		private PluginHelper helper;
		protected PluginMessage message;
		protected PluginInfo ginfo;
		public string PluginName;
		protected void Page_Load(object sender, EventArgs e)
		{
			ginfo = new PluginInfo(PluginType);
			helper = new PluginHelper(PluginType);
			init();
		}

		public PluginType PluginType
		{
			get
			{
				if (ViewState["WE7$PluginType"] == null)
				{
					ViewState["WE7$PluginType"] = PluginType.PLUGIN;
				}
				return (PluginType)ViewState["WE7$PluginType"];
			}
			set
			{
				ViewState["WE7$PluginType"] = value;
			}
		}

		private void init()
		{
			message = new PluginMessage(PluginType);
			InstallButton.Visible = false;
			BackButton.Visible = false;
			Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "showProgress", "mask.showProgressBar();");
			UploadButton.Text = "添加" + message.PluginLabel;
		}

		private void displayInstallButton()
		{
			if (PluginType == PluginType.PLUGIN)
			{
				InstallButton.Visible = true;
				BackButton.Visible = true;
			}
		}

		protected void UploadButton_Click(object sender, EventArgs e)
		{
			try
			{
				string pluginPath = ginfo.PluginsClientPath;
				if (!Directory.Exists(pluginPath))
					Directory.CreateDirectory(pluginPath);
				helper.ExtractZipFile(ZipFileUpload.FileBytes, pluginPath);

				messages.ShowMessage(message.UploadSuccess);
				displayInstallButton();

				try
				{
					PluginName = Path.GetFileNameWithoutExtension(ZipFileUpload.FileName);
					PluginInfo info = PluginInfoCollection.CreateInstance(PluginType)[PluginName];
					info.IsLocal = true;
					//info.IsInstalled = true;
					info.Save();
				}
				catch (Exception ex)
				{
					We7.Framework.LogHelper.WriteLog(typeof(Plugin_Start), ex);
				}
			}
			catch (Exception ex)
			{
				messages.ShowError(message.UploadError);
			}
		}

		protected void QueryButton_Click(object sender, EventArgs e)
		{
			if (Page is PluginAdd)
			{
				PluginAdd page = ((PluginAdd)Page);

				page.IsQuery = true;
				page.queryText = QueryTextBox.Text.Trim();
				page.queryType = QueryDropDownList.SelectedValue;

				Response.Redirect(String.Format("PluginAdd.aspx?tab=0&qtext={0}&qtype={1}&ptype={2}", HttpUtility.HtmlEncode(QueryTextBox.Text.Trim()), HttpUtility.HtmlEncode(QueryDropDownList.SelectedValue), PluginType));

			}
		}
		/// <summary>
		/// 判断插件是否生成菜单
		/// </summary>
		/// <param name="pinfo"></param>
		/// <returns></returns>
		public int HasMenu
		{
			get
			{
				if (string.IsNullOrEmpty(PluginName)) return 0;
				PluginInfo pinfo = new PluginInfo(Server.MapPath("/Plugins/" + PluginName + "/Plugin.xml"));
				return pinfo.HasMenu > 3 ? 3 : pinfo.HasMenu;
			}
		}
		/// <summary>
		/// 清除菜单数据
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnClearMenuClick(object sender, EventArgs e)
		{
			Response.Redirect(this.Request.RawUrl +
					(this.Request.RawUrl.Contains("?") ? "&" : "?") + "reload=menu");
		}
	}
}