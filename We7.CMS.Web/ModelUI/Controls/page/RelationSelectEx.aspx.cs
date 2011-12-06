using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Thinkment.Data;
using System.Data;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS;
using We7.Model.Core;
using System.Xml.Serialization;
using We7.Model.UI.Controls.cs;
using We7.Model.Core.Data;
using We7.Model.Core.UI;

namespace We7.Model.UI.Controls.page
{
	public partial class RelationSelectEx : Page
	{
		private ParamCollection _Params = new ParamCollection();
		/// <summary>
		/// 参数集合
		/// </summary>
		[XmlElement("param")]
		public ParamCollection Params
		{
			get { return _Params; }
			set { _Params = value; }
		}


		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				//清空缓存
				Response.CacheControl = "no-cache";
				Response.Expires = 0;

				if (Request["NewName"] != null && Request["NewID"] != null && Request["Model"] != null)
				{
					string newName = Request["NewName"].ToString();
					string newID = Request["NewID"].ToString();
					string model = Request["Model"].ToString();
					ModelDBHelper helper = ModelDBHelper.Create(model);
					Dictionary<string, object> dic = new Dictionary<string, object>();
					dic.Add("Name", newName);
					dic.Add("ID", newID);
					dic.Add("AccountID", We7.CMS.Accounts.Security.CurrentAccountID);
					dic.Add("State",1);
					dic.Add("Index",999);
					dic.Add("Created", DateTime.Now);
					dic.Add("Updated", DateTime.Now);
					helper.Insert(dic);

				}

			}
		}



	}
}
