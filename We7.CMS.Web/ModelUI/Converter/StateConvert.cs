using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.ListControl;
using We7.Model.Core.UI;
using We7.CMS.Common;
using We7.Framework;
using We7.CMS;

namespace We7.Model.UI.Converter
{
    /// <summary>
    /// 类别转化
    /// </summary>
    public class StateConvert : IUxConvert
    {
        #region IUxConvert 成员

        public string GetText(object dataItem, We7.Model.Core.ColumnInfo columnInfo)
        {
            string v = ModelControlField.GetValue(dataItem, columnInfo.Name);
            switch (v)
            {
                    case "1": return "<font color=green>已发布</font>";
                    case "2": return "<font color=#aa0>审核中</font>";
                    case "3": return "<font color=#888>已过期</font>";
                    case "4": return "<font color=#009>已删除</font>";
                    default:
                    case "0": return "<font color=red>已停用</font>";
            }
        }

        #endregion
    }
}
