using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using We7.CMS.Common;
using We7.Framework.Common;

namespace We7.Model.UI.Controls.page
{
    public partial class SimpleAttachment : System.Web.UI.Page
    {
        public string ArticleID
        {
            get
            {
                return Request["aid"];
            }
        }

        public ResourceArray Resources
        {
            get
            {
                if (ViewState["$Resources"] == null)
                {
                    ViewState["$Resources"] = new ResourceArray();
                }
                return ViewState["$Resources"] as ResourceArray;
            }
        }

        public string GetFileName(object o)
        {
            string fn = o != null ? o.ToString() : String.Empty;
            return Path.GetFileName(fn);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rptList.ItemCommand+=new RepeaterCommandEventHandler(rptList_ItemCommand);
        }

        protected void BindResouse()
        {
            rptList.DataSource=Resources;
            rptList.DataBind();
            AttachmentValue.Value = Resources.ToJson();
        }

        void  rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int index=-1;
            for (int i=0; i < Resources.Count; i++)
            {
                if (Resources[i].ToString() == e.CommandArgument.ToString())
                {
                    index = i;
                    break;
                }
            }
            if(index>-1)
                Resources.RemoveAt(index);
            BindResouse();
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
            //string ext = Path.GetExtension(fileName);
            string folder = GetFileFolder();
            string newFileName = Path.GetFileName(fileName); //CreateFileName();

            string OrignPath = folder.TrimEnd('/') + "/" + newFileName;// +ext;
            string physicalpath = Server.MapPath(folder);
            if (!Directory.Exists(physicalpath))
            {
                Directory.CreateDirectory(physicalpath);
            }
            string physicalfilepath = Server.MapPath(OrignPath);
            fuImage.SaveAs(physicalfilepath);

            Resources.Add(OrignPath);
            BindResouse();
        }

        bool ValidateUpload()
        {
            if (String.IsNullOrEmpty(fuImage.FileName))
                return false;
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
