using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.UI;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using System.Xml;
using We7.CMS.Common;
using System.Web;
using We7.Model.Core;

namespace We7.CMS.WebControls
{
    public class AdviceEditorProvider : AutoEditor
    {
        /// <summary>
        /// 业务助手工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)(HttpContext.Current.Application[HelperFactory.ApplicationID]); }
        }

        /// <summary>
        /// 用户管理业务助手
        /// </summary>
        protected AdviceHelper AdviceHelper
        {
            get { return HelperFactory.GetHelper<AdviceHelper>(); }
        }

        protected AdviceTypeHelper AdviceTypeHelper
        {
            get { return HelperFactory.GetHelper<AdviceTypeHelper>(); }
        }

        public string CssClass { get; set; }

        private string modelName;
        public override string ModelName
        {
            get
            {
                if (String.IsNullOrEmpty(modelName))
                {
                    AdviceType advicetype = AdviceTypeHelper.GetAdviceType(ModelTypeID);
                    if (advicetype == null)
                        throw new Exception("不存在当前反馈类型");
                    modelName = advicetype.ModelName;
                }
                return modelName;
            }
            set
            {
                modelName = value;
            }
        }

        private string modeltypeId;
        public string ModelTypeID
        {
            get { return modeltypeId; }
            set
            {
                modeltypeId = value;
            }
        }

        string AdviceID
        {
            get { return ViewState["$AdviceID"] as string; }
            set { ViewState["$AdviceID"] = value; }
        }

        protected override void InitContainer()
        {
            base.InitContainer();
            OnCommandComplete += new We7.Model.Core.ModelEventHandler(AdviceEditorProvider_OnCommandComplete);
            if(bttnUpdate!=null)
            bttnUpdate.Click += new EventHandler(bttnUpdate_Click);
            PanelContext.CtrVersion = CtrVersion.V26;
            //if (Request.QueryString["success"] != null)
            //{
            //    SuccessPanel.Visible = true;
            //    FormPanel.Visible = false;
            //}
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            Advice a = new Advice();
            a.ID = AdviceID;
            a.MyQueryPwd = txtPwd.Text.Trim();
            AdviceHelper.UpdateAdvice(a, new string[] { "MyQueryPwd" });
            Alert2("修改成功");
        }

        public void OnValidateSubmit(object sender, EventArgs arg)
        {
            if (txtValidate != null)
            {
                if (String.Compare(txtValidate.Text.Trim(), Request.Cookies["CheckCode"].Value, true) == 0)
                {
                    try
                    {
                        if (!PanelContext.Objects.ContainsKey("AdviceTypeID"))
                        {
                            PanelContext.Objects.Add("AdviceTypeID", ModelTypeID);
                        }
                        else
                        {
                            PanelContext.Objects["AdviceTypeID"] = ModelTypeID;
                        }
                        OnButtonSubmit(sender, arg);
                    }
                    catch (Exception ex)
                    {
                        Alert("提交失败：" + ex.Message+"!");
                    }
                }
                else
                {
                    Alert("验证码错误!");
                }
            }
        }

        void AdviceEditorProvider_OnCommandComplete(object sender, We7.Model.Core.ModelEventArgs args)
        {
            if (txtPwd != null && mvAdvice != null)
            {
                mvAdvice.ActiveViewIndex = 1;
                string id = args.PanelContext.Row["ID"] as string;
                if (!String.IsNullOrEmpty(id))
                {
                    Advice advice = AdviceHelper.GetAdvice(id,null);
                    lblPwd.Text = advice.MyQueryPwd;
                    lblSN.Text = advice.SN.ToString();
                    AdviceID = id;
                }
            }
            else if (lblSN != null)
            {
                mvAdvice.ActiveViewIndex = 1;
                string id = args.PanelContext.Row["ID"] as string;
                if (!String.IsNullOrEmpty(id))
                {
                    Advice advice = AdviceHelper.GetAdvice(id, null);
                    lblSN.Text = advice.SN.ToString();
                    AdviceID = id;
                }
            }
            else
            {
                Alert("提交成功");
            }
        }

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
                if (lblMsg2 != null)
                    lblMsg2.Text = "";
        }

        void Alert2(string msg)
        {
            if (lblMsg2 != null)
            {
                lblMsg2.Text = msg;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + msg + "')", true);
            }
            if (lblMsg != null)
                lblMsg.Text = "";
        }

        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.Label lblMsg2;
        protected global::System.Web.UI.WebControls.TextBox txtValidate;

        protected global::System.Web.UI.WebControls.TextBox txtPwd;
        protected global::System.Web.UI.WebControls.Label lblPwd;
        protected global::System.Web.UI.WebControls.Label lblSN;
        protected global::System.Web.UI.WebControls.Button bttnSave;
        protected global::System.Web.UI.WebControls.Button bttnUpdate;
        protected global::System.Web.UI.WebControls.MultiView mvAdvice;
        //protected global::System.Web.UI.WebControls.Panel FormPanel;
        //protected global::System.Web.UI.WebControls.Panel SuccessPanel;

    }
}
