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
using We7.Model.Core;
using We7.Model.UI.Panel.system;

namespace We7.CMS.Web.Admin.Addins
{
    public partial class ModelListNoMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string model = Request["model"];
            if (String.IsNullOrEmpty(model))
            {
                Trace.Warn(String.Format("{0}::\r\n内容模型的model与panel为空!当前mode为{1}",DateTime.Now,model));
            }
            ((ListPanel)ucList).ModelName = model;
            ((ListPanel)ucList).PanelName = "list";
            ModelInfo info = ModelHelper.GetModelInfo(model);
            PagePathLiteral.Text = info.Label + "管理>" + info.Label+"列表";
            NameLabel.Text = info.Label + "管理";
        }
    }
}
