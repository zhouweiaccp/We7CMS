using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Model.Core.UI
{
    /// <summary>
    /// 模型控件
    /// </summary>
    public interface IModelContainer
    {
        /// <summary>
        /// 执行的命令事件
        /// </summary>
        event ModelEventHandler OnCommand;

        /// <summary>
        /// 执行命令之间的事件
        /// </summary>
        event ModelEventHandler OnPreCommand;

        /// <summary>
        /// 执行命令完成之后的事件
        /// </summary>
        event ModelEventHandler OnCommandComplete;

        /// <summary>
        /// 模型名
        /// </summary>
        string ModelName { get; set; }

        /// <summary>
        /// 面板配置名
        /// </summary>
        string PanelName { get; set; }
    }
}
