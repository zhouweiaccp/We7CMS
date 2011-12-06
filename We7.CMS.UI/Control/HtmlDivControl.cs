using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace We7.CMS.Controls
{
    /// <summary>
    /// div标记类控件
    /// </summary>
    public class HtmlDivControl : HtmlContainerControl
    {
        string cssClass;

        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<DIV");
            if (this.cssClass != null)
            {
                writer.Write(String.Format(" class=\"{0}\"", cssClass));
            }
            writer.Write("/>");
            base.Render(writer);
            writer.Write("</DIV>");
        }
    }
}
