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
using We7.CMS.Common;
using We7.CMS.Helpers;
using System.Collections.Generic;

namespace We7.CMS.Web.Admin.controls
{
    public partial class Channel_Module : BaseUserControl
    {
        public string ChannelID
        {
            get { return Request["id"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void BindData()
        {
            chkModules.DataSource = HelperFactory.GetHelper<ChannelModuleHelper>().GetAllModule();
            chkModules.DataTextField = "Title";
            chkModules.DataValueField = "ID";
            chkModules.DataBind();

            List<ChannelModuleMapping> mapping = HelperFactory.GetHelper<ChannelModuleHelper>().GetMappingByChannelID(ChannelID);
            foreach (ListItem item in chkModules.Items)
            {
                foreach (ChannelModuleMapping m in mapping)
                {
                    if (item.Value == m.ModuleID)
                        item.Selected = true;
                }
            }
        }


        protected void SaveButton_ServerClick(object sender, EventArgs args)
        {
            if (DemoSiteMessage)
            {
                return;
            }

            try
            {
                HelperFactory.GetHelper<ChannelModuleHelper>().DeleteMappingByChannelID(ChannelID);
                foreach (ListItem item in chkModules.Items)
                {
                    if (item.Selected)
                    {
                        HelperFactory.GetHelper<ChannelModuleHelper>().CreateMapping(ChannelID, item.Value, String.Empty);
                    }
                }
                Messages.ShowMessage("保存成功");
            }
            catch (Exception ex)
            {
                Messages.ShowError(ex.Message);
            }
        }
    }
}