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
using We7.Model.Core.UI;
using System.Xml;
using We7.CMS;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;
using System.Collections.Generic;

namespace CModel.Controls.we7
{
    public partial class ChannelSelect : FieldControl
    {
        public override void InitControl()
        {
            List<Channel> list = ChannelHelper.GetChannelByModelName(PanelContext.ModelName);
            ddlChannel.DataSource = list;
            ddlChannel.DataValueField = "ID";
            ddlChannel.DataTextField = "FullPath";
            ddlChannel.DataBind();
            ddlChannel.Items.Insert(0, new ListItem("选择栏目", ""));

            if (Value != null && !String.IsNullOrEmpty(Value.ToString()))
            {
                ddlChannel.SelectedValue = Value.ToString();
            }
            else
            {
                ddlChannel.SelectedValue = Request["oid"];
            }

            ddlChannel.CssClass = Control.CssClass;
            if (Control.Required && !ddlChannel.CssClass.Contains("required"))
            {
                ddlChannel.CssClass += " required";
            }
        }

        public override object GetValue()
        {
            return ddlChannel.SelectedValue;
        }       

        ChannelHelper ChannelHelper
        {
            get
            {
                return HelperFactory.GetHelper<ChannelHelper>();
            }
        }

        public HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

    }
}