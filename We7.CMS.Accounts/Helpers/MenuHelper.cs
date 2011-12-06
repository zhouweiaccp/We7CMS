using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using We7.CMS;
using Thinkment.Data;
using System.Text.RegularExpressions;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Framework;

namespace We7.CMS.Accounts
{
	/// <summary>
	///后台菜单提取判断处理模块
	/// </summary>
	[Helper("We7.MenuHelper")]
	public class MenuHelper : BaseHelper
	{
		public MenuHelper()
		{
		}
		HttpContext Context
		{
			get { return HttpContext.Current; }
		}

		HelperFactory HelperFactory
		{
			get { return (HelperFactory)Context.Application[HelperFactory.ApplicationID]; }
		}

		IAccountHelper AccountHelper
		{
			get { return AccountFactory.CreateInstance(); }
		}

		/// <summary>
		/// 全部菜单列表数据
		/// </summary>
		public List<MenuItem> AllMenuData
		{
			get
			{
				if (Context.Application["$Menu_AllMenuData"] == null)
				{
					List<MenuItem> allMenu = GetMenus();
					Context.Application["$Menu_AllMenuData"] = allMenu;
				}
				return Context.Application["$Menu_AllMenuData"] as List<MenuItem>;
			}
		}
		/// <summary>
		/// 菜单最大级数
		/// </summary>
		public int Recursives
		{
			get { return recursives; }
			set { recursives = value; }
		}
		private int recursives = 2;

		string AccountID
		{
			get
			{
				return HttpContext.Current.User.Identity.Name;// Security.CurrentAccountID;
			}
		}

		#region 菜单简化后新方法

		/// <summary>
		/// 获取菜单树
		/// </summary>
		/// <param name="pid">父菜单</param>
		/// <returns></returns>
		public List<We7.CMS.Common.MenuItem> GetMenuTree(string pid)
		{
			return GetMenuTree(pid, "");
		}

		public List<We7.CMS.Common.MenuItem> GetMenuTree(string pid, OwnerRank rank)
		{
			return GetMenuTree(pid, ConvertToEntityID(rank));
		}

		/// <summary>
		/// 获取菜单树
		/// </summary>
		/// <param name="pid">父菜单</param>
		/// <param name="type">类型：0-系统菜单；1-会员菜单</param>
		/// <returns></returns>
		public List<We7.CMS.Common.MenuItem> GetMenuTree(string pid, string type)
		{
			List<We7.CMS.Common.MenuItem> myMenuList = new List<We7.CMS.Common.MenuItem>();

			//先取出全部菜单节点
			string p = pid;
			if (p == null || p == String.Empty)
			{
				p = We7Helper.EmptyGUID;
			}
			List<We7.CMS.Common.MenuItem> allMenuList = new List<We7.CMS.Common.MenuItem>();

			//if (We7Helper.IsEmptyID(AccountID))  //获取系统管理员菜单
			if (We7Helper.IsEmptyID(AccountID))
			{
				myMenuList = GetMenus(p, Recursives, type);
			}
			else                                                //一般用户
			{
				allMenuList = GetMenus(p, Recursives, type);
				//按权限取出全部可操作菜单ID列表
				List<string> myMenus = null;
				if (string.IsNullOrEmpty(type) || type.Trim() == "System.Administrator")
				{
					myMenus = GetMenuIDListByAccount(AccountID);
				}
				else if (type.Trim() == "System.User")
				{
					myMenus = GetMenuIDListByAccount(AccountID, "System.User");
				}
				else if (type.Trim() == "Group.Administration")
				{
					myMenus = GetMenuIDListByAccount(AccountID, "Group.Administration");
				}
				else
				{
					myMenus = new List<string>();
					//throw new ArgumentOutOfRangeException("The user type cant't be true!");
				}

				//挨个判断是否具有权限
				foreach (We7.CMS.Common.MenuItem menu in allMenuList)
				{
					We7.CMS.Common.MenuItem parentMenu = new We7.CMS.Common.MenuItem();
					List<We7.CMS.Common.MenuItem> subList = new List<We7.CMS.Common.MenuItem>();

					foreach (We7.CMS.Common.MenuItem submenu in menu.Items)
					{
						if (myMenus.Contains(submenu.ID))
							subList.Add(submenu);
					}

					if (subList.Count > 0)
					{
						parentMenu = menu;
						parentMenu.Items.Clear();
						foreach (We7.CMS.Common.MenuItem m in subList) parentMenu.Items.Add(m);
						myMenuList.Add(parentMenu);
					}
				}
			}

			//重置父菜单节点URL，设置为第一个子菜单
			foreach (We7.CMS.Common.MenuItem menu in myMenuList)
			{
				if (menu.Items != null && menu.Items.Count > 0)
				//if (menu.Items != null)
				{
					if (menu.EntityID.Trim() != "System.User")
					{
						menu.Href = menu.Items[0].Href;
					}
				}
				else
					menu.Href = "#";
			}
			return myMenuList;
		}

		/// <summary>
		/// UpdatePermissionStateOfMenuTree
		/// </summary>
		/// <param name="mytree"></param>
		/// <param name="menuIds"></param>
		/// <returns></returns>
		public int UpdatePermissionStateOfMenuTree(ref List<We7.CMS.Common.MenuItem> mytree, List<string> menuIds)
		{
			int checkedCount = 0;
			foreach (We7.CMS.Common.MenuItem menu in mytree)
			{
				int checkedSons = 0;
				if (menu.Items != null && menu.Items.Count > 0)
				{
					List<We7.CMS.Common.MenuItem> sons = new List<MenuItem>();
					sons = menu.Items;
					checkedSons = UpdatePermissionStateOfMenuTree(ref sons, menuIds);
					menu.Items = sons;
				}

				if (menuIds.Contains(menu.ID))
				{
					menu.PermissionState = "checked open";
					checkedCount++;
				}
				else if (checkedSons != 0 && checkedSons == menu.Items.Count)
				{
					menu.PermissionState = "checked open";
					checkedCount++;
				}
				else if (checkedSons > 0 && checkedSons < menu.Items.Count)
					menu.PermissionState = "undetermined open";
				else
					menu.PermissionState = "unchecked";
			}
			return checkedCount;
		}

		/// <summary>
		/// 取得当前用户的Menu列表值
		/// </summary>
		/// <returns></returns>
		public List<MenuItem> GetMyMenuList()
		{
			return GetMyMenuList("System.Administration");
		}
		/// <summary>
		/// GetMyMenuList
		/// </summary>
		/// <param name="rank"></param>
		/// <returns></returns>
		public List<MenuItem> GetMyMenuList(OwnerRank rank)
		{
			return GetMyMenuList(ConvertToEntityID(rank));
		}

		/// <summary>
		/// 取得当前用户的Menu列表
		/// </summary>
		/// <param name="rank">类别</param>
		/// <returns></returns>
		public List<MenuItem> GetMyMenuList(string entityID)
		{
			if (string.IsNullOrEmpty(entityID))
				entityID = "System.Administration";

			List<MenuItem> myMenuList = new List<MenuItem>();
			if (We7Helper.IsEmptyID(AccountID))
			{
				foreach (MenuItem menu in AllMenuData)
				{
					if (menu.EntityID == entityID && menu.Type != 2)
					{
						myMenuList.Add(menu);
					}
				}
			}
			else
			{
				List<string> myMenus = GetMenuIDListByAccount(AccountID, entityID);
				//挨个判断是否具有权限
				foreach (MenuItem menu in AllMenuData)
				{
					if (myMenus.Contains(menu.ID) && menu.EntityID == entityID && menu.Type != 2)
					{
						myMenuList.Add(menu);
					}
				}
			}
			return myMenuList;
		}

		/// <summary>
		/// 递归：获取父节点下的菜单树
		/// </summary>
		/// <param name="parentID">父节点</param>
		/// <param name="recursive">递归级数</param>
		/// <returns></returns>
		public List<MenuItem> GetMenus(string parentID, int recursive, string entityID)
		{
			return GetMenus(parentID, recursive, entityID, false);
		}

		/// <summary>
		///  递归：获取父节点下的菜单树
		/// </summary>
		/// <param name="parentID">父节点</param>
		/// <param name="recursive">递归级数</param>
		/// <param name="entityID">分类：系统菜单？会员菜单？</param>
		/// <param name="inludeHide">是否包括隐藏菜单</param>
		/// <returns></returns>
		public List<MenuItem> GetMenus(string parentID, int recursive, string entityID, bool inludeHide)
		{
			if (string.IsNullOrEmpty(entityID))
				entityID = "System.Administration";

			List<MenuItem> myList = new List<MenuItem>();
			foreach (MenuItem item in AllMenuData)
			{
				if (item.ParentID == parentID && item.EntityID == entityID && (item.Type != 2 || inludeHide))
					myList.Add(item);
			}
			if (recursive > 0)
			{
				foreach (MenuItem item in myList)
				{
					item.Items.Clear();
					List<MenuItem> subItems = GetMenus(item.ID, recursive - 1, entityID, inludeHide);
					item.Items.AddRange(subItems);
				}
			}
			return myList;
		}

		/// <summary>
		/// 取出Menu表的全部数据
		/// </summary>
		/// <returns></returns>
		public List<MenuItem> GetMenus()
		{
			Order[] o = new Order[] { new Order("Index", OrderMode.Asc) };
			return Assistant.List<MenuItem>(null, o);
		}

		/// <summary>
		/// 根据条件得到Menu
		/// </summary>
		/// <param name="criteria"></param>
		/// <returns></returns>
		public List<MenuItem> GetMenusByCriteria(Criteria criteria)
		{
			Order[] o = new Order[] { new Order("Index", OrderMode.Asc) };
			return Assistant.List<MenuItem>(criteria, o);
		}

		/// <summary>
		/// 根据用户accountID取得菜单ID列表，
		/// </summary>
		/// <param name="accountID"></param>
		/// <returns></returns>
		List<string> GetMenuIDListByAccount(string accountID)
		{
			return GetMenuIDListByAccount(accountID, "System.Administration");
		}

		/// <summary>
		/// 根据用户accountID取得菜单ID列表，
		/// </summary>
		/// <param name="accountID"></param>
		/// <param name="entityID"></param>
		/// <returns></returns>
		List<string> GetMenuIDListByAccount(string accountID, string entityID)
		{
			return AccountHelper.GetPermissionContents(accountID, entityID);
		}

		/// <summary>
		/// 去重后合并字符串列表
		/// </summary>
		/// <param name="sourceList"></param>
		/// <param name="newList"></param>
		/// <returns></returns>
		List<string> MergeList(List<string> sourceList, List<string> newList)
		{
			List<string> resultList = sourceList;
			foreach (string itemString in newList)
			{
				if (!sourceList.Exists(delegate(string s) { return (s == itemString) ? true : false; }))
				{
					resultList.Add(itemString);
				}
			}
			return resultList;
		}

		#endregion

		#region 会员菜单管理
		/// <summary>
		/// 取得会员菜单
		/// </summary>
		/// <param name="orderKey">排序字段</param>
		/// <param name="start">开始条目</param>
		/// <param name="end">条目数</param>
		/// <returns></returns>
		public List<MenuItem> GetMemberMenu(string orderKey, int start, int end)
		{
			Criteria c = new Criteria(CriteriaType.NotEquals, "ID", "");

			//Criteria c = new Criteria(CriteriaType.Equals, "Type", type);
			if (orderKey == null || orderKey == "") orderKey = "Created";
			Order[] orders = new Order[] { new Order(orderKey, OrderMode.Desc) };
			if (Assistant.Count<MenuItem>(c) > 0)
			{
				return Assistant.List<MenuItem>(c, orders, start, end);
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// 取得菜单数目
		/// </summary>
		/// <returns></returns>
		public int GetMenuCount()
		{
			Criteria c = new Criteria(CriteriaType.NotEquals, "ID", "");
			//if (type != null)
			//    c.Add(CriteriaType.Equals, "Type", type);
			return Assistant.Count<MenuItem>(c);
		}
		/// <summary>
		/// 取得上级名称
		/// </summary>
		/// <param name="parentID"></param>
		/// <returns></returns>
		public string GetParentName(string parentID)
		{
			Criteria c = new Criteria(CriteriaType.Equals, "ID", parentID);
			if (Assistant.Count<MenuItem>(c) > 0)
			{
				return Assistant.List<MenuItem>(c, null)[0].Title;
			}
			else
			{
				return "顶级菜单";
			}
		}
		/// <summary>
		/// 取得菜单信息
		/// </summary>
		/// <param name="id">菜单ID</param>
		/// <returns></returns>
		public MenuItem GetMenuItem(string id)
		{
			Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
			if (Assistant.Count<MenuItem>(c) > 0)
			{
				return Assistant.List<MenuItem>(c, null)[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 通过名称取得菜单信息
		/// </summary>
		/// <param name="title">菜单名称</param>
		/// <returns></returns>
		public MenuItem GetMenuItemByTitle(string title)
		{
			Criteria c = new Criteria(CriteriaType.Equals, "Title", title);
			if (Assistant.Count<MenuItem>(c) > 0)
			{
				return Assistant.List<MenuItem>(c, null)[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 更新菜单信息
		/// </summary>
		/// <param name="menuItem">菜单信息</param>
		public void UpdateMenuItem(MenuItem menuItem)
		{
			Context.Application.Remove("$Menu_AllMenuData");
			Context.Session.Remove("ALLSHOWMEMUITEM");
			try
			{
				string[] listString = new string[] { "Title", "Name", "Href", "Index", "Icon", "IconHover", "Group" };
				Assistant.Update(menuItem, listString);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 更新菜单类型
		/// </summary>
		/// <param name="id">菜单Id</param>
		/// <param name="type">菜单类型</param>
		/// <returns></returns>
		public string UpdateMenuItem(string id, int type)
		{
			Context.Application.Remove("$Menu_AllMenuData");
			Context.Session.Remove("ALLSHOWMEMUITEM");
			try
			{
				MenuItem menuItem = GetMenuItem(id);
				menuItem.Type = type;
				string[] listString = new string[] { "Type" };
				Assistant.Update(menuItem, listString);
				return menuItem.Name;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 删除菜单
		/// </summary>
		/// <param name="ids">菜单ID列表</param>
		/// <returns></returns>
		public int DeleteMenuItem(List<string> ids)
		{
			int count = 0;
			if (ids != null)
			{
				foreach (string id in ids)
				{
					DeleteMenuItem(id);
					count++;
				}
			}
			return count;
		}
		/// <summary>
		/// 删除菜单
		/// </summary>
		/// <param name="id">菜单ID</param>
		/// <returns></returns>
		public string DeleteMenuItem(string id)
		{
			Context.Application.Remove("$Menu_AllMenuData");
			if (Context.Session != null)
				Context.Session.Remove("ALLSHOWMEMUITEM");
			MenuItem menuItem = GetMenuItem(id);
			if (menuItem != null)
			{
				List<MenuItem> childMenu = GetMenuItemByParentID(menuItem.ID);
				if (childMenu != null)
				{
					foreach (MenuItem childMenuItem in childMenu)
					{
						DeleteMenuItem(childMenuItem.ID);
					}
				}
				Assistant.Delete(menuItem);
				Assistant.DeleteList<MenuItem>(new Criteria(CriteriaType.Equals, "ReferenceID", menuItem.ID));
				return menuItem.Name;
			}
			return "";
		}
		
		/// <summary>
		/// 检测菜单是否存在
		/// </summary>
		/// <param name="menuName">菜单名</param>
		/// <param name="id">菜单ID</param>
		/// <returns></returns>
		public bool ExistMenuItem(string menuName, string id)
		{
			Criteria c = new Criteria(CriteriaType.Equals, "Name", menuName);
			if (id != null)
			{
				c.Add(CriteriaType.NotEquals, "ID", id);
			}
			if (Assistant.Count<MenuItem>(c) > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 取得索引号
		/// </summary>
		/// <param name="parentID"></param>
		/// <returns></returns>
		public int GetLastIndex(string parentID)
		{
			Criteria c = new Criteria(CriteriaType.Equals, "ParentID", parentID);
			Order[] orders = new Order[] { new Order("Index", OrderMode.Desc) };
			if (Assistant.Count<MenuItem>(c) > 0)
			{
				return Assistant.List<MenuItem>(c, orders)[0].Index;
			}
			else
			{
				return 1;
			}
		}

		/// <summary>
		/// 通过上级ID取得所有子菜单
		/// </summary>
		/// <param name="parentID">上级ID</param>
		/// <returns></returns>
		public List<MenuItem> GetMenuItemByParentID(string parentID)
		{
			Criteria c = new Criteria(CriteriaType.Equals, "ParentID", parentID);
			//Order[] orders = new Order[] { new Order("Index", OrderMode.Desc) };

			Order[] orders = new Order[2];
			orders[0] = new Order();
			orders[0].Mode = OrderMode.Desc;
			orders[0].Name = "Index";

			orders[1] = new Order();
			orders[1].Mode = OrderMode.Desc;
			orders[1].Name = "Updated";
			if (Assistant.Count<MenuItem>(c) > 0)
			{
				return Assistant.List<MenuItem>(c, orders);
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// 取得最后一个菜单信息
		/// </summary>
		/// <param name="parentID">上级菜单ID</param>
		/// <returns></returns>
		public MenuItem GetLastMenuItem(string parentID)
		{
			Criteria c = new Criteria(CriteriaType.Equals, "ParentID", parentID);
			Order[] orders = new Order[] { new Order("Index", OrderMode.Desc) };
			if (Assistant.Count<MenuItem>(c) > 0)
			{
				return Assistant.List<MenuItem>(c, orders)[0];
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// ID号是否有效
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool ExsitMenuID(string id)
		{
			Regex myRex = new Regex("^[-+]?[1-9]+[.]?[0-9]*([eE][-+]?[0-9]+)?$");
			Match m = myRex.Match(id);
			return m.Success;
		}
		/// <summary>
		/// 根据最后菜单ID取得菜单ID
		/// </summary>
		/// <param name="lastMenuID">菜单ID最后几位</param>
		/// <returns></returns>
		public string GetMenuID(string lastMenuID)
		{
			string lastID = "";
			string menuID = "";
			int id = 0;
			if (lastMenuID != null)
			{
				menuID = lastMenuID.Substring(15, 4);
				if (ExsitMenuID(menuID))
				{
					id = Int32.Parse(menuID) + 1;
					lastID = "{00000000-0000-" + id + "-0000-000000000000}";
				}
				else
				{
					menuID = lastMenuID.Substring(16, 3);
					if (ExsitMenuID(menuID))
					{
						id = Int32.Parse(menuID) + 1;
						lastID = "{00000000-0000-0" + id + "-0000-000000000000}";
					}
					else
					{
						menuID = lastMenuID.Substring(17, 2);
						if (ExsitMenuID(menuID))
						{
							id = Int32.Parse(menuID) + 1;
							lastID = "{00000000-0000-00" + id + "-0000-000000000000}";
						}
						else
						{
							menuID = lastMenuID.Substring(18, 1);
							if (ExsitMenuID(menuID))
							{
								if (Int32.Parse(menuID) < 9)
								{
									id = Int32.Parse(menuID) + 1;
									lastID = "{00000000-0000-000" + id + "-0000-000000000000}";
								}
								else
								{
									lastID = "{00000000-0000-0010-0000-000000000000}";
								}
							}
						}
					}
				}
			}
			return lastID;
		}
		/// <summary>
		/// 取得第二段菜单ID
		/// </summary>
		/// <param name="lastMenuID">最后一段菜单ID</param>
		/// <returns></returns>
		public string GetSecondMenuID(string lastMenuID)
		{
			string lastID = "";
			string menuID = "";
			int id = 0;
			string firstID = lastMenuID.Substring(0, 20);
			if (lastMenuID != null)
			{
				menuID = lastMenuID.Substring(20, 4);
				if (ExsitMenuID(menuID))
				{
					id = Int32.Parse(menuID) + 1;
					lastID = firstID + id + "-000000000000}";
				}
				else
				{
					menuID = lastMenuID.Substring(21, 3);
					if (ExsitMenuID(menuID))
					{
						id = Int32.Parse(menuID) + 1;
						lastID = firstID + "0" + id + "-000000000000}";
					}
					else
					{
						menuID = lastMenuID.Substring(22, 2);
						if (ExsitMenuID(menuID))
						{
							id = Int32.Parse(menuID) + 1;
							lastID = firstID + "00" + id + "-000000000000}";
						}
						else
						{
							menuID = lastMenuID.Substring(23, 1);
							if (ExsitMenuID(menuID))
							{
								if (Int32.Parse(menuID) < 9)
								{
									id = Int32.Parse(menuID) + 1;
									lastID = firstID + "000" + id + "-000000000000}";
								}
								else
								{
									lastID = firstID + "0010-000000000000";
								}

							}
						}
					}
				}
			}
			return lastID;
		}
		/// <summary>
		/// 添加反馈菜单信息
		/// </summary>
		/// <param name="name"></param>
		/// <param name="informationType"></param>
		/// <param name="menuID"></param>
		public void AddAdivcieMenu(string name, string informationType, string menuID)
		{
			MenuItem childMenuItem1 = new MenuItem();
			MenuItem LastMenu = GetLastMenuItem(menuID);
			childMenuItem1.ID = GetSecondMenuID(LastMenu.ID);
			childMenuItem1.Created = DateTime.Now;
			string child1Name = name + "管理";
			childMenuItem1.Name = child1Name;
			childMenuItem1.Title = child1Name;
			childMenuItem1.ParentID = menuID;
			childMenuItem1.Updated = DateTime.Now;
			int index = GetLastIndex(menuID);
			childMenuItem1.Index = index + 1;
			//生成会员菜单：
			//childMenuItem1.Type = 99;
			//生成系统菜单
			childMenuItem1.Type = 1;
			childMenuItem1.Href = "/AddIns/Advices.aspx?adviceTypeID=" + informationType + "";
			Assistant.Insert(childMenuItem1);
		}
		/// <summary>
		/// AddMenu
		/// </summary>
		/// <param name="name"></param>
		/// <param name="url"></param>
		/// <param name="index"></param>
		/// <param name="nameText"></param>
		/// <param name="parentID"></param>
		public void AddMenu(string name, string url, int index, string nameText, string parentID)
		{
			MenuItem childMenuItem1 = new MenuItem();
			MenuItem LastmenuItem = GetLastMenuItem(parentID);
			if (LastmenuItem == null)
			{
				childMenuItem1.ID = parentID.Substring(0, 19) + "-0001-000000000000}";
			}
			else
			{
				childMenuItem1.ID = GetSecondMenuID(LastmenuItem.ID);
			}
			childMenuItem1.Created = DateTime.Now;
			childMenuItem1.Name = nameText;
			childMenuItem1.Title = name;
			childMenuItem1.ParentID = parentID;
			childMenuItem1.Updated = DateTime.Now;
			childMenuItem1.Index = index;
			//生成会员菜单：
			//childMenuItem1.Type = 99;
			//生成系统菜单
			childMenuItem1.Type = 1;
			childMenuItem1.Href = url;
			Assistant.Insert(childMenuItem1);
		}

		/// <summary>
		/// AddMenu
		/// </summary>
		/// <param name="firstName"></param>
		/// <param name="secondName"></param>
		/// <param name="url"></param>
		/// <param name="index"></param>
		/// <param name="nameText"></param>
		/// <param name="fileName"></param>
		/// <param name="group"></param>
		public void AddMenu(string firstName, string secondName, string url, int index, string nameText, string fileName, int group)
		{
			MenuItem menuItem = new MenuItem();
			MenuItem LastMenu = GetLastMenuItem("{00000000-0000-0000-0000-000000000000}");
			string menuIDs = GetMenuID(LastMenu.ID);
			if (menuIDs != "")
			{
				menuItem.ID = menuIDs;
			}
			else
			{
				menuItem.ID = We7Helper.CreateNewID();
			}
			menuItem.Created = DateTime.Now;
			menuItem.Name = firstName;
			menuItem.Title = firstName;
			menuItem.ParentID = We7Helper.EmptyGUID;
			menuItem.Updated = DateTime.Now;
			int indexs = GetLastIndex("{00000000-0000-0000-0000-000000000000}");
			menuItem.Index = indexs + 1;
			//生成会员菜单：
			//menuItem.Type = 99;
			//生成系统菜单
			menuItem.Type = 1;
			menuItem.Href = url;
			menuItem.Group = group;
			menuItem.Icon = fileName;
			Assistant.Insert(menuItem);

			MenuItem childMenuItem1 = new MenuItem();
			childMenuItem1.ID = menuItem.ID.Substring(0, 19) + "-0001-000000000000}";
			childMenuItem1.Created = DateTime.Now;
			childMenuItem1.Name = nameText;
			childMenuItem1.Title = secondName;
			childMenuItem1.ParentID = menuItem.ID;
			childMenuItem1.Updated = DateTime.Now;
			childMenuItem1.Index = index;
			//生成会员菜单：
			//menuItem.Type = 99;
			//生成系统菜单
			menuItem.Type = 1;
			childMenuItem1.Href = url;
			Assistant.Insert(childMenuItem1);
		}


		/// <summary>
		/// AddMenu
		/// </summary>
		/// <param name="name"></param>
		/// <param name="informationType"></param>
		/// <param name="type"></param>
		public void AddMenu(string name, string informationType, int type)
		{
			MenuItem menuItem = new MenuItem();
			MenuItem LastMenu = GetLastMenuItem("{00000000-0000-0000-0000-000000000000}");
			string menuIDs = GetMenuID(LastMenu.ID);
			if (menuIDs != "")
			{
				menuItem.ID = menuIDs;
			}
			else
			{
				menuItem.ID = We7Helper.CreateNewID();
			}
			menuItem.Created = DateTime.Now;
			menuItem.Name = name;
			menuItem.Title = name;
			menuItem.ParentID = We7Helper.EmptyGUID;
			menuItem.Updated = DateTime.Now;
			int index = GetLastIndex("{00000000-0000-0000-0000-000000000000}");
			menuItem.Index = index + 1;
			//生成会员菜单：
			//menuItem.Type = 99;
			//生成系统菜单
			menuItem.Type = 1;
			if (type == 0)
			{
				menuItem.Href = "/AddIns/Articlelist.aspx?type=" + informationType + "";
			}
			if (type == 1)
			{
				menuItem.Href = "/AddIns/Advices.aspx?adviceTypeID=" + informationType + "";
			}
			Assistant.Insert(menuItem);

			MenuItem childMenuItem1 = new MenuItem();
			childMenuItem1.ID = menuItem.ID.Substring(0, 19) + "-0001-000000000000}";
			childMenuItem1.Created = DateTime.Now;
			string child1Name = name + "管理";
			childMenuItem1.Name = child1Name;
			childMenuItem1.Title = child1Name;
			childMenuItem1.ParentID = menuItem.ID;
			childMenuItem1.Updated = DateTime.Now;
			childMenuItem1.Index = 1;
			//生成会员菜单：
			//menuItem.Type = 99;
			//生成系统菜单
			menuItem.Type = 1;
			if (type == 0)
			{
				childMenuItem1.Href = "/AddIns/Articlelist.aspx?type=" + informationType + "";
			}
			if (type == 1)
			{
				childMenuItem1.Href = "/AddIns/Advices.aspx?adviceTypeID=" + informationType + "";
			}
			Assistant.Insert(childMenuItem1);
			if (type == 0)
			{
				MenuItem childMenuItem2 = new MenuItem();
				childMenuItem2.ID = menuItem.ID.Substring(0, 19) + "-0002-000000000000}";
				childMenuItem2.Created = DateTime.Now;
				string child2Name = "发布" + name;
				childMenuItem2.Name = child2Name;
				childMenuItem2.Title = child2Name;
				childMenuItem2.ParentID = menuItem.ID;
				childMenuItem2.Updated = DateTime.Now;
				childMenuItem2.Index = 2;
				//生成会员菜单：
				//menuItem.Type = 99;
				//生成系统菜单
				menuItem.Type = 1;
				childMenuItem2.Href = "/addins/ArticleEdit.aspx?type=" + informationType + "";
				Assistant.Insert(childMenuItem2);
			}
		}
		#endregion

		#region 工具函数
		/// <summary>
		/// 汉字字符串转成拼音
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public string GetChineseSpell(string strText)
		{
			int len = strText.Length;
			string myStr = "";
			for (int i = 0; i < len; i++)
			{
				myStr += getSpell(strText.Substring(i, 1));
			}
			return myStr;
		}

		/// <summary>
		/// 单个汉字转拼音
		/// </summary>
		/// <param name="cnChar"></param>
		/// <returns></returns>
		public string getSpell(string cnChar)
		{
			byte[] arrCN = Encoding.Default.GetBytes(cnChar);
			if (arrCN.Length > 1)
			{
				int area = (short)arrCN[0];
				int pos = (short)arrCN[1];
				int code = (area << 8) + pos;
				int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
				for (int i = 0; i < 26; i++)
				{
					int max = 55290;
					if (i != 25) max = areacode[i + 1];
					if (areacode[i] <= code && code < max)
					{
						return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
					}
				}
				return "*";
			}
			else return cnChar;
		}

		string CoverToParentID(string MenuID)
		{
			string s = String.Format("{0}{1}", MenuID.Substring(0, 20), "0000-000000000000}");
			return s;
		}

		class DinoComparer : IComparer<MenuItem>
		{
			public int Compare(MenuItem x, MenuItem y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return 0;
					}
					else
					{
						return -1;
					}
				}
				else
				{
					if (y == null)
					{
						return 1;
					}
					else
					{
						int retval = x.Index.CompareTo(y.Index);

						if (retval != 0)
						{
							return retval;
						}
						else
						{
							return x.Index.CompareTo(y.Index);
						}
					}
				}
			}


		}

		/// <summary>
		/// 查找类
		/// </summary>
		class Finder
		{
			private MenuItem extItem;

			public Finder(MenuItem extItem)
			{
				this.extItem = extItem;
			}

			public bool FindItem(MenuItem item)
			{
				return (item.ID == extItem.ID);
			}
		}

		#endregion

		#region 自定义生成菜单及其维护

		/// <summary>
		/// 根据类型统计菜单数
		/// </summary>
		/// <param name="type">类型</param>
		/// <returns></returns>
		public int GetMenuCountByType(int type, string entityID)
		{
			int count = 0;
			foreach (MenuItem menu in AllMenuData)
			{
				if (menu.EntityID == entityID && (menu.Type == type || type == 100))
				{
					count++;
				}
			}
			return count;
		}

		/// <summary>
		/// 查询菜单列表
		/// </summary>
		/// <param name="type">菜单类型</param>
		/// <param name="searcherKey">搜索关键字</param>
		/// <returns></returns>
		public List<MenuItem> GetMenuList(int type, string searcherKey, string entityID)
		{
			List<MenuItem> myMenuList = new List<MenuItem>();
			if (We7Helper.IsEmptyID(AccountID))
			{
				foreach (MenuItem menu in AllMenuData)
				{
					if (menu.EntityID == entityID && (menu.Type == type || type == 100) && menu.Name.IndexOf(searcherKey) >= 0)
					{
						myMenuList.Add(menu);
					}
				}
			}
			else
			{
				List<string> myMenus = GetMenuIDListByAccount(AccountID, "System.User");
				//挨个判断是否具有权限
				foreach (MenuItem menu in AllMenuData)
				{
					if (menu.EntityID == entityID && myMenus.Contains(menu.ID) && (menu.Type == type || type == 100) && menu.Name.IndexOf(searcherKey) >= 0)
					{
						myMenuList.Add(menu);
					}
				}
			}
			return myMenuList;
		}

		/// <summary>
		/// 创建内容模型菜单
		/// </summary>
		/// <param name="mianNameText"></param>
		/// <param name="mainTitle"></param>
		/// <param name="maingroup"></param>
		/// <param name="mainIndex"></param>
		/// <param name="mainIconName"></param>
		/// <param name="firstNameText"></param>
		/// <param name="firstTitle"></param>
		/// <param name="firstUrl"></param>
		/// <param name="firstIndex"></param>
		/// <param name="secondNameText"></param>
		/// <param name="secondTitle"></param>
		/// <param name="secondUrl"></param>
		/// <param name="secondIndex"></param>
		public void CreateContentMenu(string mianNameText, string mainTitle, int maingroup, int mainIndex, string mainIconName, string firstNameText, string firstTitle, string firstUrl, int firstIndex, string secondNameText, string secondTitle, string secondUrl, int secondIndex, string entityID)
		{
			try
			{
				//创建主菜单
				string id = ExistMenuItem(mianNameText);
				string parentID = CreateMainMenu(mianNameText, mainTitle, maingroup, mainIndex, mainIconName, firstUrl, id, entityID);

				//创建子菜单一
				string firstID = ExistMenuItem(firstNameText);
				firstID = CreateSubMenu(firstNameText, firstTitle, firstUrl, firstIndex, parentID, firstID, entityID);

				//创建子菜单二
				string secondID = ExistMenuItem(secondNameText);
				secondID = CreateSubMenu(secondNameText, secondTitle, secondUrl, secondIndex, parentID, secondID, entityID);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 创建栏目菜单
		/// </summary>
		/// <param name="parentID"></param>
		/// <param name="menuIndex"></param>
		/// <param name="firstNameText"></param>
		/// <param name="firstTitle"></param>
		/// <param name="firstUrl"></param>
		/// <param name="firstIndex"></param>
		/// <param name="secondNameText"></param>
		/// <param name="secondTitle"></param>
		/// <param name="secondUrl"></param>
		/// <param name="secondIndex"></param>
		/// <param name="entityID"></param>
		public void CreateModelMenu(string parentID, int menuIndex, string firstNameText, string firstTitle, string firstUrl, int firstIndex, string secondNameText, string secondTitle, string secondUrl, int secondIndex, string entityID)
		{
			//创建子菜单一
			string firstID = ExistMenuItem(firstNameText);
			firstID = CreateSubMenu(firstNameText, firstTitle, firstUrl, firstIndex, parentID, firstID, entityID);

			//创建子菜单二
			string secondID = ExistMenuItem(secondNameText);
			secondID = CreateSubMenu(secondNameText, secondTitle, secondUrl, secondIndex, firstID, secondID, entityID);

		}

		/// <summary>
		/// 创建栏目菜单
		/// </summary>
		/// <param name="parentID"></param>
		/// <param name="menuIndex"></param>
		/// <param name="firstNameText"></param>
		/// <param name="firstTitle"></param>
		/// <param name="firstUrl"></param>
		/// <param name="firstIndex"></param>
		/// <param name="entityID"></param>
		/// <returns></returns>
		public string CreateModelMenu(string parentID, int menuIndex, string firstNameText, string firstTitle, string firstUrl, int firstIndex, string entityID)
		{
			//创建子菜单一
			string firstID = ExistMenuItem(firstNameText);
			return CreateSubMenu(firstNameText, firstTitle, firstUrl, firstIndex, parentID, firstID, entityID);
		}

		/// <summary>
		/// 创建或更新主菜单
		/// </summary>
		/// <param name="mianNameText"></param>
		/// <param name="mainTitle"></param>
		/// <param name="maingroup"></param>
		/// <param name="mainIndex"></param>
		/// <param name="mainIconName"></param>
		/// <param name="url"></param>
		/// <param name="id"></param>
		public string CreateMainMenu(string mianNameText, string mainTitle, int maingroup, int mainIndex, string mainIconName, string url, string id, string entityID)
		{
			Context.Application.Remove("$Menu_AllMenuData");
			MenuItem menuItem = new MenuItem();
			menuItem.Group = maingroup;
			menuItem.Href = url;
			menuItem.Index = mainIndex;
			menuItem.Name = mianNameText;
			menuItem.Title = mainTitle;
			menuItem.ParentID = We7Helper.EmptyGUID;
			menuItem.Updated = DateTime.Now;
			if (string.IsNullOrEmpty(id) || this.GetMenuItem(id) == null)
			{
				if (string.IsNullOrEmpty(id))
					menuItem.ID = We7Helper.CreateNewID();
				else menuItem.ID = id;
				menuItem.Icon = mainIconName;
				menuItem.Type = 99;
				menuItem.EntityID = entityID;
				menuItem.Created = DateTime.Now;
				Assistant.Insert(menuItem); //插入
			}
			else
			{
				string[] listString = new string[] { "Title", "Href", "Index", "Updated", "Group" };
				menuItem.ID = id;
				if (mainIconName != "")
				{
					menuItem.Icon = mainIconName;
					listString = new string[] { "Title", "Href", "Index", "Icon", "Updated", "Group" };
				}

				Assistant.Update(menuItem, listString);  //更新
			}
			return menuItem.ID;
		}

		/// <summary>
		/// 创建或更新会员主菜单
		/// </summary>
		/// <param name="mianNameText"></param>
		/// <param name="mainTitle"></param>
		/// <param name="maingroup"></param>
		/// <param name="mainIndex"></param>
		/// <param name="mainIconName"></param>
		/// <param name="url"></param>
		/// <param name="id"></param>
		public string CreateMainMenu_User(string mianNameText, string mainTitle, int maingroup, int mainIndex, string mainIconName, string url, string id, string entityID, int menuType, string referenceID)
		{
			Context.Application.Remove("$Menu_AllMenuData");
			MenuItem menuItem = new MenuItem();
			menuItem.Group = maingroup;
			menuItem.Href = url;
			menuItem.Index = mainIndex;
			menuItem.Name = mianNameText;
			menuItem.Title = mainTitle;
			menuItem.ParentID = We7Helper.EmptyGUID;
			menuItem.Updated = DateTime.Now;
			menuItem.MenuType = menuType;
			menuItem.ReferenceID = referenceID;
			if (id != "")
			{
				string[] listString = new string[] { "Title", "Href", "Index", "Updated", "Group" };
				menuItem.ID = id;
				if (mainIconName != "")
				{
					menuItem.Icon = mainIconName;
					listString = new string[] { "Title", "Href", "Index", "Icon", "Updated", "Group" };
				}

				Assistant.Update(menuItem, listString);  //更新
			}
			else
			{
				menuItem.ID = We7Helper.CreateNewID();
				menuItem.Icon = mainIconName;
				menuItem.Type = 99;
				menuItem.EntityID = entityID;
				menuItem.Created = DateTime.Now;
				Assistant.Insert(menuItem); //插入
			}
			return menuItem.ID;
		}

		/// <summary>
		/// 更新或创建子菜单
		/// </summary>
		/// <param name="nameText"></param>
		/// <param name="title"></param>
		/// <param name="url"></param>
		/// <param name="index"></param>
		/// <param name="parentID"></param>
		/// <param name="id"></param>
		/// <param name="entityID"></param>
		/// <returns></returns>
		public string CreateSubMenu(string nameText, string title, string url, int index, string parentID, string id, string entityID)
		{
			Context.Application.Remove("$Menu_AllMenuData");
			MenuItem menuItem = new MenuItem();
			menuItem.Href = url; ;
			menuItem.Index = index;
			menuItem.Name = nameText;
			menuItem.Title = title;

			menuItem.Updated = DateTime.Now;
			menuItem.ParentID = parentID;
			if (string.IsNullOrEmpty(id) || this.GetMenuItem(id) == null)
			{
				if (string.IsNullOrEmpty(id))
					menuItem.ID = We7Helper.CreateNewID();
				else menuItem.ID = id;
				menuItem.Created = DateTime.Now;
				menuItem.Updated = DateTime.Now;
				menuItem.Type = 99;
				menuItem.EntityID = entityID;

				Assistant.Insert(menuItem);  //插入
			}
			else
			{
				menuItem.ID = id;
				string[] listString = new string[] { "Title", "Href", "Index", "Updated", "ParentID" };
				Assistant.Update(menuItem, listString); //更新
			}

			return menuItem.ID;
		}

		/// <summary>
		/// 更新或创建会员子菜单
		/// </summary>
		/// <param name="nameText"></param>
		/// <param name="title"></param>
		/// <param name="url"></param>
		/// <param name="index"></param>
		/// <param name="parentID"></param>
		/// <param name="id"></param>
		/// <param name="entityID"></param>
		/// <param name="menuType"></param>
		/// <param name="referenceID"></param>
		/// <returns></returns>
		public string CreateSubMenu_User(string nameText, string title, string url, int index, string parentID, string id, string entityID, int menuType, string referenceID)
		{
			Context.Application.Remove("$Menu_AllMenuData");
			MenuItem menuItem = new MenuItem();
			menuItem.Href = url; ;
			menuItem.Index = index;
			menuItem.Name = nameText;
			menuItem.Title = title;

			menuItem.Updated = DateTime.Now;
			menuItem.ParentID = parentID;
			menuItem.MenuType = menuType;
			menuItem.ReferenceID = referenceID;
			if (string.IsNullOrEmpty(id) || this.GetMenuItem(id) == null)
			{
				menuItem.ID = string.IsNullOrEmpty(id) ? We7Helper.CreateNewID() : id;
				menuItem.Created = DateTime.Now;
				menuItem.Updated = DateTime.Now;
				menuItem.Type = 99;
				menuItem.EntityID = entityID;

				Assistant.Insert(menuItem);  //插入
			}
			else
			{
				menuItem.ID = id;
				string[] listString = new string[] { "Title", "Href", "Index", "Updated", "ParentID" };
				Assistant.Update(menuItem, listString); //更新
			}

			return menuItem.ID;
		}


		/// <summary>
		/// 当前菜单是否存在
		/// </summary>
		/// <param name="menuName">菜单名称</param>
		/// <returns></returns>
		public string ExistMenuItem(string menuName)
		{
			Criteria c = new Criteria(CriteriaType.Equals, "Name", menuName);
			List<MenuItem> MenuList = Assistant.List<MenuItem>(c, null);
			if (MenuList != null && MenuList.Count > 0)
			{
				return MenuList[0].ID;
			}
			else
			{
				return "";
			}
		}
		/// <summary>
		/// 当前菜单是否存在
		/// </summary>
		/// <param name="menuName">菜单名称</param>
		/// <returns></returns>
		public string ExistMenuItemByType(string menuName, string type)
		{
			Criteria c = new Criteria(CriteriaType.Equals, "Name", menuName);
			c.Add(CriteriaType.Equals, "EntityID", type);
			List<MenuItem> MenuList = Assistant.List<MenuItem>(c, null);
			if (MenuList != null && MenuList.Count > 0)
			{
				return MenuList[0].ID;
			}
			else
			{
				return "";
			}
		}


		/// <summary>
		/// 取得菜单信息
		/// </summary>
		/// <param name="id">菜单ID</param>
		/// <returns></returns>
		public MenuItem GetMenuItemByID(string id)
		{
			Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
			List<MenuItem> MenuList = Assistant.List<MenuItem>(c, null);
			if (MenuList != null && MenuList.Count > 0)
			{
				return MenuList[0];
			}
			else
			{
				return null;
			}
		}

		#endregion

		#region Menu URL 权限

		/// <summary>
		/// 判断某一菜单ID是否存在于菜单树中
		/// </summary>
		/// <param name="menuID"></param>
		/// <param name="menus">菜单树列表</param>
		/// <returns></returns>
		bool MenuIDInList(string menuID, List<We7.CMS.Common.MenuItem> menus)
		{
			foreach (MenuItem menu in menus)
			{
				if (menu.ID == menuID)
					return true;
				if (menu.Items.Count > 0)
				{
					bool ret = MenuIDInList(menuID, menu.Items);
					if (ret) return true;
				}
			}
			return false;
		}

		string ConvertToEntityID(OwnerRank rank)
		{
			string entityID = "";
			switch (rank)
			{
				case OwnerRank.Admin:
					entityID = "System.Administration";
					break;
				case OwnerRank.Normal:
					entityID = "System.User";
					break;
				case OwnerRank.Group:
					entityID = "Group.Administration";
					break;
				default:
					break;
			}
			return entityID;

		}

		/// <summary>
		/// 判断当前URL是否在可访问列表中;使用HttpContext缓存
		/// </summary>
		/// <param name="httpContext"></param>
		/// <returns></returns>
		public bool URLHavePermission(HttpContext httpContext, OwnerRank ownerRank)
		{
			bool ret = false;
			string key = "$URLPermission";
			if (httpContext.Items[key] == null)
			{
				List<MenuItem> mylist = GetMyMenuList(ConvertToEntityID(ownerRank));
				foreach (MenuItem m in mylist)
				{
					if (m.Href != null && m.Href != "" && httpContext.Request.FilePath.ToLower() == m.PageLocation.Trim().ToLower() &&
							We7Helper.UrlContainKeys(httpContext, m.QueryKey))
					{
						ret = true;
						break;
					}
				}
				httpContext.Items[key] = ret;
			}
			else
				ret = (bool)httpContext.Items[key];

			return ret;
		}

		/// <summary>
		/// 从menu表中获取权限定义项目，如Channel.Article
		/// </summary>
		/// <param name="EntityID"></param>
		/// <returns></returns>
		public List<MenuItem> GetEntityPermissions(string EntityID)
		{
			List<MenuItem> pss = new List<MenuItem>();
			foreach (MenuItem m in AllMenuData)
			{
				if (m.EntityID == EntityID)
					pss.Add(m);
			}
			return pss;
		}

		#endregion

	}

}
