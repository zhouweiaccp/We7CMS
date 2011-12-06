using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.Framework;

namespace We7.CMS.Web.Admin.manage
{
	public partial class UserMenulistNew : BasePage
	{
		/// <summary>
		/// 当前过滤条件
		/// </summary>
		protected int CurrentState
		{
			get
			{
				int s = 100;
				if (Request["state"] != null)
				{
					if (We7Helper.IsNumber(Request["state"].ToString()))
						s = int.Parse(Request["state"].ToString());
				}
				return s;
			}
		}

		string Keyword
		{
			get
			{
				return Request["keyword"];
			}
		}

        string EntityID
        {
            get
            {
                    NameLabel.Text = "会员菜单设置";
                    SummaryLabel.Text = "会员中心菜单设置";
                    AddNewHyperLink.NavigateUrl = "AddNewMenuUser.aspx?type=1";
                    AddFeedbackMenuHyperLink.NavigateUrl = "AddFeedbackMenu.aspx?type=1";
                    AddMenuHyperLink.NavigateUrl = "AddMenuUserNew.aspx?type=1";
                    return "System.User";
            }
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request["add"] != null)
				Messages.ShowMessage(string.Format("您成功创建了菜单项 {0}，请展开左边菜单查看。", Request["add"].ToString()));
			else if (Request["del"] != null)
				Messages.ShowMessage(string.Format("您成功删除了菜单项 {0}。", Request["del"].ToString()));
			else if (Request["hide"] != null)
				Messages.ShowMessage(string.Format("您成功隐藏了菜单项 {0}。", Request["hide"].ToString()));
			else if (Request["show"] != null)
				Messages.ShowMessage(string.Format("您成功显示了菜单项 {0}。", Request["show"].ToString()));

			BindingMenu();
		}

		void BindingMenu()
		{
			List<We7.CMS.Common.MenuItem> allMenuItem = MenuHelper.GetMenus(We7Helper.EmptyGUID, 2, EntityID, true);
			List<MenuItemEx> allMenuItemEx = new List<MenuItemEx>();
			if (allMenuItem != null)
			{
				foreach (We7.CMS.Common.MenuItem menuItem in allMenuItem)
				{

					MenuItemEx mainEx = ConvertToEx(menuItem, Keyword, CurrentState);
					if (mainEx != null) allMenuItemEx.Add(mainEx);
					foreach (We7.CMS.Common.MenuItem submenu in menuItem.Items)
					{
						MenuItemEx subEx = ConvertToEx(submenu, Keyword, CurrentState);
						if (subEx != null)
						{
							subEx.Title = "├──" + subEx.Name.ToString();
							allMenuItemEx.Add(subEx);
						}
						foreach (We7.CMS.Common.MenuItem thirdmenu in submenu.Items)
						{
							MenuItemEx thirdEx = ConvertToEx(thirdmenu, Keyword, CurrentState);
							if (thirdEx != null)
							{
								thirdEx.Title = "├──├─" + thirdEx.Name.ToString();
								allMenuItemEx.Add(thirdEx);
							}
						}
					}
				}
			}

			Pager.RecorderCount = 0;
			if (allMenuItemEx != null)
				Pager.RecorderCount = allMenuItemEx.Count;

			if (Pager.Count < 0)
				Pager.PageIndex = 0;

			Pager.FreshMyself();

			if (Pager.RecorderCount > Pager.Begin + Pager.Count)
			{
				allMenuItemEx = allMenuItemEx.GetRange(Pager.Begin, Pager.Count);
			}
			else
			{
				allMenuItemEx = allMenuItemEx.GetRange(Pager.Begin, Pager.RecorderCount - Pager.Begin);
			}

			DataGridView.DataSource = allMenuItemEx;
			DataGridView.DataBind();
			StateLiteral.Text = BuildStateLinks();

		}

		MenuItemEx ConvertToEx(We7.CMS.Common.MenuItem menuItem, string keyword, int state)
		{
			if (state < 100)
			{
				if (state != menuItem.Type)
					return null;
				else if (!string.IsNullOrEmpty(keyword) && menuItem.Name.IndexOf(keyword) == -1)
					return null;
			}

			MenuItemEx menuItemEx = new MenuItemEx();
			menuItemEx.Import(menuItem);
            string suffix ="";
            switch(menuItem.MenuType)
            {
                case 0:
                    suffix = "(普通)";
                    break;
                case 1:
                    suffix = "(顶级)";
                    break;
                case 2:
                    suffix = "(分组)";
                    break;
                case 3:
                    suffix = "(引用)";
                    break;
                default:
                    suffix = "(普通)";
                    break;
            }
			if (menuItem.Type == 99)
			{
                menuItemEx.MenuType = "用户自定义菜单" + suffix;
				menuItemEx.MenuDelVisble = "";
				menuItemEx.MenuSystemVisble = "none";
				menuItemEx.MenuSystemShowVisble = "none";
				menuItemEx.MenuDelUrl = String.Format("javascript:DeleteConfirm('{0}','{1}');", menuItem.ID, menuItem.Name);
			}
			else if (menuItem.Type == 2)
			{
                menuItemEx.MenuType = "隐藏菜单" + suffix;
				menuItemEx.MenuDelVisble = "none";
				menuItemEx.MenuSystemVisble = "none";
				menuItemEx.MenuSystemShowVisble = "";
				menuItemEx.MenuSystemShowUrl = String.Format("javascript:ShowConfirm('{0}','{1}');", menuItem.ID, menuItem.Name);
			}
			else
			{
                menuItemEx.MenuType = "系统菜单" + suffix;
				menuItemEx.MenuDelVisble = "none";
				menuItemEx.MenuSystemVisble = "";
				menuItemEx.MenuSystemShowVisble = "none";
				menuItemEx.MenuSystemUrl = String.Format("javascript:HideConfirm('{0}','{1}');", menuItem.ID, menuItem.Name);
			}
			return menuItemEx;
		}

		/// <summary>
		/// 构建按类型过滤的超级链接字符串
		/// </summary>
		/// <returns></returns>
		string BuildStateLinks()
		{
			string links = @"<li> <a href='{8}state=100'   {0} >所有菜单<span class=""count"">({1})</span></a> |</li>
            <li><a href='{8}state=0'  {2} >系统菜单<span class=""count"">({3})</span></a> |</li>
            <li><a href='{8}state=99'  {4} >自定义菜单<span class=""count"">({5})</span></a> |</li>
            <li><a href='{8}state=2'  {6}  >隐藏菜单<span class=""count"">({7})</span></a></li>";

			string css100, css0, css1, css2;
			css100 = css0 = css1 = css2 = "";
			if (CurrentState == 100) css100 = "class=\"current\"";
			if (CurrentState == 0) css0 = "class=\"current\"";
			if (CurrentState == 99) css1 = "class=\"current\"";
			if (CurrentState == 2) css2 = "class=\"current\"";
			string url = "UserMenulistNew.aspx?";
			//if (Request["type"] != null && Request["type"].ToString() == "1")
			url += "type=1&";
			links = string.Format(links, css100, MenuHelper.GetMenuCountByType(100, EntityID),
				css0, MenuHelper.GetMenuCountByType(0, EntityID), css1, MenuHelper.GetMenuCountByType(99, EntityID),
				css2, MenuHelper.GetMenuCountByType(2, EntityID), url);

			return links;
		}
		protected void Pager_Fired(object sender, EventArgs e)
		{
			BindingMenu();
		}

		protected void ShowMenuButton_Click(object sender, EventArgs e)
		{
			string id = IDTextBox.Text.Trim();
			string name = MenuHelper.UpdateMenuItem(id, 0);
			string url = We7Helper.AddParamToUrl(Request.RawUrl, "reload", "menu");
			url = We7Helper.AddParamToUrl(url, "show", name);
			HttpContext.Current.Session.Clear();
			Response.Redirect(url);
		}
		protected void HideButton_Click(object sender, EventArgs e)
		{
			if (AppCtx.IsDemoSite)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "aler", "alert('演示站点，不能隐藏！');", true);
				return;
			}

			string id = IDTextBox.Text.Trim();
			string name = MenuHelper.UpdateMenuItem(id, 2);
			string url = We7Helper.AddParamToUrl(Request.RawUrl, "reload", "menu");
			url = We7Helper.AddParamToUrl(url, "hide", name);
			HttpContext.Current.Session.Clear();
			Response.Redirect(url);
		}
		protected void DeleteMenuButton_Click(object sender, EventArgs e)
		{
			if (AppCtx.IsDemoSite)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "aler", "alert('演示站点，不能删除！');", true);
				return;
			}

			string id = IDTextBox.Text.Trim();
			string name = MenuHelper.DeleteMenuItem(id);
			/*
			 * Content:删除菜单后同时删除XML数据, 根据数据库删除规则，删除顶级菜单时 级联删除子菜单
			 * Author: 勾立国 2011-7-13
			 * Begin:
			 */
			if (EntityID == "System.User")
			{

				//MenuItemXmlHelper helper = new MenuItemXmlHelper(Server.MapPath("/user/Resource/menuItems.xml"));
				//helper.DeleteMenuItemWidthChilds(id);

			}
			/*
			 * End
			 */

			string url = We7Helper.AddParamToUrl(Request.RawUrl, "reload", "menu");
			url = We7Helper.AddParamToUrl(url, "del", name);
			HttpContext.Current.Session.Clear();
			Response.Redirect(url);
		}

		[Serializable]
		public class MenuItemEx : We7.CMS.Common.MenuItem
		{
			private string menuType = "";
			public string MenuType
			{
				get { return menuType; }
				set { menuType = value; }
			}
			private string menuName = "";
			public string MenuName
			{
				get { return menuName; }
				set { menuName = value; }
			}
			private string menuDelVisble = "";
			public string MenuDelVisble
			{
				get { return menuDelVisble; }
				set { menuDelVisble = value; }
			}
			private string menuDelUrl = "";
			public string MenuDelUrl
			{
				get { return menuDelUrl; }
				set { menuDelUrl = value; }
			}
			private string menuSystemVisble = "";
			public string MenuSystemVisble
			{
				get { return menuSystemVisble; }
				set { menuSystemVisble = value; }
			}
			private string menuSystemUrl = "";
			public string MenuSystemUrl
			{
				get { return menuSystemUrl; }
				set { menuSystemUrl = value; }
			}
			private string menuSystemShowVisble = "";
			public string MenuSystemShowVisble
			{
				get { return menuSystemShowVisble; }
				set { menuSystemShowVisble = value; }
			}
			private string menuSystemShowUrl = "";
			public string MenuSystemShowUrl
			{
				get { return menuSystemShowUrl; }
				set { menuSystemShowUrl = value; }
			}
			public void Import(We7.CMS.Common.MenuItem it)
			{
				Created = it.Created;
				Name = it.Name;
				ID = it.ID;
				Title = it.Title;
				ParentID = it.ParentID;
				Type = it.Type;
				Index = it.Index;
				Href = it.Href;
				Group = it.Group;
			}
		}
	}
}
