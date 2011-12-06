using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using We7.Framework.TemplateEnginer;
using System.Web.UI;
using System.IO;
using We7.CMS.WebControls.Core;

namespace We7.CMS.WebControls
{
    //public abstract partial class FrontUserControl
    //{
    //    /// <summary>
    //    /// 包装器路径
    //    /// </summary>
    //    [Option("KeyValueSelector", "vswrapper")]
    //    [Required]
    //    [Index(-3)]
    //    [Desc("外观", "定义部件最外围样式")]
    //    public string Wrapper { get; set; }

    //    /// <summary>
    //    /// more链接地址
    //    /// </summary>        
    //    [Option("String")]
    //    [Required]
    //    [Index(-1)]
    //    [Desc("导航地址", "定义用于包装器的导航地址")]
    //    public virtual string NavigationUrl { get; set; }

    //    /// <summary>
    //    /// 头部标题
    //    /// </summary>        
    //    [Option("String")]
    //    [Required]
    //    [Index(-2)]
    //    [Desc("头部标题", "定义用于包装器的头部标题")]
    //    public virtual string HeadTitle { get; set; }

    //    /// <summary>
    //    /// 重写信息重现
    //    /// </summary>
    //    /// <param name="writer"></param>
    //    protected override void Render(HtmlTextWriter writer)
    //    {
    //        RenderArguments args = new RenderArguments(this, UxParams);

    //        StringWriter strWriter = new StringWriter();
    //        HtmlTextWriter tempWriter = new HtmlTextWriter(strWriter);
    //        try
    //        {
    //            base.Render(tempWriter);
    //        }
    //        catch (Exception ex)
    //        {
    //            args.Exception = ex;
    //            args.IsError = true;
    //        };

    //        string content = strWriter.ToString();
    //        new RenderChain().DoRender(ref content, args);
    //        writer.Write(content);
    //    }
    //}
}
