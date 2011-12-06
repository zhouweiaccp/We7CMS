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
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.CMS.Config;
using We7.Framework.Config;
using We7.Framework.Util;
using System.Text;
using System.IO;
using We7.Model.Core;
using We7.Model.Core.Config;

namespace We7.CMS.Web.Admin.ContentModel
{
    public partial class UxStyleEditor : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        /// <summary>
        /// 样式文件地址
        /// </summary>
        protected string UxStyleCssFile;
        /// <summary>
        /// script
        /// </summary>
        protected string strScript;

        private ModelInfo modelInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindContent();
            }
        }



        void BindContent()
        {
            string type = RequestHelper.Get<string>("type");
            string modelName = RequestHelper.Get<string>("model");
            if (type.Length > 0 && modelName.Length > 0)
            {
                modelInfo = ModelHelper.GetModelInfo(modelName);
                if (modelInfo != null)
                {
                    UxStyleCssFile = String.Format("{0}/{1}/{2}/{3}", ModelConfig.ModelsDirectory.Replace("~/", "/"), modelInfo.GroupName, modelInfo.Name, "GenerateLayout."+type+".css");

                    if (FileHelper.Exists(Server.MapPath(UxStyleCssFile)))
                    {
                        string fileContent = FileHelper.ReadFileWithLine(Server.MapPath(UxStyleCssFile), new UTF8Encoding(true));
                        CtrCodeTextBox.Text = fileContent;
                    }
                    msgLabel.Text = "正在编辑文件：" + UxStyleCssFile;
                }
                else
                {
                    Response.Write("内容模型不存在或已被删除！");
                    Response.End();
                }
            }
            else
            {
                Response.Write("错误的访问路径！");
                Response.End();
            }
        }

        /// <summary>
        /// 保存css
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string type = RequestHelper.Get<string>("type");
                string modelName = RequestHelper.Get<string>("model");
                modelInfo = ModelHelper.GetModelInfo(modelName);
                if (modelInfo != null && modelInfo.Layout != null && modelInfo.Layout.Panels.Count > 0)
                {
                    UxStyleCssFile = String.Format("{0}/{1}/{2}/{3}", ModelConfig.ModelsDirectory.Replace("~/", "/"), modelInfo.GroupName, modelInfo.Name, "GenerateLayout." + type + ".css");

                    if (!string.IsNullOrEmpty(editorValue.Value))
                    {
                        string msg = FileHelper.WriteFileWithEncoding(Server.MapPath(UxStyleCssFile), editorValue.Value, FileMode.Append, new UTF8Encoding(true));


                        EditInfo info = modelInfo.Layout.Panels["edit"].EditInfo;
                        if (info != null)
                            if (string.IsNullOrEmpty(msg))
                            {

                                switch (type)
                                {
                                    case "EditCss":
                                        info.EditCss = UxStyleCssFile;
                                        break;
                                    case "ViewerCss":
                                        info.ViewerCss = UxStyleCssFile;
                                        break;
                                    case "UcCss":
                                        info.UcCss = UxStyleCssFile;
                                        break;
                                }

                                modelInfo.Layout.Panels["edit"].EditInfo = info;
                                ModelHelper.SaveModelInfo(modelInfo, modelInfo.ModelName);

                                strScript = "window.parent.CloseChild('已保存!');";
                            }
                        msgLabel.Text = msg;
                    }
                }
            }
            catch(Exception ex)
            {
                strScript = "window.parent.CloseChild('添加失败,错误消息：" + ex.Message + "!');";
            }
         
        }

    }
}
