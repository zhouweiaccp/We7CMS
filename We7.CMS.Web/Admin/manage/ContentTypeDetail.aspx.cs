using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

using Thinkment.Data;

namespace We7.CMS.Web.Admin
{
    public partial class ContentTypeDetail : BasePage
    {
        /// <summary>
        /// Config文件名
        /// </summary>
        string FileName
        {
            get { return Request["file"]; }
        }

        string CurrentFolder
        {
            get
            {
                string p = Request["folder"];
                if (p == null)
                {
                    p = "\\Config\\ContentModel";
                    ReturnHyperLink.NavigateUrl = "ContentTypeList.aspx";
                }
                 return p;
            }
        }
        protected override void Initialize()
        {
            if (FileName != null)
            {
                SummaryLabel.Text = String.Format("编辑Config文件{0}", FileName);
                string path = Server.MapPath(Path.Combine(CurrentFolder, FileName));
                if (File.Exists(path))
                {
                    Read(Server.MapPath(Path.Combine(CurrentFolder, FileName)));
                }
                else
                {
                    SummaryLabel.Text = "创建一个新的内容模型文件";
                    ConfigNameLabel.Visible = ConfigNameTextBox.Visible = ConfigFileExt.Visible = true;
                    ConfigNameLabel.Text = ConfigNameTextBox.Text = FileName;
                    ConfigNameTextBox.Enabled = false;
                }
            }
            else
            {
                SummaryLabel.Text = "创建一个新的内容模型文件";
                ConfigNameLabel.Visible = ConfigNameTextBox.Visible =ConfigFileExt.Visible= true;
            }
        }
        /// <summary>
        /// 读取Config文件
        /// </summary>
        /// <param name="path"></param>
        public void Read(string path)
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    ConfigTextBox.Text += s + "\r\n";
                }
            }
        }
        /// <summary>
        /// 写入Config文件
        /// </summary>
        /// <param name="path"></param>
        public void Write(string path)
        {
            if(File.Exists(path)) File.SetAttributes(path, FileAttributes.Normal);//修改文件只读属性 
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(ConfigTextBox.Text);
                sw.Flush();
                sw.Close();
            }
            Messages.ShowMessage( "已成功保存配置文件。"+DateTime.Now.ToString());
        }
        /// <summary>
        /// 保存Config文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (FileName != null)
            {
                Write(Server.MapPath(Path.Combine(CurrentFolder, FileName)));

                //记录日志
                string content = string.Format("修改了内容模型文件“{0}”", FileName);
                AddLog("编辑内容模型文件", content);
            }
            else
            {
                CreateFile();
            }
        }
        /// <summary>
        /// 创建Config文件
        /// </summary>
        /// <param name="path"></param>
        protected void CreateFile()
        {
            string path = Server.MapPath(Path.Combine(CurrentFolder, ConfigNameTextBox.Text + ".xml"));
            if (!File.Exists(path))
            {
                Write(Server.MapPath(Path.Combine(CurrentFolder, ConfigNameTextBox.Text + ".xml")));

                //记录日志
                string content = string.Format("创建了内容模型文件“{0}”", ConfigNameTextBox.Text + ".xml");
                AddLog("创建内容模型文件", content);
            }
            else
            {
                Messages.ShowError("已有此文件名，请换一个文件名再试。");
            }
        }
    }
}
