using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.CMS.WebControls.Core;
using System.Text.RegularExpressions;
using System.IO;

namespace We7.CMS.UI.Template
{
    public class WidgetTemplateProcessorHelper : ITemplateProcessorHelper
    {
        BaseControlHelper BCHelper = new BaseControlHelper();
        DataControlHelper DCHelper = new DataControlHelper();

        public DataControl GetDataControlByPath(string path)
        {
            if (BCHelper.IsControl(path))
            {
                return DCHelper.GetDataControlByPath(path);
            }
            else
            {
                DataControlInfo info = BCHelper.GetWidgetControlInfo(path);
                return info != null && info.Controls.Count > 0 ? info.Controls[0] : null;
            }           
        }

        public DataControlInfo GetDataControlInfoByPath(string path)
        {
            if (BCHelper.IsControl(path))
            {
                return DCHelper.GetDataControlInfoByPath(path);
            }
            else
            {
                return BCHelper.GetWidgetControlInfo(path);
            }            
        }

        public string GetTemplatePath(string fileName)
        {
            Regex regex = new Regex(@"(?<=/)\w+/Page/.*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match mc = regex.Match(fileName);
            if (mc.Success)
            {
                return "We7Controls/" + mc.Value;
            }
            else if (fileName.StartsWith("/Widgets/", StringComparison.OrdinalIgnoreCase))
            {
                return fileName;
            }
            else
            {
                return String.Empty;
            }
        }

        public string GetUCName(string filePath)
        {
            string ctr = filePath;
            if (filePath.ToLower().EndsWith(".ascx"))
                ctr = Path.GetFileNameWithoutExtension(filePath);

            if (!String.IsNullOrEmpty(ctr))
            {
                return ctr.Replace(".", "_");
            }
            return String.Empty;
        }
    }
}
