using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework;

namespace We7.Model.UI.Controls.we7
{
    public partial class IP : We7FieldControl
    {
        public override void InitControl()
        {
            string ip=Value as string;

            if (String.IsNullOrEmpty(ip))
            {
                lblIP.Text = hfIP.Value = Request.UserHostAddress;
            }
            else
            {
                lblIP.Text = ip;
            }
        }

        public override object GetValue()
        {
            return hfIP.Value;
        }
    }
}