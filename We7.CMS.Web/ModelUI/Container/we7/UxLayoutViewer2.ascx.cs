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
using We7.Model.Core;
using We7.Model.Core.UI;
using We7.Framework;

namespace We7.Model.UI.Container.we7
{
    public partial class UxLayoutViewer2 : ViewerContainer
    {
        LayoutEditor editor;

        protected override void InitContainer()
        {
            if (editor == null)
            {
                editor = UIHelper.LoadLayoutEditor(PanelContext.Panel.EditInfo.ViewerLayout);
                editor.ID = "UxLayoutCtr";
                editor.IsViewer = true;
                UxLayout.Controls.Clear();
                UxLayout.Controls.Add(editor);
            }
            editor.InitLayout(PanelContext);
        }
    }
}