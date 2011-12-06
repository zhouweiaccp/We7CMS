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
using We7.Model.Core.Config;
using We7.Model.Core;
using System.IO;
using System.Text;

namespace We7.CMS.Web.Admin.manage.ContentsSchema
{
    public partial class ModelControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bttnGenerate_Click(object sender, EventArgs e)
        {
            ModelInfo model = ModelHelper.GetModelInfo(txtModelName.Text.Trim());
            ModelHelper.CreateControls(model);
        }
    }
}
