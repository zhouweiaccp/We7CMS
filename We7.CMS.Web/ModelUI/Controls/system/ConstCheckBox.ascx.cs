using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Model.Core.UI;
using System.Text;
using System.Xml;
using We7.Model.Core.Config;
using System.IO;

namespace We7.Model.UI.Controls.system
{
    public partial class ConstCheckBox : FieldControl
    {
        public override void InitControl()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (Control.Params[Constants.DIRECTION] == "v")
            {
                chklEnum.RepeatDirection = RepeatDirection.Vertical;
            }
            else
            {
                chklEnum.RepeatDirection = RepeatDirection.Horizontal;
            }
            string data = GetConstStr(Control.Params[Constants.DATA]);
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
            chklEnum.DataSource = dic;
            chklEnum.DataTextField = "value";
            chklEnum.DataValueField = "key";
            chklEnum.DataBind();
            string v = Value == null ? Control.DefaultValue : Value.ToString();
            if (!String.IsNullOrEmpty(v))
            {
                string[] ss = v.Split(",".ToCharArray());
                foreach (ListItem item in chklEnum.Items)
                {
                    foreach (string str in ss)
                    {
                        if (str == item.Value)
                        {
                            item.Selected = true;
                            continue;
                        }
                    }
                }
            }

            chklEnum.CssClass = Control.CssClass;
            if (Control.Required && !chklEnum.CssClass.Contains("required"))
            {
                chklEnum.CssClass += " required";
            }
        }

        public override object GetValue()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ListItem item in chklEnum.Items)
            {
                if (item.Selected)
                {
                    sb.Append(item.Value).Append(",");
                }
            }
            We7.Framework.Util.Utils.TrimEndStringBuilder(sb, ",");
            return sb.ToString();
        }        
    }
}