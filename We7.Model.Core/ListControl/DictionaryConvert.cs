using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.UI;

namespace We7.Model.Core.ListControl
{
    public class DictionaryConvert:IUxConvert
    {
        #region IUxConvert 成员

        public string GetText(object dataItem, ColumnInfo columnInfo)
        {
            Dictionary<string, string> dic = GetDic(columnInfo);
            string v=ModelControlField.GetValue(dataItem,columnInfo.Name);
            return dic.ContainsKey(v) ? dic[v] : String.Empty;            
        }

        Dictionary<string, string> GetDic(ColumnInfo columnInfo)
        {
            if (String.IsNullOrEmpty(columnInfo.Expression))
                columnInfo.Expression = "0|否,1|是";

            string[] kvs = columnInfo.Expression.Split(',');
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string kv in kvs)
            {
                string[] ss = kv.Split('|');
                if (ss.Length == 1)
                {
                    dic.Add(ss[0], ss[0]);
                }
                else
                {
                    dic.Add(ss[0], ss[1]);
                }
            }
            return dic;
        }

        #endregion
    }
}
