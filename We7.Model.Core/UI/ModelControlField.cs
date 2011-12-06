using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace We7.Model.Core.UI
{
	/// <summary>
	/// 模型字段数据控件字段
	/// </summary>
	public abstract class ModelControlField : DataControlField
	{
		/// <summary>
		/// 当前序号
		/// </summary>
		public int RowIndex
		{
			get
			{
				if (ViewState["RowIndex"] == null)
				{
					ViewState["RowIndex"] = -1;
				}
				return (int)ViewState["RowIndex"];
			}
			set
			{
				ViewState["RowIndex"] = value;
			}
		}

		/// <summary>
		/// 模型字段信息
		/// </summary>
		public ColumnInfo Column
		{
			get
			{
				return ViewState["ColumnInfo"] as ColumnInfo;
			}
			set
			{
				ViewState["ColumnInfo"] = value;
			}
		}

		/// <summary>
		/// 模型信息
		/// </summary>
		public ModelInfo ModelInfo
		{
			get;
			set;
		}

		public We7DataSet DataSet
		{
			get
			{
				return ViewState["$DataSet"] as We7DataSet;
			}
			set
			{
				ViewState["$DataSet"] = value;
			}
		}

		protected override void CopyProperties(DataControlField newField)
		{
			ModelControlField mcf = newField as ModelControlField;
			mcf.Column = Column;
			base.CopyProperties(newField);
		}

		public sealed override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			RowIndex = rowIndex;
			HeaderText = String.IsNullOrEmpty(Column.Params["lheader"]) ? Column.Label : Column.Params["lheader"];
			if (String.IsNullOrEmpty(HeaderText) && DataSet != null && DataSet.Tables.Count > 0)
			{
				We7DataColumn col = DataSet.Tables[0].Columns[Column.Name];
				if (col != null)
				{
					HeaderText = col.Label;
				}
			}
			base.InitializeCell(cell, cellType, rowState, rowIndex);
			InitializeCell(cell, cellType, rowState);
		}

		public override bool Initialize(bool sortingEnabled, Control control)
		{
			if (!String.IsNullOrEmpty(Column.Width))
			{
				ItemStyle.Width = Unit.Parse(Column.Width);
				HeaderStyle.Width = ItemStyle.Width;
			}
			if (Column.Align != HorizontalAlign.NotSet)
			{
				ItemStyle.HorizontalAlign = Column.Align;
				//HeaderStyle.HorizontalAlign = Column.Align;
			}
			return base.Initialize(sortingEnabled, control);
		}

		/// <summary>
		/// 格式化单元格
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="cellType"></param>
		void InitCellStyle(DataControlFieldCell cell, DataControlCellType cellType)
		{
			cell.Width = Unit.Parse(Column.Params["lwidth"]);
			if (cellType == DataControlCellType.DataCell)
			{
				if (!String.IsNullOrEmpty(Column.Params["align"]))
				{
					cell.Style.Add(HtmlTextWriterStyle.TextAlign, Column.Params["align"]);
				}
			}
		}

		/// <summary>
		/// 将文本或控件添加到单元格的控件集合中。
		/// </summary>
		/// <param name="cell">一个 System.Web.UI.WebControls.DataControlFieldCell，包含 System.Web.UI.WebControls.DataControlField的文本或控件。</param>
		/// <param name="cellType">System.Web.UI.WebControls.DataControlCellType 值之一。</param>
		/// <param name="rowState">System.Web.UI.WebControls.DataControlRowState 值之一，指定包含 System.Web.UI.WebControls.DataControlFieldCell的行的状态。</param>
		protected abstract void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState);

		/// <summary>
		/// 从前的DataItem中取得数据
		/// </summary>
		/// <param name="dataitem">当前的DataItem</param>
		/// <param name="expression">表达示</param>
		/// <returns>查询到的值</returns>
		public static string GetValue(object dataitem, string expression)
		{
			object o = expression.Contains(".") ? DataBinder.Eval(dataitem, expression) : DataBinder.GetPropertyValue(dataitem, expression);
			return o != null ? o.ToString() : "";
		}

		/// <summary>
		/// 解析对齐方式
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected HorizontalAlign ParseHorizonAlign(string value)
		{
			HorizontalAlign align = HorizontalAlign.NotSet;
			try
			{
				align = (HorizontalAlign)Enum.Parse(typeof(HorizontalAlign), value, true);
			}
			catch
			{
			}
			return align;
		}
	}
}
