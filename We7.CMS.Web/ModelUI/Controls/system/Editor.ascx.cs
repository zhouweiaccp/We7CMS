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

namespace CModel.Controls.system
{
    public partial class Editor : FieldControl
    {

        public override void InitControl()
        {
            fckEditor.BasePath = "/admin/fckeditor/";
            fckEditor.Value = Value == null ? Control.DefaultValue : Value.ToString();
            fckEditor.Width = !String.IsNullOrEmpty(Control.Width) ? Unit.Parse(Control.Width) : Unit.Pixel(545);
            fckEditor.Height = !String.IsNullOrEmpty(Control.Height) ? Unit.Parse(Control.Height) : Unit.Pixel(300);
        }

        public override object GetValue()
        {
            return fckEditor.Value;
        }
    }
}