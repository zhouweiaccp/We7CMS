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
using We7.Model.Core;
using We7.Model.UI.Controls.cs;
using We7.Framework.Config;
using We7.Framework.Util;
namespace We7.Model.UI.Controls.system.page
{
    public partial class UploadThumbnail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitOriginalImagePath();
            if (!IsPostBack)
            {
                InitControls();
                InitSizeDroplistFromXML();
                BindImgList();
            }
        }
        public int width;
        public int height;
        public string bigHeadImage;
        public string EditorImgName { get; set; }
        public string ArticleID
        {
            get { return Request["ID"]; }
        }
        public string ImagePaths
        {
            get
            {
                Article a = new Article() { ID = ArticleID };

                return a.AttachmentUrlPath + "/thumbnail";
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
        #region 初始化
        void InitControls()
        {
            // UploadImage.Attributes["onclick"] = "return articleImageCheck('" + this.ClientID + "','" + ImagePaths + "');";
            SizesDropDownList.Attributes["onchange"] = "changeFrame(this)";

        }
        /// <summary>
        /// 初始化原图
        /// </summary>
        void InitOriginalImagePath()
        {

            string imgEgitNmame = Request.QueryString["imgEdit"] == null ? "" : Request.QueryString["imgEdit"];
            if (imgEgitNmame != "")
            {
                OriginalImagePath = ImagePaths + "/" + imgEgitNmame;
                localHostPaneUpload.Visible = false;
                imgBackground.ImageUrl = OriginalImagePath;
                ThumbnailImages imageHelp = new ThumbnailImages();
                ImageInformation imageInfo = (ImageInformation)imageHelp.GetImageInfo(Server.MapPath(OriginalImagePath));
                width = imageInfo.Width;
                height = imageInfo.Height;
                Page.ClientScript.RegisterStartupScript(typeof(UploadThumbnail), "Step()", "<script type='text/javascript'>Step();</script>");
            }
        }
        void InitSizeDroplistFromXML()
        {
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

                }
            }
            return thumbnailConfigList;
        }
        public string OriginalImagePath { get; set; }
        /// <summary>
        /// 所处应用的虚拟路径
        /// </summary>
        /// 
        #endregion
        public string AppPath
        {
            get
            {
                return "/admin";
            }
        }
        #region 图片上传
        void UploadImageFile()
        {
            try
            {
                UploadAndCreateThumbnails(Path.GetFileName(ImageFileUpload.FileName));
            }
            catch (IOException)
            {
                //this.Messages.ShowError("图片上传失败,请重试!");
                return;
            }
        }
        #region 生成小缩略图
        /// <summary>
        /// 生成小图
        /// </summary>
        /// <param name="pathabsolute"></param>
        protected void SetThumbnail(string pathabsolute, string SizeType, int newWidth, int newHeight)
        {

            string fileName = Path.GetFileName(pathabsolute);
            string smallFilename = fileName.Insert(fileName.LastIndexOf("."), SizeType);
            string newPath = Server.MapPath(ImagePaths + "/" + smallFilename);
            ImageUtils.MakeThumbnail(pathabsolute, newPath, newWidth, newHeight, "HW");

        }
        protected void GenerateThumbImage(string pathabsolute)
        {   //生成默认缩略小图
            SetThumbnail(pathabsolute, "_Min", 30, 30);
        }
        #endregion
        /// <summary>
        /// 找出缩略图对应尺寸
        /// </summary>
        /// <returns></returns>
        protected string GetThumbnailConfigsByNmame(string str)
        {
            string value = string.Empty;
            if (File.Exists(Server.MapPath("/Config/thumbnail.xml")))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Server.MapPath("/Config/thumbnail.xml"));
                XmlNodeList ItemListNodes = doc.SelectNodes("/configuration/item");

                foreach (XmlNode oldNode in ItemListNodes)
                {
                    if (oldNode.Attributes["value"].Value == str)
                    {
                        string[] v = oldNode.Attributes["name"].Value.Split(new string[] { ":", "：" }, StringSplitOptions.RemoveEmptyEntries);
                        if (v.Length > 1)
                            value = v[1];
                    }
                }
            }
            return value;
        }
        protected bool GetThumbnailConfigsByNmameExt(string str)
        {
            bool Exist = false;
            if (File.Exists(Server.MapPath("/Config/thumbnail.xml")))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Server.MapPath("/Config/thumbnail.xml"));
                XmlNodeList ItemListNodes = doc.SelectNodes("/configuration/item");

                foreach (XmlNode oldNode in ItemListNodes)
                {
                    int s = str.IndexOf(oldNode.Attributes["value"].Value);
                    if (s > 0)
                    {
                        Exist = true;
                    }
                }
            }
            return Exist;
        }
        public string ExistFilePath()
        {
            Article a = new Article() { ID = ArticleID };
            string folderPath = a.AttachmentUrlPath + "/thumbnail/";
            FileHelp.IsDirExists(folderPath);
            return Server.MapPath(folderPath);
        }

        /// <summary>
        /// 上传原图
        /// </summary>
        /// <param name="fileName"></param>
        void UploadAndCreateThumbnails(string fileName)
        {
            string folderPath = ExistFilePath();
            //上传后图片的路径
            string fn = Path.Combine(folderPath, fileName);
            //上传
            ImageFileUpload.SaveAs(fn);
            //生成小图片
            GenerateThumbImage(fn);
        }


        protected void UploadImage_ServerClick(object sender, EventArgs e)
        {
            UploadImageFile();
            BindImgList();
        }
        #endregion

        #region 裁剪图片
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalFilePath"></param>
        /// <param name="ext"></param>
        /// <param name="sn"></param>
        string GenerateThumbImage(string originalFilePath, string ext, string sn)
        {
            InitOriginalImagePath();
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
                {
                    GenarateHandTypeThumbImage(originalFilePath, thumbnailFilePath);
                    GenerateThumbImage(thumbnailFilePath);
                }
                else
                {
                    ImageUtils.MakeThumbnail(originalFilePath, thumbnailFilePath, width, height, smallCutType);
                }

                if (AddWatermarkCheckbox.Checked)
                    AddWatermarkToImage(thumbnailFilePath, originalFilePath);

                return type;
            }
            else
            {
                // Messages.ShowError("缩略图尺寸设置不合法！");
                return "";
            }

        }

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
            int cutTop = Int32.Parse(Request.Form["txtTop"]);
            int cutLeft = Int32.Parse(Request.Form["txtLeft"]);
            int dropWidth = Int32.Parse(Request.Form["txtWidth"]);
            int dropHeight = Int32.Parse(Request.Form["txtHeight"]);
            ThumbnailImages imgHelp = new ThumbnailImages();
            imgHelp.GetPart(originalFilePath, thumbFilePath, 0, 0, dropWidth, dropHeight, cutLeft, cutTop, width, height);
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
        protected void LinkButtonlocalHost_Click(object sender, EventArgs e)
        {
            string url = Request.RawUrl;
            string imgEgit = Request.QueryString["imgEdit"] == null ? "" : Request.QueryString["imgEdit"];
            if (imgEgit != "")
            {
                int s = Request.RawUrl.IndexOf("&imgEdit");
                if (s > 0)
                {
                    url = Request.RawUrl.Remove(s);
                }
            }
            Response.Redirect(url);
        }
        /// <summary>
        /// 取图片原名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetImgEditNmae(string str)
        {
            return Path.GetFileName(str.Replace(We7.Model.Core.UI.Constants.SHOWTHUMB, ""));
        }
        /// <summary>
        /// 绑定指定目录下的文件到ReptImgList
        /// </summary>
        /// <param name="directory"></param>
        protected void BindImgList()
        {
            ArrayList list = new ArrayList();
            #region 读取文件
            DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(ImagePaths));
            try
            {
                foreach (FileInfo f in di.GetFiles())
                {
                    if (!GetThumbnailConfigsByNmameExt(f.Name))
                    {
                        string pathrelatively = ImagePaths + "/" + f.Name;
                        list.Add(pathrelatively);
                    }
                }
            }
            catch
            {
                //对应目录下没有文件
            }
            #endregion
            ImagesRepeater.DataSource = list;
            ImagesRepeater.DataBind();
        }

        protected void GenerateButton_Click(object sender, EventArgs e)
        {
            InitOriginalImagePath();
            string path = imgBackground.ImageUrl;
            string fn = Server.MapPath(path);
            string ext = Path.GetExtension(fn);
            string fileName = path.Substring(path.LastIndexOf("/") + 1, path.LastIndexOf(".") - path.LastIndexOf("/") - 1);
            string ret = GenerateThumbImage(fn, ext, fileName);
            Page.ClientScript.RegisterStartupScript(typeof(UploadThumbnail), "Ok()", "<script type='text/javascript'>Ok();</script>");
        }
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
