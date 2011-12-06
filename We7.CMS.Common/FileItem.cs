using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace We7.CMS.Common
{
    /// <summary>
    /// 文件信息类
    /// </summary>
    [Serializable]
    public class FileItem
    {
        string fullName;
        string name;
        bool isDirectory;
        string fileType;
        string size;
        string created;
        string url;
        string icon;

        /// <summary>
        /// 文件信息类构造函数
        /// </summary>
        public FileItem()
        {
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 完整名称
        /// </summary>
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        /// <summary>
        /// 是否显示目录
        /// </summary>
        public bool IsDirectory
        {
            get { return isDirectory; }
            set { isDirectory = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// 大小
        /// </summary>
        public string Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType
        {
            get { return fileType; }
            set { fileType = value; }
        }

        /// <summary>
        /// 图片
        /// </summary>
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
    }

    /// <summary>
    /// 目录侦测类
    /// </summary>
    public class DirectoryDiscover
    {
        string basePath;
        string path;
        List<FileItem> items;
        string folderUrl;
        string fileUrl="{0}";
        string filter;
        string folderFilter;
        bool autoCreate;
        bool onlyFolder = false;

        /// <summary>
        /// 是否只是文件夹
        /// </summary>
        public bool OnlyFolder
        {
            get { return onlyFolder; }
            set { onlyFolder = value; }
        }

        /// <summary>
        /// 目录检查
        /// </summary>
        public DirectoryDiscover()
        {
            items = new List<FileItem>();
            Filter = "*";
            FolderFilter = "*";
        }

        /// <summary>
        /// 路径名称
        /// </summary>
        public string PathName
        {
            get { return path; }
            set { path = value; }
        }

        /// <summary>
        /// 文件信息集合对象
        /// </summary>
        public List<FileItem> Items
        {
            get { return items; }
        }

        /// <summary>
        /// 原始路径
        /// </summary>
        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
        }

        /// <summary>
        /// 过滤器
        /// </summary>
        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        /// <summary>
        /// 文件夹过滤器
        /// </summary>
        public string FolderFilter
        {
            get { return folderFilter; }
            set { folderFilter = value; }
        }

        /// <summary>
        /// 文件夹路径
        /// </summary>
        public string FolderUrl
        {
            get { return folderUrl; }
            set { folderUrl = value; }
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileUrl
        {
            get { return fileUrl; }
            set { fileUrl = value; }
        }

        /// <summary>
        /// 根据类型获取图片
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetIcon(string type)
        {
            switch (type.ToLower())
            {
                case ".bmp":
                case ".jpg":
                case ".gif":
                case ".jpeg":
                case ".tiff":
                    return "gif.gif";

                case ".doc":
                    return "doc.gif";

                case ".zip":
                case ".rar":
                    return "zip.gif";

                case ".cs":
                    return "code.gif";

                case ".pdf":
                    return "pdf.gif";

                default:
                    return "file.gif";
            }
        }

        /// <summary>
        /// 是否自动创建
        /// </summary>
        public bool AutoCreate
        {
            get { return autoCreate; }
            set { autoCreate = value; }
        }

        /// <summary>
        /// 开始检索……
        /// </summary>
        public void Process()
        {
            Items.Clear();

            string dir = Path.Combine(BasePath, PathName);

            if (!Directory.Exists(dir))
            {
                if (AutoCreate)
                {
                    DirectoryInfo newDir = new DirectoryInfo(dir);
                    newDir.Create();
                }
                else
                {
                    return;
                }
            }

            if (!(PathName == "" && FolderFilter == "_*" && OnlyFolder))//显示根目录下数据文件夹
            {
                string fn = Path.Combine(dir, "forbid");
                if (File.Exists(fn))
                {
                    return;
                }
            }

            DirectoryInfo di = new DirectoryInfo(dir);
            DirectoryInfo[] ds = di.GetDirectories(FolderFilter);
            foreach (DirectoryInfo d in ds)
            {
                FileItem it = new FileItem();
                it.Created = d.CreationTime.ToString();
                it.FileType = "文件夹";
                it.FullName = Path.Combine(PathName, d.Name);
                it.FullName = it.FullName.Replace("\\", "/");
                it.IsDirectory = true;
                it.Name = d.Name;
                it.Size = "";
                it.Url = String.Format(FolderUrl, it.FullName,it.Name);
                it.Icon = "folder.gif";
                Items.Add(it);
            }
            if (!OnlyFolder)
            {
                FileInfo[] files = di.GetFiles(Filter);
                foreach (FileInfo f in files)
                {
                    FileItem it = new FileItem();
                    it.Created = f.CreationTime.ToString();
                    it.FileType = Path.GetExtension(f.Name);
                    if (it.FileType.StartsWith("."))
                    {
                        it.FileType.Remove(0, 1);
                    }
                    it.FullName = Path.Combine(PathName, f.Name);
                    it.FullName = it.FullName.Replace("\\", "/");
                    if (!it.FullName.StartsWith("/")) it.FullName = "/" + it.FullName;

                    it.IsDirectory = false;
                    it.Name = f.Name;
                    it.Size = String.Format("{0:N0}", f.Length);
                    it.Url = String.Format(FileUrl, it.FullName, it.Name);
                    it.Icon = GetIcon(it.FileType);
                    Items.Add(it);
                }
            }
        }
    }
}
