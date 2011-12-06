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

namespace We7.CMS.Web.Admin
{
    public partial class AdviceTransfer : BasePage
    {
        private AdviceInfo advice;
        private string adviceID;
        private IAdviceHelper adviceHelper = AdviceFactory.Create();

        protected void Page_Load(object sender, EventArgs e)
        {
            NameLabel.Text = AdviceType.Title + "办理中心";
            SummaryLabel.Text = AdviceType.Description;
            if (!IsPostBack)
            {
                BindData();
            }
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

        void BindData()
        {
            rdAdviceTypeList.DataSource = adviceHelper.GetRelatedAdviceTypes(AdviceID);
            rdAdviceTypeList.DataTextField = "Title";
            rdAdviceTypeList.DataValueField = "ID";
            rdAdviceTypeList.DataBind();
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

        protected void lnkTransfer_Click(object sender, EventArgs arg)
        {
            try
            {
                if (!String.IsNullOrEmpty(rdAdviceTypeList.SelectedValue))
                {
                    string typeID = Advice.TypeID; //防止值在转发后发生变化
                    adviceHelper.TransferAdvice(AdviceID, rdAdviceTypeList.SelectedValue,fckContent.Value.Trim());
                    ProccessMsg.Redirect(1, typeID, "转办成功！");
                }
                else
                {
                    Messages.ShowError("请选择转发项");
                }

            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            catch (Exception ex)
            {
                Messages.ShowError("应用程序错误！" + ex.Message);
            }
        }

        protected void lnkBack_Click(object sender, EventArgs arg)
        {
            Response.Redirect("AdviceListEx.aspx?typeID=" + Advice.TypeID);
        }

    }
}
