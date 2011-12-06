using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
namespace We7.CMS.Common
{
    /// <summary>
    /// 关键字组（用于关键字过滤）
    /// </summary>
      [Serializable]
    public class KeyWordGroup : IXml
    {
        string name;
        string basePath;
        string fileName;
        List<Item> items;

        public KeyWordGroup()
        {
            items = new List<Item>();
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public List<Item> Items
        {
            get { return items; }
        }

        public void FromFile(string path, string fn)
        {
            BasePath = path;
            FileName = fn;
            XmlDocument doc = new XmlDocument();
            string file = Path.Combine(BasePath, FileName);
            if (File.Exists(file))
            {
                doc.Load(file);
                this.FromXml(doc.DocumentElement);
            }
            else
            {
                throw new Exception("没有找到该文件" + fn);
            }
        }

        public void ToFile(string path, string fn)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "gb2312", "");
            doc.AppendChild(dec);
            doc.AppendChild(this.ToXml(doc));
            doc.Save(Path.Combine(path, fn));
        }

          /// <summary>
          /// 关键字项
          /// </summary>
        [Serializable]
        public class Item : IXml
        {
            string keywordType;

            string words;

            public string Words
            {
                get { return words; }
                set { words = value; }
            }

            public string KeywordType
            {
                get { return keywordType; }
                set { keywordType = value; }
            }           

            public IXml FromXml(XmlElement xe)
            {
                words = xe.GetAttribute("words");
                keywordType = xe.GetAttribute("type");
                return this;
            }
            public XmlElement ToXml(XmlDocument doc)
            {
                XmlElement xe = doc.CreateElement("Item");
                return xe;
            }
        }
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("keywords");
            return xe;
        }
        public IXml FromXml(XmlElement xe)
        {
            foreach (XmlElement node in xe.SelectNodes("Item"))
            {
                Item it = new Item();
                it.FromXml(node);
                Items.Add(it);
            }
            return this;
        }
    }
}

