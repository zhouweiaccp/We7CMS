using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.TemplateEnginer;

namespace We7.CMS.WebControls
{
    ///// <summary>
    ///// 控件包装器
    ///// </summary>
    //public class WrapperRender : IRender
    //{
    //    #region IRender 成员

    //    public void Render(RenderChain renders, ref string content, RenderArguments args)
    //    {
    //        if (!String.IsNullOrEmpty(args.Control.Wrapper) && !String.IsNullOrEmpty(args.Control.Wrapper.Trim()))
    //        {
    //            Dictionary<string, object> dic = new Dictionary<string, object>();
    //            dic.Add("MoreUrl", args.Control.NavigationUrl);
    //            dic.Add("Title", args.Control.HeadTitle);
    //            dic.Add("Content", content);
    //            content = NVelocityHelper.GetFormatString(We7.CMS.Constants.WidgetsWrapperFolderPhysicalDirectory, args.Control.Wrapper + ".vm", dic);
    //        }
    //        renders.DoRender(ref content, args);
    //    }

    //    #endregion
    //}
}
