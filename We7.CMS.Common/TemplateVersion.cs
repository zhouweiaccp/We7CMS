using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// Ä£°æ°æ±¾
    /// </summary>
    [Serializable]
    public class TemplateVersion
    {
        string version;
        string templatePath;
        bool useSkin;
        string fileName;
        string basePath;

        public TemplateVersion()
        {

        }

        public bool UseSkin
        {
            get { return useSkin; }
            set { useSkin = value; }
        }

        public string TemplatePath
        {
            get { return templatePath; }
            set { templatePath = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
        }

        public void FromFile(string bp, string fn)
        {
            BasePath = bp;
            FileName = Path.GetFileNameWithoutExtension(fn);
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(basePath, fn));
            FromXml(doc.DocumentElement);
        }

        public void ToFile(string fn)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xd = doc.CreateXmlDeclaration("1.0", "gb2312", "");
            doc.AppendChild(xd);
            doc.AppendChild(ToXml(doc));
            doc.Save(fn);
        }

        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("TemplateVersion");
            xe.SetAttribute("Version", Version);
            xe.SetAttribute("TemplatePath", TemplatePath);
            xe.SetAttribute("UseSkin", UseSkin ? Boolean.TrueString : Boolean.FalseString);
            return xe;
        }

        public TemplateVersion FromXml(XmlElement xe)
        {
            version = xe.GetAttribute("Version");
            templatePath = xe.GetAttribute("TemplatePath");
            useSkin = xe.GetAttribute("UseSkin") == Boolean.TrueString;
            return this;
        }
    }

}
