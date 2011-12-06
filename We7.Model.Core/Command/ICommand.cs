using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Util;
using We7.Model.Core.Config;
using We7.Model.Core.Command;

namespace We7.Model.Core
{
    /// <summary>
    /// 操作命令
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="data">模型数据</param>
        object Do(PanelContext data);
    }

    /// <summary>
    /// 命令工厂
    /// </summary>
    public class CommandFactory
    {
        /// <summary>
        /// 根据命令名称获取命令对象
        /// </summary>
        /// <param name="cmd">命令名称</param>
        /// <returns>命令对象</returns>
        public static ICommand GetCommand(string cmd)
        {
            ICommand command=AppCtx.Cache.RetrieveObject<ICommand>(GetCommandKey(cmd));
            if (command == null)
            {
                command = Utils.CreateInstance<ICommand>(ModelConfig.Commands[cmd]);
                if (command == null)
                {
                    throw new SysException("CmdFactory::[" + ModelConfig.Commands[cmd] + "]当前命令不存在!");
                }
                if (command is CompositeCommand)
                {
                    CompositeCommand compCmd = command as CompositeCommand;
                    foreach (NameValue nv in ModelConfig.Commands.Get(cmd).Params)
                    {
                        ICommand subCmd = Utils.CreateInstance<ICommand>(nv.Value);
                        if(subCmd==null)
                            throw new SysException("CmdFactory::[" + nv.Value + "]当前命令不存在!");
                        compCmd.AddCommand(subCmd);
                    }                    
                }
                AppCtx.Cache.AddObjectWithFileChange(GetCommandKey(cmd), command, ModelConfig.ConfigFilePath);
            }
            return command;
        }

        static string GetCommandKey(string key)
        {
            return String.Format("{0}_command", key);
        }
    }
}
