using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;

namespace We7.Model.Core.UI
{
    /// <summary>
    /// 命令类容器
    /// </summary>
    public class CommandContainer:ModelContainer
    {
        /// <summary>
        /// 初始化模型数据
        /// </summary>
        protected override void InitModelData()
        {
        }

        /// <summary>
        /// 按钮类控件执行方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        protected virtual void OnButtonSubmit(object sender, EventArgs arg)
        {
            IButtonControl bc = sender as IButtonControl;
            if (bc == null)
            {
                throw new Exception("触发控件不是按钮类控件");
            }
            OnCommandSubmit(bc.CommandName, bc.CommandArgument);
        }
    }
}
