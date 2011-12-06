using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.IO;
using We7.CMS.Common;
using We7.Framework.Zip;

namespace We7.CMS.Web.Admin
{
    public partial class DataControlDownload :BasePage
    {

        string FileName
        {
            get { 
                return "package.DataControl"+System.DateTime.Now.ToShortDateString().Replace("/","_");
            }
        }


        DataControl[] DataControls;

        protected override void Initialize()
        {

            DataControls = TemplateHelper.GetDataControls(null);
            CreateZip(FileName);
        }

        protected void CreateZip(string FileName)
        {
            //目标文件夹
            string CopyToPath = Server.MapPath("~/_Temp");
            //在目标文件夹 创建文件夹.Files及子文件夹
            Directory.CreateDirectory(CopyToPath + "/" + FileName + ".Files/controls");
            Directory.CreateDirectory(CopyToPath + "/" + FileName + ".Files/res");

            string ascxShotName = "";
            for (int i = 0; i < DataControls.Length; i++)
            {
                ascxShotName = DataControls[i].FileName.Replace(".xml", "");

                // ascx文件
                string AscxFile = Server.MapPath("~/" + Constants.ControlBasePath.Replace("\\", "/")) + "/" + ascxShotName;
                //xml文件 
                string AscxXmlFile = Server.MapPath("~/" + Constants.ControlBasePath.Replace("\\", "/")) + "/" + DataControls[i].FileName;
                string ascxCsFile = Server.MapPath("~/" + Constants.ControlBasePath.Replace("\\", "/")) + "/" + ascxShotName + ".cs";
                string designerFile = Server.MapPath("~/" + Constants.ControlBasePath.Replace("\\", "/")) + "/" + ascxShotName + ".designer.cs";

                //拷贝到目标文件夹
                if (File.Exists(AscxFile))
                {
                    try
                    {
                        File.Copy(AscxFile, CopyToPath + "/" + FileName + ".Files/controls/" + ascxShotName, true);
                    }
                    catch { }

                }
                if (File.Exists(AscxXmlFile))
                {
                    try   //原xml格式错误
                    {
                        File.Copy(AscxXmlFile, CopyToPath + "/" + FileName + ".Files/controls/" + DataControls[i].FileName, true);
                    }
                    catch { }
                }
                if (File.Exists(ascxCsFile))
                {
                    try
                    {
                        File.Copy(ascxCsFile, CopyToPath + "/" + FileName + ".Files/controls/" + ascxShotName + ".cs", true);
                    }
                    catch { }
                }
                if (File.Exists(designerFile))
                {
                    try
                    {
                        File.Copy(designerFile, CopyToPath + "/" + FileName + ".Files/controls/" + ascxShotName + ".designer.cs", true);
                    }
                    catch { }
                }
            }

            string[] FileProperties = new string[2];
            FileProperties[0] = CopyToPath + "/" + FileName + ".Files";//压缩目录
            FileProperties[1] = Server.MapPath(Constants.TempBasePath) + "/" + FileName + ".zip";//压缩后的目录
            //压缩文件
            try
            {
                ZipClass.ZipFileMain(FileProperties);
                DonwloadHyperLink.NavigateUrl = Constants.TempBasePath + "/" + FileName + ".zip";
            }
            catch
            {

            }
        }
    }
}
