using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Model.Core.Command
{
    class CompositeCommand : ICommand
    {
        List<ICommand> commands = new List<ICommand>();

        /// <summary>
        /// 添加操作命令
        /// </summary>
        /// <param name="cmd">添加的命令</param>
        public void AddCommand(ICommand cmd)
        {
            commands.Add(cmd);
        }

        #region ICommand 成员

        public object Do(PanelContext data)
        {
            object result=null;
            foreach (ICommand cmd in commands)
            {
                if (result == null) result = cmd.Do(data);
                else cmd.Do(data);
            }
            return result;
        }

        #endregion
    }
}
