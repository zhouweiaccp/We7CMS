using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace We7.CMS.Web.Admin
{
    public partial class DataControls : BasePage
    {

        protected override void Initialize()
        {
            DataControlsGridView.DataSource = TemplateHelper.GetDataControls(null);
            DataControlsGridView.DataBind();
        }
    }
}