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
using We7.Model.Core.Config;
using We7.CMS.Common;
using We7.Framework.Util;
using System.Collections.Generic;
using System.Xml;
using We7.Framework.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin.ContentModel.Controls.Page
{
    public partial class ImageUploadExNew : System.Web.UI.Page
    {
        ImageInfo info = new ImageInfo();

        public string ThumbPath
        {
            get
            {
                return ViewState["_ThumbPath"] as string;
            }
            set
            {
                ViewState["_ThumbPath"] = value;
            }
        }

        public string OrignPath
        {
            get
            {
                return ViewState["_OrignPath"] as string;
            }
            set
            {
                ViewState["_OrignPath"] = value;
            }
        }

        public string ArticleID
        {
            get
            {
                return Request["aid"];
            }
        }

        protected string _ConfigPath;
        /// <summary>
        /// xml文件路径
        /// </summary>
        protected string ConfigPath
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["xml"]))
                {
                    _ConfigPath = Request["xml"];
                }
                else
                {
                    _ConfigPath = "/Config/thumbnailNew.xml";
                }
                return _ConfigPath;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ltlMsg.Text = "";
            if (!IsPostBack)
            {
                ThumbPath = Request["v"];
                OrignPath = Request["v"];
                imgThumb.ImageUrl = String.IsNullOrEmpty(ThumbPath) ? ResolveClientUrl("~/ModelUI/skin/images/nopic.gif") : ThumbPath;
                LoadThumbnailSize();
            }
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

            OrignPath = folder.TrimEnd('/') + "/" + newFileName + ext;
            string physicalpath = Server.MapPath(folder);
            if (!Directory.Exists(physicalpath))
            {
                Directory.CreateDirectory(physicalpath);
            }
            string physicalfilepath = Server.MapPath(OrignPath);
            fuImage.SaveAs(physicalfilepath);

            ThumbPath = folder.TrimEnd('/') + "/" + newFileName + "_thumb" + ext;
            string physicaltargetpath = Server.MapPath(ThumbPath);
            CreateTumbnail(physicalfilepath, physicaltargetpath);
            imgThumb.ImageUrl = ThumbPath;

            
            info.Add(new ImageItem() { Size="", Type="", Src=OrignPath});
            ThumbnailConfig cfg=GetThumbnailCfg();
            if(cfg!=null)
            {
                info.Add(new ImageItem() { Size = cfg.Value, Type = cfg.Tag, Src = ThumbPath,Desc=cfg.Name });
            }
            ImageValue.Value = info.ToJson();
        }

        bool ValidateUpload()
        {
            if (String.IsNullOrEmpty(fuImage.FileName))
                return false;
            string ext = Path.GetExtension(fuImage.FileName).Trim('.');
            string[] list = new string[] { "jpg", "jpeg", "gif", "png", "bmp" };
            return We7.Framework.Util.Utils.InArray(ext.ToLower(), list);
        }


        void CreateTumbnail(string op, string tp)
        {
            int width, height;
            GetSize(out width, out height);

            if (width == 0 || height == 0)
                return;

            width = width > 0 ? width : 200;
            height = height > 0 ? height : 150;
            string ext = Path.GetExtension(op).Trim('.').ToLower();
            if (ext == "jpg" || ext == "jpeg")
            {
                ImageUtils.MakeThumbnail(op, tp, width, height, ddlThumbType.SelectedValue);
            }
            else
            {
                ImageUtils.MakeThumbnail(op, tp, width, height, ddlThumbType.SelectedValue, "");
            }
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
            return article.AttachmentUrlPath.TrimEnd("/".ToCharArray()) + "/Thumbnail";
        }

        string CreateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random((int)DateTime.Now.Ticks).Next();
        }

        public void LoadThumbnailSize()
        {
            List<ThumbnailConfig> configList = GetAllThumbnailConfigs();
            ddlSize.DataSource = configList;
            ddlSize.DataTextField = "Name";
            ddlSize.DataValueField = "Tag";
            ddlSize.DataBind();
        }

        public void GetSize(out int width, out int height)
        {
            width = height = 0;
            ThumbnailConfig cfg = GetThumbnailCfg();
            if (cfg != null)
            {
                string size = cfg.Value;
                string[] ss = size.Split('*');
                Int32.TryParse(ss[0],out width);
                Int32.TryParse(ss[1],out height);
            }
        }

        public ThumbnailConfig GetThumbnailCfg()
        {
            List<ThumbnailConfig> configList = GetAllThumbnailConfigs();
            return configList.Find(item => item.Tag == ddlSize.SelectedValue);
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
                if (File.Exists(Server.MapPath(ConfigPath)))
                {
                    thumbnailConfigList = new List<ThumbnailConfig>();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(Server.MapPath(ConfigPath));
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

        ChannelHelper ChannelHelper { get { return HelperFactory.Instance.GetHelper<ChannelHelper>(); } }
    }
}
