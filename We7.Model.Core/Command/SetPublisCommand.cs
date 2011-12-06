using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using We7.Framework.Helper;
using We7.Framework.Cache;

namespace We7.Model.Core.Command
{
    class SetPublisCommand:ICommand
    {
        #region ICommand 成员

        public object Do(PanelContext data)
        {
            List<DataKey> dataKeys = data.State as List<DataKey>;
            if (dataKeys != null)
            {
                foreach (DataKey key in dataKeys)
                {
                    string id = key["ID"] as string;
                    DbHelper.ExecuteSql(String.Format("UPDATE [{0}] SET [State]=1 WHERE [ID]='{1}'", data.Table.Name, id));
                }
            }
            CacheRecord.Create(data.ModelName).Release();
            return null;
        }

        #endregion
    }
}
