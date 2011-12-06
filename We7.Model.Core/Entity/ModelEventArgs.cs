using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Collections;

namespace We7.Model.Core
{
    /// <summary>
    /// 模型命令参数
    /// </summary>
    public class ModelEventArgs : EventArgs
    {
        /// <summary>
        /// 模型事件参数
        /// </summary>
        public ModelEventArgs() { }

        /// <summary>
        /// 模型事件参数
        /// </summary>
        /// <param name="commandNamd">命令名称</param>
        /// <param name="data">模型数据</param>
        public ModelEventArgs(string commandName, PanelContext data) : this(commandName, "", data) { }

        /// <summary>
        /// 模型事件参数
        /// </summary>
        /// <param name="commandNamd">命令名称</param>
        /// <param name="args">命令参数</param>
        /// <param name="data">模型数据</param>
        public ModelEventArgs(string commandName, object commandArgs, PanelContext data)
        {
            CommandName = commandName;
            CommandArguments = commandArgs;
            PanelContext = data;
        }

        /// <summary>
        /// 命令名称
        /// </summary>
        public string CommandName;
        /// <summary>
        /// 附加命令参数值
        /// </summary>
        public Object CommandArguments;
        /// <summary>
        /// 模型数据
        /// </summary>
        public PanelContext PanelContext;
        /// <summary>
        /// 当前命令是否被禁用
        /// </summary>
        public bool Disable = false;

        /// <summary>
        /// 状态信息
        /// </summary>
        public object State;

        /// <summary>
        /// 键段对参数，用于存储更多的参数
        /// </summary>
        public Hashtable HashTable = new Hashtable();
    }

    /// <summary>
    /// 模型委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void ModelEventHandler(object sender, ModelEventArgs args);
}
