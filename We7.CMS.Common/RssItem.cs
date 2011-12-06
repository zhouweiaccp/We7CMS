using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using We7.Framework;

namespace We7.CMS.Common
{
    /// <summary>
    /// RSS项
    /// </summary>
    [Serializable]
    public class RssItem : IXml
    {
        private string title;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string link;

        /// <summary>
        /// 链接地址
        /// </summary>
        public string Link
        {
            get { return link; }
            set { link = value; }
        }
        private string creator;

        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }
        private string author;

        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        private string pubDate;
        /// <summary>
        /// 发布日期
        /// </summary>
        public string PubDate
        {
            get { return pubDate; }
            set { pubDate = value; }
        }
        private string guid;

        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        private string comment;

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        private string comments;

        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        private string slash;

        public string Slash
        {
            get { return slash; }
            set { slash = value; }
        }
        private string commentRss;

        public string CommentRss
        {
            get { return commentRss; }
            set { commentRss = value; }
        }
        private string ping;

        public string Ping
        {
            get { return ping; }
            set { ping = value; }
        }
        private string description;

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        #region IX Members

        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("item");
            CreateElement(doc, xe, "title", title);
            CreateElement(doc, xe, "link", link);
            //CreateElement(doc, xe, "creator", creator);
            CreateElement(doc, xe, "author", author);
            CreateElement(doc, xe, "pubDate", pubDate);
            //CreateElement(doc, xe, "guid", guid);
            //CreateElement(doc, xe, "comment", comment);
            //CreateElement(doc, xe, "comments", comments);
            //CreateElement(doc, xe, "comments", slash);
            //CreateElement(doc, xe, "commentRss", commentRss);
            //CreateElement(doc, xe, "ping", ping);
            CreateElement(doc, xe, "description", description);
            return xe;

        }

        /// <summary>
        /// 生成Channel中的文件
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xe"></param>
        /// <param name="element"></param>
        /// <param name="value"></param>
        private void CreateElement(XmlDocument doc, XmlElement xe, string element, string value)
        {
            XmlElement t = doc.CreateElement(element);
            if (element != "description")
            {
                t.InnerText = value;
            }
            else
            {
                XmlNode xn = doc.CreateCDataSection(value);
                t.AppendChild(xn);
            }
            xe.AppendChild(t);
        }

        private string getValue(XmlElement xe, string filed)
        {
            return xe.SelectSingleNode(filed).InnerText;
        }
        private string getCDDATAValue(XmlElement xe, string filed)
        {
            XmlNode xxe = xe.SelectSingleNode(filed);
            XmlNode x=xxe.ChildNodes[0];
            return x.InnerText;
        }

        public IXml FromXml(XmlElement xe)
        {
            title = getValue(xe, "title");
            link = getValue(xe, "link");
            //creator = getValue(xe, "creator");
            author = getValue(xe, "author");
            pubDate = getValue(xe, "pubDate");
            //guid = getValue(xe, "guid");
            //comment = getValue(xe, "comment");
            //comments = getValue(xe, "comments");
            //slash = getValue(xe, "comments");
            //commentRss = getValue(xe, "commentRss");
            //ping = getValue(xe, "ping");
            description = getCDDATAValue(xe, "description");

            return this;
        }

        #endregion
    }
}
