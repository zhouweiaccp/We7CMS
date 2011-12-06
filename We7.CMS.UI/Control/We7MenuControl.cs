using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Accounts;

namespace We7.CMS.Controls
{
    /// <summary>
    /// 后台用新菜单控件（后台可折叠菜单，类似WP）
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:We7MenuControl runat=server></{0}:We7MenuControl>")]
    public class We7MenuControl : WebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        /// <summary>
        /// 菜单类别+
        /// 后台菜单-“System.Administration”
        /// 会员菜单-“System.User”
        /// 站群菜单- “Group.Administration”
        /// </summary>
        public string EntityID { get; set; }

        protected string AccountID
        {
            get
            {
                return Security.CurrentAccountID;
            }
        }

        protected MenuHelper MenuHelper
        {
            get
            {
                HelperFactory hf = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                return hf.GetHelper<MenuHelper>();
            }
        }


        /// <summary>
        /// 当前一级菜单项
        /// </summary>
        protected string CurrentMenuID
        {
            get
            {
                   List<We7.CMS.Common.MenuItem> menus = AllShowMemuItem;
                   for (int i = 0; i < menus.Count; i++)
                   {
                       for (int j = 0; j < menus[i].Items.Count; j++)
                       {
                           if (menus[i].Items[j].ID == CurrentSubMenuID)
                               return menus[i].ID;
                       }
                   }
                return "";
            }
        }
        /// <summary>
        /// 当前子菜单项
        /// </summary>
        protected string CurrentSubMenuID
        {
            get
            {
                List<We7.CMS.Common.MenuItem> menuUrls = new List<We7.CMS.Common.MenuItem>();
                if (HttpContext.Current.Session["ALLMENUURL"] != null)
                    menuUrls = (List<We7.CMS.Common.MenuItem>)HttpContext.Current.Session["ALLMENUURL"];
                else
                {
                    menuUrls = MenuHelper.GetMyMenuList(EntityID);
                    HttpContext.Current.Session["ALLMENUURL"] = menuUrls;
                }

                foreach (We7.CMS.Common.MenuItem mu in menuUrls)
                {
                    if (mu.PageLocation != null && mu.PageLocation != "" &&
                        HttpContext.Current.Request.FilePath.ToLower() == mu.PageLocation.Trim().ToLower() &&
                        We7Helper.UrlContainKeys(HttpContext.Current, mu.QueryKey))
                        return mu.ID;
                }
                return "";
            }
        }

        /// <summary>
        /// 菜单树的所有数据，存储在HttpContext.Current.Items中
        /// </summary>
        protected List<We7.CMS.Common.MenuItem> AllShowMemuItem
        {
            get
            {
                List<We7.CMS.Common.MenuItem> menus = new List<We7.CMS.Common.MenuItem>();
                if (HttpContext.Current.Items["ALLSHOWMEMUITEM"] != null)
                    menus = (List<We7.CMS.Common.MenuItem>)HttpContext.Current.Items["ALLSHOWMEMUITEM"];
                else
                {
                    menus = MenuHelper.GetMenuTree(We7Helper.EmptyGUID,EntityID);
                    HttpContext.Current.Items["ALLSHOWMEMUITEM"] = menus;
                }

                return menus;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void Initialize()
        {
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.WriteLine("<ul id=\"adminmenu\">");
            RenderMenuItems(output);
            output.WriteLine(" </ul>");
            output.WriteLine(GenerateMenuJS(AllShowMemuItem));
        }

        void RenderMenuItems(HtmlTextWriter output)
        {
            output.WriteLine(CreateMenuString());
        }

        /// <summary>
        /// 返回菜单数据，包括js数据
        /// </summary>
        /// <returns></returns>
        public string AllMenuHtml()
        {
            string s = "<ul id=\"adminmenu\">";
            s += CreateMenuString();
            s += " </ul>";
            s += GenerateMenuJS(AllShowMemuItem);
            return s;
        }

        string CreateMenuString()
        {
            string allMenu = "";
            int group = 1;
            List<We7.CMS.Common.MenuItem> menus = AllShowMemuItem;
            for (int i = 0; i < menus.Count; i++)
            {
                if (menus[i] != null && menus[i].Name.Trim().Length > 0)
                {
                    string pos = "";
                    if (i == 0) pos = "first";//第一个
                    if (i + 1 >= menus.Count || menus[i + 1].Group == group + 1) pos += " last"; //最后一个菜单

                    if (menus[i].Group == group)
                        allMenu += GenerateMenuItem(menus[i], pos, i) + "\n";
                    else
                    {
                        allMenu += @"<li class=""we7-menu-separator""><br></li>" + "\n";
                        pos = "first";
                        allMenu += GenerateMenuItem(menus[i], pos, i) + "\n";
                        group += 1;
                    }
                }
            }

            return allMenu;
        }

        /// <summary>
        /// 生成一级菜单字符串
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        string GenerateMenuItem(We7.CMS.Common.MenuItem menu, string pos, int index)
        {
            string frmMenu = @"
            <li class=""{0}"" id=""{1}"" {6} >
	            <div class=""we7-menu-image""><br></div>
	            <div class=""we7-menu-toggle""><br></div>
	            <a  class=""{3}"" tabindex=""1"">{4}</a>
	            <div class=""we7-submenu"">
		            <div class=""we7-submenu-head"">{4}</div>
		            <ul>
                    {5}
                    </ul>
	             </div>
            </li>";
            string liClass = "menu-top";
            if (index == 0) liClass=AddCssClass(liClass, "we7-first-item");
            if (menu.Items.Count > 0) liClass=AddCssClass(liClass, "we7-has-submenu");
            if (pos.IndexOf("first") > -1) liClass=AddCssClass(liClass, "menu-top-first");
            if (pos.IndexOf("last") > -1) liClass=AddCssClass(liClass, "menu-top-last");
            string js = "";// @"onmousemove=""menuHover(this)"" onmouseout=""menuOut(this)"" ";
            if (menu.ID == CurrentMenuID)
            {
                liClass=AddCssClass(liClass, "we7-has-current-submenu");
                liClass=AddCssClass(liClass, "we7-menu-open");
                js = "";
            }

            //兼容admin目录
            //if (!menu.Href.ToLower().StartsWith("/admin/")) menu.Href = "/admin" + menu.Href;

            string aClass = liClass;
            string ret = string.Format(frmMenu, liClass, GetIDFromIcon(menu.Icon), menu.Href, aClass, menu.Title, GenerateSubMenuItem(menu.Items, GetIDFromIcon(menu.Icon)), js);
            return ret;
        }

        /// <summary>
        /// 生成子菜单
        /// </summary>
        /// <param name="menus">主菜单下子菜单列表</param>
        /// <returns></returns>
        string GenerateSubMenuItem(List<We7.CMS.Common.MenuItem> menus, string parentID)
        {
            string frmSubMenu = @"<li class=""{0}"" id=""{4}"">{5}. <a href=""javascript:menuClick('{1}','{4}');"" class=""{2}"" tabindex=""1""> {3}</a> {6}</li>";
            string ret = "";
            for (int i = 0; i < menus.Count; i++)
            {
                if (menus[i] != null && menus[i].Name.Trim().Length > 0)
                {
                    string cssClass = "";
                    if (i == 0) cssClass = AddCssClass(cssClass, "we7-first-item");
                    if (menus[i].ID == CurrentSubMenuID) cssClass = AddCssClass(cssClass, "current");

                    string thirdMenus = "";
                    if (menus[i].Items.Count > 0)
                    {
                        for (int j = 0; j < menus[i].Items.Count; j++)
                        {
                            thirdMenus += string.Format(" | <a href=\"javascript:menuClick('{0}','{1}');\">{2}</a>", menus[i].Items[j].Href, parentID + "_" + (i + 1).ToString(), menus[i].Items[j].Title);
                        }
                    }
                    ret += string.Format(frmSubMenu, cssClass, menus[i].Href, cssClass, menus[i].Title, parentID + "_" + (i + 1).ToString(), (i + 1).ToString(), thirdMenus) + "\n";
                }
            }
            return ret;
        }

        /// <summary>
        /// 生成菜单初始化JS
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        string GenerateMenuJS(List<We7.CMS.Common.MenuItem> menus)
        {
            string js = @" <script type=""text/javascript"">";

            //for (int i = 0; i < menus.Count; i++)
            //{
            //    if (menus[i] != null && menus[i].Icon!=null && menus[i].Icon.Trim().Length > 0)
            //    {
            //        if(menus[i].ID==CurrentMenuID)
            //            js += string.Format("menuInit(\"{0}\",true);", GetIDFromIcon(menus[i].Icon));
            //        else
            //            js += string.Format("menuInit(\"{0}\");", GetIDFromIcon(menus[i].Icon)) ;
            //    }
            //}
            js += "adminMenu.init();";
            js += "</script>";
            return js;
        }

        string AddCssClass(string classStrings,string newClass)
        {
            if (classStrings == string.Empty)
                return newClass;
            else
                return classStrings + " " + newClass;
        }

        string GetIDFromIcon(string icon)
        {
            if (icon == null || icon == string.Empty)
                return string.Empty;
            else
                return icon.Substring(0, icon.IndexOf('.'));
        }

    }
}
