using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Model.Core;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.ContentModel
{
    public partial class Model : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void bttnReCreateIndex_Click(object sender, EventArgs e)
        {
            try
            {
                ModelHelper.ReCreateModelIndex();
                Msg.ShowMessage("重建索引成功");
            }
            catch (Exception ex)
            {
                Msg.ShowError("重建索引失败,错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// ContentModel Type
        /// </summary>
        public string modelType
        {
            get
            {
                string t = We7Request.GetQueryString("modelType");
                if (t.Length == 0)
                    return ModelType.ARTICLE.ToString();
                else
                    return t;
            }
        }

        /// <summary>
        /// 模型名称（区别用户和信息）
        /// </summary>
        public string ModelTypeName
        {
            get
            {
                switch (modelType.ToUpper())
                {
                    case "ACCOUNT": return "用户模型";
                    case "ARTICLE": return "信息模型";
                    default: return "反馈模型";
                }
            }
        }

        /// <summary>
        /// 获取所有内容模型
        /// </summary>
        /// <returns>所有内容模型集合</returns>
        public ContentModelCollection GetModelCollection()
        {
            ModelType type = (ModelType)Enum.Parse(typeof(ModelType), modelType);
            ContentModelCollection collection = ModelHelper.GetContentModel(type);
            return collection;
        }

        /// <summary>
        /// 转换模型类型为文字
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <returns></returns>
        public string ConvertModelType(ModelType modelType)
        {
            string modelTypeName = string.Empty;
            switch (modelType)
            {
                case ModelType.ARTICLE:
                    modelTypeName = "信息类型";
                    break;
                case ModelType.ADVICE:
                    modelTypeName = "反馈类型";
                    break;
                case ModelType.ACCOUNT:
                    modelTypeName = "用户类型";
                    break;
                default:
                    modelTypeName = "信息类型";
                    break;
            }

            return modelTypeName;
        }

        /// <summary>
        /// 根据模型名称获取模型组名
        /// </summary>
        /// <param name="modelName">模型名</param>
        /// <returns></returns>
        public string CovertModelGroupName(string modelName)
        {
            return ModelHelper.CovertModelGroupName(modelName);
        }

        protected void Search(object sender, EventArgs e)
        {
            throw new NotImplementedException("未实现");
        }
    }
}
