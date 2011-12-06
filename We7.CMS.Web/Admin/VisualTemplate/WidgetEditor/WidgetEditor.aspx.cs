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
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.CMS.Config;
using We7.Framework.Config;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.VisualTemplate
{
    public partial class WidgetEditor : BasePage
    {
        string controlfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        string ControlFile
        {
            get
            {
                if (String.IsNullOrEmpty(controlfile))
                {
                    controlfile = Request["ctr"];
                    if (String.IsNullOrEmpty(controlfile))
                        throw new Exception("控件参数不能为空");
                }
                return controlfile;
            }
        }


        bool IsEdit
        {
            get
            {
                if (ViewState["WE$IsEditor"] == null)
                {
                    string cmd = Request["cmd"];
                    return String.IsNullOrEmpty(cmd) ? false : cmd.ToLower() == "edit";
                }
                return (bool)ViewState["WE$IsEditor"];
            }
            set
            {
                ViewState["WE$IsEditor"] = value;
            }
        }
        string Group
        {
            get
            {
                string tmpfolder = String.Empty;
                if (!String.IsNullOrEmpty(Request["gp"]))
                    tmpfolder = Request["gp"];
                else
                {
                    tmpfolder = CDHelper.Config.DefaultTemplateGroupFileName;
                    tmpfolder = tmpfolder.Remove(tmpfolder.IndexOf("."));
                }
                return tmpfolder.TrimStart('~');
            }
        }

        string GroupCopy
        {
            get
            {
                GeneralConfigInfo config = GeneralConfigs.GetConfig();
                if (config.SiteBuildState == "run")
                {
                    return String.Format("~{0}", Group);
                }
                else
                {
                    return Group;
                }
            }
        }

        /// <summary>
        /// 增加来源判断
        /// 2011-11-5,
        /// author:Brian.G
        /// 
        /// </summary>
        string Source
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["source"]))
                    return Request["source"].ToString();
                else
                    return "";
            }
        }

        void BindData()
        {
            BindContent();

            //增加来源判断，如果来自内容模型的编辑，则不显示另存为
            switch (Source.ToLower())
            {
                case "contentmodel":
                    StoreButton.Visible = false;
                    break;
            }
        }

        void BindContent()
        {
            CtrCodeTextBox.Text = LoadWidget(ControlFile);
        }

        protected void OriControlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindContent();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            EditWidget(ControlFile, CtrCodeTextBox.Text.Trim());
            string returnPath = Request["ctr"];
            returnPath = returnPath.Replace("\\", "~");
            switch (Source.ToLower())
            {
                case "contentmodel":
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "<script>window.parent.CloseChild('已保存')</script>");
                    break;
                default:
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "<script>Close('" + returnPath + "');</script>");
                    break;
            }

          
        }

        /// <summary>
        /// 另存为新部件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StoreButton_Click(object sender, EventArgs e)
        {
            string widgetName = hdFileName.Value;
            if (!string.IsNullOrEmpty(widgetName))
            {
                //获取原部件所在目录
                string fullpath = Server.MapPath(Request["ctr"]);
                FileInfo fi = new FileInfo(fullpath);
                if (fi.Exists)
                {
                    string file = widgetName;
                    if (!widgetName.ToLower().EndsWith(".ascx"))
                        widgetName += ".ascx";
                    string filePhysical = Path.Combine(fi.Directory.Parent.FullName, file + "\\" + widgetName);
                    MakeFileCode(fi, file, filePhysical);
                }
            }
            Response.Write("<script>closeBg();</script>");
        }
        /// <summary>
        /// 操作文件&&字符替换
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="file"></param>
        /// <param name="filePhysical"></param>
        private void MakeFileCode(FileInfo fi, string file, string filePhysical)
        {
            if (File.Exists(filePhysical))
            {
                // ClientScript.RegisterClientScriptBlock(this.GetType(), "errors", "<script>alert('已存在相同的文件，请更换文件名');</script>", true);
                Response.Write("<script>alert('已存在相同的文件，请更换文件名');</script>");
            }
            else if (CtrCodeTextBox.Text.IndexOf("CodeFile=") == -1)  //内容模型
            {
                try
                {
                    FileHelper.WriteFile(filePhysical, CtrCodeTextBox.Text.Trim(), FileMode.Create);
                    Macksuccess(fi.Directory.FullName, Path.Combine(fi.Directory.Parent.FullName, file + "\\" + file + ".ascx"));
                }
                catch (Exception)
                {
                    ErrorMake(fi.Directory.FullName);
                }

            }
            else    // 非静态部件
            {
                string temp = CtrCodeTextBox.Text.Trim();
                string CodeFile = Regex.Match(temp, "CodeFile=\"[\\s|\\S]*?\"").Value;
                string CodeFileName = Regex.Match(CodeFile, "CodeFile=\"(?<value>[\\s|\\S]*?)\"").Groups["value"].Value;
                string Inherits = Regex.Match(temp, "Inherits=\"We7.CMS.Web.Widgets.(?<value>[\\s|\\S]*?)\"").Groups["value"].Value;
                string InheritsName = file.IndexOf('.') != -1 ? file.Replace('.', '_') : file;
                string css = Regex.Match(temp, "<link href=\"[\\s|\\S]*?\"").Value;
                string cssName = Regex.Match(css, "<link href=\"Style/(?<value>[\\s|\\S]*?)\"").Groups["value"].Value;
                //css Old路径
                string cssOldPath = fi.Directory.FullName + "\\Style\\" + fi.Directory.Name + ".css";
                //css New路径
                string cssPath = fi.Directory.Parent.FullName + "\\" + file + "\\Style\\" + file + ".css";

                if (!string.IsNullOrEmpty(CodeFile) && !string.IsNullOrEmpty(Inherits))
                {
                    string WidgetValue = string.Empty;
                    if (!string.IsNullOrEmpty(css))
                    {
                        WidgetValue = temp.Replace(CodeFile, "CodeFile=\"" + file + ".cs\"").Replace(css, "<link href=\"Style/" + file + ".css\"");  //替换ascx文件内的 link css && CodeFile && Inherits  

                    }
                    else
                        WidgetValue = temp.Replace(CodeFile, "CodeFile=\"" + file + ".cs\"");
                    WidgetValue = WidgetValue.Replace(Inherits, InheritsName);
                    if (FileHelper.CopyDirectory(fi.Directory.FullName + "\\Images", fi.Directory.Parent.FullName + "\\" + file + "\\Images", true))    //复制文件夹images
                    {

                        try
                        {
                            #region 写css文件
                            if (FileHelper.Exists(cssOldPath))
                            {
                                string cssCode = FileHelper.ReadFileWithLine(cssOldPath, Encoding.Default);
                                string cssControlName = fi.Directory.Name;  //Replace control
                                string cssStyleOld = cssControlName;  //style
                                string cssStyleNew = file;  //style
                                cssCode = cssCode.Replace("Control:" + cssControlName, "Control:" + file);
                                if (cssControlName.IndexOf('.') != -1)  //如果带"."，则替换为下划线
                                {
                                    cssStyleOld = cssControlName.Replace('.', '_');
                                    if (file.IndexOf('.') != -1)
                                    {
                                        cssStyleNew = file.Replace('.', '_');
                                    }
                                    cssCode = cssCode.Replace("Style:" + cssStyleOld, "Style:" + cssStyleNew);  //Replace style
                                }
                                else
                                {
                                    cssCode = cssCode.Replace("Style:" + cssStyleOld, "Style:" + cssStyleNew); //Replace style

                                }
                                cssCode = cssCode.Replace("." + cssStyleOld, "." + cssStyleNew);
                                FileHelper.WriteFile(cssPath, cssCode, FileMode.Create);  //写入csc文件
                            }
                            #endregion

                            #region 写ascx文件
                            FileHelper.WriteFile(filePhysical, WidgetValue); //写ascx文件 
                            #endregion

                            #region 写cs文件
                            if (FileHelper.Exists(fi.Directory + "\\" + CodeFileName))
                            {
                                string csCode = FileHelper.ReadFileWithLine(fi.Directory + "\\" + CodeFileName, Encoding.Default);  //读取cs文件
                                string csClass = Regex.Match(csCode, "public partial class [\\s|\\S]*?:").Value;
                                string defaultType = Regex.Match(csCode, "DefaultType = \"[\\s|\\S]*?\"").Value;
                                string csResult = string.Empty;
                                if (!string.IsNullOrEmpty(defaultType))
                                {
                                    csResult = csCode.Replace(defaultType, "DefaultType = \"" + file + "\"");//.Replace(csClassName, file);  //替换class name && defaultType


                                }
                                csResult = csResult.Replace(Inherits, InheritsName);
                                FileHelper.WriteFile(fi.Directory.Parent.FullName + "\\" + file + "\\" + file + ".cs", csResult); //写cs文件 

                            }
                            #endregion
                            Macksuccess(fi.Directory.FullName, Request["ctr"].Replace(fi.Directory.Name, file));
                        }
                        catch (Exception)
                        {
                            ErrorMake(fi.Directory.FullName);
                        }
                    }
                    else
                    {
                        ErrorMake(fi.Directory.FullName);
                    }
                }
                else
                {
                    Response.Write("<script>alert('顶部代码错误，请修正代码！');</script>");
                }
            }
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        private void Macksuccess(string Floderpath, string returnPath)
        {
            try
            {
                We7.CMS.WebControls.Core.BaseControlHelper Helper = new We7.CMS.WebControls.Core.BaseControlHelper();
                Helper.CreateWidegetsIndex();
                returnPath = returnPath.Replace("\\", "~");
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "<script>Close('" + returnPath + "');</script>");
            }
            catch (Exception ex)
            {
                //log exMessage
                ErrorMake(Floderpath);
            }

        }
        /// <summary>
        /// 文件操作失败
        /// </summary>
        /// <param name="Floderpath">文件夹目录（删除）</param>
        private void ErrorMake(string Floderpath)
        {
            Response.Write("<script>alert('未知原因导致另存失败，请联系管理员');</script>");
            if (Directory.Exists(Floderpath))
            {
                Directory.Delete(Floderpath, true);
            }
        }

        #region 控件操作
        protected string LoadWidget(string file)
        {
            string result = string.Empty;
            string fullpath = Server.MapPath(file);
            FileInfo fi = new FileInfo(fullpath);
            if (fi.Exists)
            {
                using (StreamReader sr = new StreamReader(fullpath, Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
            }

            return result;
        }

        protected bool EditWidget(string file, string code)
        {
            try
            {
                string fullpath = Server.MapPath(file);
                FileInfo fi = new FileInfo(fullpath);
                if (fi.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(fullpath, false, Encoding.UTF8))
                    {
                        sw.Write(code);
                    }
                }
                

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}