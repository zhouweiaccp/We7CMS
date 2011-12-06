<%@ WebHandler Language="C#" Class="UpDownloadHandler" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Text;
using System.IO;
using We7.Model.Core;
using We7.Model.Core.Data;

public class UpDownloadHandler : IHttpHandler, IRequiresSessionState
{

	public void ProcessRequest(HttpContext context)
	{
		context.Response.ContentType = "text/plain";
		context.Response.ContentEncoding = System.Text.Encoding.UTF8;
		//通过传来的参数判断执行哪个方法
		if (context.Request["GetFunction"].Equals("UploadFile", StringComparison.InvariantCultureIgnoreCase))
			UploadFile(context);
		if (context.Request["GetFunction"].Equals("DownloadFile", StringComparison.InvariantCultureIgnoreCase))
			DownloadFile(context);
		if (context.Request["GetFunction"].Equals("DeleteDocument", StringComparison.InvariantCultureIgnoreCase))
			DeleteDocument(context);

	}

	public bool IsReusable
	{
		get
		{
			return false;
		}
	}
	//上传文件
	private void UploadFile(HttpContext context)
	{
		string uploadFilePath = string.Format("/_data/Models/{0}/{1}/{2}/", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
		HttpPostedFile file = context.Request.Files["Filedata"];

		UploadDocumentItem objUploadDocumentItem = new UploadDocumentItem();
		objUploadDocumentItem.DocMuid = Guid.NewGuid().ToString();//生成一个新的MUID，保证文件名称的唯一性
		objUploadDocumentItem.DocName = file.FileName;
		objUploadDocumentItem.UploadDate = DateTime.Now.ToShortDateString();

		ModelDBHelper helper = ModelDBHelper.Create(We7.We7Helper.GetParamValueFromUrl(context.Request.RawUrl, "model"));

		string uploadPath = HttpContext.Current.Server.MapPath(uploadFilePath);

		if (file != null)
		{
			//如果没有该目录则创建该上传目录
			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);
			}
			string extension = file.FileName.Substring(file.FileName.LastIndexOf('.'));
			string filepath = uploadPath + objUploadDocumentItem.DocMuid + extension;
			file.SaveAs(filepath);
			We7.Framework.Util.ImageUtils.MakeThumbnail(filepath, uploadPath + objUploadDocumentItem.DocMuid + "_thumb" + extension, 100, 100, "Cut");
			context.Response.Write(uploadFilePath + objUploadDocumentItem.DocMuid + extension);
			//context.Response.Write(new JavaScriptSerializer().Serialize(objUploadDocumentItem));
		}
	}

	//下载文件
	private void DownloadFile(HttpContext context)
	{
		UploadDocumentItem objUploadDocumentItem = new UploadDocumentItem();
		if (!string.IsNullOrEmpty(context.Request["DocMuid"]))
		{
			/*客户端保存的文件名,我写这个DEMO的时候没有数据库，所以下载文件名我就固定为'DEMO'
			使用这DEMO的朋友可以配合数据库，读出上传时写入数据库的文件名字和格式*/
			string fileName = "demo";
			string filePath = HttpContext.Current.Server.MapPath(@"/Upload/" + context.Request["DocMuid"]);

			//以字符流的形式下载文件
			FileStream fs = new FileStream(filePath, FileMode.Open);
			byte[] bytes = new byte[(int)fs.Length];
			fs.Read(bytes, 0, bytes.Length);
			fs.Close();
			context.Response.Clear();
			context.Response.ContentType = "application/octet-stream";
			//通知浏览器下载文件而不是打开
			context.Response.AddHeader("Content-Disposition", "attachment;   filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
			context.Response.BinaryWrite(bytes);
			context.Response.Flush();
			context.Response.End();
		}
	}
	//删除文件
	private void DeleteDocument(HttpContext context)
	{

		string filePath = HttpContext.Current.Server.MapPath(@"/Upload/" + context.Request["DocMuid"]);
		System.IO.FileInfo file = new System.IO.FileInfo(filePath);
		if (file.Exists)
		{
			file.Delete();
		}
		if (!string.IsNullOrEmpty(context.Request["DocMuid"]))
		{
			context.Response.Write(new JavaScriptSerializer().Serialize("Success"));
		}
	}

}
public class UploadDocumentItem
{
	public UploadDocumentItem()
	{ }
	private string docMuid;
	private string docName;
	private string uploadDate;
	public string DocMuid { get { return docMuid; } set { docMuid = value; } }
	public string DocName { get { return docName; } set { docName = value; } }
	public string UploadDate { get { return uploadDate; } set { uploadDate = value; } }
}