using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using We7.Model.Core.UI;
using System.IO;
using System.Xml;
using We7.CMS.Common;
using We7.Model.Core;
namespace We7.Model.UI.Controls.system
{
    public partial class Thumbnail : We7FieldControl
    {
        public override void InitControl()
        {

            TxtPath.Text = Value == null ? Control.DefaultValue : Value.ToString();
            TxtPath.CssClass = Control.CssClass;

            if (!String.IsNullOrEmpty(Control.Width))
            {
                TxtPath.Width = Unit.Parse(Control.Width);
            }
            if (!String.IsNullOrEmpty(Control.Height))
            {
                TxtPath.Height = Unit.Parse(Control.Height);
            }
            //Article a = ArticleHelper.GetArticle(ArticleID, null);
            Article a = new Article() { ID = ArticleID };
            DialogPath = a.AttachmentUrlPath + "/thumbnail";
            DialogUrl = ResolveUrl("~/ModelUI/Controls/system/page/UploadThumbnail.aspx");
            //显示已经有的图片
            BindImgList();
            // BindSizesDropDownList();
        }
        public override object GetValue()
        {
            return TxtPath.Text;
        }
        /// <summary>
        /// 文章附件目录相对路径
        /// </summary>
        public string DialogPath { get; set; }

        public string DialogUrl { get; set; }
        /// <summary>
        /// 绑定指定目录下的文件到ReptImgList
        /// </summary>
        /// <param name="directory"></param>
        protected void BindImgList()
        {
            string strHtml = We7.Model.UI.Controls.system.page.ShowImgList.BindImgList(DialogPath);
            ImagesLiteralList.Text = strHtml.ToString();
        }
    }
} 





