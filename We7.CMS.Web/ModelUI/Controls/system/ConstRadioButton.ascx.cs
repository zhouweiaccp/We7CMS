using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Model.Core.UI;
using We7.Framework.Util;

namespace We7.Model.UI.Controls.system
{
    public partial class ConstRadionButton : FieldControl
    {
        public override void InitControl()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (Control.Params[Constants.DIRECTION] == "v")
            {
                rblEnum.RepeatDirection = RepeatDirection.Vertical;
            }
            else
            {
                rblEnum.RepeatDirection = RepeatDirection.Horizontal;
            }
            string data =GetConstStr(Control.Params[Constants.DATA]);
            if (String.IsNullOrEmpty(data))
                return;
            string[] s = data.Split(',');
            if (s.Length > 0)
            {
                foreach (string ss in s)
                {
                    string[] skvp = ss.Split('|');
                    if (skvp.Length > 1)
                    {
                        dic.Add(skvp[0], skvp[1]);
                    }
                    else
                    {
                        dic.Add(skvp[0], skvp[0]);
                    }
                }
            }
            rblEnum.DataSource = dic;
            rblEnum.DataTextField = "value";
            rblEnum.DataValueField = "key";
            rblEnum.DataBind();
            string v = Value == null ? Control.DefaultValue : Value.ToString();
            if (String.IsNullOrEmpty(v) && rblEnum.Items.Count > 0)
            {
                rblEnum.SelectedIndex = 0;
            }
            else
            {
                rblEnum.SelectedValue = v;
            }

            rblEnum.CssClass = Control.CssClass;
            if (Control.Required && !rblEnum.CssClass.Contains("required"))
            {
                rblEnum.CssClass += " required";
            }
        }

        public override object GetValue()
        {
            return TypeConverter.StrToObjectByTypeCode(rblEnum.SelectedValue, Column.DataType);
        }
    }
}