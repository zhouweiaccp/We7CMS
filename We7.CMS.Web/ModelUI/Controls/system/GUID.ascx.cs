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
using Util = We7.Framework.Util.Utils;
using We7.Model.UI.Controls;
using We7.Framework;

namespace We7.Model.UI.Container.system
{
    public partial class GUID : We7FieldControl
    {
        public override void InitControl()
        {
            string value = Value as string;
            if (!String.IsNullOrEmpty(value))
            {
                hfValue.Value = value;
            }
            else
            {
                hfValue.Value = ArticleID;
            }
        }

        bool CheckParentIsFiledControl(Control parent)
        {
            if (parent is FieldControl)
                return true;
            return parent != null && parent.Parent != null ? CheckParentIsFiledControl(parent.Parent) : false;
        }

        public override object GetValue()
        {
            if (CheckParentIsFiledControl(this.Parent) && hfValue.Value == ArticleID)
            {
                return We7Helper.CreateNewID();
            }
            else
            {
            return We7Helper.FilterHtmlChars(hfValue.Value);
            }
        }
    }
}