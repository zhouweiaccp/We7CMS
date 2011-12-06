using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using We7.CMS.Common;

namespace We7.Model.UI.Controls.page
{
    public partial class AttachmentUpload : System.Web.UI.Page
    {
        public string ArticleID
        {
            get
            {
                return Request["aid"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

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

            string OrignPath = folder.TrimEnd('/') + "/" + newFileName + ext;
            string physicalpath = Server.MapPath(folder);
            if (!Directory.Exists(physicalpath))
            {
                Directory.CreateDirectory(physicalpath);
            }
            string physicalfilepath = Server.MapPath(OrignPath);
            fuImage.SaveAs(physicalfilepath);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "orign", "setValue('" + ResolveUrl(OrignPath) + "')", true);
        }

        bool ValidateUpload()
        {
            if (String.IsNullOrEmpty(fuImage.FileName))
                return false;
            string ext = Path.GetExtension(fuImage.FileName).Trim('.');
            string[] list = new string[] { "jpg", "jpeg", "gif", "png", "bmp" };
            return true;
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
            return article.AttachmentUrlPath.TrimEnd('/') + "/Attachment";
        }

        string CreateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random((int)DateTime.Now.Ticks).Next();
        }
    }
}
