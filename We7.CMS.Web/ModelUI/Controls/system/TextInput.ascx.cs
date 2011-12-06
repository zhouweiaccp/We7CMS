using System;
using We7.Model.Core.UI;
using System.Data;
using We7.Framework.Util;
using System.Web.UI.WebControls;
using System.Web;
using We7.Framework;

namespace We7.CMS.UI.Controls
{
    public partial class TextInput : FieldControl
    {
        public override void InitControl()
        {
            txtInput.Text = Value == null ? Control.DefaultValue : Value.ToString();
            txtInput.CssClass = Control.CssClass;

            if (!String.IsNullOrEmpty(Control.Width))
            {
                txtInput.Width = Unit.Parse(Control.Width);
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
            return TypeConverter.StrToObjectByTypeCode(HttpUtility.HtmlEncode(txtInput.Text), Column.DataType);
        }
    }
}