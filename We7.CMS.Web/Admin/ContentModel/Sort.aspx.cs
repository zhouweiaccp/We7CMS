using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using We7.Model.Core.Data;
using We7.Model.Core;
using Thinkment.Data;

namespace We7.CMS.Web.Admin.ContentModel
{
	public partial class Sort : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
		public string ModelName { get { return We7Helper.GetParamValueFromUrl(Request.RawUrl, "model"); } }
		ModelInfo ModelInfo { get { return ModelHelper.GetModelInfoByName(ModelName); } }
		ModelDBHelper helper { get { return ModelDBHelper.Create(ModelName); } }
		private DataRowCollection drows;
		public DataRowCollection SortData
		{
			get
			{
				if (drows == null)
				{
					Criteria c = new Criteria();
					foreach (string urlParam in Request.QueryString.AllKeys)
					{
						if (urlParam == "model") continue;
						if (ModelInfo.DataSet.Tables[0].Columns.Contains(urlParam))
							c.Add(CriteriaType.Equals, urlParam, Request.QueryString[urlParam]);
					}
					List<Order> orders = new List<Order>();
					orders.Add(new Order("Index"));
					orders.Add(new Order("IsShow", OrderMode.Desc));
					orders.Add(new Order("Updated", OrderMode.Desc));
					orders.Add(new Order("ID", OrderMode.Desc));
					drows = helper.Query(c, orders).Rows;
				}
				return drows;
			}
		}
		private string sortImg;
		public string SortImg
		{
			get
			{
				sortImg = We7Helper.GetParamValueFromUrl(Request.RawUrl, "sortImg");
				if (string.IsNullOrEmpty(sortImg))
				{
					sortImg = SortText;
				}
				return sortImg;
			}
		}
		private string sortText;
		public string SortText
		{
			get
			{
				return sortText = We7Helper.GetParamValueFromUrl(Request.RawUrl, "sortText");
			}
		}

		public string SortImgUrl(DataRow dr)
		{
			int i=dr[SortImg].ToString().LastIndexOf('.');
			if (i > 0) return
				dr[SortImg].ToString().Insert(i, "_thumb");
			else return "/admin/images/flower.jpg";
			
		}
	}
}