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
using System.Collections.Generic;
using We7.Framework.Util;

namespace We7.Model.UI.Controls.system
{
    public partial class Select : FieldControl
    {
        public override void InitControl()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string data = Control.Params[Constants.DATA];
            string[] s = data.Split(',');
            foreach (string ss in s)
            {
                string[] sl=ss.Split('|');
                dic[sl[0]] =sl.Length > 1?sl[1]:sl[0];
            }
            ddlEnum.DataSource = dic;
            ddlEnum.DataTextField = "value";
            ddlEnum.DataValueField = "key";
            ddlEnum.DataBind();
            ddlEnum.Items.Insert(0, new ListItem("请选择", ""));
            ddlEnum.SelectedValue =Value == null ? Control.DefaultValue : Value.ToString();

            if (!String.IsNullOrEmpty(Control.Width))
            {
                ddlEnum.Width = Unit.Parse(Control.Width);
            }
            if (!String.IsNullOrEmpty(Control.Height))
            {
                ddlEnum.Height = Unit.Parse(Control.Height);
            }

            ddlEnum.CssClass = Control.CssClass;
            if (Control.Required && !ddlEnum.CssClass.Contains("required"))
            {
                ddlEnum.CssClass += " required";
            }
        }

        public override object GetValue()
        {
            return TypeConverter.StrToObjectByTypeCode(ddlEnum.SelectedValue, Column.DataType);
        }
    }
}