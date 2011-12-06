using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Util;

namespace We7.Model.Core
{
    public partial class ModelHelper
    {
        /// <summary>
        /// 获取导入数据项
        /// </summary>
        /// <param name="modelName">模型名称</param>
        /// <param name="panelName">面板名称</param>
        /// <returns></returns>
        public static DataFieldCollection GetImportRow(string modelName,string panelName,out PanelContext pctx)
        {
            pctx = ModelHelper.GetPanelContext(modelName, panelName);
            return pctx.Row;
        }

        /// <summary>
        /// 获取导入数据项
        /// </summary>
        /// <param name="modelName">模型名称</param>
        /// <param name="panelName">面板名称</param>
        /// <returns></returns>
        public static DataFieldCollection GetImportRow(string modelName, out PanelContext pctx)
        {
            pctx = ModelHelper.GetPanelContext(modelName, "edit");
            return pctx.Row;
        }

        /// <summary>
        /// 保存当前行记录
        /// </summary>
        /// <param name="ctx">当前数据项</param>
        public static void SaveContext(PanelContext ctx)
        {
            ctx.Row["ID"] = Utils.CreateGUID();
            ICommand command = CommandFactory.GetCommand("add");
            command.Do(ctx);
        }
    }
}
