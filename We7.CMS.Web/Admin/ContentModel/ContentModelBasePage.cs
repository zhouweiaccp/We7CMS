using System;
using System.Collections.Generic;
using System.Web;
using We7.Model.Core;
using We7.Framework;

namespace We7.CMS.Web.Admin.ContentModel
{
    /// <summary>
    /// 数据传递方式
    /// </summary>
    public enum ActionType
    {
        Add,
        Edit,
        Delete,
        List
    }
    /// <summary>
    /// 内容模型基类
    /// </summary>
    public class ContentModelBasePage : BasePage
    {
        

        #region Request & Propertys

        /// <summary>
        /// 操作类型
        /// </summary>
        public ActionType Action
        {
            get
            {
                return Lazy.New<ActionType>(GetAction);
            }
        }

        private ActionType GetAction()
        {
            string action = Request["action"];
            //默认为列表
            ActionType type = ActionType.List;
            if (!string.IsNullOrEmpty(action))
            {
                switch (action.ToLower().Trim())
                {
                    case "add":
                        type = ActionType.Add;
                        break;
                    case "edit":
                        type = ActionType.Edit;
                        break;
                    case "delete":
                        type = ActionType.Delete;
                        break;
                    case "list":
                        type = ActionType.List;
                        break;
                    default:
                        type = ActionType.List;
                        break;
                }
            }

            return type;
        }

        /// <summary>
        /// 模型名称(Request)
        /// </summary>
        public string ModelName
        {
            get
            {
                return Request["modelname"];

            }
        }

        /// <summary>
        /// 内容模型
        /// </summary>
        public ModelInfo Model
        {
            get
            {
                return Lazy.New<ModelInfo>(GetModelInfo);
            }
        }

        private ModelInfo GetModelInfo()
        {
            return ModelHelper.GetModelInfoByName(ModelName);
        }
        /// <summary>
        /// 内容模型路径
        /// </summary>
        public string ModelPath
        {
            get
            {
                return Lazy.New<string>(GetModelPath);
            }
        }

        private string GetModelPath()
        {
            return Server.MapPath(ModelHelper.GetModelInfoPath(ModelName));
        }
        #endregion
    }
}
