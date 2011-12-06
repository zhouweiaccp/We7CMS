using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace We7.CMS.Common
{

    public enum DataControlType
    {
        /// <summary>
        /// 系统控件
        /// </summary>
        System,
        /// <summary>
        /// 插件中的控件
        /// </summary>
        Plugin,
        Model
    }
    /// <summary>
    /// 数据控件信息类
    /// </summary>
    public class DataControlInfo:IJsonResult
    {
        public string DataControlConfig = "DataControl.xml";

        
        List<DataControlParameter> parameters = new List<DataControlParameter>();

        private string name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string desc;
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        private List<DataControl> controls;
        /// <summary>
        /// 控件
        /// </summary>
        public List<DataControl> Controls
        {
            get { return controls; }
            set { controls = value; }
        }
        private string directory;
        /// <summary>
        /// 控件目录
        /// </summary>
        public string Directory
        {
            get { return directory; }
            set { directory = value; }
        }
        private string tag;
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string Created { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 默认的控件名称
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// 当前控件的模型名称
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 当前模型控件的类型
        /// </summary>
        public string CtrType { get; set; }

        /// <summary>
        /// 控件路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 控件所属分组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 默认控件名称
        /// </summary>
        public DataControl DefaultControl { get; set; }
       
        #region 组描述属性
        /// <summary>
        /// 组标签
        /// </summary>
        public string GroupLabel { get; set; }
        /// <summary>
        /// 组描述
        /// </summary>
        public string GroupDesc { get; set; }
        /// <summary>
        /// 组ICON
        /// </summary>
        public string GroupIcon { get; set; }
        /// <summary>
        /// 组默认控件
        /// </summary>
        public string GroupDefaultType { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataControlInfo()
        {
            controls = new List<DataControl>();
        }

        public DataControlInfo(string configName):this()
        {
            DataControlConfig = configName;
        }


        #region IJsonResult 成员

        /// <summary>
        /// 转化为Json
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder("{");
            sb.AppendFormat("{0}:'{1}',", "name", Name);
            sb.AppendFormat("{0}:'{1}',", "desc", Desc);
            sb.AppendFormat("{0}:'{1}',", "dir", Directory);
            StringBuilder sbctrs=new StringBuilder("[");
            foreach (DataControl dc in controls)
            {
                sbctrs.AppendFormat("{0},", dc.ToJson());
            }
            if(sbctrs.Length>1)
                sbctrs.Remove(sbctrs.Length-1,1);
            sbctrs.Append("]");
            sb.AppendFormat("{0}:{1}", "items", sbctrs.ToString());
            sb.Append("}");
            return sb.ToString();
        }

        #endregion
        /// <summary>
        /// 加载控件信息
        /// </summary>
        /// <param name="ctrdir">控件的根目录(物理路径)</param>
        public void Load(string ctrdir)
        {
            DirectoryInfo di= new DirectoryInfo(ctrdir);
            if (!di.Exists)
                return;
            Directory = di.Name;
           
            string cfgPath = System.IO.Path.Combine(ctrdir, DataControlConfig);
            XmlDocument doc = new XmlDocument();
            doc.Load(cfgPath);

            FromXml(doc.DocumentElement);
            LoadControl(ctrdir);
        }

        /// <summary>
        /// 从Xml中加载数据
        /// </summary>
        /// <param name="xe"></param>
        public void FromXml(XmlElement xe)
        {            
            Name = xe.GetAttribute("name");
            Desc = xe.GetAttribute("desc");
            Tag=xe.GetAttribute("tag");
            Author = xe.GetAttribute("author");
            Version = xe.GetAttribute("version");
            Created = xe.GetAttribute("created");
            Remark = xe.GetAttribute("remark");
            Default = xe.GetAttribute("default");
            Model = xe.GetAttribute("model");
            CtrType = xe.GetAttribute("ctrtype");
            Group = xe.GetAttribute("group");
            if (String.IsNullOrEmpty(Group))
            {
                if (!String.IsNullOrEmpty(Model))
                {
                    Group = "模型控件";
                }
                else
                {
                    Group = "系统控件";
                }
            }

            foreach (XmlElement e in xe.SelectNodes("Parameter"))
            {
                DataControlParameter dp = new DataControlParameter();
                dp.FromXml(e);
                parameters.Add(dp);
            }
        }
        
        /// <summary>
        /// 加载控件信息
        /// </summary>
        /// <param name="ctrdir"></param>
        void LoadControl(string ctrdir)
        {
            string ucDir = System.IO.Path.Combine(ctrdir,"Page");
            DirectoryInfo di = new DirectoryInfo(ucDir);
            if (di.Exists)
            {
                Regex regex = new Regex("(?<=<!-+#+.*)(\\S+?)\\s*?=\\s*?[\"'](\\S+?)[\"'](?=.*#+-+>)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);
                Regex regexFile = new Regex(String.Format("{0}(\\.\\w+)?.ascx", Directory), RegexOptions.IgnoreCase);
                FileInfo[] fs = di.GetFiles(String.Format("{0}*.ascx", Directory));
                foreach (FileInfo f in fs)
                {
                    if (!regexFile.IsMatch(f.Name))
                        break;

                    DataControl dc = new DataControl();
                    dc.Control = System.IO.Path.GetFileNameWithoutExtension(f.Name);
                    dc.FileName =GetRelateivePath(f.FullName);
                    dc.Parameters = parameters;
                    Controls.Add(dc);
                    using (StreamReader sr = f.OpenText())
                    {
                        //string s = sr.ReadToEnd(); 
                        //thehim修改：上面语句遇到控件中的注释符较多或控件字符较多时会导致性能极具下降
                        string s = sr.ReadLine();
                        while (!s.EndsWith("##-->") && !sr.EndOfStream)
                            s += sr.ReadLine();

                        if (s.EndsWith("##-->"))
                        {

                            MatchCollection mc = regex.Matches(s);
                            foreach (Match m in mc)
                            {
                                switch (m.Groups[1].Value.Trim().ToLower())
                                {
                                    case "name":
                                        dc.Name = m.Groups[2].Value;
                                        break;
                                    case "author":
                                        dc.Author = m.Groups[2].Value;
                                        break;
                                    case "tag":
                                        dc.Tag = m.Groups[2].Value;
                                        break;
                                    case "desc":
                                        dc.Description = m.Groups[2].Value;
                                        break;
                                    case "created":
                                        DateTime t = new DateTime();
                                        DateTime.TryParse(m.Groups[2].Value, out t);
                                        dc.Created = t;
                                        break;
                                    case "remark":
                                        dc.Remark = m.Groups[2].Value;
                                        break;
                                    case "version":
                                        dc.Version = m.Groups[2].Value;
                                        break;
                                    case "demourl":
                                        dc.DemoUrl = m.Groups[2].Value;
                                        break;
                                }
                            }
                        }
                    }
                    //dc.DemoUrl = GetDemoUrl(ctrdir, dc.Control, dc.DemoUrl);
                    dc.DemoUrl = GetDemoUrl(dc);
                    if (!String.IsNullOrEmpty(Default) && dc.Control.ToLower().Trim() == Default.ToLower().Trim())
                        DefaultControl = dc;
                }
                if (DefaultControl == null && Controls.Count > 0)
                    DefaultControl = Controls[0];
            }
        }

        /// <summary>
        /// 获取数据控件展示图片地址
        /// </summary>
        /// <param name="ctrDir"></param>
        /// <param name="ctrName"></param>
        /// <param name="demoUrl"></param>
        /// <returns></returns>
        string GetDemoUrl(string ctrDir,string ctrName,string demoUrl)
        {
            if (String.IsNullOrEmpty(demoUrl) || !File.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, demoUrl)))
            {
                demoUrl = String.Format("We7Controls/{0}/Page/{1}.jpg",ctrName.Split('.')[0],ctrName);
                if (!File.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, demoUrl)))
                {
                    demoUrl = "Admin/images/s.jpg";
                }
            }
            return demoUrl;
        }

        string GetDemoUrl(DataControl dc)
        {
            string demoUrl=dc.DemoUrl;
            if (String.IsNullOrEmpty(demoUrl) || !File.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, demoUrl)))
            {
                demoUrl = dc.FileName.Replace(System.IO.Path.GetExtension(dc.FileName), ".jpg").TrimStart('/');
                if (!File.Exists(HttpContext.Current.Server.MapPath("~/"+demoUrl)))
                {
                    demoUrl = "Admin/images/s.jpg";
                }
            }
            return demoUrl;
        }



        /// <summary>
        /// 取得控件的相对路径("以/开头")
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetRelateivePath(string fullPath)
        {
            if (String.IsNullOrEmpty(fullPath))
                throw new ArgumentNullException("控件路径不能为空");
            //Regex regex = new Regex("We7Controls", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            fullPath=Regex.Replace(fullPath.Replace("/", "\\"), "^"+AppDomain.CurrentDomain.BaseDirectory.Replace("\\","\\\\"), "", RegexOptions.IgnoreCase);
            return  "/" + fullPath.TrimStart('/').TrimStart('\\').Replace("\\", "/");
        }


        /// <summary>
        /// 向一个节点中添加所有数据
        /// </summary>
        /// <param name="parentnode"></param>
        /// <param name="children"></param>
        void AppendNodeList(XmlNode parentnode, XmlNodeList children)
        {
            foreach (XmlNode n in children)
            {
                parentnode.AppendChild(n.Clone());
            }
        }

        /// <summary>
        /// 加载控件信息
        /// </summary>
        /// <param name="ctrPath">控件路径</param>
        /// <returns></returns>
        public static DataControlInfo LoadInfo(string ctrPath)
        {
            DataControlInfo dci = new DataControlInfo();
            dci.Load(ctrPath);
            return dci;
        }

        /// <summary>
        /// 加载控件信息
        /// </summary>
        /// <param name="ctrPath">控件路径</param>
        /// <returns></returns>
        public static DataControlInfo LoadInfo(string ctrPath,string configName)
        {
            DataControlInfo dci = new DataControlInfo(configName);
            dci.Load(ctrPath);
            return dci;
        }
    }
}
