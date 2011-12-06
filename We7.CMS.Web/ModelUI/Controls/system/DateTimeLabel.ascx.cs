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
    public partial class DateTimeLabel : FieldControl
    {
        protected global::System.Web.UI.WebControls.HiddenField hfValue;
        public override void InitControl()
        {
            if (Value is System.DateTime)
            {
                hfValue.Value = Value.ToString();
                string fmt = !String.IsNullOrEmpty(Control.Params["fmt"]) ? Control.Params["fmt"] : "yyyy-MM-dd HH:mm";
                ltlText.Text = ((System.DateTime)Value).ToString(fmt);
            }
        }

        public override object GetValue()
        {
            System.DateTime dt;
            System.DateTime.TryParse(hfValue.Value, out dt);
            return dt;
        }
    }
}