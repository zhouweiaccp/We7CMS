using System;
using System.Collections.Generic;
using System.Web;
using We7.Model.Core;
using System.IO;
using We7.CMS.WebControls.Core;
using We7.Model.Core.Data;
using We7.CMS.Accounts;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Util;
using Newtonsoft.Json;
using Thinkment.Data;

namespace We7.CMS.Web.Admin.ContentModel.ajax
{
    /// <summary>
    /// Summary description for ContentModel1
    /// </summary>
    public class ContentModel1 : IHttpHandler
    {
        protected HelperFactory HelperFactory
        {
            get
            {
                return We7.Framework.HelperFactory.Instance;
            }
        }

        ObjectAssistant assistant;
        /// <summary>
        /// 当前Helper的数据访问对象
        /// </summary>
        protected ObjectAssistant Assistant
        {
            get
            {
                if (assistant == null)
                {
                    assistant = HelperFactory.Assistant;
                }
                return assistant;
            }
            set { assistant = value; }
        }

        /// <summary>
        /// ProcessRequest
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = We7Request.GetQueryString("action");
            string result = "";
            switch (action)
            {
                case "DeleteModel": result = DeleteModel();
                    break;
            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 删除模型
        /// </summary>
        /// <returns></returns>
        public string DeleteModel()
        {
            string modelName = We7Request.GetQueryString("model");
            string msg = "";
            bool success = true;
            //2011-10-10 取消生成控件
            //ModelHelper.CreateControls(modelInf);


            /*
             * TODO:
              以下这些代码应该集成到ModelHelper.DeleteContentModel中，但是下列代码中需要一些方法是调用We7.CMS.WebControls.Core.BaseControlHelper中的方法，这个类物理文件存储在We7.CMS.UI里面.
             */

            ModelInfo modelInf = ModelHelper.GetModelInfo(modelName);
            //反馈模型删除前需要解除类型的绑定
            if (modelInf.Type.ToString().ToLower().Equals("advice"))
            {
                Criteria c = new Criteria(CriteriaType.Equals, "ModelName", modelInf.ModelName);
                List<AdviceType> adviceList = Assistant.List<AdviceType>(c, null);

                if (adviceList != null && adviceList.Count > 0)
                {
                    success = false;
                    msg = "当前模型尚有绑定的反馈类型，请先解除绑定再进行删除！";
                    return "{\"success\":\"" + success.ToString().ToLower() + "\",\"msg\":\"" + msg + "\"}";
                }
            }
            //布局控件
            string layoutPath = ModelHelper.GetModelLayoutDirectory(modelInf.ModelName);
            //部件
            int widgetCount = 0;
            string viewPath = ModelHelper.GetWidgetDirectory(modelInf, "View");
            string listPath = ModelHelper.GetWidgetDirectory(modelInf, "List");
            string pageListPath = ModelHelper.GetWidgetDirectory(modelInf, "PagedList");
            try
            {
                if (Directory.Exists(layoutPath))
                    Directory.Delete(layoutPath, true);
                if (Directory.Exists(viewPath))
                {
                    Directory.Delete(viewPath, true);
                    widgetCount++;
                }
                if (Directory.Exists(listPath))
                {
                    Directory.Delete(listPath, true);
                    widgetCount++;
                }
                if (Directory.Exists(pageListPath))
                {
                    Directory.Delete(pageListPath, true);
                    widgetCount++;
                }
                if (widgetCount > 0)
                {
                    //重建部件索引
                    BaseControlHelper ctrHelper = new BaseControlHelper();
                    ctrHelper.CreateIntegrationIndexConfig();
                    ctrHelper.CreateWidegetsIndex();
                }
            }
            catch
            {
                msg += "部分文件夹删除失败，请手动删除以下文件夹：\r\n";
                msg += "1:" + layoutPath + "\r\n";
                msg += "2:" + viewPath + "\r\n";
                msg += "3:" + listPath + "\r\n";
                msg += "4:" + pageListPath + "\r\n";
                success = false;
            }

            //删除数据库表
            try
            {
                DataBaseHelperFactory.Create().DeleteTable(modelInf);
            }
            catch
            {
            }
            //检查左侧菜单
            MenuHelper MenuHelper = HelperFactory.Instance.GetHelper<MenuHelper>();
            MenuItem item = MenuHelper.GetMenuItemByTitle(modelInf.Label + "管理");
            if (item != null && !string.IsNullOrEmpty(item.ID))
            {
                try
                {
                    MenuHelper.DeleteMenuItem(item.ID);
                }
                catch
                {
                    success = false;
                    msg += "左侧菜单删除失败，请在【后台菜单管理里手动删除】";
                }
            }
            //检查是否关联了Channel
            List<Channel> channelList = HelperFactory.Instance.GetHelper<ChannelHelper>().
                                                                 GetChannelByModelName(modelInf.ModelName);
            if (channelList != null && channelList.Count > 0)
            {
                try
                {
                    foreach (Channel c in channelList)
                    {
                        c.ModelName = "";
                        HelperFactory.Instance.GetHelper<ChannelHelper>().UpdateChannel(c);
                    }
                }
                catch(Exception ex)
                {
                    success = false;
                    msg += "内容模型已关联栏目，取消关联失败，请手动取消栏目和内容模型的绑定关系!错误消息：" + ex.Message;
                }
            }

            //删除XML节点及文件
            success = !ModelHelper.DeleteContentModel(modelName, ref msg) ? false : success;
            string strResult = "{\"success\":\"" + success.ToString().ToLower() + "\",\"msg\":\"" +Utils.JsonCharFilter(msg) + "\"}";
            //return JavaScriptConvert.SerializeObject(strResult).Replace("null", "\"\""); ;
            return strResult;
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