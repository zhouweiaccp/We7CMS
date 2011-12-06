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
using System.Drawing;

namespace We7.Model.UI.Container.system
{
    public partial class SimplenNavigation : CommandContainer
    {
        protected override void LoadContainer()
        {
            base.LoadContainer();
            foreach (Control c in Controls)
            {
                if (c is LinkButton)
                {
                    ((LinkButton)c).Click += new EventHandler(OnButtonSubmit);
                }
            }
        }

        protected new void OnButtonSubmit(object sender,EventArgs args)
        {
            ResetColor(sender);
            base.OnButtonSubmit(sender, args);
        }
        void ResetColor(object sender)
        {
            foreach (Control c in Controls)
            {
                if (c is LinkButton)
                {

                    ((LinkButton)c).ForeColor =c==sender?Color.Red:Color.Black;
                }
            }
        }
    }
}