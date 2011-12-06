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
using We7.CMS.Common.Enum;
using We7.CMS.Helpers;
using We7.CMS.Common;
using We7.CMS.Common.PF;

namespace We7.CMS.Web.User
{
    public partial class PointAdd : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return  MasterPageMode.NoMenu;
            }
        }

        PointHelper PointHelper
        {
            get { return HelperFactory.GetHelper<PointHelper>(); }
        }

        string CurrentAccountID
        {
            get { return Request["id"]; }
        }

        PointAction Action
        {
            get
            {
                PointAction pa = PointAction.PointIn;
                if (Request["action"] != null && We7Helper.IsNumber(Request["action"].ToString()))
                    pa = (PointAction)int.Parse(Request["action"].ToString());
                return pa;
            }
        }

        public string ActionText
        {
            get
            {
                if (Action == PointAction.PointIn)
                    return "奖励";
                else
                    return "扣除";
            }
        }

        public Account ThisAccount
        {
            get
            {
                Account a = new Account();
                if (!string.IsNullOrEmpty(CurrentAccountID))
                    a = AccountHelper.GetAccount(CurrentAccountID, null);
                return a;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void bttnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int pvalue=int.Parse(PointValueTextBox.Text);
                if (Action == PointAction.PointOut &&  pvalue> ThisAccount.Point)
                    Messages.ShowError("没有足够的积分可以扣除！");
                else
                {
                    Account a = ThisAccount;
                    Point p = new Point();
                    p.Action = (int)Action;
                    p.Created = DateTime.Now;
                    p.AccountID = CurrentAccountID;
                    p.AccountName = a.LastName;
                    p.Description = DescTextBox.Text;
                    p.Value = int.Parse(PointValueTextBox.Text);
                    PointHelper.AddPoint(p);

                    if (Action == PointAction.PointOut)
                        a.Point -=  pvalue;
                    else
                        a.Point += pvalue;
                    AccountHelper.UpdateAccount(a, new string[] { "Point" });

                    Messages.ShowMessage(ActionText + "积分操作成功！");

                }
            }
            catch (Exception ex)
            {
               Messages.ShowError("添加失败！详细信息：" + ex.Message);
            }
        }

        protected void bttnReset_Click(object sender, EventArgs e)
        {
            //Response.Redirect("PointList.aspx");
        }
    }
}
