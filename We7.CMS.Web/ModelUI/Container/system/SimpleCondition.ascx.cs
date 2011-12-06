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
using We7.Model.Core.UI;
using We7.Model.Core;

namespace CModel.Container.system
{
    public partial class SimpleCondition:ConditionContainer
    {
        protected override void InitContainer()
        {
            foreach (We7Control field in Panel.ConditionInfo.Controls)
            {
                TableHeaderCell cell = new TableHeaderCell();
                cell.Text = field.Label;
                cell.Visible = field.Visible;
                cell.Width = Unit.Parse("50px");
                trQuery.Cells.AddAt(trQuery.Cells.Count - 1, cell);

                cell = new TableHeaderCell();
                cell.Visible = field.Visible;
                cell.Width = Unit.Parse("50px");
                FieldControl fc = UIHelper.GetControl(field);
                cell.Controls.Add(fc);

                trQuery.Cells.AddAt(trQuery.Cells.Count - 1, cell);
            }
            trQuery.Cells.Add(new TableCell());
        }

        public override void Refresh()
        {            
            OnButtonSubmit(bttnQuery, EventArgs.Empty);
        }
    }
}