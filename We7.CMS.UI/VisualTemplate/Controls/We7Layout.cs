using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using We7.Framework.Util;
using System.IO;
using We7.Framework.TemplateEnginer;
using HtmlAgilityPack;
using We7.CMS.Module.VisualTemplate.Services;
using Newtonsoft.Json;
using We7.CMS.Module.VisualTemplate.Models.Temp;
namespace We7.CMS.Module.VisualTemplate.Controls
{
    [ParseChildren(false), PersistChildren(true)]
    public class We7Layout : Control
    {
        public We7Layout()
        {
            Design = !string.IsNullOrEmpty(RequestHelper.Get<string>("state"));
        }
        /// <summary>
        /// 是否是设计状态
        /// </summary>
        public bool Design
        {
            get;
            set;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        public string BuildData()
        {
            WidgetDesign design = new WidgetDesign();
            design.ID = ID;
            design.Tag = "we7layout";
            design.TagPrefix = "we7design";
            design.WidgetType = WidgetType.layout;

            foreach (Control col in this.Controls)
            {
                if (string.Compare(col.GetType().Name, "We7LayoutColumn", true) == 0)
                {
                    We7LayoutColumn colControl= col as  We7LayoutColumn;

                    Dictionary<string, object> column = new Dictionary<string, object>();
                    column.Add("id", colControl.ID);
                    column.Add("style", colControl.Style);
                    column.Add("width", colControl.Width);
                    column.Add("cssclass", colControl.CssClass);
                    column.Add("widthunit", colControl.WidthUnit);
                    design.Columns.Add(column);
                }
            }

            return JavaScriptConvert.SerializeObject(design);
            
        }
        protected override void Render(HtmlTextWriter writer)
        {

            if (Design)
            {   
                //可视化设计时
                StringWriter output = new StringWriter();
                HtmlTextWriter tw = new HtmlTextWriter(output);
                base.Render(tw);
                string ControlHtml = output.ToString();
                //格式化代码
                string formatControlHtml = FormatHtml(ControlHtml);

                NVelocityHelper helper = new NVelocityHelper(We7.CMS.Constants.VisualTemplatePhysicalTemplateDirectory);

                helper.Put("controlId", this.ID);
                helper.Put("controlContent", formatControlHtml);
                helper.Put("controlData", BuildData());
                var rendHtml = helper.Save("We7LayoutDesign.vm");
                //格式化
                rendHtml = FormatHtml(rendHtml);
                //输出代码
                writer.Write(rendHtml);
            }
            else
            {
                writer.Write("<div id=\""+this.ID+"\" class=\"sf_cols\">");
                base.Render(writer);
                //writer.Write("<div style= \"clear:both; height:0;\"></div>");
                writer.Write("<div style= \"clear:both;height:0;font-size:1px;\"></div>");
                writer.Write("</div>");

            }

        }
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        protected virtual string FormatHtml(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.OptionAutoCloseOnEnd = true;
            doc.OptionCheckSyntax = true;
            doc.OptionOutputOriginalCase = true;
            try
            {
                doc.LoadHtml(html);
            }
            catch
            {
                throw new Exception("格式化HTML错误");
            }
            StringWriter output = new StringWriter();
            doc.Save(output);

            return output.ToString();
        }

    }
}
