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
using We7.CMS.Module.VisualTemplate.Models;
using System.Collections.Generic;

namespace We7.CMS.Web.Admin.VisualTemplate
{
    public partial class TestFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SettingFile settings=new SettingFile();

            for (int i = 0; i < 10; i++)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("name", "名称" + i.ToString());
                dic.Add("img", "img" + i.ToString());

                settings.Items.Add(dic);
                
            }
            SettingFileService.SaveFile("c:\\1.xml", settings);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var settings = SettingFileService.GetSettings("c:\\1.xml");

            var json = settings.ToJson();

            Response.Write(json);
        }
    }
}
