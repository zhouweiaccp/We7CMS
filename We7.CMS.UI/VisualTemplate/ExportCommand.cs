using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.Data;
using System.Data;
using We7.Model.Core.Converter;
using We7.Model.Core;

namespace We7.CMS.Module.VisualTemplate
{
    /// <summary>
    /// 添加数据
    /// </summary>
    public class ExportCommand : ICommand
    {
        public object Do(PanelContext data)
        {
            int recordcount,pageindex=data.PageIndex;
            DataTable dt=DbProvider.Instance(data.Model.Type).Query(data, out recordcount, ref pageindex);
            ModelHelper.ExtendDataTable(dt, data);
            DesignHelper.ExprotData(dt);
            return null;
        }
    }
}
