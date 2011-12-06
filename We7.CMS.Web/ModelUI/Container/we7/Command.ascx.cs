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
using System.Text;
using We7.CMS.Common;
using We7.CMS;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.Model.Core;

namespace We7.Model.UI.Container.we7
{
	public partial class Command : CommandContainer
	{
		protected override void LoadContainer()
		{
			TagsLiteral.Text = LoadTagDictionary();
			string paramName = string.Empty;
			string paramValue = string.Empty;
			foreach (We7DataColumn colum in PanelContext.DataSet.Tables[0].Columns)
			{
				string urlParam = We7Helper.GetParamValueFromUrl(Request.RawUrl, colum.Name);
				if (string.IsNullOrEmpty(urlParam)) continue;
				paramName = colum.Name;
				paramValue = urlParam;
				break;
			}
			lnkNewArticle.NavigateUrl = String.Format("~/admin/addins/ModelEditor.aspx?notiframe={3}&model={0}&panel=edit&ID={1}{2}{4}",
				Request["model"], We7Helper.CreateNewID(), String.IsNullOrEmpty(Request["oid"]) ? "" : String.Format("&oid={0}", Request["oid"]),
				Request["notiframe"], string.IsNullOrEmpty(paramValue) ? "" : string.Format("&{0}={1}", paramName, paramValue));
		}

		protected void lnkPubLish_Click(object sender, EventArgs e)
		{
			try
			{
				OnButtonSubmit(sender, e);
				Core.UIHelper.SendMessage("发布成功");
			}
			catch (Exception ex)
			{
				Core.UIHelper.SendError("发布错误:" + ex.Message);
			}
		}

		protected void lnkStopPubLish_Click(object sender, EventArgs e)
		{
			try
			{
				OnButtonSubmit(sender, e);
				Core.UIHelper.SendMessage("取消发布成功");
			}
			catch (Exception ex)
			{
				Core.UIHelper.SendError("取消发布错误:" + ex.Message);
			}
		}

		protected void lnkSubmitAudit_Click(object sender, EventArgs e)
		{
			try
			{
				OnButtonSubmit(sender, e);
				Core.UIHelper.SendMessage("提交审核成功");
			}
			catch (Exception ex)
			{
				Core.UIHelper.SendError("提交审核错误:" + ex.Message);
			}
		}

		protected void lnkSetTop_Click(object sender, EventArgs e)
		{
			try
			{
				OnButtonSubmit(sender, e);
				Core.UIHelper.SendMessage("置顶成功");
			}
			catch (Exception ex)
			{
				Core.UIHelper.SendError("置顶错误:" + ex.Message);
			}
		}

		protected void lnkCancelTop_Click(object sender, EventArgs e)
		{
			try
			{
				OnButtonSubmit(sender, e);
				Core.UIHelper.SendMessage("取消置顶成功");
			}
			catch (Exception ex)
			{
				Core.UIHelper.SendError("取消置顶错误:" + ex.Message);
			}
		}

		protected void lnkPublishShared_Click(object sender, EventArgs e)
		{
			try
			{
				OnButtonSubmit(sender, e);
				Core.UIHelper.SendMessage("共享发布成功");
			}
			catch (Exception ex)
			{
				Core.UIHelper.SendError("共享发布错误:" + ex.Message);
			}
		}

		protected void lnkReciveShared_Click(object sender, EventArgs e)
		{
			try
			{
				OnButtonSubmit(sender, e);
				Core.UIHelper.SendMessage("获取共享成功");
			}
			catch (Exception ex)
			{
				Core.UIHelper.SendError("获取共享失败:" + ex.Message);
			}
		}



		protected void lnkMoveTo_Click(object sender, EventArgs e)
		{
			try
			{
				PanelContext.Objects["oid"] = hfMoveTo.Value;
				OnButtonSubmit(sender, e);
				Core.UIHelper.SendMessage("信息移动成功！");
			}
			catch (Exception ex)
			{
				Core.UIHelper.SendError("信息移动失败:" + ex.Message);
			}
		}

		protected void lnkLinkTo_Click(object sender, EventArgs e)
		{
			try
			{
				PanelContext.Objects["oid"] = hfLinkTo.Value;
				OnButtonSubmit(sender, e);
				Core.UIHelper.SendMessage("信息移动成功！");
			}
			catch (Exception ex)
			{
				Core.UIHelper.SendError("信息移动失败：" + ex.Message);
			}
		}

		protected void lnkAddTag_Click(object sender, EventArgs e)
		{
			try
			{
				PanelContext.Objects["tag"] = hfTag.Value;
				OnButtonSubmit(sender, e);
				Core.UIHelper.SendMessage("添加标签成功！");
			}
			catch (Exception ex)
			{
				Core.UIHelper.SendError("添加标签失败：" + ex.Message);
			}
		}

		string LoadTagDictionary()
		{
			string tagA = "<a href=\"javascript:addTag('{0}')\" title=\"为文章添加标签 {0}？\"  >{0}</a> ";
			int maxCount = 20;
			StringBuilder sb = new StringBuilder();

			TagsGroup ag = TagsHelper.GetTagsGroup();
			int i = 1;
			foreach (TagsGroup.Item tag in ag.Items)
			{
				sb.AppendLine(string.Format(tagA, tag.Words));
				if (i % 2 == 0) sb.AppendLine("<br/>");
				if (i > maxCount) break;
				i++;
			}

			return sb.ToString();
		}

		TagsHelper TagsHelper
		{
			get { return HelperFactory.GetHelper<TagsHelper>(); }
		}

		HelperFactory HelperFactory
		{
			get
			{
				return (HelperFactory)Application[HelperFactory.ApplicationID];
			}
		}

		public string URLParam
		{
			get
			{
				int i = Request.RawUrl.IndexOf("?");
				if (i > 0)
				{
					string param = Request.RawUrl.Remove(0, i);
					Group group = PanelContext.Model.Layout.Panels["list"].ListInfo.Groups[GroupIndex];

					foreach (ColumnInfo col in group.Columns)
					{
						if (col.IsThumb)
						{
							param += "&sortImg=" + col.Name;

						}
						else if (col.IsLink) param += "&sortText=" + col.Name;
					}
					return param;
				}
				else return string.Empty;
			}
		}
	}
}