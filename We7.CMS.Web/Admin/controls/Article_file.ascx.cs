using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Framework;

namespace We7.CMS.Web.Admin.controls
{
    public partial class Article_file : BaseUserControl
    {
        public string OwnerID
        {
            get
            {
                if (Request["oid"] != null)
                    return Request["oid"];
                else
                {
                    if (ViewState["$VS_OwnerID"] == null)
                    {
                        if (ArticleID != null)
                        {
                            Article a = ArticleHelper.GetArticle(ArticleID, null);
                            ViewState["$VS_OwnerID"] = a.OwnerID;
                        }
                    }
                    return (string)ViewState["$VS_OwnerID"];
                }
            }
        }

        public string ArticleID
        {
            get { return Request["id"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAttachmentList();
            }
            //多文件上传
            //string jscript = "function UploadComplete(){" + this.Page.ClientScript.GetPostBackEventReference(LinkButton1, "") + "};";
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Article a = ArticleHelper.GetArticle(ArticleID);
            Session["ARTICLEDETAIL_CHANNELFILEPATH"] = a.AttachmentUrlPath;

            if (Request["count"] != null && Request["count"].ToString() != "")
            {
                Messages.ShowMessage("您成功上传"+Request["count"].ToString() +"个图片");
            }
        }
        /*
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
           //多图片上传
            int count = 0;
            if (Session["flashFiles"] != null)
            {
                string photoPath = Session["flashFiles"] as string;
                if (photoPath.Contains(","))
                {
                    string[] photoPathTeam = photoPath.Split(',');
                    foreach (string path in photoPathTeam)
                    {
                        InsertAttachment(path);
                        count++;
                    }
                }
                else
                {
                    InsertAttachment(photoPath);
                    count++;
                }
            }
            Session["flashFiles"] = null;
            string rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "saved");
            rawurl = We7Helper.AddParamToUrl(rawurl, "saved", "1");
            rawurl = We7Helper.AddParamToUrl(rawurl, "count", count.ToString());
            Response.Redirect(rawurl);
        }
         * */

        void InsertAttachment(string path)
        {
            Attachment a = new Attachment();
            string ap = Server.MapPath(path);
            a.FileName = Path.GetFileName(ap);
            a.FileType = Path.GetExtension(ap);
            a.FilePath = path.Replace("/" + a.FileName,"");
            a.ArticleID = ArticleID;
            int type = (int)ArticleAttachment.ArticlePhoto;
            a.EnumState = type.ToString();
            AttachmentHelper.AddAttachment(a);

        }
        void ArticleAttachmentFileUpload()
        {
            if (AttachmentFileUpload.FileName.Length < 1)
            {
                Messages.ShowError("附件不能为空!");
                return;
            }
            if (CDHelper.CanUploadAttachment(AttachmentFileUpload.FileName))
            {
                Messages.ShowError("不支持上传该类型的文件。");
                return;
            }
            string ap = GenerateFileName(Path.GetFileName(AttachmentFileUpload.FileName));
            try
            {
                AttachmentFileUpload.SaveAs(ap);
            }
            catch (IOException ex)
            {
                Messages.ShowError("附件上传失败!" + ex.Message);
                return;
            }

            Attachment a = new Attachment();
            a.FileName = Path.GetFileName(ap);
            a.FilePath = GetAttachmentPath();
            a.FileSize = AttachmentFileUpload.PostedFile.ContentLength;
            a.FileType = Path.GetExtension(ap);
            a.ArticleID = ArticleID;
            AttachmentHelper.AddAttachment(a);
            string rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "saved");
            rawurl = We7Helper.AddParamToUrl(rawurl, "saved", "1");
            Response.Redirect(rawurl);
        }

        string GenerateFileName(string fileName)
        {
            string folderPath = Server.MapPath(GetAttachmentPath());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fn = Path.Combine(folderPath, AttachmentFileUpload.FileName);
            string ext = Path.GetExtension(fn);
            string fw = Path.GetFileNameWithoutExtension(fn);
            int i = 1;
            while (File.Exists(fn))
            {
                fn = Path.Combine(folderPath, String.Format("{0}({1}){2}", fw, i++, ext));
            }
            return fn;
        }

        void BindAttachmentList()
        {
            List<Attachment> atms = AttachmentHelper.GetAttachments(ArticleID);
            DataGridView.DataSource = atms;
            DataGridView.DataBind();
        }

        string GetAttachmentPath()
        {
            string sn = We7Helper.GUIDToFormatString(ArticleID);
            return Constants.AttachmentUrlPath + sn;
        }


        protected void AttachmentUpload_ServerClick(object sender, EventArgs e)
        {
            ArticleAttachmentFileUpload();
        }

        protected void DeleteAttachmentButton_Click(object sender, EventArgs e)
        {
            string aid = AttachmentIDTextBox.Text;
            AttachmentHelper.DeleteAttachment(aid);
            BindAttachmentList();
        }
    }
}