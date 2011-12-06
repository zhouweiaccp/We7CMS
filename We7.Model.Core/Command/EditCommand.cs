using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.Data;
using We7.Framework.Cache;

namespace We7.Model.Core.Command
{
    /// <summary>
    /// 更新命令
    /// </summary>
    class EditCommand : ICommand
    {
        public object Do(PanelContext data)
        {
            DbProvider.Instance(data.Model.Type).Update(data);
            CacheRecord.Create(data.ModelName).Release();
            return null;
        }
    }
}
