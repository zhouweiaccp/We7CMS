using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework;
using We7.Framework.Common;
using System.IO;

namespace We7.Model.UI.Controls.we7
{
    public partial class AttachView : We7FieldControl
    {
        public override void InitControl()
        {
            string resource = Value as string;
            if (!String.IsNullOrEmpty(resource))
            {
                lnkAttachment.NavigateUrl = resource;
                lnkAttachment.Text = Path.GetFileName(resource);
            }
        }

        public override object GetValue()
        {
            return lnkAttachment.NavigateUrl;
        }
    }
}