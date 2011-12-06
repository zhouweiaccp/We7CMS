using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.UI;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using We7.Framework.Util;
using System.Web;

namespace We7.Model.Core.ListControl
{
	/// <summary>
	/// 列表显示缩略图字段
	/// </summary>
	public class ThumbField : ModelControlField
	{
		protected override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState)
		{
			if (cellType == DataControlCellType.DataCell)
			{
				//Literal ltl = new Literal();
				//ltl.DataBinding += new EventHandler(ltl_DataBinding);
				//cell.Controls.Add(ltl);
				HtmlImage img = new HtmlImage();
				img.Style.Add("max-height", "80px");
				img.Style.Add("max-width", "80px");
				img.DataBinding += new EventHandler(img_DataBinding);
				cell.Controls.Add(img);
			}
		}

		void img_DataBinding(object sender, EventArgs e)
		{
			//Literal ltl = sender as Literal;
			HtmlImage img = sender as HtmlImage;
			//object dataitem = DataBinder.GetDataItem(ltl.NamingContainer);
			object dataitem = DataBinder.GetDataItem(img.NamingContainer);

			string columnValue = GetValue(dataitem, Column.Name);
			if (columnValue.StartsWith("[[{"))
			{
				int start = columnValue.IndexOf('\'');
				int end = columnValue.IndexOf(',');
				columnValue = columnValue.Substring(start + 1, end - start - 2);
			}
			if (string.IsNullOrEmpty(columnValue) || columnValue.LastIndexOf('.') <= 0) { img.Alt = "无图"; return; }
			string temp = columnValue.Substring(0, columnValue.LastIndexOf('.'));
			if (!temp.EndsWith("_thumb"))
			{
				temp = string.Format("{0}_thumb{1}", temp, columnValue.Substring(columnValue.LastIndexOf('.')));
				if (File.Exists(HttpContext.Current.Server.MapPath(temp))) columnValue = temp;
			}


			img.Src = columnValue;
		}

		protected override DataControlField CreateField()
		{
			return new HtmlField();
		}
	}
}
