using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Controls;
using Thinkment.Data;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 友情链接控件数据提供者
    /// </summary>
    public class LinksProvider : BaseWebControl
    {


        /// <summary>
        /// 友情链接列表
        /// </summary>
        public List<Link> Links
        {
            get;
            set;
        }

        private string cssClass;
        /// <summary>
        /// 本控件应用样式
        /// </summary>
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        private string tag = "";
        /// <summary>
        /// 标签参数（默认为""）
        /// </summary>
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        private string defaultLink="选择链接";
        /// <summary>
        /// 默认链接
        /// </summary>
        public string DefaultLink
        {
            get { return defaultLink; }
            set { defaultLink = value; }
        }

        private int columnCount;
        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnCount
        {
            get { return columnCount; }
            set { columnCount = value; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Links = new List<Link>();
            LinkHelper LinkHelper = HelperFactory.GetHelper<LinkHelper>();
            if (tag.Contains(","))
            {
                tag = tag.Substring(0, tag.IndexOf(","));
            }
            if (DesignHelper.IsDesigning)
            {
                List<Link> links;
                DesignHelper.FillItems<Link>(out links, PageSize);
                foreach (Link link in links)
                {
                    link.Thumbnail = DesignHelper.GetTagThumbnail("small");
                }
                Links = links;
            }
            else
            {
                Links = LinkHelper.GetPagedAllLinks(0, PageSize, "", tag);
            }
            if (ColumnCount <= 0)
            {
                columnCount = PageSize;
            }
        }

    }
}
