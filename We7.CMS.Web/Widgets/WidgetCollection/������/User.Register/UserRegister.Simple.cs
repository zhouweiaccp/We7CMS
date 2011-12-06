using System;
using System.Collections.Generic;
using System.Web;
using We7.CMS.WebControls.Core;
using We7.CMS.WebControls;
using System.IO;
using System.Text;

namespace We7.CMS.Web.Widgets
{
    [ControlGroupDescription(Label = "用户注册", Icon = "用户注册", Description = "用户注册", DefaultType = "UserRegister.Simple")]
    [ControlDescription(Desc = "用户注册")]
	public partial class UserRegister : BaseControl
    {
		/// <summary>
		/// 自定义Css类名称
		/// </summary>
		[Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "UserRegister_Simple")]
		public string CssClass;

		[Parameter(Title = "是否显示注册协议", Type = "Boolean", DefaultValue = "0",Required=true)]
		public bool ShowProtocol;

		/// <summary>
		/// 协议
		/// </summary>
		public string Protocol
		{
			get
			{
				if (ViewState["$Protocol"] == null)
				{

					string path = Server.MapPath(Path.Combine(TemplateSourceDirectory, "Protocol.txt"));
					if (File.Exists(path))
					{
						ViewState["$Protocol"] = File.ReadAllText(path, Encoding.UTF8);
					}
					else
					{
						ViewState["$Protocol"] = String.Empty;
					}
				}
				return ViewState["$Protocol"].ToString();
			}
		}
		public string AccountID { get { return Request["v"]; } }
    }
}