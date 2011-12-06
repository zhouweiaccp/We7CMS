using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace We7
{
    /// <summary>
    /// We7 XML序列化与反序列化接口
    /// </summary>
    public interface IXml
    {
        /// <summary>
        /// 输出到XML文件
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        XmlElement ToXml(XmlDocument doc);
        /// <summary>
        /// 从XML文件加载数据
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        IXml FromXml(XmlElement xe);
    }
}
