using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using We7.CMS.Common;
using System.Web;

namespace We7.CMS
{
    /// <summary>
    /// 模板副本处理类
    /// </summary>
    public class TemplateCopies
    {
        #region 模版编辑副本管理

        /// <summary>
        /// 获得当前使用副本模板的完整文件路径
        /// </summary>
        /// <param name="file">正本文件路径</param>
        /// <returns>副本文件路径</returns>
        public static string GetThisTemplateCopy(Template t,string file)
        {
            string cfn = HttpContext.Current.Server.MapPath(string.Format("/{0}/{1}/{2}", Constants.SiteSkinsBasePath, "~" + t.SkinFolder, file));
            string cpath = HttpContext.Current.Server.MapPath(string.Format("/{0}/{1}/", Constants.SiteSkinsBasePath, "~" + t.SkinFolder));
            string spath = HttpContext.Current.Server.MapPath(string.Format("/{0}/{1}/", Constants.SiteSkinsBasePath, t.SkinFolder));
            if (!Directory.Exists(cpath))
            {
                Directory.CreateDirectory(cpath);
                CreateAllTemplateCopy(spath, cpath);
            }
            return cfn;
        }



        /// <summary>
        /// 发布Css的副本
        /// </summary>
        public static void PublistCopy(Template t,string fileName,string content)
        {
            TemplateProcessor tp = new TemplateProcessor(t.SkinFolder);
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_skins/" + t.SkinFolder + "/" + fileName);
            string csspath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_skins/" + t.SkinFolder + "/css");
            if (!Directory.Exists(csspath)) Directory.CreateDirectory(csspath);

            tp.OrignInput = LoadOrignTemplate(file,t);

            tp.Input = content;
            tp.IsSubTemplate = t.IsSubTemplate;
            tp.IsMasterPage = t.IsMasterPage;

            tp.FromVisualBoxText();
            tp.FileName = fileName.Trim();
            tp.PublishStyle();
            tp.PublishControl();
        }

        /// <summary>
        /// 取得模版文件的文件内容
        /// </summary>
        /// <param name="fn"></param>
        public static string LoadOrignTemplate(string fn,Template t)
        {
            TemplateProcessor pa = new TemplateProcessor();
            pa.FileName = fn;
            pa.IsSubTemplate = t.IsSubTemplate;
            pa.IsMasterPage = t.IsMasterPage;

            pa.Load();
            return We7Helper.FilterXMLChars(pa.BodyContent);
        }

        /// <summary>
        /// 合并模板正本到副本；
        /// </summary>
        /// <param name="file">模板文件</param>
        public static void MergeToTemplateCopy(string file,Template t)
        {
            if (File.Exists(file))
            {
                string copyFile = GetThisTemplateCopy(t,file);
                if (File.Exists(copyFile))
                {
                    //File.Copy(copyFile, copyFile + ".auto." + DateTime.Now.ToString("yyMMddHH")+".ascx");
                    File.Delete(copyFile);
                }
                File.Copy(file, copyFile);
            }
        }

        /// <summary>
        /// 创建所有模板的副本
        /// </summary>
        /// <param name="spath">正式模板组目录</param>
        /// <param name="cpath">模板组副本目录</param>
        public static void CreateAllTemplateCopy(string spath, string cpath)
        {
            string filter = "*.ascx";
            DirectoryInfo di = new DirectoryInfo(spath);
            FileInfo[] files = di.GetFiles(filter);
            foreach (FileInfo f in files)
            {
                string file = Path.Combine(cpath, f.Name);
                if (!File.Exists(file)) f.CopyTo(file, false);
            }

            CopyCssFile(spath, cpath);
        }

        /// <summary>
        /// 复制Css文件
        /// </summary>
        /// <param name="spath"></param>
        /// <param name="cpath"></param>
        public static void CopyCssFile(string spath, string cpath)
        {
            FileInfo cssfile = new FileInfo(Path.Combine(cpath, "CSS/We7Control.css"));
            if (!cssfile.Exists)
            {
                if (!cssfile.Directory.Exists)
                    cssfile.Directory.Create();
                string sourcefile = Path.Combine(spath, "CSS/We7Control.css");
                if (File.Exists(sourcefile))
                    File.Copy(sourcefile, cssfile.FullName);
            }
        }

        #endregion
    }
}
