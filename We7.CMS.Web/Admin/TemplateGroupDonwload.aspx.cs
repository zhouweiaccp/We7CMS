using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.Framework.Config;
using We7.Framework.Zip;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateGroupDonwload : BasePage
    {

        string FileName
        {
            get { return Request["file"]; }
        }

        protected TemplateGroup Data
        {
            get { return ViewState["$VS_TEMPLATEGROUP_DATA"] as TemplateGroup; }
            set { ViewState["$VS_TEMPLATEGROUP_DATA"] = value; }
        }


        public static bool EnableSiteSkins
        {
            get
            {
                string _default = SiteSettingHelper.Instance.Config.EnableSiteSkins;
                if (_default.ToLower() == "true")
                    return true;
                return false;
            }
        }

        protected override void Initialize()
        {
            Data = TemplateHelper.GetTemplateGroup(FileName);
            TemplateGroupNameLabel.Text = Path.GetFileNameWithoutExtension(FileName);
            if (EnableSiteSkins)
                CreateTempFile();
            else
                CreateZip(FileName);
        }

        protected void CreateZip(string FileName)
        {
            //string CopyToPath = Server.MapPath(Constants.TempBasePath);//目标文件夹
            string CopyToPath = String.Format("{0}\\Templates.{1}\\controls", Server.MapPath("~/_temp/"), Path.GetFileNameWithoutExtension(FileName));
            //在目标文件夹 创建文件夹.Files及子文件夹
            string stylesPath = String.Format("{0}\\styles", CopyToPath);//css文件目录
            string groupsPath = String.Format("{0}\\groups", CopyToPath);//模板组文件目录
            string imagePath = String.Format("{0}\\images", CopyToPath);//模板组文件目录
            Directory.CreateDirectory(stylesPath);
            Directory.CreateDirectory(groupsPath);

            //模板组Jpg预览图片文件
            string JpgFile = Server.MapPath(String.Format("/{0}/{1}.jpg", Constants.TemplateGroupBasePath, FileName));
            //模板组Xml文件
            string XmlFile = Server.MapPath(String.Format("/{0}/{1}", Constants.TemplateGroupBasePath, FileName));
            
            

            //拷贝模板组文件
            if (File.Exists(JpgFile))
            {
                File.Copy(JpgFile, String.Format("{0}/{1}.jpg", groupsPath, FileName), true);
            }
            if (File.Exists(XmlFile))
            {
                File.Copy(XmlFile, String.Format("{0}/{1}", groupsPath, FileName), true);
            }

            string imgPath = String.Format("{0}/images", Server.MapPath(Constants.TemplateBasePath));
            CopyFolder(imagePath, imgPath,true);
            string templateFile = Server.MapPath(Constants.TemplateBasePath);
            CopyFolder(CopyToPath,templateFile,false);
            string cssPath = String.Format("{0}/styles", Server.MapPath(Constants.TemplateBasePath));
            CopyFolder(stylesPath, cssPath, true);
            //创建res文件夹
            string resPath = String.Format("{0}\\Templates.{1}\\res", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName));
            Directory.CreateDirectory(resPath);
            //创建版本文件
            //CreateVersion(resPath);

            string[] FileProperties = new string[2];
            FileProperties[0] = String.Format("{0}\\Templates.{1}", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName)); ;//压缩目录
            //FileProperties[1] = String.Format("{0}/{1}.zip", Server.MapPath(Constants.TemplateGroupBasePath), FileName);//压缩后的目录
            FileProperties[1] = String.Format("{0}/Templates.{1}.zip", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName));//压缩后的目录
            //压缩文件
            try
            {
                ZipClass.ZipFileMain(FileProperties);
                DonwloadHyperLink.NavigateUrl = String.Format("~/{0}/Templates.{1}.zip", Constants.TempBasePath.TrimStart('\\').Trim('/'), Path.GetFileNameWithoutExtension(FileName));
                DeleteFolder(String.Format("{0}\\Templates.{1}", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName)));
            }
            catch
            {

            }
        }

        void CreateTempFile()
        {
            string DirectoryName = String.Format("Templates.{0}", Path.GetFileNameWithoutExtension(FileName));
            //目标文件夹
            string CopyToPath = String.Format("{0}/Templates.{1}", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName));
            //源文件夹
            string CopyFromPath = TemplateHelper.TemplateGroupPath;

            CopyFromPath = String.Format("{0}/{1}", CopyFromPath, Path.GetFileNameWithoutExtension(FileName));
            CopyToPath = String.Format("{0}/{2}/Skin/{1}", CopyToPath, Path.GetFileNameWithoutExtension(FileName), DirectoryName);
            //复制模板文件夹
            CopyFolder(CopyToPath, CopyFromPath,true);

            //预览图片源
            string JpgFile = String.Format("{0}.xml.jpg", CopyFromPath);
            //Xml文件源
            string XmlFile = String.Format("{0}.xml", CopyFromPath);

            CopyToPath = String.Format("{0}/Templates.{1}", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName));//目标文件夹
            //复制XML文件、预览图片
            if (File.Exists(JpgFile))
            {
                string str = String.Format("{0}/{2}/Skin/{1}.xml.jpg", CopyToPath, Path.GetFileNameWithoutExtension(FileName), DirectoryName);
                File.Copy(JpgFile, str, true);
            }
            if (File.Exists(XmlFile))
            {
                File.Copy(XmlFile, String.Format("{0}/{2}/Skin/{1}", CopyToPath, FileName, DirectoryName), true);
            }

            //创建res文件夹
            //string resPath = String.Format("{0}/res", CopyToPath);
            //Directory.CreateDirectory(resPath);
            //创建版本文件
            //CreateVersion(resPath);

            //TODO:css、ascx文件路径处理
            //打包
            string[] FileProperties = new string[2];
            FileProperties[0] = CopyToPath;//压缩目录
            FileProperties[1] = String.Format("{0}/Templates.{1}.zip", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName));//压缩后的目录
            //压缩文件
            try
            {
                ZipClass.CreateTemplateZip(FileProperties);
                DonwloadHyperLink.NavigateUrl = String.Format("~/{0}/Templates.{1}.zip", Constants.TempBasePath.TrimStart('/').TrimStart('\\'), Path.GetFileNameWithoutExtension(FileName));
                DeleteFolder(CopyToPath);
            }
            catch
            {

            }
        }

        void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件 
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
                Directory.Delete(dir); //删除已空文件夹 
            }
        }

        void CopyStyleFile(string templateName, string tempPath, string filePath)
        {
            FileInfo[] styleFiles = GetCssStyles(templateName,filePath);
            foreach (FileInfo f in styleFiles)
            {
                //拷贝模板的CSS文件
                File.Copy(f.FullName, String.Format("{0}\\{1}", tempPath, f.Name));
                File.SetAttributes(String.Format("{0}\\{1}", tempPath, f.Name), FileAttributes.Normal);
            }
        }
        /// <summary>
        /// 得到模板的CSS文件列表
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns`></returns>
        public FileInfo[] GetCssStyles(string queryName, string cssPath)
        {
            string styleName = String.Format("{0}_CD", queryName);
            DirectoryInfo di = new DirectoryInfo(cssPath);
            FileInfo[] files = di.GetFiles(String.Format("{0}*.css", styleName), SearchOption.TopDirectoryOnly);
            return files;
        }

        void CopyFolder(string aimPath, string srcPath, bool isDirectory)
        {
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                aimPath += Path.DirectorySeparatorChar;

            if (!Directory.Exists(aimPath))
                Directory.CreateDirectory(aimPath);

            string[] fileList = Directory.GetFileSystemEntries(srcPath);
            foreach (string file in fileList)
            {
                //   先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                if (isDirectory)
                {
                    if (Directory.Exists(file))
                    {
                        CopyFolder(aimPath + Path.GetFileName(file), file,true);
                    }
                    //   否则直接Copy文件   
                    else
                    {
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                        File.SetAttributes(aimPath + Path.GetFileName(file), FileAttributes.Normal);
                    }
                }
                else
                {
                    if (File.Exists(file))
                    {
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                        File.SetAttributes(aimPath + Path.GetFileName(file), FileAttributes.Normal);
                    }
                }
            }
        }

        void CreateVersion(string basePath)
        {
            TemplateVersion tv = new TemplateVersion();
            GeneralConfigInfo si = GeneralConfigs.GetConfig();

            tv.TemplatePath = Constants.TemplateBasePath;
            tv.Version = si.ProductVersion;
            tv.UseSkin = EnableSiteSkins;
            tv.FileName = "TemplateVersion.config";
            tv.BasePath = basePath;

            TemplateVersionHelper.SaveTemplateVersion(tv);
        }

    }

}
