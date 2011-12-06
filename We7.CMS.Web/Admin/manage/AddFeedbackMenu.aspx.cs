using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class AddFeedbackMenu : BasePage
    {
        protected AdviceTypeHelper AdviceTypeHelper
        {
            get { return HelperFactory.GetHelper<AdviceTypeHelper>(); }
        }
        string MenuTypeID
        {
            get
            {
                if (Request["id"] != null && Request["id"] != "")
                {
                    return Request["id"];
                }
                else if (Request["menuTypeID"] != null && Request["menuTypeID"] != "")
                {
                    return Request["menuTypeID"];
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
                if (Request["type"] != null && Request["type"].ToString() == "1")
                    return "System.User";
                else
                    return "System.Administration";
            }
        }
        string menuName
        {
            get
            {
                return Request["menuName"];
            }
        }
        public string MenuVisble
        {
            get
            {
                if (MenuTypeID != null && MenuTypeID != "")
                    return "none";
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 返回链接
        /// </summary>
        protected string returnURL
        {
            get
            {
                if (string.IsNullOrEmpty(Request["returnURL"]))
                    return null;
                return Request["returnURL"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindingChildIndex();
                BindingData();
                if (EntityID == "System.User")
                {
                    ReturnHyperLink.NavigateUrl = "Menulist.aspx?type=1";
                }
                else
                {
                    ReturnHyperLink.NavigateUrl = "Menulist.aspx";
                }
            }
        }
        void BindingTypeData()
        {
            List<AdviceType> list = AdviceTypeHelper.GetAdviceTypes();
            if (list == null)
            {
                return;
            }
            foreach (AdviceType adviceType in list)
            {

                string name = adviceType.Title;
                string value =adviceType.ID;
                ListItem item = new ListItem(name, value);
                MenuDropDownList.Items.Add(item);

            }
        }
        void BindingChildIndex()
        {
            List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID, 1, EntityID);

            //List<MenuItem> GetMainMenu = MenuHelper.GetMenuListByParentID(Helper.EmptyGUID);
            foreach (We7.CMS.Common.MenuItem menuItem in menus)
            {
                string name = "├" + menuItem.Title.ToString();
                string value = menuItem.ParentID + "," + menuItem.Index.ToString();
                ListItem item = new ListItem(name, value);
                SecondIndexDropDownList.Items.Add(item);
                foreach (We7.CMS.Common.MenuItem submenu in menuItem.Items)
                {
                    string sname = "├──" + submenu.Title.ToString();
                    string svalue = submenu.ParentID + "," + submenu.Index.ToString();
                    ListItem sitem = new ListItem(sname, svalue);
                    SecondIndexDropDownList.Items.Add(sitem);
                }
            }

            if (SecondIndexDropDownList.Items.Count == 0)
            {
                Messages.ShowMessage("无子菜单");
                return;
            }

            ListItem currentItem = SecondIndexDropDownList.Items.FindByText("├──评论");
            if (currentItem != null)
                currentItem.Selected = true;
        }

        void BindingData()
        {
            if (menuName != null && menuName != "")
            {
                TitleTextBox.Text = menuName;
                DesTextBox.Text = menuName;
                UrlTextBox.Text = "/admin/Advice/AdviceListEx.aspx?typeID=" + MenuTypeID + "";
            }
            else
            {
                BindingTypeData();
            }
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string nameText = TitleTextBox.Text.Trim();
            string title = DesTextBox.Text.Trim();
            string url = UrlTextBox.Text.Trim();
            int index = 0;
            if (Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]) > 0)
            {
                index = Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]) - 1;
            }
            string parentID = SecondIndexDropDownList.SelectedItem.Value.Split(',')[0];
            MenuHelper.CreateSubMenu(nameText, title, url, index, parentID, "", EntityID);

            string lisUrl = We7Helper.AddParamToUrl(ReturnHyperLink.NavigateUrl, "reload", "menu");
            lisUrl = We7Helper.AddParamToUrl(lisUrl, "add", title);
            if(returnURL!=null)
                lisUrl = We7Helper.AddParamToUrl(lisUrl, "returnURL", returnURL);
            HttpContext.Current.Session.Clear();
            Response.Redirect(lisUrl);
        }

        protected void InitButton_Click(object sender, EventArgs e)
        {
            string title = MenuDropDownList.SelectedItem.Text;
            if (MenuHelper.ExistMenuItem(title) != "")
            {
                Messages.ShowMessage(title + "该反馈模型已经存在，不能再创建");
                return;
            }
            string value = MenuDropDownList.SelectedItem.Value;
            TitleTextBox.Text = MenuDropDownList.SelectedItem.Text;
            DesTextBox.Text = MenuDropDownList.SelectedItem.Text;
            UrlTextBox.Text = "/admin/Advice/AdviceListEx.aspx?TypeID=" + value + "";
        }
    }
}
