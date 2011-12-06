using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;

namespace We7.CMS.Common
{
    /// <summary>
    /// 附件类
    /// </summary>
    [Serializable]
    public class Attachment
    {
        string id;
        string articleID;
        int sequenceIndex;
        string fileType;
        string fileName;
        long fileSize;
        string filePath;
        string description;
        DateTime uploadDate;
        int downloadTimes;
        string enumState;

        string imgPath;
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;

        public Attachment()
        {
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleID
        {
            get { return articleID; }
            set { articleID = value; }
        }

        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return sequenceIndex; }
            set { sequenceIndex = value; }
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
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        /// <summary>
        /// 文件大小格式化显示，如2.5M
        /// </summary>
        public string FileSizeText
        {
            get
            {
                return We7Helper.FormatFileSize(FileSize);
            }
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime UploadDate
        {
            get { return uploadDate; }
            set { uploadDate = value; }
        }

        /// <summary>
        /// 下载次数
        /// </summary>
        public int DownloadTimes
        {
            get { return downloadTimes; }
            set { downloadTimes = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgPath
        {
            get
            {
                return "/images/icon_attach.gif";
            }
            set { imgPath = value; }
        }

        /// <summary>
        /// 附件下载地址
        /// </summary>
        public string DownloadUrl
        {
            get
            {
                return string.Format("/go/AttachmentDownload.aspx?id={0}", ID);
            }
        }
    }
}
