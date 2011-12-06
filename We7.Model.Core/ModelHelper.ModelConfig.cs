using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework.Util;
using System.IO;
using System.Xml.Serialization;
using System.Web;
using System.Xml;
using System.Data;
using We7.Model.Core.Config;
using We7.Framework;
using We7.Framework.TemplateEnginer;
using Thinkment.Data;
using We7.CMS.Common;

namespace We7.Model.Core
{
    public partial class ModelHelper
    {
        #region Helper
        protected static HelperFactory HelperFactory
        {
            get
            {
                return We7.Framework.HelperFactory.Instance;
            }
        }

        /// <summary>
        /// 当前Helper的数据访问对象
        /// </summary>
        protected static ObjectAssistant Assistant
        {
            get
            {
                return HelperFactory.Assistant;
            }
        }
        #endregion

        #region const

        /// <summary>
        /// 内容模型索引文件路径
        /// </summary>
        public static string ContentModelConfigPath = ModelConfig.ModelIndexFile;

        /// <summary>
        /// 默认内容模型索引文件
        /// </summary>
        public static string DefaultModelConfigPath = ModelConfig.DefaultModelFile;
        /// <summary>
        /// 模型控件路径
        /// </summary>
        public static string ModelControlsIndex = ModelConfig.ModelControlsIndex;
        private static string CDPath = ModelConfig.CDPath;
        private static string ModelUCConfigTemplate = ModelConfig.ModelUCConfigTemplate;

        /// <summary>
        /// 内容模型部件存储目录
        /// </summary>
        private static string WidgetsSavePath = "~/Widgets/WidgetCollection/内容模型类/";

        /// <summary>
        /// 缓存的关键字
        /// </summary>
        public const string CATCH_CONTENTMODEL_COLLECTION = "CATCH_CONTENTMODEL_COLLECTION";

        #endregion

        #region 内容模型索引

        /// <summary>
        /// 获取索引对象模型集合
        /// </summary>
        /// <returns>用户所有对象模型索引集合</returns>
        public static ContentModelCollection GetAllContentModel()
        {
            ContentModelCollection contentModels = AppCtx.Cache.RetrieveObject<ContentModelCollection>(CATCH_CONTENTMODEL_COLLECTION);
            if (contentModels == null)
            {
                //判断配置文件是否存在,不存在则创建个空文件
                if (File.Exists(ContentModelConfigPath))
                {
                    contentModels = SerializationHelper.Load(typeof(ContentModelCollection), ContentModelConfigPath)
                                                          as ContentModelCollection;
                    if (contentModels == null)
                    {
                        contentModels = new ContentModelCollection();
                        SaveContentModel(contentModels);
                    }
                }
                else
                {
                    contentModels = new ContentModelCollection();
                    SaveContentModel(contentModels);
                }
                AppCtx.Cache.AddObjectWithFileChange(CATCH_CONTENTMODEL_COLLECTION, contentModels, ContentModelConfigPath);
            }

            return contentModels;
        }

        /// <summary>
        /// 重新创建内容模型索引
        /// </summary>
        public static void ReCreateModelIndex()
        {
            DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(ModelConfig.ModelsDirectory));
            if (!di.Exists) return;

            ContentModelCollection cm = new ContentModelCollection();
            ModelGroupCollection mgc = ModelHelper.GetModelGroups();
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                if (String.Compare(d.Name, "inc", true) == 0)
                    continue;
                foreach (FileInfo fi in d.GetFiles())
                {
                    if (!fi.FullName.EndsWith(".xml", StringComparison.CurrentCultureIgnoreCase))
                        continue;
                    try
                    {
                        ModelInfo model = ModelHelper.GetModelInfo(String.Format("{0}.{1}", d.Name, Path.GetFileNameWithoutExtension(fi.Name)));
                        ContentModel c = new ContentModel();
                        c.Name = model.ModelName;
                        c.Label = model.Label;
                        c.Description = model.Desc;
                        c.Type = model.Type;
                        c.DefaultContentName = c.GetDefaultModel(model.Type);
                        c.State = 1;
                        cm.Add(c);
                        ModelGroup mg = mgc[model.GroupName];
                        if (mg == null)
                        {
                            mg = new ModelGroup();
                            mg.Name = d.Name;
                            mg.Label = d.Name;
                            mg.System = false;
                            mg.Description = "重建索引时自动生成";
                            mgc.Add(mg);
                        }
                    }
                    catch { }
                }
            }
            ModelHelper.SaveContentModel(cm);
            ModelHelper.SaveModelGroups(mgc);
        }

        /// <summary>
        /// 按模型类型来取模型集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ContentModelCollection GetContentModel(ModelType type)
        {
            ContentModelCollection result = new ContentModelCollection();
            ContentModelCollection cmc = GetAllContentModel();
            foreach (ContentModel c in cmc)
            {
                if (c.Type == type)
                {
                    result.Add(c);
                }
            }
            return result;
        }
        /// <summary>
        /// 获取所有模型集合
        /// </summary>
        /// <returns></returns>
        public static ContentModelCollection GetContentModel()
        {
            ContentModelCollection cmc = GetAllContentModel();

            return cmc;
        }
        /// <summary>
        /// 获取索引对象模型
        /// </summary>
        /// <returns>对象模型索引</returns>
        public static ContentModel GetContentModelByName(string name)
        {
            ContentModel model = new ContentModel();

            ContentModelCollection contentModels = GetAllContentModel();

            if (contentModels != null && contentModels.Count > 0)
            {
                model = contentModels[name];
            }
            return model;
        }
        /// <summary>
        /// 序列化对象模型集合
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool SaveContentModel(ContentModelCollection collection)
        {
            return SerializationHelper.Save(collection, ContentModelConfigPath);
        }

        /// <summary>
        ///  保存对象模型
        /// </summary>
        /// <param name="modelinfo">对象模型</param>
        /// <returns></returns>
        public static bool SaveContentModel(ContentModel model)
        {
            ContentModelCollection contentModels = ModelHelper.GetAllContentModel();
            if (contentModels == null)
            {
                contentModels = new ContentModelCollection();
            }
            else
            {
                //存在该对象名称，则保存，否则新建
                if (contentModels[model.Name] != null)
                {
                    contentModels[model.Name] = model;
                }
                else
                {
                    contentModels.Add(model);
                }
            }

            return SaveContentModel(contentModels);

        }

        #endregion

        #region 内容模型

        /// <summary>
        /// 序列化一个对象模型
        /// </summary>
        /// <param name="modelInfo">内容模型</param>
        /// <param name="fileName">名称</param>
        /// <returns></returns>
        public static bool SaveModelInfo(ModelInfo modelInfo, string fileName)
        {
            fileName = GetModelInfoPath(fileName);
            if (modelInfo != null)
            {
                foreach (We7DataColumn dc in modelInfo.DataSet.Tables[0].Columns)
                {
                    if (!String.IsNullOrEmpty(dc.Name))
                        dc.Name = dc.Name.Trim();
                }
            }
            return SerializationHelper.Save(modelInfo, fileName);
        }

        /// <summary>
        /// 保存模型信息
        /// </summary>
        /// <param name="modelInfo">内容模型</param>
        /// <returns></returns>
        public static bool SaveModelInfo(ModelInfo modelInfo)
        {
            string fileName = GetModelInfoPath(modelInfo.ModelName);
            return SerializationHelper.Save(modelInfo, fileName);
        }

        /// <summary>
        /// 根据名称获取对象模型
        /// </summary>
        /// <param name="name">模型名称</param>
        /// <returns></returns>
        public static ModelInfo GetModelInfoByName(string name)
        {
            //根据ModelInfo的名称获取路径
            string fileName = GetModelInfoPath(name);

            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            ModelInfo modelInfo = SerializationHelper.Load(typeof(ModelInfo), fileName)
                                as ModelInfo;
            if (modelInfo != null)
            {
                return modelInfo;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 删除内容模型
        /// update{
        /// date:2010-10-10,
        /// author:Brian.Gou,
        /// Description:同步删除内容模型产生的各种残留文件及数据
        /// }
        /// </summary>
        /// <param name="name">内容模型名称</param>
        /// <param name="msg">error message</param>
        /// <returns></returns>
        public static bool DeleteContentModel(string name, ref string msg)
        {
            /*
             * 1:remove Web/Models/ContentModel.config xml node,
             * 2:delete Models/System/model.xml 
             */
            if (!File.Exists(ContentModelConfigPath))
            {
                return false;
            }
            bool b = XmlHelper.DeleteXmlNode(ContentModelConfigPath, "//ContentModel[@name='" + name + "']");
            if (b)
            {
                //删除节点成功 再删除 对应文件
                string modelPhysicalPath = GetModelInfoPath(name);
                if (File.Exists(modelPhysicalPath))
                {
                    try
                    {
                        File.Delete(modelPhysicalPath);
                    }
                    catch
                    {
                        msg += "删除" + modelPhysicalPath + "失败\r\n"; ;
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 删除字段节点(同时删除相关节点)
        /// </summary>
        /// <param name="modelName">模型名称</param>
        /// <param name="fieldname">字段名称</param>
        /// <returns></returns>
        public static bool DeleteModelField(string modelName, string fieldname)
        {
            bool result = false;

            string modelinfoPath = GetModelInfoPath(modelName);

            result = XmlHelper.DeleteXmlNode(modelinfoPath, "//dataColumn[@name='" + fieldname + "']");
            if (result)
            {
                //级联删除
                //TODO::需要确认
                XmlHelper.DeleteXmlNode(modelinfoPath, "//control[@name='" + fieldname + "']");
                XmlHelper.DeleteXmlNode(modelinfoPath, "//column[@name='" + fieldname + "']");

            }
            return result;
        }
        #endregion

        #region private

        //获取保存的对象模型路径
        public static string GetModelInfoPath(string name)
        {
            return GetModelPath(name);
        }

        //获取默认模型路径
        private static string GetDefaultModelPath(string name)
        {
            return GetModelInfoPath(name);
        }
        #endregion

        /// <summary>
        /// 获取数据字典
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllDataColumn(string name)
        {
            Dictionary<string, string> dics = new Dictionary<string, string>();

            ModelInfo modelInfo = GetModelInfoByName(name);

            if (modelInfo != null && modelInfo.DataSet != null
                && modelInfo.DataSet.Tables.Count > 0 && modelInfo.DataSet.Tables[0].Columns.Count > 0)
            {
                for (int i = 0; i < modelInfo.DataSet.Tables[0].Columns.Count; i++)
                {
                    dics.Add(modelInfo.DataSet.Tables[0].Columns[i].Name, modelInfo.DataSet.Tables[0].Columns[i].Label);
                }
            }
            return dics;
        }

        public static Dictionary<string, ColumnInfo> GetAllListColumn(string name, string panelName)
        {
            Dictionary<string, ColumnInfo> dics = new Dictionary<string, ColumnInfo>();

            ModelInfo modelInfo = GetModelInfoByName(name);

            if (modelInfo != null && modelInfo.Layout.Panels[panelName].ListInfo != null && modelInfo.Layout.Panels[panelName].ListInfo.Groups[0].Columns.Count > 0)
            {
                for (int i = 0; i < modelInfo.Layout.Panels[panelName].ListInfo.Groups[0].Columns.Count; i++)
                {
                    dics.Add(modelInfo.Layout.Panels[panelName].ListInfo.Groups[0].Columns[i].Name, modelInfo.Layout.Panels[panelName].ListInfo.Groups[0].Columns[i]);
                }
            }
            return dics;
        }

        public static Dictionary<string, string> GetAllControls()
        {
            string path = HttpContext.Current.Server.MapPath(ModelControlsIndex);
            Dictionary<string, string> dics = new Dictionary<string, string>();
            if (File.Exists(path))
            {
                XmlNodeList nodes = XmlHelper.GetXmlNodeList(path, "//control");
                if (nodes != null && nodes.Count > 0)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        dics.Add(nodes[i].Attributes["type"].Value, nodes[i].Attributes["label"].Value);
                    }
                }

            }
            return dics;
        }

        public static string GetControlLabel(string controlType)
        {
            string path = HttpContext.Current.Server.MapPath(ModelControlsIndex);
            string label = "未知";
            if (File.Exists(path))
            {
                XmlAttribute attr = XmlHelper.GetXmlAttribute(path, "/controls/control[@type='" + controlType.Trim() + "']", "label");
                if (attr != null)
                {
                    label = attr.Value;
                }
            }
            return label;
        }

        /// <summary>
        /// 取得模型的Value值
        /// </summary>
        /// <param name="modename"></param>
        /// <returns></returns>
        public static int GetModelValue(string modename)
        {
            if (!String.IsNullOrEmpty(modename))
            {
                ContentModelCollection cmc = GetAllContentModel();
                foreach (ContentModel c in cmc)
                {
                    if (modename.Equals(c.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        return c.Value;
                    }
                }
            }
            return 0;
        }

        public static ContentModel GetModelByValue(int value)
        {
            ContentModelCollection cmc = GetAllContentModel();
            foreach (ContentModel c in cmc)
            {
                if (c.Value == value)
                    return c;
            }
            return null;
        }

        #region 默认模型

        /// <summary>
        /// 获取默认的模板
        /// </summary>
        /// <returns></returns>
        public static IList<DefaultModel> GetDefaultModels()
        {
            IList<DefaultModel> defaultModels = new List<DefaultModel>();

            XmlNode root = XmlHelper.GetXmlNode(DefaultModelConfigPath, "/models");
            if (root != null && root.ChildNodes != null && root.ChildNodes.Count > 0)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    DefaultModel model = new DefaultModel();
                    if (root.ChildNodes[i].Attributes["name"] != null)
                    {
                        model.Name = root.ChildNodes[i].Attributes["name"].Value;
                    }
                    if (root.ChildNodes[i].Attributes["label"] != null)
                    {
                        model.Label = root.ChildNodes[i].Attributes["label"].Value;
                    }
                    if (root.ChildNodes[i].Attributes["system"] != null)
                    {
                        string system = root.ChildNodes[i].Attributes["system"].Value;
                        if (system == "true" || system == "1" || string.IsNullOrEmpty(system))
                        {
                            model.System = true;
                        }

                    }
                    else
                    {
                        model.System = false;
                    }
                    if (root.ChildNodes[i].Attributes["mapping"] != null)
                    {
                        model.MappingFields = root.ChildNodes[i].Attributes["mapping"].Value;

                    }
                    defaultModels.Add(model);
                }
            }
            return defaultModels;
        }

        /// <summary>
        /// 根据名称获取默认模型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ModelInfo GetDefaultModelInfoByName(string name)
        {
            ModelInfo modelInfo = new ModelInfo();

            string fileName = GetDefaultModelPath(name);
            if (!string.IsNullOrEmpty(fileName))
            {
                modelInfo = SerializationHelper.Load(typeof(ModelInfo), fileName)
                          as ModelInfo;
            }

            return modelInfo;
        }
        #endregion

        #region 前台控件
        /// <summary>
        /// 创建前台控件
        /// </summary>
        /// <param name="model">模型信息</param>
        public static void CreateControls(ModelInfo model)
        {
            if (model.Type == null || model.Type == ModelType.ARTICLE)
            {
                CreateArticleViewControl(model);
                CreateArticleListControl(model);
            }
            else if (model.Type == ModelType.ADVICE)
            {
                CreateAdviceEditControl(model);
                CreateAdviceListControl(model);
                CreateAdviceQueryListControl(model);
                CreateAdviceDetailsControl(model);
            }
        }

        public static void CreateWidgets(ModelInfo model)
        {
            if (model.Type == null || model.Type == ModelType.ARTICLE)
            {
                CreateArticleViewWidget(model);
                CreateArticleListWidget(model);
                CreateArticlePagedListWidget(model);
            }
            else if (model.Type == ModelType.ADVICE)
            {
                CreateAdviceEditWidget(model);
                CreateAdviceListWidget(model);
                CreateAdviceQueryListWidget(model);
                CreateAdviceDetailsWidget(model);
            }
        }

        /// <summary>
        /// 反馈前台录入(自动布局)
        /// </summary>
        /// <param name="model"></param>
        private static void CreateAdviceEditWidget(ModelInfo model)
        {
            //Criteria c = new Criteria(CriteriaType.Equals, "ModelName", model.ModelName);
            //List<AdviceType> adviceList = Assistant.List<AdviceType>(c, null);

            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateWidgetDirectory(model, "Edit");
            helper.Put("model", model);
            helper.Put("controls", model.Layout.Panels["edit"].EditInfo.Controls);
            helper.Put("CurrentDate", DateTime.Now.ToString());
            //helper.Put("adviceType", adviceList);

            helper.Save("WidgetAdvice.vm", path);
        }

        private static void CreateAdviceListWidget(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateWidgetDirectory(model, "List");
            helper.Put("model", model);
            helper.Put("columns", model.DataSet.Tables[0].Columns);
            helper.Put("CurrentDate", DateTime.Now.ToString());

            helper.Save("WidgetAdviceList.vm", path);
        }

        private static void CreateAdviceQueryListWidget(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateWidgetDirectory(model, "QueryList");
            helper.Put("model", model);
            helper.Put("columns", model.DataSet.Tables[0].Columns);
            helper.Put("CurrentDate", DateTime.Now.ToString());

            helper.Save("WidgetAdviceQueryList.vm", path);
        }

        private static void CreateAdviceDetailsWidget(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateWidgetDirectory(model, "View");
            helper.Put("model", model);
            helper.Put("columns", model.DataSet.Tables[0].Columns);
            helper.Put("CurrentDate", DateTime.Now.ToString());

            helper.Save("WidgetAdviceView.vm", path);
        }

        /// <summary>
        /// 生成反馈详细 前台控件
        /// </summary>
        /// <param name="model"></param>
        static void CreateAdviceDetailsControl(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateDirectory(model, "View", "AdviceViewDataControl", "详细");
            helper.Put("model", model);
            helper.Put("columns", model.DataSet.Tables[0].Columns);
            helper.Put("CurrentDate", DateTime.Now.ToString());

            helper.Save("AdviceView.vm", path);
        }

        /// <summary>
        /// 生成反馈列表 前台控件
        /// </summary>
        /// <param name="model"></param>
        static void CreateAdviceListControl(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateDirectory(model, "List", "AdviceListDataControl", "列表");
            helper.Put("model", model);
            helper.Put("columns", model.DataSet.Tables[0].Columns);
            helper.Put("CurrentDate", DateTime.Now.ToString());

            helper.Save("AdviceList.vm", path);
        }

        /// <summary>
        /// 生成反馈类型 前台控件
        /// </summary>
        /// <param name="model"></param>
        static void CreateAdviceQueryListControl(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateDirectory(model, "QueryList", "AdviceQueryListDataControl", "查询列表");
            helper.Put("model", model);
            helper.Put("columns", model.DataSet.Tables[0].Columns);
            helper.Put("CurrentDate", DateTime.Now.ToString());

            helper.Save("AdviceQueryList.vm", path);
        }

        /// <summary>
        /// 生成反馈类型 前台控件
        /// </summary>
        /// <param name="model"></param>
        static void CreateAdviceEditControl(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateDirectory(model, "Edit", "AdviceModelEditDataControl", "录入");
            helper.Put("model", model);
            helper.Put("controls", model.Layout.Panels["edit"].EditInfo.Controls);
            helper.Put("CurrentDate", DateTime.Now.ToString());

            helper.Save("Advice.vm", path);
        }

        /// <summary>
        /// 生成文章类型前台控件
        /// </summary>
        /// <param name="model"></param>
        static void CreateArticleViewControl(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateDirectory(model, "View", "ArticleModelViewDataControl", "详细");
            helper.Put("modelDesc", string.IsNullOrEmpty(model.Desc) ? model.Desc : model.ModelName);
            helper.Put("model", model);
            We7DataColumnCollection dcs = null;
            if (model.Layout.UCContrl.DetailFieldArray == null)
            {
                dcs = model.DataSet.Tables[0].Columns;
            }
            else
            {
                dcs = new We7DataColumnCollection();
                foreach (We7DataColumn dc in model.DataSet.Tables[0].Columns)
                {
                    if (Array.Exists(model.Layout.UCContrl.DetailFieldArray, s => s == dc.Name))
                        dcs.Add(dc);
                }
            }

            helper.Put("columns", dcs);
            helper.Put("CurrentDate", DateTime.Now.ToString());
            if (ModelConfig.IsCreateArticleUC)
            {
                helper.Save("ArticleView.vm", path);
            }
            helper.Save("DbModelDetails.vm", CreateNewFilePath(path, "DB"));
        }

        /// <summary>
        /// 生成文章类型前台部件
        /// </summary>
        /// <param name="model"></param>
        static void CreateArticleViewWidget(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateWidgetDirectory(model, "View");
            helper.Put("modelDesc", string.IsNullOrEmpty(model.Desc) ? model.Desc : model.ModelName);
            helper.Put("model", model);
            We7DataColumnCollection dcs = null;
            if (model.Layout.UCContrl.WidgetDetailFieldArray == null)
            {
                dcs = model.DataSet.Tables[0].Columns;
            }
            else
            {
                dcs = new We7DataColumnCollection();
                foreach (We7DataColumn dc in model.DataSet.Tables[0].Columns)
                {
                    if (Array.Exists(model.Layout.UCContrl.WidgetDetailFieldArray, s => s == dc.Name))
                        dcs.Add(dc);
                }
            }

            helper.Put("columns", dcs);
            helper.Put("CurrentDate", DateTime.Now.ToString());
            string[] tpls = GetWidgetModelViewTemplate();
            CreateWidgets(helper, path, tpls);
        }
        /// <summary>
        /// 生成模型布局
        /// </summary>
        /// <param name="model"></param>
        /// <returns>布局存放路径</returns>
        public static string CreateModelLayout(ModelInfo model)
        {


            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            //string path = CreateDirectory(model, "Edit", "ArticleModelEditDataControl", "录入");
            //string path2 = CreateModelDirectory(model, "Edit", "ArticleModelEditDataControl", "录入");

            string path = GetModelLayoutDirectory(model.ModelName) + "GenerateLayout.ascx";
            FileInfo fi = new FileInfo(path);
            if (!fi.Directory.Exists)
                fi.Directory.Create();

            helper.Put("model", model);
            helper.Put("controls", model.Layout.Panels["edit"].EditInfo.Controls);
            helper.Put("CurrentDate", DateTime.Now.ToString());

            helper.Save("ArticleEditor.vm", path);

            path = String.Format("{0}/{1}/{2}/{3}", ModelConfig.ModelsDirectory, model.GroupName, model.Name, "GenerateLayout.ascx");
            return path;

            //helper.Save("ArticleEditor.vm", path);
            //helper.Save("ArticleEditor.vm", path2);
        }

        /// <summary>
        /// 获取布局存储目录
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public static string GetModelLayoutDirectory(string modelName)
        {
            string[] ss = modelName.Split('.');
            string groupName = modelName.Contains(".") ? ss[0] : "System";
            string name = modelName.Contains(".") ? ss[1] : ss[0];
            return HttpContext.Current.Server.MapPath(String.Format("{0}/{1}/{2}/", ModelConfig.ModelsDirectory, groupName, name));
        }

        /// <summary>
        /// 生成文章类型前台控件
        /// </summary>
        /// <param name="model"></param>
        static void CreateArticleListControl(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateDirectory(model, "List", "ArticleModelListDataControl", "列表");
            helper.Put("modelDesc", string.IsNullOrEmpty(model.Desc) ? model.Desc : model.ModelName);
            helper.Put("model", model);

            We7DataColumnCollection dcs = null;
            if (model.Layout.UCContrl.DetailFieldArray == null)
            {
                dcs = model.DataSet.Tables[0].Columns;
            }
            else
            {
                dcs = new We7DataColumnCollection();
                foreach (We7DataColumn dc in model.DataSet.Tables[0].Columns)
                {
                    if (Array.Exists(model.Layout.UCContrl.DetailFieldArray, s => s == dc.Name))
                        dcs.Add(dc);
                }
            }

            helper.Put("columns", dcs);

            helper.Put("CurrentDate", DateTime.Now.ToString());
            if (ModelConfig.IsCreateArticleUC)
            {
                helper.Save("ArticleList.vm", path);
            }
            helper.Save("DbModelList.vm", CreateNewFilePath(path, "DB"));
        }
        /// <summary>
        /// 生成文章类型前台控件
        /// </summary>
        /// <param name="model"></param>
        static void CreateArticleListWidget(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateWidgetDirectory(model, "List");
            helper.Put("modelDesc", string.IsNullOrEmpty(model.Desc) ? model.Desc : model.ModelName);
            helper.Put("model", model);

            We7DataColumnCollection dcs = null;
            if (model.Layout.UCContrl.WidgetListFields == null)
            {
                dcs = model.DataSet.Tables[0].Columns;
            }
            else
            {
                dcs = new We7DataColumnCollection();
                foreach (We7DataColumn dc in model.DataSet.Tables[0].Columns)
                {
                    if (Array.Exists(model.Layout.UCContrl.WidgetListFieldArray, s => s == dc.Name))
                        dcs.Add(dc);
                }
            }

            helper.Put("columns", dcs);

            helper.Put("CurrentDate", DateTime.Now.ToString());


            string[] tpls = GetWidgetModelListTemplate();
            CreateWidgets(helper, path, tpls);
        }

        /// <summary>
        /// 生成文章类型前台控件
        /// </summary>
        /// <param name="model"></param>
        static void CreateArticlePagedListWidget(ModelInfo model)
        {
            NVelocityHelper helper = new NVelocityHelper(ModelConfig.ModelControlTemplatePath);
            string path = CreateWidgetDirectory(model, "PagedList");
            helper.Put("modelDesc", string.IsNullOrEmpty(model.Desc) ? model.Desc : model.ModelName);
            helper.Put("model", model);

            We7DataColumnCollection dcs = null;
            if (model.Layout.UCContrl.WidgetListFields == null)
            {
                dcs = model.DataSet.Tables[0].Columns;
            }
            else
            {
                dcs = new We7DataColumnCollection();
                foreach (We7DataColumn dc in model.DataSet.Tables[0].Columns)
                {
                    if (Array.Exists(model.Layout.UCContrl.WidgetListFieldArray, s => s == dc.Name))
                        dcs.Add(dc);
                }
            }

            helper.Put("columns", dcs);

            helper.Put("CurrentDate", DateTime.Now.ToString());


            string[] tpls = GetWidgetModelPagedListTemplate();
            CreateWidgets(helper, path, tpls);
        }

        /// <summary>
        /// 创建部件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="refFilePath"></param>
        /// <param name="tpls"></param>
        static void CreateWidgets(NVelocityHelper helper, string refFilePath, string[] tpls)
        {
            foreach (string tpl in tpls)
            {
                string file = Path.GetFileName(tpl);
                string[] ss = file.Split('.');
                string ext = ss.Length > 2 ? ss[1] : "Default";
                helper.Save(file, CreateNewWidgetPath(refFilePath, ext));
            }
        }

        static string CreateNewWidgetPath(string originPath, string ext)
        {
            return originPath.Insert(originPath.LastIndexOf(".") + 1, ext + ".");
        }
        static string CreateNewFilePath(string originPath, string ext)
        {
            return originPath.Insert(originPath.LastIndexOf("."), ext);
        }
        static string[] GetWidgetModelListTemplate()
        {
            string path = HttpContext.Current.Server.MapPath("~/ModelUI/Config/GeneratorTemplate");
            List<string> list = new List<string>();
            list.AddRange(Directory.GetFiles(path, "WidgetModelList.*.vm"));
            return list.ToArray();
        }
        static string[] GetWidgetModelPagedListTemplate()
        {
            string path = HttpContext.Current.Server.MapPath("~/ModelUI/Config/GeneratorTemplate");
            List<string> list = new List<string>();
            list.AddRange(Directory.GetFiles(path, "WidgetModelPagedList.*.vm"));
            return list.ToArray();
        }
        static string[] GetWidgetModelViewTemplate()
        {
            string path = HttpContext.Current.Server.MapPath("~/ModelUI/Config/GeneratorTemplate");
            List<string> list = new List<string>();
            list.AddRange(Directory.GetFiles(path, "WidgetModelView.*.vm"));
            return list.ToArray();
        }

        /// <summary>
        /// 创建部件目录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static string CreateWidgetDirectory(ModelInfo model, string type)
        {
            //string[] ss = model.ModelName.Split('.');
            string modelName = model.ModelName.Replace(".", "_");//ss.Length > 1 ? ss[1] : ss[0];
            string ctrName = modelName + type;
            //如果以下路径更改，请同步更改
            string filePath = HttpContext.Current.Server.MapPath(String.Format(WidgetsSavePath + "{0}/{0}.ascx", ctrName)); //更改目录 update by dl
            FileInfo file = new FileInfo(filePath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            return filePath;
        }

        /// <summary>
        /// 获取内容模型存储文件目录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetWidgetDirectory(ModelInfo model, string type)
        {
            string modelName = model.ModelName.Replace(".", "_");
            string ctrName = modelName + type;
            string filePath = HttpContext.Current.Server.MapPath(String.Format(WidgetsSavePath + "{0}/", ctrName));
            return filePath;
        }

        /// <summary>
        /// 创建前台目录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <param name="refControl"></param>
        /// <returns></returns>
        static string CreateDirectory(ModelInfo model, string type, string refControl, string msg)
        {
            string[] ss = model.ModelName.Split('.');
            string modelName = ss.Length > 1 ? ss[1] : ss[0];
            string ctrName = modelName + type;
            string filePath = HttpContext.Current.Server.MapPath(String.Format("~/Models/{0}/{1}/{2}/Page/{2}.Genarate.ascx", model.GroupName, modelName, ctrName));
            string configPath = HttpContext.Current.Server.MapPath(String.Format("~/Models/{0}/{1}/{2}/DataControl.xml", model.GroupName, modelName, ctrName));
            string refPath = HttpContext.Current.Server.MapPath(String.Format(ModelUCConfigTemplate, refControl));
            FileInfo file = new FileInfo(filePath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            if (!file.Exists)
            {
                FileStream stream = file.Create();
                stream.Close();
                stream.Dispose();
            }
            //为了便于更新，每次执行都覆盖一次
            XmlDocument doc = new XmlDocument();
            doc.Load(refPath);
            doc.DocumentElement.Attributes["desc"].Value = (!String.IsNullOrEmpty(model.Desc) ? model.Desc : (!String.IsNullOrEmpty(model.Label) ? model.Label : ctrName)) + msg;
            doc.DocumentElement.Attributes["name"].Value = (!String.IsNullOrEmpty(model.Label) ? model.Label : (!String.IsNullOrEmpty(model.Desc) ? model.Desc : ctrName)) + msg;
            doc.DocumentElement.Attributes["dir"].Value = ctrName;
            doc.DocumentElement.Attributes["model"].Value = model.ModelName;
            doc.Save(configPath);
            return filePath;
        }

        static string CreateModelDirectory(ModelInfo model, string type, string refControl, string msg)
        {
            string ctrName = "MC" + model.ModelName.Replace(".", "dot") + type;
            string filePath = HttpContext.Current.Server.MapPath(String.Format("~/Models/{0}/{1}/{2}/Page/{2}.Genarate.ascx", model.GroupName, model.Name, ctrName));
            string configPath = HttpContext.Current.Server.MapPath(String.Format("~/Models/{0}/{1}/{2}/DataControl.xml", model.GroupName, model.Name, ctrName));
            string refPath = HttpContext.Current.Server.MapPath(String.Format(ModelUCConfigTemplate, refControl));
            FileInfo file = new FileInfo(filePath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            if (!file.Exists)
            {
                FileStream stream = file.Create();
                stream.Close();
                stream.Dispose();
            }
            //为了便于更新，每次执行都覆盖一次
            XmlDocument doc = new XmlDocument();
            doc.Load(refPath);
            doc.DocumentElement.Attributes["desc"].Value = (!String.IsNullOrEmpty(model.Desc) ? model.Desc : (!String.IsNullOrEmpty(model.Label) ? model.Label : ctrName)) + msg;
            doc.DocumentElement.Attributes["name"].Value = (!String.IsNullOrEmpty(model.Label) ? model.Label : (!String.IsNullOrEmpty(model.Desc) ? model.Desc : ctrName)) + msg;
            doc.DocumentElement.Attributes["dir"].Value = ctrName;
            doc.Save(configPath);
            return filePath;
        }

        #endregion
    }


    //2010-6-3 tedyding
    public partial class ModelHelper
    {
        public static ModelGroupCollection GetModelGroups()
        {
            ModelGroupCollection models = null;
            try
            {
                models = SerializationHelper.Load(typeof(ModelGroupCollection), ModelConfig.ModelGroupIndexFile) as ModelGroupCollection;
                return models;
            }
            catch
            {
                models = new ModelGroupCollection();
                SaveModelGroups(models);
            }
            return models;
        }


        public static bool CreateModelGroup(ModelGroup modelGroup)
        {
            bool result = false;

            //模型组名称不能为空
            if (string.IsNullOrEmpty(modelGroup.Name))
            {
                return result;
            }

            //创建模型组文件夹
            string directoryPath = System.IO.Path.Combine
                (HttpContext.Current.Server.MapPath(ModelConfig.ModelsDirectory), modelGroup.Name);

            if (!Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch (IOException)
                {
                    //logg

                    return result;
                }
            }

            if (!File.Exists(ModelConfig.ModelGroupIndexFile))
            {
                ModelGroupCollection models = new ModelGroupCollection();
                models.AddOrUpdate(modelGroup);
                SerializationHelper.Save(models, ModelConfig.ModelGroupIndexFile);

                result = true;
            }
            else
            {
                ModelGroupCollection models = GetModelGroups();
                models.AddOrUpdate(modelGroup);
                SerializationHelper.Save(models, ModelConfig.ModelGroupIndexFile);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 保存内容模型组
        /// </summary>
        /// <param name="mgc"></param>
        public static void SaveModelGroups(ModelGroupCollection mgc)
        {
            SerializationHelper.Save(mgc, ModelConfig.ModelGroupIndexFile);
        }

        public static bool DeleteModelGroup(string groupName)
        {
            bool result = false;

            //检查正在删除的模板组中是否仍含有模板
            ContentModelCollection collection = ModelHelper.GetAllContentModel();
            for (int i = 0; i < collection.Count; i++)
            {
                string group = collection[i].Name.Split('.')[0];

                if (group == groupName)
                {
                    // 禁止删除含有模板的模板组
                    return false;
                }
            }


            try
            {
                ModelGroupCollection groups = GetModelGroups();

                groups.Remove(groups[groupName]);

                bool save = SerializationHelper.Save(groups, ModelConfig.ModelGroupIndexFile);
                save = SerializationHelper.Save(collection, ModelConfig.ModelIndexFile);
                if (save)
                {
                    //删除文件夹
                    // 注意，同时删除子文件夹
                    Directory.Delete(HttpContext.Current.Server.MapPath(Path.Combine(ModelConfig.ModelsDirectory, groupName)), true);
                }
                result = true;
            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        }

        public static bool SaveModelInfo(ModelInfo modelInfo, string groupName, string fileName)
        {
            string fileDir = Path.Combine(HttpContext.Current.Server.MapPath(ModelConfig.ModelsDirectory), groupName);

            string filePath = Path.Combine(fileDir, fileName + ".xml");

            return SerializationHelper.Save(modelInfo, filePath);
        }


        /// <summary>
        /// 根据名称获取对象模型
        /// </summary>
        /// <param name="name">模型名称</param>
        /// <returns></returns>
        public static ModelGroup GetModelGroupByName(string group)
        {
            ModelGroupCollection groupCollection = GetModelGroups();

            if (groupCollection != null && groupCollection.Count > 0)
            {
                return groupCollection[group];
            }
            return null;
        }


        public static IList<string> GetDataField(string table)
        {
            IList<string> fileds = new List<string>();

            XmlNode node = XmlHelper.GetXmlNode(HttpContext.Current.Server.MapPath(CDPath), "//Object[@table='" + table + "']");

            if (node != null && node.HasChildNodes)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {

                    var name = node.ChildNodes[i].Attributes["name"].Value;
                    if (!Isignore(name))
                    {
                        fileds.Add(name);
                    }
                }
            }
            return fileds;
        }

        private static string[] IngoreWords = { "ModelName", "TableName", "ModelConfig", "ModelSchema" };
        private static bool Isignore(string value)
        {
            bool result = false;
            for (int i = 0; i < IngoreWords.Length; i++)
            {
                if (value == IngoreWords[i])
                {
                    result = true;
                    return result;
                }
            }

            return result;
        }


        /// <summary>
        /// 根据模型名称获取模型组名
        /// </summary>
        /// <param name="modelName">模型名</param>
        /// <returns></returns>
        public static string CovertModelGroupName(string modelName)
        {
            string group = string.Empty;

            string[] temp = modelName.Split(new char[] { '.' });

            if (!string.IsNullOrEmpty(temp[0]))
            {
                ModelGroupCollection groups = GetModelGroups();

                if (groups != null && groups[temp[0]] != null)
                {
                    group = groups[temp[0]].Label;
                }
                else
                {
                    group = temp[0];
                }
            }

            return group;
        }
    }
}
