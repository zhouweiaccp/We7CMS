using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core.UI;
using We7.Model.Core;
using System.Collections.Specialized;
using We7.Framework;

namespace We7.Model.UI.Panel.system
{
    public partial class SimpleEditorPanel : ModelPanel
    {
        public event EventHandler OnSuccess;

        protected override void OnPreRender(EventArgs e)
        {
        }
        public bool IsViewer { get; set; }

        private bool InitLock = false;
        protected override void InitPanel()
        {
            if (Navigation != null)
            {
                phNavigation.Controls.Clear();
                phNavigation.Controls.Add(Navigation);
                Navigation.OnPreCommand += new ModelEventHandler(ucNavigation_OnPreCommand);
            }
            if (!IsViewer && Editor != null)
            {
                phEditor.Controls.Clear();
                phEditor.Controls.Add(Editor);
                Editor.OnPreCommand += new ModelEventHandler(ucEditor_OnPreCommand);
                Editor.OnCommandComplete += new ModelEventHandler(ucEditor_OnCommandComplete);
                Editor.OnSetData += new EventHandler(Editor_OnSetData);
            }

            if (IsViewer && Viewer != null)
            {
                phEditor.Controls.Clear();
                phEditor.Controls.Add(Viewer);
            }
            if (Condition != null)
            {
                phCondition.Controls.Clear();
                phCondition.Controls.Add(Condition);
                Condition.OnPreCommand += new ModelEventHandler(ucCondition_OnPreCommand);
                Condition.OnCommandComplete += new ModelEventHandler(ucCondition_OnCommandComplete);
            }
        }

        void Editor_OnSetData(object sender, EventArgs e)
        {
        }

        void ucNavigation_OnPreCommand(object sender, ModelEventArgs args)
        {
        }

        void ucCondition_OnCommandComplete(object sender, ModelEventArgs args)
        {
            if (!IsPostBack || InitLock)
            {
                InitLock = false;
                ListResult result = args.State as ListResult;
                if (result != null && result.DataTable != null && result.DataTable.Rows.Count > 0)
                {
                    OrderedDictionary dic = new OrderedDictionary();
                    foreach (DataField fd in args.PanelContext.QueryFields)
                    {
                        dic.Add(fd.Column.Name, fd.Value);
                    }
                    if (IsViewer)
                        Viewer.SetData(result.DataTable.Rows[0], dic);
                    else
                        Editor.SetData(result.DataTable.Rows[0], dic);
                }
                else
                {
                    if (IsViewer)
                        Viewer.SetData(null, null);
                    else
                        Editor.SetData(null, null);
                }
            }
        }

        void ucCondition_OnPreCommand(object sender, ModelEventArgs args)
        {
        }

        void ucEditor_OnPreCommand(object sender, ModelEventArgs args)
        {
            if (args.CommandName == "reset")
            {
                Response.Redirect(We7Helper.AddParamToUrl(Request.RawUrl, We7.Model.Core.UI.Constants.EntityID, We7Helper.CreateNewID()));
                args.Disable = true;
            }
        }

        void ucEditor_OnCommandComplete(object sender, ModelEventArgs args)
        {
            if (OnSuccess != null)
            {
                OnSuccess(this, EventArgs.Empty);
            }
            if (args.CommandName == "add")
            {
                UIHelper.Message.AppendInfo(MessageType.ADDSUCCESS, "添加成功");
            }
            else
            {
                UIHelper.Message.AppendInfo(MessageType.EDITSUCCESS, "编辑成功");
            }
        }
    }
}