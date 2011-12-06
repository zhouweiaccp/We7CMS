using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using We7.CMS.Common;
using System.Collections.Generic;
using We7.CMS.Common.Enum;
using We7.Model.Core;
using We7.Framework.Config;
using System.IO;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateGroupInfo : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.NoMenu;
            }
        }

        string FileName
        {
            get { return Request["file"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Control ctl = this.LoadControl("../Template/controls/TemplateGroup_Info.ascx");
            ContentHolder.Controls.Add(ctl);
            if (!string.IsNullOrEmpty(FileName))
                NameLabel.Text = string.Format("编辑模板组{0}基本信息", Path.GetFileNameWithoutExtension(FileName));
            else
                NameLabel.Text = "新建模板组";
        }

    }
}
