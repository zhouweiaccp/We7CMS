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
    public partial class AdviceProcessEx : BasePage
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

        protected void Page_Load(object sender, EventArgs e)
        {
            NameLabel.Text = AdviceType.Title + "办理中心";
            SummaryLabel.Text = AdviceType.Description;
            BindAdviceInfo();
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
                    string id = Request["id"];
                    advice = adviceHelper.GetAdvice(id);
                }
                return advice;
            }
        }

        protected void lnkRefuse_Click(object sender, EventArgs arg)
        {
            Response.Redirect("AdviceRefuse.aspx?id=" + AdviceID);
        }
        protected void lnkProccess_Click(object sender, EventArgs arg)
        {
            try
            {
                adviceHelper.ReplyAdvice(AdviceID, fckContent.Value.Trim());
                ProccessMsg.Redirect(1, Advice.TypeID, "处理成功！");
            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            catch (Exception ex)
            {
                ProccessMsg.Redirect(0, Advice.TypeID, "应用程序错误！错误原因：" + ex.Message);
            }
        }
        protected void lnkTransfer_Click(object sender, EventArgs arg)
        {
            Response.Redirect("AdviceTransfer.aspx?id=" + AdviceID);
        }
        protected void lnkBack_Click(object sender, EventArgs arg)
        {
            Response.Redirect("AdviceListEx.aspx?typeID=" + Advice.TypeID);
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
            lnkProccess.Visible = Permissions.Contains("Advice.Handle");
            lnkTransfer.Visible = Permissions.Contains("Advice.Handle");
            lnkRefuse.Visible = permissions.Contains("Advice.Accept");
        }
    }
}
