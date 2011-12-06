using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Text;
using System.Collections;
namespace We7.Model.UI.Controls.system.page
{
    public class ShowImgList
    {
        public static string BindImgList(string DialogPath)
        {
            StringBuilder strHtml = new StringBuilder();
            DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(DialogPath));
            try
            {
                foreach (FileInfo f in di.GetFiles())
                {
                    int s = f.Name.LastIndexOf("_Min");
                    if (s > 0)
                    {
                        string pathrelatively = DialogPath + "/" + f.Name;
                        strHtml.Append(" <div style=\"float: left; text-align: center\">");
                        strHtml.Append(" <div style=\"width: 30px; height: 30px; margin: 6px; padding: 2px; background: #fff;");
                        strHtml.Append(" border: 1px solid #808080\">");
                        strHtml.Append("<img src=\"" + pathrelatively + "\" style=\"width: 30px; height: 30px\" /></div>");
                        strHtml.Append("  <div>");
                        strHtml.Append(" <img src=\"/modelui/skin/images/close.gif\" onclick=\"DelFile('" + pathrelatively + "')\" />");
                        strHtml.Append(" </div>");
                        strHtml.Append(" </div>");
                    }
                }
            }
            catch
            {
                //对应目录下没有文件
            }
            return strHtml.ToString();

        }
    }
}
