using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Accounts;
using System.IO;
using System.Xml;
using We7.Framework.Util;
using We7.Framework;
using We7.CMS.Common;
using We7.CMS.Plugin;
using System.Web.UI.HtmlControls;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin.Plugin
{
	public partial class PluginMenu : Page
	{

		/// <summary>
		/// 插件信息
		/// </summary>
		private PluginInfo curInfo = new PluginInfo();

		/// <summary>
		/// 菜单ID
		/// </summary>
		string curID = string.Empty;
		/// <summary>
		/// 是否已有用户中心菜单
		/// </summary>
		public string HasUserMenu
		{
			get
			{
				foreach (UrlItem ui in curInfo.Pages)
					if (ui.EntityID == "System.User") return "true";
				return "false";
			}
		}

		string pluginname = string.Empty;
		/// <summary>
		/// 插件名称
		/// </summary>
		public string PluginName { get { return pluginname; } }

		/// <summary>
		/// 页面初始化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			pluginname = Request["PluginName"];
			if (pluginname.StartsWith("http://") && pluginname.EndsWith(".zip"))
				pluginname = pluginname.Substring(pluginname.LastIndexOf('/'), pluginname.LastIndexOf(".zip") - pluginname.LastIndexOf('/'));
			if (!File.Exists(Server.MapPath(string.Format("/Plugins/{0}/Plugin.xml", pluginname))))
			{
				Messages.ShowMessage("无菜单项！");
				formtable.Visible = false;
				Refresh();
				return;
			}
			curInfo.LoadXml(Server.MapPath(string.Format("/Plugins/{0}/Plugin.xml", pluginname)));

			if (Request["action"] == "uninstall" || Request["action"] == "delete")
			{
				formtable.Visible = false;
				for (int i = 0; i < curInfo.Pages.Count; i++)
				{
					Messages.ShowMessage("正在删除菜单……");
					MenuHelper.DeleteMenuItem(curInfo.Pages[i].ID);
				}
				Refresh();
				return;
			}

			if (!IsPostBack)
			{
				ClearOldMenus();
				if (curInfo.Pages.Count > 0)
				{
					string parentid = string.Empty;
					foreach (UrlItem ui in curInfo.Pages)
					{
						We7.CMS.Common.MenuItem mi = MenuHelper.GetMenuItem(ui.ID);
						if (mi == null)
						{
							mi = new Common.MenuItem();
							mi.ID = We7Helper.CreateNewID();
							mi.Name = ui.Name;
							mi.Title = ui.Title;
							mi.Href = ui.Url;
							if (string.IsNullOrEmpty(parentid)) parentid = ui.Main ? mi.ID : string.Empty;
							mi.ParentID = ui.Main ? We7Helper.EmptyGUID : parentid;
							mi.EntityID = string.IsNullOrEmpty(ui.EntityID) ? "System.Administration" : ui.EntityID;
						}
						pluginMenus.Add(mi);
					}
				}
				else FromInstall();
				if (PluginMenus.Count <= 0)
				{
					Messages.ShowMessage("无菜单项！");
					formtable.Visible = false;
					Refresh();
					return;
				}
				BindChildIndex();
				OutputInfo();
			}
		}
		string parentid = string.Empty;
		/// <summary>
		/// 更新插件并管理菜单时删除旧有菜单
		/// </summary>
		private void ClearOldMenus()
		{
			string oldXml = Server.MapPath(string.Format("/Plugins/{0}/PluginOld.xml", pluginname));
			if (File.Exists(oldXml) && new FileInfo(oldXml).LastWriteTime < new FileInfo(Server.MapPath(string.Format("/Plugins/{0}/Plugin.xml", pluginname))).LastWriteTime)
			{
				PluginInfo oldinfo = new PluginInfo(oldXml);
				for (int i = 0; i < oldinfo.Pages.Count; i++)
				{
					MenuHelper.DeleteMenuItem(oldinfo.Pages[i].ID);
				}
			}
		}

		/// <summary>
		/// 将菜单的信息输出到页面上隐藏位置
		/// </summary>
		private void OutputInfo()
		{
			foreach (We7.CMS.Common.MenuItem mi in PluginMenus)
			{
				TableRow tr = new TableRow();
				TableCell td1 = new TableCell();
				td1.Text = mi.Name;
				TableCell td2 = new TableCell();
				td2.Text = mi.Title;
				TableCell td3 = new TableCell();
				td3.Text = mi.Href;
				TableCell td4 = new TableCell();
				if (!string.IsNullOrEmpty(mi.ParentID))
					td4.Text = string.Format("{0},{1}", mi.ParentID, mi.Index);
				else if (!string.IsNullOrEmpty(parentid))
					td4.Text = string.Format("{0},{1}", parentid, mi.Index);
				TableCell td5 = new TableCell();
				td5.Text = mi.ID; ;
				TableCell td6 = new TableCell();
				td6.Text = mi.EntityID;
				tr.Cells.Add(td1);
				tr.Cells.Add(td2);
				tr.Cells.Add(td3);
				tr.Cells.Add(td4);
				tr.Cells.Add(td5);
				tr.Cells.Add(td6);
				allmenus.Rows.Add(tr);
			}
		}

		/// <summary>
		/// 从install.xml文件的Insert语句中读取菜单相关信息
		/// </summary>
		private bool FromInstall()
		{
			XmlDocument installdoc = new XmlDocument();
			foreach (string install in curInfo.Deployment.Install)
			{
				string installpath = Server.MapPath(string.Format("/Plugins/{0}/Data/{1}", pluginname, install));
				if (File.Exists(installpath))
				{
					installdoc.Load(installpath);
					foreach (XmlNode node in installdoc.SelectNodes("//Sql"))
					{
						string insert = node.InnerText.Trim().ToLower();
						if (insert.Contains("insert") && insert.Contains("menu"))
						{
							We7.CMS.Common.MenuItem cur = new We7.CMS.Common.MenuItem();
							int firstb = insert.IndexOf('(');
							int firste = insert.IndexOf(')');
							int secondb = insert.Remove(0, firste + 1).IndexOf('(');
							int seconde = insert.Remove(0, firste + 1).IndexOf(')');
							string[] keys = insert.Substring(firstb + 1, firste - firstb - 1).Split(',');
							string[] values = insert.Remove(0, firste + 1).Substring(secondb + 1, seconde - secondb - 1).Split(',');
							for (int i = 0; i < keys.Length; i++)
							{
								if (keys[i].Contains("nametext")) cur.Name = values[i].TrimStart('\'').TrimEnd('\'');
								else if (keys[i].Contains("href")) cur.Href = values[i].TrimStart('\'').TrimEnd('\'');
								else if (keys[i].Contains("title")) cur.Title = values[i].TrimStart('\'').TrimEnd('\'');
								else if (keys[i].Contains("parentid") && values[i].TrimStart('\'').TrimEnd('\'') == We7Helper.EmptyGUID)
								{
									cur.ParentID = We7Helper.EmptyGUID;
									cur.ID = We7Helper.CreateNewID();
									parentid = cur.ID;
								}
								else if (keys[i].Contains("entityid"))
								{
									cur.EntityID = values[i].TrimStart('\'').TrimEnd('\'');
									if (cur.EntityID == "system.administration") cur.EntityID = "System.Administration";
									else if (cur.EntityID == "system.user") cur.EntityID = "System.User";
								}
							}
							if (string.IsNullOrEmpty(cur.EntityID)) cur.EntityID = "System.Administration";
							pluginMenus.Add(cur);
						}
					}
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 菜单业务对象
		/// </summary>
		protected MenuHelper MenuHelper
		{
			get { return HelperFactory.Instance.GetHelper<MenuHelper>(); }
		}

		/// <summary>
		/// 绑定菜单下拉框
		/// </summary>
		void BindChildIndex()
		{
			List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID, 2, "System.Administration");

			string myname = "├插件";
			int selectIndex = 0, j = 0;

			foreach (We7.CMS.Common.MenuItem menuItem in menus)
			{
				string name = "├" + menuItem.Title.ToString();
				string value = menuItem.ID + ",0";
				ListItem item = new ListItem(name, value);
				SecondIndexDropDownList.Items.Add(item);
				int i = 0;
				foreach (We7.CMS.Common.MenuItem submenu in menuItem.Items)
				{
					string sname = "├──" + submenu.Title.ToString();
					string svalue = submenu.ParentID + "," + submenu.Index.ToString();
					ListItem sitem = new ListItem(sname, svalue);
					i = submenu.Index;
					SecondIndexDropDownList.Items.Add(sitem);
					j++;
				}
				ListItem appendItem = new ListItem("├──（追加到这里）", menuItem.ID + "," + (i + 2).ToString());
				SecondIndexDropDownList.Items.Add(appendItem);
				j++;

				if (name == myname)
				{
					selectIndex = j;
				}

				j++;
			}
			SecondIndexDropDownList.SelectedIndex = selectIndex;

			menus = MenuHelper.GetMenus("{00000000-0001-0000-0000-000000000000}", 2, "System.User");
			foreach (We7.CMS.Common.MenuItem menuItem in menus)
			{
				string name = "├" + menuItem.Title.ToString();
				string value = menuItem.ID + ",0";
				ListItem item = new ListItem(name, value);
				UserMenusDropDownList.Items.Add(item);
				int i = 0;
				foreach (We7.CMS.Common.MenuItem submenu in menuItem.Items)
				{
					string sname = "├──" + submenu.Title.ToString();
					string svalue = submenu.ParentID + "," + submenu.Index.ToString();
					ListItem sitem = new ListItem(sname, svalue);
					i = submenu.Index;
					UserMenusDropDownList.Items.Add(sitem);
					j++;
				}
				ListItem appendItem = new ListItem("├──（追加到这里）", menuItem.ID + "," + (i + 2).ToString());
				UserMenusDropDownList.Items.Add(appendItem);
				j++;
				j++;
			}
		}

		/// <summary>
		/// 生成菜单
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void SubmitButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (!UserCheckBox.Checked)
					foreach (UrlItem ui in curInfo.Pages)
						if (ui.EntityID == "System.User")
							MenuHelper.DeleteMenuItem(ui.ID);
				curInfo.Pages.Clear();
				string[] results = resultValue.Value.Split('|');

				foreach (string result in results)
				{
					string[] fileds = result.Split('$');
					if (fileds.Length < 5) continue;
					string parentID = fileds[4].Split(',')[0];
					int maingroup = Int32.Parse(fileds[4].Split(',')[1]);
					int firstIndex = 0;
					if (maingroup > 0)
					{
						firstIndex = maingroup - 1;
					}
					string firstNameText = fileds[1];
					string firstTitle = fileds[2];
					string firstUrl = fileds[3];
					if (fileds[5].ToLower() == "system.administration")
						if (parentID == We7Helper.EmptyGUID)
						{
							curID = MenuHelper.CreateMainMenu(firstNameText, firstTitle, 1, firstIndex, null, firstUrl, fileds[0], fileds[5]);
							curInfo.Pages.Add(new UrlItem(curID, firstNameText, firstTitle, firstUrl, true, fileds[5]));
						}
						else
						{
							curID = MenuHelper.CreateSubMenu(firstNameText, firstTitle, firstUrl, firstIndex, parentID, fileds[0], fileds[5]);
							curInfo.Pages.Add(new UrlItem(curID, firstNameText, firstTitle, firstUrl, false, fileds[5]));
						}
					else if (fileds[5].ToLower() == "system.user")
					{
						if (parentID == "{00000000-0001-0000-0000-000000000000}")
						{
							curID = MenuHelper.CreateSubMenu_User(firstNameText, firstTitle, firstUrl, firstIndex, parentID, fileds[0], fileds[5], (int)TypeOfMenu.GroupMenu, "");
							curInfo.Pages.Add(new UrlItem(curID, firstNameText, firstTitle, firstUrl, true, fileds[5]));
						}
						else
						{
							curID = MenuHelper.CreateSubMenu_User(firstNameText, firstTitle, firstUrl, firstIndex, parentID, fileds[0], fileds[5], (int)TypeOfMenu.NormalMenu, "");
							curInfo.Pages.Add(new UrlItem(curID, firstNameText, firstTitle, firstUrl, false, fileds[5]));
						}
					}
				}

				curInfo.Save();
				File.Copy(Server.MapPath(string.Format("/Plugins/{0}/Plugin.xml", pluginname)), Server.MapPath(string.Format("/Plugins/{0}/PluginOld.xml", pluginname)), true);
				Refresh();
			}
			catch (Exception ex)
			{
				Messages.ShowError("无法保存：" + ex.Message);
			}
		}

		/// <summary>
		/// 刷新父页面
		/// </summary>
		private void Refresh()
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "refresh", "<script>$(\"#formtable\").hide();window.parent.document.getElementById(window.parent.refreshbutton).click();</script>");
		}

		private List<We7.CMS.Common.MenuItem> pluginMenus = new List<We7.CMS.Common.MenuItem>();
		/// <summary>
		/// 所有要添加的菜单
		/// </summary>
		public List<We7.CMS.Common.MenuItem> PluginMenus
		{
			get
			{
				List<We7.CMS.Common.MenuItem> temp = pluginMenus;
				bool hasuser = false;
				foreach (We7.CMS.Common.MenuItem tem in temp)
				{
					if (tem.EntityID == "System.User")
					{
						hasuser = true;
						break;
					}
				}
				if (!hasuser)
				{
					int tempCount = temp.Count;
					for (int i = 0; i < tempCount; i++)
					{
						We7.CMS.Common.MenuItem te = new Common.MenuItem();
						te.Name = temp[i].Name;
						te.Title = temp[i].Title;
						te.Href = temp[i].Href;
						te.ID = We7Helper.CreateNewID();
						te.EntityID = "System.User";
						if (temp[i].ParentID == We7Helper.EmptyGUID)
							te.ParentID = "{00000000-0001-0000-0000-000000000000}";
						pluginMenus.Add(te);
					}
				}
				return pluginMenus;
			}
		}
	}


}