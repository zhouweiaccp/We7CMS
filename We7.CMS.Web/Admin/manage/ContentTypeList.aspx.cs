using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

namespace We7.CMS.Web.Admin.manage
{
    public partial class ContentTypeList : BasePage
    {
        public string ContentTypePath
        {
            get { return Server.MapPath("\\Config\\ContentModel"); }
        }

        protected override void Initialize()
        {
            if(Request["del"]!=null) DeleteFile(Request["del"].ToString());
            FileGridView.DataSource = GetContentConfigFiles(null);
            FileGridView.DataBind();
        }

        void DeleteFile(string file)
        {
            if (file.ToLower() == "cotenttype.xml")
                Messages.ShowError("抱歉，cotenttype.xml为系统配置文件不允许删除！");
            else
            {
                string path = Path.Combine(ContentTypePath, file);
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                        Messages.ShowMessage("已成功删除配置文件" + file + "。另外：您还需要修改cotenttype.xml文件，删除其中的项目，才能彻底删除。");
                    }
                    catch (Exception ex)
                    {
                        Messages.ShowError("不能删除文件" + file + "，原因：" + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 读取xml文件列表
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        public FileInfo[] GetContentConfigFiles(string queryName)
        {
            if (!Directory.Exists(ContentTypePath))
            {
                Directory.CreateDirectory(ContentTypePath);
            }
            DirectoryInfo di = new DirectoryInfo(ContentTypePath);
            FileInfo[] files = di.GetFiles("*" + ".xml", SearchOption.TopDirectoryOnly);

            return files;
        }
    }
}
