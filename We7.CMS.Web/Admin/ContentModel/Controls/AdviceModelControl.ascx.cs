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
using We7.Model.Core;
using System.IO;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.ContentModel.Controls
{
    public partial class AdviceModelControl : System.Web.UI.UserControl
    {

        AdviceType adviceType = new AdviceType();

        private ModelInfo modelInfo;
        public ModelInfo ModelInfo
        {
            get
            {
                if (modelInfo == null)
                {
                    string modelName = Request["modelname"];
                    if (String.IsNullOrEmpty(modelName))
                        throw new Exception("当前模型不存在");
                    modelInfo = ModelHelper.GetModelInfo(modelName);
                }
                return modelInfo;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                adviceType = new AdviceTypeHelper().GetAdviceType(AdviceTypeID);
            }
        }

        /// <summary>
        /// 获取反馈模型ID
        /// </summary>
        public string AdviceTypeID
        {
            get
            {
                if (Request["adviceTypeID"] != null)
                {
                    return We7Helper.FormatToGUID((string)Request["adviceTypeID"]);
                }
                return Request["adviceTypeID"];
            }
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(adviceType.ModelName))
                {
                    adviceType.Updated = DateTime.Now;
                    adviceType.ID = AdviceTypeID;
                    new AdviceTypeHelper().UpdateAdviceType(AdviceTypeID, ModelInfo.ModelName);
                }

                ModelHelper.CreateControls(ModelInfo);
                ModelHelper.CreateWidgets(ModelInfo);

                Msg.ShowMessage("成功生成前台录入控件UC" + ModelInfo.ModelName.Replace(".", "dot") + "Edit");
            }
            catch (Exception ex)
            {
                Msg.ShowError("生成控件出错:" + ex.Message);
            }
        }
    }
}