using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Framework;
using We7.CMS.Accounts;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 会员菜单数据提供者
    /// </summary>
    public class UserMenuProvider : BaseWebControl
    {
        /// <summary>
        /// 菜单助手
        /// </summary>
        protected MenuHelper MenuHelper
        {
            get { return HelperFactory.GetHelper<MenuHelper>(); }
        }

        string cssClass;
        /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        int maxTreeClass = 2;
        /// <summary>
        /// 最大菜单层数
        /// </summary>
        public int MaxTreeClass
        {
            get { return maxTreeClass; }
            set { maxTreeClass = value; }
        }
        /// <summary>
        /// 分级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<MenuItem> Menus { get; set; }

        /// <summary>
        /// 当前菜单ID
        /// </summary>
        public string ActiveMenuID
        {
            get
            {
                if (Level == 1)
                    return CurrentMenuID;
                else
                    return CurrentSubMenuID;
            }
        }

        /// <summary>
        /// 当前一级菜单
        /// </summary>
        public MenuItem ActiveMenu
        {
            get
            {
                List<MenuItem> menus = GetMenuTree();
                for (int i = 0; i < menus.Count; i++)
                {
                    for (int j = 0; j < menus[i].Items.Count; j++)
                    {
                        if (menus[i].Items[j].ID == CurrentSubMenuID)
                        {
                            return menus[i];
                        }
                    }
                }
                return new MenuItem();
            }
        }


        /// <summary>
        /// 当前一级菜单项
        /// </summary>
        protected string CurrentMenuID
        {
            get
            {
                return ActiveMenu.ID;
            }
        }

        private MenuItem activeSubMenu;

        /// <summary>
        /// 当前二级菜单
        /// </summary>
        public MenuItem ActiveSubMenu
        {
            get
            {
                if (activeSubMenu == null)
                {
                    List<MenuItem> menuUrls = new List<MenuItem>();
                    if (HttpContext.Current.Session["ALLUERMENUURL"] != null)
                        menuUrls = (List<MenuItem>)HttpContext.Current.Session["ALLUERMENUURL"];
                    else
                    {
                        menuUrls = MenuHelper.GetMyMenuList(OwnerRank.Normal);
                        HttpContext.Current.Session["ALLUERMENUURL"] = menuUrls;
                    }

                    foreach (MenuItem mu in menuUrls)
                    {   //&& !We7Helper.IsEmptyID(mu.ParentID)
                        if (mu.PageLocation != null && mu.PageLocation != "" &&
                            HttpContext.Current.Request.FilePath.ToLower() == mu.PageLocation.Trim().ToLower() &&
                            We7Helper.UrlContainKeys(HttpContext.Current, mu.QueryKey))
                        {
                            activeSubMenu = mu;
                            break;
                        }
                    }
                }
                return activeSubMenu;
            }
        }
        /// <summary>
        /// 当前子菜单项ID
        /// </summary>
        protected string CurrentSubMenuID
        {
            get
            {
                return ActiveSubMenu.ID;
            }
        }

        /// <summary>
        /// 菜单树的所有数据，存储在session中
        /// </summary>
        protected List<MenuItem> GetMenuTree()
        {
            List<MenuItem> menus = new List<MenuItem>();
            if (HttpContext.Current.Session["ALLUSERMEMUITEMS"] != null)
                menus = (List<MenuItem>)HttpContext.Current.Session["ALLUSERMEMUITEMS"];
            else
            {
                menus = MenuHelper.GetMenuTree(We7Helper.EmptyGUID, OwnerRank.Normal);
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session["ALLUSERMEMUITEMS"] = menus;
                }
            }
            return menus;
        }

        /// <summary>
        /// 根据用户菜单类型返回数据
        /// </summary>
        /// <param name="menuType"></param>
        /// <returns></returns>
        protected List<MenuItem> GetMenuTreeByType(int menuType)
        {
            //用户菜单
            List<MenuItem> allMenus = GetMenuTree();
            List<MenuItem> result = new List<MenuItem>();
            foreach (MenuItem item in allMenus)
            {
                if (item.MenuType == menuType)
                {
                    result.Add(item);
                }
            }
            return result;
        }


        /// <summary>
        /// 获取会员子菜单列表
        /// </summary>
        /// <param name="myMenus">菜单树</param>
        /// <param name="parentID">菜单ID</param>
        /// <returns></returns>
        protected List<MenuItem> GetSubMenuList(List<MenuItem> myMenus, string parentID)
        {
            foreach (MenuItem i in myMenus)
            {
                if (i.ID == parentID)
                {                    
                    return i.Items;
                }
            }
            return null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Level == 1)
                Menus = GetMenuTree();
            else
                Menus = GetSubMenuList(GetMenuTree(), CurrentMenuID);
        }

        public bool ExcludeID(string[] list, string id)
        {
            foreach (string s in list)
            {
                if (s == id)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 根菜单
        /// </summary>
        protected string RootID
        {
            get
            {
                string levelOneID = ActiveSubMenu.ParentID;
                if (levelOneID == We7Helper.EmptyGUID)
                {
                    return ActiveSubMenu.ID;//一级顶部菜单的root
                }
                string levelTwoID = MenuHelper.GetMenuItem(levelOneID).ParentID;
                if (levelTwoID == We7Helper.EmptyGUID)
                {
                    return levelOneID;//二级分组菜单的root
                }
                string levelTreeID = MenuHelper.GetMenuItem(levelTwoID).ParentID;
                if (levelTreeID == We7Helper.EmptyGUID)
                {
                    return levelTwoID;//三级普通菜单的root
                }
                string levelFourID = MenuHelper.GetMenuItem(levelTreeID).ParentID;
                if (levelFourID == We7Helper.EmptyGUID)
                {
                    return levelTreeID;
                }
                return "";
            }
        }
    }
}
