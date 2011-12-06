using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace We7.CMS.Config
{
    /// <summary>
    /// 策略的配置信息
    /// </summary>
    [Serializable]
    public class StrategyInfo
    {
        private string key;
        /// <summary>
        /// 策略的关键字
        /// </summary>
        [XmlAttribute]
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }
        /// <summary>
        /// 策略的名称
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }


        private List<string> allowIPRang = new List<string>();
        /// <summary>
        /// 信任IP范围
        /// </summary>
        [XmlArrayItem("IP")]
        public List<String> AllowIPRang
        {
            get
            {
                return allowIPRang;
            }
            set
            {
                allowIPRang = value;
            }
        }


        private List<string> denyIPRang = new List<string>();
        /// <summary>
        /// 禁止IP范围
        /// </summary>
        [XmlArrayItem("IP")]
        public List<String> DenyIPRang 
        {
            get
            {
                return denyIPRang;
            }
            set
            {
                denyIPRang = value;
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("Desc")]
        public string Desc { get; set; }

        /// <summary>
        /// 策略是否可用
        /// </summary>
        [XmlAttribute]
        public bool Enable { get; set; }

    }
}
