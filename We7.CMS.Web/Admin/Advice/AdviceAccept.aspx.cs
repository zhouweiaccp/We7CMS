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
    public partial class AdviceAccept : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request["id"];
                string type = Request["type"];
                if (!String.IsNullOrEmpty(id))
                {
                    try
                    {
                        IAdviceHelper helper = AdviceFactory.Create();
                        AdviceInfo advice = helper.GetAdvice(id);
                        if (advice != null)
                        {
                            helper.UpdateAdviceState(id, 2);
                            ProccessMsg.Redirect(1, advice.TypeID, "受理成功！");
                        }
                        else
                        {
                            ProccessMsg.Redirect(0, advice.TypeID, "当前记录不存在！");
                        }
                    }
                    catch (System.Threading.ThreadAbortException ex)
                    {
                    }
                    catch (Exception ex)
                    {
                        ProccessMsg.Redirect(0, String.Empty, "应用程序错误！错误原因：" + ex.Message);
                    }
                }
                else
                {
                    ProccessMsg.Redirect(2,String.Empty, "当前记录不存在！");
                }
            }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }
    }
}
