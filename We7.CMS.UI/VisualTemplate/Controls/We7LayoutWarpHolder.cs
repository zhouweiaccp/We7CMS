using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using HtmlAgilityPack;
using System.IO;
using We7.Framework.TemplateEnginer;

namespace We7.CMS.Module.VisualTemplate.Controls
{
    /// <summary>
    /// 包括布局自定义控件
    /// </summary>
    [ParseChildren(false), PersistChildren(true)]
    public class We7LayoutWarpHolder : PlaceHolder
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

                var rendHtml = helper.Save("We7LayoutDesign.vm");
                //格式化
                rendHtml = FormatHtml(rendHtml);
                //输出代码
                writer.Write(rendHtml);
            }
            else
            {
                base.Render(writer);
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
