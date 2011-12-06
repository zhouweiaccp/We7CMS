using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using We7.Framework.Util;
using System.Xml;

namespace We7.CMS.Module.VisualTemplate.Models
{
    public class SettingFile
    {
        public SettingFile()
        {
            Items = new List<Dictionary<string, object>>();
        }
        public List<Dictionary<string, object>> Items
        {
            get;
            set;
        }

        public string ToJson()
        {
            return Newtonsoft.Json.JavaScriptConvert.SerializeObject(this);
        }
    }


    public class SettingFileService
    {
        public static void SaveFile(string filename, SettingFile settings)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

               var declara= doc.CreateXmlDeclaration("1.0", "utf-8", null);

               doc.AppendChild(declara);

               XmlElement root = doc.CreateElement("root");
               

               foreach (var item in settings.Items)
               {
                   XmlElement el = doc.CreateElement("item");

                   foreach (var attr in item)
                   {
                       el.SetAttribute(attr.Key.ToString(), attr.Value.ToString());
                   }
                   root.AppendChild(el);
               }
                
               doc.AppendChild(root);
               doc.Save(filename);
            }
            catch ( Exception ex)
            {

                throw ex ;
            }
        }

        public static SettingFile GetSettings(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                var settingFile = new SettingFile();

                var root = doc.SelectSingleNode("//root");

                foreach (XmlNode node in root.ChildNodes)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        dic.Add(attr.Name, attr.Value);
                    }
                    settingFile.Items.Add(dic);
                }

                return settingFile;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
