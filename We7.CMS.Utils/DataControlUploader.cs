using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// 新控件上传类
    /// </summary>
    [Serializable]
    public class DataControlUploader : Uploader
    {
        List<DataControl> controls;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataControlUploader()
            : base()
        {
            controls = new List<DataControl>();
        }

        /// <summary>
        /// 所有控件
        /// </summary>
        public DataControl[] Controls
        {
            get { return controls.ToArray(); }
        }

        /// <summary>
        /// 控件路径
        /// </summary>
        protected override string ControlPath
        {
            get { return Path.Combine(BasePath, "controls"); }
        }

        /// <summary>
        /// 控件布署路径
        /// </summary>
        protected override string DeployControlPath
        {
            get { return Path.Combine(WebRoot, Constants.ControlBasePath); }
        }

        /// <summary>
        /// 控件布署资源
        /// </summary>
        protected override string DeployResourcePath
        {
            get { return WebRoot; }
        }

        /// <summary>
        /// 通配符表示的路径
        /// </summary>
        protected override string FileExtension
        {
            get { return "*" + Constants.ControlFileExtension; }
        }

        /// <summary>
        /// 资源路径
        /// </summary>
        protected override string ResourcePath
        {
            get { return Path.Combine(BasePath, "res"); }
        }

        /// <summary>
        /// 添加控件信息
        /// </summary>
        /// <param name="file">控件配置文件</param>
        protected override void ProcessFile(FileInfo file)
        {
            DataControl dc = new DataControl();
            dc.FromFile(ControlPath, file.Name);
            controls.Add(dc);
        }

        /// <summary>
        /// 添加控件信息
        /// </summary>
        /// <param name="file">控件配置文件</param>
        /// <param name="templateGroupName">模板组</param>
        protected override void ProcessFile(FileInfo file, string templateGroupName)
        {
        }
    }
}
