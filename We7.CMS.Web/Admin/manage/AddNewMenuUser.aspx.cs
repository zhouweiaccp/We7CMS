using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using We7.Framework;
using We7.Framework.Util;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using System.Collections;
namespace We7.CMS.Web.Admin
{
	public partial class AddNewMenuUser : BasePage
	{

		protected string TabPanl
		{
			get
			{
				return Request["tabPanl"];
			}
		}
		public string Menuvisble
		{
			get
			{
				if (MenuID != null && MenuID != "")
				{
					return "none";
				}
				else
				{
					return "";
				}
			}
		}
		/// <summary>
		/// 一级顶部菜单是否显示
		/// </summary>
		public string TopMenuVisible
		{
			get
			{
				if (!string.IsNullOrEmpty(TabPanl) && TabPanl != "1")
				{
					return "none";
				}
				else if (MenuID != null && MenuID != "")
				{
					if (MenuHelper.GetMenuItem(MenuID).MenuType == (int)TypeOfMenu.TopMenu)
					{
						MenuRadioButtonList.SelectedValue = "1";
						return "";
					}
					else
					{
						return "none";
					}
				}
				else
				{
					MenuRadioButtonList.SelectedValue = "1";
					return "";
				}
			}
		}

		/// <summary>
		/// 分组菜单是否显示
		/// </summary>
		public string GroupMenuVisible
		{
			get
			{
				if (TabPanl == "2")
				{
					MenuRadioButtonList.SelectedValue = "2";
					return "";
				}
				else if (MenuID != null && MenuID != "")
				{
					if (MenuHelper.GetMenuItem(MenuID).MenuType == (int)TypeOfMenu.GroupMenu)
					{
						MenuRadioButtonList.SelectedValue = "2";
						return "";
					}
					else
					{
						return "none";
					}
				}
				else
				{
					return "none";
				}
			}
		}
		/// <summary>
		/// 子菜单是否显示
		/// </summary>
		public string ChildMenuVisible
		{
			get
			{
				if (TabPanl == "3")
				{
					MenuRadioButtonList.SelectedValue = "3";
					return "";
				}
				else if (MenuID != null && MenuID != "")
				{
					if (MenuHelper.GetMenuItem(MenuID).MenuType == (int)TypeOfMenu.NormalMenu)
					{
						MenuRadioButtonList.SelectedValue = "3";
						return "";
					}
					else
					{
						return "none";
					}
				}
				else
				{
					return "none";
				}
			}
		}
		/// <summary>
		/// 引用菜单是否显示
		/// </summary>
		public string ReferenceMenuVisible
		{
			get
			{
				if (TabPanl == "4")
				{
					MenuRadioButtonList.SelectedValue = "4";
					return "";
				}
				else if (MenuID != null && MenuID != "")
				{
					if (MenuHelper.GetMenuItem(MenuID).MenuType == (int)TypeOfMenu.ReferenceMenu)
					{
						MenuRadioButtonList.SelectedValue = "4";
						return "";
					}
					else
					{
						return "none";
					}
				}
				else
				{
					return "none";
				}
			}
		}

		string EntityID
		{
			get
			{
				if (CurrentMenu != null)
				{
					return CurrentMenu.EntityID;
				}
				else
				{
					return "System.User";
				}
			}
		}

		public string MenuText
		{
			get
			{
				if (CurrentMenu != null)
				{

					return "修改" + CurrentMenu.Name + "用户菜单";
				}
				else
				{
					return "新建用户菜单";
				}
			}
		}

		public string MenuLastText
		{
			get
			{
				if (CurrentMenu != null)
				{

					return "设置" + CurrentMenu.Name + "个性化属性";
				}
				else
				{
					return "根据用户自己习惯新建菜单，可以快捷的操作";
				}
			}
		}
		public We7.CMS.Common.MenuItem CurrentMenu
		{
			get
			{
				if (MenuID != null && MenuID != "")
				{
					return MenuHelper.GetMenuItemByID(MenuID);
				}
				else
				{
					return null;
				}
			}
		}
		public string MenuMainvisble
		{
			get
			{
				if (CurrentMenu != null)
				{

					if (CurrentMenu.ParentID == We7Helper.EmptyGUID)
					{
						return "";
					}
					else
					{
						return "none";
					}
				}
				else
				{
					return "";
				}
			}
		}
		public string MenuChildvisble
		{
			get
			{
				if (CurrentMenu != null)
				{
					if (CurrentMenu.ParentID == We7Helper.EmptyGUID)
					{
						return "none";
					}
					else
					{
						return "";
					}
				}
				else
				{
					return "none";
				}
			}
		}
		string MenuID
		{
			get
			{
				return Request["id"];
			}
		}
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Context.Application["$Menu_AllMenuData"] = null;
				BindMainIndex();
				BindDdlParent();
				BindChildIndex();
				BindDdlReferenceMenu();
				if (MenuID != null && MenuID != "")
				{
					BindingData();
					BindingGroupMenuData();
					BindingChildMenuData();
					BindingReferenceMenuData();

					SecondIndexDropDownList.Visible = false;
					ddlIndex_Reference.Visible = false;
					DropDownListType.Visible = false;

					//DropDownListType.Visible = false;
					//SecondIndexDropDownList.Visible = false;
					//ddlIndex_Reference.Visible = false;
				}
				ReturnHyperLink.NavigateUrl = "UserMenulistNew.aspx";

			}
		}


		void BindingData()
		{
			if (CurrentMenu != null)
			{
				if (CurrentMenu.ParentID == We7Helper.EmptyGUID)
				{
					MainTitleTextBox.Text = CurrentMenu.Name;
					MainDesTextBox.Text = CurrentMenu.Title;
					MianUrlTextBox.Text = CurrentMenu.Href;
					DropDownListType.SelectedIndex = -1;
					foreach (ListItem item in DropDownListType.Items)
					{
						string id = item.Value.Split(new char[] { ',' })[0];
						if (id == CurrentMenu.ParentID.ToString())
						{
							item.Selected = true;
							break;
						}
					}
				}
				else
				{
					TitleTextBox.Text = CurrentMenu.Name;
					DesTextBox.Text = CurrentMenu.Title;
					UrlTextBox.Text = CurrentMenu.Href;
				}
			}
		}

		/// <summary>
		/// 绑定分组菜单
		/// </summary>
		void BindingGroupMenuData()
		{
			txtTitle_Group.Text = CurrentMenu.Name;
			txtDes_Group.Text = CurrentMenu.Title;
			ddlParent.SelectedIndex = -1;
			foreach (ListItem item in ddlParent.Items)
			{
				string id = item.Value.Split(new char[] { ',' })[0];
				if (id == CurrentMenu.ParentID.ToString())
				{
					item.Selected = true;
					break;
				}
			}
		}

		/// <summary>
		/// 绑定子菜单
		/// </summary>
		void BindingChildMenuData()
		{
			TitleTextBox.Text = CurrentMenu.Name;
			DesTextBox.Text = CurrentMenu.Title;
			UrlTextBox.Text = CurrentMenu.Href;
			SecondIndexDropDownList.SelectedIndex = -1;
			foreach (ListItem item in SecondIndexDropDownList.Items)
			{
				//有问题
				string index = item.Value.Substring(item.Value.LastIndexOf(",") + 1);
				if (index == CurrentMenu.Index.ToString())
				{
					item.Selected = true;
					break;
				}
			}
		}

		/// <summary>
		/// 绑定引用菜单
		/// </summary>
		void BindingReferenceMenuData()
		{
			txtTitle_Reference.Text = CurrentMenu.Name;
			txtDes_Reference.Text = CurrentMenu.Title;
			ddlReferenceMenu.SelectedValue = CurrentMenu.ID;

			ddlIndex_Reference.SelectedIndex = -1;
			foreach (ListItem item in ddlIndex_Reference.Items)
			{
				string id = item.Value.Split(new char[] { ',' })[0];
				if (id == CurrentMenu.ParentID.ToString())
				{
					item.Selected = true;
					break;
				}
			}
		}

		void BindMainIndex()
		{
			List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID, 0, EntityID);
			int i = 0;
			int group = 1;
			foreach (We7.CMS.Common.MenuItem menuItem in menus)
			{
				string name = menuItem.Title.ToString();
				string value = menuItem.ID + "," + menuItem.Group.ToString() + "," + menuItem.Index.ToString();
				ListItem item = new ListItem(name, value);
				i = menuItem.Index;
				group = menuItem.Group;
				DropDownListType.Items.Add(item);
			}

			ListItem appendItem = new ListItem("（追加到这里）", "," + group.ToString() + "," + (i + 2).ToString());
			DropDownListType.Items.Add(appendItem);

			string myname = "文章";
			if (MenuID != null && MenuID != "")
			{
				myname = CurrentMenu.Title;
			}

			DropDownListType.SelectedIndex = -1;
			ListItem currentItem = DropDownListType.Items.FindByText(myname);
			if (currentItem != null)
				currentItem.Selected = true;
		}

		/// <summary>
		/// 绑定分组菜单的父菜单下拉框
		/// </summary>
		void BindDdlParent()
		{
			List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID, 0, EntityID);
			int i = 0;
			int group = 1;
			foreach (We7.CMS.Common.MenuItem menuItem in menus)
			{
				string name = menuItem.Title.ToString();
				string value = menuItem.ID + "," + menuItem.Group.ToString() + "," + menuItem.Index.ToString();
				ListItem item = new ListItem(name, value);
				i = menuItem.Index;
				group = menuItem.Group;
				ddlParent.Items.Add(item);
			}
		}

		void BindChildIndex()
		{
			List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID, 2, EntityID);

			foreach (We7.CMS.Common.MenuItem menuItem in menus)
			{
				string name = "├" + menuItem.Title.ToString();
				string value = menuItem.ID + ",0";
				if (menuItem.Items.Count > 0) value = menuItem.Items[0].ID + ",0";
				ListItem item = new ListItem(name, value);
				SecondIndexDropDownList.Items.Add(item);
				ddlIndex_Reference.Items.Add(item);
				int i = 0;
				foreach (We7.CMS.Common.MenuItem submenu in menuItem.Items)
				{
					string sname = "├──" + submenu.Title.ToString();
					string svalue = submenu.ID + "," + submenu.Index.ToString();
					ListItem sitem = new ListItem(sname, svalue);
					i = submenu.Index;
					SecondIndexDropDownList.Items.Add(sitem);
					ddlIndex_Reference.Items.Add(sitem);
					int j = 0;
					foreach (We7.CMS.Common.MenuItem thirdmenu in submenu.Items)
					{
						string tname = "├──├─" + thirdmenu.Title.ToString();
						string tvalue = submenu.ParentID + "," + thirdmenu.ParentID + "," + thirdmenu.Index.ToString();
						ListItem thitem = new ListItem(tname, tvalue);
						j = thirdmenu.Index;
						SecondIndexDropDownList.Items.Add(thitem);
						ddlIndex_Reference.Items.Add(thitem);
					}
					ListItem appendItem3 = new ListItem("├──├─（追加到这里-三级菜单）", submenu.ID + "," + (j + 2).ToString());
					SecondIndexDropDownList.Items.Add(appendItem3);
					ddlIndex_Reference.Items.Add(appendItem3);
				}
				ListItem appendItem = new ListItem("├──（追加到这里）", menuItem.ID + "," + (i + 2).ToString());
				//SecondIndexDropDownList.Items.Add(appendItem);
				ddlIndex_Reference.Items.Add(appendItem);
			}

			string myname = "流量统计";
			if (MenuID != null && MenuID != "")
			{
				myname = CurrentMenu.Title;
			}

			SecondIndexDropDownList.SelectedIndex = -1;
			ddlIndex_Reference.SelectedIndex = -1;

			ListItem currentItem = SecondIndexDropDownList.Items.FindByText("├──" + myname);
			if (currentItem == null)
				currentItem = SecondIndexDropDownList.Items.FindByText("├──├─" + myname);
			if (currentItem != null)
			{
				SecondIndexDropDownList.SelectedIndex = -1;
				currentItem.Selected = true;
			}

			ListItem currentItemReference = ddlIndex_Reference.Items.FindByText("├──" + myname);
			if (currentItemReference == null)
				currentItemReference = ddlIndex_Reference.Items.FindByText("├──├─" + myname);
			if (currentItemReference != null)
			{
				ddlIndex_Reference.SelectedIndex = -1;
				currentItemReference.Selected = true;
			}

		}

		/// <summary>
		/// 绑定引用菜单
		/// </summary>
		void BindDdlReferenceMenu()
		{
			List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID, 2, EntityID);

			foreach (We7.CMS.Common.MenuItem menuItem in menus)
			{
				string name = "├" + menuItem.Title.ToString();
				string value = menuItem.ID;
				ListItem item = new ListItem(name, value);
				ddlReferenceMenu.Items.Add(item);
				int i = 0;
				foreach (We7.CMS.Common.MenuItem submenu in menuItem.Items)
				{
					string sname = "├──" + submenu.Title.ToString();
					string svalue = submenu.ID;
					ListItem sitem = new ListItem(sname, svalue);
					i = submenu.Index;
					ddlReferenceMenu.Items.Add(sitem);
					int j = 0;
					foreach (We7.CMS.Common.MenuItem thirdmenu in submenu.Items)
					{
						string tname = "├──├─" + thirdmenu.Title.ToString();
						string tvalue = thirdmenu.ID;
						ListItem thitem = new ListItem(tname, tvalue);
						j = thirdmenu.Index;
						ddlReferenceMenu.Items.Add(thitem);
					}
				}
			}
			ddlReferenceMenu.Items.Insert(0, new ListItem("请选择", ""));
			ddlReferenceMenu.SelectedIndex = 0;
		}

		string GetIconFileName()
		{
			string theme = SiteSettingHelper.Instance.Config.CMSTheme;
			if (theme == null || theme == "") theme = "classic";
			string path = "~/admin/" + Constants.ThemePath + "/" + theme + "/images";

			Boolean fileOk = false;
			//判断是否选择了文件
			if (IconFileUpload.HasFile && HoverIconFileUpload.HasFile)
			{
				//返回文件的扩展名
				string fileExtension1 = System.IO.Path.GetExtension(IconFileUpload.FileName).ToLower();
				string fileExtension2 = System.IO.Path.GetExtension(HoverIconFileUpload.FileName).ToLower();
				//设置文件类型
				string[] allowExtensions = { ".gif", ".png" };
				//判断选择文件是否符合条件
				for (int i = 0; i < allowExtensions.Length; i++)
				{
					if (fileExtension1 == allowExtensions[i] && fileExtension2 == allowExtensions[i])
					{
						fileOk = true;
					}
				}
				//文件大小的设置
				if (IconFileUpload.PostedFile.ContentLength > 1024000 || HoverIconFileUpload.PostedFile.ContentLength > 1024000)
				{
					fileOk = false;
				}
				if (fileOk)
				{
					string tmpPath = "/_temp";
					if (!Directory.Exists(Server.MapPath(tmpPath)))
						Directory.CreateDirectory(Server.MapPath(tmpPath));
					string NewpathFile = Path.Combine(Server.MapPath(tmpPath), IconFileUpload.FileName);
					string hoverNewPath = Path.Combine(Server.MapPath(tmpPath), HoverIconFileUpload.FileName);
					IconFileUpload.PostedFile.SaveAs(NewpathFile);
					HoverIconFileUpload.PostedFile.SaveAs(hoverNewPath);
					CreateUploadImageIcon(path, NewpathFile, hoverNewPath, ".gif");
					return CreateUploadImageIcon(path, NewpathFile, hoverNewPath, ".png");
				}
				else
				{
					throw new Exception("图片类型不对或文件超出1024KB！");
				}
			}
			else if (!IconFileUpload.HasFile && !HoverIconFileUpload.HasFile)
			{
				CreateLetterNewIcon(path, ".gif");
				return CreateLetterNewIcon(path, ".png");
			}
			else
				throw new Exception("两个图标都得选，都不选的话，默认生成首字母图标。");
		}

		/// <summary>
		/// 上传的图片生成图标
		/// </summary>
		/// <param name="pathFile1"></param>
		/// <param name="pathFile2"></param>
		/// <returns></returns>
		string CreateUploadImageIcon(string path, string pathFile1, string pathFile2, string imgType)
		{
			string imgID = MenuHelper.GetChineseSpell(MainTitleTextBox.Text);
			imgID = imgID.Replace(".", "_");
			string iconFile = Path.Combine(Server.MapPath(path), "menu_u_" + imgID + imgType);
			string iconHoverFile = Path.Combine(Server.MapPath(path), "menu_u_" + imgID + "_hover" + imgType);
			ImageUtils.GenerateIcon(pathFile1, iconFile, 30, 30, 15, 15, imgType);
			ImageUtils.GenerateIcon(pathFile2, iconHoverFile, 30, 30, 15, 15, imgType);
			return Path.GetFileName(iconFile);
		}

		/// <summary>
		/// 菜单名称首字母生成图标
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string CreateLetterNewIcon(string path, string imgType)
		{
			string imgID = MenuHelper.GetChineseSpell(MainTitleTextBox.Text);
			imgID = imgID.Replace(".", "_");
			string firstChar = MainTitleTextBox.Text.Substring(0, 1);
			firstChar = MenuHelper.GetChineseSpell(firstChar);
			firstChar = firstChar.Substring(0, 1).ToUpper();
			string iconFile = Path.Combine(Server.MapPath(path), "menu_u_" + imgID + imgType);
			string iconHoverFile = Path.Combine(Server.MapPath(path), "menu_u_" + imgID + "_hover" + imgType);
			ImageUtils.GenerateImgFromText(iconFile, firstChar, 30, 30, "Times New Roman", 14, System.Drawing.Color.DarkGray, imgType);
			ImageUtils.GenerateImgFromText(iconHoverFile, firstChar, 30, 30, "Times New Roman", 14, System.Drawing.Color.Red, imgType);
			return Path.GetFileName(iconFile);
		}

		/// <summary>
		/// 一级顶部菜单提交
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void SubmitButton_Click(object sender, EventArgs e)
		{
			if (AppCtx.IsDemoSite)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "aler", "alert('演示站点，不能新建菜单！');", true);
				return;
			}

			try
			{
				string mainIconName = "";
				string id = "";
				if (MenuID != null && MenuID != "")
				{
					//if (IconFileUpload.HasFile && HoverIconFileUpload.HasFile)
					//{
					//    mainIconName = GetIconFileName();
					//}
					id = MenuID;
					We7.CMS.Common.MenuItem menuTemp = MenuHelper.GetMenuItem(id);
					if (menuTemp != null)
					{
						menuTemp.Name = MainTitleTextBox.Text.ToString();
						menuTemp.Title = MainDesTextBox.Text.ToString();
						menuTemp.Href = MianUrlTextBox.Text.ToString();
					}
					MenuHelper.UpdateMenuItem(menuTemp);
					Messages.ShowMessage("您成功修改" + menuTemp.Title + "菜单,更新成功之后请退出重新登陆才能生效");
				}
				else
				{
					//mainIconName = GetIconFileName();

					string mianNameText = MainTitleTextBox.Text.Trim();
					string mainTitle = MainDesTextBox.Text.Trim();
					string mianUrl = MianUrlTextBox.Text.Trim();
					int maingroup = Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[1]);

					//wait:暂时插入到最后
					//int mainIndex = 0;
					//if (Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[2]) > 0)
					//{
					//    mainIndex = Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[2]) - 1;
					//}
					int mainIndex = DropDownListType.Items.Count + 1;

					string oldId = MenuHelper.CreateMainMenu_User(mianNameText, mainTitle, maingroup, mainIndex, mainIconName, mianUrl, id, EntityID, (int)TypeOfMenu.TopMenu, "");
					Response.Redirect(Request.RawUrl + "?tabPanl=1");
				}
			}
			catch (Exception ex)
			{
				Messages.ShowMessage(ex.Message);
			}
		}


		/// <summary>
		/// 分组菜单提交
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void SaveButton_Click(object sender, EventArgs e)
		{
			if (AppCtx.IsDemoSite)
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "aler", "alert('演示站点，不能新建菜单！');", true);
				return;
			}
			try
			{
				string mainIconName = "";
				string id = "";
				if (MenuID != null && MenuID != "")
				{
					//if (IconFileUpload.HasFile && HoverIconFileUpload.HasFile)
					//{
					//    mainIconName = GetIconFileName();
					//}
					id = MenuID;
					We7.CMS.Common.MenuItem menuTemp = MenuHelper.GetMenuItem(id);
					if (menuTemp != null)
					{
						menuTemp.Name = txtTitle_Group.Text.ToString();
						menuTemp.Title = txtDes_Group.Text.ToString();
					}
					MenuHelper.UpdateMenuItem(menuTemp);
					Messages.ShowMessage("您成功修改" + menuTemp.Name + "菜单,更新成功之后请退出重新登陆才能生效");
				}
				else
				{
					//mainIconName = GetIconFileName();

					string mianNameText = txtTitle_Group.Text.Trim();
					string mainTitle = txtDes_Group.Text.Trim();
					//string mianUrl = ""; //MianUrlTextBox.Text.Trim();
					string parentID = ddlParent.SelectedValue.Split(',')[0];
					int maingroup = Int32.Parse(ddlParent.SelectedItem.Value.Split(',')[1]);

					//wait:暂时插入到最后
					//int mainIndex = 0;
					//if (Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[2]) > 0)
					//{
					//    mainIndex = Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[2]) - 1;
					//}
					int mainIndex = ddlParent.Items.Count + 1;

					string oldId = MenuHelper.CreateSubMenu_User(mianNameText, mainTitle, "", mainIndex, parentID, id, EntityID, (int)TypeOfMenu.GroupMenu, "");
					Response.Redirect(Request.RawUrl + "?tabPanl=2");
				}
			}
			catch (Exception ex)
			{
				Messages.ShowMessage(ex.Message);
			}
		}

		/// <summary>
		/// 子菜单提交
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnChildSave_Click(object sender, EventArgs e)
		{
			string id = "";
			if (MenuID != null && MenuID != "")
			{
				id = MenuID;
			}
			string firstTitle = DesTextBox.Text.Trim();
			string firstNameText = TitleTextBox.Text.Trim();
			string firstUrl = UrlTextBox.Text.Trim();

			#region 此处逻辑在三级菜单时报异常，更改如下
			//if (Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]) > 0)
			//{
			//   firstIndex= Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]) - 1;
			//}
			#endregion
			string selectValue = SecondIndexDropDownList.SelectedValue;
			int index = int.Parse(selectValue.Substring(selectValue.LastIndexOf(",") + 1));
			int firstIndex = index > 0 ? index : 0;
			string parentID = selectValue.Split(',')[0];
			if (parentID == We7Helper.EmptyGUID)
			{
				Messages.ShowError("您编辑的是子菜单，不能选择主菜单的位置，请您选择子菜单。");
			}
			else
			{
				if (MenuID != null && MenuID != "")
				{
					id = MenuID;
					We7.CMS.Common.MenuItem menuTemp = MenuHelper.GetMenuItem(id);
					if (menuTemp != null)
					{
						menuTemp.Name = firstNameText;
						menuTemp.Title = firstTitle;
						menuTemp.Href = firstUrl;
					}
					MenuHelper.UpdateMenuItem(menuTemp);
					Messages.ShowMessage("您成功修改" + menuTemp.Name + "菜单,更新成功之后请退出重新登陆才能生效");

				}
				else
				{
					string oldId = MenuHelper.CreateSubMenu_User(firstNameText, firstTitle, firstUrl, firstIndex, parentID, id, EntityID, (int)TypeOfMenu.NormalMenu, "");
					Response.Redirect(Request.RawUrl + "?tabPanl=3");
				}
			}
		}

		/// <summary>
		/// 引用菜单提交
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnReferenceSave_Click(object sender, EventArgs e)
		{
			string id = "";
			if (MenuID != null && MenuID != "")
			{
				id = MenuID;
			}
			string firstTitle = txtTitle_Reference.Text.Trim();
			string firstNameText = txtDes_Reference.Text.Trim();

			#region 此处逻辑在三级菜单时报异常，更改如下
			//if (Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]) > 0)
			//{
			//   firstIndex= Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]) - 1;
			//}
			#endregion
			string selectValue = ddlIndex_Reference.SelectedValue;
			int index = int.Parse(selectValue.Substring(selectValue.LastIndexOf(",") + 1));
			int firstIndex = index > 0 ? index : 0;
			string referenceID = ddlReferenceMenu.SelectedValue;

			string parentID = selectValue.Split(',')[0];
			if (parentID == We7Helper.EmptyGUID)
			{
				Messages.ShowError("您编辑的是子菜单，不能选择主菜单的位置，请您选择子菜单。");
			}
			else
			{
				if (MenuID != null && MenuID != "")
				{
					id = MenuID;
					We7.CMS.Common.MenuItem menuTemp = MenuHelper.GetMenuItem(id);
					if (menuTemp != null)
					{
						menuTemp.Name = firstNameText;
						menuTemp.Title = firstTitle;
						menuTemp.ReferenceID = referenceID;
					}
					MenuHelper.UpdateMenuItem(menuTemp);
					Messages.ShowMessage("您成功修改" + menuTemp.Name + "菜单,更新成功之后请退出重新登陆才能生效");
				}
				else
				{
					string oldId = MenuHelper.CreateSubMenu_User(firstNameText, firstTitle, "", firstIndex, parentID, id, EntityID, (int)TypeOfMenu.ReferenceMenu, referenceID);
					Response.Redirect(Request.RawUrl + "?tabPanl=4");
				}
			}
		}
	}
}
