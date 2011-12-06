using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
namespace We7.Model.Core
{
    [Serializable]
    public class DefaultModel
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("label")]
        public string Label { get; set; }

        [XmlAttribute("system")]
        public bool System { get; set; }

        [XmlAttribute("mapping")]
        public string MappingFields { get; set; }
    }
}
