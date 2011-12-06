using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Xml;

using We7.CMS.Config;
using System.Web.Configuration;
using We7.Framework.Config;
using We7.Framework;
using We7.Framework.Zip;
using System.Collections.Generic;
using We7.Framework.Util;

namespace We7.CMS.Install
{
    public class upgrade : SetupPage
    {
        protected FileUpload UpdateFileUpload;
        protected Label UploadSummary;
        protected Button CopyFilesButton;
        protected Button UploadButton;
        protected Literal UnZipLiteral;
        protected Panel NewversionPanel;
        protected Label NewVersionLabel;
        protected Button DownloadInstallButton;
        protected HyperLink DownloadLocalHyperLink;
        protected CheckBox BackUpCheckBox;
        protected CheckBox ClearOldCheckBox;
        protected Panel BackUpPanel;
        protected System.Web.UI.HtmlControls.HtmlGenericControl ShowErrorLabel;
        protected System.Web.UI.HtmlControls.HtmlInputButton FinishButton;

        protected string UnZipPath
        {
            get
            {
                return (string)ViewState["We7$UnZipPath"];
            }
            set
            {
                ViewState["We7$UnZipPath"] = value;
            }
        }

        protected string NewVersion
        {
            get
            {
                return (string)ViewState["We7$NewVersion"];
            }
            set
            {
                ViewState["We7$NewVersion"] = value;
            }
        }

        protected string UploadFile
        {
            get
            {
                return (string)ViewState["We7$UploadFile"];
            }
            set
            {
                ViewState["We7$UploadFile"] = value;
            }
        }



        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Init();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DisableSubmitBotton(this, this.CopyFilesButton);
            if (!IsPostBack)
            {
                if (IsAdminLogin())
                {
                    CheckWebConfig();
                    //CheckNewVersion();
                    CopyFilesButton.Enabled = false;
                }
                else
                {
                    Response.Redirect("signin.aspx?returnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl));
                }
            }
        }

        bool IsAdminLogin()
        {
            return (HttpContext.Current.Request.IsAuthenticated && HttpContext.Current.User.Identity.Name == "administrator");
        }

        void CheckWebConfig()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            object o = config.GetSection("system.web/httpRuntime");
            HttpRuntimeSection section = o as HttpRuntimeSection;
            int max = section.MaxRequestLength;
            if (max < 10 * 1024)
            {
                section.MaxRequestLength = 40 * 1024;
                section.AppRequestQueueLimit = 5000;
                section.ExecutionTimeout = new TimeSpan(0, 0, 3000);

                config.Save();

                ShowErrorLabel.InnerText = "原网站环境不支持大文件上传，现已更新为" + section.MaxRequestLength + "KB，请重新启动IIS!";
                ShowErrorLabel.Style.Remove("display");

                UploadButton.Enabled = false;
                DownloadInstallButton.Enabled = false;
                UpdateFileUpload.Enabled = false;
                CopyFilesButton.Enabled = false;
                //FinishButton.Disabled = true;
            }
        }

        void CheckNewVersion()
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            string url = "http://www.westengine.com/go/latestCore.aspx";
            string latestVersion = Installer.GetRemoteWebString(url, 8000, 0, Encoding.UTF8);
            string oldVersion = "";
            if (si != null && si.ProductVersion != null)
                oldVersion = si.ProductVersion;
            if (latestVersion != "" && Installer.VersionLater(oldVersion, latestVersion))
            {
                DownloadLocalHyperLink.NavigateUrl = "http://www.westengine.com/_data/We7.CMS-" + latestVersion + ".zip";
                DownloadLocalHyperLink.Target = "_blank";
                DownloadLocalHyperLink.Text = "下载" + latestVersion;
                NewversionPanel.Visible = true;
                NewVersionLabel.Text = string.Format("有一个新的 {0} CMS 版本可供升级", ProductBrand) + latestVersion;
                NewVersion = latestVersion;
            }
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DownloadInstallButton.Click += new EventHandler(this.DownloadInstallButton_Click);
            this.CopyFilesButton.Click += new EventHandler(this.CopyFilesButton_Click);
            this.UploadButton.Click += new EventHandler(this.UploadButton_Click);
            this.Load += new EventHandler(this.Page_Load);
        }
        #endregion

        private void DownloadInstallButton_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("/_temp");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            Installer.DownloadFileFromUrl(DownloadLocalHyperLink.NavigateUrl, folderPath);
            string filename = DownloadLocalHyperLink.NavigateUrl.Substring(DownloadLocalHyperLink.NavigateUrl.LastIndexOf('/') + 1);
            UploadFile = Path.Combine(folderPath, filename);
            UnZipPath = UnZipFile(Path.Combine(folderPath, filename));
            if (UnZipPath == "")
            {
                RegisterScript("alert('无法正确解压，请检查压缩文件是否为合法的Zip压缩格式。');");
                return;
            }
            else
                UnZipLiteral.Text = FileListSummary(UnZipPath);
        }

        private void RegisterScript(string script)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script, true);
        }


        private void CopyFilesButton_Click(object sender, EventArgs e)
        {
            //configuration文件夹改名 兼容2.7以前版本方法
            string oldConfigPath = Path.Combine(Server.MapPath("~/"), "configuration");
            if (Directory.Exists(oldConfigPath))
            {
                string destDirName = Path.Combine(Server.MapPath("~/"), "config");
                if (Directory.Exists(destDirName))
                {
                    Directory.Delete(destDirName, true);
                }
                Directory.Move(oldConfigPath, Path.Combine(Server.MapPath("~/"), "config"));
            }

            if (BackUpPanel.Visible)
            {
                if (BackUpCheckBox.Checked)
                    Installer.BackupOldFiles(Server.MapPath("~/"), Server.MapPath("~/_backup/update/"));

                //检查是否有冗余文件
                if (ClearOldCheckBox.Visible && ClearOldCheckBox.Checked)
                {
                    //此处应该返回错误信息，目前存储在LOG文件中
                    DeleteFiles();
                }
            }

            string ext = Path.GetExtension(UploadFile);
            switch (ext.ToLower())
            {
                case ".zip":
                    try
                    {
                        //DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath("~/bin"));
                        //Helper.DeleteFileTree(directoryInfo, false);
                        We7Helper.CopyDirectory(UnZipPath, Server.MapPath("~/"));

                        Directory.Delete(UnZipPath, true);
                        //
                        /*
                         * 
                         * 保存新版本号
                        GeneralConfigInfo si = GeneralConfigs.GetConfig();
                        if (si != null)
                        {
                            //si.ProductVersion = NewVersion;
                            GeneralConfigs.SaveConfig(si);
                        }
                         */
                        RegisterScript("alert('操作成功!');location.href='upgrade-db.aspx?from=upgrade.aspx'");
                    }
                    catch (IOException ex)
                    {
                        RegisterScript("alert('文件复制失败。原因：" + ex.Message + "');");
                    }
                    break;

                case ".dll":
                    try
                    {
                        string targetfile = Path.Combine(Server.MapPath("~/bin/"), Path.GetFileName(UploadFile));
                        File.Copy(UploadFile, targetfile, true);
                        RegisterScript("alert('文件更新成功！');");
                    }
                    catch (IOException ex)
                    {
                        RegisterScript("alert('文件复制失败。原因：" + ex.Message + "');");
                    }
                    break;

                case ".xml":
                    try
                    {
                        //读取默认db.config文件内容
                        BaseConfigInfo bci = BaseConfigs.GetBaseConfig();
                        if (bci != null && bci.DBType != "" && bci.DBConnectionString != "")
                        {
                            Installer.ExcuteSQL(bci, UploadFile);
                        }
                        RegisterScript("alert('XML文件执行成功！');");
                    }
                    catch (IOException ex)
                    {
                        RegisterScript("alert(''XML文件执行出现错误。原因：" + ex.Message + "');");
                    }
                    break;
            }
        }

        private string DeleteFiles()
        {
            string result = "";

            string fileStr = We7Request.GetString("delFiles");
            if (fileStr.Length > 0)
            {
                string[] filePathList = Utils.SplitString(fileStr, ",");
                int totalCount = 0;
                int errorCount = 0;
                foreach (string filePath in filePathList)
                {
                    bool exist = false;
                    bool isFile = false;
                    string physicalPath = "";
                    if (filePath.Length > 0)
                    {
                        totalCount++;
                        try
                        {
                            physicalPath = Server.MapPath(filePath);
                            isFile = (Path.GetExtension(physicalPath).Length > 0 ? true : false);
                            if (isFile)
                                exist = File.Exists(physicalPath);
                            else
                                exist = Directory.Exists(physicalPath);
                        }
                        catch
                        {

                        }
                        if (exist)
                        {
                            try
                            {
                                if (isFile)
                                {
                                    File.Delete(physicalPath);
                                }
                                else
                                {
                                    Directory.Delete(physicalPath, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                result += "<br/>“" + physicalPath + "” 删除失败，原因：" + ex.Message;


                                errorCount++;
                            }
                        }
                    }
                }
                result = "升级文件异常：总文件数：" + totalCount + ",其中删除失败：" + errorCount + ",详细信息：" + result;
                if (errorCount > 0)
                    We7.Framework.LogHelper.WriteLog(typeof(upgrade), result);
            }

            return result;
        }

        /// <summary>
        /// 上传更新包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_Click(object sender, EventArgs e)
        {
            if (UpdateFileUpload.FileName == null || UpdateFileUpload.FileName == "")
            {
                RegisterScript("alert('您还没有选择要上传的文件！');");
                return;
            }

            string ap = GenerateFileName(Path.GetFileName(UpdateFileUpload.FileName));
            UploadFile = ap;
            string ext = Path.GetExtension(ap).ToLower();
            if (ext != ".zip" && ext != ".dll" && ext != ".xml")
            {
                RegisterScript("alert('更新包格式不能识别，请检查！');");
                return;
            }

            BackUpPanel.Visible = false;
            try
            {
                UpdateFileUpload.SaveAs(ap);

                switch (ext.ToLower())
                {
                    case ".zip":
                        //解压缩文件到Temp文件下
                        UnZipPath = UnZipFile(ap);
                        if (UnZipPath == "")
                        {
                            RegisterScript("alert('无法正确解压，请检查压缩文件是否为合法的Zip压缩格式。');");
                            return;
                        }
                        else
                        {
                            UnZipLiteral.Text = FileListSummary(UnZipPath);


                            try
                            {
                                //压缩包已解压 删除压缩包
                                File.Delete(ap);
                            }
                            catch
                            {
                                We7.Framework.LogHelper.WriteLog(typeof(upgrade), "临时文件：" + ap+"删除失败，请手动删除");
                            }

                            //检查是否有需要删除的文件
                            string pathJson = GetOperatorPath(UnZipPath);
                            ClientScript.RegisterStartupScript(this.GetType(), "", "$('#files').val('" + pathJson + "')", true);
                        }
                        BackUpPanel.Visible = true;

                        break;

                    case ".dll":
                        UnZipLiteral.Text = Path.GetFileName(ap) + "将会直接覆盖到 bin 目录下，如果您确认要这样操作，请您点击“开始更新”执行更新！";
                        break;

                    case ".xml":
                        UnZipLiteral.Text = Path.GetFileName(ap) + "为数据库结构升级文件，系统将会直接更新当前数据库，如果您确认要这样操作，请您点击“开始更新”执行更新！";
                        break;

                    default:
                        RegisterScript("alert('更新包格式不能识别，请检查！');");
                        break;
                }

            }
            catch (IOException ex)
            {
                RegisterScript("alert('更新包上传失败。原因：" + ex.Message + "');");
                return;
            }

            CopyFilesButton.Enabled = true;
        }

        string GetOperatorPath(string path)
        {
            string strScript = "";
            path += @"\install\Files\Delete.xml";
            if (File.Exists(path))
            {
                StringBuilder sb = new StringBuilder();
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList nodeList = doc.SelectNodes("/Delete/File");
                  

                    if (nodeList != null && nodeList.Count > 0)
                    {
                        sb.Append("{\"Exist\":true,\"Files\":[");
                        foreach (XmlNode node in nodeList)
                        {
                            sb.Append("{\"Path\":\"" + node.InnerText + "\",\"Type\":\"" + node.Attributes["Type"].Value + "\"},");
                        }
                        sb.Append("]");
                        sb.Append("}");
                        strScript = sb.ToString();
                        strScript = We7.Framework.Util.Utils.JsonCharFilter(strScript.Remove(strScript.LastIndexOf(","), 1));

                        return strScript;
                    }
                }
                catch
                {
                   
                }
            }

            return "";
        }

        string GenerateFileName(string fileName)
        {
            string folderPath = Server.MapPath("/_temp");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fn = Path.Combine(folderPath, fileName);
            if (File.Exists(fn))
            {
                File.Delete(fn);
            }
            return fn;
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        string UnZipFile(string file)
        {
            string path = "";
            if (String.Compare(Path.GetExtension(file), ".zip", true) == 0)
            {
                path = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                if (Directory.Exists(path))
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    We7Helper.DeleteFileTree(dir);
                }

                FileStream s = File.OpenRead(file);
                ZipUtils.ExtractZip((Stream)s, path);
            }
            return path;
        }

        /// <summary>
        /// 压缩包列表
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        string FileListSummary(string dir)
        {

            StringBuilder sb = new StringBuilder();


            DirectoryInfo di = new DirectoryInfo(dir);
            DirectoryInfo[] ds = di.GetDirectories();
            FileInfo[] files = di.GetFiles();

            sb.Append("<p style='font-weight:bold'>更新包包含文件如下,点击查看更多显示完整列表，点击 更新文件 进行更新覆盖</p>");
            sb.Append("<table cellSpacing='0' cellPadding='0' width='90%' border='0' bgcolor='#f8f8f8' style='font-size:12px;'>");

            sb.Append(string.Format("<tr ><td width=20><img src='images/folder.gif' /></td><td>{0}</td> <td>{1}</td></tr>", "<a id='showMore' show='t' href='javascript:void(0)' onclick=\"ShowMoreFiles('.moreFiles',this)\" >查看详细</a>", ""));
            foreach (DirectoryInfo d in ds)
            {
                string rowFolder = "<tr class='moreFiles' style='display:none;'><td width=20><img src='images/folder.gif' /></td><td>{0}</td> <td>{1}</td></tr>";
                sb.Append(string.Format(rowFolder, d.Name, d.LastWriteTime));
            }

            foreach (FileInfo f in files)
            {
                string rowFile = "<tr class='moreFiles' style='display:none;'><td width=20><img src='images/file.gif' /></td><td>{0}</td> <td>{1}</td></tr>";
                sb.Append(string.Format(rowFile, f.Name, f.LastWriteTime));
            }
            sb.Append("</table>");
            return sb.ToString();
        }
    }
}
