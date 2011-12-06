using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using We7.CMS;
using We7.CMS.Controls;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin.controls
{
    public partial class Advice_Option : BaseUserControl
    {
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
                return
                    Request["adviceTypeID"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (AdviceTypeID == null || AdviceTypeID == "")
                {
					ExtraProperties.Visible = false;
					#region 被注释的代码

					// 在新建模型时仅需登记这些信息，故无需呈现这些控件
					// 问题hideExtraProperties

					//if (AccountID == "{00000000-0000-0000-0000-000000000000}")
					//{
					//    AdviceCreatedText.Text = "超级管理员";
					//}
					//else
					//{
					//    AdviceCreatedText.Text = AccountHelper.GetAccount(AccountID, new string[] { "LoginName" }).LoginName;
					//}
					//StartTimeText.Text = DateTime.Now.ToString();

					#endregion
                }
                else
                {
					ExtraProperties.Visible = true;
                    //更新反馈模型信息
                    InitializePage();
                }
                if (Request["saved"] != null)
                {
                    Messages.ShowMessage("您已成功创建反馈模型！请点击选项卡“办理流程”、“反馈表单”、“办理权限”进一步设置模型。");
                }
            }
        }
        /// <summary>
        /// 更新时初始化页面数据
        /// </summary>
        void InitializePage()
        {
			// 问题hideExtraProperties

            AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
            if (adviceType != null)
            {
                AdviceNameText.Text = adviceType.Title;
                RemarkText.Text = adviceType.Description;
				StartTimeText.Text = adviceType.CreateDate.ToString();
				AdviceCreatedText.Text = GetAccountNameText(adviceType.AccountID);
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            AdviceType adviceType = new AdviceType();
			adviceType.CreateDate = DateTime.Now;
			adviceType.Title = AdviceNameText.Text.Trim();
            adviceType.Description = RemarkText.Text.Trim();

			if (string.IsNullOrEmpty(adviceType.Title))
			{
				Messages.ShowError("模型名称不能为空");
				return;
			}

            if (AdviceTypeID == null || AdviceTypeID == "")		// 新建
            {
                adviceType.AccountID = AccountID;
                string adviceTypeID = We7Helper.CreateNewID();
                adviceType.ID = adviceTypeID;
                AdviceTypeHelper.AddAdviceType(adviceType);
            }
            else		// 修改
            {
                adviceType.ID = AdviceTypeID;
                AdviceTypeHelper.UpdateAdviceType(adviceType);
                Messages.ShowMessage("" + AdviceNameText.Text + " 模型修改成功!!");
            }
            //记录日志
            string content = string.Format("编辑了模型“{0}”的信息", adviceType.Title);
            AddLog("编辑反馈模型", content);

            if (AdviceTypeID == null || AdviceTypeID == "")
            {
                string rawurl = We7Helper.AddParamToUrl(Request.RawUrl, "saved", "1");
                rawurl = We7Helper.RemoveParamFromUrl(rawurl, "adviceTypeID");
                rawurl = We7Helper.AddParamToUrl(rawurl, "adviceTypeID", We7Helper.GUIDToFormatString(adviceType.ID));
                Response.Redirect(rawurl);
            }
        }

		/// <summary>
		/// 将 AccountID 翻译显示为名称
		/// <remarks>
		/// 讨论：是否可改为基类的 GetAccountName ？
		/// </remarks>
		/// </summary>
		private string GetAccountNameText(string accountID)
		{
			if (accountID == null || accountID == "")
			{
				return string.Empty;
			}
			else if (accountID == "{00000000-0000-0000-0000-000000000000}")
			{
				return "超级管理员";
			}
			else
			{
				return AccountHelper.GetAccount(accountID, new string[] { "LoginName" }).LoginName;
			}
		}
    }
}