using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common.Enum;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.VisualTemplate.HtmlWidgets
{
    public partial class HtmlWidgets : BasePage
    {
        /// <summary>
        /// 母版模式
        /// </summary>
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                Inint();
            } 
        }
        private void Inint()
        {   string controlfile = Request["ctr"];
            if (!string.IsNullOrEmpty(controlfile))
            {
                ViewState["IsNew"] = false;
                WidgetContentTextBox.Value = LoadWidget(controlfile);
            }
            else
            {
                ViewState["IsNew"] = true;
            }
        }
        protected string LoadWidget(string file)
        {
            string result = string.Empty;
            string fullpath = Server.MapPath(file);
            result = FileHelper.ReadFileWithLine(fullpath,new System.Text.UTF8Encoding(true));
            string WidgetDescription = System.Text.RegularExpressions.Regex.Match(result, "Desc[\\s|\\S]*?\"(?<value>[\\s|\\S]*?)\"").Groups["value"].Value; //设置部件名称
            NameTextbox.Text = WidgetDescription;
            string filename = file.Substring(file.LastIndexOf('/') + 1);
            FilenameTextBox.Text = filename.Remove(filename.LastIndexOf('.'));//设置部件文件名称
            string resultWidgetValue = System.Text.RegularExpressions.Regex.Match(result, @"<!--htmlWidgetStart-->(?<value>[\s|\S]*?)<!--HtmlWidgetEnd-->").Groups["value"].Value;  //替换部件Content
            return resultWidgetValue;
        }
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            string widgetName = NameTextbox.Text; //部件description名称
            string fileName = FilenameTextBox.Text;  //部件文件名称
            string widgetValue = WidgetContentTextBox.Value;  //部件内容
            string path = Constants.We7HtmlWidgetsFileFolder;
            string returnPath ="\\"+ Constants.We7HtmlWidgetFolder + "\\" + fileName + "\\" + fileName + ".ascx";
            EnumCreateHtmlWidget result = HtmlWidgetHelper.Scope.CreateCreateHtmlWidget(widgetName, fileName, widgetValue, path, (bool)ViewState["IsNew"]);
            if (EnumCreateHtmlWidget.success == result)
            {
                try
                {
                    SaveButton.Enabled = false;
                    saveAricleButton.Disabled = true;
                    We7.CMS.WebControls.Core.BaseControlHelper Helper = new We7.CMS.WebControls.Core.BaseControlHelper();
                    Helper.CreateWidegetsIndex();
                    Messages.ShowMessage(string.Format("生成部件 {0} 成功！重新部件索引成功！", fileName + ".ascx"));
                    returnPath = returnPath.Replace("\\", "~");
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "<script>Close('" + returnPath + "');</script>");
                }
                catch (Exception ex)
                {
                    SaveButton.Enabled = true;
                    saveAricleButton.Disabled = false;
                    Messages.ShowError(string.Format("生成部件 {0} 成功！重建部件索引失败：{1}", fileName + ".ascx", ex.Message));
                }
            }
            else if (EnumCreateHtmlWidget.repeat == result)
            {
                Messages.ShowError(string.Format("存在相同的文件“{0}”！请更换名称。", fileName));
            }
            else if (EnumCreateHtmlWidget.error == result)
            {
                Messages.ShowError("未知原因引起的错误！请联系管理员。");
            }
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "<script>closeBg();</script>");

        }

    }
}