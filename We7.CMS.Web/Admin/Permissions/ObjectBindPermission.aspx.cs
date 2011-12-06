using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class ObjectBindPermission : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
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

        public string TabID
        {
            get { return Request["tab"]; }
        }

        public string OwnerID
        {
            get { return Request["oid"]; }
        }

        public string OwnerType
        {
            get { return Request["type"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string strTitle = "{0} {1} 在网站 {2} 拥有的 {3} 权限";
            string ownerTitle = OwnerType == "role" ? "角色" : "用户";
            string ownerName = "";
            if (OwnerType == "role")
                ownerName = AccountHelper.GetRole(OwnerID).Name;
            else
                ownerName = AccountHelper.GetAccount(OwnerID, new string[] { "LastName" }).LastName;

            string objectTitle = TabID == "0" ? "后台菜单" : "会员菜单";

            NameLabel.Text = string.Format(strTitle, ownerTitle, ownerName, SiteConfigs.GetConfig().SiteName, objectTitle);

            LoadControl();
        }

        void LoadControl()
        {
            int tab = 0;
            if (TabID != null && We7Helper.IsNumber(TabID))
                tab = int.Parse(TabID);

            //向动态控件传参
            Permission_Func ctl = this.LoadControl("../Permissions/Permission_Func.ascx") as Permission_Func;
            ctl.OwnerType = OwnerType;
            ctl.OwnerID = OwnerID;

            if (tab==0)
            {
                ctl.ObjectID = "System.Administration";
                ctl.EntityID = "System.Administration";
            }
            else
            {
                ctl.ObjectID = "System.User";
                ctl.EntityID = "System.User";
            }

            ContentHolder.Controls.Add(ctl);
        }
    }
}
