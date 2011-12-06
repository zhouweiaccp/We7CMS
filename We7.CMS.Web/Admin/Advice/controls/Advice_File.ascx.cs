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
using We7.CMS.Common;
using We7.Model.Core;
using We7.Framework;

namespace We7.CMS.Web.Admin.controls
{
    public partial class Advice_File : BaseUserControl
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        AdviceType adviceType = new AdviceType();

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializePage();
            }
        }

        /// <summary>
        ///  界面加载
        /// </summary>
        void InitializePage()
        {
            ContentModelCollection cmc = new ContentModelCollection();
            ContentModelCollection orcmc = ModelHelper.GetAllContentModel();
            foreach (We7.Model.Core.ContentModel c in orcmc)
            {
                if (c.Type == ModelType.ADVICE)
                    cmc.Add(c);
            }

            ddlAdviceType.DataSource = cmc;
            ddlAdviceType.DataTextField = "label";
            ddlAdviceType.DataValueField = "name";
            ddlAdviceType.DataBind();
            ddlAdviceType.Items.Insert(0, new ListItem("请选择", ""));
            adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
            if (adviceType != null)
            {
                ConfigNameTextBox.Text = adviceType.Title.ToString();
                ddlAdviceType.SelectedValue = adviceType.ModelName;
            }
        }

        /// <summary>
        /// 保存Config文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            CreateFile();
        }

        /// <summary>
        /// 创建Config文件
        /// </summary>
        /// <param name="path"></param>
        protected void CreateFile()
        {
            if (String.IsNullOrEmpty(ddlAdviceType.SelectedValue))
            {
                Messages.ShowMessage("请选择模型！");
                return;
            }
            if (AdviceTypeID != null && AdviceTypeID != "")
            {
                adviceType.Updated = DateTime.Now;
                adviceType.ID = AdviceTypeID;
                AdviceTypeHelper.UpdateAdviceType(AdviceTypeID, ddlAdviceType.SelectedValue);
            }
            //记录日志
            string content = string.Format("修改反馈模型:“{0}”", adviceType.Title);
            AddLog("反馈模型管理", content);
            Messages.ShowMessage("您成功修改" + ConfigNameTextBox.Text.ToString().Trim() + "模型信息。");
        }
    }
}