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
using System.Text;
using System.Collections.Generic;
using We7.Model.UI.Panel.system;
using We7.CMS.Common.PF;
using We7.Framework.Config;


namespace We7.CMS.Web.Admin
{
    public partial class AdviceView : BasePage
    {

        private AdviceInfo advice;
        protected AdviceReplyInfo adviceReplyInfo;
        private string adviceID;
        private IAdviceHelper adviceHelper = AdviceFactory.Create();
        private AdviceTypeHelper adviceTypeHelper = new AdviceTypeHelper();

        private List<AdviceReplyInfo> adviceReplyInfoList;
        protected IAdviceTransferHelper adviceTransferHelper = AdviceTransferFactory.Create();


        protected void Page_Load(object sender, EventArgs e)
        {

            NameLabel.Text = AdviceType.Title + "信息";
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


        public List<AdviceReplyInfo> Replies
        {
            get
            {
                if (adviceReplyInfoList == null)
                {
                    adviceReplyInfoList = adviceHelper.QueryReplies(AdviceID);
                }
                return adviceReplyInfoList;
            }
        }

        public string GetNameByUserID(string userid)
        {
            if (userid == We7Helper.EmptyGUID)
            {
                return SiteConfigs.GetConfig().AdministratorName;
            }
            else
            {
                Account act = AccountFactory.CreateInstance().GetAccount(userid, null);
                return act != null ? String.Format("{0}{1}{2}", act.FirstName, act.MiddleName, act.LastName) : String.Empty;
            }
        }
        protected AdviceReplyInfo AdviceReplyInfo
        {
            get
            {
                if (adviceReplyInfo == null)
                {

                }
                return null;
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
                    advice = adviceHelper.GetAdvice(AdviceID);
                }
                return advice;
            }
        }

        protected void lnkBack_Click(object sender, EventArgs arg)
        {
            if (!String.IsNullOrEmpty(Request["typeID"]))
            {
                Response.Redirect("AdviceListEx.aspx?typeID=" + Request["typeID"]);
            }
            else
            {
                Response.Redirect("AdviceListEx.aspx?typeID=" + Advice.TypeID);
            }
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

        protected string FormatDate(object o)
        {
            if (o != null)
            {
                DateTime dt = (DateTime)o;
                return dt.Year == DateTime.Now.Year ? dt.ToString("M月d日 HH时mm分") : dt.ToString("yyyy-MM-dd HH:mm");
            }
            return String.Empty;
        }
    }
}
