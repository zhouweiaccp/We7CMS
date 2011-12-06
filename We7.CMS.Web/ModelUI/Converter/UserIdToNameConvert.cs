using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.UI;
using We7.CMS;
using We7.CMS.Common.PF;
using We7.CMS.Accounts;

namespace We7.Model.Core.ListControl
{
    public class UserIdToNameConvert  :IUxConvert
    {
        #region IUxConvert 成员

        public string GetText(object dataItem, ColumnInfo columnInfo)
        {            
            string v = ModelControlField.GetValue(dataItem,columnInfo.Name);
            IAccountHelper accountHelper = AccountFactory.CreateInstance();
            Account account = accountHelper.GetAccount(v, null);
            if (account == null)
            {
                return "";
            }
            else
            {
                if (v == We7Helper.EmptyGUID)
                {
                    return "admin";
                }
                return "<a href='" + GetUrl(columnInfo) + v + "'>" + account.LoginName + "</a>";
            }
            //return 
        }

        string GetUrl(ColumnInfo columnInfo)
        {
            if (String.IsNullOrEmpty(columnInfo.Expression))
                columnInfo.Expression = "/admin/Permissions/AccountEdit.aspx|id";

            string[] kvs = columnInfo.Expression.Split('|');
            return kvs[0] + "?&" + kvs[1] + "=";
        }

        #endregion
    }
}
