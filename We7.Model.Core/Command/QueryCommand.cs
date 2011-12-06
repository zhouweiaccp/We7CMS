using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.Data;
using System.Data;
using We7.Model.Core.Converter;

namespace We7.Model.Core.Command
{
    /// <summary>
    /// 添加数据
    /// </summary>
    class QueryCommand : ICommand
    {
        public object Do(PanelContext data)
        {
            int recordcount,pageindex=data.PageIndex;
            DataTable dt=DbProvider.Instance(data.Model.Type).Query(data, out recordcount, ref pageindex);
            ModelHelper.ExtendDataTable(dt, data);
            ModelHelper.UpdateFields(data, null);//将数据清空
            return new ListResult() { DataTable=dt, RecoredCount=recordcount, PageIndex=pageindex };
        }
    }
}
