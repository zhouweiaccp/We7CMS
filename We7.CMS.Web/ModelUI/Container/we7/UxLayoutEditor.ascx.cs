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
using We7.Model.Core;
using We7.Model.Core.UI;
using We7.Framework;

namespace We7.Model.UI.Container.we7
{
    public partial class UxLayoutEditor : EditorContainer
    {
        protected string NewUrl
        {
            get
            {
                return We7Helper.AddParamToUrl(Request.RawUrl, We7.Model.Core.UI.Constants.EntityID, We7Helper.CreateNewID());
            }
        }

        LayoutEditor editor;

        protected override void InitContainer()
        {
            ChangeState();
            if (editor == null)
            {
                editor = UIHelper.LoadLayoutEditor(PanelContext.Panel.EditInfo.Layout);
                editor.ID = "UxLayoutCtr";
                UxLayout.Controls.Clear();
                UxLayout.Controls.Add(editor);
            }
            editor.InitLayout(PanelContext);
            ModelLabel.Text = PanelContext.Model.Label;
            MenuTabLabel.Text = BuildNavString();
        }

        protected override void ChangeState()
        {
            bttnEdit.Visible = IsEdit;
            bttnSave.Visible = !IsEdit;
        }

        /// <summary>
        /// 构建标签项
        /// </summary>
        /// <returns></returns>
        string BuildNavString()
        {
            string str1 = @"<LI class=TabOut id=tab{0}  style='display:{2}'><A  href={3}>{1}</A> </LI>";
            string str2 = @"<LI class=TabIn id=tab{0} style='display:{2}'><A>{1}</A> </LI>";
            return String.Format(str2, 1, "发布"+PanelContext.Model.Label, "block");
        }

        protected void bttnSave_Click(object sender, EventArgs e)
        {
            try
            {
                OnButtonSubmit(sender, e);
                UIHelper.SendMessage("添加成功");
            }
            catch (Exception ex)
            {
                UIHelper.SendError("添加失败:" + ex.Message);
            }
        }

        protected void bttnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                OnButtonSubmit(sender, e);
                UIHelper.SendMessage("修改成功");
            }
            catch (Exception ex)
            {
                UIHelper.SendError("修改失败:" + ex.Message);
            }
        }
    }
}