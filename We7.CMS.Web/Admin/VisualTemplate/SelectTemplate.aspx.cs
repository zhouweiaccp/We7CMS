using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VTModule = We7.CMS.Module.VisualTemplate;
using We7.Framework.Util;
using We7.CMS.Module.VisualTemplate.Services;
using VisualDesign.Module;
using We7.Framework.Config;
namespace We7.CMS.Web.Admin.VisualTemplate
{
    public partial class SelectTemplate : BasePage
    {
        /// <summary>
        /// 是否使用母版
        /// </summary>
        protected override We7.CMS.Common.Enum.MasterPageMode MasterPageIs
        {
            get
            {
                return We7.CMS.Common.Enum.MasterPageMode.NoMenu;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public VTModule.Models.TemplateList GetTemplateList()
        {
            VisualLayoutTemplate layoutTemplateService = new VisualLayoutTemplate();

            var templates = layoutTemplateService.GetSystemTemplates();

            foreach (var item in templates.Templates)
            {
                if (!item.NotInherit)
                {
                    item.Floder = templates.Floder;
                }

            }
            return templates;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save(object sender, EventArgs e)
        {
            var layoutPath = this.layoutName.Value;
            if (!layoutPath.StartsWith("~"))
            {
                layoutPath = "~" + layoutPath;
            }
            layoutPath = Server.MapPath(layoutPath);

            string folder = RequestHelper.Get<string>("folder");
            string file = RequestHelper.Get<string>("file");
            string path = Server.MapPath(string.Format("~/_skins/~{0}/{1}", folder, file));
            string url = string.Format("VisualDesign.aspx?folder=" + folder + "&file=" + file + "&state=design1&virtualdata=virtualdata");
            try
            {
                //文件夹不存在则创建
                if (!System.IO.Directory.Exists(Server.MapPath(string.Format("~/_skins/~{0}/", folder))))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(string.Format("~/_skins/~{0}/", folder)));
                }
                if (FileHelper.Exists(path))
                    FileHelper.DeleteFile(path);

                string strHtml = FileHelper.ReadFileWithLine(layoutPath, System.Text.Encoding.UTF8);
                FileHelper.WriteFileWithEncoding(path, strHtml, System.IO.FileMode.CreateNew, System.Text.Encoding.UTF8);

                //FileHelper.Copy(layoutPath, path,true);

                VisualDesignFile vdFile = new VisualDesignFile(folder, file);

                new System.Security.Permissions.FileIOPermission
    (System.Security.Permissions.FileIOPermissionAccess.Write, new string[] { path }).Demand();

                vdFile.AddOrUpdateThemeFile(GeneralConfigs.GetConfig().Theme);
                vdFile.SaveDraft();
                Response.Redirect(url, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
