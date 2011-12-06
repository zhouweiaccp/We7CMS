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
using We7.CMS.Common.Enum;
using We7.Model.UI.Panel.system;

namespace We7.CMS.Web.Admin.Addins
{
    public partial class ModelEditor : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                if (Request["notiframe"] != null && Request["notiframe"].ToString() == "1")
                    return MasterPageMode.FullMenu;
                else
                    return MasterPageMode.NoMenu;
            }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //以下方法会造成扎线程中止
            //if (String.IsNullOrEmpty(Request[We7.Model.Core.UI.Constants.EntityID]))
            //{
            //    Response.Redirect(We7Helper.AddParamToUrl(Request.RawUrl, We7.Model.Core.UI.Constants.EntityID, We7Helper.CreateNewID()));
            //}

            string model = Request["model"];
            ((EditorPanel)ucEditor).ModelName = model;
            ((EditorPanel)ucEditor).PanelName = "edit";
            ModelInfo info = ModelHelper.GetModelInfo(model);
            PagePathLiteral.Text = info.Label + "管理>发布" + info.Label;
            NameLabel.Text = info.Label + "管理";
        }
    }
}
