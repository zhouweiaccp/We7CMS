using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

using We7.CMS.Config;
using We7.Framework.Config;


namespace We7.CMS.Install
{
    /// <summary>
    /// SetupPage 的摘要说明。
    /// </summary>
    public class SetupPage : System.Web.UI.Page
    {

        private static FileVersionInfo AssemblyFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

        public static string producename = "";  //当前产品版本名称

        public static string footer = "";

        public static string logo = "<img src=\"images/logo.jpg\" width=\"180\" height=\"300\">"; //安装的LOGO

        public static string header = ""; //html页的的<head>属性

        public static string SelectDB = "";

        public static void Init()
        {
            header = "<HEAD><title>安装 " + GetAssemblyProductName() + "</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n";
            header += "<LINK rev=\"stylesheet\" media=\"all\" href=\"css/general.css\" type=\"text/css\" rel=\"stylesheet\">\r\n";
            header += "<script language=\"javascript\" src=\"js/setup.js\"></script>\r\n";
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si != null)
            {
                string copyright = si.CopyrightOfWe7;
                if (si.IsOEM) copyright = si.Copyright;
                footer = string.Format("<div class='pubfooter'><p>{0}</p></div>", copyright);
            }
            else
            {
                footer = "<div class='pubfooter'><p>Powered by <a  href=\"http://we7.cn\" target=\"_blank\">" + GetAssemblyProductName() + "</a>";
                footer += " &nbsp; &copy;" + GetAssemblyCopyright().Split(',')[0] + " <a  href=\"http://www.westengine.com\" target=\"_blank\">WestEngine Inc.</a></p></div>";
            }

            producename = GetAssemblyProductName();
        }


        //当用户点击按钮时，将其置为无效
        public void DisableSubmitBotton(Page mypage, System.Web.UI.WebControls.Button submitbutton)
        {
            RegisterAdminPageClientScriptBlock();

            //保证 __doPostBack(eventTarget, eventArgument) 正确注册
            mypage.ClientScript.GetPostBackEventReference(submitbutton, "");

            StringBuilder sb = new StringBuilder();

            //保证验证函数的执行
            sb.Append("if (typeof(Page_ClientValidate) == 'function') { if (Page_ClientValidate() == false) { return false; }}");

            // disable所有submit按钮
            sb.Append("disableOtherSubmit();");

            //sb.Append("document.getElementById('Layer5').innerHTML ='正在运行操作</td></tr></table><BR />';");
            sb.Append("document.getElementById('success').style.display ='block';");

            sb.Append(this.ClientScript.GetPostBackEventReference(submitbutton, ""));
            sb.Append(";");
            submitbutton.Attributes.Add("onclick", sb.ToString());
        }


        public void RegisterAdminPageClientScriptBlock()
        {

            string script = "<div id=\"success\" style=\"position:absolute;z-index:300;height:120px;width:284px;left:50%;top:50%;margin-left:-150px;margin-top:-80px;\">\r\n" +
                          "	<div id=\"Layer2\" style=\"position:absolute;z-index:300;width:270px;height:90px;background-color: #FFFFFF;border:solid #000000 1px;font-size:14px;\">\r\n" +
                          "		<div id=\"Layer4\" style=\"height:26px;background:#333333;line-height:26px;padding:0px 3px 0px 3px;font-weight:bolder;color:#fff \">操作提示</div>\r\n" +
                          "		<div id=\"Layer5\" style=\"height:64px;line-height:150%;padding:0px 3px 0px 3px;\" align=\"center\"><br />正在执行操作,请稍等...</div>\r\n" +
                          "	</div>\r\n" +
                          "	<div id=\"Layer3\" style=\"position:absolute;width:270px;height:90px;z-index:299;left:4px;top:5px;background-color: #cccccc;\"></div>\r\n" +
                          "</div>\r\n" +
                          "<script> \r\n" +
                          "document.getElementById('success').style.display ='none'; \r\n" +
                          "</script> \r\n" ;


            base.ClientScript.RegisterClientScriptBlock(this.GetType(), "Page", script);
        }

        public new void RegisterStartupScript(string key, string scriptstr)
        {

            string message = "<BR />操作成功!";

            if (key == "PAGE")
            {
                string script = "";

                script = "<div id=\"success\" style=\"position:absolute;z-index:300;height:120px;width:284px;left:50%;top:50%;margin-left:-150px;margin-top:-80px;\">\r\n" +
                       "	<div id=\"Layer2\" style=\"position:absolute;z-index:300;width:270px;height:90px;background-color: #FFFFFF;border:solid #000000 1px;font-size:14px;\">\r\n" +
                       "		<div id=\"Layer4\" style=\"height:26px;background:#333;line-height:26px;padding:0px 3px 0px 3px;font-weight:bolder;color:#fff \">操作提示</div>\r\n" +
                       "		<div id=\"Layer5\" style=\"height:64px;line-height:150%;padding:0px 3px 0px 3px;\" align=\"center\">" + message + "</div>\r\n" +
                       "	</div>\r\n" +
                       "	<div id=\"Layer3\" style=\"position:absolute;width:270px;height:90px;z-index:299;left:4px;top:5px;background-color: #cccccc;\"></div>\r\n" +
                       "</div>\r\n" +
                       "<script> \r\n" +
                       "var bar=0;\r\n" +
                       "document.getElementById('success').style.display = \"block\"; \r\n" +
                       "count() ; \r\n" +
                       "function count(){ \r\n" +
                       "bar=bar+4; \r\n" +
                       "if (bar<99) \r\n" +
                       "{setTimeout(\"count()\",100);} \r\n" +
                       "else { \r\n" +
                       "	document.getElementById('success').style.display = \"none\"; \r\n" +
                       scriptstr + "} \r\n" +
                       "} \r\n" +
                       "</script> \r\n"+
                       "<script> window.onload = function(){HideOverSels('success')};</script>\r\n";

                Response.Write(script);
            }
            else
            {
                base.ClientScript.RegisterStartupScript(this.GetType(), key, scriptstr);
            }
        }

        public static string InitialSystemValidCheck(ref bool error)
        {
            error = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellSpacing='0' cellPadding='0' width='90%' align='center' border='0' bgcolor='#666666' style='font-size:12px'>");

            HttpContext context = HttpContext.Current;

            string filename = null;
            if (context != null)
                filename = context.Server.MapPath("/config/db.config");
            else
                filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/db.config");

            //系统BIN目录检查
            sb.Append(IISSystemBINCheck(ref error));


            //检查db.config文件的有效性
            if (!SystemConfigCheck())
            {
                sb.Append("<tr style=\"height:15px\"><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'> db.config 不可写或没有放置正确, 相关问题详见安装文档!</td></tr>");
                error = true;
            }
            else
            {
                sb.Append("<tr style=\"height:15px\"><td bgcolor='#ffffff' width='5%'><img src='images/ok.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%' style='color:#080'>对 db.config 验证通过!</td></tr>");
            }

            if (!SystemConfigCheck())
            {
                sb.Append("您对系统配置文件db.config没有写入权限!<br />");
            }

            //自动建立数据目录_data,_skins
            checkDataFilePath();
            //检查系统目录的有效性
            string folderstr = "admin,config,app_data,_data,_skins";
            foreach (string foldler in folderstr.Split(','))
            {
                if (!SystemFolderCheck(foldler))
                {
                    sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>对 " + foldler + " 目录没有写入和删除权限!</td></tr>");
                    error = true;
                }
                else
                {
                    sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/ok.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'  style='color:#080'>对 " + foldler + " 目录权限验证通过!</td></tr>");
                }
            }
            string filestr = "install\\systemfile.aspx";
            foreach (string file in filestr.Split(','))
            {
                if (!SystemFileCheck(file))
                {
                    sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>对 " + file.Substring(0,file.LastIndexOf('\\')) + " 目录没有写入和删除权限!</td></tr>");
                    error = true;
                }
                else
                {
                    sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/ok.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%' style='color:#080'>对 " + file.Substring(0, file.LastIndexOf('\\')) + " 目录权限验证通过!</td></tr>");
                }
            }

           if(!TempTest())
           {
               sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>您没有对 " + Path.GetTempPath() + " 文件夹访问权限，详情参见安装文档.</td></tr>");
               error = true;
           }
           else
           {
            if (!SerialiazeTest())
            {
                sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>对config文件反序列化失败，请保证config下的文件具有可写权限及格式正确。<br></td></tr>");
                error = true;
            }
            else
            {
                sb.Append("<tr><td bgcolor='#ffffff' width='5%'><img src='images/ok.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'  style='color:#080'>反序列化验证通过！</td></tr>");
            } 
           }
            sb.Append("</table>");
         
            return sb.ToString();
        }

        /// <summary>
        /// 检查数据文件夹，不存在则自动创建
        /// </summary>
        private static void checkDataFilePath()
        {
            HttpContext context = HttpContext.Current;
            string physicsPath = null;

            if (context != null)
                physicsPath = context.Server.MapPath("/");
            else
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;

            string mypath = Path.Combine(physicsPath, "_data");
            if (!Directory.Exists(mypath))
                Directory.CreateDirectory(mypath);

            mypath = Path.Combine(physicsPath, "_skins");
            if (!Directory.Exists(mypath))
                Directory.CreateDirectory(mypath);
        }

        public static bool SystemConfigCheck()
        {
            HttpContext context = HttpContext.Current;

            string physicsPath = null;

            if (context != null)
                physicsPath = context.Server.MapPath("/config");
            else
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                using (FileStream fs = new FileStream(physicsPath + "\\a.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }

                System.IO.File.Delete(physicsPath + "\\a.txt");

                return true;
            }
            catch
            {
                return false;
            }

        }

        #region 安装过程标记
        public static bool LockFileExist()
        {
            HttpContext context = HttpContext.Current;
            string physicsPath = null;
            if (context != null)
                physicsPath = context.Server.MapPath("/config");
            else
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;
            return File.Exists(physicsPath + "\\db-is-creating.lock");
        }

        public static void DeleteLockFile()
        {
            HttpContext context = HttpContext.Current;
            string physicsPath = null;
            if (context != null)
                physicsPath = context.Server.MapPath("/config");
            else
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;
            System.IO.File.Delete(physicsPath + "\\db-is-creating.lock");
        }

        public static void CreateLockFile()
        {
            HttpContext context = HttpContext.Current;

            string physicsPath = null;

            if (context != null)
                physicsPath = context.Server.MapPath("/config");
            else
                physicsPath = AppDomain.CurrentDomain.BaseDirectory;

            using (FileStream fs = new FileStream(physicsPath + "\\db-is-creating.lock", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fs.Close();
            }
        }


        #endregion

        public static string IISSystemBINCheck(ref bool error)
        {
            string binfolderpath = HttpRuntime.BinDirectory;

            string result = "";
            try
            {
                string[] assemblylist = new string[] { "We7.CMS.Common.dll", "We7.CMS.Config.dll", 
                    "We7.CMS.Utils.dll", "We7.CMS.Web.Admin.dll", "We7.CMS.Web.dll", "We7.Framework.dll"};
                bool isAssemblyInexistence = false;
                ArrayList inexistenceAssemblyList = new ArrayList();
                foreach (string assembly in assemblylist)
                {
                    if (!File.Exists(binfolderpath + assembly))
                    {
                        isAssemblyInexistence = true;
                        error = true;
                        inexistenceAssemblyList.Add(assembly);
                    }
                }
                if (isAssemblyInexistence)
                {
                    foreach (string assembly in inexistenceAssemblyList)
                    {
                            result += "<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>" + assembly + " 文件放置不正确<br/>请将所有的dll文件复制到目录 " + binfolderpath + " 中.</td></tr>";
                    }
                }
            }
            catch
            {
                result += "<tr><td bgcolor='#ffffff' width='5%'><img src='images/error.gif' width='16' height='16'></td><td bgcolor='#ffffff' width='95%'>请将所有的dll文件复制到目录 " + binfolderpath + " 中.</td></tr>";
                error = true;
            }

            return result;
        }

        public static bool SystemFolderCheck(string foldername)
        {

            string physicsPath = HttpContext.Current.Server.MapPath(@"..\" + foldername);
            try
            {
                using (FileStream fs = new FileStream(physicsPath + "\\a.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }
                if (File.Exists(physicsPath + "\\a.txt"))
                {
                    System.IO.File.Delete(physicsPath + "\\a.txt");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool SystemFileCheck(string filename)
        {
            filename = HttpContext.Current.Server.MapPath(@"..\" + filename);
            try
            {
                if (filename.IndexOf("systemfile.aspx") == -1 && !File.Exists(filename))
                    return false;
                if (filename.IndexOf("systemfile.aspx") != -1)  //做删除测试
                {
                    File.Delete(filename);
                    return true;
                }
                StreamReader sr = new StreamReader(filename);
                string content = sr.ReadToEnd();
                sr.Close();
                content += " ";
                StreamWriter sw = new StreamWriter(filename, false);
                sw.Write(content);
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool SerialiazeTest()
        {
            try
            {
                string configPath = HttpContext.Current.Server.MapPath("/config/general.config");
                GeneralConfigInfo __configinfo=new GeneralConfigInfo();
                if(!File.Exists(configPath))
                    GeneralConfigs.Serialiaze(__configinfo, configPath);
                __configinfo = GeneralConfigs.Deserialize(configPath);
                //__configinfo.IcpInfo ="";
                GeneralConfigs.Serialiaze(__configinfo, configPath);
                return true;
            }
            catch
            {
                return false;
            }
        }


        private static bool TempTest()
        {
            string UserGuid = Guid.NewGuid().ToString();
            string TempPath = Path.GetTempPath();
            string path = TempPath + UserGuid;
            try
            {

                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(DateTime.Now);
                }

                using (StreamReader sr = new StreamReader(path))
                {
                    sr.ReadLine();
                    return true;
                }


            }
            catch
            {
                return false;

            }

        }

        protected string ProductBrand
        {
            get
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si != null)
                {
                    return si.ProductName;
                }
                else
                    return "We7";
            }
        }

        /// <summary>
        /// 获得Assembly版本号
        /// </summary>
        /// <returns></returns>
        public string GetAssemblyVersion()
        {
            return string.Format("{0}.{1}.{2}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart);
        }

        /// <summary>
        /// 获得Assembly产品名称
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyProductName()
        {
             GeneralConfigInfo si = GeneralConfigs.GetConfig();
             if (si != null)
             {
                 return si.ProductName;
             }
             else
                 return AssemblyFileVersion.ProductName;
        }

        /// <summary>
        /// 获得Assembly产品版权
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyCopyright()
        {

            return AssemblyFileVersion.LegalCopyright;
        }
    }
}