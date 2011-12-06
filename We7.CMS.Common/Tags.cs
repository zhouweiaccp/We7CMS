using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 标签类
    /// </summary>
    public class Tags
    {
        string id;
        string identifier;
        int frequency;
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;

        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 标识符
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        /// <summary>
        /// 频率
        /// </summary>
        public int Frequency
        {
            get { return frequency; }
            set { frequency = value; }
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
        public Tags()
        {
        }

        /// <summary>
        /// 保存XML信息
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("ChannelTags");
            xe.SetAttribute("id", ID);
            xe.SetAttribute("identifier", Identifier);
            return xe;
        }

        /// <summary>
        /// 获取XML信息
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public Tags FromXml(XmlElement xe)
        {
            id = xe.GetAttribute("id");
            identifier = xe.GetAttribute("identifier");
            return this;
        }
    }
}
