using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Util;
using We7.CMS.Module.VisualTemplate.Services;
using Newtonsoft.Json;
using We7.CMS.Module.VisualTemplate.Models.Temp;
using System.Web.UI;
using System.IO;

namespace We7.CMS.Module.VisualTemplate.Controls
{
    public enum Float
    {
        left,
        right,
        none
    }

    [ParseChildren(false),PersistChildren(true)]
    public class We7LayoutColumn:System.Web.UI.Control
    {
        public We7LayoutColumn()
        {
            Design = !string.IsNullOrEmpty(RequestHelper.Get<string>("state"));
            Float = Float.none;
            Width = "100";
            CssClass = string.Empty;
            WidthUnit = "%";
            Style = string.Empty;
            
        }
        /// <summary>
        /// 是否是设计状态
        /// </summary>
        public bool Design
        {
            get;
            set;
        }

        public string Width
        {
            get;
            set;
        }
        public Float Float
        {
            get;
            set;
        }

        public string Style
        {
            get;
            set;
        }

        public string CssClass
        {
            get;
            set;
        }

        public string WidthUnit
        {
            get;
            set;
        }
        protected string BuildData()
        {
            WidgetDesign design = new WidgetDesign();
            design.ID = ID;
            design.WidgetType = WidgetType.layoutColumn;
            design.TagPrefix = "we7design";
            design.Tag = "We7LayoutColumn";
            design.Params.Add("id", ID);
            design.Params.Add("width", Width);
            design.Params.Add("float", Float);
            design.Params.Add("style", Style);
            design.Params.Add("cssclass", CssClass);
            design.Params.Add("widthunit", WidthUnit);
            return JavaScriptConvert.SerializeObject(design);

        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (Design)
            {
                #region mxy修改(2011-03-22)
                writer.Write("<div style=\"float: " + Float + "; width: " + Width + WidthUnit + "; margin: 0;" + Style + ";\" class=\"sf_colsOut\">");
                writer.Write("<div  class=\"sf_colsIn " + CssClass + "\">");// style=\" "+Style+"\"
                #endregion
                writer.Write("<div style=\"min-width: 10px; min-height: 10px;\" class=\"RadDockZone \" id=\"" + ID + "\"controlid=\""+ID+"\">");
                base.Render(writer);
                writer.Write("</div>");
                writer.Write("</div>");
                writer.Write("<input type=\"hidden\" id=\""+ID+"_ClientState\" name=\""+ID+"_ClientState\" value='"+BuildData()+"'");
                writer.Write("</div>");
            }
            else
            {
                writer.Write("<div style=\"float: " + Float + "; width: " + Width + WidthUnit + "; margin: 0; " + Style + ";\" class=\"sf_colsOut\">");
                writer.Write("<div class=\"sf_colsIn " + CssClass + "\">");// style=\"" + Style + "\"
                base.Render(writer);
                writer.Write("</div>");
                writer.Write("</div>");
            }
           
        }

    }
}
