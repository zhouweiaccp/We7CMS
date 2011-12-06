using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core.UI;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS;
using System.IO;
using We7.CMS.Common;
using System.Collections;
using We7.Model.UI.Controls.cs;
namespace We7.Model.UI.Controls.system.page
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    public class ThumbnailServer : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string id = context.Request.QueryString["ID"];
            Article a = new Article() { ID = id };
            DialogPath = a.AttachmentUrlPath + "/thumbnail";
            string ret = "操作失败";
            if (context.Request.QueryString["Type"] == "BindImgList")
            {
                //显示已经有的图片
                ret = BindImgList();
            }
            if (context.Request.QueryString["Type"] == "DelFile")
            {
                string fileName = context.Request.QueryString["fileName"];
                ret = fileName;
                DelFile(fileName);
            }
            context.Response.Write(ret);
        }
        protected string BindImgList()
        {
            string strHtml = We7.Model.UI.Controls.system.page.ShowImgList.BindImgList(DialogPath);
            return strHtml.ToString();

        }
        protected void DelFile(string FileName)
        {
            FileHelp.DeleteFile(FileName);
        }
        public string DialogPath { get; set; }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }
}
