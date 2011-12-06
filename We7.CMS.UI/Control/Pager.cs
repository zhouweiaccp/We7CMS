using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace We7.CMS.Controls
{
    /// <summary>
    /// 分页控件（服务器类控件，参数使用Viewstate保存）
    /// </summary>
    [Serializable]
    public class Pager : WebControl, INamingContainer
    {
        protected int _padLeft;
        protected int _padRight;

        public Pager()
        {
            PageIndex = 0;
            PageSize = 15;
            RecorderCount = 0;
            NavPageCount = 9;
        }

        public int PageIndex
        {
            get { return (int)ViewState["VS_PAGE_INDEX"]; }
            set { ViewState["VS_PAGE_INDEX"] = value; }
        }

        public int PageSize
        {
            get { return (int)ViewState["VS_PAGE_SIZE"]; }
            set { ViewState["VS_PAGE_SIZE"] = value; }
        }

        public int RecorderCount
        {
            get { return (int)ViewState["VS_RECORDER_COUNT"]; }
            set { ViewState["VS_RECORDER_COUNT"] = value; }
        }

        public int NavPageCount
        {
            get { return (int)ViewState["VS_NAV_PAGE_COUNT"]; }
            set { ViewState["VS_NAV_PAGE_COUNT"] = value; }
        }


        public int PageCount
        {
            get
            {
                int count = RecorderCount / PageSize;
                if (RecorderCount % PageSize > 0)
                {
                    count++;
                }
                return count;
            }
        }

        public int Count
        {
            get { return End - Begin; }
        }

        public int Begin
        {
            get { return PageSize * PageIndex; }
        }

        public int End
        {
            get
            {
                int i = PageSize * (PageIndex + 1);
                return i < RecorderCount ? i : RecorderCount;
            }
        }

        public event EventHandler Fired;

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion

        public void ReRenderPages()
        {
            Controls.Clear();
            //if (PageCount > 0)
            //{
            //    Label lb = new Label();
            //    lb.Text = "共"+PageCount+"页 "+RecorderCount+"条记录";
            //    this.Controls.Add(lb);
            //}
            if (PageIndex > 0)
            {
                LinkButton fb = new LinkButton();
                fb.Text = "<<首页";
                fb.Click += new EventHandler(fb_Click);
                this.Controls.Add(fb);
            }

            CalcPadding(NavPageCount);
            for (int i = Start; i < Stop; i++)
            {
                if (i >= 0 && i < PageCount)
                {
                    string text = (i + 1).ToString();
                    if (i == PageIndex)
                    {
                        Label lbl = new Label();
                        lbl.Text = text;
                        this.Controls.Add(lbl);
                    }
                    else
                    {
                        LinkButton lb = new LinkButton();
                        lb.Text = text;
                        lb.Click += new EventHandler(LinkButoon_Click);
                        this.Controls.Add(lb);
                    }
                }
            }
            if (PageIndex < PageCount - 1)
            {
                LinkButton eb = new LinkButton();
                eb.Text = "尾页>>";
                eb.Click += new EventHandler(eb_Click);
                this.Controls.Add(eb);
            }
        }

        void eb_Click(object sender, EventArgs e)
        {
            PageIndex = PageCount-1;
            //this.PageIndex++;
            //if (PageIndex >= PageCount)
            //{
            //    PageIndex = PageCount - 1;
            //}
            ReRenderPages();
            OnFired();
        }

        void fb_Click(object sender, EventArgs e)
        {
            PageIndex = 0;
            //this.PageIndex--;
            //if (PageIndex < 0)
            //{
            //    PageIndex = 0;
            //}
            ReRenderPages();
            OnFired();
        }

        void Page_Load(object sender, System.EventArgs e)
        {
            ReRenderPages();
        }

        public void FreshMyself()
        {
            ReRenderPages();
        }

        void OnFired()
        {
            if (Fired != null)
            {
                Fired(this, EventArgs.Empty);
            }
        }

        void LinkButoon_Click(object sender, EventArgs e)
        {
            int idx = Convert.ToInt32(((LinkButton)sender).Text);
            this.PageIndex = idx - 1;

            if (PageIndex < 0)
            {
                PageIndex = 0;
            }
            else if (PageIndex >= PageCount)
            {
                PageIndex = PageCount - 1;
            }
            ReRenderPages();
            OnFired();
        }

        protected void CalcPadding(int displayPages)
        {
            // want even padding if we can have it
            _padLeft = displayPages / 2;
            _padRight = _padLeft;

            // but if PageSize is even, shift current over one slot to the left by reducing _padLeft
            if (displayPages % 2 == 0)
                _padLeft--;
        }

        int Start
        {
            get
            {
                if ((PageIndex - _padLeft) <= 0)
                {
                    // our current index falls inside the padded beginning: underflow
                    _padRight += _padLeft - PageIndex ;
                    _padLeft = PageIndex ;
                }
                else if ((PageIndex + _padRight) > PageCount)
                {
                    // our current index falls inside the padded end: overflow
                    _padLeft += _padRight - (PageCount - PageIndex);
                    _padRight = PageCount - PageIndex;
                }

                int counter = _padLeft;
                int idx = 0;
                while (counter > 0)
                {
                    idx = PageIndex - counter;
                    if (idx >= 0)
                        break;
                    counter--;
                }

                return idx;
                //if (NavPageCount <= PageCount)
                //{
                //    if (PageIndex >= NavPageCount / 2)
                //    {
                //        return PageIndex - (NavPageCount / 2);
                //    }
                //}
                //return 0;
            }
        }

        int Stop
        {
            get
            {
                if ((PageIndex - _padLeft) <= 0)
                {
                    // our current index falls inside the padded beginning: underflow
                    _padRight += _padLeft - PageIndex ;
                    _padLeft = PageIndex ;
                }
                else if ((PageIndex + _padRight) > PageCount)
                {
                    // our current index falls inside the padded end: overflow
                    _padLeft += _padRight - (PageCount - PageIndex);
                    _padRight = PageCount - PageIndex;
                }

                int counter = _padLeft;
                int idx = 0;
                while (counter > 0)
                {
                    idx = PageIndex - counter;
                    if (idx >= 0)
                        break;
                    counter--;
                }

                return idx+NavPageCount;
                //if (NavPageCount <= PageCount)
                //{
                //    if (PageIndex >= NavPageCount / 2)
                //    {
                //        return ((PageIndex + (NavPageCount / 2)) <= PageCount) ? (PageIndex + (NavPageCount / 2)) : PageCount;
                //    }
                //}
                //return PageCount;
            }
        }
    }
}
