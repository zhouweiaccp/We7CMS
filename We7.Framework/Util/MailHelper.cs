#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using System.Web.UI;
using OpenPOP.POP3;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Net.Mail;
using System.Text;
using System.Net;
using We7.Framework.Config;


namespace We7.Framework.Util
{
	/// <summary>
	/// 邮件发送助手类
	/// </summary>
	public class MailHelper
    {
        #region 属性

        public MailHelper() { }

        private string _adminEmail;//"master@duanke.com";
        public string AdminEmail
        {
            get { return _adminEmail; }
            set { _adminEmail = value; }
        }

        private string _smtpServer;//"mail.duanke.com";
        public string SmtpServer
        {
            get { return _smtpServer; }
            set { _smtpServer = value; }
        }

        private string _popServer;//"mail.duanke.com";
        /// <summary>
        /// 收邮件服务
        /// </summary>
        public string PopServer
        {
            get { return _popServer; }
            set { _popServer = value; }
        }

        private string _password;// "master@duanke.com888";
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _userName;// "master@duanke.com";
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        #endregion

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Send(string to, string from, string subject, string message, string priority)
        {
            //string SmtpServer = "mail.duanke.cn";
            //string AdminEmail = "webmaster@duanke.cn";
            //string Password = "webmaster8888";
            //string UserName = "webmaster@duanke.cn";

            ////第一类方法：MailMessage
            //System.Web.Mail .MailMessage mailMessage = new System.Web.Mail.MailMessage();
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(from);//发送人地址
            mailMessage.To.Add(to);//接受人地址
            mailMessage.Subject = subject.Trim().Replace("\r\n"," ").Replace("<br/>"," ");

            mailMessage.SubjectEncoding = Encoding.UTF8;            
            mailMessage.Body = message.Replace("\r\n", "<br/>");
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;             
            switch (priority)       //邮件优先级
            {
                case "High":
                    mailMessage.Priority = System.Net.Mail.MailPriority.High;
                    break;
                case "Low":
                    mailMessage.Priority = System.Net.Mail.MailPriority.Low;
                    break;
                case "Normal":
                    mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
                    break;
                default:
                    mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
                    break;
            }            
            SmtpClient smtp = new SmtpClient(); // 提供身份验证的用户名和密码 // 网易邮件用户可能为：username password // Gmail 用户可能为：username@gmail.com password 

            smtp.Credentials = new NetworkCredential(UserName, Password);
            smtp.Port = 25; // Gmail 使用 465 和 587 端口 
            smtp.Host = SmtpServer; // 如 smtp.163.com, smtp.gmail.com 
            smtp.EnableSsl = false; // 如果使用GMail，则需要设置为true
            //smtp.SendCompleted += new SendCompletedEventHandler(SendMailCompleted);
            try
            {
                smtp.Send(mailMessage);
            }
            catch (SmtpException ex)
            {
                SendMailMessageToXml(mailMessage);
                throw new Exception("邮件发送失败，请登录管理后台检查邮件配置是否正确。原因："+ex.Message);
            }            

            //第二类方法：OpenSMTP
            //string smtpHost = SmtpServer; //"smtp.163.com"; 
            //int smtpPort = 25;
            //string senderEmail = AdminEmail; //"thehim@163.com";
            //Smtp smtp = new Smtp(smtpHost, smtpPort);
            //smtp.Password = Password;//"mypass";//用户密码 
            //smtp.Username = UserName; //"thehim"; //用户名称

            ////定义邮件信息==========================================================
            //OpenSmtp.Mail.MailMessage msg = new OpenSmtp.Mail.MailMessage();//(senderEmail, recipientEmail);
            //OpenSmtp.Mail.EmailAddress addfrom = new EmailAddress(senderEmail); //发件人
            //addfrom.Name = "短客网";
            //msg.From = addfrom;

            //OpenSmtp.Mail.EmailAddress addbcc = new EmailAddress(to);
            //msg.AddRecipient(addbcc, AddressType.To);

            //msg.Subject = subject;
            //msg.Charset = "gb2312";
            //msg.Body = message;

            //smtp.SendMail(msg);

            ////第三类方法：Mailserder Using
            //MailSender ms = new MailSender();
            //ms.From = AdminEmail;
            //ms.To = to;
            //ms.Subject = subject;
            //ms.Body = message;
            //ms.UserName = UserName;  
            //ms.Password = Password; 
            //ms.Server = SmtpServer;

            ////ms.Attachments.Add(new MailSender.AttachmentInfo(@"D:\\test.txt"));
            //ms.SendMail();
            return true;
        }

        /// <summary>
        /// 接受邮件，处理所有正确存在邮件
        /// </summary>
        public MailResult ReceiveMail(string asmName, string typeName, string methodName ,bool delete)
        {
            MailResult result = new MailResult();
            string strPort = "";
            if (strPort == "" || strPort == string.Empty) strPort = "110";
            POPClient popClient = new POPClient();
            try
            {
                popClient.Connect(PopServer, Convert.ToInt32(strPort));
                popClient.Authenticate(UserName, Password);
                int count = popClient.GetMessageCount();
                                
                int resultCount = 0;
                for (int i = count; i >= 1; i--)
                {
                    OpenPOP.MIMEParser.Message msg = popClient.GetMessage(i, false);
                    if (msg != null)
                    {
                        resultCount++;
                        //获取DLL所在路径:
                        string dllPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                        dllPath = Path.GetDirectoryName(dllPath);
                        //依据所要执行类类型名，获取类实例：
                        string asmNames = asmName;//程序集名称（*.dll）
                        string dllFile = Path.Combine(dllPath, asmNames);
                        Assembly asm = Assembly.LoadFrom(dllFile);
                        //获取类方法并执行：
                        object obj = asm.CreateInstance(typeName, false);
                        Type type = obj.GetType();//类名
                        MethodInfo method = type.GetMethod(methodName);//方法名称
                        //如果需要参数则依此行
                        object[] args = new object[] { (object)msg ,(object)result};
                        //执行并调用方法
                        method.Invoke(obj, args);
                        if (delete)
                        {
                            popClient.DeleteMessage(i); //邮件保存成功，删除服务器备份
                        }
                    }
                }
                result.Count = resultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }  
            finally
            {
                popClient.Disconnect();
            }
            return result;
        }

        /// <summary>
        /// 接受邮件，处理所有正确存在邮件
        /// </summary>
        public MailResult ReceiveMail(string asmName, string typeName, string methodName, bool delete,string stateText)
        {
            MailResult result = new MailResult();
            result.StateText = stateText;
            string strPort = "";
            if (strPort == "" || strPort == string.Empty) strPort = "110";
            POPClient popClient = new POPClient();
            try
            {
                popClient.Connect(PopServer, Convert.ToInt32(strPort));
                popClient.Authenticate(UserName, Password);
                int count = popClient.GetMessageCount();

                int resultCount = 0;
                for (int i = count; i >= 1; i--)
                {
                    OpenPOP.MIMEParser.Message msg = popClient.GetMessage(i, false);
                    if (msg != null)
                    {
                        resultCount++;
                        
                        //获取DLL所在路径:
                        string dllPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                        dllPath = Path.GetDirectoryName(dllPath);
                        //依据所要执行类类型名，获取类实例：
                        string asmNames = asmName;//程序集名称（*.dll）
                        string dllFile = Path.Combine(dllPath, asmNames);
                        Assembly asm = Assembly.LoadFrom(dllFile);
                        //获取类方法并执行：
                        object obj = asm.CreateInstance(typeName, false);
                        Type type = obj.GetType();//类名
                        MethodInfo method = type.GetMethod(methodName);//方法名称
                        //如果需要参数则依此行
                        object[] args = new object[] { (object)msg, (object)result };
                        //执行并调用方法
                        method.Invoke(obj, args);
                        if (delete)
                        {
                            popClient.DeleteMessage(i); //邮件保存成功，删除服务器备份
                        }
                    }
                }
                result.Count = resultCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                popClient.Disconnect();
            }
            return result;
        }

        /// <summary>
        /// 未能正确发送的邮件将以XML形式转存至/_Data/SendEmail/目录下
        /// </summary>
        /// <param name="mailMessage"></param>
        public void SendMailMessageToXml(MailMessage mailMessage)
        {
            try
            {
                string subject = mailMessage.Subject.ToString();//邮件标题
                string body = (string)mailMessage.Body;//邮件正文
                string replyTime = DateTime.Now.ToString();//邮件
                string user = mailMessage.To[0].Address;//收件人地址
                string formUser = mailMessage.From.Address;//发件人地址

                if (subject != "")
                {
                    string filePath = HttpContext.Current.Server.MapPath("/_Data/SendEmail/");
                    DateTime time = Convert.ToDateTime(replyTime);
                    string fileName = subject + DateTime.Now.ToString(".yyyy_MM_dd_HH_mm_ss") + ".xml";
                    string path = Path.Combine(filePath, fileName);

                    //检查是否XML文件存放临时路径存在，如果不存在则进行处理
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    //检查XMLSchema文件是否存在，如果不存在则进行处理
                    if (!File.Exists(subject))
                    {
                        XmlDocument doc = new XmlDocument();
                        //转换字符
                        subject = We7Helper.Base64Encode(subject);
                        user = We7Helper.Base64Encode(user);
                        body = We7Helper.Base64Encode(body);
                        formUser = We7Helper.Base64Encode(formUser);

                        string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n"
                            + "<root><infoSubject>" + subject + "</infoSubject><infoUser>" +user + "</infoUser><infoFormUser>" +
                            formUser + "</infoFormUser><infoBody>" + body + "</infoBody><infoTime>"
                            + replyTime + "</infoTime></root>";
                        doc.LoadXml(xml);
                        doc.Save(path);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

	}

    public class MailResult
    {
        int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        int success;

        public int Success
        {
            get { return success; }
            set { success = value; }
        }

        string stateText;

        public string StateText
        {
            get { return stateText; }
            set { stateText = value; }
        }

         

    }

    /// <summary>
    /// 邮件消息模板类
    /// </summary>
    public class MailMessageTemplate
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="templateFile"></param>
        /// <param name="itemName"></param>
        public MailMessageTemplate(string templateFile, string itemName)
        {
            string configFilePath = HttpContext.Current.Server.MapPath("~/config/"+templateFile);
            if (File.Exists(configFilePath))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(configFilePath);
                XmlNode root = xml.SelectSingleNode("configuration");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "item"
                        && n.Attributes["name"].Value == itemName )
                    {
                        foreach (XmlNode m in n.ChildNodes)
                        {
                            if (m.NodeType != XmlNodeType.Comment && m.Name == "EmailTitle")
                                Subject = m.InnerXml;
                            if (m.NodeType != XmlNodeType.Comment && m.Name == "Emailcontent")
                                Body = m.InnerXml;
                        }
                    }
                }

                FormatTemplateValue();
            }
        }

        void FormatTemplateValue()
        {
            if (!string.IsNullOrEmpty(Subject))
            {
                Subject = Subject.Replace("${SiteName}",SiteConfigs.GetConfig().SiteName);
            }
            
            if (!string.IsNullOrEmpty(Body))
            {
                Body = Body.Replace("${SiteName}", SiteConfigs.GetConfig().SiteName);
                Body = Body.Replace("${SiteUrl}", SiteConfigs.GetConfig().RootUrl);
                Body = Body.Replace("${DateTime.Now}",DateTime.Now.ToString());
            }

        }
    }
}

