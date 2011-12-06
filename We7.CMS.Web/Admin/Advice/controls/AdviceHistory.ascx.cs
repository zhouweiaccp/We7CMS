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
using System.Collections.Generic;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceHistory : System.Web.UI.UserControl
    {
        private AdviceInfo advice;
        private AdviceType adviceType;
        private string adviceID;
        protected int SN = 1;
        private IAdviceHelper adviceHelper = AdviceFactory.Create();

        protected void Page_Load(object sender, EventArgs e)
        {

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

        protected AdviceType AdviceType
        {
            get
            {
                if (adviceType == null)
                {
                    adviceType = HelperFactory.Instance.GetHelper<AdviceTypeHelper>().GetAdviceType(Advice.TypeID) ?? new AdviceType();
                }
                return adviceType;
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

        private List<We7.CMS.Common.AdviceTransfer> trans;
        protected List<We7.CMS.Common.AdviceTransfer> Trans
        {
            get
            {
                if (trans == null)
                {
                    trans = adviceHelper.QueryTransferHistories(AdviceID);
                }
                return trans;
            }
        }

        protected string GetTypeName(string typeID)
        {
            AdviceType type = HelperFactory.Instance.GetHelper<AdviceTypeHelper>().GetAdviceType(typeID);
            return type != null ? type.Title : String.Empty;
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