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

namespace We7.CMS.Web.Admin.Addins
{
    public partial class ModelHandler :ModelHandlerPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string model = Request["model"];
            if (String.IsNullOrEmpty(model))
            {
                throw new Exception("没有指定模型名称");
            }
            ucMulti.ModelName = model;
        }
    }
}
