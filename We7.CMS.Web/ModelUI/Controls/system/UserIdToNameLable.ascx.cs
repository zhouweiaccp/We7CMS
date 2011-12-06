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
using We7.CMS;
using We7.CMS.Common.PF;
using We7.Model.Core.Data;
using Thinkment.Data;
using System.Collections.Generic;
using We7.CMS.Accounts;

namespace We7.Model.UI.Controls.system
{
    public partial class UserIdToNameLable : FieldControl
    {
        protected global::System.Web.UI.WebControls.HiddenField hfValue;
        public override void InitControl()
        {
            hfValue.Value=lblValue.Text = (Value ?? "").ToString();
            IAccountHelper accountHelper = AccountFactory.CreateInstance();
            Account account = accountHelper.GetAccount(hfValue.Value, null); 
            if (account == null)
            {
                ltlText.Text = "";
            }
            else
            {
               string data = Control.Params[We7.Model.Core.UI.Constants.DATA];
               if (!string.IsNullOrEmpty(data) && data == "admin")
               {
                   //ShopPlugin.AdvanceUser
                   DataTable dt = ModelDBHelper.Create("ShopPlugin.AdvanceUser").Query(new Criteria(CriteriaType.Equals, "UserID", hfValue.Value), new List<Order>() { new Order("ID", OrderMode.Desc) }, 0, 0);
                   if (dt != null && dt.Rows.Count > 0)
                   {
                       ltlText.Text = "<a href='/admin/AddIns/ModelEditor.aspx?notiframe=1&model=ShopPlugin.AdvanceUser&ID=" + dt.Rows[0]["ID"].ToString() + "'>" + account.LoginName + "</a>";
                   }
                   else
                   {
                       ltlText.Text = "<a href='/admin/Permissions/AccountEdit.aspx?id=" + account.ID + "'>" + account.LoginName + "</a>";
                   }
               }
               else
               {
                   ltlText.Text = account.LoginName;
               }               
            }
        }

        public override object GetValue()
        {
            if (Column.DataType == TypeCode.String
                || Column.DataType == TypeCode.Char)
            {
                return hfValue.Value;
            }
            else
            {
                return TypeConverter.StrToObjectByTypeCode(We7Helper.FilterHtmlChars(hfValue.Value), Column.DataType);
            }
        }
    }
}