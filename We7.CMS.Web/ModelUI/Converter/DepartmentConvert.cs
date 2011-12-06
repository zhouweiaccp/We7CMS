using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.ListControl;
using We7.Model.Core.UI;
using We7.Framework;
using We7.CMS;
using We7.CMS.Common.PF;

namespace We7.Model.UI.Converter
{
    /// <summary>
    /// 部门转化
    /// </summary>
    public class DepartmentConvert : IUxConvert
    {
        #region IUxConvert 成员

        public string GetText(object dataItem, We7.Model.Core.ColumnInfo columnInfo)
        {
            string v=ModelControlField.GetValue(dataItem, columnInfo.Name);
            Department depart = AccountFactory.CreateInstance().GetDepartment(v, new string[] { "Name" });
            return depart != null ? depart.Name : String.Empty;
        }

        #endregion
    }
}
