using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;

using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// 模板板本业务类
    /// </summary>
    [Serializable]
    [Helper("We7.TemplateVersionHelper")]
    public class TemplateVersionHelper : BaseHelper
    {
        /// <summary>
        /// 版本信息文件路径
        /// </summary>
        public string TemplateVersionFileName
        {
            get { return  Constants.TemplateVersionFileName; }
        }

        /// <summary>
        /// 保存版本信息
        /// </summary>
        /// <param name="tv">版本信息</param>
        public void SaveTemplateVersion(TemplateVersion tv)
        {
            string target = Path.Combine(tv.BasePath, tv.FileName);
            tv.ToFile(target);
        }
        /// <summary>
        /// 取得版本信息
        /// </summary>
        /// <param name="tvPath">版本信息文件路径</param>
        /// <returns></returns>
        public TemplateVersion GetTemplateVersion(string tvPath)
        {
            string templateVersionPath = tvPath;

            if (File.Exists(Path.Combine(templateVersionPath, TemplateVersionFileName)))
            {
                TemplateVersion t = new TemplateVersion();
                t.FromFile(templateVersionPath,TemplateVersionFileName);
                return t;
            }
            return null;
        }
    }
}
