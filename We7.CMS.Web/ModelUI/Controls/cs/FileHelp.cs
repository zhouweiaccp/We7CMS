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
namespace We7.Model.UI.Controls.cs
{
    public class FileHelp
    {
        /// <summary>
        /// 当前目录存在否,如果没有就创建
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
      public static  bool IsDirExists(string dirName)
        {
            string dir = System.Web.HttpContext.Current.Server.MapPath(dirName);
            bool IsEx = true;
            try
            {
                if (System.IO.Directory.Exists(dir))
                {
                    //  throw   new   Exception( "目录已存在 "); 
                    IsEx = false;
                }
                else
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
            }
            catch
            {
                // throw new Exception(ee.Message);
                IsEx = false;
            }
            return IsEx;
        }
        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
      public static bool DeleteFile(string dirName)
        {

            string dir = System.Web.HttpContext.Current.Server.MapPath(dirName);
            if (File.Exists(dir))
            {
                File.Delete(dir);
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 取文件名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
      public static string GetFileName(string str)
        {
            return Path.GetFileName(System.Web.HttpContext.Current.Server.MapPath(str));
        }
    }
}
