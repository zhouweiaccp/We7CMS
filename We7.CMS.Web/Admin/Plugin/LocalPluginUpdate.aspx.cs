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
using We7.CMS.Plugin;
using System.IO;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.Plugin
{
    public partial class LocalPluginUpdate : System.Web.UI.Page
    {
        private PluginHelper helper = new PluginHelper();
        protected PluginInfo ginfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            ginfo = new PluginInfo(PluginType);
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            try
            {
                string pluginName = Path.GetFileNameWithoutExtension(ZipFileUpload.FileName);
                if (String.IsNullOrEmpty(pluginName))
                {
                    Response.Write("文件不能为空！");
                    return;
                }

                string type = Request.QueryString["type"] ?? "";
                type = type.Trim();
                if (pluginName != type)
                {
                    Response.Write("请上传需要更新的插件");
                    return;
                }

                string temppath = Path.Combine(ginfo.PluginsClientPath, "_temp");
                if (!Directory.Exists(temppath))
                {
                    Directory.CreateDirectory(temppath);
                }
                helper.ExtractZipFile(ZipFileUpload.FileBytes, temppath);

                try
                {
                    string pluginCfg = Path.Combine(temppath, pluginName);
                    pluginCfg = Path.Combine(pluginCfg, "Plugin.xml");
                    PluginInfo tempInfo = new PluginInfo(pluginCfg);
                    if (String.IsNullOrEmpty(tempInfo.Version) || String.Compare(tempInfo.Version, PluignCheckVersionCommand.Version, true) < 0)
                    {
                        Response.Write("插件版本低于" + PluignCheckVersionCommand.Version + ",不能进行更新!");
                    }
                    else
                    {
                        helper.ExtractZipFile(ZipFileUpload.FileBytes, ginfo.PluginsClientPath);

                        PluginInfo info = PluginInfoCollection.CreateInstance(PluginType)[pluginName];
                        if (info != null)
                        {
                            info.LoadXml();
                            info.IsLocal = true;
                            info.Save();
                        }

                        string script = "parent.mask.showMessageProgressBar({0})";
                        string update = "update";
                        switch (PluginType)
                        {
                            case PluginType.PLUGIN:
                                update = "update";
                                break;
                            case PluginType.RESOURCE:
                                update = "insctr";
                                break;
                        }
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update", String.Format(script, "{" + String.Format("action:'{0}',plugin:'{1}',title:'{2}',message:'{3}'", update, pluginName, "更新插件", "更新成功") + "}"), true);
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    Directory.Delete(temppath, true);
                }
            }
            catch (Exception ex)
            {
                Response.Write("更新失败!错误原因：" + ex.Message);
            }
        }

        private PluginType PluginType
        {
            get
            {
                if (ViewState["WE7$PluginType"] == null)
                {
                    string pltype = Request["pltype"];
                    if (String.IsNullOrEmpty(pltype))
                    {
                        ViewState["WE7$PluginType"] = PluginType.PLUGIN;
                    }
                    else
                    {
                        switch (pltype.ToLower().Trim())
                        {
                            case "constrol":
                                ViewState["WE7$PluginType"] = PluginType.RESOURCE;
                                break;
                            case "plugin":
                                ViewState["WE7$PluginType"] = PluginType.PLUGIN;
                                break;
                            default:
                                ViewState["WE7$PluginType"] = PluginType.PLUGIN;
                                break;
                        }
                    }
                }
                return (PluginType)ViewState["WE7$PluginType"];
            }
        }
    }
}
