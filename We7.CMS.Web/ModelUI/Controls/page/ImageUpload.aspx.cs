using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using We7.Model.Core.Config;
using We7.CMS.Common;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.ContentModel.Controls.Page
{
    public partial class ImageUpload : System.Web.UI.Page
    {
        public string ThumbPath
        {
            get
            {
                return ViewState["_ThumbPath"] as string;
            }
            set
            {
                ViewState["_ThumbPath"] = value;
            }
        }

        public string OrignPath
        {
            get
            {
                return ViewState["_OrignPath"] as string;
            }
            set
            {
                ViewState["_OrignPath"] = value;
            }
        }

        public string ArticleID
        {
            get
            {
                return Request["aid"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ltlMsg.Text = "";
            if (!IsPostBack)
            {
                ThumbPath = Request["v"];
                OrignPath = Request["v"];
                imgThumb.ImageUrl = String.IsNullOrEmpty(ThumbPath) ? ResolveClientUrl("~/ModelUI/skin/images/nopic.gif") : ThumbPath;
            }
        }

        protected void bttnUpload_Click(object sender, EventArgs e)
        {
            if (!ValidateUpload())
            {
                ltlMsg.Text = "<br><font color='red'>文件为空或文件格式不对</font>";
                return;
            }
            UploadFile();
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        void UploadFile()
        {
            string fileName = fuImage.FileName;
            string ext = Path.GetExtension(fileName);
            string folder = GetFileFolder();
            string newFileName = CreateFileName();

            OrignPath = folder.TrimEnd('/') + "/" + newFileName + ext;
            string physicalpath = Server.MapPath(folder);
            if (!Directory.Exists(physicalpath))
            {
                Directory.CreateDirectory(physicalpath);
            }
            string physicalfilepath = Server.MapPath(OrignPath);
            fuImage.SaveAs(physicalfilepath);

            ThumbPath = folder.TrimEnd('/') +"/"+newFileName + "_thumb" + ext;
            string physicaltargetpath = Server.MapPath(ThumbPath);
            CreateTumbnail(physicalfilepath, physicaltargetpath);
            imgThumb.ImageUrl = ThumbPath;
        }

        bool ValidateUpload()
        {
            if (String.IsNullOrEmpty(fuImage.FileName))
                return false;
            string ext = Path.GetExtension(fuImage.FileName).Trim('.');
            string[] list = new string[] { "jpg", "jpeg", "gif", "png", "bmp" };
            return We7.Framework.Util.Utils.InArray(ext.ToLower(), list);
        }


        void CreateTumbnail(string op, string tp)
        {
            int width, height;
            int.TryParse(txtWidth.Text.Trim(), out width);
            int.TryParse(txtHeight.Text.Trim(), out height);

            width = width > 0 ? width : 200;
            height = height > 0 ? height : 150;
            string ext = Path.GetExtension(op).Trim('.').ToLower();
            if (ext == "jpg" || ext == "jpeg")
            {
                ImageUtils.MakeThumbnail(op, tp, width, height, ddlThumbType.SelectedValue);
            }
            else
            {
                ImageUtils.MakeThumbnail(op, tp, width, height, ddlThumbType.SelectedValue,"");
            }
        }

        /// <summary>
        /// 创建文件路径
        /// </summary>
        /// <param name="ext">文件扩展名</param>
        /// <returns>文件的绝地路径</returns>
        string GetFileFolder()
        {
            Article article = new Article();
            article.ID = ArticleID;
            return article.AttachmentUrlPath.TrimEnd("/".ToCharArray())+"/Thumbnail";
        }

        string CreateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random((int)DateTime.Now.Ticks).Next();
        }

        protected void bttnOrigin_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "orign", "setValue('" + ResolveUrl(OrignPath) + "')", true);
        }

        protected void bttnThumbnail_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "thumb", "setValue('" + ResolveUrl(ThumbPath) + "')", true);
        }
    }
}
