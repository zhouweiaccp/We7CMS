using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.UI;

namespace We7.CMS.WebControls
{
    public class ModelEditorProvider : AutoEditor
    {
        LayoutEditor editor;

        public string CssClass { get; set; }

        protected override void InitControls()
        {
            if (editor == null)
            {
                editor = UIHelper.LoadLayoutEditor(PanelContext.Panel.EditInfo.UcLayout);
                editor.ID = "UxLayoutCtr";
                phContainer.Controls.Clear();
                phContainer.Controls.Add(editor);
            }
            editor.InitLayout(PanelContext);
        }

        protected override void InitModelData()
        {
            if (!(FormPanel != null && FormPanel.Visible == false))
            {
                base.InitModelData();
                PanelContext.Row["ID"] = We7Helper.CreateNewID();
            }
        }

        public void OnValidateSubmit(object sender, EventArgs arg)
        {
            if (ViewState["ModelID"] == null) //判断是否是刷新页面引起的重复提交
            {
                if (txtValidate != null)
                {
                    if (String.Compare(txtValidate.Text.Trim(), Request.Cookies["CheckCode"].Value, true) == 0)
                    {
                        try
                        {
                            OnButtonSubmit(sender, arg);
                            if (FormPanel != null)
                                FormPanel.Visible = false;
                            if (SuccessPanel != null)
                                SuccessPanel.Visible = true;
                            else
                                Alert("您的信息已成功提交。");
                            ViewState["ModelID"] = PanelContext.Row["ID"];
                        }
                        catch (Exception ex)
                        {
                            Alert("信息提交失败：" + ex.Message + "!");
                            if (SuccessPanel != null) SuccessPanel.Visible = true;
                        }
                    }
                    else
                    {
                        Alert("验证码错误!");
                    }
                }
            }
        }

        #region 私有方法

        void Alert(string msg)
        {
            if (lblMsg != null)
            {
                lblMsg.Text = msg;
                lblMsg.Visible = true;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + msg + "')", true);
            }
        }

        #endregion

        #region 映射控件

        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.TextBox txtValidate;
        protected global::System.Web.UI.WebControls.Button bttnSave;
        protected global::System.Web.UI.WebControls.PlaceHolder phContainer;
        protected global::System.Web.UI.WebControls.Panel ctLayout;
        protected global::System.Web.UI.WebControls.Panel FormPanel;
        protected global::System.Web.UI.WebControls.Panel SuccessPanel;

        #endregion
    }
}
