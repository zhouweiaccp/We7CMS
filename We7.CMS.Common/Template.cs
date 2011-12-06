using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 模板页对象
    /// </summary>
    [Serializable]
    public class Template
    {
        string name;
        string description;
        string fileName;
        string basePath;
        bool isSubTemplate;
        bool isCode;
        DateTime created=DateTime.Now;
        DateTime updated = DateTime.Now;
        List<string> controls;
        List<ResourceFile> files;
        string templateType;
        string templateTypeText;

        /// <summary>
        /// 模板实体对象
        /// </summary>
        public Template()
        {
            controls = new List<string>();
            files = new List<ResourceFile>();
        }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<string> Controls
        {
            get { return controls; }
        }
        
        public List<ResourceFile> Files
        {
            get { return files; }
        }

        /// <summary>
        /// 描述
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
        /// 路径
        /// </summary>
        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
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
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// 是否是子模板
        /// </summary>
        public bool IsSubTemplate
        {
            get { return isSubTemplate; }
            set { isSubTemplate = value; }
        }

        /// <summary>
        /// 是否代码编辑
        /// </summary>
        public bool IsCode
        {
            get { return isCode; }
            set { isCode = value; }
        }

        /// <summary>
        /// 是否母版
        /// </summary>
        public bool IsMasterPage { get; set; }

        public string IsSubTemplateText
        {
            get 
            {
                if (IsSubTemplate)
                    return "子模板";
                else if (TemplateType == "9")
                    return "母版";
                else
                    return "";
            }
        }


        /// <summary>
        /// 模板类型。普通模板 1、子模板 0、母版 9
        /// </summary>
        public string TemplateType
        {
            get { return templateType; }
            set { templateType = value; }
        }

        public string Version { get; set; }

        /// <summary>
        /// 是否为可视化模板
        /// </summary>
        public bool IsVisualTemplate { get; set; }

        /// <summary>
        /// 模板类型文本状态
        /// </summary>
        public string TemplateTypeText
        {
            get
            {
                switch (TemplateType)
                {
                    case "0":
                        return "子模板";
                    case "1":
                        return "模板";
                    case "9":
                        return "母版";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 默认绑定列表
        /// </summary>
        public string DefaultBindText { get; set; }

        #region 临时属性
        /// <summary>
        /// 正在编辑的实际文件全名：指副本文件
        /// </summary>
        public string EditFileFullPath { get; set; }
        /// <summary>
        /// 正本模板文件路径，如/_skins/mydefault/home.ascx
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 所属栏目组
        /// </summary>
        public string SkinFolder { get; set; }
        /// <summary>
        /// 是否新建
        /// </summary>
        public bool IsNew { get; set; }

        #endregion

        public void FromFile(string bp, string fn)
        {
            BasePath = bp;
            FileName = Path.GetFileNameWithoutExtension(fn);
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(basePath, fn));
            FromXml(doc.DocumentElement);
        }

        public void ToFile(string fn)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xd = doc.CreateXmlDeclaration("1.0", "utf-8", "");
            doc.AppendChild(xd);
            doc.AppendChild(ToXml(doc));
            doc.Save(fn);
        }

        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("template");
            xe.SetAttribute("name", Name);
            xe.SetAttribute("description", Description);
            xe.SetAttribute("templateType", TemplateType);
            xe.SetAttribute("templateTypeText", TemplateTypeText);
            xe.SetAttribute("created", Created.ToString());
            xe.SetAttribute("updated", Created.ToString());
            xe.SetAttribute("isSub", IsSubTemplate ? Boolean.TrueString : Boolean.FalseString);
            xe.SetAttribute("isCode", IsCode ? Boolean.TrueString : Boolean.FalseString);
            xe.SetAttribute("bindText", DefaultBindText);
            xe.SetAttribute("isVisualTemplate", IsVisualTemplate ? Boolean.TrueString : Boolean.FalseString);
            return xe;
        }

        public Template FromXml(XmlElement xe)
        {
            name = xe.GetAttribute("name");
            description = xe.GetAttribute("description");
            if(!string.IsNullOrEmpty(xe.GetAttribute("created")))
                created = Convert.ToDateTime(xe.GetAttribute("created"));
            if (!string.IsNullOrEmpty(xe.GetAttribute("updated")))
                updated = Convert.ToDateTime(xe.GetAttribute("updated"));
            IsSubTemplate = xe.GetAttribute("isSub") == Boolean.TrueString;
            IsCode = xe.GetAttribute("isCode") == Boolean.TrueString;
            IsVisualTemplate = xe.GetAttribute("isVisualTemplate") == Boolean.TrueString;
            Version = xe.GetAttribute("ver");

            templateType = xe.GetAttribute("templateType");
            templateTypeText = xe.GetAttribute("templateTypeText");
            DefaultBindText = xe.GetAttribute("bindText");

            //foreach (XmlElement e in xe.SelectNodes("File"))
            //{
            //    ResourceFile f = new ResourceFile();
            //    f.FromXml(e);
            //    Files.Add(f);
            //}
            //foreach (XmlElement e in xe.SelectNodes("DataControl"))
            //{
            //    Controls.Add(e.InnerText);
            //}
            return this;
        }

        /// <summary>
        /// 验证模版文件是否已被外部编辑器编辑
        /// </summary>
        /// <param name="fn">模版文件路径</param>
        /// <returns></returns>
        public bool VerifyUpdated(string fn)
        {
            FileInfo f = new FileInfo(fn);
            return f.LastWriteTime > Updated;
        }
    }

    /// <summary>
    /// 模板绑定配置类
    /// </summary>
    public class TemplateBindConfig
    {
        /// <summary>
        ///处理者
        /// </summary>
        public string Handler { get; set; }
        /// <summary>
        /// 处理者中文名称
        /// </summary>
        public string HandlerName { get; set; }
        /// <summary>
        /// 子分类，模式：登录？注册？主页？
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 模式名称
        /// </summary>
        public string ModeText { get; set; }
        /// <summary>
        /// 内容模型
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 内容模型中文名称
        /// </summary>
        public string ModelText { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 中文描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 信息是否足够
        /// </summary>
        public bool Enough
        {
            get
            {
                return !string.IsNullOrEmpty(Handler) && !string.IsNullOrEmpty(Mode);
            }
        }
    }

    public enum TemplateType : int
    {
        /// <summary>
        /// 普通模板
        /// </summary>
        Common = 1,
        /// <summary>
        /// 子模板
        /// </summary>
        Sub= 0,
        /// <summary>
        /// 母版
        /// </summary>
        MasterPage = 9,
        /// <summary>
        /// 已指定为默认模板
        /// </summary>
        HaveBinded = -1,
        /// <summary>
        /// 全部
        /// </summary>
        All= 100
    }
}
