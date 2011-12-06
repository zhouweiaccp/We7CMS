using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace We7.CMS.Module.VisualTemplate.Utils
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class ToolHelper
    {
        public static string GeneratorId(string prix)
        {
            if (string.IsNullOrEmpty(prix))
            {
                prix = "we7generator";
            }

            Guid guid = Guid.NewGuid();

            return string.Format("{0}_{1}",prix,guid.ToString().Replace("-",string.Empty));
        }

        /// <summary>
        /// 根据路径获取控件名称
        /// </summary>
        /// <param name="controlPath"></param>
        /// <returns></returns>
        public static string GetControrolType(string controlPath)
        {
            return Path.GetFileNameWithoutExtension(controlPath);
        }

        /// <summary>
        /// 转换控件名称
        /// </summary>
        /// <param name="controlType"></param>
        /// <returns></returns>
        public static string ConvertUC(string controlType)
        {
            return controlType.Replace('.', '_');
        }

      
    }
}
