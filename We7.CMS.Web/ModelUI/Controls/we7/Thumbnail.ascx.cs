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
using We7.Model.UI.Controls;
using We7.Framework;

namespace We7.CMS.Web.Admin.ContentModel.Controls
{
    public partial class Thumbnail : We7FieldControl
    {
        public override void InitControl()
        {
            txtPath.Text = Value != null ? Value.ToString() : "";
            txtPath.CssClass = Control.CssClass;
            if (Control.Required && !txtPath.CssClass.Contains("required"))
            {
                txtPath.CssClass += " required";
            }
        }

        public override object GetValue()
        {
            return We7Helper.FilterHtmlChars(txtPath.Text);
        }
    }
}