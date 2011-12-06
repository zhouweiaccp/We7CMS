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
    public class ChannelConvert : IUxConvert
    {
        #region IUxConvert 成员

        public string GetText(object dataItem, We7.Model.Core.ColumnInfo columnInfo)
        {
            string v = ModelControlField.GetValue(dataItem, columnInfo.Name);
            Channel ch = HelperFactory.Instance.GetHelper<ChannelHelper>().GetChannel(v,null);
            return ch != null ? ch.Name : String.Empty;
        }

        #endregion
    }
}
