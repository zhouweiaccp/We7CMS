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
using We7.Framework.Cache;

namespace We7.CMS.Web.Admin.manage
{
    public partial class CacheRefresh : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                chkItem.DataSource = CacheConfig.Instance.Labels;
                chkItem.DataTextField = "value";
                chkItem.DataValueField = "key";
                chkItem.DataBind();
            }
        }

        protected void bttnGenerate_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in chkItem.Items)
            {
                CacheRecord.Create(item.Value).Release();
            }
            Messages.ShowMessage("缓存更新成功");
        }
    }
}
