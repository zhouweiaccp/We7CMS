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
    public partial class Request : FieldControl
    {
        public override void InitControl()
        {
        }

        public override object GetValue()
        {
            string param=Control.Params["param"];
            if (string.IsNullOrEmpty(param))
                param = Control.Name;
            return !String.IsNullOrEmpty(Request[param])?HttpUtility.UrlDecode(Request[param]):Control.DefaultValue;
        }
    }
}