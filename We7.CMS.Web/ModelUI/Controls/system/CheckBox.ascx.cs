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
using System.Text;

namespace We7.Model.UI.Controls.system
{
    public partial class CheckBox : FieldControl
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
            string data = Control.Params[Constants.DATA];
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
                string[] ss=v.Split(",".ToCharArray());
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