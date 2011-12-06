using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin.Addins
{
	public partial class Comment : BasePage
	{


		protected int TitleMaxLength = 25;

		string ArticleID
		{
			get { return Request["aid"]; }
		}

		protected override void Initialize()
		{
			LoadComments();
		}

		protected void DataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
		{

		}

		protected void Pager_Fired(object sender, EventArgs e)
		{
			LoadComments();
		}

		protected void DeleteBtn_Click(object sender, EventArgs e)
		{
			if (DemoSiteMessage) return;//是否是演示站点
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				MessageLabel.Text = "你没有选择任何一条记录";
				MessagePanel.Visible = true;
				return;
			}

			string aTitle = "";

			foreach (string id in ids)
			{
				Comments c = CommentsHelper.GetComment(id, new string[] { "Content" });
				CommentsHelper.DeleteComment(id);

				string con = c.Content;
				if (c.Content.Length > 10)
				{
					con = c.Content.Substring(0, 10);
				}

				aTitle += String.Format("{0};", con);
			}

			//记录日志
			string content = string.Format("删除了{0}条评论:“{1}”", ids.Count.ToString(), aTitle);
			AddLog("评论管理", content);

			MessageLabel.Text = string.Format("您已经成功删除{0}条记录", ids.Count.ToString());
			MessagePanel.Visible = true;
			LoadComments();
		}

		/// <summary>
		/// 启用评论
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void StartButton_Click(object sender, EventArgs e)
		{
			SetState(1);
		}

		/// <summary>
		/// 禁用评论
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void StopButton_Click(object sender, EventArgs e)
		{
			SetState(0);
		}

		/// <summary>
		/// 加载评论列表
		/// </summary>
		void LoadComments()
		{
			List<Comments> list = new List<Comments>();

			if (ArticleID == null)
			{
				CommentPager.RecorderCount = CommentsHelper.QueryAllCommentsCount();
				list = CommentsHelper.QueryAllComments(CommentPager.Begin, CommentPager.Count, null);

				SummaryLabel.Text = "管理全部评论";
			}
			else
			{
				CommentPager.RecorderCount = CommentsHelper.ArticleIDQueryCommentsCount(ArticleID);
				list = CommentsHelper.ArticleIDQueryComments(ArticleID, CommentPager.Begin, CommentPager.Count, null);

				try
				{
					SummaryLabel.Text = String.Format("管理文章：“{0}”的评论", ArticleHelper.GetArticle(ArticleID, new string[] { "Title" }).Title);
				}
				catch
				{
					string chID = ChannelHelper.GetChannelIDByOnlyName(ArticleID);
					Channel ch = ChannelHelper.GetChannel(chID, new string[] { "FullPath" });
					if (ch != null)
					{
						SummaryLabel.Text = String.Format("管理栏目：“{0}”的评论", ch.FullPath);
					}
				}
			}

			foreach (Comments c in list)
			{
				if (c.Content.Length > TitleMaxLength)
				{
					c.Content = String.Format("{0}...", c.Content.Substring(0, TitleMaxLength));
				}

				string[] fields = new string[] { "Title","ModelName" };
				string acticleTitle = "";
				if (!We7Helper.IsEmptyID(c.ArticleID))
				{
					//文章下的评论
					try
					{
						Article ac = ArticleHelper.GetArticle(c.ArticleID, fields);
						if (ac != null)
						{
							acticleTitle = String.Format("{1}:{0}", ac.Title, "System.Article" == ac.ModelName ? "文章" : Model.Core.ModelHelper.GetModelInfoByName(ac.ModelName).Label);
						}
					}
					//栏目下的评论
					catch
					{
						string chID = ChannelHelper.GetChannelIDByOnlyName(c.ArticleID);
						Channel ch = ChannelHelper.GetChannel(chID, new string[] { "FullPath" });
						if (ch != null)
						{
							acticleTitle = String.Format("栏目:{0}", ch.FullPath);
						}
					}
				}

				if (acticleTitle.Length > TitleMaxLength)
				{
					c.ArticleTitle = String.Format("{0}...", acticleTitle.Substring(0, TitleMaxLength));
				}
				else
				{
					c.ArticleTitle = acticleTitle;
				}
			}
			DataGridView.DataSource = list;
			DataGridView.DataBind();
		}

		/// <summary>
		/// 获取选中评论的ID
		/// </summary>
		/// <returns></returns>
		List<string> GetIDs()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < DataGridView.Rows.Count; i++)
			{
				if (((CheckBox)DataGridView.Rows[i].FindControl("chkItem")).Checked)
				{
					list.Add(((Label)(DataGridView.Rows[i].FindControl("lblID"))).Text);
				}
			}
			return list;
		}

		/// <summary>
		/// 启用或禁用评论
		/// </summary>
		/// <param name="state"></param>
		void SetState(int state)
		{
			List<string> ids = GetIDs();

			if (ids.Count < 1)
			{
				MessageLabel.Text = "你没有选择任何一条记录";
				MessagePanel.Visible = true;
				return;
			}

			string aTitle = "";
			string content = "";
			foreach (string id in ids)
			{
				Comments c = new Comments();
				c.ID = id;
				c.State = state;
				CommentsHelper.UpdateComments(c, new string[] { "ID", "State" });

				c = CommentsHelper.GetComment(id, new string[] { "Content" });

				string con = c.Content;
				if (c.Content.Length > 10)
				{
					con = c.Content.Substring(0, 10);
				}
				aTitle += String.Format("{0};", con);
			}

			MessageLabel.Text = string.Format("您已经成功启用{0}条评论", ids.Count.ToString());
			content = string.Format("启用了{0}条评论:“{1}”", ids.Count.ToString(), aTitle);
			if (state == 0)
			{
				MessageLabel.Text = string.Format("您已经成功禁用{0}条评论", ids.Count.ToString());
				content = string.Format("禁用了{0}条评论:“{1}”", ids.Count.ToString(), aTitle);
			}

			AddLog("评论管理", content);

			MessagePanel.Visible = true;
			LoadComments();
		}

	}
}
