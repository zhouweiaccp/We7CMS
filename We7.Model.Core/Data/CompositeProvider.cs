using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;

namespace We7.Model.Core.Data
{
    public class CompositeProvider : IDbProvider
    {
        List<IDbProvider> providers = new List<IDbProvider>();

        public void Add(IDbProvider provider)
        {
            providers.Add(provider);
        }

        public void Remove(IDbProvider provider)
        {
            if (providers.Contains(provider))
                providers.Remove(provider);
        }

        #region IDbProvider 成员

        public bool Insert(PanelContext data)
        {
            foreach (IDbProvider provider in providers)
            {
               provider.Insert(data);
            }
            return true;
        }

        public bool Update(PanelContext data)
        {
            foreach (IDbProvider provider in providers)
            {
               provider.Update(data);
            }
            return true;
        }

        public bool Delete(PanelContext data)
        {
            foreach (IDbProvider provider in providers)
            {
               provider.Delete(data);
            }
            return true;
        }

        public System.Data.DataTable Query(PanelContext data, out int recordcount, ref int pageindex)
        {
            if (providers.Count > 0)
                return providers[0].Query(data, out recordcount, ref pageindex);
            recordcount = 0;
            return null;
        }

        public System.Data.DataRow Get(PanelContext data)
        {
            if (providers.Count > 0)
                return providers[0].Get(data);
            Logger.Error("内容模型::数据提供者为空");
            return null;
        }

        public int GetCount(PanelContext data)
        {
            if (providers.Count > 0)
                return providers[0].GetCount(data);
            Logger.Error("内容模型::数据提供者为空");
            return 0;
        }

        #endregion
    }
}
