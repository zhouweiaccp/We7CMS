using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.Reflection;
using System.IO;

namespace We7.Framework.Util
{
    /// <summary>
    /// 手机短信发送助手
    /// </summary>
    public class MobileHelper
    {
        private static object lockConfig= new object();
        private static SMSConfig MyConfig = null;

        private static SMSConfig LoadConfig()
        {
            string smsConfigFile = HttpContext.Current.Server.MapPath("~/config/sms.config");
            if (File.Exists(smsConfigFile))
            {
                SMSConfig myConfig = new SMSConfig();
                XmlDocument xml = new XmlDocument();
                xml.Load(smsConfigFile);
                XmlNode root = xml.SelectSingleNode("items");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "item")
                    {
                        XmlAttribute name = n.Attributes["name"];
                        XmlAttribute value = n.Attributes["value"];

                        if (name != null && name.Name == "Assembly" && value != null)
                            myConfig.Assembly = value.Value;
                        if (name != null && name.Name == "ClassName" && value != null)
                            myConfig.ClassName = value.Value;
                        if (name != null && name.Name == "ConfigString" && value != null)
                            myConfig.ConfigString = value.Value;
                    }
                }
                return myConfig;
            }
            else
                return null;
        }

        private static SMSConfig GetConfig()
        {
            if (MyConfig == null)
            {
                lock (lockConfig)
                {
                    if (MyConfig == null)
                    {
                        MyConfig=LoadConfig();
                        Assembly a = Assembly.LoadFrom(MyConfig.Assembly);
                        //创建类的实例  
                        MyConfig.TheClass = a.CreateInstance(MyConfig.ClassName);
                    }
                }
            }

            return MyConfig;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobiles">手机号码</param>
        /// <param name="content">内容</param>
        /// <returns>发送结果：0-成功；其他-错误信息</returns>
        public static string SendMessage(string mobiles, string content)
        {
            MyConfig = GetConfig();
            if (MyConfig != null)
            {
                ISendSMS iSend = (ISendSMS)MyConfig.TheClass;
                iSend.Config = MyConfig.ConfigString;
                return iSend.Send(mobiles, content);
            }
            else
                return "无法加载短信网关配置信息，请检查sms.config配置文件是否正确。";
        }

        class SMSConfig
        {
            /// <summary>
            /// dll文件名
            /// </summary>
            public string Assembly { get; set; }
            /// <summary>
            /// 类名（完整命名空间）
            /// </summary>
            public string ClassName { get; set; }
            /// <summary>
            /// 配置字符串
            /// </summary>
            public string ConfigString { get; set; }
            /// <summary>
            /// 已创建类实例
            /// </summary>
            public Object TheClass { get; set; }
        }
    }

    /// <summary>
    /// 短信网关接口定义，具体短信网关实现程序继承此接口进行方法实现
    /// </summary>
    public interface ISendSMS
    {
        /// <summary>
        /// 赋值配置参数，解析参数字符串，如sn=SDK-BZK-010-00001;pwd=705706;sign=[短客网]
        /// 每个网关都不一样，请自行解析
        /// </summary>
        string Config { set; }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phones">电话号码，多个用英文逗号,隔开</param>
        /// <param name="content">内容</param>
        /// <returns>发送结果：0-成功；其他-错误信息</returns>
        string Send(string phones, string content);
    }

    /// <summary>
    /// 发送短信基础类（抽象类）
    /// </summary>
    public abstract class BaseSendSMS : ISendSMS
    {
        /// <summary>
        /// 赋值配置参数，解析参数字符串，如sn=SDK-BZK-010-00001;pwd=705706;sign=[短客网]
        /// 每个网关都不一样，请自行解析
        /// </summary>
        public abstract string Config { set; }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phones">电话号码，多个用英文逗号,隔开</param>
        /// <param name="content">内容</param>
        /// <returns>发送结果：0-成功；其他-错误信息</returns>
        public abstract string Send(string phones, string content);
    }
}
