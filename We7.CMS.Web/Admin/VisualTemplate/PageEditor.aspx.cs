using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using We7.Framework.TemplateEnginer;
using HtmlAgilityPack;
using We7.Framework.Util;
using System.Collections.Generic;
using System.Xml;
using We7.CMS.Common.Enum;
using We7.Framework.Config;
using VisualDesign.Module;

namespace We7.CMS.Web.Admin.VisualTemplate
{
    public partial class PageEditor : BasePage
    {
        #region 属性字段
        protected override We7.CMS.Common.Enum.MasterPageMode MasterPageIs
        {
            get
            {
                return We7.CMS.Common.Enum.MasterPageMode.None;
            }
        }


        public virtual string FileName
        {
            get
            {
                var file = Context.Request["file"];
                return file;
            }
        }

        public virtual string Group
        {
            get
            {
                var folder = Context.Request["folder"];
                return folder;
            }
        }

        public string PageVirualPath
        {
            get
            {
                return string.Format("~/_skins/{0}/{1}", Group, FileName);
            }
        }

        public string DesignPageVirualPath
        {
            get
            {
                return string.Format("~/_skins/~{0}/{1}", Group, FileName);
            }
        }

        private readonly string resorcePath = "~/Admin/VisualTemplate/Config/resource.xml";
        private readonly string resorceCacheKey = "__RESORCEKEY__";
        /// <summary>
        /// 注册部件匹配正则
        /// </summary>
        private readonly static string registerPattern =
            "<%@\\s+Register\\s+Src=\"(?<src>[^\"]+)\"\\s+TagName=\"(?<tag>[^\"]+)\"\\s+TagPrefix=\"(?<prefix>[^\"]+)\"\\s+%>";
        /// <summary>
        /// 注册部件的样式表匹配正则
        /// </summary>
        private readonly static string stylePattern =
            "<link\\s+href=\"{0}\"[^>]+>";
        /// <summary>
        /// 注册部件引用的匹配正则
        /// </summary>
        private readonly static string controlPattern =
            "<{0}:{1}[^>]+></{0}:{1}>";
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="e"></param>
        #region override void OnInit(EventArgs e)
        protected override void OnInit(EventArgs e)
        {
            //检查文件是否存在
            CheckDesignFile();
            
            //加载页面
            try
            {
                //检查设计时模板引用路径是否正确，不正确进行修正
                string filePath =  Server.MapPath(string.Format("~/_skins/~{0}/{1}",Group,FileName));
                //检查文件引用是否正确
                CheckDesignPage(filePath);

                //载入设计时文件
                Control template = this.Page.LoadControl(DesignPageVirualPath);
                this.Page.Controls.Add(template);
                RegisterScript();
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(PageEditor),ex);
            }

            base.OnInit(e);
        }
        #endregion
        
        /// <summary>
        /// 渲染
        /// </summary>
        /// <param name="writer"></param>
        #region override void Render(HtmlTextWriter writer)
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            #region 注释掉的
            ////可视化设计时
            //StringWriter output = new StringWriter();
            //HtmlTextWriter tw = new HtmlTextWriter(output);
            //base.Render(tw);

            //string html = output.ToString();

            //HtmlDocument doc = new HtmlDocument();
            //doc.OptionAutoCloseOnEnd = true;
            //doc.OptionCheckSyntax = true;
            //doc.OptionOutputOriginalCase = true;
            //try
            //{
            //    doc.LoadHtml(html);
            //}
            //catch
            //{
            //    throw new Exception("格式化HTML错误");
            //}

            //var head = doc.DocumentNode.SelectSingleNode("//head").InnerHtml;
            //var body = doc.DocumentNode.SelectSingleNode("//body").InnerHtml;

            //NVelocityHelper helper = new NVelocityHelper(We7.CMS.Constants.VisualTemplatePhysicalTemplateDirectory);

            //helper.Put("head", head);
            //helper.Put("body", body);

            //var rendHtml = helper.Save("EditorPage.vm");

            ////格式化
            //rendHtml = FormatHtml(rendHtml);
            ////输出代码
            //writer.Write(rendHtml);
            #endregion
        }
        #endregion

        /// <summary>
        /// 检查设计页面里的引用路径是否正确
        /// </summary>
        /// <param name="DesignFilePath"></param>
        #region void CheckDesignPage(string DesignFilePath)
        protected void CheckDesignPage(string DesignFilePath)
        {
            string htmlContentOriginal = FileHelper.ReadFileWithLine(
                DesignFilePath, Encoding.Default); 
            string htmlContent = null;

            bool hasError = VerifyPageHtml(htmlContentOriginal, out htmlContent);

            //有错误发生，备份原有模板
            if (hasError)
            {
                //备份文件为模板名称加 .bak
                string bakFileName = DesignFilePath + ".bak";
                if (File.Exists(bakFileName))
                    File.Delete(bakFileName);
                FileHelper.WriteFileEx(bakFileName, htmlContentOriginal, false);

                //保存新的模板内容
                FileHelper.WriteFileEx(DesignFilePath, htmlContent, false);
            }            
        }
        #endregion

        /// <summary>
        /// 校验模板中的html描述路径，是否存在错误
        /// </summary>
        /// <param name="html">原html</param>
        /// <param name="newHtml">如有错误引用，修正后的html</param>
        /// <returns>true:有错误发生; false:没有错误发生
        /// </returns>
        #region bool VerifyPageHtml(string html,out string newHtml)
        public bool VerifyPageHtml(string html,out string newHtml)
        {
            bool result = false;
            newHtml = html;

            //Regex regexRegister = new Regex(registerPattern, RegexOptions.IgnorePatternWhitespace 
            //    | RegexOptions.IgnoreCase);
            //regexRegister.Matches(newHtml);

            MatchCollection matchCollection = Regex.Matches(newHtml, registerPattern, 
                RegexOptions.IgnorePatternWhitespace| RegexOptions.IgnoreCase);
            if (matchCollection != null && matchCollection.Count > 0)
            {
                foreach (Match m in matchCollection)
                {
                    string src = m.Groups["src"].Value;
                    string tagPrefix = m.Groups["prefix"].Value;
                    string tagName = m.Groups["tag"].Value;
                    string cssFileUrl = string.Format(
                        stylePattern, src.Insert(src.LastIndexOf('/'), "/style").Replace(".ascx", ".css") );

                    //检查部件是否存在
                    string widgetPath = Server.MapPath(src);
                    if (!File.Exists(widgetPath))
                    {
                        result = true;
                        //剔除css引用
                        MatchCollection matchCss = Regex.Matches(newHtml, cssFileUrl,
                            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                        if (matchCss != null && matchCss.Count > 0)
                        {
                            foreach (Match mCss in matchCss)
                            {
                                newHtml = newHtml.Replace(mCss.Value, "");
                            }
                        }
                        //剔除掉部件引用
                        string ctrlPattern = string.Format(controlPattern,tagPrefix,tagName);
                        //Regex regexCtrl = new Regex(ctrlPattern, 
                        //    RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                        //regexCtrl.Matches(newHtml);
                        MatchCollection matchCtrls = Regex.Matches(newHtml, ctrlPattern,
                            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);                            
                        if (matchCtrls != null && matchCtrls.Count > 0)
                        {
                            foreach (Match mCtrl in matchCtrls)
                            {
                                newHtml = newHtml.Replace(mCtrl.Value, "");
                            }
                        }
                        //剔除掉部件注册
                        newHtml = newHtml.Replace(m.Value, "");
                    }
                }
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 格式化HTML内容
        /// </summary>
        /// <param name="html"></param>
        #region virtual string FormatHtml(string html)
        protected virtual string FormatHtml(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.OptionAutoCloseOnEnd = true;
            doc.OptionCheckSyntax = true;
            doc.OptionOutputOriginalCase = true;
            try
            {
                doc.LoadHtml(html);
            }
            catch
            {
                throw new Exception("格式化HTML错误");
            }
            StringWriter output = new StringWriter();
            doc.Save(output);

            return output.ToString();
        }
        #endregion

        /// <summary>
        /// 检查设计时文件是否存在
        /// </summary>
        #region void CheckDesignFile()
        private void CheckDesignFile()
        {
            //设计文件不存在
            var pagePath = Server.MapPath(PageVirualPath);
            var editPagePath = Server.MapPath(DesignPageVirualPath);
            if (!File.Exists(editPagePath))
            {
                //查看原始文件
                if (!File.Exists(pagePath))
                {
                    //跳转到页面:TODO
                }
                else
                {
                    //COPY
                    FileHelper.Copy(pagePath, editPagePath, true);
                }
            }
        }
        #endregion

        /// <summary>
        /// 注册脚本
        /// </summary>
        #region void RegisterScript()
        private void RegisterScript()
        {
            RegisterCommonScript();

            List<Resource> resources = new List<Resource>();
            if (HttpContext.Current.Cache[resorceCacheKey] != null)
            {
                resources = Cache[resorceCacheKey] as List<Resource>;
            }
            else
            {
                string path = Server.MapPath(resorcePath);
                XmlNode root = XmlHelper.GetXmlNode(path, "/resource");
                foreach (XmlNode node in root.ChildNodes)
                {
                    string name = node.Attributes["name"].Value;
                    string location = node.Attributes["src"].Value;
                    string type = node.Attributes["type"].Value;
                    if (string.IsNullOrEmpty(type))
                    {
                        type = "script";
                    }
                    Resource rc = new Resource(name, location, type);
                    resources.Add(rc);
                }

                Cache.Add(resorceCacheKey, resources, new System.Web.Caching.CacheDependency(path),
                    System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(1, 1, 1), System.Web.Caching.CacheItemPriority.Default, null);

            }
            foreach (var rc in resources)
            {
                if (rc.Type.ToLower().Trim() == "script")
                {
                    HtmlGenericControl script = new HtmlGenericControl("script");
                    script.Attributes["type"] = "text/javascript";
                    script.Attributes["src"] = rc.Location;
                    this.Header.Controls.Add(script);
                }
                else if (rc.Type.ToLower().Trim() == "style")
                {
                    HtmlGenericControl style = new HtmlGenericControl("link");
                    style.Attributes["type"] = "text/css";
                    style.Attributes["href"] = rc.Location;
                    style.Attributes["rel"] = "stylesheet";
                    this.Header.Controls.Add(style);
                }
            }
        }
        #endregion

        /// <summary>
        /// 加载公用脚本
        /// </summary>
        #region void RegisterCommonScript()
        private void RegisterCommonScript()
        {
            HtmlGenericControl script = new HtmlGenericControl("script");
            script.Attributes["type"] = "text/javascript";
            script.InnerText = "var Common={};Common.Theme='" + GeneralConfigs.GetConfig().Theme + "';";
            this.Header.Controls.Add(script);
        }
        #endregion
    }
}

/// <summary>
/// 脚本资源
/// </summary>
public class Resource
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Location { get; set; }

    public Resource()
    {
        Type = "script";
    }

    public Resource(string name, string location)
        : this()
    {
        Name = name;
        Location = location;
    }

    public Resource(string name, string location, string type)
    {
        Name = name;
        Location = location;
        Type = type;
    }
}
