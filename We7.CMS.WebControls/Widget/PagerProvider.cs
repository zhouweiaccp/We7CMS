using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using We7.Framework.Util;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// 分页数据提供者
    /// </summary>
    public class PagerProvider:BaseWebControl
    {
        private IPager Pager;

        /// <summary>
        /// 相关控件
        /// </summary>
        public string RelatedID { get; set; }

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitPager();
        }

        /// <summary>
        /// 初始化分页控件
        /// </summary>
        void InitPager()
        {            
            if (DesignHelper.IsDesigning)
            {
                Pager = new BaseWebControl() { PageSize = 10, PageIndex = 2, RecordCount = 100 };
            }
            else
            {
                if (String.IsNullOrEmpty(RelatedID))
                    throw new Exception("RelatedID不能为空");

                Pager = Utils.FindControl(RelatedID, Page) as IPager;
                                
            }
            base.IncludeJavaScript("Pager.js");            
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="pager"></param>
        public new void Load(IPager pager)
        {
            this.Pager = pager;
        }

        private int numberCount = 4;

        /// <summary>
        /// 当前页码
        /// </summary>
        public new int PageIndex
        {
            get
            {
                return Pager == null ? 0 : Pager.PageIndex;
            }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public new int RecordCount
        {
            get
            {
                return Pager == null ? 0 : Pager.RecordCount;
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public new int PageCount
        {
            get
            {
                return Pager == null ? 0 : Pager.PageCount;
            }
        }

        /// <summary>
        /// 每一页的显示条数
        /// </summary>
        public new int PageSize
        {
            get
            {
                return Pager == null ? 0 : Pager.PageSize;
            }
        }

        /// <summary>
        /// 开始页
        /// </summary>
        public int Start
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// 结束页
        /// </summary>
        public int End
        {
            get
            {
                return PageCount;
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public int Previous
        {
            get
            {
                return PageIndex > 1 ? PageIndex - 1 : 1;
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public int Next
        {
            get
            {
                return PageIndex < PageCount ? PageIndex + 1 : PageCount;
            }
        }
        private int fixedLength=10;
        /// <summary>
        /// 固定显示数字记录
        /// </summary>
        public int FixedLength
        {
            get { return fixedLength; }
            set { fixedLength = value; }
        }

        /// <summary>
        /// 开始页
        /// </summary>
        public int BeginNumber
        {
            get
            {
                if (!Fixed)
                {
                    return (PageIndex - numberCount) > 1 ? (PageIndex - numberCount) : 1;
                }
                else
                {
                    int begin = PageIndex % FixedLength == 0 ? (PageIndex / FixedLength - 1) * FixedLength + 1 : (PageIndex / FixedLength) * FixedLength + 1;
                    return begin < 1 ? 1 : begin;
                }

            }
        }
        /// <summary>
        /// 结束页
        /// </summary>
        public int LastNumber
        {
            get
            {
                if (!Fixed)
                {
                    return (PageIndex + numberCount) < PageCount ? (PageIndex + numberCount) : PageCount;
                }
                else
                {
                    int last = BeginNumber + FixedLength-1;
                    return last > PageCount ? PageCount : last;
                }
            }
        }
        /// <summary>
        /// 是否显示GO
        /// </summary>
        public bool ShowGo { get; set; }

        /// <summary>
        /// 是否显示下拉框
        /// </summary>
        public bool ShowNav { get; set; }

        /// <summary>
        /// 是否显示记录信息
        /// </summary>
        public bool ShowMessage { get; set; }

        /// <summary>
        /// 是否显示数字导航
        /// </summary>
        public bool ShowNumNav { get; set; }

        /// <summary>
        /// Css
        /// </summary>
        public string CssClass { get; set; }
        
        /// <summary>
        /// 是否固定显示
        /// </summary>
        public bool  Fixed { get; set; }

        public string startLabel = "首页";
        /// <summary>
        /// 首页的显示文字
        /// </summary>
        public string StartLabel
        {
            get { return startLabel; }
            set { startLabel = value; }
        }

        public string preLabel = "上一页";
        /// <summary>
        /// 上一页显示文字
        /// </summary>
        public string PreLabel
        {
            get { return preLabel; }
            set { preLabel = value; }
        }

        public string nextLabel = "下一页";
        /// <summary>
        /// 下一页显示文字
        /// </summary>
        public string NextLabel
        {
            get { return nextLabel; }
            set { nextLabel = value; }
        }

        public string endLabel = "尾页";
        /// <summary>
        /// 尾页显示文字
        /// </summary>
        public string EndLabel
        {
            get { return endLabel; }
            set { endLabel = value; }
        }

        /// <summary>
        /// 显示GO
        /// </summary>
        protected string DisplayGO
        {
            get
            {
                return ShowGo ? "" : "none";
            }
        }

        /// <summary>
        /// 显示Nav
        /// </summary>
        protected string DisplayNav
        {
            get
            {
                return ShowNav ? "" : "none";
            }
        }

        /// <summary>
        /// 显示Message
        /// </summary>
        protected string DisplayMessage
        {
            get
            {
                return ShowMessage ? "" : "none";
            }
        }

        /// <summary>
        /// 显示NumNav
        /// </summary>
        protected string DisplayNumNav
        {
            get
            {
                return ShowNumNav ? "" : "none";
            }
        }
    }
}
