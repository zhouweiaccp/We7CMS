using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using We7.CMS.Config;
using We7.CMS;
using We7.CMS.Common;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.Framework;

namespace We7.CMS.Web.Admin.controls
{
	/// <summary>
	/// 缩略图上传
	/// </summary>
	public partial class Article_image : BaseUserControl
	{
		#region 属性
		private string reName;
		/// <summary>
		/// 获得是不是编辑不同的照片缩略图的表单参数
		/// 目的是标识是不是需要替换文件名称
		/// </summary>
		public string ReName
		{
			get
			{
				if (this.Request.Form["reName"] != null)
				{
					this.reName = this.Request.Form["reName"].Trim();
				}
				return reName;
			}

		}
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
		public string ImagePaths
		{
			get
			{
				string imagepath = "0";
				if (ArticleID != null && ArticleID != "")
				{
					Article a = ArticleHelper.GetArticle(ArticleID);
					if (a != null && a.Thumbnail != null && a.Thumbnail != "")
					{
						imagepath = "1";
					}
				}
				return imagepath;
			}
		}
		public string Succeed
		{
			get { return Request["succeed"]; }
		}
		GeneralConfigInfo config;
		/// <summary>
		/// 水印参数
		/// </summary>
		public GeneralConfigInfo ImageConfig
		{
			get
			{
				if (config == null)
					config = GeneralConfigs.Deserialize(We7Utils.GetMapPath("~/Config/general.config"));
				return config;
			}
		}
		public string OriginalImagePath { get; set; }

		public int width;
		public int height;
		public string bigHeadImage;

		#endregion

		/// <summary>
		/// 页面加载
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Request["uploaded"] != null)
					Messages.ShowMessage("您已成功上传原图" + Request["uploaded"].ToString() + " 。");
				else if (Request["generated"] != null)
					Messages.ShowMessage(string.Format("您成功生成标签为 {0} 的缩略图！", Request["generated"].ToString()));

				InitControls();
				InitSizeDroplistFromXML();
				InitOriginalImagePath();
				LoadImages();
			}
			Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
		}

		#region 初始化
		/// <summary>
		/// 内容初始化
		/// </summary>
		void InitControls()
		{
			UploadImage.Attributes["onclick"] = "return articleImageCheck('" + this.ClientID + "','" + ImagePaths + "');";
			SizesDropDownList.Attributes["onchange"] = "changeFrame(this)";
			SizesDropDownList.Attributes["onblur"] = "saveImageSizeToCookies(this)";
		}

		/// <summary>
		/// 初始化原图
		/// </summary>
		void InitOriginalImagePath()
		{
			string imagepath = "/admin/images/article_small.gif";
			Article a = ArticleHelper.GetArticle(ArticleID);
			if (a.Thumbnail != null && a.Thumbnail.Length > 0)
			{
				string path = a.Thumbnail;
				path = Server.MapPath(path);
				string filename = Path.GetFileNameWithoutExtension(path);
				if (!filename.Contains("_S") && File.Exists(path))
				{
					imagepath = a.Thumbnail;
				}
				if (filename.Contains("_S"))
				{
					path = path.Replace("_S", "");
					if (File.Exists(path))
					{
						imagepath = a.Thumbnail.Replace("_S", "");
					}
				}
			}
			OriginalImagePath = imagepath;
			ThumbnailImages imageHelp = new ThumbnailImages();
			ImageInformation imageInfo = (ImageInformation)imageHelp.GetImageInfo(Server.MapPath(OriginalImagePath));
			width = imageInfo.Width;
			height = imageInfo.Height;
		}

		/// <summary>
		/// 加载缩略图列表
		/// </summary>
		void LoadImages()
		{
			if (ArticleID != null)
			{
				Article a = ArticleHelper.GetArticle(ArticleID);
				List<ImageDetail> imageList = new List<ImageDetail>();
				string folderPath = ExistFilePath();
				if (!Directory.Exists(folderPath))
				{
					return;
				}
				else
				{
					List<ThumbnailConfig> configList = GetAllThumbnailConfigs();
					System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderPath);
					//string[] fields = new string[] { "ChannelFolder" };
					//Channel ch = ChannelHelper.GetChannel(OwnerID, fields);
					//string aid = Helper.GUIDToFormatString(ArticleID);
					StringBuilder sb = new StringBuilder();
					foreach (System.IO.FileInfo fi in dir.GetFiles())
					{
						if (Server.MapPath(a.Thumbnail) == fi.FullName)
						{
							continue;
						}
						ImageDetail image = new ImageDetail();
						image.ImagePath = string.Format("/{0}/{1}", VFolderPath.Trim('/'), fi.Name);

						image.ImagePath = image.ImagePath.Replace("\\", "/");
						string idChar = fi.Name.Substring(fi.Name.LastIndexOf("_") + 1, fi.Name.LastIndexOf(".") - fi.Name.LastIndexOf("_") - 1);
						ThumbnailConfig config = SearchThumbnailConfig(configList, idChar);
						image.FileName = fi.Name;
						if (config != null)
						{
							image.Name = config.Name;
							image.Tag = config.Tag;
							imageList.Add(image);
						}
					}
				}

				ImagesRepeater.DataSource = imageList;
				ImagesRepeater.DataBind();
			}
		}

		/// <summary>
		/// 根据标签tag，获取缩略图对应规格
		/// </summary>
		/// <param name="list"></param>
		/// <param name="IdentityChar"></param>
		/// <returns></returns>
		ThumbnailConfig SearchThumbnailConfig(List<ThumbnailConfig> list, string IdentityChar)
		{
			ThumbnailConfig tc = new ThumbnailConfig();
			if (list != null && list.Count > 0)
			{
				foreach (ThumbnailConfig thumbnailConfig in list)
				{
					if (thumbnailConfig.Tag == IdentityChar)
					{
						tc = thumbnailConfig;
						return tc;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// 加载所有规格列表
		/// </summary>
		/// <returns></returns>
		List<ThumbnailConfig> GetAllThumbnailConfigs()
		{
			List<ThumbnailConfig> thumbnailConfigList = Context.Cache["$THUMBNAILCONFIGLIST"] as List<ThumbnailConfig>;

			if (thumbnailConfigList == null)
			{
				if (File.Exists(Server.MapPath("/Config/thumbnail.xml")))
				{
					thumbnailConfigList = new List<ThumbnailConfig>();
					XmlDocument doc = new XmlDocument();
					doc.Load(Server.MapPath("/Config/thumbnail.xml"));
					XmlNodeList ItemListNodes = doc.SelectNodes("/configuration/item");

					foreach (XmlNode oldNode in ItemListNodes)
					{
						ThumbnailConfig tc = new ThumbnailConfig();
						tc.Name = oldNode.Attributes["name"].Value;
						string[] v = tc.Name.Split(new string[] { ":", "：" }, StringSplitOptions.RemoveEmptyEntries);
						if (v.Length > 1)
							tc.Value = v[1];
						tc.Tag = oldNode.Attributes["value"].Value;
						thumbnailConfigList.Add(tc);
					}
					ChannelHelper.CacherCache("$THUMBNAILCONFIGLIST", Context, thumbnailConfigList, CacheTime.Short);
				}
			}
			return thumbnailConfigList;
		}

		/// <summary>
		/// 从配置文件加载缩略图规格
		/// </summary>
		void InitSizeDroplistFromXML()
		{
			//小缩略图
			SizesDropDownList.Items.Clear();
			ListItem item = new ListItem("-请选择大小规格- ", "");
			SizesDropDownList.Items.Add(item);
			List<ThumbnailConfig> configList = GetAllThumbnailConfigs();
			foreach (ThumbnailConfig config in configList)
			{
				item = new ListItem(config.Name, config.Tag);
				SizesDropDownList.Items.Add(item);
			}

			SizesDropDownList.SelectedIndex = 0;
			SizesDropDownList.Items[0].Selected = true;
		}

		#endregion

		#region 图片上传
		/// <summary>
		/// 图片上传
		/// </summary>
		void UploadImageFile()
		{
			if (ImageFileUpload.FileName.Length < 1)
			{
				this.Messages.ShowError("请选择图片文件再上传！");
				return;
			}
			if (!CDHelper.CanUpload(ImageFileUpload.FileName))
			{
				this.Messages.ShowError("系统不支持上传该类型的图片，请在本地转换为gif或jpg格式的再试。");
				return;
			}

			try
			{
				UploadAndCreateThumbnails(Path.GetFileName(ImageFileUpload.FileName));
				Response.Redirect(Request.RawUrl + "&uploaded=" + Path.GetFileName(ImageFileUpload.FileName));
			}
			catch (IOException)
			{
				this.Messages.ShowError("图片上传失败,请重试!");
				return;
			}
		}

		/// <summary>
		/// 获取要上传图片路径
		/// </summary>
		/// <returns></returns>
		public string ExistFilePath()
		{
			return Server.MapPath(VFolderPath);
		}

		private string vFolderPath;
		/// <summary>
		/// 缩略图所在文件夹的相对路径
		/// </summary>
		public string VFolderPath
		{
			get
			{
				if (string.IsNullOrEmpty(vFolderPath))
				{
					Article article = ArticleHelper.GetArticle(ArticleID, null);
					if (!string.IsNullOrEmpty(article.Thumbnail))
					{
						vFolderPath = Server.MapPath(article.Thumbnail.Remove(article.Thumbnail.LastIndexOf("/")));
						if (Directory.Exists(vFolderPath)) vFolderPath = article.Thumbnail.Remove(article.Thumbnail.LastIndexOf("/"));
						else vFolderPath = article.AttachmentUrlPath;
					}
					else vFolderPath = article.AttachmentUrlPath;
				}
				return vFolderPath;
			}
		}

		/// <summary>
		/// 上传原图
		/// </summary>
		/// <param name="fileName"></param>
		void UploadAndCreateThumbnails(string fileName)
		{
			string folderPath = ExistFilePath();
			//判断路径是否存在
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}
			//上传后图片的路径
			string fn = Path.Combine(folderPath, fileName);

			//上传
			ImageFileUpload.SaveAs(fn);

			//文件后缀
			string ext = Path.GetExtension(fn);

			//去掉后缀的文件名
			string imgName = string.Format("{0}", Path.GetFileNameWithoutExtension(fn));

			string originalFilePath = CheckFileName(imgName, ext, folderPath, "");
			//删除原图
			Article article = ArticleHelper.GetArticle(ArticleID, null);

			if (article != null && article.Thumbnail != null && article.Thumbnail != "")
			{
				string thumbnail = article.Thumbnail.Replace("_S", "");
				if (File.Exists(Server.MapPath(thumbnail)))
				{
					File.Delete(Server.MapPath(thumbnail));
				}
			}

			SaveImageToDB(originalFilePath);
		}
		/// <summary>
		/// 保存缩略图源文件名到数据库
		/// </summary>
		/// <param name="thumbnailFile"></param>
		void SaveImageToDB(string thumbnailFile)
		{
			string rootPath = Server.MapPath("/");
			thumbnailFile = thumbnailFile.Remove(0, rootPath.Length);
			thumbnailFile = string.Format("/{0}", thumbnailFile.Replace("\\", "/"));

			Article a = new Article();
			a.ID = ArticleID;
			a.Thumbnail = thumbnailFile;
			a.IsImage = 1;

			if (a.ID == null)
			{
				Article article = ArticleHelper.AddArticles(a);
			}
			else
			{
				string[] fields = new string[] { "Thumbnail", "WapImage", "OriginalImage", "IsImage" };
				ArticleHelper.UpdateArticle(a, fields);
			}
		}

		/// <summary>
		/// 上传图片按钮点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void UploadImage_ServerClick(object sender, EventArgs e)
		{
			UploadImageFile();
		}


		#endregion

		#region 生成缩略图

		/// <summary>
		/// 生成缩略图
		/// </summary>
		/// <param name="originalFilePath"></param>
		/// <param name="ext"></param>
		/// <param name="sn"></param>
		string GenerateThumbImage(string originalFilePath, string ext, string sn)
		{
			string smallCutType = "Cut";
			if (CutTypeDropDownList.SelectedIndex > 0)
				smallCutType = CutTypeDropDownList.SelectedItem.Value; ;
			string type = SizesDropDownList.SelectedValue;
			if (type != "")
			{
				string[] units = SizesDropDownList.SelectedItem.Text.Split(new string[] { ":", "：" }, StringSplitOptions.RemoveEmptyEntries);
				string imageSize = "";
				if (units.Length > 1)
					imageSize = units[1];
				else
					imageSize = units[0];
				string[] izeSplit = imageSize.Split('*');
				int width = int.Parse(izeSplit[0]);
				int height = int.Parse(izeSplit[1]);
				string folderPath = ExistFilePath();
				string thumbnailFilePath = CheckFileName(sn + "_" + type, ext, folderPath, "");
				System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderPath);
				foreach (System.IO.FileInfo fi in dir.GetFiles())
				{
					if (fi.Name.Contains(type))
					{
						fi.Delete();
					}
				}
				if (smallCutType == "CustomerCut")
					GenarateHandTypeThumbImage(originalFilePath, thumbnailFilePath);
				else
					ImageUtils.MakeThumbnail(originalFilePath, thumbnailFilePath, width, height, smallCutType);

				if (AddWatermarkCheckbox.Checked)
					AddWatermarkToImage(thumbnailFilePath, originalFilePath);

				return type;
			}
			else
			{
				Messages.ShowError("缩略图尺寸设置不合法！");
				return "";
			}

		}

		/// <summary>
		/// 获取缩略图文件名
		/// </summary>
		/// <param name="fileName">文件名</param>
		/// <param name="ext">文件扩展名</param>
		/// <param name="folderPath">文件所在文件夹</param>
		/// <param name="imgType">缩略图标识</param>
		/// <returns></returns>
		string CheckFileName(string fileName, string ext, string folderPath, string imgType)
		{
			//文件名重复 重命名缩略图名称
			string newFileName = Path.Combine(folderPath, String.Format("{0}{1}{2}", fileName, imgType, ext));
			return newFileName;
		}

		/// <summary>
		/// 手工裁切模式生成缩略图
		/// </summary>
		/// <param name="originalFilePath"></param>
		/// <param name="thumbFilePath"></param>
		void GenarateHandTypeThumbImage(string originalFilePath, string thumbFilePath)
		{
			int imageWidth = Int32.Parse(txt_width2.Text.Replace("px", ""));
			int imageHeight = Int32.Parse(txt_height2.Text.Replace("px", ""));
			int cutTop = Int32.Parse(txt_top2.Text);
			int cutLeft = Int32.Parse(txt_left2.Text);
			int dropWidth = Int32.Parse(txt_DropWidth2.Text);
			int dropHeight = Int32.Parse(txt_DropHeight2.Text);
			ThumbnailImages imgHelp = new ThumbnailImages();
			imgHelp.GetPart(originalFilePath, thumbFilePath, 0, 0, dropWidth, dropHeight, cutLeft, cutTop, imageWidth, imageHeight);
		}

		/// <summary>
		/// 生成缩略图按钮点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void GenarateButton_ServerClick(object sender, EventArgs e)
		{
			Article a = ArticleHelper.GetArticle(ArticleID);
			string fn = Server.MapPath(a.Thumbnail);
			string ext = Path.GetExtension(fn);
			string path = a.Thumbnail;
			if (String.IsNullOrEmpty(a.Thumbnail) || String.IsNullOrEmpty(a.Thumbnail.Trim()))
			{
				Messages.ShowError("请先上传原始图片");
			}
			else
			{
				string fileName = Path.GetFileNameWithoutExtension(path);

				string ret = GenerateThumbImage(Server.MapPath(a.Thumbnail), ext, fileName);
				if (ret != "")
				{
					Response.Redirect(We7Helper.AddParamToUrl(Request.RawUrl, "generated", ret));
				}
			}
		}
		/// <summary>
		/// 加水印
		/// </summary>
		/// <param name="thumbnailFile"></param>
		/// <param name="originalFilePath"></param>
		void AddWatermarkToImage(string thumbnailFile, string originalFilePath)
		{
			ImageConfig.WaterMarkPicfile = Constants.DataUrlPath + "/watermark/" + config.WaterMarkPic;
			ArticleHelper.AddWatermarkToImage(ImageConfig, thumbnailFile, originalFilePath);
		}
		#endregion

		#region 删除缩略图

		protected void DeleteButton_Click(object sender, EventArgs e)
		{
			string path = IDTextBox.Text.Trim();
			if (path != "")
			{
				File.Delete(Server.MapPath(path));
				Messages.ShowMessage("您成功删除一个缩略图！");
				LoadImages();
				InitOriginalImagePath();
			}
		}


		#endregion

		/// <summary>
		/// 图片信息类
		/// </summary>
		class ImageDetail
		{
			private string name;
			public string Name
			{
				get { return name; }
				set { name = value; }
			}

			private string imagePath;
			public string ImagePath
			{
				get { return imagePath; }
				set { imagePath = value; }
			}

			private string tag;

			public string Tag
			{
				get { return tag; }
				set { tag = value; }
			}

			string fileName;

			public string FileName
			{
				get { return fileName; }
				set { fileName = value; }
			}
		}
	}
}