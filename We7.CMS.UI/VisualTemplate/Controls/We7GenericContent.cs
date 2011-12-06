using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace We7.CMS.Module.VisualTemplate.Controls
{
    /// <summary>
    /// GenricContent
    /// </summary>
    [ParseChildren(false), PersistChildren(true)]
    public class We7GenericContent:PlaceHolder
    {
        /// <summary>
        /// 是否是设计模式
        /// </summary>
        public bool Design
        {
            get { return Context.Request["design"] != null; }
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (Design)
            {
                if (string.IsNullOrEmpty(ID))
                {
                    Random r = new Random();
                    int id = r.Next(1000, 10000000);
                    ID = string.Format("GenericContent_{0}", id);
                }
                writer.Write("<div class=\"GenericContent\" id=\"" + ID + "\" controlid=\"" + this.ID + "\" controltype=\"GenericContent\">");
                base.Render(writer);
                writer.Write("</div>");
            }
            else
            {
                base.Render(writer);
            }
        }
    }
}
