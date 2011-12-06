using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Model.Core;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.ContentModel.Controls
{
    public partial class UserCenter_Edit : System.Web.UI.UserControl
    {
        string ModelName
        {
            get
            {
                return Request["modelname"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ModelName))
            {
                We7.Model.Core.ContentModel cm = ModelHelper.GetContentModelByName(ModelName);
                
                FormTitleLiteral.Text = cm.Label + "录入表单自定义";
                FormDesciptionLiteral.Text = cm.Description;
                //显示更新到会员中心复选框
                if (cm.Type == ModelType.ARTICLE)
                {
                    string panel = RequestHelper.Get<string>("panel", "edit");

                    if (panel.ToLower() == "edit")
                    {
                        copyControls.Visible = true;
                    }
                    else
                    {
                        copyControls.Visible = false;
                    }
                }
                else
                {
                    copyControls.Visible = false;
                }
            }
        }
    }
}