using System;
using System.Collections.Generic;
using System.Text;
using Thinkment.Data;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.Xml.Serialization;
using We7.Framework;
using We7.Framework.Util;
using We7.Framework.TemplateEnginer;
using We7.CMS.WebControls.Core;
using We7.Model.Core.Data;
using System.Text.RegularExpressions;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 控件分页器
    /// </summary>
    [Serializable]
    public class ControlPager
    {
        #region 分页参数
        /// <summary>
        /// 每一页显示的记录数
        /// </summary>
        [Parameter(Title = "控件每页记录", Type = "Number", DefaultValue = "10")]
        public int PageSize { get; set; }

        private int pageIndex;
        /// <summary>
        /// 当前的页码
        /// </summary>
        public int PageIndex
        {
            get
            {

                try
                {
                    string sp = HttpContext.Current.Request != null ? HttpContext.Current.Request[RequestPageIndex] : "1";
                    pageIndex = String.IsNullOrEmpty(sp) ? 1 : Convert.ToInt32(sp);
                }
                catch { pageIndex = 1; }
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        /// <summary>
        /// 记录总条数
        /// </summary>
        public int RecordCount { get; set; }

        private int pageCount;
        /// <summary>
        /// 总共页数
        /// </summary>
        public int PageCount
        {
            get
            {
                pageCount = RecordCount / PageSize;
                pageCount = RecordCount % PageSize == 0 ? pageCount : (pageCount + 1);
                return pageCount;
            }
            set
            {
                pageCount = value;
            }
        }

        /// <summary>
        ///　分页开始的条目
        /// </summary>
        public int StartItem
        {
            get
            {

                int start = (PageIndex - 1) * PageSize;
                return start < 0 ? 0 : start;
            }
        }

        /// <summary>
        ///  分页结束的条目
        /// </summary>
        public int EndItem
        {
            get
            {
                int end = PageIndex * PageSize - 1;
                return end >= RecordCount ? (RecordCount - 1) : end;
            }
        }

        /// <summary>
        /// 当前页面的记录数
        /// </summary>
        public int PageItemsCount
        {
            get
            {
                int count = EndItem - StartItem + 1;
                return count > RecordCount ? 0 : count;
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private int PrevPage
        {
            get
            {
                if (PageIndex <= 1)
                    return 1;
                return PageIndex - 1;
            }
        }

        /// <summary>
        /// 前几页码集合
        /// </summary>
        private List<int> BeforePages
        {
            get
            {
                List<int> list = new List<int>();
                if (PageIndex > 1)
                {
                    int j = 0;
                    for (int i = 1; i < PageIndex && j < 5; i++, j++)
                        list.Add(i);
                }
                return list;
            }
        }

        /// <summary>
        /// 后几页码集合
        /// </summary>
        private List<int> BehindPages
        {
            get
            {
                List<int> list = new List<int>();
                if (PageIndex < PageCount)
                {
                    int j = 0;
                    for (int i = PageIndex + 1; i <= PageCount && j < 5; i++, j++)
                        list.Add(i);
                }
                return list;
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private int NextPage
        {
            get
            {
                if (PageIndex >= PageCount)
                    return PageCount;
                return PageIndex + 1;
            }
        }

        #endregion

        #region 其他参数
        /// <summary>
        /// 当前页号Request参数名称
        /// </summary>
        [Parameter(Title = "当前页号Request参数名称", Type = "String", DefaultValue = "pi")]
        public string RequestPageIndex { get; set; }
        /// <summary>
        /// 分页控件整体样式类名称
        /// </summary>
        [Parameter(Title = "分页控件整体样式类名称", Type = "String", DefaultValue = "page_css page_line")]
        public string PagerDivClass { get; set; }
        /// <summary>
        /// 分页控件外包span样式类名称
        /// </summary>
        [Parameter(Title = "分页控件外包span样式类名称", Type = "String", DefaultValue = "pagecss")]
        public string PagerSpanClass { get; set; }

        /// <summary>
        /// 分页样式模板文件名(/Widgets/Skin/vm/pager.vm)
        /// </summary>
        //[Parameter(Title = "分页样式模板文件名", Type = "Fields", Data = "简单分页|/Widgets/Skin/vm/pager.vm")]
        [Parameter(Title = "分页样式模板文件名", Type = "String", DefaultValue = "/Widgets/WidgetCollection/文章列表类/PagedArticleList.Default/vm/pager.vm")]
        public string VmTemplateFileName { get; set; }
        #endregion        

        /// <summary>
        /// 输出分页器html代码
        /// </summary>
        public string PagedHtml
        {
            get
            {
                if (RecordCount <= 0)
                    return "没有获取到数据记录数！<br />";

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("PageSize", PageSize);
                dic.Add("PageIndex", PageIndex);
                dic.Add("PageCount", PageCount);
                dic.Add("RecordCount", RecordCount);
                dic.Add("RequestPageIndex", RequestPageIndex);
                dic.Add("PrevPage", PrevPage);
                dic.Add("NextPage", NextPage);
                dic.Add("BeforePages", BeforePages);
                dic.Add("BehindPages", BehindPages);
                dic.Add("PagerDivClass", PagerDivClass.Replace("&nbsp;", " "));
                dic.Add("PagerSpanClass", PagerSpanClass.Replace("&nbsp;", " "));

                Regex reg = new Regex("[?&]" + RequestPageIndex + @"=\d+");
                string CurrPage = reg.Replace(HttpContext.Current.Request.RawUrl, "");
                dic.Add("CurrPage", CurrPage);

                dic.Add("Symbol", CurrPage.IndexOf("?") > -1 ? "&" : "?"); //连接符

                string path = HttpContext.Current.Server.MapPath(VmTemplateFileName);
                FileInfo fi = new FileInfo(path);
                
                return NVelocityHelper.GetFormatString(fi.Directory.FullName, fi.Name, dic);
            }
        }
    }
}
