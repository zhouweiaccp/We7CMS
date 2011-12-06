using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.UI;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace We7.Model.Core.ListControl
{
    public class UxField : ModelControlField
    {
        protected override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState)
        {
            if (cellType == DataControlCellType.DataCell)
            {
                Literal ltl = new Literal();
                ltl.DataBinding += new EventHandler(ltl_DataBinding);
                cell.Controls.Add(ltl);
            }
        }

        void ltl_DataBinding(object sender, EventArgs e)
        {
            Literal ltl = sender as Literal;
            object dataitem = DataBinder.GetDataItem(ltl.NamingContainer);
            IUxConvert uxConvert=UxConvertFactory.GetConvert(Column);
            if (uxConvert != null)
            {
                ltl.Text =uxConvert.GetText(dataitem,Column);
            }
        }

        protected override DataControlField CreateField()
        {
            return new HtmlField();
        }
    }
}
