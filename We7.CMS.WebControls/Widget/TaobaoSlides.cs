using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.WebControls
{
    public class TaobaoSlides : BaseWebControl
    {
        /// <summary>
        /// Css样式
        /// </summary>
        public string CssClass { get; set; }

        public int SLWidth { get; set; }

        public int SLHeight { get; set; }

        public int Interval { get; set; }

        public int Step { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IncludeJavaScript("TaobaoSlides.js");
        }
    }
}
