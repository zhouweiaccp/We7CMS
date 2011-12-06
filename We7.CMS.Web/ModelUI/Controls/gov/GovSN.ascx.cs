using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework;
using We7.Model.Core;
using We7.Model.Core.UI;
using We7.Model.Core.Data;
using Thinkment.Data;
using System.Data;
using We7.CMS.Accounts;
using We7.CMS;
using We7.CMS.Common.PF;

namespace We7.Model.UI.Controls.gov
{
    public partial class SN : We7FieldControl
    {

        int snLength = 5;

        public override void InitControl()
        {
            if (Value == null)
            {
                txtSN.Visible = false;
                ddlYear.Visible = true;

                for (int i = DateTime.Now.Year; i > 2000; i--)
                {
                    ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            else
            {
                txtSN.Visible = true;
                ddlYear.Visible = false;
                txtSN.Text = Value.ToString();
            }
            string snlength = Control.Params["SNLength"];
            if (String.IsNullOrEmpty(snlength) || !Int32.TryParse(snlength, out snLength))
            {
                snLength = 5;
            }
        }

        public override object GetValue()
        {
            if (!String.IsNullOrEmpty(txtSN.Text))
            {
                return txtSN.Text.Trim();
            }
            else
            {
                string sn = String.Empty;

                string departID = (GetRelationValue(Control.Params["DepartCtr"]) ?? String.Empty).ToString();
                string subjectCode = (GetRelationValue(Control.Params["SubjectCtr"]) ?? String.Empty).ToString();
                string year = ddlYear.SelectedValue;

                sn += GetDepartmentSN(departID).PadRight(7, '0') + "-";
                sn += subjectCode.PadRight(snLength, '0') + "-";
                sn += year + "-";
                sn += GetSN(sn).ToString().PadLeft(snLength, '0');
                return sn;
            }
        }

        private string GetDepartmentSN(string departID)
        {
            IAccountHelper helper = AccountFactory.CreateInstance();
            Department depart = helper.GetDepartment(departID, null);
            return depart != null ? depart.Number : String.Empty;
        }

        private int GetSN(string preSN)
        {
            ModelDBHelper helper = ModelDBHelper.Create(PanelContext.ModelName);
            Criteria c = new Criteria(CriteriaType.Like, Control.Name, preSN + "%");
            int count = helper.Count(c);
            if (count > 0)
            {
                DataTable dt = helper.Query(c, new List<Order> { new Order(Control.Name, OrderMode.Desc) }, 0, 1);
                string sn = dt.Rows[0][Control.Name].ToString();
                string[] ss=sn.Split('-');
                if (ss != null && ss.Length > 0)
                {
                    int n;
                    if (int.TryParse(ss[ss.Length-1], out n))
                    {
                        return ++n;
                    }
                }
            }
            return 1;
        }
    }
}