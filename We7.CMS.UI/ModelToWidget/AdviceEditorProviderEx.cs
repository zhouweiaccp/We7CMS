using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using System.Xml;
using System.IO;
using We7.Framework.Util;
using We7.Framework;
using Thinkment.Data;
using We7.CMS.WebControls.Core;

namespace We7.CMS.WebControls
{
    public class AdviceEditorProviderEx : AutoEditor
    {
        //private string adviceTypeID;
        private string queryKey = "advicetype";
        IAdviceHelper helper = AdviceFactory.Create();

        protected AdviceTypeHelper AdviceTypeHelper
        {
            get { return HelperFactory.Instance.GetHelper<AdviceTypeHelper>(); }
        }

        public string RelationModelName;

        public string CssClass { get; set; }

        private string modelName;
        public override string ModelName
        {
            get
            {
                if (String.IsNullOrEmpty(modelName))
                {
                    modelName = hdModelName.Value;
                }
                return modelName;
            }
            set
            {
                modelName = value;
            }
        }

        string AdviceID
        {
            get { return ViewState["$AdviceID"] as string; }
            set { ViewState["$AdviceID"] = value; }
        }

        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryKey
        {
            get { return queryKey; }
            set { queryKey = value; }
        }

        [Parameter(Title = "是否加密", Required = true, Type = "Boolean", DefaultValue = "false")]
        public bool Encryption;

        [Parameter(Title = "是否选择反馈类型", Required = true, Type = "Boolean", DefaultValue = "false")]
        public bool IsChoiceType;

        /// <summary>
        /// 审核类型为上级部门，不显示在选择列表中
        /// </summary>
        [Parameter(Title = "审核类型", Description = "此反馈类型不出现在前台列表中", Type = "KeyValueSelector", Data = "adviceTypeID", DefaultValue = "")]
        public string AuditType;

        /// <summary>
        /// 受理类型不为空，则隐藏反馈类型选择
        /// </summary>
        [Parameter(Title = "受理类型",Description="所有反馈都会先提交到这个类型下", Type = "KeyValueSelector", Data = "adviceTypeID", DefaultValue = "")]
        public string AcceptType;

        private string adviceTypeID;
        /// <summary>
        /// 反馈类型
        /// </summary>
        public virtual string AdviceTypeID
        {
            get
            {
                if (string.IsNullOrEmpty(AcceptType))
                    adviceTypeID = string.IsNullOrEmpty(hdAdvice.Value) ? drpAdvice.SelectedValue : hdAdvice.Value;
                else
                    adviceTypeID = AcceptType;
                return adviceTypeID;
            }
        }

        private List<AdviceType> adviceList;
        protected void BindAdviceTypeList()
        {
            if (adviceList == null)
            {
                Criteria c = new Criteria(CriteriaType.Equals, "ModelName", ModelName);
                adviceList = Assistant.List<AdviceType>(c, null);
            }

            if (!string.IsNullOrEmpty(AuditType))
            {
                for (int i = adviceList.Count - 1; i >= 0; i--)
                {
                    if (adviceList[i].ID.Equals(AuditType))
                    {
                        adviceList.RemoveAt(i);
                    }
                }
            }

            drpAdvice.DataSource = adviceList;
            drpAdvice.DataBind();
        }

        protected override void InitContainer()
        {
            base.InitContainer();

            BindAdviceTypeList();

            OnCommandComplete += new We7.Model.Core.ModelEventHandler(AdviceEditorProvider_OnCommandComplete);
            if (bttnUpdate != null)
                bttnUpdate.Click += new EventHandler(bttnUpdate_Click);

            //受理类型不为空，则隐藏反馈类型选择
            trAdviceType.Visible = IsChoiceType && string.IsNullOrEmpty(AcceptType);
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            AdviceInfo a = helper.GetAdvice(AdviceID);
            a.MyQueryPwd = txtPwd.Text.Trim();
            helper.UpdateAdvice(a);
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
                            PanelContext.Objects.Add("AdviceTypeID", AdviceTypeID);
                        }
                        else
                        {
                            PanelContext.Objects["AdviceTypeID"] = AdviceTypeID;
                        }
                        OnButtonSubmit(sender, arg);
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

        void AdviceEditorProvider_OnCommandComplete(object sender, We7.Model.Core.ModelEventArgs args)
        {
            if (!Encryption)
            {
                Alert("提交成功");
            }
            else if (txtPwd != null && mvAdvice != null)
            {
                mvAdvice.ActiveViewIndex = 1;
                string id = args.PanelContext.Row["ID"] as string;
                if (!String.IsNullOrEmpty(id))
                {
                    AdviceInfo advice = helper.GetAdvice(id);
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
                    AdviceInfo advice = helper.GetAdvice(id);
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
        protected global::System.Web.UI.WebControls.DropDownList drpAdvice;
        protected global::System.Web.UI.WebControls.HiddenField hdAdvice;
        protected global::System.Web.UI.HtmlControls.HtmlTableRow trAdviceType;
        protected global::System.Web.UI.WebControls.HiddenField hdModelName;
        //protected global::System.Web.UI.WebControls.Panel FormPanel;
        //protected global::System.Web.UI.WebControls.Panel SuccessPanel;

    }
}
