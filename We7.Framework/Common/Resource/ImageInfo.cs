using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Xml;
using We7.Framework.Util;
using System.Text.RegularExpressions;
using System.Web;

namespace We7.Framework.Common
{
    /// <summary>
    /// ImageInfoCollection的简写
    /// </summary>
    [Serializable]
    public class IIC : ImageInfoCollection
    {
        public IIC(string json)
            : base(json)
        {
        }

        public IIC() { }
    }

    [Serializable]
    public class ImageInfoCollection : Collection<ImageInfo>
    {
        public ImageInfoCollection(string json)
        {
            FromJson(json);
        }

        public ImageInfoCollection() { }

        public void FromJson(string json)
        {
            this.Clear();
            Regex regex = new Regex(@"\[[^\[\]]*\]",RegexOptions.Compiled);
            MatchCollection mc=regex.Matches(json);
            foreach (Match m in mc)
            {
                if (m.Success)
                {
                    ImageInfo info = new ImageInfo();
                    info.FromJson(m.Value);
                    this.Add(info);
                }
            }
        }

        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (ImageInfo info in this)
            {
                sb.Append(info.ToJson()).Append(",");
            }
            Utils.TrimEndStringBuilder(sb, ",");
            sb.Append("]");
            return sb.ToString();
        }
    }

    [Serializable]
    [XmlRoot]
    public class ImageInfo : Collection<ImageItem>
    {
        public ImageItem GetItemBySize(string size)
        {
            foreach (ImageItem item in this)
            {
                if (String.Compare(item.Size, size, true) == 0)
                    return item;
            }
            return null;
        }

        public ImageItem GetItemByType(string type)
        {
            foreach (ImageItem item in this)
            {
                if (String.Compare(item.Type, type, true) == 0)
                    return item;
            }
            return null;
        }

        public string ToXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root=doc.CreateElement("ImageInfo");
            foreach(ImageItem item in this){
                XmlElement xe=doc.CreateElement("Item");
                xe.SetAttribute("Size", item.Size);
                xe.SetAttribute("Type", item.Type);
                xe.SetAttribute("Src", item.Src);
                root.AppendChild(xe);
            }
            doc.AppendChild(root);
            return doc.OuterXml;
        }

        public void FromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            this.Clear();
            foreach (XmlElement xe in doc.SelectNodes("//Item")){
                ImageItem item = new ImageItem();
                item.Size = xe.GetAttribute("Size");
                item.Src = xe.GetAttribute("Src");
                item.Type = xe.GetAttribute("Type");
                this.Add(item);
            }
        }

        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (ImageItem item in this)
            {
                sb.Append("{");
                sb.AppendFormat("src:'{0}',",item.Src);
                sb.AppendFormat("size:'{0}',",item.Size);
                sb.AppendFormat("type:'{0}',",item.Type);
                sb.AppendFormat("desc:'{0}'", item.Desc);
                sb.Append("},");
            }
            Utils.TrimEndStringBuilder(sb, ",");
            sb.Append("]");
            return sb.ToString();
        }

        Regex regex = new Regex(@"\{\s*src\s*:\s*['""](?<src>.*?)['""]\s*,size\s*:\s*['""](?<size>.*?)['""],\s*type\s*:\s*['""](?<type>.*?)['""]\s*\,\s*desc\s*:\s*['""](?<desc>.*?)['""]\s*\}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public void FromJson(string s)
        {
            MatchCollection mc=regex.Matches(s);
            this.Clear();
            foreach (Match m in mc)
            {
                if (m.Success)
                {
                    ImageItem item = new ImageItem();
                    item.Type = m.Groups["type"].Success ? m.Groups["type"].Value : String.Empty;
                    item.Src = m.Groups["src"].Success ? m.Groups["src"].Value : String.Empty;
                    item.Size = m.Groups["size"].Success ? m.Groups["size"].Value : String.Empty;
                    item.Desc = m.Groups["desc"].Success ? m.Groups["desc"].Value : String.Empty;
                    this.Add(item);
                }
            }
        }
    }

    /// <summary>
    /// 图片项目信息
    /// </summary>
    [Serializable]
    public class ImageItem
    {
        [XmlAttribute]
        public string Size { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        public string Src { get; set; }

        [XmlAttribute]
        public string Desc { get; set; }
    }
}
