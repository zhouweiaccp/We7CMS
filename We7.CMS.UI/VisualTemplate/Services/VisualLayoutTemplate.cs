using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Module.VisualTemplate.Models;
using System.Web;
using We7.Framework.Util;

namespace We7.CMS.Module.VisualTemplate.Services
{
   

    /// <summary>
    /// 网页布局模板服务类
    /// </summary>
    public class VisualLayoutTemplate
    {
        /// <summary>
        /// 系统模板虚拟路径
        /// </summary>
        private const string SystemTemplateConfigVirtualPath = "~/Admin/VisualTemplate/Config/LayoutTemplate.xml";

        /// <summary>
        /// 获取系统模板
        /// </summary>
        /// <returns></returns>
        public TemplateList GetSystemTemplates()
        {
            //绝对路径
            string path = HttpContext.Current.Server.MapPath(SystemTemplateConfigVirtualPath);

            if (FileHelper.Exists(path))
            {
                try
                {
                   var templates= SerializationHelper.Load<TemplateList>(path);

                   return templates;
                }
                catch 
                {
                    throw new Exception("系统模板文件格式不正确!");
                }

            }
            else
            {
                throw new Exception("系统模板文件不存在!");
            }

        }
    }
}
