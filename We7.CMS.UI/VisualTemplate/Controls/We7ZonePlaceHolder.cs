using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace We7.CMS.Module.VisualTemplate.Controls
{
    /// <summary>
    /// we7设计时Zone PlaceHolder
    /// </summary>
    [ParseChildren(false), PersistChildren(true)]
    public class We7ZonePlaceHolder : PlaceHolder
    {
        /// <summary>
        /// 是否是设计模式
        /// </summary>
        public bool Design
        {
            get { return Context.Request["state"] != null; }
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (Design)
            {
                if (string.IsNullOrEmpty(ID))
                {
                    throw new ArgumentNullException("ID不能为空!");
                }
                writer.Write("<div style=\"min-width: 10px; min-height: 10px;\" class=\"RadDockZone  \" id=\"" + ID + "\"  controlid=\"" + this.ID + "\" >");
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
