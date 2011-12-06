using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework.Util;
using System.Text;
using System.IO;
using We7.Model.Core;

namespace We7.CMS.Web.Admin.ContentModel
{
    public partial class AdvanceEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetValue();
            }
        }

        //设置初始值
        private void SetValue()
        {
          string modelname = RequestHelper.Get<string>("modelname","");    
          string file=   File.ReadAllText(GetModelInfoPath(modelname), Encoding.UTF8);
          ModelXMLTextBox.Text = file;
             
        }

        //获取模型路径
        private static string GetModelInfoPath(string name)
        {
            return ModelHelper.GetModelPath(name);
            //if (name.IndexOf(".") > 0)
            //{
            //    string[] tempPath = name.Split(new char[] { '.' });
            //    //ModelConfig.ModelsDirectory
            //    return string.Format(@"~/Models/{0}/{1}.xml", tempPath[0], tempPath[1]);

            //}
            //else
            //{
            //    return string.Format(@"~/Models/System/{0}.xml", name);
            //}
        }

        public void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                string modelname = RequestHelper.Get<string>("modelname", "");
                File.WriteAllText(GetModelInfoPath(modelname), this.ModelXMLTextBox.Text);
                this.Messages.ShowMessage("保存成功!");
            }
            catch (Exception ex)
            {
                this.Messages.ShowError("保存失败!出错:"+ex.Message);
            }
        }
    }
}
