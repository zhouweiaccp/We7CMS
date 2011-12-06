using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Web.Script.Services;
using System.Data;
using System.Text;
using Newtonsoft.Json;

using We7.Model.Core;
using We7.Model.Core.Data;
using We7.Framework.Util;
using We7.Framework;
using We7.CMS.Accounts;
using We7.CMS.WebControls.Core;
using Thinkment.Data;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.ContentModel.ajax
{
    /// <summary>
    /// Ajax返回信息
    /// </summary>
    public class AjaxMessage
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 返回信息提示
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回的数据
        /// </summary>
        public object Data { get; set; }
    }
    /// <summary>
    /// ContentModel 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://we7.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService()]
    public class ContentModel : System.Web.Services.WebService
    {

        /// <summary>
        /// 获取数据列json
        /// </summary>
        /// <param name="model">model名称</param>
        /// <returns>数据列json</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDataColumn(string model)
        {

            ModelInfo modelInfo = ModelHelper.GetModelInfo(model);

            We7DataColumnCollection columns = modelInfo.DataSet.Tables[0].Columns;
            We7DataColumnCollection dcs = new We7DataColumnCollection();
            foreach (We7DataColumn dc in columns)
            {
                if (dc.IsSystem && !dc.List)
                    continue;
                if (dc.Direction == ParameterDirection.ReturnValue)
                    continue;

                dcs.Add(dc);
            }

            //string json = JavaScriptConvert.SerializeObject(columns).Replace("null", "\"\"");
            string json = JavaScriptConvert.SerializeObject(dcs).Replace("null", "\"\"");

            return json;
        }


        /// <summary>
        /// 获取查询数据列json
        /// </summary>
        /// <param name="model">model名称</param>
        /// <returns>数据列json</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetSearchDataColumn(string model)
        {

            ModelInfo modelInfo = ModelHelper.GetModelInfo(model);

            We7DataColumnCollection columns = GetConditionControl(modelInfo);

            string json = JavaScriptConvert.SerializeObject(columns).Replace("null", "\"\"");

            return json;
        }

        /// <summary>
        /// 获取编辑控件
        /// </summary>
        /// <param name="model">模型名称</param>
        /// <param name="panel">面板名称</param>
        /// <param name="index">页面索引</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetEditControls(string model, string panel, int index)
        {
            ModelInfo modelInfo = ModelHelper.GetModelInfo(model);
            AjaxMessage ajaxMessage = new AjaxMessage();
            ajaxMessage.Success = false;
            ajaxMessage.Message = "控件为空!";
            if (modelInfo.Layout.Panels[panel] == null || modelInfo.Layout.Panels[panel].EditInfo == null)
            {
                return JavaScriptConvert.SerializeObject(ajaxMessage);
            }
            else
            {
                GroupCollection groups = modelInfo.Layout.Panels[panel].EditInfo.Groups;
                Group group = GetGroupByIndex(index, groups);
                if (group != null)
                {
                    ajaxMessage.Data = group;// editControls;
                    ajaxMessage.Success = true;
                    ajaxMessage.Message = "获取成功!";
                }

                return JavaScriptConvert.SerializeObject(ajaxMessage).Replace("null", "\"\"");
            }
        }
        /// <summary>
        /// 获取组
        /// </summary>
        /// <param name="model">模型名称</param>
        /// <param name="panel">面板名称</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetGroups(string model, string panel)
        {
            ModelInfo modelInfo = ModelHelper.GetModelInfo(model);
            if (modelInfo.Layout.Panels[panel] == null || modelInfo.Layout.Panels[panel].EditInfo == null
                || modelInfo.Layout.Panels[panel].EditInfo.Groups == null)
            {
                AjaxMessage ajaxMessage = new AjaxMessage();
                ajaxMessage.Success = false;
                ajaxMessage.Message = "控件为空!";
                return JavaScriptConvert.SerializeObject(ajaxMessage);
            }
            else
            {
                AjaxMessage ajaxMessage = new AjaxMessage();
                ajaxMessage.Success = true;
                ajaxMessage.Message = "获取成功!";
                GroupCollection groups = modelInfo.Layout.Panels[panel].EditInfo.Groups;
                ajaxMessage.Data = groups;// editControls;
                return JavaScriptConvert.SerializeObject(ajaxMessage).Replace("null", "\"\"");
            }
        }
        /// <summary>
        /// 添加配置文件中的group
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <param name="panel"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AddGroup(string model, string name, int index, string panel)
        {
            ModelInfo modelInfo = ModelHelper.GetModelInfo(model);
            AjaxMessage ajaxMessage = new AjaxMessage();
            ajaxMessage.Success = false;
            ajaxMessage.Message = "添加错误!";

            if (modelInfo.Layout.Panels[panel] == null || modelInfo.Layout.Panels[panel].EditInfo == null)
            {
                return JavaScriptConvert.SerializeObject(ajaxMessage);
            }
            else
            {
                if (HasGroup(name, ref index, modelInfo.Layout.Panels[panel].EditInfo.Groups))
                    ajaxMessage.Message = "已存在该标记！";
                else
                {
                    ajaxMessage.Success = true;
                    ajaxMessage.Message = "添加成功!";
                    Group group = new Group();
                    group.Index = index;
                    group.Name = name;
                    group.Next = index;
                    modelInfo.Layout.Panels[panel].EditInfo.Groups.Add(group);
                    ModelHelper.SaveModelInfo(modelInfo);
                    ajaxMessage.Data = modelInfo.Layout.Panels[panel].EditInfo.Groups;
                }
                return JavaScriptConvert.SerializeObject(ajaxMessage).Replace("null", "\"\"");
            }
        }

        /// <summary>
        /// 组是否存在
        /// </summary>
        /// <param name="name">组名称</param>
        /// <param name="index">索引</param>
        /// <param name="groups">组列表</param>
        /// <returns></returns>
        private static bool HasGroup(string name, ref int index, GroupCollection groups)
        {
            foreach (Group group in groups)
            {
                if (group.Name == name) return true;
                if (group.Index == index)
                {
                    index++;
                    HasGroup(name, ref index, groups);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据索引值获取group
        /// </summary>
        /// <param name="index">索引值</param>
        /// <param name="groups">所要查询的组列表</param>
        /// <returns></returns>
        private static Group GetGroupByIndex(int index, GroupCollection groups)
        {
            foreach (Group group in groups)
            {
                if (group.Index == index) return group;
            }
            return null;
        }

        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="model">内容模型名</param>
        /// <param name="index">要删除的group索引</param>
        /// <param name="panel">面板名称</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DelGroup(string model, int index, string panel)
        {
            ModelInfo modelInfo = ModelHelper.GetModelInfo(model);
            AjaxMessage ajaxMessage = new AjaxMessage();
            ajaxMessage.Success = false;
            ajaxMessage.Message = "不存在!";
            if (modelInfo.Layout.Panels[panel] == null || modelInfo.Layout.Panels[panel].EditInfo == null ||
            modelInfo.Layout.Panels[panel].EditInfo.Groups == null)
            {
                return JavaScriptConvert.SerializeObject(ajaxMessage);
            }
            else
            {
                bool flag = false;
                GroupCollection groups = modelInfo.Layout.Panels[panel].EditInfo.Groups;
                foreach (Group group in groups)
                {
                    if (group.Next == index) group.Next = group.Index;
                    if (group.Index == index)
                    {
                        modelInfo.Layout.Panels[panel].EditInfo.Groups.Remove(group);
                        flag = true;
                        break;
                    }
                }
                ajaxMessage.Success = flag;
                if (flag)
                {
                    ajaxMessage.Message = "删除成功!";
                    ajaxMessage.Data = modelInfo.Layout.Panels[panel].EditInfo.Groups;
                    ModelHelper.SaveModelInfo(modelInfo);
                }
                else
                {
                    ajaxMessage.Message = "删除失败！";
                }
                return JavaScriptConvert.SerializeObject(ajaxMessage).Replace("null", "\"\"");
            }
        }

        /// <summary>
        /// 获取列表列
        /// </summary>
        /// <param name="model">模型名称</param>
        /// <param name="panel">面板名称</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetListCoulumn(string model, string panel, int index)
        {
            ModelInfo modelInfo = ModelHelper.GetModelInfo(model);
            if (modelInfo.Layout.Panels[panel] == null && panel == "multi")
            {
                AddMutiPanel(modelInfo, model);
                modelInfo = ModelHelper.GetModelInfo(model);
            }
            ListInfo listInfo = modelInfo.Layout.Panels[panel].ListInfo;
            return JavaScriptConvert.SerializeObject(GetGroupByIndex(index, listInfo.Groups)).Replace("null", "\"\""); ;
        }

        /// <summary>
        /// 获取条件控件
        /// </summary>
        /// <param name="model">模型名称</param>
        /// <param name="panel">面板名称</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetConditionControls(string model, string panel)
        {
            ModelInfo modelInfo = ModelHelper.GetModelInfo(model);
            if (modelInfo.Layout.Panels[panel] == null && panel == "multi")
            {
                AddMutiPanel(modelInfo, model);
                modelInfo = ModelHelper.GetModelInfo(model);
            }
            ConditionInfo condition = modelInfo.Layout.Panels[panel].ConditionInfo;
            return JavaScriptConvert.SerializeObject(condition).Replace("null", "\"\"");
        }

        /// <summary>
        /// 保存列表面板
        /// </summary>
        /// <param name="model">模型名称</param>
        /// <param name="panel">面板名称</param>
        /// <param name="list">列表列</param>
        /// <param name="condition">查询列</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="index"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SaveList(string model, string panel, string list, string condition, string pagesize, int index, string enable)
        {
            string colsJson = HttpContext.Current.Server.UrlDecode(list);

            string conditionJson = HttpContext.Current.Server.UrlDecode(condition);
            ModelInfo modelInfo = ModelHelper.GetModelInfo(model);
            try
            {
                if (modelInfo.Layout.Panels[panel] == null && panel == "multi")
                {
                    AddMutiPanel(modelInfo, model);
                    modelInfo = ModelHelper.GetModelInfo(model);
                }
                ColumnInfoCollection cols = JavaScriptConvert.DeserializeObject<ColumnInfoCollection>(colsJson);

                We7ControlCollection conditionCtr = JavaScriptConvert.DeserializeObject<We7ControlCollection>(conditionJson);

                ModelUIHandler.DealSystemColumn(cols, modelInfo);

                GetGroupByIndex(index, modelInfo.Layout.Panels[panel].ListInfo.Groups).Columns = cols;
                GetGroupByIndex(index, modelInfo.Layout.Panels[panel].ListInfo.Groups).Enable = bool.Parse(enable);
                modelInfo.Layout.Panels[panel].Context.PageSize = Convert.ToInt32(pagesize);
                modelInfo.Layout.Panels[panel].ConditionInfo.Controls = conditionCtr;

                ModelHelper.SaveModelInfo(modelInfo, model);

                //生成数据表
                DataBaseHelperFactory.Create().CreateTable(modelInfo);
            }
            catch (Exception ex)
            {
                //TODO:: resolve exception

                return ex.Message;
            }

            return "保存完毕";
        }


        /// <summary>
        /// 保存编辑面板
        /// </summary>
        /// <param name="model">模型名称</param>
        /// <param name="panel">面板名称</param>
        /// <param name="editControls">编辑控件</param>
        /// <param name="copy">是否复制</param>
        /// <param name="index">group的索引</param>
        /// <param name="next">当前页的后继页</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SaveEdit(string model, string panel, string editControls, string copy, int index, string next, string enable)
        {
            string editJson = HttpContext.Current.Server.UrlDecode(editControls);
            ModelInfo modelInfo = ModelHelper.GetModelInfo(model);
            try
            {
                We7ControlCollection editCtrls = JavaScriptConvert.DeserializeObject<We7ControlCollection>(editJson);
                if (modelInfo.Layout.Panels[panel] == null && panel == "multi")
                {
                    AddMutiPanel(modelInfo, model);
                    modelInfo = ModelHelper.GetModelInfo(model);
                }
                foreach (We7Control editCtrl in editCtrls)
                {
                    if (editCtrl.Type == "RelationSelect")
                    {
                        //添加统计字段
                        AddCount(editCtrl, modelInfo);
                    }
                }

                Group group = GetGroupByIndex(index, modelInfo.Layout.Panels[panel].EditInfo.Groups);
                group.Controls = editCtrls;
                group.Next = int.Parse(next);
                group.Enable = bool.Parse(enable);
                if (modelInfo.Type == ModelType.ACCOUNT)
                {
                    List<We7Control> DeleteCtrs = new List<We7Control>();
                    foreach (We7Control ctr in modelInfo.Layout.Panels["fedit"].EditInfo.Controls)
                    {
                        if (ctr.Name == "ID")
                            continue;
                        DeleteCtrs.Add(ctr);
                    }
                    foreach (We7Control ctr in DeleteCtrs)
                    {
                        modelInfo.Layout.Panels["fedit"].EditInfo.Controls.Remove(ctr);
                    }
                    foreach (We7Control ctr in modelInfo.Layout.Panels["edit"].EditInfo.Controls)
                    {
                        if (ctr.Name == "ID")
                            continue;
                        group.Controls.Add(ctr.Clone() as We7Control);
                    }
                }

                if (copy == "true")
                {
                    if (modelInfo.Type == ModelType.ARTICLE)
                    {
                        if (modelInfo.Layout.Panels["multi"] != null)
                        {

                            modelInfo.Layout.Panels["multi"].EditInfo = modelInfo.Layout.Panels["edit"].EditInfo;
                        }
                    }
                }
                ModelHelper.SaveModelInfo(modelInfo, model);

                //生成数据表
                DataBaseHelperFactory.Create().CreateTable(modelInfo);
            }
            catch (Exception ex)
            {
                //TODO:: resolve exception

                return ex.Message;
            }

            return "保存完毕";
        }

        private static void AddCount(We7Control contrl, ModelInfo subModel)
        {
            if (contrl.Params["count"] != "true") return;
            ModelInfo modelInfo = ModelHelper.GetModelInfoByName(contrl.Params["model"]);

            We7DataColumn column = new We7DataColumn();
            column.DataType = TypeCode.Int32;
            column.Label = subModel.Label + "统计";
            column.Name = subModel.Name + "_Count";
            ParameterDirection direction = ParameterDirection.Input;
            column.Direction = direction;
            column.Mapping = string.Format("{0}|{1}", contrl.Name, contrl.Params["valuefield"]);
            if (modelInfo.DataSet.Tables == null)
            {
                We7.Model.Core.We7DataTable table = new We7DataTable();
                modelInfo.DataSet.Tables.Add(table);
            }
            modelInfo.DataSet.Tables[0].Columns.AddOrUpdate(column);
            bool success = ModelHelper.SaveModelInfo(modelInfo, contrl.Params["model"]);
        }

        /// <summary>
        /// AddMutiPanel
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <param name="name"></param>
        public void AddMutiPanel(ModelInfo modelInfo, string name)
        {
            Panel panel = new Panel();
            panel.Name = "multi";
            panel.Label = "会员中心显示面板";
            panel.CommandInfo = new CommandInfo();
            panel.CommandInfo.Path = "System.SimpleCommandSystem.SimpleCommand";
            panel.CommandInfo.Visible = true;
            panel.ConditionInfo = new ConditionInfo();
            panel.ConditionInfo.Path = "System.SimpleCondition";
            panel.ConditionInfo.Visible = true;
            panel.Context = new PanelContext();
            panel.EditInfo = new EditInfo();
            panel.EditInfo.Path = "System.CascadeEditor";
            panel.EditInfo.Visible = true;
            panel.EditInfo.Groups = new GroupCollection();
            panel.EditInfo.Groups.Add(new Group());
            panel.EditInfo.Groups[0].Controls = new We7ControlCollection();
            panel.ListInfo = new ListInfo();
            panel.ListInfo.Path = "System.SimpleList";
            panel.ListInfo.Visible = true;
            panel.ListInfo.Groups[0].Columns = new ColumnInfoCollection();
            panel.NavigationInfo = new NavigationInfo();
            panel.NavigationInfo.Path = "System.SimplenNavigation";
            panel.NavigationInfo.Visible = true;
            panel.PagerInfo = new PagerInfo();
            panel.PagerInfo.Path = "System.SimplePage";
            panel.PagerInfo.Visible = true;
            panel.Context.PageSize = 10;
            panel.Context.DataKeyString = "ID";
            //panel.Context.DataKey =new System.Web.UI.WebControls.DataKey(

            modelInfo.Layout.Panels.Add(panel);

            ModelHelper.SaveModelInfo(modelInfo, name);

            //生成数据表
            DataBaseHelperFactory.Create().CreateTable(modelInfo);
        }


        /// <summary>
        /// 删除模型组
        /// </summary>
        /// <param name="group">组名称</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DeleteModelGroup(string group)
        {
            bool success = ModelHelper.DeleteModelGroup(group);

            return "{\"success\":\"" + success.ToString().ToLower() + "\"}";
        }

        /// <summary>
        /// 生成前台部件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="widgetDetailFields"></param>
        /// <param name="widgetListFields"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CreateModelControls(string model, string widgetDetailFields, string widgetListFields)
        {
            try
            {
                ModelInfo modelInfo = ModelHelper.GetModelInfo(model);

                //2011-10-10 取消生成控件
                //ModelHelper.CreateControls(modelInf);
                if (widgetDetailFields.Length > 0)
                {
                    if (widgetDetailFields.LastIndexOf(",") > 0)
                    {
                        widgetDetailFields = widgetDetailFields.Substring(0, widgetDetailFields.LastIndexOf(","));
                        modelInfo.Layout.UCContrl.WidgetDetailFields = widgetDetailFields;
                    }
                }
                if (widgetListFields.Length > 0)
                {
                    if (widgetListFields.LastIndexOf(",") > 0)
                    {
                        widgetListFields = widgetListFields.Substring(0, widgetListFields.LastIndexOf(","));
                        modelInfo.Layout.UCContrl.WidgetListFields = widgetListFields;
                    }
                }
                ModelHelper.SaveModelInfo(modelInfo, modelInfo.ModelName);
                ModelHelper.CreateWidgets(modelInfo);


            }
            catch (Exception ex)
            {
                return "{\"success\":false,\"msg\":\"生成失败，错误消息:" + Utils.JsonCharFilter(ex.Message) + "\"}";
            }

            return "{\"success\":true }";
        }

        /// <summary>
        /// 重建索引（部件，控件)
        /// TODO:该方法不应写在内容模型的WebService里
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CreateModelWidegetsIndex()
        {
            try
            {
                BaseControlHelper ctrHelper = new BaseControlHelper();
                ctrHelper.CreateIntegrationIndexConfig();
                ctrHelper.CreateWidegetsIndex();
            }
            catch (Exception ex)
            {
                return "{\"success\":false,\"msg\":\"生成索引失败，错误消息:" + Utils.JsonCharFilter(ex.Message) + "\"}";

            }
            return "{\"success\":true}";
        }


        /// <summary>
        /// 创建模型数据表
        /// </summary>
        /// <param name="model">模型名称</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CreateModelTable(string model)
        {
            string retVal;
            try
            {
                ModelInfo info = ModelHelper.GetModelInfo(model);
                DataBaseHelperFactory.Create().CreateTable(info);
                retVal = "{\"success\":\"true\"}";
            }
            catch (Exception ex)
            {
                retVal = "{\"success\":\"false\",\"msg\":\"" +Utils.JsonCharFilter( ex.Message) + "\"}";
            }

            return retVal;
        }

        /// <summary>
        /// 创建内容模型自定义布局
        /// </summary>
        /// <param name="model">模型名称</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CreateModelLayout(string model)
        {
            string retVal;
            try
            {
                ModelInfo info = ModelHelper.GetModelInfo(model);
                string path = ModelHelper.CreateModelLayout(info);
                retVal = "{\"success\":\"true\",\"msg\":\"" + path + "\"}";
            }
            catch (Exception ex)
            {
                retVal = "{\"success\":\"false\",\"msg\":\"" + Utils.JsonCharFilter(ex.Message) + "\"}";
            }

            return retVal;
        }
        /// <summary>
        /// 绑定布局
        /// </summary>
        /// <param name="model"></param>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ModelLayoutBind(string model, string path, string type)
        {
            string retVal;
            try
            {
                ModelInfo modelInfo = ModelHelper.GetModelInfo(model);
                bool isBind = path.Length > 0;

                EditInfo info = modelInfo.Layout.Panels["edit"].EditInfo;
                //info.EditCss = ddlAdminViewerCss.SelectedValue;
                //info.ViewerCss = ddlAdminEditorCss.SelectedValue;
                //info.UcCss = ddlUcCss.SelectedValue;
                switch (type)
                {
                    case "Layout":
                        if (!isBind)
                        {
                            info.Path = "We7.Editor";
                            info.Layout = "";
                        }
                        else
                        {
                            info.Path = "We7.UxLayoutEditor";
                            info.Layout = path;
                        }
                        break;
                    case "ViewerLayout":
                        if (!isBind)
                        {
                            info.ViewerPath = "We7.Viewer";
                            info.ViewerLayout = "";
                        }
                        else
                        {
                            info.ViewerPath = "We7.UxLayoutViewer";
                            info.ViewerLayout = path;
                        }
                        break;
                    case "UcLayout":
                        if (!isBind)
                        {
                            // info.Path = "We7.Editor";
                            info.UcLayout = "";
                        }
                        else
                        {
                            //info.Path = "We7.UxLayoutEditor";
                            info.UcLayout = path;
                        }

                        break;
                    default:

                        break;
                }

                modelInfo.Layout.Panels["edit"].EditInfo = info;
                ModelHelper.SaveModelInfo(modelInfo, modelInfo.ModelName);



                retVal = "{\"success\":\"true\"}";
            }
            catch (Exception ex)
            {
                retVal = "{\"success\":\"false\",\"msg\":\"" + Utils.JsonCharFilter(ex.Message) + "\"}";
            }

            return retVal;
        }


        /// <summary>
        /// 创建模型组
        /// </summary>
        /// <param name="cnName"></param>
        /// <param name="enName"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CreateModelGroup(string cnName, string enName)
        {
            string retVal;
            try
            {
                ModelGroup group = ModelHelper.GetModelGroupByName(enName);
                if (group != null)
                {
                    retVal = "{success:false,'msg':'当前模型组已存在'}";
                }
                else
                {
                    group = new ModelGroup();
                    group.Name = enName;
                    group.Label = cnName;
                    group.System = false;
                    ModelHelper.CreateModelGroup(group);
                    retVal = "{'success':true}";
                }
            }
            catch (Exception ex)
            {

                retVal = "{'success':false,'msg':'" + Utils.JsonCharFilter(ex.Message) + "'}";
            }

            return retVal;
        }

        /// <summary>
        /// 加载模型数据
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string LoadModelGroup()
        {
            string retVal;
            try
            {
                StringBuilder sb = new StringBuilder("{success:true,data:[");
                ModelGroupCollection groups = ModelHelper.GetModelGroups();
                foreach (ModelGroup group in groups)
                {
                    sb.Append("{txt:'").Append(group.Label).Append("',val:'").Append(group.Name).Append("'},");
                }
                Utils.TrimEndStringBuilder(sb, ",");
                sb.Append("]}");
                retVal = sb.ToString();
            }
            catch (Exception ex)
            {
                retVal = "{'success':'false','msg':'" + Utils.JsonCharFilter(ex.Message) + "'}";
            }

            return retVal;
        }

        /// <summary>
        /// 获取数据列json
        /// </summary>
        /// <param name="model">model名称</param>
        /// <returns>数据列json</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetOutDataColumn(string model)
        {

            ModelInfo modelInfo = ModelHelper.GetModelInfo(model);

            We7DataColumnCollection columns = modelInfo.DataSet.Tables[0].Columns;

            We7DataColumnCollection cols = new We7DataColumnCollection();

            for (int i = 0; i < columns.Count; i++)
            {
                if (!columns[i].IsSystem && (columns[i].Direction == ParameterDirection.Output || columns[i].Direction == ParameterDirection.InputOutput))
                {
                    cols.Add(columns[i]);
                }
            }
            string json = JavaScriptConvert.SerializeObject(cols).Replace("null", "\"\"");

            return json;
        }

          /// <summary>
          /// 删除内容模型字段
          /// </summary>
          /// <param name="model">内容模型名称</param>
          /// <param name="field">字段名称</param>
          /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DeleteFiled(string model, string field)
        {
            bool success = false;
            try
            {
                ModelHelper.DeleteModelField(model, field);
                success = true;
            }
            catch { }

            return "{\"success\":\"" + success.ToString().ToLower() + "\"}";
        }
        /// <summary>
        /// 添加单个数据项
        /// </summary>
        /// <param name="model"></param>
        /// <param name="label"></param>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="search"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AddSingleDataColumn(string model, string label, string name, string title, string search, string dataType, int maxLength)
        {
            //获取对应的modelinfo
            ModelInfo modelInfo = ModelHelper.GetModelInfoByName(model);

            bool success = false;

            AjaxMessage ajaxMessage = new AjaxMessage();
            ajaxMessage.Success = success;

            if (modelInfo == null)
            {
                ajaxMessage.Message = "模型对象为空!";
                return JavaScriptConvert.SerializeObject(ajaxMessage);
            }

            if (CheckRepetColumn(modelInfo, delegate(We7DataColumn dc)
            {
                return dc.Label == label;
            }))
            {
                ajaxMessage.Message = "当前中文名称已存在";
                return JavaScriptConvert.SerializeObject(ajaxMessage);
            }

            if (CheckRepetColumn(modelInfo, delegate(We7DataColumn dc)
            {
                return String.Compare(dc.Name, name, true) == 0;
            }))
            {
                ajaxMessage.Message = "当前英文名已存在";
                return JavaScriptConvert.SerializeObject(ajaxMessage);
            }


            We7DataColumn column = new We7DataColumn();
            column.DataType = (TypeCode)Enum.Parse(typeof(TypeCode), dataType, true);
            if (column.DataType == TypeCode.String)
            {
                column.MaxLength = maxLength;
            }
            ParameterDirection direction = ParameterDirection.Input;
            string mapping = string.Empty;
            if (title == "true")
            {
                direction = ParameterDirection.Output;
                if (hasTitle(modelInfo))
                {
                    ajaxMessage.Message = "已经拥有标题项!";
                    return JavaScriptConvert.SerializeObject(ajaxMessage);
                }
                mapping = "Title";
            }
            if (title != "true" && search == "true")
            {
                int count = 0;
                direction = ParameterDirection.Output;
                mapping = GetMapping(modelInfo, out count);

                if (string.IsNullOrEmpty(mapping))
                {
                    ajaxMessage.Message = "已经拥有最大查询项:" + count.ToString();
                    return JavaScriptConvert.SerializeObject(ajaxMessage);
                }
            }
            column.Direction = direction;
            column.Label = label;
            column.Name = name;
            //column.Require = true;
            column.Mapping = mapping;


            //TODO::tedyding 是否存在Tables 以及多个表
            if (modelInfo.DataSet.Tables == null)
            {
                We7.Model.Core.We7DataTable table = new We7DataTable();
                modelInfo.DataSet.Tables.Add(table);
            }

            modelInfo.DataSet.Tables[0].Columns.AddOrUpdate(column);


            success = ModelHelper.SaveModelInfo(modelInfo, model);
            if (success)
            {
                ajaxMessage.Success = success;
                ajaxMessage.Message = "添加成功!";
            }
            else
            {
                ajaxMessage.Message = "添加失败!";
            }
            return JavaScriptConvert.SerializeObject(ajaxMessage);
        }

        //获取查询项
        private We7DataColumnCollection GetConditionControl(ModelInfo modelInfo)
        {
            We7DataColumnCollection collections = new We7DataColumnCollection();

            We7DataColumnCollection cols = modelInfo.DataSet.Tables[0].Columns;

            IList<DefaultModel> defaultModels = ModelHelper.GetDefaultModels();

            DefaultModel defaultModel = null;
            for (int i = 0; i < defaultModels.Count; i++)
            {
                if (defaultModels[i].Name == ConvertModelType(modelInfo.Type))
                {
                    defaultModel = defaultModels[i];
                }
            }
            string[] mappingField = null;
            if (defaultModel != null)
            {
                mappingField = defaultModel.MappingFields.Split(new char[] { '|' });
            }

            bool hasTitle = false;

            if (mappingField != null && mappingField.Length > 0)
            {
                for (int i = 0; i < mappingField.Length; i++)
                {

                    for (int j = 0; j < cols.Count; j++)
                    {
                        if (mappingField[i] == cols[j].Mapping)
                        {
                            collections.Add(cols[j]);
                        }

                        if (!hasTitle && cols[j].Mapping == "Title")
                        {
                            collections.Add(cols[j]);
                            hasTitle = true;
                        }

                    }
                }

            }

            return collections;
        }
        /// <summary>
        /// 复制到会员中心
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CopyToUserCenter(string model)
        {
            AjaxMessage ajaxMessage = new AjaxMessage();
            bool success = false;
            ModelInfo modelInfo = ModelHelper.GetModelInfoByName(model);

            if (modelInfo.Type != ModelType.ARTICLE)
            {
                ajaxMessage.Success = success;
                ajaxMessage.Message = "模型类型不是文章类型,没有会员中心!";

                return JavaScriptConvert.SerializeObject(ajaxMessage);
            }

            modelInfo.Layout.Panels["multi"].EditInfo.Groups[0].Controls = modelInfo.Layout.Panels["edit"].EditInfo.Groups[0].Controls;

            success = ModelHelper.SaveModelInfo(modelInfo, model);

            ajaxMessage.Success = success;

            if (success)
            {
                ajaxMessage.Message = "复制成功!";
            }
            else
            {
                ajaxMessage.Message = "复制失败!";
            }
            return JavaScriptConvert.SerializeObject(ajaxMessage);
        }

        //获取查询项
        private string GetMapping(ModelInfo modelInfo, out int count)
        {

            We7DataColumnCollection cols = modelInfo.DataSet.Tables[0].Columns;


            IList<DefaultModel> defaultModels = ModelHelper.GetDefaultModels();

            DefaultModel defaultModel = null;
            for (int i = 0; i < defaultModels.Count; i++)
            {
                if (defaultModels[i].Name == ConvertModelType(modelInfo.Type))
                {
                    defaultModel = defaultModels[i];
                }
            }

            string[] mappingField = null;
            if (defaultModel != null)
            {
                mappingField = defaultModel.MappingFields.Split(new char[] { '|' });
            }

            string mapping = string.Empty;

            if (mappingField != null && mappingField.Length > 0)
            {
                bool bout = false;
                for (int i = 0; i < mappingField.Length; i++)
                {
                    if (bout)
                    {
                        break;
                    }

                    for (int j = 0; j < cols.Count; j++)
                    {
                        if (mappingField[i] == cols[j].Mapping)
                        {
                            break;
                        }
                        if (j == cols.Count - 1 && mappingField[i] != cols[j].Mapping)
                        {
                            mapping = mappingField[i];
                            bout = true;
                        }
                    }
                }

            }
            count = mappingField.Length;
            return mapping;
        }


        //转换默认类型
        private string ConvertModelType(ModelType modelType)
        {
            string str = string.Empty;
            switch (modelType)
            {
                case ModelType.ARTICLE:
                    str = "Template.ArticleModel";
                    break;
                case ModelType.ADVICE:
                    str = "Template.AdviceModel";
                    break;
                case ModelType.ACCOUNT:
                    str = "Template.AccountModel";
                    break;
                default:
                    str = "Template.ArticleModel";
                    break;
            }

            return str;
        }
        //是否包含标题项
        private bool hasTitle(ModelInfo modelInfo)
        {
            bool has = false;
            We7DataColumnCollection cols = modelInfo.DataSet.Tables[0].Columns;

            for (int i = 0; i < cols.Count; i++)
            {
                if (cols[i].Mapping == "Title")
                {
                    has = true;
                    break;
                }
            }

            return has;
        }

        /// <summary>
        /// 检测重复列，有重复信息返回true,没有返回false
        /// </summary>
        /// <param name="info"></param>
        /// <param name="pre"></param>
        /// <returns></returns>
        bool CheckRepetColumn(ModelInfo info, Predicate<We7DataColumn> pre)
        {
            foreach (We7DataColumn dc in info.DataSet.Tables[0].Columns)
            {
                if (pre(dc))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 拖动排序
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ModelSort(string model, string data)
        {
            ModelDBHelper helper = ModelDBHelper.Create(model);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (string d in data.Split('|'))
            {
                string[] dd = d.Split(':');
                if (dd.Length != 2) continue;
                dic.Clear();
                dic.Add("Index", dd[1]);
                helper.Update(dic, new Criteria(CriteriaType.Equals, "ID", dd[0]));
            }

        }
    }

    internal sealed class ModelUIHandler
    {
        public static void DealSystemColumn(ColumnInfoCollection cols, ModelInfo model)
        {
            ColumnInfoCollection tplCols = GetTemplateColumns();
            foreach (ColumnInfo col in cols)
            {
                We7DataColumn dc = model.DataSet.Tables[0].Columns[col.Name];
                if (dc != null && dc.IsSystem && dc.List && tplCols[col.Name] != null)
                {
                    col.Expression = tplCols[col.Name].Expression;
                    col.Params = tplCols[col.Name].Params;
                    col.Type = tplCols[col.Name].Type;
                    col.Mapping = tplCols[col.Name].Mapping;
                    col.ConvertType = tplCols[col.Name].ConvertType;
                }
            }
        }

        static ColumnInfoCollection GetTemplateColumns()
        {
            string cacheKey = "$ColumnInfoCollection$SystemControls$";

            ColumnInfoCollection cols = AppCtx.Cache.RetrieveObject<ColumnInfoCollection>(cacheKey);
            if (cols == null)
            {
                string path = HttpContext.Current.Server.MapPath("~/Admin/ContentModel/Config/Columns.tpl");
                if (File.Exists(path))
                {
                    cols = SerializationHelper.Load(typeof(ColumnInfoCollection), path) as ColumnInfoCollection;
                }
                cols = cols ?? new ColumnInfoCollection();
                AppCtx.Cache.AddObjectWithFileChange(cacheKey, cols, path);
            }
            return cols;
        }


    }
}
