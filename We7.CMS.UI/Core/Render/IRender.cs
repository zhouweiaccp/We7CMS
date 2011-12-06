using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using We7.Framework;
using System.IO;
using We7.Framework.TemplateEnginer;
using We7.Framework.Util;
using System.Xml;

namespace We7.CMS.WebControls
{
    /// <summary>
    /// Html呈现格式化
    /// </summary>
    public interface IRender
    {
        /// <summary>
        /// 数据呈现处理
        /// </summary>
        /// <param name="renders">数据呈现链</param>
        /// <param name="content">进行处理的内容</param>
        /// <param name="args">系统传入的参数</param>
        void Render(RenderChain renders, ref string content, RenderArguments args);
    }


}
