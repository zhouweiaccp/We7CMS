using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using We7.Model.Core;
using We7.Framework.Util;
using We7.Framework;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
    public partial class AddMenuUser : BasePage
    {

        string EntityID
        {
            get
            {
                return "System.User";

            }
        }

        string ModelName
        {
            get
            {
                return Request["modelname"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ModelName != null)
                {
                    ContentSelectTable.Visible = false;
                    ModelInfo model = ModelHelper.GetModelInfoByName(ModelName);
                    InitMenuData(model.Label, ModelName);
                    ListItem item = new ListItem(model.Label, ModelName);
                    MenuDropDownList.Items.Add(item);
                }
                else
                {
                    BindingData();
                }

                BindChildIndex();
                ReturnHyperLink.NavigateUrl = "UserMenulist.aspx?type=1";

            }
        }
        void BindingData()
        {
            ContentModelCollection cmc = ModelHelper.GetContentModel(ModelType.ARTICLE);
            MenuDropDownList.DataSource = cmc;
            MenuDropDownList.DataTextField = "Label";
            MenuDropDownList.DataValueField = "Name";
            MenuDropDownList.DataBind();
        }



        void BindChildIndex()
        {
            string filePath = Server.MapPath("/user/Resource/menuItems.xml");
            MenuItemXmlHelper menuHelper = new MenuItemXmlHelper(filePath);

            string xPath = "/root/menuTree";
            List<We7.CMS.Common.MenuItemXml> lsItemsLevel1 = menuHelper.GetMenuItemXmls(xPath);

            int count = menuHelper.GetMenuItemXmls("/root/items").Count;

            foreach (We7.CMS.Common.MenuItemXml menuItem in lsItemsLevel1)
            {
                string name = "├" + menuItem.Lable.ToString();
                string value = menuItem.ID + "," + menuItem.ID;
                ListItem item = new ListItem(name, value);
                SecondIndexDropDownList.Items.Add(item);
                int i = 0;
                List<MenuItemXml> lsItemsLevel2 = menuHelper.GetMenuItemXmls(xPath + "/menu[@id='" + menuItem.ID + "']");

                foreach (We7.CMS.Common.MenuItemXml submenu in lsItemsLevel2)
                {
                    string sname = "├──" + submenu.Lable.ToString();
                    string svalue = menuItem.ID + "," + submenu.ID.ToString();
                    ListItem sitem = new ListItem(sname, svalue);
                    i = Convert.ToInt32(submenu.ID);
                    SecondIndexDropDownList.Items.Add(sitem);

                    List<We7.CMS.Common.MenuItemXml> lsItemsLevel3 = menuHelper.GetMenuItemXmls(xPath + "/menu[@id='" + menuItem.ID + "']" + "/menu[@id='" + submenu.ID + "']");
                    foreach (We7.CMS.Common.MenuItemXml subSubMenu in lsItemsLevel3)
                    {
                        string ssname = "├────" + subSubMenu.Lable.ToString();
                        string ssvalue = menuItem.ID + "," + submenu.ID + "," + subSubMenu.ID.ToString();
                        ListItem ssitem = new ListItem(ssname, ssvalue);
                        i = Convert.ToInt32(subSubMenu.ID);
                        SecondIndexDropDownList.Items.Add(ssitem);
                    }
                    int result = Convert.ToInt32(menuHelper.GetMenuItemXmls("/root/items")[count - 1].ID) + 1;
                    ListItem appendItem = new ListItem("├────（追加到这里）", menuItem.ID + "," + submenu.ID + "," + result);
                    SecondIndexDropDownList.Items.Add(appendItem);
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("/user/Resource/menuItems.xml");
            MenuItemXmlHelper menuXmlHelper = new MenuItemXmlHelper(filePath);
            try
            {
                int count = menuXmlHelper.GetMenuItemXmls("/root/items").Count;

                int firstIndex = Convert.ToInt32(menuXmlHelper.GetMenuItemXmls("/root/items")[count - 1].ID) + 1;

                if (SecondIndexDropDownList.SelectedValue.Split(new char[] { ',' }).Length < 3)
                {
                    Messages.ShowError("请选择（追加到这里）选项！");
                    return;
                }
                string parentID = SecondIndexDropDownList.SelectedItem.Value.Split(',')[1];
                string rootID = SecondIndexDropDownList.SelectedItem.Value.Split(',')[0];

                string firstTitle = DesTextBox.Text.Trim();
                string firstNameText = TitleTextBox.Text.Trim();
                string firstUrl = UrlTextBox.Text.Trim();



                /*
                 * parentID：
                 * 存入数据时 应为GUID，并且与该节点的oldParent保持一致
                 * 存入XML时 为int类型
                 * 勾立国 2011-7-13
                 */
                string xPath = "/root/items/item[@id='" + parentID + "']";
                string oldParent = menuXmlHelper.GetMenuItemXml(xPath).Oldid;
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
                item1.Group = "1";//自定义菜单
                item1.Type = 0;//显示
                menuXmlHelper.AddMenuItemXmls(item1, "/root/items");

                MenuItemXml item2 = new MenuItemXml();
                item2.ID = firstIndex.ToString();
                item2.Lable = firstNameText;
                item2.NodeName = "menu";
                item2.Link = "";
                xPath = "/root/menuTree/menu[@id='" + rootID + "']/menu[@id='" + parentID + "']";

                menuXmlHelper.AddMenuItemDisplay(item2, xPath);
                Messages.ShowMessage("保存成功！");

                //string url = We7Helper.AddParamToUrl(ReturnHyperLink.NavigateUrl, "reload", "menu");
                //url = We7Helper.AddParamToUrl(url, "add", firstTitle + "（"+ secondTitle + "）");
                //HttpContext.Current.Session.Clear();
                //Response.Redirect(url);
            }
            catch (Exception ex)
            {
                Messages.ShowError("无法保存：" + ex.Message);
            }
        }
        protected void InitButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(MenuDropDownList.SelectedValue))
            {
                Messages.ShowMessage("请选择内容模型");
                return;
            }
            string title = MenuDropDownList.SelectedItem.Text;
            string value = MenuDropDownList.SelectedItem.Value;
            InitMenuData(title, value);
        }

        void InitMenuData(string title, string value)
        {
            //if (MenuHelper.ExistMenuItemByType(title, "System.User") != "" || MenuHelper.ExistMenuItemByType(title + "管理", "System.User") != "")
            //{
            //    Messages.ShowMessage(title + "内容模型已经存在，不能再创建");
            //    return;
            //}
            //MainTitleTextBox.Text = title;
            //MainDesTextBox.Text = title;
            //DisplayTextBox.Text = title;
            TitleTextBox.Text = title + "管理";
            DesTextBox.Text = title + "管理";
            UrlTextBox.Text = "/User/ModelHandler.aspx?model=" + value + "";
            //IndexTextBox.Text = "1";



        }
    }
}
