using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 友情链接表
    /// </summary>
    [Serializable]
    public class Link
    {
        string id;
        string title;
        string url;
        DateTime created=DateTime.Now;
        string thumbnail;
        string tag;
        int orderNumber;
        DateTime updated=DateTime.Now;

        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }
        public Link()
        {
        }
        /// <summary>
        /// 链接ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set 
            {
                 title = value; 
            }
        }
        /// <summary>
        /// url地址
        /// </summary>
        public string Url
        {
            get 
            {
                if (url!=null && !url.ToLower().StartsWith("http://"))
                    return "http://" + url;
                else
                    return url;  
            }
            set 
            {
                if (value!=null && !value.ToLower().StartsWith("http://"))
                    url = "http://" + value;
                else
                    url = value;  
             }
        }
        /// <summary>
        /// 图片
        /// </summary>
        public string Thumbnail
        {
            get { return thumbnail; }
            set { thumbnail = value; }
        }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        public int OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }

        }
    }
}
