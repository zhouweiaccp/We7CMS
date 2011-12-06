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
using System.Text.RegularExpressions;

using We7.CMS;
using We7.CMS.Controls;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.controls
{
    public partial class Channel_option : BaseUserControl
    {
        static string TextCannotModifyColumn = "不能修改栏目";

        string ParentID
        {
            get
            {
                string pid = Request["pid"];
                if (pid == null || pid.Length == 0)
                {
                    if (ParentIDTextBox.Text.Length == 0)
                    {
                        pid = We7Helper.EmptyGUID;
                    }
                    else
                        pid = ParentIDTextBox.Text;
                }
                return pid;
            }
            set
            {
                ParentIDTextBox.Text = value;
            }
        }

        public string ChannelID
        {
            get { return Request["id"]; }
        }

        protected IPSecurityHelper IPSecurityHelper
        {
            get { return HelperFactory.GetHelper<IPSecurityHelper>(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            InitControls();
            if (!IsPostBack)
            {
                if (We7Helper.IsEmptyID(ChannelID))
                {
                    throw new CDException(TextCannotModifyColumn);
                }
                if (Request["saved"] != null && Request["saved"].ToString() == "1")
                {
                    Messages.ShowMessage("栏目选项已经成功更新。");
                }

                ShowInfomation();
            }
        }

        protected void SaveButton_ServerClick(object sender, EventArgs e)
        {
            SaveInformation();
        }

        void InitControls()
        {
            ProcessDropDownList.Attributes["onchange"] = "visibleFlowList('" + this.ClientID + "');";
        }

        #region 保存信息

        void SaveInformation()
        {
            if (DemoSiteMessage)
            {
                return;
            }

            Channel ch = ChannelHelper.GetChannel(ChannelID, null);
            if (ch != null)
            {
                //ch.ReferenceID = ReferenceIDTextBox.Text;
                ch.RefAreaID = AreaIDTextBox.Text;
                ch.SecurityLevel = Convert.ToInt32(SecurityDropDownList.SelectedValue);
                if (TitleImageFileUpload.FileName != "")
                {
                    ch.TitleImage = UploadImage(ch.FullUrl);
                }

                ch.Process = ProcessDropDownList.SelectedValue;
                if (ch.Process == "1")
                {
                    ch.ProcessLayerNO = ProcessLayerDropDownlist.SelectedValue;
                    ch.ProcessEnd = ProcessEndingDropDownList.SelectedValue;
                }

                ch.KeyWord = KeywordTextBox.Text;
                ch.DescriptionKey = DescriptionTextBox.Text;
                ch.Parameter = ParameterTextBox.Text.Trim();
                ChannelHelper.UpdateChannel(ch);

                //Messages.ShowMessage("栏目信息已经成功更新。");

                string rawurl = We7Helper.AddParamToUrl(Request.RawUrl, "saved", "1");
                Response.Redirect(rawurl);

                //记录日志
                string content = string.Format("修改了栏目“{0}”的信息", ch.Name);
                AddLog("编辑栏目", content);
            }

        }

        string UploadImage(string url)
        {
            string ChannelPath = Server.MapPath(Channel.ChannelUrlPath);

            if (TitleImageFileUpload.FileName.Length < 1)
            {
                Messages.ShowError("图片不能为空!");
            }
            if (!CDHelper.CanUpload(TitleImageFileUpload.FileName))
            {
                Messages.ShowError("不支持上传该类型的图片。");
            }

            string logoName = url.Replace("/", "_");
            if (logoName.EndsWith("_"))
                logoName = logoName.Remove(logoName.Length - 1);
            if (logoName.StartsWith("_"))
                logoName = logoName.Remove(0, 1);
            string path = string.Format("{0}\\{1}{2}", ChannelPath, logoName, Path.GetExtension(TitleImageFileUpload.FileName));
            string ImagefileName = string.Format("{0}/{1}{2}", Channel.ChannelUrlPath, logoName, Path.GetExtension(TitleImageFileUpload.FileName));
            string thumbnailFilePath = string.Format("{0}\\{1}_S{2}", ChannelPath, logoName, Path.GetExtension(TitleImageFileUpload.FileName));

            if (!Directory.Exists(ChannelPath))
                Directory.CreateDirectory(ChannelPath);

            try
            {
                TitleImageFileUpload.SaveAs(path);
                ImageUtils.MakeThumbnail(path, thumbnailFilePath, 110, 90, "W");
            }
            catch (IOException)
            {
                Messages.ShowError("图片上传失败,请重试!");
            }

            return ImagefileName;
        }
        #endregion

        void ShowInfomation()
        {
            Channel ch = ChannelHelper.GetChannel(ChannelID, null);
            if (ch != null)
            {
                ParentID = ch.ParentID;
                KeywordTextBox.Text = ch.KeyWord;
                DescriptionTextBox.Text = ch.DescriptionKey;
                //ReferenceIDTextBox.Text = ch.ReferenceID;
                AreaIDTextBox.Text = ch.RefAreaID;
                ParameterTextBox.Text = ch.Parameter;

                SetDropdownList(SecurityDropDownList, ch.SecurityLevel.ToString());

                SetDropdownList(ProcessDropDownList, ch.Process);
                if (ch.Process == "1")
                {
                    ProcessLayerDropDownlist.Style["display"] = "";
                    ProcessEndingDropDownList.Style["display"] = "";
                    SetDropdownList(ProcessLayerDropDownlist, ch.ProcessLayerNO);
                    if (string.IsNullOrEmpty(ch.ProcessEnd)) ch.ProcessEnd = "0";
                    SetDropdownList(ProcessEndingDropDownList, ch.ProcessEnd);
                }

                //显示图片
                if (!string.IsNullOrEmpty(ch.TitleImage))
                {
                    string ext = Path.GetExtension(Server.MapPath(ch.TitleImage));
                    string filename = Path.GetFileNameWithoutExtension(Server.MapPath(ch.TitleImage));
                    string thumbnailFilePath = ch.TitleImage.Replace(ext, "_S" + ext);
                    TitleImage.ImageUrl = thumbnailFilePath;
                    TitleImage.NavigateUrl = ch.TitleImage;
                    TitleImage.Target = "_blank";
                    TitleImage.Visible = true;
                }
                else
                {
                    TitleImage.Visible = false;
                }

            }
        }

        string ThumbnailFileName(string oFile)
        {
            string tFile = oFile.Substring(0, oFile.IndexOf(".")) + "_S";
            tFile = tFile + oFile.Substring(oFile.IndexOf("."));
            return tFile;
        }

        protected void SetDropdownList(DropDownList list, string value)
        {
            int i = 0;
            foreach (ListItem item in list.Items)
            {
                if (item.Value == value)
                {
                    list.SelectedIndex = i;
                    return;
                }
                i++;
            }
        }

    }
}