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
using We7.Model.Core;
using We7.Model.UI.Data;
using We7.Model.Core.Data;

namespace We7.CMS.Web.Admin.ContentModel
{
    public partial class ModelTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bttnGenerate_Click(object sender, EventArgs e)
        {
            ModelInfo info = ModelHelper.GetModelInfo(txtModelName.Text.Trim());
            if (info != null)
            {
                DataBaseHelperFactory.Create().CreateTable(info);
            }
            else
            {
                Response.Write("当前模型文件不存在");
            }
        }
    }
}
