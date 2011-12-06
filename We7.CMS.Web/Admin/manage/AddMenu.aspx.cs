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
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin
{
    public partial class AddMenu : BasePage
    {
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

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                if (Request["nomenu"] != null)
                    return MasterPageMode.NoMenu;
                else
                    return MasterPageMode.FullMenu;
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
                    TitleTextBox.Enabled = false;
                    ModelInfo model = ModelHelper.GetModelInfoByName(ModelName);
                    InitMenuData(model.Label, ModelName);
                    ListItem item = new ListItem(model.Label, ModelName);
                    MenuDropDownList.Items.Add(item);

                    //如果没有左侧菜单，来自于弹出框，不显示“返回”按钮
                    if (Request["nomenu"] != null)
                    {
                        ReturnHyperLink.Visible = false;
                    }
                }
                else
                {
                    BindingData();
                }

                BindChildIndex();
                if (EntityID == "System.User")
                    ReturnHyperLink.NavigateUrl = "Menulist.aspx?type=1";
                else
                    ReturnHyperLink.NavigateUrl = "Menulist.aspx";
                if (EntityID == "System.User")
                {
                    trAddMenu.Visible = false;
                }
                else
                {
                    trAddMenu.Visible = true;
                }
            }
        }

        #region 已注释
        /*
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
        string CreateUploadImageIcon(string path,string pathFile1,string pathFile2,string imgType)
        {
            string imgID = MenuDropDownList.SelectedItem.Value;
            imgID = imgID.Replace(".", "_");
            string iconFile = Path.Combine(Server.MapPath(path), "menu_c_" + imgID + imgType);
            string iconHoverFile = Path.Combine(Server.MapPath(path),  "menu_c_" + imgID + "_hover" + imgType);
            ImageUtils.GenerateIcon(pathFile1, iconFile, 30, 30, 15, 15, imgType); 
            ImageUtils.GenerateIcon(pathFile2,iconHoverFile, 30, 30, 15, 15, imgType);
            return Path.GetFileName(iconFile);
        }

        /// <summary>
        /// 菜单名称首字母生成图标
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string CreateLetterNewIcon(string path, string imgType)
        {
            string imgID = MenuDropDownList.SelectedItem.Value;
            imgID = imgID.Replace(".", "_");
            string firstChar = MainTitleTextBox.Text.Substring(0, 1);
            firstChar = MenuHelper.GetChineseSpell(firstChar);
            firstChar = firstChar.Substring(0, 1).ToUpper();
            string iconFile = Path.Combine(Server.MapPath(path), "menu_c_" + imgID + imgType);
            string iconHoverFile = Path.Combine(Server.MapPath(path), "menu_c_" + imgID + "_hover" + imgType);
            ImageUtils.GenerateImgFromText(iconFile, firstChar, 30, 30, "Times New Roman", 14, System.Drawing.Color.DarkGray, imgType);
            ImageUtils.GenerateImgFromText(iconHoverFile, firstChar, 30, 30, "Times New Roman", 14, System.Drawing.Color.Red, imgType);
            return Path.GetFileName(iconFile);
        }
       
         * void BindingIndex()
        {
            List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID, 0, EntityID);

            foreach (We7.CMS.Common.MenuItem menuItem in menus)
            {
                string name =  menuItem.Title.ToString();
                string value = menuItem.Group.ToString() + "," + menuItem.Index.ToString();
                ListItem item = new ListItem(name, value);
                DropDownListType.Items.Add(item);
            }

            if (DropDownListType.Items.Count == 0)
            {
                Messages.ShowMessage("无主菜单");
                return;
            }

            ListItem currentItem = DropDownListType.Items.FindByText("文件");
            if (currentItem != null)
                currentItem.Selected = true;
        }
         * 
        */
        #endregion


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
            List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID, 2, EntityID);

            string myname = "├内容发布";
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
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                //string mainIconName = GetIconFileName();
                //string mainTitle = MainDesTextBox.Text.Trim();
                //string mianNameText = MainTitleTextBox.Text.Trim();
                //int maingroup = Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[0]);

                //int mainIndex =0; 
                //if (Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[1]) > 0)
                //{ 
                //    mainIndex = Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[1]) - 1; 
                //}

                //int maingroup = Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]);
                int firstIndex = 0;
                if (Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]) > 0)
                {
                    firstIndex = Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]) - 1;
                }
                string parentID = SecondIndexDropDownList.SelectedItem.Value.Split(',')[0];

                string firstTitle = DesTextBox.Text.Trim();
                string firstNameText = TitleTextBox.Text.Trim();
                string firstUrl = UrlTextBox.Text.Trim();

                //发布菜单
                string secondTitle = ReleaseDesTextBox.Text.Trim();
                string secondnameText = ReleaseTitleTextBox.Text.Trim();
                string secondurl = ReleaseUrlTextBox.Text.Trim();
                int secondindex = firstIndex;
                if (EntityID == "System.User")
                {
                    MenuHelper.CreateModelMenu(parentID, firstIndex, firstNameText, firstTitle, firstUrl, firstIndex, EntityID);
                }
                else
                {
                    MenuHelper.CreateModelMenu(parentID, firstIndex, firstNameText, firstTitle, firstUrl, firstIndex, secondnameText, secondTitle, secondurl, secondindex, EntityID);
                }
                //MenuHelper.CreateContentMenu(firstNameText, firstTitle, maingroup, firstIndex, "", firstNameText, firstTitle, firstUrl, firstIndex, secondnameText, secondTitle, secondurl, secondindex, EntityID);
                //if (EntityID == "System.User")
                //{
                //    string path = HttpContext.Current.Server.MapPath("~/User/Resource/menuItems.xml");
                //    if (File.Exists(path))
                //    {
                //        XmlNodeList nodes = XmlHelper.GetXmlNodeList(path, "//items");
                //        XmlNode itemsNode = XmlHelper.GetXmlNode(path, "//items");

                //    }
                //}

                //如果没有左侧菜单，来自于弹出框，保存成功后提示成功，调用父窗体关闭Form方法
                if (Request["nomenu"] != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.parent.CloseForm('添加成功',window.parent.ReloadMenu)", true);
                }
                else
                {
                    string url = We7Helper.AddParamToUrl(ReturnHyperLink.NavigateUrl, "reload", "menu");
                    url = We7Helper.AddParamToUrl(url, "add", firstTitle + "（" + firstTitle + "," + secondTitle + "）");
                    HttpContext.Current.Session.Clear();
                    Response.Redirect(url);
                }
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
            if (MenuHelper.ExistMenuItem(title) != "")
            {
                Messages.ShowMessage(title + "内容模型已经存在，不能再创建");
                return;
            }
            //MainTitleTextBox.Text = title;
            //MainDesTextBox.Text = title;
            //DisplayTextBox.Text = title;

            //如果【管理】二字需要变更 请同步更改：\We7.CMS.Web\Admin\ContentModel\ajax\ContentModel.asmx.cs
            //                                中的 public string DeleteModel(string model)方法
            TitleTextBox.Text = title + "管理";
            DesTextBox.Text = title + "管理";

            //2011-10-10 为了删除内容模型的时候同步删除左侧菜单内容，所以内容模型加入左侧菜单的名称不可更改
            TitleTextBox.Enabled = false;

            if (EntityID == "System.User")
                UrlTextBox.Text = "/User/ModelHandler.aspx?model=" + value + "";
            else
                UrlTextBox.Text = "/admin/AddIns/ModelList.aspx?notiframe=1&model=" + value + "";
            //IndexTextBox.Text = "1";
            ReleaseTitleTextBox.Text = "发布" + title;
            ReleaseDesTextBox.Text = "新添";
            //ReleaseIndexTextBox.Text = "2";
            ReleaseUrlTextBox.Text = "/admin/addins/ModelEditor.aspx?notiframe=1&model=" + value + "";




        }
    }
}
