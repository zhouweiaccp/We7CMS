using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using We7.Model.Core.Data;

namespace We7.Model.Core.Command
{
    /// <summary>
    /// 获取记录
    /// </summary>
    class GetCommand : ICommand
    {
        public object Do(PanelContext data)
        {
            int recordcount, pageindex = data.PageIndex;
            return DbProvider.Instance(data.Model.Type).Get(data);
        }
    }
}
