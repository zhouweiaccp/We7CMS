using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// 模板上传处理类
    /// </summary>
    [Serializable]
    public class TemplateUploader : Uploader
    {
        List<Template> templates;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TemplateUploader()
        {
            templates = new List<Template>();
        }

        /// <summary>
        /// 模板组
        /// </summary>
        public Template[] Templates
        {
            get { return templates.ToArray(); }
        }

        /// <summary>
        /// 控件目录
        /// </summary>
        protected override string ControlPath
        {
            get { return Path.Combine(BasePath, "controls"); }
        }

        /// <summary>
        /// 控件布署目录
        /// </summary>
        protected override string DeployControlPath
        {
            get { return Path.Combine(WebRoot, Constants.TemplateBasePath); }
        }

        /// <summary>
        /// 控件资料目录
        /// </summary>
        protected override string DeployResourcePath
        {
            get { return WebRoot; }
        }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        protected override string FileExtension
        {
            get { return "*" + Constants.TemplateFileExtension; }
        }

        /// <summary>
        /// 资源路径
        /// </summary>
        protected override string ResourcePath
        {
            get { return Path.Combine(BasePath, "res"); }
        }

        /// <summary>
        ///  处理文件
        /// </summary>
        /// <param name="file">文件名称</param>
        protected override void ProcessFile(FileInfo file)
        {
            Template tp = new Template();
            //tp.FromFile(ControlPath, file.Name);
            tp.FromFile(file.Directory.FullName,file.Name);
            templates.Add(tp);
        }

        /// <summary>
        /// 处理文件　
        /// </summary>
        /// <param name="file">文件名称</param>
        /// <param name="templatePath">模板路径</param>
        protected override void ProcessFile(FileInfo file, string templatePath)
        {
            Template tp = new Template();

            tp.FromFile(templatePath, file.Name);
            templates.Add(tp);
        }
    }
}
