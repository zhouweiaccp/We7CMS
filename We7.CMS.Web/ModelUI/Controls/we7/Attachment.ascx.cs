using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework;

namespace We7.Model.UI.Controls.we7
{
    public partial class Attachment : We7FieldControl
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