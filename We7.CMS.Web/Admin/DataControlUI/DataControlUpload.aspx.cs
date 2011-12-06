using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace We7.CMS.Web.Admin
{
    public partial class DataControlUpload : BasePage
    {

        protected override void Initialize()
        {
            Uploader = new DataControlUploader();
            Uploader.TemporaryPath = Path.GetTempPath();
            Uploader.WebRoot = Server.MapPath(".");
        }

        public DataControlUploader Uploader
        {
            get { return (DataControlUploader)ViewState["$VS_STORED_FILE"]; }
            set { ViewState["$VS_STORED_FILE"] = value; }
        }

        void ShowMessage(string m)
        {
            MessageLabel.Text = m;
            MessagePanel.Visible = true;
        }
        
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DataControlFileUpload.HasFile)
                {
                    ShowMessage("请先选择一个控件数据包文件（.zip）。");
                }
                else
                {
                    string fn = DataControlFileUpload.FileName;
                    if (String.Compare(Path.GetExtension(fn), ".zip", true) != 0)
                    {
                        ShowMessage("控件数据包文件必须是zip文件。");
                    }
                    else
                    {
                        string fileName = Path.GetTempFileName() + ".zip";
                        Uploader.FileName = fileName;
                        DataControlFileUpload.SaveAs(Uploader.FileName);
                        Uploader.Process();
                        DataControlsGridView.DataSource = Uploader.Controls;
                        DataControlsGridView.DataBind();

                        ReviewPanel.Visible = true;
                        UploadPanel.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                Uploader.Deploy();
                Uploader.Clean();

                //记录日志
                string content = string.Format("上传了数据控件“{0}”", DataControlFileUpload.FileName);
                AddLog( "上传数据控件", content);

                Response.Redirect("DataControls.aspx");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}
