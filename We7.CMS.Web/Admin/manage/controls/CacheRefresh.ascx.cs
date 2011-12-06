using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework.Cache;

namespace We7.CMS.Web.Admin.manage.controls
{
    public partial class CacheRefresh : System.Web.UI.UserControl
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