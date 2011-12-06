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
using We7.Model.Core.UI;
using We7.CMS;
using Wuqi.Webdiyer;

namespace We7.Model.UI.Container.we7
{
    public partial class We7Pager : PagerContainer
    {
        protected override void LoadContainer()
        {
            Pager.RecordCount = RecordCount;
            Pager.PageSize = PanelContext.PageSize;
        }

        public override int RecordCount
        {
            get
            {
                return Pager.RecordCount;
            }
            set
            {
                Pager.RecordCount = value;
            }
        }

        public override int CurrentPageIndex
        {
            get
            {
                return Pager.CurrentPageIndex;
            }
            set
            {
                Pager.CurrentPageIndex = value;
            }
        }

        protected void Pager_PageChanging(object src, PageChangingEventArgs e)
        {
            DoCommand(src, "pageindex", e.NewPageIndex);
        }
    }
}