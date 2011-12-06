using System;
using System.Collections.Generic;
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
using We7.CMS.Common;
using We7.Framework;
using We7.CMS.Accounts;

namespace We7.CMS.Web
{
    public partial class AttachmentDownload : Page
    {
        AttachmentHelper AttachmentHelper
        {
            get { return HelperFactory.Instance.GetHelper<AttachmentHelper>(); }
        }

        ArticleHelper ArticleHelper
        {
            get { return HelperFactory.Instance.GetHelper<ArticleHelper>(); }
        }

        ChannelHelper ChannelHelper
        {
            get { return HelperFactory.Instance.GetHelper<ChannelHelper>(); }
        }

        string AccountID
        {
            get { return Security.CurrentAccountID; }
        }

        string AttachmentID
        {
            get { return Request["id"]; }
        }

        string OwnerID
        {
            get 
            { 
                if(Request["oid"]!=null)
                    return Request["oid"];
                else if (Request["id"] != null)
                {
                    Attachment a = AttachmentHelper.GetAttachment(Request["id"].ToString());
                    if (a != null)
                        return a.ArticleID;
                }
                return null;
            }
        }

        string FileType
        {
            get { return Request["type"]; }
        }
        string FilesName
        {
            get { return Context.Server.UrlDecode(Request["fileName"]); }
        }

        Attachment ThisAttachment { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HasReadPermission();
            }
        }

        string FormatUrl()
        {
            string attachmentPath = "";
            if (!We7Helper.IsEmptyID(AttachmentID))
                ThisAttachment = AttachmentHelper.GetAttachment(AttachmentID);
            else if (!We7Helper.IsEmptyID(OwnerID) && FileType != "")
            {
                ThisAttachment = AttachmentHelper.GetFirstAttachment(OwnerID, FileType, FilesName);
            }
            if (ThisAttachment != null)
            {
                attachmentPath = string.Format("\\{0}\\{1}", ThisAttachment.FilePath, ThisAttachment.FileName);
            }
            return attachmentPath;
        }

        void FileDownload(string attachmentPath)
        {
            if (attachmentPath != "")
            {
                string FullFileName = Server.MapPath(attachmentPath);
                try
                {
                    //刷新下载次数
                    ThisAttachment.DownloadTimes += 1;
                    AttachmentHelper.UpdateAttachment(ThisAttachment);

                    FileInfo DownloadFile = new FileInfo(FullFileName); //设置要下载的文件
                    Response.Clear(); //清除缓冲区流中的所有内容输出
                    Response.ClearHeaders(); //清除缓冲区流中的所有头
                    Response.Buffer = false; //设置缓冲输出为false
                    //设置输出流的 HTTP MIME 类型为application/octet-stream
                    Response.ContentType = "application/octet-stream";
                    //将 HTTP 头添加到输出流
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFile.Name, System.Text.Encoding.UTF8));

                    Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());

                    //将指定的文件直接写入 HTTP 内容输出流。

                    Response.WriteFile(DownloadFile.FullName);
                    Response.Flush(); //向客户端发送当前所有缓冲的输出
                    Response.End(); //将当前所有缓冲的输出发送到客户端


                }
                catch (IOException)
                {
                    AttachmentMsgLabel.Text = "文件下载失败,请确认资源是否存在!";
                    AttachmentMsgLabel.Visible = true;
                    return;
                }
            }
            else
            {
                AttachmentMsgLabel.Text = "文件下载失败,该文件不存在!";
                AttachmentMsgLabel.Visible = true;
            }
        }

        /// <summary>
        /// 判断访问该栏目的权限
        /// </summary>
        /// <returns></returns>
        void HasReadPermission()
        {
            string BindColumnID = ArticleHelper.GetArticle(OwnerID, new string[] { "OwnerID" }).OwnerID;

            Channel ch = ChannelHelper.GetChannel(BindColumnID, new string[] { "SecurityLevel" });
            if (ch.SecurityLevel == 1)
            {
                if (AccountID != null)
                {
                    FileDownload(FormatUrl());
                }
                else
                {
                    Response.Write("<script language='javascript'>alert('您还没有登陆,请登陆后再下载该附件');window.location='Login.aspx';</script>");
                }
            }
            else if (ch.SecurityLevel == 2)
            {
                if (AccountID != null)
                {
                    // "您没有权限下载该附件";
                }
                else
                {
                    Response.Write("<script language='javascript'>alert('您还没有登陆,请登陆后再下载该附件');window.location='Login.aspx';</script>");
                }
            }
            else
            {
                FileDownload(FormatUrl());
            }
        }
    }
}
