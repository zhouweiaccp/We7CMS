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

namespace We7.Model.UI.Controls.system
{
    public partial class Image : FieldControl
    {
        public override void InitControl()
        {
            if (Value != null)
            {
                img.Src = Value.ToString();
            }
            if (!String.IsNullOrEmpty(Control.Width))
            {
                img.Width = (int)Unit.Parse(Control.Width).Value;
            }
            if (!String.IsNullOrEmpty(Control.Height))
            {
                img.Height = (int)Unit.Parse(Control.Height).Value;
            }
        }

        public override object GetValue()
        {
            return img.Src;
        }
    }
}