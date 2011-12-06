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
namespace We7.CMS.Web.Admin
{
    public partial class AddNewMenu : BasePage
    {

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
                    if (Request["type"] != null && Request["type"].ToString() == "1")
                        return "System.User";
                    else
                        return "System.Administration";
                }
            }
        }

        public string MenuText
        {
            get
            {
                if (CurrentMenu != null)
                {
                    if (CurrentMenu.ParentID == We7Helper.EmptyGUID)
                    {
                        return "修改" + CurrentMenu.Name + "系统菜单";
                    }
                    else
                    {
                        return "修改" + CurrentMenu.Name + "用户菜单";
                    }
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
                    if (CurrentMenu.ParentID == We7Helper.EmptyGUID)
                    {
                        return "设置" + CurrentMenu.Name + "个性化属性";
                    }
                    else
                    {
                        return "设置" + CurrentMenu.Name + "个性化属性";
                    }
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
                BindMainIndex();
                BindChildIndex();
                if (MenuID != null && MenuID != "")
                {
                    BindingDate();
                }
                if (EntityID == "System.User")
                    ReturnHyperLink.NavigateUrl = "Menulist.aspx?type=1";
                else
                    ReturnHyperLink.NavigateUrl = "Menulist.aspx";
            }
        }

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
                    if (IconFileUpload.HasFile && HoverIconFileUpload.HasFile)
                    {
                        mainIconName = GetIconFileName();
                    }
                    id = MenuID;
                }
                else
                {
                    mainIconName = GetIconFileName();
                }
                string mainTitle = MainDesTextBox.Text.Trim();
                string mianNameText = MainTitleTextBox.Text.Trim();
                string mianUrl = MianUrlTextBox.Text.Trim();
                int maingroup = Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[0]);
                int mainIndex = 0;
                if (Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[1]) > 0)
                {
                    mainIndex = Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[1]) - 1;
                }

                MenuHelper.CreateMainMenu(mianNameText, mainTitle, maingroup, mainIndex, mainIconName, mianUrl, id, EntityID);
                if (MenuID != null && MenuID != "")
                {
                    Messages.ShowMessage("您成功修改" + mainTitle + "菜单,更新成功之后请退出重新登陆才能生效");
                }
                else
                {
                    string url = We7Helper.AddParamToUrl(ReturnHyperLink.NavigateUrl, "reload", "menu");
                    url = We7Helper.AddParamToUrl(url, "add", mainTitle);
                    HttpContext.Current.Session.Clear();
                    Response.Redirect(url);
                }
            }
            catch (Exception ex)
            {
                Messages.ShowMessage(ex.Message);
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
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
            string selectValue=SecondIndexDropDownList.SelectedValue;
            int index = int.Parse(selectValue.Substring(selectValue.LastIndexOf(",")+1));
            int firstIndex = index > 0 ? index : 0;

            int rootID = 0;
            string parentID = selectValue.Split(',')[0];
            if (parentID == We7Helper.EmptyGUID)
            {
                Messages.ShowError("您编辑的是子菜单，不能选择主菜单的位置，请您选择子菜单。");
            }
            else
            {
                string itemId=MenuHelper.CreateSubMenu(firstNameText, firstTitle, firstUrl, firstIndex, parentID, id,EntityID);
                MenuItemXmlHelper helper = new MenuItemXmlHelper(Server.MapPath("/user/Resource/menuItems.xml"));
                if (MenuID != null && MenuID != "")
                {
                    if (EntityID == "System.User")
                    {
                        string xPath="/root/items/item[@oldid='" + id + "']";
                        MenuItemXml node = helper.GetMenuItemXml(xPath);
                        node.Lable = firstTitle;
                        node.Url = firstUrl;
                        node.Oldparent = parentID;
                        node.Parent = helper.GetMenuItemXml("/root/items/item[@oldid='" + parentID + "']").ID;
                        helper.UpdateMenuItemXml(node, xPath);
                    }
                    Messages.ShowMessage("您成功修改" + firstTitle + "菜单,更新成功之后请退出重新登陆才能生效");
                }
                else
                {
                    string xPath = "/root/items/item[@id='" + parentID + "']";
                    string oldParent = helper.GetMenuItemXml(xPath).Oldid;
                    string oldId = MenuHelper.CreateModelMenu(oldParent, firstIndex, firstNameText, firstTitle, firstUrl, firstIndex, EntityID);

                    MenuItemXml item1 = new MenuItemXml();
                    item1.ID = firstIndex.ToString();
                    item1.Lable = firstNameText;
                    item1.MatchParameter = "true";
                    item1.Name = "";
                    item1.NodeName = "item";
                    item1.Oldid = oldId;
                    item1.Oldparent = oldParent;
                    item1.Parent = parentID;
                    item1.Url = firstUrl;
                    helper.AddMenuItemXmls(item1, "/root/items");

                    MenuItemXml item2 = new MenuItemXml();
                    item2.ID = firstIndex.ToString();
                    item2.Lable = firstNameText;
                    item2.NodeName = "menu";
                    item2.Link = "";
                    xPath = "/root/menuTree/menu[@id='" + rootID + "']/menu[@id='" + parentID + "']";

                    helper.AddMenuItemDisplay(item2, xPath);

                    Messages.ShowMessage("您成功创建" + firstTitle + "菜单,生成成功之后请退出重新登陆才能生效");
                }
            }
        }
        void BindingDate()
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
                        string index = item.Value.Substring(item.Value.LastIndexOf(",") + 1);
                        if (index == CurrentMenu.Index.ToString())
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

        void BindMainIndex()
        {
            List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID, 0, EntityID);
            int i = 0;
            int group = 1;
            foreach (We7.CMS.Common.MenuItem menuItem in menus)
            {
                string name = menuItem.Title.ToString();
                string value = menuItem.Group.ToString() + "," + menuItem.Index.ToString();
                ListItem item = new ListItem(name, value);
                i = menuItem.Index;
                group = menuItem.Group;
                DropDownListType.Items.Add(item);
            }

            ListItem appendItem = new ListItem("（追加到这里）", group.ToString() + "," + (i + 2).ToString());
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

        void BindChildIndex()
        {
            List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID,2,EntityID);

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
                    int j = 0;
                    foreach (We7.CMS.Common.MenuItem thirdmenu in submenu.Items)
                    {
                        string tname = "├──├─" + thirdmenu.Title.ToString();
                        string tvalue =submenu.ParentID + ","+ thirdmenu.ParentID + "," + thirdmenu.Index.ToString();
                        ListItem thitem = new ListItem(tname, tvalue);
                        j = thirdmenu.Index;
                        SecondIndexDropDownList.Items.Add(thitem);
                    }
                    ListItem appendItem3 = new ListItem("├──├─（追加到这里-三级菜单）", submenu.ID + "," + (j + 2).ToString());
                    SecondIndexDropDownList.Items.Add(appendItem3);
                }
                ListItem appendItem = new ListItem("├──（追加到这里）", menuItem.ID + "," + (i + 2).ToString());
                SecondIndexDropDownList.Items.Add(appendItem);
            }

            string myname = "流量统计";
            if (MenuID != null && MenuID != "")
            {
                myname = CurrentMenu.Title;
            }

            SecondIndexDropDownList.SelectedIndex = -1;
            ListItem currentItem = SecondIndexDropDownList.Items.FindByText("├──" + myname);
            if(currentItem==null)
                currentItem = SecondIndexDropDownList.Items.FindByText("├──├─" + myname);
            if (currentItem != null)
            {
                SecondIndexDropDownList.SelectedIndex = -1;
                currentItem.Selected = true;
            }

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
    }
}
