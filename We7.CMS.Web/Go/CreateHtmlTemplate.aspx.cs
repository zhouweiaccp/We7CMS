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
using We7.CMS.Config;
using We7.Framework.Config;
using System.Text;

namespace We7.CMS.Web
{
    public partial class CreateHtmlTemplate : FrontBasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //AddStatistic(string.Empty, string.Empty);
        }

        public string TemplatePath;

        protected bool IsHtmlTemplate
        {
            get
            {
                return GeneralConfigs.GetConfig().EnableHtmlTemplate && String.IsNullOrEmpty(Request["CreateHtml"]);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            //初始化TemplatePath
            string result = IsHtmlTemplate ?
                TemplateHelper.GetHtmlTemplateByHandlers(ColumnMode, "/", null, null)
                :TemplateHelper.GetTemplateByHandlers(ColumnMode, "/", null, null);
            if (!string.IsNullOrEmpty(result))
            {
                if (!result.StartsWith("/"))
                {
                    TemplatePath = "/" + result;
                }
                else
                {
                    TemplatePath = result;
                }
            }
            if (!string.IsNullOrEmpty(TemplatePath))
            {
                if (File.Exists(Context.Server.MapPath(TemplatePath)))
                {
                    Control ctl = LoadControl(TemplatePath);
                    if (ctl != null)
                    {
                        this.Controls.Add(ctl);
                    }
                    else
                        throw new Exception("无法加载模板文件" + TemplatePath + "！请检查模板是否正确。");
                }
                else
                    throw new Exception("没有找到模板" + TemplatePath + "！请确认模板路径是否正确。");
            }
        }

        /// <summary>
        /// 重写Render方法
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            StringWriter strWriter = new StringWriter();
            HtmlTextWriter tempWriter = new HtmlTextWriter(strWriter);
            try
            {
                base.Render(tempWriter);
            }
            catch (Exception ex)
            {
                strWriter.Write("");
            };
            string content = strWriter.ToString();
            writer.Write(content);
            WriteFile(TemplatePath, content);
        }


        #region 创建文件
        /****************************************
          * 函数名称：CreateFile
          * 功能说明：创建文件
          * 参     数：Path:文件路径
          * 调用示列：
          *            string Path = Server.MapPath("Default2.aspx");       
          *            EC.FileObj.WriteFile(Path,Strings);
         *****************************************/
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Strings">文件内容</param>
        public  void CreateFile(string Path)
        {
            if (!System.IO.File.Exists(Path))
            {
                System.IO.FileStream f = System.IO.File.Create(Path);
                f.Close();
            }
        }
        #endregion


        #region 写文件
        /****************************************
          * 函数名称：WriteFile
          * 功能说明：写文件,会覆盖掉以前的内容
          * 参     数：Path:文件路径,Strings:文本内容
          * 调用示列：
          *            string Path = Server.MapPath("Default2.aspx");       
          *            string Strings = "这是我写的内容啊";
          *            EC.FileObj.WriteFile(Path,Strings);
         *****************************************/
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Strings">文件内容</param>
        public  void WriteFile(string Path, string Strings)
        {
            string targetDir = Path.Substring(0, (Path.LastIndexOf(@"/") + 1));
            CreateDirectory(targetDir);
            Path = Server.MapPath(Path);
            using (StreamWriter sw = new StreamWriter(Path, false, Encoding.GetEncoding("GB2312")))
            {
                sw.Write(Strings);
                sw.Close();
            }
        }
        #endregion


        /// <summary>
        /// 创建指定目录
        /// </summary>
        /// <param name="targetDir"></param>
        public  void CreateDirectory(string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDir);
            if (!dir.Exists)
                dir.Create();
        }

    }
}
