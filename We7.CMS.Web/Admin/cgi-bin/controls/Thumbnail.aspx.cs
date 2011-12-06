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
using We7.CMS.Common;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Config;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin.cgi_bin.controls
{
    public partial class Thumbnail : System.Web.UI.Page
    {
        //这里可以设置上传设置文件类型。
        private string[] fileTypes = new string[] { "jpg","gif", "jpeg","png","bmp" };
        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            
            get { return (HelperFactory)Application[HelperFactory.ApplicationID];}
        }
        /// <summary>
        /// 栏目业务对象
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        private int _width;
        public int width
        {
            get 
            {
                if(ViewState["____Width"]==null)
                    ViewState["____Width"]=0;
                return (int)ViewState["____Width"];
            }
            set
            {
                ViewState["____Width"] = value;
            }
        }
        public int height
        {
            get
            {
                if (ViewState["____Height"] == null)
                    ViewState["____Height"] = 0;
                return (int)ViewState["____Height"];
            }
            set
            {
                ViewState["____Height"] = value;
            }
        }
        public string bigHeadImage;

        private string folder;
        public string ThumbnailFolder
        {
            get
            {
                if (String.IsNullOrEmpty(folder))
                {
                    folder = String.IsNullOrEmpty(Request["folder"]) ? "/_data/UploadFile/" : Request["folder"];
                    if(!folder.StartsWith("/_data/",StringComparison.CurrentCultureIgnoreCase))
                        folder = "/_data/" + folder.Trim('/').Trim('\\');
                }
                return folder;
            }
        }

        GeneralConfigInfo config;
        public GeneralConfigInfo ImageConfig
        {
            get
            {
                if (config == null)
                    config = GeneralConfigs.GetConfig();
                return config;
            }
        }

        public string OriginalImagePath
        {
            get
            {
                if (ViewState["OriginalImagePath"] == null)
                {
                    ViewState["OriginalImagePath"] = "/admin/images/article_small.gif";
                }
                return ViewState["OriginalImagePath"] as string;
            }
            set
            {
                ViewState["OriginalImagePath"] = value;
            }
        }

        public string ThumbnailPath
        {
            get
            {
                if (ViewState["ThumbnailPath"] == null)
                {
                    ViewState["ThumbnailPath"] = "/admin/images/article_small.gif";
                }
                return ViewState["ThumbnailPath"] as string;
            }
            set
            {
                ViewState["ThumbnailPath"] = value;
            }
        }

        public string ThumbnailServerFolder
        {
            get
            {
                string path = Server.MapPath(ThumbnailFolder);
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

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
                //LoadImages();
            }
            Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }


        void InitControls()
        {
            //UploadImage.Attributes["onclick"] = "return articleImageCheck('" + this.ClientID + "','" + ImagePaths + "');";
            SizesDropDownList.Attributes["onchange"] = "changeFrame(this)";
            SizesDropDownList.Attributes["onblur"] = "saveImageSizeToCookies(this)";
        }

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
        /// 初始化原图
        /// </summary>
        void InitOriginalImagePath()
        {
            InitImageInfo();
        }

        void InitImageInfo()
        {
            ThumbnailImages imageHelp = new ThumbnailImages();
            ImageInformation imageInfo = (ImageInformation)imageHelp.GetImageInfo(Server.MapPath(OriginalImagePath));
            width = imageInfo.Width;
            height = imageInfo.Height;
        }

        protected void SaveUploadImage(object sender, EventArgs args)
        {
            string photoName1 = ImageFileUpload.FileName;
            if (ImageFileUpload.FileName.Length < 1)
            {
                this.Messages.ShowError("请选择图片文件再上传！");
                return;
            }

            int lastIndex = photoName1.LastIndexOf("."); //取得文件名中最后一个"."的索引

            string newext = photoName1.Substring(lastIndex); //获取文件扩展名
         
            //上传文件是否正确
            int count = 0;
            foreach (string typeName in fileTypes)
            {
                
                if (newext == typeName.ToLower())
                {
                    count = 1;
                }
            }
            if (count == 0)
            {
                Messages.ShowMessage("您上传的文件类型不正确!");
                return;
            }
            //图片的大小
            int ImgSize = ImageFileUpload.PostedFile.ContentLength;//此处取得的文件大小的单位是byte
            if (ImgSize / 1024 < 1024)//转换为kb
            {
                try
                {
                    UploadAndCreateThumbnails(Path.GetFileName(ImageFileUpload.FileName));
                }
                catch (IOException)
                {
                    this.Messages.ShowError("图片上传失败,请重试!");
                    return;
                }
            }
            else
            {
                this.Messages.ShowError("图片大小不能超过1M！");
            }
           
        }

        /// <summary>
        /// 上传原图
        /// </summary>
        /// <param name="fileName"></param>
        void UploadAndCreateThumbnails(string fileName)
        {
            //上传后图片的路径
            string fn = Path.Combine(ThumbnailServerFolder, fileName);

            //上传
            ImageFileUpload.SaveAs(fn);

            OriginalImagePath = ThumbnailFolder.TrimEnd('\\').TrimEnd('/') + "/" + fileName;
            InitImageInfo();

            ////文件后缀
            //string ext = Path.GetExtension(fn);

            ////去掉后缀的文件名
            //string imgName = string.Format("{0}", Path.GetFileNameWithoutExtension(fn));
        }

        protected void GenarateButton_ServerClick(object sender, EventArgs e)
        {
            ThumbnailPath = GenerateThumbImage(Server.MapPath(OriginalImagePath), Path.GetExtension(OriginalImagePath), Path.GetFileNameWithoutExtension(OriginalImagePath));
        }

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
                string folderPath = ThumbnailServerFolder;
                string thumbnailFilePath = CheckFileName(sn + "_" + type, ext, folderPath, "");
                //DirectoryInfo dir = new DirectoryInfo(folderPath);
                //foreach (FileInfo fi in dir.GetFiles())
                //{
                //    if (fi.Name.Contains(type))
                //    {
                //        fi.Delete();
                //    }
                //}
                if (File.Exists(thumbnailFilePath))
                    File.Delete(thumbnailFilePath);
                if (smallCutType == "CustomerCut")
                    GenarateHandTypeThumbImage(originalFilePath, thumbnailFilePath);
                else
                    ImageUtils.MakeThumbnail(originalFilePath, thumbnailFilePath, width, height, smallCutType);

                if (AddWatermarkCheckbox.Checked)
                    AddWatermarkToImage(thumbnailFilePath, originalFilePath);

                return GetThumbnailName(sn + "_" + type, ext, ThumbnailFolder, "");
            }
            else
            {
                Messages.ShowError("缩略图尺寸设置不合法！");
                return "";
            }
        }

        string CheckFileName(string fileName, string ext, string folderPath, string imgType)
        {
            //文件名重复 重命名缩略图名称
            string newFileName = Path.Combine(folderPath, String.Format("{0}{1}{2}", fileName, imgType, ext));
            return newFileName;
        }

        string GetThumbnailName(string fileName, string ext, string folderPath, string imgType)
        {
            return folderPath + String.Format("/{0}{1}{2}", fileName, imgType, ext);
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
        /// 加水印
        /// </summary>
        /// <param name="thumbnailFile"></param>
        /// <param name="originalFilePath"></param>
        void AddWatermarkToImage(string thumbnailFile, string originalFilePath)
        {
            ImageConfig.WaterMarkPicfile = Constants.DataUrlPath + "/watermark/" + config.WaterMarkPic;
            ArticleHelper.AddWatermarkToImage(ImageConfig, thumbnailFile, originalFilePath);
        }
    }
}
