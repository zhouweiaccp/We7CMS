using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using System.Web;

namespace We7.CMS.Common
{
    /// <summary>
    /// 数据控件类
    /// </summary>
    [Serializable]
    public class DataControl : IXml, IJsonResult
    {
        string basePath = string.Empty;
        string name = string.Empty;
        string description = string.Empty;
        string fileName = string.Empty;
        string author = string.Empty;
        string version = string.Empty;
        string tag = string.Empty;
        DateTime created = DateTime.Now;
        List<DataControlParameter> parameters;
        List<ResourceFile> files;
        Dictionary<string, string> styles;
        bool deepLoad = false;
        string demoUrl = string.Empty;
        string remark = string.Empty;
        string control = string.Empty;
        string type = string.Empty;
        string controlBasePath = "We7Controls";

        /// <summary>
        /// 数据控件类构造函数
        /// </summary>
        public DataControl()
        {
            parameters = new List<DataControlParameter>();
            files = new List<ResourceFile>();
            created = DateTime.Now;
            styles = new Dictionary<string, string>();
        }

        /// <summary>
        /// 数据控件参数类对象
        /// </summary>
        public List<DataControlParameter> Parameters
        {
            set { parameters = value; }
            get { return parameters; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ResourceFile> Files
        {
            get { return files; }
        }

        /// <summary>
        /// 版本信息
        /// </summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
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
        /// 创建时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        /// <summary>
        /// 原地址
        /// </summary>
        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool DeepLoad
        {
            get { return deepLoad; }
            set { deepLoad = value; }
        }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// 详细介绍
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 缩略图 图片地址
        /// </summary>
        public string DemoUrl
        {
            get { return demoUrl; }
            set { demoUrl = value; }
        }

        /// <summary>
        /// 控件的物理文件名称,不含后辍名.
        /// </summary>
        public string Control
        {
            get { return control; }
            set { control = value; }
        }

        /// <summary>
        /// 控件的物理文件名称,不含后辍名.
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 控件的标签
        /// </summary>
        public string CtrDir
        {
            get
            {
                string[] strs = Control.Split('.');
                if (strs == null || String.IsNullOrEmpty(strs[0]))
                    throw new Exception("无法取得控件的根目录");

                return strs[0];
            }
        }

        /// <summary>
        /// 控件的样式文件
        /// </summary>
        public Dictionary<string,string> Styles
        {
            get
            {
                return styles;
            }
        }

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
        /// 获取目录(注意:这个方法对新控件无效)
        /// </summary>
        /// <param name="root"></param>
        /// <param name="fn"></param>
        public void FromFile(string root, string fn)
        {
            Parameters.Clear();
            Files.Clear();
            BasePath = root;
            FileName = fn;
            
            string file = Path.Combine(root, fn);
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            this.FromXml(doc.DocumentElement);
        }

        /// <summary>
        /// 保存XML信息
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("DataControl");
            xe.SetAttribute("name", Name);
            xe.SetAttribute("description", Description);
            xe.SetAttribute("author", Author);
            xe.SetAttribute("version", Version);
            xe.SetAttribute("created", Created.ToString());
            xe.SetAttribute("tag", Tag);
            xe.SetAttribute("remark", Remark);
            xe.SetAttribute("demoUrl", DemoUrl);
            xe.SetAttribute("control", Control);
            xe.SetAttribute("type", Type);

            if (DeepLoad)
            {
                foreach (DataControlParameter dp in parameters)
                {
                    xe.AppendChild(dp.ToXml(doc));
                }
                foreach (ResourceFile df in files)
                {
                    xe.AppendChild(df.ToXml(doc));
                }
            }
            return xe;
        }

        /// <summary>
        /// 获取XML信息
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public IXml FromXml(XmlElement xe)
        {
            Name = xe.GetAttribute("name");
            Description = xe.GetAttribute("description");
            Author = xe.GetAttribute("author");
            Version = xe.GetAttribute("version");
            DateTime.TryParse(xe.GetAttribute("created"), out created);
            Tag = xe.GetAttribute("tag");
            Remark = xe.GetAttribute("remark");
            DemoUrl = xe.GetAttribute("demoUrl");
            Control = xe.GetAttribute("control");
            Type = xe.GetAttribute("type");
            FileName = xe.GetAttribute("filePath");

            foreach (XmlElement e in xe.SelectNodes("Parameter"))
            {
                DataControlParameter dp = new DataControlParameter();
                dp.FromXml(e);
                Parameters.Add(dp);
            }
            foreach (XmlElement e in xe.SelectNodes("File"))
            {
                ResourceFile df = new ResourceFile();
                df.FromXml(e);
                Files.Add(df);
            }
            return this;          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            LoadStyles();
            int num=0;
            StringBuilder sb = new StringBuilder("{");
            sb.AppendFormat("{0}:'{1}',", "name", name);
            sb.AppendFormat("{0}:'{1}',", "description", description);
            sb.AppendFormat("{0}:'{1}',", "author", author);
            sb.AppendFormat("{0}:'{1}',", "version", version);
            sb.AppendFormat("{0}:'{1}',", "created", created.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendFormat("{0}:'{1}',", "demoUrl", demoUrl);
            sb.AppendFormat("{0}:'{1}',", "control", control);
            sb.AppendFormat("{0}:'{1}',", "ctrDir", CtrDir);
            sb.AppendFormat("{0}:'{1}',", "remark", remark);
            sb.AppendFormat("{0}:'{1}',", "type", Type);
            sb.AppendFormat("{0}:'{1}',", "fileName", FileName);

            StringBuilder sbParam = new StringBuilder("[");
            for (int i = 0; i < parameters.Count; i++)
            {
                sbParam.AppendFormat("{0},", parameters[i].ToJson());
                if (i == parameters.Count - 1)
                    sbParam.Remove(sbParam.Length - 1, 1);
            }
            sbParam.Append("]");
            sb.AppendFormat("{0}:{1},", "parameters", sbParam.ToString());

            sbParam.Length = 0;
            sbParam.Append("[");
            for (int i = 0; i < files.Count; i++)
            {
                sbParam.AppendFormat("{0},", files[i].ToJson());
                if (i == files.Count - 1)
                    sbParam.Remove(sbParam.Length - 1, 1);
            }
            sbParam.Append("]");
            sb.AppendFormat("{0}:{1}", "files", sbParam.ToString());

            sbParam.Length = 0;
            sbParam.Append("[");
            foreach (KeyValuePair<string, string> kvp in styles)
            {
                sbParam.Append("{");
                sbParam.AppendFormat("key:'{0}',value:'{1}'", kvp.Key, kvp.Value);
                sbParam.Append("},");
            }
            if(sbParam.Length>1)sbParam.Remove(sbParam.Length-1,1);
            sbParam.Append("]");
            sb.AppendFormat(",{0}:{1}", "styles",sbParam.ToString());
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 加载指定目录下的Css样式
        /// </summary>
        /// <param name="styledir"></param>
        public void LoadStyles()
        {
            string ctrPath = HttpContext.Current.Server.MapPath(FileName);
            FileInfo fi = new FileInfo(ctrPath);
            if (fi.Exists)
            {
                DirectoryInfo di = fi.Directory.Parent;
                string styledir = Path.Combine(di.FullName, "Style");
                DirectoryInfo dir = new DirectoryInfo(styledir);
                if (!dir.Exists)
                    return;
                FileInfo[] files = dir.GetFiles(String.Format("{0}_*.css", Control));
                foreach (FileInfo f in files)
                {
                    try
                    {
                        string fn = Path.GetFileNameWithoutExtension(f.Name);
                        string[] kvp = fn.Split('_')[1].Split('.');
                        styles.Add(kvp[0], kvp[1]);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
