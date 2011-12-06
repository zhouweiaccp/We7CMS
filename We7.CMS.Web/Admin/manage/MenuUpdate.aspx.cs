using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using We7.CMS.Accounts;

namespace We7.CMS.Web.Admin
{
    public partial class MenuUpdate : BasePage
    {
        protected MenuHelper MenuHelper
        {
            get { return HelperFactory.GetHelper<MenuHelper>(); }
        }

        string MenuID
        {
            get
            {
                return Request["id"];
            }
        }
        public string OneButtonSelect
        {
            get
            {
                if (MenuID != null && MenuID == "{00000000-0000-0000-0000-000000000000}")
                {
                    return "";
                }
                else
                {
                    return "none";

                }
            }
        }
        public string TwoButtonSelect
        {
            get
            {
                if (MenuID != null && MenuID == "{00000000-0000-0000-0000-000000000000}")
                {
                    return "";
                }
                else
                {
                    return "none";

                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindingData();
            }
        }
        void BindingData()
        {
            We7.CMS.Common.MenuItem menuItem = new We7.CMS.Common.MenuItem();
            if (MenuID != null)
            {
                menuItem = MenuHelper.GetMenuItem(MenuID);
                TitleTextBox.Text = menuItem.Title;
                DesTextBox.Text = menuItem.Name;
                IndexTextBox.Text = menuItem.Index.ToString();
                UrlTextBox.Text = menuItem.Href;
                CreateDateLabel.Text = menuItem.Created.ToString("yyyy-MM-dd");
                if (MenuID == "{00000000-0000-0000-0000-000000000000}")
                {
                    GroupTextBox.Text = menuItem.Group.ToString();
                }
            }
        }
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            We7.CMS.Common.MenuItem menuItem = new We7.CMS.Common.MenuItem();
            if (MenuID != null)
            {
                string menuName = TitleTextBox.Text.Trim();
                if (MenuHelper.ExistMenuItem(menuName, MenuID))
                {
                    Messages.ShowMessage("请您更换菜单名字");
                    return;
                }
                menuItem.ID = MenuID;
                menuItem.Index =Int32.Parse(IndexTextBox.Text.Trim());
                menuItem.Href = UrlTextBox.Text.Trim();
                menuItem.Name = DesTextBox.Text.Trim();
                menuItem.Title = TitleTextBox.Text.Trim();
                if (MenuID == "{00000000-0000-0000-0000-000000000000}")
                {
                    string theme = SiteSettingHelper.Instance.Config.CMSTheme;
                    if (theme == null || theme == "") theme = "classic";
                    string path = "~/" + Constants.ThemePath + "/" + theme + "/images";
                    Boolean fileOk = false;
                    //判断是否选择了文件
                    if (IconFileUpload.HasFile)
                    {
                        //返回文件的扩展名
                        string fileExtension = System.IO.Path.GetExtension(IconFileUpload.FileName).ToLower();
                        //设置文件类型
                        string[] allowExtensions = { ".jpg", ".gif", ".png" };
                        //判断选择文件是否符合条件
                        for (int i = 0; i < allowExtensions.Length; i++)
                        {
                            if (fileExtension == allowExtensions[i])
                            {
                                fileOk = true;
                            }
                        }
                        //文件大小的设置
                        if (IconFileUpload.PostedFile.ContentLength > 1024000)
                        {
                            fileOk = false;
                        }
                        if (fileOk)
                        {
                            string pathFile = Path.Combine(path, IconFileUpload.FileName);
                            IconFileUpload.PostedFile.SaveAs(Server.MapPath(pathFile));
                            menuItem.Icon = IconFileUpload.FileName;
                        }
                        else
                        {
                            throw new Exception("图片类型不对或文件超出1024KB！");
                        }
                    }
                    else
                    {
                        throw new Exception("请选择需要上传的图片...");
                    }
                    menuItem.Group = Int32.Parse(GroupTextBox.Text.Trim());
                }
                MenuHelper.UpdateMenuItem(menuItem);
                Messages.ShowMessage("更新成功");
            }
        }
    }
}
