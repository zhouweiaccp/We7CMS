using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;
using We7.Framework.Zip;

namespace We7.CMS
{
    /// <summary>
    /// 资源上传处理类
    /// </summary>
    [Serializable]
    public abstract class Uploader
    {
        string temporaryPath;
        string fileName;
        string uniqueID;
        string webRoot;
        string deployGroupPath;

        /// <summary>
        /// 文件上传
        /// </summary>
        public Uploader()
        {
            uniqueID = We7Helper.CreateNewID();
        }

        /// <summary>
        /// UID
        /// </summary>
        public string UniqueID
        {
            get { return uniqueID; }
        }

        /// <summary>
        /// 临时文件路径
        /// </summary>
        public string TemporaryPath
        {
            get { return temporaryPath; }
            set { temporaryPath = value; }
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 网站根路径
        /// </summary>
        public string WebRoot
        {
            get { return webRoot; }
            set { webRoot = value; }
        }

        /// <summary>
        /// 基本路径
        /// </summary>
        public string BasePath
        {
            get { return Path.Combine(TemporaryPath, uniqueID); }
        }

        /// <summary>
        /// 是否启用网站皮肤
        /// </summary>
        public static bool EnableSiteSkins
        {
            get
            {
                string _default = SiteSettingHelper.Instance.Config.EnableSiteSkins;
                if (_default.ToLower() == "true")
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 控件路径
        /// </summary>
        protected abstract string ControlPath
        {
            get;
        }

        /// <summary>
        /// 资源路径
        /// </summary>
        protected abstract string ResourcePath
        {
            get;
        }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        protected abstract string FileExtension
        {
            get;
        }

        /// <summary>
        /// 控件布署路径
        /// </summary>
        protected abstract string DeployControlPath
        {
            get;
        }

        public string SkinPath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_skins"); }
        }

        /// <summary>
        /// 资源路径
        /// </summary>
        protected abstract string DeployResourcePath
        {
            get;
        }

        /// <summary>
        /// 布署的模板组
        /// </summary>
        public string DeployGroupPath
        {
            get { return deployGroupPath; }
            set { deployGroupPath = value; }
        }

        /// <summary>
        /// 布署资源路径
        /// </summary>
        public string DeployResPath
        {
            get { return Path.Combine(WebRoot, Constants.TempBasePath); }
        }

        /// <summary>
        /// 处理文件
        /// </summary>
        /// <param name="file">文件名</param>
        protected abstract void ProcessFile(FileInfo file);

        /// <summary>
        /// 处理文件
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="templateGroupName">模板组</param>
        protected abstract void ProcessFile(FileInfo file, string templateGroupName);

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="zipName">压缩文件名</param>
        public void Process(string zipName)
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            using (FileStream fs = File.Open(FileName, FileMode.Open))
            {
                ZipUtils.ExtractZip(fs, BasePath);
            }

            string templatePath = ControlPath;

            HelperFactory helperFactory = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
            TemplateVersionHelper templateVersionHelper = helperFactory.GetHelper<TemplateVersionHelper>();

            DirectoryInfo di = new DirectoryInfo(BasePath);
            FileInfo[] fis=di.GetFiles("*.xml");
            foreach (FileInfo f in fis)
            {
                try
                {
                    ProcessFile(f);
                }
                catch { }
            }
            //TemplateVersion tv = templateVersionHelper.GetTemplateVersion(String.Format("{0}\\res", BasePath));
            //if (tv != null)
            //{
            //    if (tv.TemplatePath == "cgi-bin\\templates\\groups")
            //        tv.TemplatePath = "cgi-bin\\templates";
            //    DeployGroupPath = Path.Combine(WebRoot, tv.TemplatePath);

            //    if (tv.UseSkin)
            //    {
            //        templatePath = String.Format("{0}\\{1}", ControlPath, zipName.Replace("Package.Templates.", ""));
            //    }
            //    if (Directory.Exists(templatePath))
            //    {
            //        DirectoryInfo di = new DirectoryInfo(templatePath);
            //        foreach (FileInfo fi in di.GetFiles(FileExtension))
            //        {
            //            if (tv.UseSkin)
            //            {
            //                ProcessFile(fi, templatePath);
            //            }
            //            else
            //            {
            //                ProcessFile(fi);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    DeployGroupPath = DeployControlPath;
            //    if (DeployControlPath == "_skins")
            //        DeployGroupPath = "_templates";
            //    if (Directory.Exists(ControlPath))
            //    {
            //        DirectoryInfo di = new DirectoryInfo(ControlPath);
            //        foreach (FileInfo fi in di.GetFiles(FileExtension))
            //        {
            //            ProcessFile(fi);
            //        }
            //    }
            //}
        }

       　/// <summary>
       　/// 处理文件
       　/// </summary>
        public void Process()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            using (FileStream fs = File.Open(FileName, FileMode.Open))
            {
                ZipUtils.ExtractZip(fs, BasePath);
            }

            if (Directory.Exists(ControlPath))
            {
                DirectoryInfo di = new DirectoryInfo(ControlPath);
                foreach (FileInfo fi in di.GetFiles(FileExtension))
                {
                    ProcessFile(fi);
                }
            }
        }

        /// <summary>
        /// 布署
        /// </summary>
        public void Deploy()
        {
            if (Directory.Exists(BasePath))
            {
                if (!Directory.Exists(SkinPath))
                    Directory.CreateDirectory(SkinPath);
                We7Helper.CopyDirectory(BasePath, SkinPath);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Clean()
        {
            if (Directory.Exists(BasePath))
            {
                Directory.Delete(BasePath, true);
            }
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
        }
    }
}
