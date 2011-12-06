using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.UI;

namespace We7.CMS.WebControls
{
    public class ArticleEditorProvider : AutoEditor
    {
        protected override void InitContainer()
        {
            base.InitContainer();
            OnCommandComplete += new We7.Model.Core.ModelEventHandler(ArticleEditorProvider_OnCommandComplete);
        }

        protected void ArticleEditorProvider_OnCommandComplete(object sender, EventArgs args)
        {
        }

        public void OnValidateSubmit(object sender, EventArgs arg)
        {
            if (txtValidate != null)
            {
                if (String.Compare(txtValidate.Text.Trim(), Request.Cookies["CheckCode"].Value, true) == 0)
                {
                    try
                    {
                        OnButtonSubmit(sender, arg);
                        Alert("添加成功");
                    }
                    catch (Exception ex)
                    {
                        Alert("提交失败：" + ex.Message + "!");
                    }
                }
                else
                {
                    Alert("验证码错误!");
                }
            }
        }

        #region 私有方法

        void Alert(string msg)
        {
            if (lblMsg != null)
            {
                lblMsg.Text = msg;
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

        #endregion

    }
}
