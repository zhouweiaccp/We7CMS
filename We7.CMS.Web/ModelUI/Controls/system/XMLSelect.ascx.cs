using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using We7.Model.Core.UI;
using System.IO;
using System.Xml;
using We7.CMS.Common;
using We7.Model.Core;
using We7.Framework.Util;

namespace We7.Model.UI.Controls.system
{
    public partial class XMLSelect : We7FieldControl
    {
        public override void InitControl()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string data = Control.Params[Constants.DATA];
            if (File.Exists(Server.MapPath(data)))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Server.MapPath(data));
                XmlNodeList ItemListNodes = doc.SelectNodes("//Item");

                for (int i = 0; i < ItemListNodes.Count; i++)
                {
                    string[] sl = ItemListNodes[i].InnerText.Split('|');
                    if (sl.Length > 1)
                    {
                        dic.Add(sl[0], sl[1]);
                    }
                    else
                    {
                        dic.Add(sl[0], sl[0]);
                    }
                }
            }
            DdlXml.DataSource = dic;
            DdlXml.DataTextField = "value";
            DdlXml.DataValueField = "key";
            DdlXml.DataBind();
            DdlXml.Items.Insert(0, new ListItem("请选择", ""));
            DdlXml.SelectedValue = Value != null ? Value.ToString() : Control.DefaultValue;

            if (Control.Required)
            {
                RequiredFieldValidator rfv = new RequiredFieldValidator();
                rfv.ControlToValidate = "DdlXml";
                rfv.Display = ValidatorDisplay.Dynamic;
                rfv.Text = "*";
                rfv.InitialValue = "";
                phValidate.Controls.Clear();
                phValidate.Controls.Add(rfv);
            }
        }
        public override object GetValue()
        {
            return TypeConverter.StrToObjectByTypeCode(DdlXml.SelectedValue, Column.DataType);
        }
    }
}