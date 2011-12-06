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
using We7.CMS.Common;
using We7.Model.UI.Panel.system;
using System.Collections.Generic;
using We7.CMS.Accounts;

namespace We7.CMS.Web.Admin
{
    /// <summary>
    /// 受理页面（用户须具胡Permission.Accept且反馈状态为0：未处理）
    /// </summary>
    public partial class AdviceDistribute : BasePage
    {
        #region 属性字段
        private AdviceInfo advice;
        private string adviceID;
        private IAdviceHelper adviceHelper = AdviceFactory.Create();

        /// <summary>
        /// 反馈类型
        /// </summary>
        public string TypeID { get { return Request["typeID"]; } }

        /// <summary>
        /// 权限列表
        /// </summary>       
        protected List<string> Permissions
        {
            get
            {
                if (permissions == null)
                {
                    permissions = adviceHelper.GetPermissions(TypeID, Security.CurrentAccountID);
                }
                return permissions;
            }
        }
        List<string> permissions;
        #endregion

        /// <summary>
        /// 加载时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ButtonHandle();
            BindAdviceInfo();            
            NameLabel.Text = AdviceType.Title+"受理中心";
            SummaryLabel.Text = AdviceType.Description;
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        private AdviceType adviceType;
        public AdviceType AdviceType
        {
            get
            {
                if (adviceType == null)
                {
                    adviceType = adviceHelper.GetAdviceType(Advice.TypeID);
                }
                return adviceType;
            }
        }

        protected string AdviceID
        {
            get
            {
                if (String.IsNullOrEmpty(adviceID))
                {
                    adviceID = Request["ID"];
                }
                return adviceID;
            }
        }

        protected AdviceInfo Advice
        {
            get
            {
                if (advice == null)
                {
                    string id=Request["id"];
                    advice=adviceHelper.GetAdvice(id);
                }
                return advice;
            }
        }

        protected void lnkAccept_Click(object sender, EventArgs arg)
        {
            Response.Redirect("AdviceAccept.aspx?id="+AdviceID);
        }
        protected void lnkRefuse_Click(object sender, EventArgs arg)
        {
            Response.Redirect("AdviceRefuse.aspx?id=" + AdviceID);
        }
        protected void lnkProcess_Click(object sender, EventArgs arg)
        {
            Response.Redirect("AdviceProcessEx.aspx?id="+AdviceID);
        }
        protected void lnkTransfer_Click(object sender, EventArgs arg)
        {
            Response.Redirect("AdviceTransfer.aspx?id="+AdviceID);
        }
        protected void lnkBack_Click(object sender, EventArgs arg)
        {
            Response.Redirect("AdviceListEx.aspx?typeID="+Advice.TypeID);
        }

        void BindAdviceInfo()
        {
            if (Advice != null)
            {
                SimpleEditorPanel uc = this.LoadControl("/ModelUI/Panel/System/SimpleEditorPanel.ascx") as SimpleEditorPanel;
                uc.PanelName = "adminView";
                uc.ModelName = Advice.ModelName;
                uc.IsViewer = true;
                ModelDetails.Controls.Add(uc);
            }
        }

        /// <summary>
        /// 按钮显示逻辑
        /// </summary>
        void ButtonHandle()
        {
            bttnAccept.Visible = Permissions.Contains("Advice.Accept");
            lnkRefuse.Visible = Permissions.Contains("Advice.Accept");
            lnkProcess.Visible = Permissions.Contains("Advice.Handle");
            lnkTransfer.Visible = Permissions.Contains("Advice.Handle");
        }
    }
}