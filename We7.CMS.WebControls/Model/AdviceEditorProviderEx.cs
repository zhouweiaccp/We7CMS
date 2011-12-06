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
using System.IO;

namespace We7.CMS.WebControls
{
    /*public class AdviceEditorProviderEx : AutoEditor
    {
        private string adviceTypeID;
        private string queryKey = "advicetype";
        IAdviceHelper helper = AdviceFactory.Create();

        protected AdviceTypeHelper AdviceTypeHelper
        {
            get { return HelperFactory.Instance.GetHelper<AdviceTypeHelper>(); }
        }

        public string CssClass { get; set; }

        private string modelName;
        public override string ModelName
        {
            get
            {
                if (String.IsNullOrEmpty(modelName))
                {
                    AdviceType advicetype = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
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

        /// <summary>
        /// 反馈类型
        /// </summary>
        public virtual string AdviceTypeID
        {
            get
            {
                if (String.IsNullOrEmpty(adviceTypeID))
                {
                    string query = Request[QueryKey];
                    if (!String.IsNullOrEmpty(query))
                    {
                        query = query.Trim().ToLower();
                        string path = We7Utils.GetMapPath("/Config/advicemapping.xml");
                        if (File.Exists(path))
                        {
                            XmlNode node = XmlHelper.GetXmlNode(path, "//item[@key='" + query + "']");
                            if (node != null)
                            {
                                XmlElement xe = node as XmlElement;
                                adviceTypeID = (xe.GetAttribute("value") ?? "").Trim();
                            }
                        }
                    }
                    if (String.IsNullOrEmpty(adviceTypeID))
                    {
                        //throw new Exception("advicemapping.xml不存在对应的类型");
                        Server.Transfer("/admin/Error.html");
                        //log
                    }
                }
                return adviceTypeID;
            }
            set { adviceTypeID = value; }
        }


        protected override void InitContainer()
        {
            base.InitContainer();
            OnCommandComplete += new We7.Model.Core.ModelEventHandler(AdviceEditorProvider_OnCommandComplete);
            if(bttnUpdate!=null)
            bttnUpdate.Click += new EventHandler(bttnUpdate_Click);
            //if (Request.QueryString["success"] != null)
            //{
            //    SuccessPanel.Visible = true;
            //    FormPanel.Visible = false;
            //}
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            AdviceInfo a =helper.GetAdvice(AdviceID);
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
        //protected global::System.Web.UI.WebControls.Panel FormPanel;
        //protected global::System.Web.UI.WebControls.Panel SuccessPanel;

    }*/
}
