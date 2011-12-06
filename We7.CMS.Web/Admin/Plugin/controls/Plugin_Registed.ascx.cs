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
using We7.Framework.Config;
using System.Collections.Generic;
using System.IO;
using We7.CMS.Plugin;
using We7.Framework;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.Plugin.controls
{
	public partial class Plugin_Registed : System.Web.UI.UserControl
	{
		private SiteConfigInfo SiteInfo = SiteConfigs.GetConfig();


		private ShopService.ShopService _ShopService;
		public ShopService.ShopService ShopService
		{
			get
			{
				if (_ShopService == null)
				{
					_ShopService = new We7.CMS.Web.Admin.ShopService.ShopService();
					_ShopService.Url = GeneralConfigs.GetConfig().ShopService.TrimEnd('/').TrimEnd('\\') + "/Plugins/ShopPlugin/ShopService.asmx";
				}
				return _ShopService;
			}
		}

		protected string RegisteUrl
		{
			get
			{
				return GeneralConfigs.GetConfig().ShopService.TrimEnd('/').TrimEnd('\\') + "/register.aspx";
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ShowPanel();
			}
		}

		void ShowPanel()
		{
			string sln = SiteConfigs.GetConfig().ShopLoginName;
			if (String.IsNullOrEmpty(sln) || String.IsNullOrEmpty(sln.Trim()))
			{
				Messages.ShowMessage("本站尚未登记We7插件商店帐号！只有登记了We7插件商店帐号，才能在本地站点看到已购买的We7插件。");
				ShowRegistePanl(true);
			}
			else
			{
				try
				{
					SiteConfigInfo si = SiteConfigs.GetConfig();
					string[] states = ShopService.CheckSite(si.ShopLoginName, si.ShopPassword, si.SiteUrl);
					if (states != null && states.Length > 0 && states[0] == "1")
					{
						ShowRegistePanl(false);
					}
					else
					{
						Messages.ShowMessage("本站使用的的插件商店帐号有误！请重新登记插件商店帐号！");
						ShowRegistePanl(true);
					}
				}
				catch (Exception ex)
				{
					Messages.ShowError("应用程序错误！错误原因：" + ex.Message);
					plList.Visible = false;
					plSN.Visible = false;
				}
			}
		}
		void ShowRegistePanl(bool show)
		{
			if (show)
			{
				plList.Visible = false;
				plSN.Visible = true;
			}
			else
			{
				plList.Visible = true;
				plSN.Visible = false;
				BindData();
			}
		}

		protected void Pager_Fired(object sender, EventArgs args)
		{
			DataBind();
		}

		private int _resultsPageNumber = 1;
		/// <summary>
		/// 当前页
		/// </summary>
		protected int PageNumber
		{
			get
			{
				if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
					_resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
				return _resultsPageNumber;
			}
		}

		void BindData()
		{
			try
			{
				ShopService.ProductInfo[] infos = ShopService.LoadRegistedProducts(SiteInfo.ShopLoginName, SiteInfo.ShopPassword, SiteInfo.SiteUrl);

				Pager.PageIndex = PageNumber;

				Pager.ItemCount = infos != null ? infos.Length : 0;
				Pager.UrlFormat = We7Helper.AddParamToUrl(Request.RawUrl.Replace("{", "{{").Replace("}", "}}"), Keys.QRYSTR_PAGEINDEX, "{0}");
				Pager.PrefixText = "共 " + Pager.MaxPages + "  页 ·   第 " + Pager.PageIndex + "  页 · ";

				PluginListGridView.DataSource = new List<ShopService.ProductInfo>(infos).GetRange(Pager.Begin - 1, Pager.Count);
				PluginListGridView.DataBind();
			}
			catch (Exception ex)
			{
				Messages.ShowError("应用程序程序！");
			}
		}

		protected void bttnSave_Click(object sender, EventArgs e)
		{
			string name = txtLoginName.Text.Trim();
			string pwd = txtPassword.Text.Trim();
			if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(name.Trim()))
			{
				Messages.ShowError("会员名不能为空");
				return;
			}
			if (String.IsNullOrEmpty(pwd) || String.IsNullOrEmpty(pwd.Trim()))
			{
				Messages.ShowError("密码不能为空");
				return;
			}

			try
			{
				string[] result = ShopService.RegisteSite(name, pwd, SiteInfo.SiteUrl, SiteInfo.SiteName);
				if (result != null && result.Length >= 1)
				{
					if (result[0] == "1")
					{
						SiteInfo.ShopLoginName = name;
						SiteInfo.ShopPassword = ShopService.EncryptPassword(name, pwd);
						SiteConfigs.SaveConfig(SiteInfo);
						ShowRegistePanl(false);
					}
					else
					{
						Messages.ShowError(result.Length > 1 ? result[1] : "注册失败！");
					}
				}
				else
				{
					throw new Exception("应用程序执行结果有误!");
				}
			}
			catch (Exception ex)
			{
				Messages.ShowError("应用程序程序！错误原因：" + ex.Message);
			}
		}

		protected bool CheckInstalled(object url)
		{
			string fileName = Path.GetFileNameWithoutExtension(url as string);
			PluginHelper helper = new PluginHelper();
			return helper.isInstalled(fileName);
		}

		protected string GetStar(object point)
		{
			int intPoint = 0;
			if (point != null)
			{
				int.TryParse(point.ToString(), out intPoint);
			}
			if (intPoint > 5000)
			{
				intPoint = 5;
			}
			else if (intPoint >= 400)
			{
				intPoint = 4;
			}
			else if (intPoint >= 300)
			{
				intPoint = 3;
			}
			else if (intPoint >= 20)
			{
				intPoint = 2;
			}
			else if (intPoint >= 1)
			{
				intPoint = 1;
			}
			else
			{
				intPoint = 0;
			}
			return String.Format("<img src='/admin/images/star{0}.png' />", intPoint);
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

		/// <summary>
		/// 更新/安装按钮
		/// </summary>
		/// <param name="productCategory"></param>
		/// <param name="url"></param>
		/// <returns></returns>
		protected string GetUpdateOrInstallString(object productCategory, object url)
		{
			if (productCategory.ToString().Equals("mb"))
			{
				return string.Format("<a href='{0}'  class='Btn' target='_blank'>下载</a>", url);
			}
			else
			{
				string text = CheckInstalled(url.ToString()) ? "更新" : "安装";
				string mode = CheckInstalled(url.ToString()) ? "remoteupdate" : "remoteinstall";
				return string.Format(
					"<input type='button' class='Btn' value='{0}' onclick='submitSingleAction(\"{1}\",\"{2}\",\"{3}\");return false;' />"
					, text, mode, url, text == "更新" ? 0 : 1);
			}
		}
	}
}