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
using We7.Framework;
using We7.CMS;
using We7.CMS.Common;

namespace We7.Model.UI.Controls.system
{
    public partial class CategoryDropdownList : FieldControl
    {
        public override void InitControl()
        {
            string keyword=Control.Params["keyword"];
            string format = Control.Params["format"];
            CategoryHelper helper=HelperFactory.Instance.GetHelper<CategoryHelper>();
            CategoryCollection col;
            if (String.Compare("true", format, true) == 0)
            {
                col= helper.GetFrmChildrenByKeyword(keyword);
            }
            else
            {
                col=helper.GetChildrenByKeyword(keyword);
            }

            ddlCategory.DataSource=col;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "KeyWord";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0,new ListItem("请选择", ""));
            ddlCategory.SelectedValue = Value as string;


            if (!String.IsNullOrEmpty(Control.Width))
            {
                ddlCategory.Width = Unit.Parse(Control.Width);
            }
            if (!String.IsNullOrEmpty(Control.Height))
            {
                ddlCategory.Height = Unit.Parse(Control.Height);
            }

            ddlCategory.CssClass = Control.CssClass;
            if (Control.Required && !ddlCategory.CssClass.Contains("required"))
            {
                ddlCategory.CssClass += " required";
            }
        }

        public override object GetValue()
        {
            return ddlCategory.SelectedValue;
        }
    }
}