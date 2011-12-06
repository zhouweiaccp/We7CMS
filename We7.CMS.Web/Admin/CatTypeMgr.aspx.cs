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
using We7.Framework;

namespace We7.CMS.Web.Admin
{
	public partial class CatTypeMgr : BasePage
	{
		CategoryHelper CategoryHelper = HelperFactory.Instance.GetHelper<CategoryHelper>();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				BindData();
			}
			lnkDel.Click += new EventHandler(lnkDel_Click);
		}

		void lnkDel_Click(object sender, EventArgs e)
		{
			if (DemoSiteMessage) return;//是否是演示站点
			string strId = Request["ids"];
			if (!String.IsNullOrEmpty(strId))
			{
				string[] ss = strId.Split(',');
				int count = 0;
				foreach (string s in ss)
				{
					try
					{
						CategoryHelper.DeleteCategory(s);
					}
					catch
					{
						count++;
					}
				}
				if (count == 0)
				{
					Messages.ShowMessage("成功删除" + ss.Length + "条记录!");
				}
				else
				{
					Messages.ShowMessage("成功删除" + (ss.Length - count) + "条记录,失败" + count + "条!");
				}
				BindData();
			}
		}

		void lnkEdit_Click(object sender, EventArgs e)
		{
			string id = Request["ids"];
			Response.Redirect("CatTypeAdd.aspx?id=" + id);
		}

		protected override bool NeedAnPermission
		{
			get { return false; }
		}

		void BindData()
		{
			CategoryHelper helper = HelperFactory.GetHelper<CategoryHelper>();
			DataGridView.DataSource = helper.GetCategoryByParentID(We7Helper.EmptyGUID);
			DataGridView.DataBind();
		}
	}
}
