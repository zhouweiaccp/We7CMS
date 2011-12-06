using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.UI;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

namespace We7.Model.Core.ListControl
{
	public class HtmlField : ModelControlField
	{
		protected override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState)
		{
			if (cellType == DataControlCellType.DataCell)
			{
				Literal ltl = new Literal();
				ltl.DataBinding += new EventHandler(ltl_DataBinding);
				cell.Controls.Add(ltl);
			}
		}

		void ltl_DataBinding(object sender, EventArgs e)
		{
			Literal ltl = sender as Literal;
			object dataitem = DataBinder.GetDataItem(ltl.NamingContainer);
			if (Column.IsLink)
			{
				ltl.Text = string.Format("<a href='{0}'>{1}</a>", GetLinkUrl(dataitem), GetValue(dataitem, Column.Name));
			}
			else
				ltl.Text = GetValue(dataitem, Column.Name);
		}

		/// <summary>
		/// 获取链接地址
		/// </summary>
		/// <param name="dataitem"></param>
		/// <returns></returns>
		private string GetLinkUrl(object dataitem)
		{
			if (ModelInfo == null) ModelInfo = ModelHelper.GetModelInfo(We7Helper.GetParamValueFromUrl(HttpContext.Current.Request.RawUrl, "model"));
			string result = string.Empty;
			foreach (We7DataColumn d in ModelInfo.DataSet.Tables[0].Columns)
			{
				if (d.Name.Contains("_Count"))
				{
					string model = string.Format("{0}.{1}", ModelInfo.GroupName, d.Name.Remove(d.Name.Length - 6));
					string url = We7Helper.AddParamToUrl(HttpContext.Current.Request.RawUrl, "model", model);
					url = We7Helper.RemoveParamFromUrl(url, "mode");
					url = We7Helper.AddParamToUrl(url, d.Mapping.Split('|')[0], GetValue(dataitem, d.Mapping.Split('|')[1]));
					result = url;
					break;
				}
			}
			if (string.IsNullOrEmpty(result))
				result = string.Format("/admin/AddIns/ModelViewer.aspx?notiframe=1&model={0}&ID={1}", ModelInfo.ModelName, GetValue(dataitem, "ID"));
			return result;
		}
		protected override DataControlField CreateField()
		{
			return new HtmlField();
		}
	}
}
