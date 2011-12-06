using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using VisualDesign.Module;
using We7.CMS.Module.VisualTemplate;
using We7.CMS.Module.VisualTemplate.Utils;
using System.Collections.Generic;
using We7.CMS.Module.VisualTemplate.Services;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Web.SessionState;
using We7.CMS.Module.VisualTemplate.Models.Temp;
using We7.CMS.WebControls.Core;
using System.Web.UI;
using We7.CMS.Common;
using System.IO;
using We7.Framework.Config;
namespace We7.CMS.Web.Admin.VisualTemplate
{
    /// <summary>
    /// 可视化设计AJAX操作后台代码(包含：移动，删除,发布)
    /// </summary>
    public class WidgetAjaxService : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            AjaxResponse ajaxMessage = new AjaxResponse();

            try
            {
                //参数
                var action = context.Request["action"];
                string file = context.Request["file"];
                string folder = context.Request["folder"];


                //删除操作
                #region 删除操作
                if (string.Compare("delete", action, true) == 0)
                {
                    //获取操作
                    string widgetId = context.Request["id"];

                    VisualDesignFile vd = new VisualDesignFile(folder, file);
                    vd.Delete(widgetId);
                    vd.SaveDraft(true);
                    ajaxMessage.Success = true;
                }
                #endregion
                //移动操作
                #region 移动操作
                else if (string.Compare("move", action, true) == 0)
                {
                    //获取参数
                    var target = context.Request["target"];
                    var id = context.Request["id"];
                    var nextId = context.Request["nextid"];

                    VisualDesignFile vd = new VisualDesignFile(folder, file);
                    vd.Move(target, id, nextId);
                    vd.SaveDraft();

                }
                #endregion
                #region 编辑背景
                else if (string.Compare("editbg", action, true) == 0)
                {
                    VisualDesignFile vd = new VisualDesignFile(folder, file);
                    var bodyAttr = Base64Helper.Decode(context.Request["bodyattr"]);
                    var containerAttr = Base64Helper.Decode(context.Request["ccattr"]);
                    vd.ReplaceDomAttr(vd.Body, bodyAttr);
                    if (vd.PageContainer != null)
                        vd.ReplaceDomAttr(vd.PageContainer, containerAttr);
                    vd.SaveDraft();
                }
                #endregion
                //发布模板
                #region 发布模板
                else if (string.Compare("publish", action, true) == 0)
                {
                    VisualDesignFile vd = new VisualDesignFile(folder, file);
                    vd.Publish();

                    //样式整合
                    vd.CombineStyle();
                }
                #endregion
                //编辑布局
                #region 编辑布局
                else if (string.Compare("editlayout", action, true) == 0)
                {
                    var temp = context.Request["params"];

                    if (string.IsNullOrEmpty(temp))
                        throw new ArgumentNullException("params为空!");

                    temp = Base64Helper.Decode(temp);

                    var layoutParams = JavaScriptConvert.DeserializeObject<WidgetDesign>(temp);

                    VisualDesignFile vdFile = new VisualDesignFile(folder, file);

                    for (int i = 0; i < layoutParams.Columns.Count; i++)
                    {
                        var col = layoutParams.Columns[i];

                        var node = vdFile.GetElementById(col["id"].ToString());
                        if (node.Attributes.Contains("style"))
                        {
                            node.Attributes["style"].Value = col["style"].ToString();
                        }
                        else
                        {
                            node.Attributes.Add("style", col["style"].ToString());
                        }
                        if (node.Attributes.Contains("width"))
                        {
                            node.Attributes["width"].Value = col["width"].ToString();
                        }
                        else
                        {
                            node.Attributes.Add("width", col["width"].ToString());
                        }
                        if (node.Attributes.Contains("cssclass"))
                        {
                            node.Attributes["cssclass"].Value = col["cssclass"].ToString();
                        }
                        else
                        {
                            node.Attributes.Add("cssclass", col["cssclass"].ToString());
                        }
                        if (node.Attributes.Contains("widthunit"))
                        {
                            node.Attributes["widthunit"].Value = col["widthunit"].ToString();
                        }
                        else
                        {
                            node.Attributes.Add("widthunit", col["widthunit"].ToString());
                        }
                    }
                    vdFile.SaveDraft();
                }
                #endregion
                //更换主题
                #region 更换主题
                else if (string.Compare("changetheme", action, true) == 0)
                {
                    var theme = context.Request["theme"].Trim();

                    if (string.IsNullOrEmpty(theme))
                        throw new ArgumentNullException("主题不能为空!");
                    VisualDesignFile vdFile = new VisualDesignFile(folder, file);

                    vdFile.AddOrUpdateThemeFile(theme);

                    vdFile.SaveDraft();
                }
                #endregion
                //复制
                #region 复制
                else if (string.Compare("copy", action, true) == 0)
                {
                    var id = context.Request["id"];
                    var fileName = context.Request["fileName"];

                    VisualDesignFile vdFile = new VisualDesignFile(folder, file);
                    var node = vdFile.GetElementById(id);

                    Dictionary<string, object> WidgetParameter = new Dictionary<string, object>();

                    BaseControlHelper DcHelper = new BaseControlHelper();
                    DCInfo infoList = DcHelper.PickUp(fileName);
                    foreach (DCPartInfo partInfo in infoList.Parts)
                    {
                        List<DataControlParameter> parsList = partInfo.Params;
                        foreach (DataControlParameter par in parsList)
                        {
                            object supportCopy = par.SupportCopy;
                            if (node.Attributes.Contains(par.Name) && supportCopy.ToString() == Boolean.TrueString)
                            {
                                WidgetParameter.Add(par.Name.ToLower(), node.Attributes[par.Name].Value);
                            }
                        }
                    }
                    if (!WidgetParameter.ContainsKey("filename"))
                        WidgetParameter.Add("filename", fileName);
                    //WidgetParameter.Add("id", node.Attributes["id"].Value);
                    if (infoList.Parts.Count == 0)
                    {
                        ajaxMessage.Success = false;
                        ajaxMessage.Message = "该控件没有没有可复制的属性!";
                    }
                    else
                    {
                        ajaxMessage.Success = true;
                        ajaxMessage.Message = JavaScriptConvert.SerializeObject(WidgetParameter);
                    }
                }
                #endregion
                #region 检查模板是否存在
                else if(String.Compare("checktempexist",action,true)==0)
                {
                    string templateGroupName = 
                        GeneralConfigs.GetConfig().DefaultTemplateGroupFileName.ToLower().Replace(".xml","");
                    string fileName = HttpContext.Current.Server.MapPath(
                        string.Format("/_skins/~{0}/{1}.ascx", templateGroupName, file) );
                    if (File.Exists(fileName))
                    {
                        ajaxMessage.Success = false;
                        ajaxMessage.Message = "已经存在此模板";
                    }
                    else
                    {
                        ajaxMessage.Success = false;
                        ajaxMessage.Message = "";
                    }
                }
                #endregion
                else
                {
                    ajaxMessage.Success = false;
                    ajaxMessage.Message = "不存在该操作!";
                }
            }
            catch (Exception ex)
            {
                ajaxMessage.Success = false;
                ajaxMessage.Message = ex.Message;
            }
            context.Response.Write(ajaxMessage.ToJson());

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
