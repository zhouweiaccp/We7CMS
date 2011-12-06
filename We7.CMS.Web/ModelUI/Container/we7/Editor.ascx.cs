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
using We7.Model.Core;
using We7.Model.Core.UI;
using We7.Framework;
using We7.CMS.Accounts;

namespace We7.Model.UI.Container.we7
{
	public partial class Editor : EditorContainer
	{
		protected string NewUrl
		{
			get
			{
				return We7Helper.AddParamToUrl(Request.RawUrl, We7.Model.Core.UI.Constants.EntityID, We7Helper.CreateNewID());
			}
		}

		/// <summary>
		/// 返回地址
		/// </summary>
		public string BackUrl
		{
			get
			{
				string url = Request.RawUrl;
				url = We7Helper.RemoveParamFromUrl(url, We7.Model.Core.UI.Constants.EntityID);
				url = We7Helper.RemoveParamFromUrl(url, "panel");
				url = We7Helper.RemoveParamFromUrl(url, "groupIndex");
				return url.Replace("ModelEditor.aspx", "ModelList.aspx");
			}
		}

		protected override void InitContainer()
		{
			ChangeState();
			editMode.Controls.Clear();
			string curMode = HttpUtility.UrlDecode(We7Helper.GetParamValueFromUrl(Request.RawUrl, "mode"));
			bool flag = true;
			if (Request["groupIndex"] == null) flag = false;
			foreach (Group group in PanelContext.Panel.EditInfo.Groups)
			{
				if (group.Enable)
				{
					if (string.IsNullOrEmpty(curMode)) curMode = group.Name;
					if (!flag)
					{
						GroupIndex = group.Index;
						flag = true;
					}
				}
				HtmlAnchor a = new HtmlAnchor();
				a.InnerText = group.Name;
				a.Style.Add(HtmlTextWriterStyle.MarginRight, "20px");
				if (group.Index == GroupIndex) a.Style.Add(HtmlTextWriterStyle.FontWeight, "bolder");
				a.HRef = We7Helper.AddParamToUrl(Request.RawUrl, "mode", HttpUtility.UrlEncode(group.Name));
				a.HRef = We7Helper.AddParamToUrl(a.HRef, "groupIndex", group.Index.ToString());
				editMode.Controls.Add(a);
			}
			if (string.IsNullOrEmpty(curMode) || IsEdit) curMode = "默认";
			rpEditor.DataSource = PanelContext.Panel.EditInfo.Groups[curMode].Controls;
			rpEditor.DataBind();
			ModelLabel.Text = PanelContext.Model.Label;
			MenuTabLabel.Text = BuildNavString();
			if (IsEdit)
			{
				trBtn.Visible = Security.CurrentAccountID == (PanelContext.Row["AccountID"] ?? We7Helper.EmptyGUID).ToString() ||
					Security.CurrentAccountID == We7Helper.EmptyGUID;
			}
		}

		protected override void ChangeState()
		{
			bttnEdit.Visible = IsEdit;
			bttnSave.Visible = !IsEdit;
		}

		protected void rpEditor_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
			{
				We7Control ctr = e.Item.DataItem as We7Control;
				e.Item.Controls.Add(CreateItem(ctr));
			}
		}

		/// <summary>
		/// 创建行控件
		/// </summary>
		/// <param name="control">字段信息</param>
		/// <returns>行控件</returns>
		protected Control CreateItem(We7Control control)
		{
			HtmlTableRow row = new HtmlTableRow();
			HtmlTableCell c = new HtmlTableCell();
			c.InnerHtml = GetLabel(control) + "：";
			c.Attributes["class"] = "formTitle";
			row.Cells.Add(c);

			c = new HtmlTableCell();
			c.Attributes["class"] = "formValue";
			FieldControl fc = UIHelper.GetControl(control);
			fc.IsEdit = IsEdit;
			c.Controls.Add(fc);

			Literal ltlMsg = new Literal();
			ltlMsg.Text = control.Desc;
			c.Controls.Add(ltlMsg);

			row.Cells.Add(c);

			row.Style.Add("display", control.Visible ? "" : "none");

			return row;
		}

		/// <summary>
		/// 构建标签项
		/// </summary>
		/// <returns></returns>
		string BuildNavString()
		{
			string str1 = @"<LI class=TabOut id=tab{0}  style='display:{2}'><A  href={3}>{1}</A> </LI>";
			string str2 = @"<LI class=TabIn id=tab{0} style='display:{2}'><A>{1}</A> </LI>";
			return String.Format(str2, 1, "发布" + PanelContext.Model.Label, "block");
		}

		protected void bttnSave_Click(object sender, EventArgs e)
		{
			try
			{
				OnButtonSubmit(sender, e);
			}
			catch (Exception ex)
			{
				UIHelper.SendError("添加失败:" + ex.Message);
			}
		}

		protected void bttnEdit_Click(object sender, EventArgs e)
		{
			try
			{
				OnButtonSubmit(sender, e);
			}
			catch (Exception ex)
			{
				UIHelper.SendError("修改失败:" + ex.Message);
			}
		}
	}
}