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
    public partial class AdviceRefuse : BasePage
    {
        private AdviceInfo advice;
        private string adviceID;
        private IAdviceHelper adviceHelper = AdviceFactory.Create();

        protected void Page_Load(object sender, EventArgs e)
        {
            NameLabel.Text = AdviceType.Title + "办理中心";
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
                    string id = Request["id"];
                    advice = adviceHelper.GetAdvice(id);
                }
                return advice;
            }
        }

        protected void lnkRefuse_Click(object sender, EventArgs arg)
        {
            try
            {
                adviceHelper.RefuseAdvice(AdviceID, fckContent.Value.Trim());
                ProccessMsg.Redirect(1, Advice.TypeID, "操作成功！当前信息不在受理范围内！");
            }
            catch (System.Threading.ThreadAbortException ex)
            {
            }
            catch (Exception ex)
            {
                ProccessMsg.Redirect(0, Advice.TypeID, "应用程序错误！" + ex.Message);
            }
        }
        protected void lnkBack_Click(object sender, EventArgs arg)
        {
            Response.Redirect("AdviceListEx.aspx?typeID=" + Advice.TypeID);
        }

    }
}
