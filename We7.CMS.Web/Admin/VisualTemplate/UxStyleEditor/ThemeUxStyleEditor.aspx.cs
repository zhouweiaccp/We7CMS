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

namespace We7.CMS.Web.Admin.VisualTemplate
{
    public partial class ThemeUxStyleEditor : BasePage
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
        protected string UxStyleCssFile
        {
            get
            {
                return Server.MapPath(UxStyleCssUrl);
            }
        }

        /// <summary>
        /// 样式文件URL
        /// </summary>
        protected string UxStyleCssUrl
        {
            get
            {
                return string.Format("/Widgets/Themes/{0}/Style.css", GeneralConfigs.GetConfig().Theme);
            }
        }

        /// <summary>
        /// 是否为成功后跳转
        /// </summary>
        protected bool IsSuccessBack
        {
            get
            {
                return Request.Url.AbsoluteUri.IndexOf("#success") > -1;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {   
                BindData();
            }
        }
       

        void BindData()
        {
            BindContent();
        }

        void BindContent()
        {

            if (FileHelper.Exists(UxStyleCssFile))
            {
                msgLabel.Text = "编辑文件：" + UxStyleCssUrl;

                string fileContent = FileHelper.ReadFileWithLine(UxStyleCssFile, new UTF8Encoding(true));
                CtrCodeTextBox.Text = fileContent;
            }
            else
            {
                msgLabel.Text = UxStyleCssUrl + "不存在！";
            }
        }
                
        /// <summary>
        /// 保存css
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (FileHelper.Exists(UxStyleCssFile) && !string.IsNullOrEmpty(editorValue.Value))
            {
                string msg = FileHelper.WriteFileWithEncoding(UxStyleCssFile, editorValue.Value, FileMode.Append, new UTF8Encoding(true));
                if (string.IsNullOrEmpty(msg))
                {
                    Response.Redirect(Request.Url.AbsoluteUri+"#success");
                }
                msgLabel.Text = msg;
            }
        }
        
    }
}
