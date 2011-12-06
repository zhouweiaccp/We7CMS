using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;
using System.IO;
using We7.Framework.Util;

namespace We7.Framework.Config
{
    public class AdviceEmailConfigs
    {
        /// <summary>
        /// 根据邮件类型得到邮件配置信息
        /// </summary>
        /// <param name="emailType"></param>
        /// <returns></returns>
        public  AdviceEmailConfigInfo this[string emailType]
        {
            get
            {
                AdviceEmailConfigInfo result = new AdviceEmailConfigInfo();
                result.EmailType = emailType;
                
                string xmlFilePath = HttpRuntime.AppDomainAppPath + "\\Config\\AdviceEmailConfig.xml";     
 
                string xpathTitle = "configuration/item[@value='" + emailType + "']/EmailTitle";
                XmlNode xmlNodeTitle = XmlHelper.GetXmlNode(xmlFilePath,xpathTitle);

                string xpathContent = "configuration/item[@value='" + emailType + "']/EmailContent";
                XmlNode xmlNodeContent = XmlHelper.GetXmlNode(xmlFilePath, xpathContent);

                if(xmlNodeTitle != null)
                {
                    result.EmailTitle = xmlNodeTitle.InnerXml;
                }
                if(xmlNodeContent != null)
                {
                    result.EmailContent = xmlNodeContent.InnerXml;
                }
                return result;
            }
        }
    }
}
