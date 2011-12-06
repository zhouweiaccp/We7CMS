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
using We7.Framework.Util;

namespace We7.Model.UI.Controls.system
{
    public partial class Number : FieldControl
    {
        public override void InitControl()
        {
            txtInput.Text = Value == null ? Control.DefaultValue : Value.ToString();

            if (!String.IsNullOrEmpty(Control.Width))
            {
                txtInput.Width = Unit.Parse(Control.Width);
            }
            else
            {
                txtInput.Width = Unit.Parse("30px");
            }
            if (!String.IsNullOrEmpty(Control.Height))
            {
                txtInput.Height = Unit.Parse(Control.Height);
            }

            txtInput.CssClass = Control.CssClass;
            if (Control.Required && !txtInput.CssClass.Contains("required"))
            {
                txtInput.CssClass += " required";
            }
        }

        public override object GetValue()
        {
            return TypeConverter.StrToObjectByTypeCode(txtInput.Text.Trim(), Column.DataType);
        }
    }
}