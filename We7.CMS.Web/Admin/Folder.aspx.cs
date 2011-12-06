using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using We7.CMS.Common.Enum;
using System.IO;
using We7.CMS.Common;
namespace We7.CMS.Web.Admin
{
	public partial class Folder : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			PluginInfo pinfo = new PluginInfo(Server.MapPath(string.Format("/Plugins/FileManagement/Plugin.xml")));
			if (pinfo.IsInstalled)
			{
				Response.Redirect(string .Format ("/Plugins/FileManagement/UI/Folder.aspx?{0}",Request.QueryString.ToString()));
			}
		}
		protected override MasterPageMode MasterPageIs
		{
			get
			{
				return MasterPageMode.None;
			}
		}
	}
}