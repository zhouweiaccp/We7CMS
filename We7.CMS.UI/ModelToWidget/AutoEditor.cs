using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core;
using System.Web.UI.WebControls;

namespace We7.CMS.WebControls
{
    public class AutoEditor : EditorContainer
    {
        private string panelName = "edit";
        public override string PanelName
        {
            get
            {
                return panelName;
            }
            set
            {
                panelName = value;
            }
        }

        /// <summary>
        /// 初始化自定义容器
        /// </summary>
        protected override void InitContainer()
        {
            InitControls();
            RegisterEvent();
        }

        protected virtual void InitControls()
        {
            foreach (We7Control ctr in Panel.EditInfo.Controls)
            {
                PlaceHolder c = UIHelper.GetControl<PlaceHolder>("_" + ctr.Name, this);
                if (c != null && c.FindControl(ctr.Name) == null)
                {
                    c.Controls.Clear();
                    c.Controls.Add(UIHelper.GetControl(ctr));
                }
                //else
                //{
                //    if (Page.Form != null && Page.Form.FindControl(ctr.ID) == null)
                //    {
                //        if (this.Controls.Contains(Page.Form))
                //            Page.Form.Controls.Add(UIHelper.GetControl(ctr));
                //        else
                //            this.Controls.Add(UIHelper.GetControl(ctr));
                //        //Page.Form.Controls.Add(UIHelper.GetControl(ctr));
                //    }
                //}
            }
        }

        protected void RegisterEvent()
        {
            OnInteralCommand += new ModelEventHandler(AutoEditor_OnInteralCommand);
        }

        void AutoEditor_OnInteralCommand(object sender, ModelEventArgs args)
        {
            if (!args.Disable)
            {
                ICommand cmd = CommandFactory.GetCommand(args.CommandName);
                if (cmd == null)
                {

                    throw new SysException(cmd + "模型命令不存在");
                }
                args.State = cmd.Do(args.PanelContext);
            }
        }
    }
}
