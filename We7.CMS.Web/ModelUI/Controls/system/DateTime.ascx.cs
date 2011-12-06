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

namespace We7.Model.UI.Controls.system
{
    public partial class DateTime : FieldControl
    {
        public override void InitControl()
        {
            string dateformat = !String.IsNullOrEmpty(Control.Params[Constants.DATEFORMAT]) ? Control.Params[Constants.DATEFORMAT] : "yyyy-MM-dd HH:mm";
            System.DateTime dt = (System.DateTime)(Value==DBNull.Value||Value==null?System.DateTime.Now:Value);
            txtInput.Text = dt.ToString(dateformat);

            if (String.Compare(Control.Params["dbclick"],"true",true)==0)
            {
                txtInput.Attributes["ondbclick"] = "WdatePicker({dateFmt:'"+dateformat+"'})";
            }
            else
            {
                txtInput.Attributes["onfocus"] = "WdatePicker({dateFmt:'" + dateformat + "'})";
            }

            if (!String.IsNullOrEmpty(Control.Width))
            {
                txtInput.Width = Unit.Pixel(TypeConverter.StrToInt(Control.Width, 100));
            }
            if (!String.IsNullOrEmpty(Control.Height))
            {
                txtInput.Height = Unit.Pixel(TypeConverter.StrToInt(Control.Height, 25));
            }

            txtInput.CssClass = Control.CssClass;
            if (Control.Required && !txtInput.CssClass.Contains("required"))
            {
                txtInput.CssClass += " required";
            }
        
        }

        public override object GetValue()
        {
            string alwaysNew = Control.Params["new"];
            if (alwaysNew == "true")
            {
                return System.DateTime.Now;
            }
            else
            {
                return TypeConverter.StrToObjectByTypeCode(txtInput.Text, TypeCode.DateTime);
            }
        }
    }
}