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
    public partial class Text : FieldControl
    {
        protected global::System.Web.UI.WebControls.HiddenField hfValue;
        public override void InitControl()
        {
            hfValue.Value=ltlText.Text = (Value ?? "").ToString();
        }

        public override object GetValue()
        {
            if (Column.DataType == TypeCode.String
                || Column.DataType == TypeCode.Char)
            {
                return hfValue.Value;
            }
            else
            {
                return TypeConverter.StrToObjectByTypeCode(We7Helper.FilterHtmlChars(hfValue.Value), Column.DataType);
            }
        }
    }
}