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
using We7.Framework.Common;

namespace We7.CMS.Web.Admin.ContentModel.Controls.Page
{
    public partial class SimpleImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltlMsg.Text = "";
        }

        protected void btnUpload_Click(object sender, EventArgs args)
        {
            if (!ValidateUpload())
            {
                ltlMsg.Text = "<br><font color='red'>文件为空或文件格式不对</font>";
                return;
            }
            UploadFile();
        }

        public string ArticleID
        {
            get
            {
                return Request["aid"];
            }
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

            string OrignPath = folder.TrimEnd('/') + "/" + newFileName + ext;
            string physicalpath = Server.MapPath(folder);
            if (!Directory.Exists(physicalpath))
            {
                Directory.CreateDirectory(physicalpath);
            }
            string physicalfilepath = Server.MapPath(OrignPath);
            fuImage.SaveAs(physicalfilepath);

            imgCurrentImage.ImageUrl = OrignPath;

            ImageInfo info = new ImageInfo();
            info.Add(new ImageItem() { Size = "", Src =OrignPath, Type="" });
            ImageValue.Value = info.ToJson();
        }

        bool ValidateUpload()
        {
            if (String.IsNullOrEmpty(fuImage.FileName))
                return false;
            string ext = Path.GetExtension(fuImage.FileName).Trim('.');
            string[] list = new string[] { "jpg", "jpeg", "gif", "png", "bmp" };
            return We7.Framework.Util.Utils.InArray(ext.ToLower(), list);
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
            return article.AttachmentUrlPath.TrimEnd("/".ToCharArray()) + "/Thumbnail";
        }

        string CreateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random((int)DateTime.Now.Ticks).Next();
        }
    }
}
