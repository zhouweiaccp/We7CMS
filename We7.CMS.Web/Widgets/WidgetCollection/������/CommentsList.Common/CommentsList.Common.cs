using System;
using System.Collections.Generic;
using System.Web;
using We7.CMS.WebControls;
using We7.CMS.Common;
using We7.Framework;
using We7.CMS.Accounts;
using We7.CMS.WebControls.Core;

namespace We7.CMS.Web.Widgets
{
	[ControlGroupDescription(Label = "评论添加列表一体", Icon = "评论添加列表一体", Description = "评论添加列表一体", DefaultType = "CommentsList.Common")]
	public partial class CommentsList_Common : ThinkmentDataControl
	{

		/// <summary>
		/// 记录的总条数
		/// </summary>
		public int RecordCount
		{
			get
			{
				object o = Get("RecordCount") ?? GetListAction().RecordCount;
				return (int)o;
			}
		}

		private string actionID;
		/// <summary>
		/// 当前的ActionID
		/// </summary>
		public string ActionID
		{
			set { actionID = value; }
			get
			{
				if (String.IsNullOrEmpty(actionID))
				{
					actionID = this.ClientID;
				}
				return actionID;
			}
		}
		/// <summary>
		/// 取得Action传回来的值
		/// </summary>
		/// <param name="key">关键字</param>
		/// <returns>字段的数据值</returns>
		public object Get(string key)
		{
			return BaseAction.Get(key, ActionID);
		}
		/// <summary>
		/// 返回列表记录
		/// </summary>
		public List<Comments> Items
		{
			get
			{
				return GetListAction().Records as List<Comments>;
			}
		}

		#region 评论属性
		/// 栏目类业务助手
		/// </summary>
		protected ChannelHelper ChannelHelper
		{
			get { return HelperFactory.GetHelper<ChannelHelper>(); }
		}
		/// <summary>
		/// 评论业务助手
		/// </summary>
		protected CommentsHelper CommentsHelper
		{
			get { return HelperFactory.GetHelper<CommentsHelper>(); }
		}
		/// <summary>
		/// 文章类业务助手
		/// </summary>
		protected ArticleHelper ArticleHelper
		{
			get { return HelperFactory.GetHelper<ArticleHelper>(); }
		}

		/// <summary>
		/// 文章ID
		/// </summary>
		protected string ArticleID
		{
			get
			{
				return ArticleHelper.GetArticleIDFromURL();
			}
		}
		/// <summary>
		/// 通过导航取得文章ID
		/// </summary>
		protected string ArticleIDByRedirect
		{
			get
			{
				if (Session["ArticleComment"] != null)
				{
					string[] listString = Session["ArticleComment"] as string[];
					if (listString[0] == "ArticleCommentID")
					{
						return listString[1].ToString();
					}
					else
					{
						return "";
					}
				}
				else
				{ return ""; }
			}
		}


		/// <summary>
		/// 栏目ID
		/// </summary>
		protected string ChannelID
		{
			get { return ChannelHelper.GetChannelIDFromURL(); }
		}
		/// <summary>
		/// 显示记录条数
		/// </summary>
		[Parameter(Title = "控件每页记录", Type = "Number", DefaultValue = "10")]
		public int PageSize = 10;
		string cssClass;
		/// <summary>
		/// 本控件应用样式
		/// </summary>
		[Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "CommentsList_Common")]
		public string CssClass
		{
			get { return cssClass; }
			set { cssClass = value; }
		}
		string bindColumnID;
		/// <summary>
		///　绑定栏目
		/// </summary>
		[Parameter(Title = "栏目", Type = "Channel", Required = true)]
		public string BindColumnID
		{
			get { return bindColumnID; }
			set { bindColumnID = value; }
		}

		[Parameter(Title = "允许匿名评论", Type = "Boolean", DefaultValue = "1")]
		public bool NoLogin = true;

		public bool ShowForm { get { return NoLogin || IsSignin; } set { NoLogin = value; } }

		bool iSValidate = true;
		/// <summary>
		/// 是否验证
		/// </summary>
		public bool ISValidate
		{
			get { return iSValidate; }
			set { iSValidate = value; }
		}

		/// <summary>
		/// 选择验证
		/// </summary>
		public string SelectValidate
		{
			get
			{
				return ISValidate ? "" : "none";
			}
		}
		/// <summary>
		/// 是否显示名称
		/// </summary>
		public string NameSelect
		{
			get
			{
				if (ArticleID != null && BindColumnID != null && ArticleID != "" && BindColumnID != "")
				{
					return "";//显示
				}
				else
				{
					return "none";//不显示
				}
			}
		}

		/// <summary>
		/// 栏目名称
		/// </summary>
		public string ChannelName { get { return ""; } set { ChannelName = value; } }

		/// <summary>
		/// 检测用户是否登录
		/// </summary>
		protected bool IsSignin
		{
			get { return Security.IsAuthenticated(); }
		}
		protected string LoginName
		{
			get
			{
				if (IsSignin)
				{
					return "none";
				}
				else
				{
					return "block";
				}
			}
		}
		/// <summary>
		/// 栏目Url
		/// </summary>
		public string ChannelUrl
		{
			get
			{
				string channelUrl = "";
				if (BindColumnID != null && ArticleID != null && BindColumnID != "" && ArticleID != "")
				{
					channelUrl = String.Format("{0}{1}", ChannelHelper.GetFullUrl(BindColumnID), We7Helper.GUIDToFormatString(ArticleID) + ".html");
				}
				return channelUrl;
			}
		}

		/// <summary>
		/// 评论的条数
		/// </summary>
		public int CommentsCount
		{
			get
			{
				return CommentsHelper.ArticleIDQueryCommentsCount(ArticleID);
			}
		}

		/// <summary>
		/// 文章标题
		/// </summary>
		public string Title
		{
			get
			{
				if (ArticleIDByRedirect != null && ArticleIDByRedirect != "")
				{
					return ArticleHelper.GetArticleName(ArticleIDByRedirect);
				}
				return String.Empty;
			}
		}

		ListCommentAction action;
		/// <summary>
		/// 取得评论Action
		/// </summary>
		/// <returns>评论Action</returns>
		public IListAction GetListAction()
		{
			if (action == null)
			{
				action = GetAction<ListCommentAction>();
				if (action == null)
				{
					action = new ListCommentAction();
					action.PageSize = PageSize;
					action.PageIndex = 1;
					action.ArticleIDByRedirect = ArticleIDByRedirect;
					action.ArticleID = ArticleID;
					action.Execute();

				}
			}
			return action;
		}
		public int PageIndex
		{
			get
			{
				if (action != null)
				{
					return action.PageIndex;
				}
				else
				{
					return 1;
				}
			}
		}
		public int PageCount
		{
			get
			{
				int count = RecordCount / PageSize;
				if (RecordCount % PageSize != 0)
					count++;
				return count;
			}
		}
		/// <summary>
		/// 取得当前的Action
		/// </summary>
		/// <typeparam name="T">Action类型</typeparam>
		/// <returns>Action对象</returns>
		public T GetAction<T>()
		{
			return BaseAction.GetAction<T>(ActionID);
		}
		/// <summary>
		/// 是否显示消息(已经加了Display)
		/// </summary>
		public string MessageDisplay
		{
			get
			{
				string message = (Get("Message") ?? "").ToString();
				return String.IsNullOrEmpty(message) ? "display:none" : "";
			}
		}
		private int minActionID = 0;
		/// <summary>
		/// 创建新的ActionID
		/// </summary>
		public string CreateActionID()
		{
			ActionID = this.ClientID + minActionID;
			minActionID++;
			return ActionID;
		}
		/// <summary>
		/// 获取用户ID
		/// </summary>
		protected virtual string AccountID
		{
			get { return Security.CurrentAccountID; }
		}
		#endregion
	}
}