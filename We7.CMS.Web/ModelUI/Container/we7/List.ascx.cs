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
using We7.Model.Core;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace We7.Model.UI.Container.we7
{
	public partial class List : ListContainer
	{
		/// <summary>
		/// 当前主键字段
		/// </summary>
		private DataKey dataKey;

		public override void BindData(ListResult result)
		{
			if (GroupIndex == 0)
			{
				if (!IsPostBack)
				{
					foreach (ColumnInfo field in Columns)
					{
						if (!field.Visible)
							continue;
						ModelControlField lc = ModelHelper.GetDataControl(field.Type);
						if (lc != null)
						{
							lc.DataSet = Info.DataSet;
							lc.ModelInfo = PanelContext.Model;
							lc.Column = field;
							gvList.Columns.Add(lc);
						}
					}
					gvList.DataKeyNames = PanelContext.DataKeyString.Split(',');
				}
				if (result != null)
				{
					gvList.DataSource = result.DataTable;
					gvList.DataBind();
				}
			}
			else if (GroupIndex == 1)
			{
				gvList.Visible = false;
				ulList.Visible = true;
				InitUlList(result);
			}
		}
		bool countEnable = false;
		string columnCount = string.Empty;

		/// <summary>
		/// 显示列图模式
		/// </summary>
		/// <param name="result"></param>
		private void InitUlList(ListResult result)
		{
			foreach (DataRow dr in result.DataTable.Rows)
			{
				HtmlGenericControl li = new HtmlGenericControl("li");
				int width = 0;
				foreach (ColumnInfo field in Columns)
				{
					if (!field.Visible)
						continue;
					HtmlGenericControl div = new HtmlGenericControl("div");
					string thisWidth = field.Width.Contains("px") ? field.Width : field.Width + "px";
					string thisHeight = field.Height.Contains("px") ? field.Height : field.Height + "px";
					div.Style.Add(HtmlTextWriterStyle.Width, thisWidth);
					div.Style.Add(HtmlTextWriterStyle.Height, thisHeight);

					if (int.Parse(thisWidth.Replace("px", "")) > width) width = int.Parse(field.Width.Replace("px",""));
					if (field.IsThumb)
					{
						div.Attributes.Add("class", "img");
						HtmlImage img = new HtmlImage();
						img.Src = dr[field.Name].ToString();
						if (img.Src.LastIndexOf('.') > 0)
							img.Src = img.Src.Insert(img.Src.LastIndexOf('.'), "_thumb");

						if (string.IsNullOrEmpty(img.Src)) img.Src = "/Admin/images/flower.jpg";
						img.Style.Add(HtmlTextWriterStyle.Width, "100%");
						img.Style.Add(HtmlTextWriterStyle.Height, "100%");
						if (field.IsLink)
						{
							HtmlAnchor a = new HtmlAnchor();
							GetLinkUrl(dr, a);
							a.Controls.Add(img);
							div.Controls.Add(a);
						}
						else
							div.Controls.Add(img);
					}
					else if (field.IsLink)
					{
						HtmlAnchor a = new HtmlAnchor();
						GetLinkUrl(dr, a);
						a.InnerText = dr[field.Name].ToString();
						div.Controls.Add(a);
						div.Attributes.Add("class", "title");
					}
					if (field.Type == "action")
					{
						HtmlGenericControl span = new HtmlGenericControl("span");
						span.InnerText = countEnable ? "数量：" + (string.IsNullOrEmpty(dr[columnCount].ToString()) ? "0" : dr[columnCount].ToString()) : string.Empty; ;
						HtmlImage imgDel = new HtmlImage();
						imgDel.Src = "/Admin/images/icon_del1.gif";
						imgDel.Attributes.Add("title", "删除");
						imgDel.Attributes.Add("onclick", string.Format("Del('{0}');", dr["ID"].ToString()));
						HtmlImage imgEdit = new HtmlImage();
						imgEdit.Src = "/Admin/images/icon_edit1.gif";
						imgEdit.Attributes.Add("title", "编辑");
						string editUrl = string.Format("/admin/addins/ModelEditor.aspx?notiframe={0}&model={1}&ID={2}&groupIndex=0", We7Helper.GetParamValueFromUrl(Request.RawUrl, "notiframe"),
							We7Helper.GetParamValueFromUrl(Request.RawUrl, "model"), dr["ID"].ToString());
						imgEdit.Attributes.Add("onclick", string.Format("Edit('{0}');", editUrl));
						div.Controls.Add(span);
						div.Controls.Add(imgDel);
						div.Controls.Add(imgEdit);
						HtmlInputCheckBox selected = new HtmlInputCheckBox();
						selected.Attributes.Add("title", "选中");
						selected.Attributes.Add("class", dr["ID"].ToString());
						div.Controls.Add(selected);
						div.Attributes.Add("class", "actionbar");
					}
					li.Controls.Add(div);
				}
				li.Style.Add(HtmlTextWriterStyle.Width, width + "px");
				ulList.Controls.Add(li);
			}
		}
		/// <summary>
		/// 获取链接地址
		/// </summary>
		/// <param name="dr"></param>
		/// <param name="a"></param>
		private void GetLinkUrl(DataRow dr, HtmlAnchor a)
		{
			a.HRef = string.Empty;
			foreach (We7DataColumn d in PanelContext.Model.DataSet.Tables[0].Columns)
			{
				if (d.Name.Contains("_Count"))
				{
					string model = string.Format("{0}.{1}", PanelContext.Model.GroupName, d.Name.Remove(d.Name.Length - 6));
					string url = We7Helper.AddParamToUrl(Request.RawUrl, "model", model);
					url = We7Helper.RemoveParamFromUrl(url, "mode");
					url = We7Helper.AddParamToUrl(url, d.Mapping.Split('|')[0], dr[d.Mapping.Split('|')[1]].ToString());
					a.HRef = url;
					countEnable = true;
					columnCount = d.Name;
					break;
				}
			}
			if (string.IsNullOrEmpty(a.HRef))
			{
				a.HRef = string.Format("/admin/AddIns/ModelViewer.aspx?notiframe=1&model={0}&ID={1}", PanelContext.ModelName, dr["ID"]);
				a.Target = "_blank";
			}
		}

		/// <summary>
		/// 取得所选中行的主键值
		/// </summary>
		/// <returns></returns>
		public override List<DataKey> GetDataKeys()
		{
			List<DataKey> dataKeys = new List<DataKey>();
			if (GroupIndex == 0)
				foreach (GridViewRow row in gvList.Rows)
				{
					CheckBox c = row.Cells[0].FindControl("chkID") as CheckBox;
					if (c != null && c.Checked)
					{
						dataKeys.Add(gvList.DataKeys[row.RowIndex]);
					}
				}
			else if (GroupIndex == 1)
			{
				string[] vals = delID.Value.Split('|');
				foreach (string val in vals)
				{
					if (string.IsNullOrEmpty(val)) continue;
					OrderedDictionary dic = new OrderedDictionary();
					dic.Add("ID", val);
					dataKeys.Add(new DataKey(dic as IOrderedDictionary));
				}
				delID.Value = string.Empty;
			}
			return dataKeys;
		}

		protected override void InitModelData()
		{
			PanelContext.DataKey = dataKey;
		}

		protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				e.Row.Attributes["onmouseover"] = "this.className='mouseover'";
				e.Row.Attributes["onmouseout"] = "this.className=''";
			}
		}

		protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			dataKey = gvList.DataKeys[e.RowIndex];
			OnCommandSubmit("delete", null);
		}

		protected void gvList_RowEditing(object sender, GridViewEditEventArgs e)
		{
			dataKey = gvList.DataKeys[e.NewEditIndex];
			OnCommandSubmit("get", null);
		}

		protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			////TODO::这句会损失一点效率,但是影响不大.
			List<string> list = new List<string>(new string[] { "DELETE", "EDIT", "SELECT", "CANCEL" });
			if (!list.Contains(e.CommandName.ToUpper()))
			{
				dataKey = gvList.DataKeys[Convert.ToInt32(e.CommandArgument)];
				OnCommandSubmit(e.CommandName, e.CommandArgument);
			}
		}

		protected void DelBtn_Click(object sender, EventArgs e)
		{
			OrderedDictionary dic = new OrderedDictionary();
			dic.Add("ID", delID.Value);
			dataKey = new DataKey(dic as IOrderedDictionary);
			OnCommandSubmit("delete", null);
		}

	}
}