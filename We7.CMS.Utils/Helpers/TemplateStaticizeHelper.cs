using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using System.IO;
using System.Net;
using We7.CMS.Common;
using System.Collections.Specialized;
using System.Xml;
using System.Web;

namespace We7.CMS.Helpers
{
    /// <summary>
    /// 页面静态化
    /// </summary>
    public class TemplateStaticizeHelper : BaseHelper
    {
        private static TemplateStaticizeHelper instance;
        private static object objLock = new object();

        private string HtmlTempateDirectoryPattern = "_Skin/{0}/HtmlTemplate";
        private string htmlTemplateRoot;
        private string physicalHtmlTemplateRoot;
        private WebClient client;
        private List<string> VisitUrls = new List<string>();
        private bool isRunning = false;
        private int createdTimes = 0;
        private List<string> commonUrls;
        private DateTime StartTime;
        private List<string> SuccessMsg = new List<string>();
        private List<string> ErrorMsg = new List<string>();

        public static TemplateStaticizeHelper Instance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new TemplateStaticizeHelper();
                    }
                }
            }
            return instance;
        }

        private TemplateStaticizeHelper() { }

        public void Start()
        {
            ClearHtmlTemplate();
            CreateCommonUrls();
            CreateChannelUrls();
            FormatVisitUrls();

            StartTime = DateTime.Now;
            WriteState();
            CreateHtmlTempate();
        }

        public List<Message> GetMessage(out string persent)
        {
            List<Message> list = new List<Message>();
            string path = GetMessagePath();
            if (File.Exists(path))
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    while (!reader.EndOfStream)
                    {
                        string message = reader.ReadLine();
                        if (message.Length > 0)
                        {
                            if (message.Substring(0, 1) == "1")
                            {
                                list.Add(new Message() { Msg = message.Substring(1), Success = true });
                            }
                            else if (message.Substring(0, 1) == "0")
                            {
                                list.Add(new Message() { Msg = message.Substring(1), Success = false });
                            }
                        }
                    }
                }
            }
            string statePath = Path.Combine(PhysicalHtmlTemplateRoot, "State.txt");
            persent = "0%";
            if (File.Exists(statePath))
            {
                using (StreamReader reader = File.OpenText(statePath))
                {
                    while (!reader.EndOfStream)
                    {
                        persent = reader.ReadLine();
                    }
                }
            }
            return list;
        }



        private void CreateCommonUrls()
        {
            foreach (string url in CommonUrls)
            {
                VisitUrls.Add(url);
            }
        }

        private void CreateChannelUrls()
        {
            Channel[] chs = HelperFactory.Instance.GetHelper<ChannelHelper>().GetAllChannels();
            if (chs != null)
            {
                foreach (Channel ch in chs)
                {
                    VisitUrls.Add(ch.FullUrl);
                    VisitUrls.Add(ch.FullUrl + "1.html");
                    VisitUrls.Add(ch.FullUrl + "search.aspx");
                }
            }
        }

        private string GetMessagePath()
        {
            return Path.Combine(PhysicalHtmlTemplateRoot, "Message.txt");
        }

        private bool Runable
        {
            get { return File.Exists(GetMessagePath()); }
        }

        private void FormatVisitUrls()
        {
            HttpRequest request = HttpContext.Current.Request;
            for (int i = 0; i < VisitUrls.Count; i++)
            {
                VisitUrls[i] = String.Format("Http://{0}:{1}/{2}?CreateHtml=1", request.Url.Host, request.Url.Port, VisitUrls[i].TrimStart('/', '\\'));
            }
        }

        private List<string> CommonUrls
        {
            get
            {
                if (commonUrls == null)
                {
                    commonUrls = new List<string>();

                    XmlDocument doc = new XmlDocument();
                    doc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config/CommonUrls.xml"));
                    XmlNodeList nodes = doc.DocumentElement.SelectNodes("item");
                    if (nodes != null)
                    {
                        foreach (XmlElement node in nodes)
                        {
                            commonUrls.Add(node.GetAttribute("url"));
                        }
                    }
                }
                return commonUrls;
            }
        }

        private void CreateHtmlTempate()
        {
            foreach (string url in VisitUrls)
            {
                if (ProcessState != State.Running)
                    break;

                try
                {
                    WebRequest request = HttpWebRequest.Create(url);
                    request.Timeout = 60000;
                    using (request.GetResponse())
                    {
                        SuccessMsg.Add(url);
                    }
                }
                catch
                {
                    ErrorMsg.Add(url);
                }
                WriteState();
                WriteTempMsg();
            }
            WriteMsg();
        }

        protected void WriteTempMsg()
        {
            if (createdTimes++ % 10 == 0)
            {
                WriteMsg();
            }
        }

        protected void WriteMsg()
        {
            if (!Directory.Exists(PhysicalHtmlTemplateRoot))
            {
                Directory.CreateDirectory(PhysicalHtmlTemplateRoot);
            }
            string path = Path.Combine(PhysicalHtmlTemplateRoot, "Message.txt");
            using (StreamWriter writer = new StreamWriter(File.Open(path, FileMode.Append, FileAccess.Write)))
            {
                foreach (string s in SuccessMsg)
                {
                    writer.WriteLine("1" + s);
                }
                foreach (string s in ErrorMsg)
                {
                    writer.WriteLine("0" + s);
                }

                SuccessMsg.Clear();
                ErrorMsg.Clear();
            }
        }

        protected void WriteState()
        {
            if (!Directory.Exists(PhysicalHtmlTemplateRoot))
            {
                Directory.CreateDirectory(PhysicalHtmlTemplateRoot);
            }
            string path = Path.Combine(PhysicalHtmlTemplateRoot, "State.txt");
            using (StreamWriter writer = new StreamWriter(File.Open(path, FileMode.Create, FileAccess.Write)))
            {
                writer.WriteLine(StartTime.ToString());
                writer.WriteLine(DateTime.Now.ToString());
                writer.WriteLine(String.Format("{0}%", createdTimes * 100 / VisitUrls.Count));
            }
        }

        public State ProcessState
        {
            get
            {
                string path = Path.Combine(PhysicalHtmlTemplateRoot, "State.txt");
                if (!File.Exists(path))
                {
                    return State.None;
                }
                else
                {
                    try
                    {
                        using (StreamReader reader = File.OpenText(path))
                        {
                            string s1 = reader.ReadLine().Trim();
                            string s2 = reader.ReadLine().Trim();
                            DateTime dtStart, dtLast;
                            if (DateTime.TryParse(s1, out dtStart) && DateTime.TryParse(s2, out dtLast))
                            {
                                if (dtLast.AddSeconds(60) > DateTime.Now)
                                    return State.Running;
                                return State.Complated;
                            }
                            return State.None;
                        }
                    }
                    catch (IOException ex)
                    {
                        return State.Running;
                    }
                    catch
                    {
                        return State.None;
                    }
                }
            }
        }

        protected string MessageFile
        {
            get
            {
                return Path.Combine(PhysicalHtmlTemplateRoot, "Message.txt");
            }
        }

        private void ClearHtmlTemplate()
        {

            if (Directory.Exists(PhysicalHtmlTemplateRoot))
            {
                Directory.Delete(PhysicalHtmlTemplateRoot, true);
            }
            Directory.CreateDirectory(PhysicalHtmlTemplateRoot);
        }

        private string PhysicalHtmlTemplateRoot
        {
            get
            {
                if (String.IsNullOrEmpty(physicalHtmlTemplateRoot))
                {
                    physicalHtmlTemplateRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HtmlTempateRoot);
                }
                return physicalHtmlTemplateRoot;
            }
        }

        private string HtmlTempateRoot
        {
            get
            {
                if (String.IsNullOrEmpty(htmlTemplateRoot))
                {
                    return htmlTemplateRoot = String.Format("_Skins/{0}/HtmlTemplate", GroupName);
                }
                return htmlTemplateRoot;
            }
        }

        private string GroupName
        {
            get { return Path.GetFileNameWithoutExtension(GeneralConfigs.GetConfig().DefaultTemplateGroupFileName); }
        }

        public enum State
        {
            None, Running, Complated
        }

        public class Message
        {
            public bool Success { get; set; }

            public string Msg { get; set; }
        }
    }
}
