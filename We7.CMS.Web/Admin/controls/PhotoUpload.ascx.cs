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
using We7.Framework.Util;
using System.Text;

namespace We7.CMS.Web.Admin.ChannelModule
{
    public partial class PhotoUpload : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitSizeDroplistFromXML();
                BindData();
            }
        }

        protected void dlstPhotos_ItemCommand(object source, DataListCommandEventArgs e)
        {
            int row = e.Item.ItemIndex;
            Article article = ArticleHelper.GetArticle(ArticleID);
            if (!String.IsNullOrEmpty(article.Photos))
            {
                StringBuilder sb = new StringBuilder();
                string[] ss = article.Photos.Split('|');
                for (int i = 0; i < ss.Length; i++)
                {
                    if (i == row)
                    {
                        string s = ss[i];
                        if (File.Exists(Server.MapPath(s)))
                        {
                            File.Delete(Server.MapPath(s));
                        }
                        s = s.Substring(0, s.LastIndexOf(".")) + "_S" + s.Substring(s.LastIndexOf("."));
                        if (File.Exists(Server.MapPath(s)))
                        {
                            File.Delete(Server.MapPath(s));
                        }
                        continue;
                    }
                    sb.Append(ss[i]).Append("|");
                }
                Utils.TrimEndStringBuilder(sb, "|");
                article.Photos = sb.ToString();
                ArticleHelper.UpdateArticle(article, new string[] { "Photos" });
                BindData();
            }
        }

        protected void SaveButton_ServerClick(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(filePhotot.FileName))
            {
                Messages.ShowError("请选择上传图片");
                return;
            }
            if (String.IsNullOrEmpty(ddlSize.SelectedValue))
            {
                Messages.ShowError("请选择图片改寸");
                return;
            }
            UploadAndCreateThumbnails();
            BindData();
            HttpCookie cookie = new HttpCookie("Photo_Size", ddlSize.SelectedValue);
            cookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(cookie);
        }

        protected void BindData()
        {
            Article article = ArticleHelper.GetArticle(ArticleID);
            if (!String.IsNullOrEmpty(article.Photos))
            {
                string[] ss = article.Photos.Split('|');
                for (int i = 0; i < ss.Length; i++)
                {
                    string s = ss[i];
                    ss[i] = s.Substring(0, s.LastIndexOf(".")) + "_S" + s.Substring(s.LastIndexOf("."));
                }
                dlstPhotos.DataSource = ss;
                dlstPhotos.DataBind();
            }
            else
            {
                dlstPhotos.DataSource = null;
            }
            dlstPhotos.DataBind();
        }

        void InitSizeDroplistFromXML()
        {
            ddlSize.DataSource = GetAllThumbnailConfigs();
            ddlSize.DataTextField = "Name";
            ddlSize.DataValueField = "Value";
            ddlSize.DataBind();

            ddlSize.Items.Insert(0, new ListItem("-请选择大小规格- ", ""));

            if (Request.Cookies["Photo_Size"] != null && !String.IsNullOrEmpty(Request.Cookies["Photo_Size"].Value))
            {
                ddlSize.SelectedValue = Request.Cookies["Photo_Size"].Value;
            }
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
                    HelperFactory.Instance.GetHelper<ChannelHelper>().CacherCache("$THUMBNAILCONFIGLIST", Context, thumbnailConfigList, CacheTime.Short);
                }
            }
            return thumbnailConfigList;
        }

        public string ExistFilePath()
        {
            Article article = ArticleHelper.GetArticle(ArticleID, null);
            string folderPath = article.AttachmentUrlPath + "/thumbnail/";
            return Server.MapPath(folderPath);
        }

        public string ArticleID
        {
            get { return Request["id"]; }
        }

        /// <summary>
        /// 上传原图
        /// </summary>
        /// <param name="fileName"></param>
        void UploadAndCreateThumbnails()
        {
            try
            {
                Article article = ArticleHelper.GetArticle(ArticleID, null);
                string relPath = article.AttachmentUrlPath + "/thumbnail/";

                string folderPath = Server.MapPath(relPath);
                //判断路径是否存在
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string ext = Path.GetExtension(filePhotot.FileName);
                string imgName = DateTime.Now.Ticks.ToString();
                string fileName = DateTime.Now.Ticks.ToString() + ext;
                //上传后图片的路径
                string fn = Path.Combine(folderPath,fileName);

                //上传
                filePhotot.SaveAs(fn);
                string thumbnailFilePath = Path.Combine(folderPath, imgName + "_S" + ext);

                string imageSize = ddlSize.SelectedValue;
                string[] izeSplit = imageSize.Split('*');
                int width = int.Parse(izeSplit[0]);
                int height = int.Parse(izeSplit[1]);

                if (File.Exists(thumbnailFilePath))
                    File.Delete(thumbnailFilePath);

                ImageUtils.MakeThumbnail(fn, thumbnailFilePath, width, height, "HW");

                relPath = relPath + fileName;

                article.Photos = !String.IsNullOrEmpty(article.Photos) ? (article.Photos + "|" + relPath) : relPath;

                ArticleHelper.UpdateArticle(article, new string[] { "Photos" });
            }
            catch (Exception ex)
            {
                Messages.ShowError(ex.Message);
            }
        }
    }
}