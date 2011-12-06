using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using We7.Framework;

namespace We7.CMS.Common
{
    /// <summary>
    /// 模板组实体类（已过期，兼容2.1以前版本）
    /// </summary>
    [Obsolete]
    [Serializable]
    public class TemplateGroup : IXml
    {
        string basePath;
        string fileName;
        string name;
        string description;
        DateTime created=DateTime.Now;
        List<Item> items;

        public TemplateGroup()
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

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
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
                throw new Exception("没有找到模板组的配置文件"+fn); 
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
        /// 模板组具体项
        /// </summary>
        [Obsolete]
        [Serializable]
        public class Item : IXml
        {
            string alias;
            string template;
            string templateText;
            string detailTemplate;
            string detailTemplateText;
            bool isDetailTemplate;
            
            public string Alias
            {
                get { return alias; }
                set { alias = value; }
            }

            public string Template
            {
                get { return template; }
                set { template = value; }
            }

            public string TemplateText
            {
                get { return templateText; }
                set { templateText = value; }
            }

            public string DetailTemplate
            {
                get { return detailTemplate; }
                set { detailTemplate = value; }
            }

            public string DetailTemplateText
            {
                get { return detailTemplateText; }
                set { detailTemplateText = value; }
            }

            public bool IsDetailTemplate
            {
                get { return isDetailTemplate; }
                set { isDetailTemplate = value; }
            }

            public XmlElement ToXml(XmlDocument doc)
            {
                XmlElement xe = doc.CreateElement("Item");
                xe.SetAttribute("alias", Alias);
                xe.SetAttribute("template", Template);
                //xe.SetAttribute("detailTemplate", DetailTemplate);
                xe.SetAttribute("isDetailTemplate", IsDetailTemplate ? Boolean.TrueString : Boolean.FalseString);
                return xe;
            }

            public IXml FromXml(XmlElement xe)
            {
                alias = xe.GetAttribute("alias");
                template = xe.GetAttribute("template");
                //detailTemplate = xe.GetAttribute("detailTemplate");
                isDetailTemplate = xe.GetAttribute("isDetailTemplate") == Boolean.TrueString;
                return this;
            }
        }

        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("TempateGroup");
            xe.SetAttribute("name", Name);
            xe.SetAttribute("description", Description);
            xe.SetAttribute("created", Created.ToString());
            foreach(Item it in Items)
            {
                xe.AppendChild(it.ToXml(doc));
            }
            return xe;
        }

        public IXml FromXml(XmlElement xe)
        {
            Name = xe.GetAttribute("name");
            Description = xe.GetAttribute("description");
            Created = Convert.ToDateTime(xe.GetAttribute("created"));
            foreach (XmlElement node in xe.SelectNodes("Item"))
            {
                Item it = new Item();
                it.FromXml(node);
                Items.Add(it);
            }
            return this;
        }

        #region IX 成员

        XmlElement IXml.ToXml(XmlDocument doc)
        {
            throw new NotImplementedException();
        }

        IXml IXml.FromXml(XmlElement xe)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
