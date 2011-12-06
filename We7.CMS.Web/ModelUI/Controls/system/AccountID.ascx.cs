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
using We7.CMS.Accounts;

namespace We7.Model.UI.Controls.system
{
    public partial class AccountID : FieldControl
    {
        public override void InitControl()
        {
        }

        public override object GetValue()
        {
            return Security.CurrentAccountID;
        }
    }
}