using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;

using We7.CMS;
using We7;
using We7.CMS.Controls;

using We7.CMS.Common;
using We7.CMS.Common.Enum;
using System.IO;
using We7.Framework;


namespace We7.CMS.WebControls
{
   /// <summary>
   /// 图片展示数据提供着
   /// </summary>
    public class PhotoGallery : ArticleDataProvider
    {
        #region 字段 属性
        private string thumbnailTagSmall;
        /// <summary>
        /// 获取或者设置小缩略图路径
        /// </summary>
        public string ThumbnailTagSmall
        {
            get { return thumbnailTagSmall; }
            set { thumbnailTagSmall = value; }
        }
        private string thumbnailTagBig;
        /// <summary>
        /// 获得或设置大缩略图路径
        /// </summary>
        public string ThumbnailTagBig
        {
            get { return thumbnailTagBig; }
            set { thumbnailTagBig = value; }
        }
        protected Article article;
        private List<Attachment> attachmentList;
        /// <summary>
        /// 获得或设置图片文章的泛型列表
        /// </summary>
        protected List<Attachment> AttachmentList
        {
            get { return attachmentList; }
            set { attachmentList = value; }
        }
        private string thisArticleBigPhoto="";
        /// <summary>
        /// 获取当前文章的大缩略图路径
        /// </summary>
        public string ThisArticleBigPhoto
        {
            get {
                if (ThisArticle != null)
                {
                    thisArticleBigPhoto = this.GetBigPhoto(ThisArticle.Thumbnail);
                }
                return thisArticleBigPhoto; }
        }
        private string thisArticleSmallPhoto="";
        /// <summary>
        /// 获取当前文章的小缩略图路径
        /// </summary>
        public string ThisArticleSmallPhoto
        {
            get {
                if (ThisArticle != null)
                {
                    thisArticleSmallPhoto = this.GetSmallPhoto(ThisArticle.Thumbnail);
                }
                return thisArticleSmallPhoto; }
        }

        private int articlesCount;
        /// <summary>
        /// 获取或设置文章显示的条数
        /// </summary>
        public int ArticlesCount
        {
            get { return articlesCount; }
            set { articlesCount = value; }
        }

        #endregion

        #region 初始化重载OnLoad事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GetlistArticels();
            ArticlesCount = Articles.Count;

        }


        private Article GetlistArticels()
        {
            Article art = this.GetThisArticle();
            attachmentList = art.Attachments;
            return art;
        }
        /// <summary>
        /// 取得大图路径
        /// </summary>
        /// <param name="urlSource">大图的路径字符串</param>
        /// <returns>大图地址</returns>
        public string GetBigPhoto(string urlSource)
        {
            string photoName=null;
            if(DesignHelper.IsDesigning)
            {
                photoName = DesignHelper.GetTagThumbnail(ThumbnailTagBig);
            }
            else
            {
                photoName = ThisArticle.GetTagThumbnail(ThumbnailTagBig);
            }           
            return photoName;
        }

        /// <summary>
        /// 取得小图路径
        /// </summary>
        /// <param name="urlSource">大图的路径字符串</param>
        /// <returns>小图地址</returns>
        public string GetSmallPhoto(string urlSource)
        {
            string photoName = null;
            if (DesignHelper.IsDesigning)
            {
                photoName = DesignHelper.GetTagThumbnail(ThumbnailTagSmall);
            }
            else
            {
                photoName = ThisArticle.GetTagThumbnail(ThumbnailTagSmall);
            }
            return photoName;
        }

        #endregion


    }
}
