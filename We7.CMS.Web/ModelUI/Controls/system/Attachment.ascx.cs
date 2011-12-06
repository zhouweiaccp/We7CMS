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
using System.IO;
using We7.Model.Core.UI;
using We7.CMS;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.Model.UI.Controls;

namespace CModel.Controls.system
{
    public partial class Attachment : We7FieldControl
    {
        public override void InitControl()
        {
            AttachUpload.CssClass = Control.CssClass;
            if (!String.IsNullOrEmpty(Control.Width))
            {
                AttachUpload.Width = Unit.Parse(Control.Width);
            }
            if (!String.IsNullOrEmpty(Control.Height))
            {
                AttachUpload.Height = Unit.Parse(Control.Height);
            }

            Article a = new Article() { ID = ArticleID };
            DialogPath = a.AttachmentUrlPath + "/Attachment";
            BindDataList();
        }
        public override object GetValue()
        {
            return TxtPath.Text; ;
        }
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AttachUploadButton_Click(object sender, EventArgs e)
        {
            string fileName = Path.GetFileName(AttachUpload.FileName); ;
            string pathAbsolute = Server.MapPath(DialogPath + "/" + fileName);
            //创建附件文件夹
            IsDirExists(DialogPath);
            AttachUpload.PostedFile.SaveAs(pathAbsolute);
            BindDataList();
        }
        /// <summary>
        /// 绑定指定目录下的文件到ReptImgList
        /// </summary>
        /// <param name="directory"></param>
        protected void BindDataList()
        {
            ArrayList list = new ArrayList();
            DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(DialogPath));
            try
            {
                foreach (FileInfo f in di.GetFiles())
                {
                    string pathrelatively = DialogPath + "/" + f.Name;
                    list.Add(f.Name);
                    if (TxtPath.Text == "")
                    {
                        TxtPath.Text = pathrelatively;
                    }
                    else
                    {
                        TxtPath.Text = TxtPath.Text + "|" + pathrelatively;
                    }
                }
            }
            catch
            {
                //对应目录下没有文件
            }
            ReptImgList.DataSource = list;
            ReptImgList.DataBind();
        }

        /// <summary>
        /// 文章附件目录相对路径
        /// </summary>
        public string DialogPath { get; set; }
        /// <summary>
        /// 删除一个已经上传的文件
        /// </summary>
        /// <param name="source"></param>-
        /// <param name="e"></param>
        protected void BtnDel_Click(object source, RepeaterCommandEventArgs e)
        {
            DeleteFile(DialogPath + "/" + e.CommandName);
            BindDataList();

        }
    }
}