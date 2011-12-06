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
using System.Threading;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin
{
    public partial class download : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.Request["url"] != null)
            {
                string s = Request["url"].ToString();
                s = Server.MapPath(s);
                int i = 0;
                for (; i < 5; i++)
                {
                    try
                    {
                        FileDownload(s);
                        break;
                    }
                    catch
                    {
                        i = i == 4 ? i++ : i;
                        Thread.Sleep(1000);
                    }
                }
                if (i == 6)
                    Messages.ShowError("导出数据出错!可能服务器下载任务过多，请稍后再试！");
                Page.RegisterStartupScript("close", "<script>window.close();</script>");
            }
        }

        private void FileDownload(string FullFileName)
        {
            FileInfo DownloadFile = new FileInfo(FullFileName); //设置要下载的文件
            Response.Clear(); //清除缓冲区流中的所有内容输出
            Response.ClearHeaders(); //清除缓冲区流中的所有头
            Response.Buffer = false; //设置缓冲输出为false
            //设置输出流的 HTTP MIME 类型为application/octet-stream
            Response.ContentType = "application/octet-stream";
            //将 HTTP 头添加到输出流
            string s = HttpUtility.UrlEncode(System.Text.UTF8Encoding.UTF8.GetBytes(DownloadFile.Name));
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + s);// HttpUtility.UrlEncode(DownloadFile.Name, System.Text.Encoding.UTF8));

            Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());

            //将指定的文件直接写入 HTTP 内容输出流。

            Response.WriteFile(DownloadFile.FullName);
            Response.Flush(); //向客户端发送当前所有缓冲的输出
            Response.End(); //将当前所有缓冲的输出发送到客户端

        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }
    }
}
