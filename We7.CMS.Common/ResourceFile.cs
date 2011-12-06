using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using We7.Framework;

namespace We7.CMS.Common
{
    /// <summary>
    /// 资源文件，用于控件或模板
    /// </summary>
    [Serializable]
    public class ResourceFile : IXml, IJsonResult
    {
        string type;
        string fileName;

        public ResourceFile()
        {
        }

        public ResourceFile(string tp, string fn)
        {
            type = tp;
            fileName = fn;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 保存XML信息
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("File");
            xe.SetAttribute("fileName", FileName);
            xe.SetAttribute("type", Type);
            return xe;
        }

        /// <summary>
        /// 获取XML信息
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public IXml FromXml(XmlElement xe)
        {
            FileName = xe.GetAttribute("fileName");
            Type = xe.GetAttribute("type");
            return this;
        }

        /// <summary>
        /// 输出为Json格式文本
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder("{");
            sb.AppendFormat("{0}:'{1}',", "fileName", fileName);
            sb.AppendFormat("{0}:'{1}'", "type", type);
            sb.Append("}");
            return sb.ToString();
        }
    }
}
