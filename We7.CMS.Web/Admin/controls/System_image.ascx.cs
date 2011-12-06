using System;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.CMS.Config;
using We7.Framework.Config;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.controls
{
    public partial class System_image : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
            }
        }

        public void LoadConfigInfo()
        {
            #region 加载配置信息
            string configFile = Server.MapPath("~/Config/general.config");
            GeneralConfigInfo __configinfo = new GeneralConfigInfo();
            if (File.Exists(configFile))
            {
                __configinfo = GeneralConfigs.Deserialize(configFile);
            }
            watermarktype.SelectedValue = __configinfo.WaterMarkType.ToString();
            watermarkfontsize.Text = __configinfo.WaterMarkFontSize.ToString();
            watermarktext.Text = __configinfo.WaterMarkText.ToString();
            watermarkpic.Text = __configinfo.WaterMarkPic.ToString();
            watermarktransparency.Text = __configinfo.WaterMarkTransparency.ToString();
            MaxWidthOfUploadedImgTextbox.Text = __configinfo.MaxWidthOfUploadedImg.ToString();
            CuttoMaxCheckBox.Checked = __configinfo.CutToMaxWidthOfUploadedImg == 1;

            LoadPosition(__configinfo.WaterMarkStatus);

            LoadSystemFont();

            try
            {
                watermarkfontname.SelectedValue = __configinfo.WaterMarkFontName.ToString();
            }
            catch
            {
                watermarkfontname.SelectedIndex = 0;
            }

            #endregion
        }

        private void LoadSystemFont()
        {
            #region 加载系统字体
            watermarkfontname.Items.Clear();
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily family in fonts.Families)
            {
                watermarkfontname.Items.Add(new ListItem(family.Name, family.Name));
            }

            #endregion
        }

        public void LoadPosition(int selectid)
        {
            #region 加载水印设置界面

            position.Text = "<table width=\"256\" height=\"207\" border=\"0\" style=\"background:url(/admin/images/flower.jpg) no-repeat \">";
            for (int i = 1; i < 10; i++)
            {
                if (i % 3 == 1) position.Text += "<tr>";
                if (selectid == i)
                {
                    position.Text += "<td width=\"33%\" align=\"center\" style=\"vertical-align:middle;\"><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"" + i + "\" checked><b>#" + i + "</b></td>";
                }
                else
                {
                    position.Text += "<td width=\"33%\" align=\"center\" style=\"vertical-align:middle;\"><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"" + i + "\" ><b>#" + i + "</b></td>";
                }
                if (i % 3 == 0) position.Text += "</tr>";

            }

            position.Text += "</table><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"0\" ";
            if (selectid == 0)
            {
                position.Text += " checked";
            }
            position.Text += ">不启用水印功能";

            #endregion
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            //if (this.CheckCookie())
            //{

            //    Hashtable ht = new Hashtable();
            //    ht.Add("图片附件文字水印大小", watermarkfontsize.Text);
            //    ht.Add("JPG图片质量", attachimgquality.Text);
            //    ht.Add("图片最大高度", attachimgmaxheight.Text);
            //    ht.Add("图片最大宽度", attachimgmaxwidth.Text);

            //    foreach (DictionaryEntry de in ht)
            //    {
            //        if (!Utils.IsInt(de.Value.ToString()))
            //        {
            //            base.RegisterStartupScript("", "<script>alert('输入错误," + de.Key.ToString() + "只能是0或者正整数');window.location.href='global_attach.aspx';</script>");
            //            return;
            //        }

            //    }



            if (Convert.ToInt16(attachimgquality.Text) > 100 || (Convert.ToInt16(attachimgquality.Text) < 0))
            {
                this.Page.RegisterStartupScript("", "<script>alert('JPG图片质量只能在0-100之间');window.location.href='global_attach.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(watermarktransparency.Text) > 10 || Convert.ToInt16(watermarktransparency.Text) < 1)
            {
                this.Page.RegisterStartupScript("", "<script>alert('图片水印透明度取值范围1-10');window.location.href='global_attach.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(watermarkfontsize.Text) <= 0)
            {
                this.Page.RegisterStartupScript("", "<script>alert('图片附件添加文字水印的大小必须大于0');window.location.href='global_attach.aspx';</script>");
                return;
            }


            //if (Convert.ToInt16(attachimgmaxheight.Text) < 0)
            //{
            //    this.Page.RegisterStartupScript("", "<script>alert('图片最大高度必须大于或等于0');window.location.href='global_attach.aspx';</script>");
            //    return;
            //}

            //if (Convert.ToInt16(attachimgmaxwidth.Text) < 0)
            //{
            //    this.Page.RegisterStartupScript("", "<script>alert('图片最大宽度必须大于或等于0');window.location.href='global_attach.aspx';</script>");
            //    return;
            //}

            string configFile = Server.MapPath("~/Config/general.config");
            GeneralConfigInfo __configinfo = new GeneralConfigInfo();
            if (File.Exists(configFile))
            {
                __configinfo = GeneralConfigs.Deserialize(configFile);
            }
            //GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("~/Config/general.config"));
            __configinfo.WaterMarkStatus = We7Request.GetInt("watermarkstatus", 0);
            __configinfo.WaterMarkType = Convert.ToInt16(watermarktype.SelectedValue);
            __configinfo.WaterMarkText = watermarktext.Text;
            __configinfo.WaterMarkPic = watermarkpic.Text;
            __configinfo.WaterMarkFontName = watermarkfontname.SelectedValue;
            __configinfo.WaterMarkFontSize = Convert.ToInt32(watermarkfontsize.Text);
            __configinfo.WaterMarkTransparency = Convert.ToInt16(watermarktransparency.Text);
            __configinfo.MaxWidthOfUploadedImg = Convert.ToInt32(MaxWidthOfUploadedImgTextbox.Text);
            __configinfo.CutToMaxWidthOfUploadedImg = CuttoMaxCheckBox.Checked ? 1 : 0;

            GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("~/Config/general.config"));

            Response.Redirect("system.aspx");
            //this.Page.RegisterStartupScript("PAGE", "window.location.href='sytem.aspx';");
            #endregion
        }
    }
}
