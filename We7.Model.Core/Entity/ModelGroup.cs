using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace We7.Model.Core
{

    [Serializable]
    [XmlRoot("root")]
    public class ModelGroupCollection : Collection<ModelGroup>
    {
        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ModelGroup this[string name]
        {
            get
            {
                foreach (ModelGroup p in this)
                {
                    if (p.Name == name)
                        return p;
                }
                return null;
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Name == name)
                    {
                        this[i] = value;
                    }
                }
            }
        }

        public void AddOrUpdate(ModelGroup modelGroup)
        {
            if (this[modelGroup.Name] != null)
            {
                this[modelGroup.Name] = modelGroup;
            }
            else
            {
                this.Add(modelGroup);
            }
        }
    }

    /// <summary>
    /// 内容模型组
    /// </summary>
    [Serializable]
    public class ModelGroup
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("label")]
        public string Label {get;set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("system")]
        public bool System { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

    }
}
