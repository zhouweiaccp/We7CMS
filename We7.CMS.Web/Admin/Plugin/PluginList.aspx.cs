using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Config;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS;
using We7.CMS.Plugin;
using We7.CMS.Common;
using System.Xml;
using System.IO;

namespace We7.CMS.Web.Admin.Modules.Plugin
{
	public partial class PluginList : BasePage
	{
		/// <summary>
		/// 获取主题风格路径
		/// </summary>
		public string ThemePath
		{
			get
			{
				string theme = GeneralConfigs.GetConfig().CMSTheme;
				if (theme == null || theme == "") theme = "classic";
				return "/admin/" + Constants.ThemePath + "/" + theme;
			}
		}

		public PluginMessage Message;
		protected void Page_Load(object sender, EventArgs e)
		{
			Message = new PluginMessage(PluginType);
			if (!IsPostBack)
			{

			}
			InitPage();

			refresh.Click += new EventHandler(refresh_Click);
		}

		/// <summary>
		/// 页面初始化
		/// </summary>
		private void InitPage()
		{
			DisplayControl();
			switch (PluginType)
			{
				case PluginType.PLUGIN:
					BindData();
					break;
				case PluginType.RESOURCE:
					BindControlData();
					break;
				default:
					BindData();
					break;
			}
		}

		private void refresh_Click(object sender, EventArgs e)
		{
			BindData();
		}

		protected void EnablePager_Fired(object sender, EventArgs e)
		{
			BindData();
		}

		protected void DisablePager_Fired(object sender, EventArgs e)
		{
			BindData();
		}

		#region Properties

		List<PluginInfo> EnableDataSource
		{
			get
			{
				return PluginInfoCollection.CreateInstance(PluginType).FindAll(delegate(PluginInfo info)
				{
					return info.IsInstalled;
				});
			}
		}

		private List<PluginInfo> DisableDataSource
		{
			get
			{
				return PluginInfoCollection.CreateInstance(PluginType).FindAll(delegate(PluginInfo info)
				{
					return !info.IsInstalled;
				});
			}
		}

		#endregion

		#region Methods

		private void BindData()
		{
			PluginInfoCollection.CreateInstance(PluginType).Load();

			DisablePager.RecorderCount = DisableDataSource.Count;

			if (DisablePager.PageIndex < 0)
				DisablePager.PageIndex = 0;
			DisablePager.FreshMyself();

			EnablePager.RecorderCount = EnableDataSource.Count;

			if (EnablePager.PageIndex < 0)
				EnablePager.PageIndex = 0;
			EnablePager.FreshMyself();

			EnableGridView.DataSource = EnableDataSource.GetRange(EnablePager.Begin, EnablePager.Count);
			EnableGridView.DataBind();

			DisableGridView.DataSource = DisableDataSource.GetRange(DisablePager.Begin, DisablePager.Count);
			DisableGridView.DataBind();
		}

		private void BindControlData()
		{
			PluginInfoCollection.CreateInstance(PluginType).Load();

			EnablePager.RecorderCount = EnableDataSource.Count;

			if (EnablePager.PageIndex < 0)
				EnablePager.PageIndex = 0;
			EnablePager.FreshMyself();

			EnableGridView.DataSource = EnableDataSource.GetRange(EnablePager.Begin, EnablePager.Count);
			EnableGridView.DataBind();
		}

		private void DisplayControl()
		{
			UnstallLinkButton.Visible = false;
			ctrDel.Visible = false;
			DownLoadLinkButton.Visible = false;

			bottom.Visible = false;


			switch (PluginType)
			{
				case PluginType.PLUGIN:
					UnstallLinkButton.Visible = true;
					bottom.Visible = true;
					break;
				case PluginType.RESOURCE:
					ctrDel.Visible = true;
					DownLoadLinkButton.Visible = true;
					break;
				default:
					break;
			}
		}

		private void DownloadPlugin(string pluginlist)
		{
			string[] list = pluginlist.Split(",".ToCharArray());
			byte[] buffer = new PluginHelper(PluginType).DownLoad(list);

			Response.Clear();
			Response.ClearHeaders();
			Response.Buffer = false;
			Response.ContentType = "application/octet-stream";
			string s = HttpUtility.UrlEncode(System.Text.UTF8Encoding.UTF8.GetBytes("DownLoad" + DateTime.Now.Ticks + ".zip"));
			Response.AppendHeader("Content-Disposition", "attachment;filename=" + s);

			Response.AppendHeader("Content-Length", buffer.Length.ToString());

			Response.BinaryWrite(buffer);
			Response.Flush();
			Response.End();
		}

		#endregion

		protected void DisableLinkButton_Click(object sender, EventArgs e)
		{
			string keys = Request.Form["EnableSeletedCheckbox"];
			if (!String.IsNullOrEmpty(keys))
			{
				string[] keylist = keys.Split(",".ToCharArray());
				foreach (string key in keylist)
				{
					PluginInfo info = PluginInfoCollection.CreateInstance(PluginType)[key];
					if (info != null)
					{
						info.Enable = false;
						info.Save();
					}
				}
			}

			BindData();

		}

		protected void EnableLinkButton_Click(object sender, EventArgs e)
		{
			string keys = Request.Form["EnableSeletedCheckbox"];
			if (!String.IsNullOrEmpty(keys))
			{
				string[] keylist = keys.Split(",".ToCharArray());
				foreach (string key in keylist)
				{
					PluginInfo info = PluginInfoCollection.CreateInstance(PluginType)[key];
					if (info != null)
					{
						info.Enable = true;
						info.Save();
					}
				}
			}

			BindData();
		}

		protected void EnableGridView_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			string key = e.CommandArgument as string;
			if (e.CommandName == "statu")
			{
				if (!String.IsNullOrEmpty(key))
				{
					PluginInfo info = PluginInfoCollection.CreateInstance(PluginType)[key];
					if (info != null)
					{
						info.Enable = !info.Enable;
						info.Save();
					}
				}
				BindData();
			}
			else if (e.CommandName == "downloadzip")
			{
				if (!String.IsNullOrEmpty(key))
				{
					DownloadPlugin(key);
				}
			}
		}

		protected PluginType PluginType
		{
			get
			{
				if (ViewState["WE7$PluginType"] == null)
				{
					string pltype = Request["pltype"];
					if (String.IsNullOrEmpty(pltype))
					{
						ViewState["WE7$PluginType"] = PluginType.PLUGIN;
					}
					else
					{
						switch (pltype.ToLower().Trim())
						{
							case "constrol":
								ViewState["WE7$PluginType"] = PluginType.RESOURCE;
								break;
							case "plugin":
								ViewState["WE7$PluginType"] = PluginType.PLUGIN;
								break;
							default:
								ViewState["WE7$PluginType"] = PluginType.PLUGIN;
								break;
						}
					}
				}
				return (PluginType)ViewState["WE7$PluginType"];
			}
		}

		protected void DownLoadLinkButton_Click(object sender, EventArgs e)
		{
			string zipfiles = Request["EnableSeletedCheckbox"];
			if (!String.IsNullOrEmpty(zipfiles))
			{
				DownloadPlugin(zipfiles);
			}
		}

		/// <summary>
		/// 清除菜单数据
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnClearMenuClick(object sender, EventArgs e)
		{
			Response.Redirect(We7Helper.AddParamToUrl(this.Request.RawUrl, "reload", "menu"));
		}
		/// <summary>
		/// 获得插件默认图片
		/// </summary>
		/// <param name="directory">插件文件夹</param>
		/// <returns></returns>
		protected string GetPluginImg(string directory)
		{
			string path = string.Format("/Plugins/{0}/logo.jpg", directory);
			if (System.IO.File.Exists(Server.MapPath(path)))
				return path;
			else
				return "/admin/images/icons_plugins.gif";
		}

		/// <summary>
		/// 已激活列表操作列设置
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void EnableGridView_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				PluginInfo pinfo = e.Row.DataItem as PluginInfo;
				Literal ops = (Literal)(e.Row.Cells[5].FindControl("MenuManage"));
				int hasMenu = pinfo.HasMenu;
				if (hasMenu > 3) hasMenu = 3;
				string action = string.Empty;
				string update = string.Empty;
				string showtext = string.Empty;
				switch (pinfo.PluginType)
				{
					case Common.PluginType.PLUGIN:
						action = "uninstall";
						update = pinfo.IsLocal ? "updatelocal" : "remoteupdate";
						showtext = "卸载";
						break;
					default:
						action = "delete";
						update = pinfo.IsLocal ? "updatelocal" : "insctr";
						showtext = "删除";
						break;
				}
				ops.Text = string.Format("<a href=\"#\" onclick=\"return submitSingleAction('{0}','{1}','{2}');\">{3}</a>", action, pinfo.Directory, hasMenu.ToString(), showtext);
				ops.Text += string.Format(" | <a href=\"{0}\" {1}>更新</a>", pinfo.IsLocal ? "#" : "/admin/Plugin/PluginAdd.aspx?tab=7", pinfo.IsLocal ? string.Format("onclick=\"return submitSingleAction('{0}','{1}',{2});\"", update, pinfo.Directory, hasMenu) : string.Empty);
				if (hasMenu > 0) ops.Text += " | <a href=\"javascript:new MaskWin().showFrame('/Admin/Plugin/PluginMenu.aspx?PluginName=" + pinfo.Directory + "&action=', '更新菜单', { width: 500, height: " + (225 + 120 * (hasMenu - 1)) + "});void(0);\">菜单</a>";
			}
		}

		/// <summary>
		/// 待激活列表操作列设置
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void DisableGridView_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				PluginInfo pinfo = e.Row.DataItem as PluginInfo;
				int hasMenu = pinfo.HasMenu;
				if (hasMenu > 3) hasMenu = 3;
				e.Row.Cells[5].Text = string.Format("<a href=\"#\" onclick='return submitSingleAction(\"install\",\"{0}\",\"{1}\")'>激活</a> | <a href=\"#\" onclick='return submitSingleAction(\"delete\",\"{0}\",\"{1}\")'>删除</a>",
					pinfo.Directory, hasMenu.ToString());
			}
		}
	}
}
