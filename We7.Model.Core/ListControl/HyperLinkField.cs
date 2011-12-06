using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core.UI;
using We7.Model.Core;

namespace We7.Model.Core.ListControl
{
    public class HyperLinkField:ModelControlField
    {
        protected override DataControlField CreateField()
        {
            HyperLinkField field = new HyperLinkField();
            CopyProperties(field);
            return field;
        }

        protected override void InitializeCell(DataControlFieldCell cell,DataControlCellType cellType,DataControlRowState rowState)
        {
            if (cellType == DataControlCellType.DataCell)
            {               
                HyperLink lnk = new HyperLink();
                lnk.DataBinding += new EventHandler(lnk_DataBinding);
                cell.Controls.Add(lnk);
            }
        }

        void lnk_DataBinding(object sender, EventArgs e)
        {
            HyperLink lnk = sender as HyperLink;
            lnk.Target = Column.Params["target"];
            object dataitem = DataBinder.GetDataItem(lnk.NamingContainer);

            string urlformat = Column.Params["urlformat"];
            string[] urlfields = Column.Params["urlfields"].Split(',');
            object[] os = new object[urlfields.Length];

            for (int i = 0; i < os.Length; i++)
            {
                os[i] = GetValue(dataitem, urlfields[i]);
            }
                    
            lnk.NavigateUrl =String.Format(urlformat,os);

            string textformat = Column.Params["textformat"];
            string[] textfields = Column.Params["textfields"].Split(',');
            os = new object[textfields.Length];
            for (int i = 0; i < os.Length; i++)
            {
                os[i] = GetValue(dataitem, textfields[i]);
            }
            lnk.Text = String.Format(textformat,os);
        }
    }
}
