using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;

namespace We7.CMS.Common
{
    /// <summary>
    /// 插件统计集合
    /// </summary>
    [Serializable]
    public class PluginStatisticCollection:List<PluginStatistic>
    {
        public PluginStatistic this[string pluginName]
        {
            get
            {
                return this.Find(delegate(PluginStatistic info)
                {
                    return info.PluginName == pluginName;
                });
            }
        }

        /// <summary>
        /// 查看是否包含相应插件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return this[key] != null;
        }

        /// <summary>
        /// 保存数据到XML
        /// </summary>
        /// <param name="path"></param>
        public void SaveXML(string path)
        {
            XmlDocument doc = ToXml();
            doc.Save(path);
        }

        /// <summary>
        /// 从XML文件中加载数据
        /// </summary>
        /// <param name="path"></param>
        public void LoadXML(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            LoadXML(doc);
        }

        /// <summary>
        /// 从XML中加载数据
        /// </summary>
        /// <param name="doc"></param>
        public void LoadXML(XmlDocument doc)
        {
            this.Clear();

            if (doc == null)
                throw new Exception("统计文档为空");
            
            DateTime dt;
            int tempInt;

            XmlNodeList list=doc.DocumentElement.SelectNodes("Item");
            foreach (XmlNode node in list)
            {                
                string pluginName = node.Attributes["PluginName"] != null ? node.Attributes["PluginName"].Value.Trim() : String.Empty;

                if (String.IsNullOrEmpty(pluginName))
                    continue;

                PluginStatistic info = new PluginStatistic();
                info.PluginName = pluginName;

                if (node.Attributes["CreateTime"] != null && DateTime.TryParse(node.Attributes["CreateTime"].Value.Trim(), out dt))
                    info.CreateTime = dt;

                if (node.Attributes["UpdateTime"] != null && DateTime.TryParse(node.Attributes["UpdateTime"].Value.Trim(), out dt))
                    info.UpdateTime = dt;

                if (node.Attributes["Clicks"] != null && int.TryParse(node.Attributes["Clicks"].Value.Trim(), out tempInt))
                    info.Clicks = tempInt;

                Add(info);
            }
        }

        /// <summary>
        /// 从对象中生成XMLDocument文档
        /// </summary>
        /// <returns></returns>
        public XmlDocument ToXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root=doc.CreateElement("Statistic");
            doc.AppendChild(root);

            foreach (PluginStatistic info in this)
            {
                XmlElement el = doc.CreateElement("Item");
                el.SetAttribute("PluginName", info.PluginName);
                el.SetAttribute("CreateTime", info.CreateTime.ToString());
                el.SetAttribute("UpdateTime", info.UpdateTime.ToString());
                el.SetAttribute("Clicks", info.Clicks.ToString());

                root.AppendChild(el);
            }
            return doc;
        }
    }

    /// <summary>
    /// 插件统计数据
    /// </summary>
    public class PluginStatistic
    {
        public string PluginName { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public int Clicks { get; set; }
    }
}
