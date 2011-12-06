using System;
using System.Collections.Generic;
using System.Text;


using Thinkment.Data;
using We7.CMS.Common;
using System.IO;
using System.Web;
using We7.Framework;

namespace We7.CMS
{
    /// <summary>
    /// 附件业务类
    /// </summary>
    [Serializable]
    [Helper("We7.AttachmentHelper")]
    public class AttachmentHelper : BaseHelper
    {
        /// <summary>
        /// 添加一个附件
        /// </summary>
        /// <param name="a">附件记录</param>
        public void AddAttachment(Attachment a)
        {
            a.ID = We7Helper.CreateNewID();
            a.UploadDate = DateTime.Now;
            a.Created = a.Updated = DateTime.Now;
            Assistant.Insert(a);
        }

        /// <summary>
        /// 更新附件记录
        /// </summary>
        /// <param name="a"></param>
        public void UpdateAttachment(Attachment a)
        {
            a.Updated = DateTime.Now;
            Assistant.Update(a);
        }

        /// <summary>
        /// 获取一篇文章的附件集合
        /// </summary>
        /// <param name="articleID">文章ID</param>
        /// <returns></returns>
        public List<Attachment> GetAttachments(string articleID)
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Add(CriteriaType.Equals, "ArticleID", articleID);

            Order[] ods = new Order[] { new Order("UploadDate", OrderMode.Desc), new Order("ArticleID") };
            List<Attachment> atms = Assistant.List<Attachment>(c, ods);
            return atms;
        }

        /// <summary>
        /// 通过文章ID获取附件图片组
        /// </summary>
        /// <param name="articleID">文章ID</param>
        /// <returns></returns>
        public List<Attachment> GetPhotoAttachments(string articleID)
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Add(CriteriaType.Equals, "ArticleID", articleID);
            c.Add(CriteriaType.Equals, "EnumState", "0");
            Order[] ods = new Order[] { new Order("UploadDate", OrderMode.Desc), new Order("ArticleID") };
            List<Attachment> atms = Assistant.List<Attachment>(c, ods);
            return atms;
        }

        /// <summary>
        /// 删除一个附件
        /// </summary>
        /// <param name="attachmentID">附件ID</param>
        public void DeleteAttachment(string attachmentID)
        {
            Attachment a = GetAttachment(attachmentID);
            string file = HttpContext.Current.Server.MapPath(a.FilePath + "/" + a.FileName);
            if (File.Exists(file))
                File.Delete(file);
            Assistant.Delete(a);
        }

        /// <summary>
        /// 获取一个文章的第一附件
        /// </summary>
        /// <param name="oid">文章ID</param>
        /// <param name="fileType">附件类型</param>
        /// <param name="fileName">附件名称</param>
        /// <returns></returns>
        public Attachment GetFirstAttachment(string oid, string fileType, string fileName)
        {
            Order[] ods = new Order[] { new Order("UploadDate", OrderMode.Desc) };
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", oid);
            if (fileType != "" && fileType != ".*" && fileType != "*.*")
                c.Add(CriteriaType.Equals, "FileType", fileType);
            c.Add(CriteriaType.Equals, "FileName", fileName);
            List<Attachment> atms = Assistant.List<Attachment>(c, ods, 0, 1);

            if (atms.Count > 0)
            {
                return atms[0];
            }
            return null;
        }

        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Attachment GetAttachment(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Attachment> atms = Assistant.List<Attachment>(c,null);

            if (atms.Count > 0)
            {
                return atms[0];
            }
            else
                return null;
        }
    }
}
