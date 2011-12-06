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
using System.Xml;
using System.IO;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using We7.Framework.Config;
using We7.Framework;
using We7.Framework.Util;
using We7.Model.UI.Data;
using We7.Framework.Zip;

namespace We7.CMS.Web.Admin
{
	public partial class ArticleListControl : BaseUserControl
	{
		static int MAXTITLELENGTH = 60;

		#region 公共属性
		public bool IsMyArticle
		{
			get
			{
				if (ViewState["$Article_IsMyArticle"] == null)
					return false;
				else
					return (bool)ViewState["$Article_IsMyArticle"];
			}
			set { ViewState["$Article_IsMyArticle"] = value; }
		}

		/// <summary>
		/// 内容类型：文章、产品、下载等; -1为空
		/// </summary>
		public int ContentType
		{
			get
			{
				if (ViewState["$Article_ContentType"] != null && We7Helper.IsNumber(ViewState["$Article_ContentType"].ToString()))
					return int.Parse(ViewState["$Article_ContentType"].ToString());
				else
				{
					if (Request["type"] != null && We7Helper.IsNumber(Request["type"].ToString()))
						return int.Parse(Request["type"].ToString());
					else
					{
						if (OwnerChannel != null)
						{
							int type = StateMgr.GetStateValue(OwnerChannel.EnumState, EnumLibrary.Business.ChannelContentType);
							return type;
						}
						return -1;
					}
				}
			}
			set
			{
				ViewState["$Article_ContentType"] = value;
			}
		}

		#endregion

		#region 私有属性
		protected string OwnerID
		{
			get
			{
				string oid = Request["oid"];
				if (oid == null || oid.ToString() == "" || oid == "null")
				{
					if (IsWap)
						return We7Helper.EmptyWapGUID;
					else
						return We7Helper.EmptyGUID;
				}
				return oid.Replace("{{", "{").Replace("}}", "}");
			}
		}

		Channel OwnerChannel
		{
			get
			{
				if (We7Helper.IsEmptyID(OwnerID))
					return null;
				else
				{
					if (ViewState["$Article_OwnerChannel"] == null)
					{
						Channel ch = ChannelHelper.GetChannel(OwnerID, null);
						ViewState["$Article_OwnerChannel"] = ch;
						return ch;
					}
					else
						return (Channel)ViewState["$Article_OwnerChannel"];
				}
			}
		}

		protected bool IsWap
		{
			get
			{
				if (Request["wap"] != null && Request["wap"].ToString() == "1")
					return true;
				else
					return false;
			}
		}

		protected bool IncludeSubChannel
		{
			get
			{
				if (Request["OnlyThis"] != null && Request["OnlyThis"].ToString() == "true")
					return false;
				else
					return true;
			}
		}

		protected string OrderKey
		{
			get
			{
				return (string)ViewState["$Article_OrderKey"];
			}

			set { ViewState["$Article_OrderKey"] = value; }
		}

		protected bool OrderAsc
		{
			get
			{
				if (ViewState["$Article_OrderAsc"] == null)
					return false;
				else
					return (bool)ViewState["$Article_OrderAsc"];
			}

			set { ViewState["$Article_OrderAsc"] = value; }
		}

		protected DateTime BeginDate
		{
			get
			{
				DateTime dt = new DateTime();
				{
					dt = DateTime.MinValue;
				}
				return dt;
			}
		}

		protected DateTime EndDate
		{
			get
			{
				DateTime dt = new DateTime();
				{
					dt = DateTime.MaxValue;
				}
				return dt;
			}
		}

		/// <summary>
		/// 当前过滤条件，文章状态：启用|禁用|审核
		/// </summary>
		protected ArticleStates CurrentState
		{
			get
			{
				ArticleStates s = ArticleStates.All;
				if (Request["state"] != null)
				{
					if (We7Helper.IsNumber(Request["state"].ToString()))
						s = (ArticleStates)int.Parse(Request["state"].ToString());
				}
				return s;
			}
		}


		private int _resultsPageNumber = 1;
		/// <summary>
		/// 当前页
		/// </summary>
		protected int PageNumber
		{
			get
			{
				if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
					_resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
				return _resultsPageNumber;
			}
		}

		protected string Keyword
		{
			get
			{
				return Request["keyword"];
			}
		}

		protected string Tag
		{
			get
			{
				return Request["tag"];
			}
		}

		/// <summary>
		/// 原界面查询控件值字串
		/// 用于前台点击后给后台传来的界面值
		/// </summary>
		protected string QueryString
		{
			get { return Request["querystr"]; }
		}

		ArticleQuery query = null;
		ArticleQuery CurrentQuery
		{
			get
			{
				if (query == null)
				{
					string enumState = "00";

					query = new ArticleQuery();
					query.ModelName = "";
					query.AccountID = AccountID;
					query.ChannelID = OwnerID;
					query.KeyWord = Keyword;
					if (!string.IsNullOrEmpty(Tag))
						query.Tag = "'" + Tag + "'";
					query.BeginDate = BeginDate;
					query.EndDate = EndDate;
					query.ArticleType = 0;
					query.IncludeAllSons = IncludeSubChannel;
					if (IncludeSubChannel && OwnerChannel != null)
						query.ChannelFullUrl = OwnerChannel.FullUrl;

					query.EnumState = enumState;

					string asc = "Asc";
					if (OrderAsc)
					{
						asc = "Asc";
					}
					else
					{
						asc = "Desc";
					}

					if (String.IsNullOrEmpty(OrderKey))
					{
						query.OrderKeys = "Index,IsShow|Desc,Updated|Desc";
					}
					else
					{
						query.OrderKeys = OrderKey + "|" + asc;
					}
					//query.OrderKeys = (OrderKey == null ? "Updated" : OrderKey) + "|" + asc;

					if (IsMyArticle)
						query.IncludeAdministrable = false;
					else
						query.IncludeAdministrable = true;
				}
				return query;
			}
		}


		protected bool NotIframe
		{
			get
			{
				return (Request["notiframe"] != null);
			}
		}

		#endregion

		#region 初始化
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				try
				{
					OtherHandle();
					InitializeButtons();

					PagePathLiteral.Text = BuildPagePath();
					StateLiteral.Text = BuildStateLinks();
					PageLiteral.Text = BuildPageLinks();
					IncludeSubLiteral.Text = BuildIncludeSubLinks();
					BuildSearchControl();
					TagsLiteral.Text = LoadTagDictionary();
					LoadArticles();
				}
				catch (Exception ex)
				{
					Messages.ShowMessage(ex.Message);
					We7.Framework.LogHelper.WriteLog(typeof(ArticleListControl), ex);
				}
			}
		}

		/// <summary>
		/// 跳转型类型，另外处理
		/// </summary>
		void OtherHandle()
		{
			if (!We7Helper.IsEmptyID(OwnerID))
			{
				Channel ch = ChannelHelper.GetChannel(OwnerID, new string[] { "Process", "Type" });

				if (ch != null && (TypeOfChannel)int.Parse(ch.Type) == TypeOfChannel.ReturnChannel)
				{
					Messages.ShowError("此栏目为跳转型栏目。");
					Response.End();
				}
			}
		}

		/// <summary>
		/// 操作按钮状态初始化
		/// 1、administrator全部文档
		/// 2、普通用户，加载有管理权限的文章
		/// 3、普通用户，加载自己文章
		/// 4、按分类加载，还是加载全部
		/// </summary>
		void InitializeButtons()
		{
			bool isnormaluser = IsNormalUser;

			bool canAudit = false;
			bool canWap = false;
			bool canQuote = false;
			bool canManage = AccountID == We7Helper.EmptyGUID;
			bool canCreate = AccountID == We7Helper.EmptyGUID;

			List<string> contents = AccountHelper.GetPermissionContents(AccountID, OwnerID);
			if (OwnerChannel != null)
			{
				if (OwnerChannel != null && OwnerChannel.Process != null && OwnerChannel.Process != "")
					canAudit = true;
			}
			if (!We7Helper.IsEmptyID(OwnerID) && OwnerChannel != null)
			{
				canCreate = contents.Contains("Channel.Input");
				canManage = contents.Contains("Channel.Article");
			}

			if (We7Helper.IsEmptyID(AccountID))
			{
				canCreate = true;
				canManage = true;
			}

			if (We7Helper.IsEmptyID(OwnerID) && (ContentType == -1))
				canCreate = false;
			else
			{
				canCreate = canCreate && true;

				//这儿加了是否是普通用户的验证
				string url = IsNormalUser ? "~/User/ArticleEdit2.aspx" : "../addins/ArticleEdit.aspx";

				if (IsWap) url += "&wap=1";
				if (!We7Helper.IsEmptyID(OwnerID)) url = We7Helper.AddParamToUrl(url, "oid", OwnerID);
				if (ContentType != -1) url = We7Helper.AddParamToUrl(url, "type", ContentType.ToString());
				if (!NotIframe) url = We7Helper.AddParamToUrl(url, "nomenu", "1");
				if (OwnerChannel != null &&
					!String.IsNullOrEmpty(OwnerChannel.ModelName) &&
					!OwnerChannel.ModelName.Equals("Article", StringComparison.OrdinalIgnoreCase) &&
					!OwnerChannel.ModelName.Equals("System.Article", StringComparison.OrdinalIgnoreCase))
				{
					url = "~/Admin/Addins/ModelEditor.aspx?notiframe=1&model=" + OwnerChannel.ModelName + "&ID=" + We7Helper.CreateNewID() + "&oid=" + OwnerChannel.ID;
					if (!NotIframe) url = We7Helper.AddParamToUrl(url, "nomenu", "1");
					NewHyperLink.NavigateUrl = url;
				}
				else
				{
					NewHyperLink.NavigateUrl = url;
				}

				if (OwnerChannel != null)
				{
					canAudit = OwnerChannel.Process == "1";
				}
			}

			if (IsMyArticle) canManage = false;

			canWap = !IsWap && canManage;

			if (OwnerChannel != null && OwnerChannel.Type != null)
			{
				//引用栏目
				if ((TypeOfChannel)int.Parse(OwnerChannel.Type) == TypeOfChannel.QuoteChannel)
					canQuote = true;
			}

			if (!string.IsNullOrEmpty(Tag))
			{
				RemoveTagLi.Visible = true;
				RemoveTagLi.InnerHtml = string.Format("<a href=\"javascript:removeTag('{0}')\">删除标签-{0}</a>", Tag);
			}

			canCreate = canCreate && !canQuote;
			//根据权限启用按钮
			NewHyperLink.Visible = canCreate;
			AuditToHyperLink.Visible = canAudit;
			//WapToHyperLink.Visible = !isnormaluser && canWap;

			UpHyperLink.Visible = !isnormaluser && canManage;
			DownHyperLink.Visible = !isnormaluser && canManage;
			StartHyperLink.Visible = canManage;
			StopHyperLink.Visible = canManage;
			MoveToHyperLink.Visible = !isnormaluser && canManage;
			ShareHyperLink.Visible = canManage;
			TagSpan.Visible = canManage;
			HyperLinkCreateRefer.Visible = !isnormaluser;

			LinkToSpan.Visible = !canQuote && canManage;
			DeleteHyperLink.Visible = canManage;
			GetShareHyperLink.Visible = canManage;
			//QuoteSpan.Visible= QuoteHyperLink.Visible = canQuote;

			RefreshHyperLink.NavigateUrl = Request.RawUrl;
			DataGridView.Columns[3].Visible = !IsNormalUser;

			if (GeneralConfigs.GetConfig().AllowParentArticle)
			{
				ArticleTreeHyperLink.Visible = true;
				ArticleTreeHyperLink.NavigateUrl = string.Format("../addins/ArticleTree.aspx?oid={0}", OwnerID);
			}
			if (!SiteConfigs.GetConfig().SiteGroupEnabled)
			{
				ShareHyperLink.Visible = false;
				GetShareHyperLink.Visible = false;
			}
		}

		private bool isnormaluser, checkuser;
		/// <summary>
		/// 检测是否是普通用户
		/// </summary>
		/// <returns></returns>
		protected bool IsNormalUser
		{
			get
			{
				if (!checkuser)
				{
					if (Request.Url.AbsolutePath.ToLower().StartsWith("/admin/"))
						isnormaluser = false;
					else
					{
						Account account = AccountHelper.GetAccount(AccountID, null);
						isnormaluser = account != null && account.UserType == 1;
						checkuser = true;
					}
				}
				return isnormaluser;
			}
		}

		/// <summary>
		/// 构建当前位置导航
		/// </summary>
		/// <returns></returns>
		string BuildPagePath()
		{
			string pos = "<a href='/admin/' target='_parent'>控制台</a> > <a >内容管理</a> >  <a >文章管理</a> >  {0} > {1}";
			string channelFullPath = "全部文章";
			string action = "文章列表";

			pos = "<a href='/admin/'  target='_parent' >控制台</a> > <a >内容管理</a> >  <a >文章列表</a> >  {0} > {1}";
			action = "文章列表";
			NewLabel.Text = "新增文章";

			if (OwnerID != null)
			{
				Channel ch = ChannelHelper.GetChannel(OwnerID, null);
				if (ch != null)
				{
					channelFullPath = ChannelHelper.FullPathFormat(ch.FullPath, " > ");
				}
			}
			return string.Format(pos, channelFullPath, action);
		}

		/// <summary>
		/// 构建按类型/状态过滤的超级链接字符串
		/// </summary>
		/// <returns></returns>
		string BuildStateLinks()
		{
			string rawUrl = We7Helper.RemoveParamFromUrl(Request.RawUrl, Keys.QRYSTR_PAGEINDEX);
			rawUrl = rawUrl.Replace("{", "{{").Replace("}", "}}");
			string links = "<li> <a href='" + We7Helper.RemoveParamFromUrl(rawUrl, "state") + "'   {0} >全部<span class=\"count\">({1})</span></a> |</li>" +
			"<li><a href='" + We7Helper.AddParamToUrl(rawUrl, "state", Convert.ToString((int)ArticleStates.Started)) + "'{2}>已发布<span class=\"count\">({3})</span></a> |</li>" +
			"<li><a href='" + We7Helper.AddParamToUrl(rawUrl, "state", Convert.ToString((int)ArticleStates.Stopped)) + "'{4}>草稿<span class=\"count\">({5})</span></a> |</li>" +
			"<li><a href='" + We7Helper.AddParamToUrl(rawUrl, "state", Convert.ToString((int)ArticleStates.Checking)) + "'{6}>审核中<span class=\"count\">({7})</span></a>|</li>" +
			"<li><a href='" + We7Helper.AddParamToUrl(rawUrl, "state", Convert.ToString((int)ArticleStates.Overdued)) + "'{8}>过期<span class=\"count\">({9})</span></a></li>";

			string css99, css0, css1, css2, css3;
			css99 = css0 = css1 = css2 = css3 = "";
			if (CurrentState == ArticleStates.All) css99 = " class=\"current\"";
			if (CurrentState == ArticleStates.Started) css0 = " class=\"current\"";
			if (CurrentState == ArticleStates.Stopped) css1 = " class=\"current\"";
			if (CurrentState == ArticleStates.Checking) css2 = " class=\"current\"";
			if (CurrentState == ArticleStates.Overdued) css3 = " class=\"current\"";
			links = string.Format(links, css99, _GetArticleCountByState(ArticleStates.All),
				css0, _GetArticleCountByState(ArticleStates.Started), css1, _GetArticleCountByState(ArticleStates.Stopped),
				css2, _GetArticleCountByState(ArticleStates.Checking), css3, _GetArticleCountByState(ArticleStates.Overdued));
			return links.Replace("{{", "{").Replace("}}", "}");
		}

		/// <summary>
		/// 是否包含子栏目文章切换
		/// </summary>
		/// <returns></returns>
		string BuildIncludeSubLinks()
		{
			string rawUrl = We7Helper.RemoveParamFromUrl(Request.RawUrl, Keys.QRYSTR_PAGEINDEX);
			rawUrl = rawUrl.Replace("{", "{{").Replace("}", "}}");
			string links = "    〖 <li> <a href='" + We7Helper.RemoveParamFromUrl(rawUrl, "onlythis") + "'   {0} >包含子栏目 </a></li>" +
				"|<li> <a href='" + We7Helper.AddParamToUrl(rawUrl, "onlythis", "true") + "' class='popup' title='不包含子栏目文章'  {1} >仅本栏目</a> </li> 〗";
			string css0, css1;
			css0 = css1 = "";
			if (IncludeSubChannel)
				css0 = " class=\"current\"";
			else
				css1 = " class=\"current\"";
			links = string.Format(links, css0, css1);
			return links.Replace("{{", "{").Replace("}}", "}");
		}

		int PageCount
		{
			get
			{
				int count = 0;
				if (Request["pageCount"] != null && Request["pageCount"].ToString() != "")
				{
					string selectPage = Request["pageCount"].ToString();
					count = Int32.Parse(selectPage);
				}
				return count;
			}
		}
		string BuildPageLinks()
		{
			string rawUrl = Request.RawUrl;
			rawUrl = rawUrl.Replace("{", "{{").Replace("}", "}}");
			string links = " 每页显示条数：" + "<li><a href='" + We7Helper.AddParamToUrl(rawUrl, "pageCount", "15") + "'{0} >15</a> </li>" + ",&nbsp;" +
			"<li><a href='" + We7Helper.AddParamToUrl(rawUrl, "pageCount", "30") + "'{1}>30</a></li>" + ",&nbsp;" +
			"<li><a href='" + We7Helper.AddParamToUrl(rawUrl, "pageCount", "50") + "'{2}>50</a></li>";

			string css99, css0, css1;
			css99 = css0 = css1 = "";
			if (PageCount == 15) css99 = " class=\"current\"";
			if (PageCount == 30) css0 = " class=\"current\"";
			if (PageCount == 50) css1 = " class=\"current\"";
			links = string.Format(links, css99,
				css0, css1);
			return links.Replace("{{", "{").Replace("}}", "}");
		}

		string _GetArticleCountByState(ArticleStates state)
		{
			CurrentQuery.State = state;
			int n = ArticleHelper.QueryArtilceCountByAll(CurrentQuery);
			return n.ToString();
		}

		void BuildSearchControl()
		{
			string rawurl = Request.RawUrl;
			rawurl = We7Helper.RemoveParamFromUrl(rawurl, Keys.QRYSTR_PAGEINDEX);
			rawurl = We7Helper.RemoveParamFromUrl(rawurl, "state");
			rawurl = We7Helper.RemoveParamFromUrl(rawurl, "keyword");
			rawurl = We7Helper.RemoveParamFromUrl(rawurl, "querystr");

			string qString = "";
			searchDiv.Visible = false;
			searchBox.Visible = false;

			qString = @"<label class=""hidden"" for=""user-search-input"">搜索{0}:</label>
                <input type=""text"" class=""search-input"" id=""KeyWord"" name=""KeyWord"" value=""""  onKeyPress=""javascript:KeyPressSearch('{1}',event);""  />
                <input type=""button"" value=""搜索"" class=""button"" id=""SearchButton""  onclick=""javascript:doSearch('{1}');""  />";
			qString = string.Format(qString, "文章", rawurl);
			searchBox.Visible = true;
			SearchSimpleLiteral.Text = qString;

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

		#endregion

		#region 获取数据，格式化数据

		/// <summary>
		/// 加载文章列表，考虑以下情况：
		/// 1、administrator全部文档
		/// 2、普通用户，加载有管理权限的文章
		/// 3、普通用户，加载自己文章
		/// 4、按分类加载，还是加载全部
		/// </summary>
		void LoadArticles()
		{
			CurrentQuery.State = (ArticleStates)CurrentState;
			if (Request["pageCount"] != null && Request["pageCount"].ToString() != "")
			{
				string selectPage = Request["pageCount"].ToString();
				int pageCount = Int32.Parse(selectPage);
				ArticleUPager.PageSize = pageCount;
				if (Session["selectPageCount"] == null || Session["selectPageCount"].ToString() != selectPage)
				{
					ArticleUPager.PageIndex = 1;
				}
				else
				{
					ArticleUPager.PageIndex = PageNumber;
				}
				Session["selectPageCount"] = selectPage;
			}
			else
			{
				ArticleUPager.PageIndex = PageNumber;
			}

			ArticleUPager.ItemCount = ArticleHelper.QueryArtilceCountByAll(CurrentQuery);
			ArticleUPager.UrlFormat = We7Helper.AddParamToUrl(Request.RawUrl.Replace("{", "{{").Replace("}", "}}"), Keys.QRYSTR_PAGEINDEX, "{0}");
			ArticleUPager.PrefixText = "共 " + ArticleUPager.MaxPages + "  页 ·   第 " + ArticleUPager.PageIndex + "  页 · ";
			string[] fields = new string[] { "ID", "Title", "OwnerID", "Description", "Tags", "SN", "Source","Author",
                "Permission", "Updated", "ContentType","State", "IsShow","IsImage", "Overdue", "EnumState","Index" ,"ChannelName","ChannelFullUrl","ProcessState","FullChannelPath" };

			List<Article> list = new List<Article>();
			list = ArticleHelper.QueryArtilcesByAll(CurrentQuery, ArticleUPager.Begin - 1, ArticleUPager.Count, fields);
			List<Article> articles = new List<Article>();
			foreach (Article a in list)
			{
				//Channel ch = ChannelHelper.GetChannel(a.OwnerID, null);
				a.LinkUrl = String.Format("{0}{1}", a.ChannelFullUrl, a.FullUrl);
				a.TimeNote = GetTimeNote(a.Updated);
				if (a.Title.Length > MAXTITLELENGTH)
					a.Title = a.Title.Substring(0, MAXTITLELENGTH) + "..";
				a.IsLinkArticle = (TypeOfArticle)a.ContentType == TypeOfArticle.LinkArticle;
				a.ContentType = StateMgr.GetStateValue(a.EnumState, EnumLibrary.Business.ChannelContentType);
				if (ContentType == 21)
				{
					a.UptoTime = GetOverDueTime(a.Overdue);
				}
				else
				{
					a.UptoTime = a.Overdue.ToString("yyyy-MM-dd");
				}
				a.Tags = FormatTags(a.Tags);
				articles.Add(a);
			}
			DataGridView.DataSource = articles;
			StateLiteral.Text = BuildStateLinks();
			DataGridView.DataBind();
		}

		/// <summary>
		/// 格式化标签，使其变为 <a href=''>标签</a> 形式
		/// </summary>
		/// <param name="tags"></param>
		/// <returns></returns>
		private string FormatTags(string tags)
		{
			if (string.IsNullOrEmpty(tags)) return tags;

			string rawurl = Request.RawUrl.Replace("{", "{{").Replace("}", "}}");
			rawurl = We7Helper.AddParamToUrl(rawurl, "tag", "{0}");
			string url = "<a href='{0}'>{1}</a>";
			tags = tags.Replace("''", ",").Replace("'", "");
			string[] taglist = tags.Split(',');
			StringBuilder sb = new StringBuilder();
			foreach (string tag in taglist)
			{
				if (!string.IsNullOrEmpty(tag))
				{
					string myUrl = string.Format(rawurl, tag);
					sb.Append(string.Format(url, myUrl, tag));
					sb.Append(",");
				}
			}
			string result = sb.ToString();
			if (result.EndsWith(",")) result = result.Remove(result.Length - 1);
			return result;
		}


		string GetUrl(string ID, int state)
		{
			//TODO::IsNormalUser
			string url = IsNormalUser ? "ArticleEdit2.aspx?id={0}" : "../addins/ArticleEdit.aspx?id={0}";
			if (IsWap)
				url += "&wap=1";

			Article a = ArticleHelper.GetArticle(ID);
			Processing p = ArticleProcessHelper.GetArticleProcess(a);
			string returnurl = "";
			if (!NotIframe)
			{
				url = We7Helper.AddParamToUrl(url, "nomenu", "1");
			}

			List<string> contents = AccountHelper.GetPermissionContents(AccountID, a.OwnerID);
			if (p.ArticleState == ArticleStates.Checking)
			{
				if (contents.Contains(p.CurLayerNOToChannel))
					returnurl = String.Format(url, ID);
				else
					returnurl = "javascript:alert('文章正在审核流程，不能编辑。');";
			}
			else if (p.ArticleState == ArticleStates.Started && p.ProcessTotalLayer > 0)
				returnurl = "javascript:alert('文章已启用，不能编辑。');";
			else
				returnurl = String.Format(url, ID);

			return returnurl;
		}

		public string GetUrl(string ID, int state, string title)
		{
			//TODO::IsNormalUser
			string urlEdit = IsNormalUser ? "ArticleEdit2.aspx?id={0}" : "../addins/ArticleEdit.aspx?id={0}";
			string urlView = IsNormalUser ? "ArticleEdit2.aspx?id={0}" : "../manage/ArticleView.aspx?id={0}";
			if (IsWap)
				urlEdit += "&wap=1";
			string returnurl = "";
			string linkTitle = "<a href='{0}' target='{2}'>{1}</a>";
			if (!NotIframe)
			{
				urlEdit = We7Helper.AddParamToUrl(urlEdit, "nomenu", "1");
				urlView = We7Helper.AddParamToUrl(urlView, "nomenu", "1");
			}

			Article a = ArticleHelper.GetArticle(ID);
			Processing p = ArticleProcessHelper.GetArticleProcess(a);
			if (p.ArticleState == ArticleStates.Checking)
			{
				List<string> contents = AccountHelper.GetPermissionContents(AccountID, a.OwnerID);
				if (contents.Contains(p.CurLayerNOToChannel))
					returnurl = string.Format(linkTitle, String.Format(urlEdit, ID), title, "_self");
				else
					returnurl = string.Format(linkTitle, String.Format(urlView, ID), title, "_blank");
			}
			else if (p.ArticleState == ArticleStates.Started)
				returnurl = string.Format(linkTitle, String.Format(urlView, ID), title, "_blank");
			else
				returnurl = string.Format(linkTitle, String.Format(urlEdit, ID), title, "_self");

			return returnurl;
		}

		string GetTimeNote(DateTime date)
		{
			if (date > DateTime.Today)
				return date.ToString("今天 HH:mm");
			else if (date > DateTime.Today.AddDays(-1))
				return date.ToString("昨天 HH:mm");
			else
				return date.ToString("yyyy-MM-dd");
		}

		string GetOverDueTime(DateTime date)
		{
			if (date > DateTime.Today)
			{
				TimeSpan c = date.Subtract(DateTime.Now);
				return "剩余" + c.Days.ToString() + "天";
			}

			else
				return "已过期";
		}
		public string ShowAtHomePage(string IsShow)
		{
			string img = "<img src={0} border=0 alt='文章置顶' title='文章置顶' class='popup'  />";
			if (IsShow == "1")
				return string.Format(img, "/admin/images/icon_showhome.gif");
			else
				return "";
		}

		public string GetProcessEable(string id)
		{
			return "true";
		}

		public string GetActionsLink(string ID, int state, string linkUrl)
		{
			string actions = "<span class=\"row-actions\"><u> <span class=\"edit\"><a href=\"{0}\" target=\"_self\" title=\"编辑这篇文章\">编辑</a> | </span><span class=\"view\"><a href=\"{1}\" title=\"查看前台效果\" target=\"_blank\" >查看</a> | </span>";
			if (GeneralConfigs.GetConfig().AllowParentArticle)
			{
				string strCreate = "<span class=\"create\"><a href=\"/admin/addins/ArticleEdit.aspx?type=0&ParentID=" + ID.Replace("{", "{{").Replace("}", "}}") + "\" title=\"创建本条信息的子级信息\" >创建子级文章</a> |</span>";
				if (!NotIframe)
					strCreate = "<span class=\"create\"><a href=\"/admin/addins/ArticleEdit.aspx?nomenu=1&type=0&ParentID=" + ID.Replace("{", "{{").Replace("}", "}}") + "\" title=\"创建本条信息的子级信息\" >创建子级文章</a> |</span>";

				actions += strCreate;
			}
			actions += "<span class=\"delete\"><a href=\"javascript:deleteOneArticle('{2}');\" title=\"删除本条信息\" >删除</a></span> </u></span>";

			return string.Format(actions, GetUrl(ID, state), linkUrl, ID);
		}

		public string GetIcons(string IsImage, string IsLinkArticle, string IsShow)
		{
			string img = "";
			if (IsImage == "1")
				img = string.Format("<img src={0} border=0 alt='图片文章' title='图片文章' class='popup' /> ", "/admin/images/filetype/gif.gif");
			if (IsLinkArticle.ToLower() == "true")
				img += string.Format("<img src={0} border=0 alt='引用文章' title='引用文章' class='popup' /> ", "/admin/images/filetype/link.gif");
			img += ShowAtHomePage(IsShow);
			return img;
		}

		#endregion

		#region 按钮操作动作：1、删除；2、启用；3、审核……

		protected void lnkExprot_Click(object sender, EventArgs e)
		{
			List<string> ids = new List<string>();
			if (DeleteIDTextBox.Text.Length > 0)
			{
				ids.Add(DeleteIDTextBox.Text);
				DeleteIDTextBox.Text = "";
			}
			else
				ids = GetIDs();

			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录");
				return;
			}

			StringBuilder sb = new StringBuilder("SELECT * FROM [Article] WHERE [ID] IN(");
			foreach (string id in ids)
			{
				sb.AppendFormat("'{0}',", id);
			}
			sb.Remove(sb.Length - 1, 1);
			sb.Append(")");
			DataBaseHelper dbhelper = new DataBaseHelper();
			DataTable dt = dbhelper.Query(sb.ToString());
			DataSet ds = new DataSet();
			ds.DataSetName = "We7DataSet";
			dt.TableName = "Design";
			ds.Tables.Add(dt);
			sb.Length = 0;
			string tempDir = Server.MapPath("~/_temp/Data");
			string tempXml = Server.MapPath("~/_temp/Data/Data.xml");
			string tempSchema = Server.MapPath("~/_temp/Data/Schema.xsd");
			DirectoryInfo di = new DirectoryInfo(tempDir);
			if (!di.Exists)
				di.Create();

			ds.WriteXml(tempXml);
			ds.WriteXmlSchema(tempSchema);


			Response.Clear();
			Response.ContentType = "application/zip";
			Response.AddHeader("Content-Disposition", "attachment;filename=Design.zip");
			ZipUtils.CreateZip(tempDir, Response.OutputStream);
			Response.End();

		}


		protected void DeleteBtn_Click(object sender, EventArgs e)
		{
			if (DemoSiteMessage) return;//是否是演示站点
			List<string> ids = new List<string>();
			if (DeleteIDTextBox.Text.Length > 0)
			{
				ids.Add(DeleteIDTextBox.Text);
				DeleteIDTextBox.Text = "";
			}
			else
				ids = GetIDs();

			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录");
				return;
			}
			int delCount = 0;
			int unDelCount = 0;
			string aTitle = "";
			string message = "";
			foreach (string id in ids)
			{
				Article a = ArticleHelper.GetArticle(id, null);
				if (a.State == (int)ArticleStates.Checking || a.State == (int)ArticleStates.Started && !String.IsNullOrEmpty(a.ModelXml))
				{
					message += string.Format("\r\n“{0}”在审核中或已启用，不能删除！", ArticleHelper.GetArticleName(id));
					unDelCount += 1;
				}
				else
				{
					ArticleHelper.DeleteArticle(id);
					// 往全文检索里删除数据
					ArticleIndexHelper.InsertData(id, 2);
					delCount += 1;
				}

				aTitle += String.Format("{0};", a.Title);
			}
			//记录日志
			if (delCount > 0)
			{
				string content = string.Format("删除了{0}篇文章:“{1}”", delCount, aTitle);
				AddLog("文章管理", content);
			}

			message = string.Format("您已经成功删除{0}条记录,{1}条未删除！", delCount, unDelCount) + message;
			Messages.ShowMessage(message);

			LoadArticles();
		}

		protected void StartButton_Click(object sender, EventArgs e)
		{
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录");
				return;
			}
			int count = 0;
			GeneralConfigInfo si = GeneralConfigs.GetConfig();
			foreach (string id in ids)
			{
				//自动共享文章
				if (si.ArticleAutoShare == "true")
				{
					try
					{
						ShareEventFactory.Instance.OnAutoShareArticles(id);
					}
					catch (Exception ex)
					{
						Messages.ShowError("自动同步时发生错误！错误原因：" + ex.Message);
					}
				}

				Article a = ArticleHelper.GetArticle(id);
				//a.ID = id;
				if (!InProcess(a))
				{
					a.State = (int)ArticleStates.Started;
					a.ProcessState = ((int)ProcessStates.EndAudit).ToString();
					ArticleHelper.UpdateArticle(a, new string[] { "ID", "State", "ProcessState" });
					// 往全文检索里更新数据
					ArticleIndexHelper.InsertData(id, 1);
					count += 1;
				}
				else
				{
					try
					{
						string message = string.Format("“{0}”未审核完毕，不能启用！", ArticleHelper.GetArticleName(id));
						Messages.ShowError(message);
					}
					catch
					{
						string messages = string.Format("发生错误：“{0}”未能启用！", ArticleHelper.GetArticleName(id));
						Messages.ShowError(messages);
					}
				}
			}

			//记录日志
			string content = string.Format("启用了{0}篇文章", count.ToString());
			AddLog("文章管理", content);

			Messages.ShowMessage(string.Format("您已经成功启用{0}篇文章", count.ToString()));
			LoadArticles();
		}

		protected void StopButton_Click(object sender, EventArgs e)
		{
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录");
				return;
			}
			int count = 0;
			foreach (string id in ids)
			{
				Article a = ArticleHelper.GetArticle(id);
				if (a.State != (int)ArticleStates.Checking)
				{
					//a.ID = id;
					a.State = (int)ArticleStates.Stopped;
					ArticleHelper.UpdateArticle(a, new string[] { "ID", "State" });
					// 往全文检索里删除数据
					ArticleIndexHelper.InsertData(id, 2);
					count += 1;
				}
			}

			//记录日志
			string content = string.Format("禁用了{0}篇文章", count.ToString());
			AddLog("文章管理", content);

			Messages.ShowMessage(string.Format("您已经成功禁用{0}篇文章", count.ToString()));
			LoadArticles();
		}

		protected void QueryImageButton_Click(object sender, ImageClickEventArgs e)
		{
			ArticleUPager.PageIndex = 0;
			LoadArticles();
		}

		/// <summary>
		/// 引用到
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void LinkToButton_Click(object sender, EventArgs e)
		{
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录");
				return;
			}
			if (We7Helper.IsEmptyID(TargetIDTextBox.Text.Trim()))
			{
				Messages.ShowError("文章不可以引用到根栏目，请选择正确的目标栏目！");
				return;
			}

			string aTitle = "";
			foreach (string id in ids)
			{
				Article a = ArticleHelper.GetArticle(id, null);
				a.ID = We7Helper.CreateNewID();
				a.OwnerID = TargetIDTextBox.Text;
				a.ContentType = (int)TypeOfArticle.LinkArticle;
				a.ContentUrl = a.ChannelFullUrl + a.FullUrl;
				a.Content = "";
				a.SourceID = a.ID;
				Channel ch = ChannelHelper.GetChannel(TargetIDTextBox.Text.Trim(), null);
				if (ch != null)
				{
					a.ChannelFullUrl = ch.FullUrl;
					a.ChannelName = ch.FullPath;
					a.EnumState = ch.EnumState;
					a.ChannelFullUrl = ch.FullUrl;
					a.ChannelName = ch.FullPath;
				}
				ArticleHelper.AddArticle(a);
				// 往全文检索里更新数据
				ArticleIndexHelper.InsertData(a.ID, 0);
				aTitle += String.Format("{0};", a.Title);
			}
			//记录日志
			string content = string.Format("引用了{0}篇文章:“{1}”", ids.Count.ToString(), aTitle);
			AddLog("文章管理", content);

			Messages.ShowMessage(string.Format("您已经成功引用{0}篇文章", ids.Count.ToString()));
			LoadArticles();
		}

		protected void MoveToButton_Click(object sender, EventArgs e)
		{
			if (DemoSiteMessage) return;//是否是演示站点
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录");
				return;
			}
			if (We7Helper.IsEmptyID(TargetIDTextBox.Text.Trim()))
			{
				Messages.ShowError("文章不可以移动到根栏目，请选择正确的目标栏目！");
				return;
			}

			string aTitle = "";
			foreach (string id in ids)
			{
				Article a = new Article();
				a.ID = id;
				a.OwnerID = TargetIDTextBox.Text;
				Channel ch = ChannelHelper.GetChannel(TargetIDTextBox.Text.Trim(), null);
				if (ch != null)
				{
					a.ChannelFullUrl = ch.FullUrl;
					a.ChannelName = ch.FullPath;
				}

				ArticleHelper.UpdateArticle(a, new string[] { "ID", "OwnerID", "ChannelFullUrl", "ChannelName" });
				// 往全文检索里更新数据
				ArticleIndexHelper.InsertData(id, 1);
				Article art = ArticleHelper.GetArticle(id, new string[] { "Title" });
				aTitle += String.Format("{0};", art.Title);
			}
			//记录日志
			string content = string.Format("移动了{0}篇文章:“{1}”", ids.Count.ToString(), aTitle);
			AddLog("文章管理", content);

			Messages.ShowMessage(string.Format("您已经成功移动{0}篇文章", ids.Count.ToString()));
			LoadArticles();
		}

		/// <summary>
		/// 复制到...
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CopyToButton_Click(object sender, EventArgs e)
		{
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录");
				return;
			}
			if (We7Helper.IsEmptyID(TargetIDTextBox.Text.Trim()))
			{
				Messages.ShowError("文章不可以复制到根栏目，请选择正确的目标栏目！");
				return;
			}

			string aTitle = "";
			int success = 0, faild = 0; //成功篇数
			foreach (string id in ids)
			{
				Channel ch = ChannelHelper.GetChannel(TargetIDTextBox.Text.Trim(), null);
				Article a = ArticleHelper.GetArticle(id);
				if (ch != null && a != null)
				{
					//判断文章的类型
					if (string.IsNullOrEmpty(a.ModelName) || Constants.ArticleModelName.Equals(a.ModelName, StringComparison.OrdinalIgnoreCase))
					{
						a.ID = We7Helper.CreateNewID();
						a.OwnerID = TargetIDTextBox.Text;
						a.ChannelFullUrl = ch.FullUrl;
						a.ChannelName = ch.FullPath;
						a.FullChannelPath = ch.FullPath;
						a.Created = DateTime.Now;
						a.Updated = DateTime.Now;
						a.SN = ArticleHelper.CreateArticleSN();

						ArticleHelper.AddArticle(a);
						// 往全文检索里更新数据
						ArticleIndexHelper.InsertData(id, 1);

						aTitle += String.Format("{0};", a.Title);

						success++;
					}
					//else
					//{
					//    faild++;
					//}
				}
			}
			//记录日志
			string content = string.Format("复制了{0}篇文章:“{1}”", success.ToString(), aTitle);
			AddLog("文章管理", content);

			Messages.ShowMessage(string.Format("您已经成功复制了{0}篇文章", success.ToString()));
			LoadArticles();
		}

		protected void AddTagButton_Click(object sender, EventArgs e)
		{
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录!");
				return;
			}
			foreach (string id in ids)
			{
				Article a = ArticleHelper.GetArticle(id);
				if (a.Tags == null || a.Tags.Length == 0)
				{
					TagsHelper.Add(TargetIDTextBox.Text);
					string newTag = "'" + TargetIDTextBox.Text.Trim() + "'";
					if (a.Tags == null) a.Tags = "";
					if (a.Tags.IndexOf(newTag) < 0)
						a.Tags += newTag;
					ArticleHelper.UpdateArticle(a, new string[] { "ID", "Tags" });
					// 往全文检索里更新数据
					ArticleIndexHelper.InsertData(id, 1);
				}


				//int count = ArticleHelper.GetTagCount(TargetIDTextBox.Text, id);
				//if (count == 0)
				//{
				//    ArticleTag t = new ArticleTag();
				//    t.ID = We7Helper.CreateNewID();
				//    t.ArticleID = id;
				//    t.Identifier = TargetIDTextBox.Text;
				//    ArticleHelper.AddTag(t);
				//    string newTag = "'" + TargetIDTextBox.Text.Trim() + "'";
				//    Article a = ArticleHelper.GetArticle(id);
				//    if (a.Tags == null) a.Tags = "";
				//    if (a.Tags.IndexOf(newTag) < 0)
				//        a.Tags += newTag;
				//    ArticleHelper.UpdateArticle(a, new string[] { "ID", "Tags" });
				//    // 往全文检索里更新数据
				//    ArticleIndexHelper.InsertData(id, 1);
				//}
			}
			Messages.ShowMessage(string.Format("您已经成功为{0}篇文章增加‘{1}’标签", ids.Count.ToString(), TargetIDTextBox.Text));
			LoadArticles();
		}

		protected void RemoveTagButton_Click(object sender, EventArgs e)
		{
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录!");
				return;
			}
			foreach (string id in ids)
			{
				Article a = ArticleHelper.GetArticle(id);
				a.Tags = a.Tags.Replace("'" + Tag + "'", ""); ;
				ArticleHelper.UpdateArticle(a, new string[] { "ID", "Tags" });
				// 往全文检索里更新数据
				ArticleIndexHelper.InsertData(id, 1);
			}
			Messages.ShowMessage(string.Format("您已经成功从{0}篇文章删除标签‘{1}’。", ids.Count.ToString(), Tag));
			LoadArticles();
		}

		protected void UpButton_Click(object sender, EventArgs e)
		{
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录");
				return;
			}
			foreach (string id in ids)
			{
				Article a = new Article();
				a.ID = id;
				a.IsShow = 1;
				ArticleHelper.UpdateArticle(a, new string[] { "ID", "IsShow" });
				// 往全文检索里更新数据
				ArticleIndexHelper.InsertData(id, 1);
			}
			Messages.ShowMessage(string.Format("您已经成功为{0}篇文章设置‘置顶’标记", ids.Count.ToString()));
			LoadArticles();
		}

		protected void DownButton_Click(object sender, EventArgs e)
		{
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录");
				return;
			}
			foreach (string id in ids)
			{
				Article a = new Article();
				a.ID = id;
				a.IsShow = 0;
				ArticleHelper.UpdateArticle(a, new string[] { "ID", "IsShow" });
				// 往全文检索里更新数据
				ArticleIndexHelper.InsertData(id, 1);
			}
			Messages.ShowMessage(string.Format("您已经成功为{0}篇文章取消‘置顶’标记", ids.Count.ToString()));
			LoadArticles();

		}

		protected void OrderByTitleButton_Click(object sender, EventArgs e)
		{
			OrderKey = "Title";
			OrderAsc = !OrderAsc;
			LoadArticles();
		}

		protected void OrderByDateButton_Click(object sender, EventArgs e)
		{
			OrderKey = "Updated";
			OrderAsc = !OrderAsc;
			LoadArticles();
		}


		List<string> GetIDs()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < DataGridView.Rows.Count; i++)
			{
				//Response.Write(DataGridView.Rows[i].FindControl("chkItem"));
				if (((CheckBox)DataGridView.Rows[i].FindControl("chkItem")).Checked)
				{
					list.Add(((Label)(DataGridView.Rows[i].FindControl("lblID"))).Text);
				}
			}
			return list;
		}

		protected void WapToButton_Click(object sender, EventArgs e)
		{
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				Messages.ShowError("你没有选择任何一条记录");
				return;
			}
			int InsertCount = 0;
			foreach (string id in ids)
			{
				if (!We7Helper.IsEmptyID(WapOidTextBox.Text.Trim()))
				{
					Article wap = ArticleHelper.GetArticleBySource(WapOidTextBox.Text.Trim(), id);
					if (wap == null)
					{
						Article sourceArticle = ArticleHelper.GetArticle(id, null);
						wap = ArticleHelper.CopyToWapArticle(sourceArticle);
						wap.OwnerID = WapOidTextBox.Text.Trim();
						wap.AccountID = AccountID;
						ArticleHelper.AddArticle(wap);
						// 往全文检索里插入数据
						ArticleIndexHelper.InsertData(wap.ID, 0);
						InsertCount++;
					}
				}
			}
			if (InsertCount == ids.Count)
				Messages.ShowMessage(string.Format("您成功将{0}篇文章发布到wap站点。", ids.Count.ToString()));
			else
				Messages.ShowMessage(string.Format("您成功将{0}篇文章发布到wap站点，有{1}篇文章已经在wap站点存在，无法重复发布！", InsertCount.ToString(), ids.Count - InsertCount));

			LoadArticles();
		}

		public string GetProcessState(string id)
		{
			if (!ChannelProcess(id))
			{
				return "[流转历程]";
			}
			Article a = ArticleHelper.GetArticle(id);
			if (a != null && a.State != 2)
			{
				return "[流转历程]";
			}
			//string channelID = ArticleHelper.GetArticle(id).OwnerID;
			//string channelProcess = ChannelHelper.GetChannel(channelID, null).ProcessLayerNO;
			Processing ap = ArticleProcessHelper.GetArticleProcess(a);
			string processText = "草稿";
			if (ap != null)
				processText = ap.ProcessDirectionText + ap.ProcessText;
			return processText;
		}

		protected void SubmitButton_Click(object sender, EventArgs e)
		{
			List<string> list = new List<string>();
			list = GetIDs();
			int count = 0;
			if (list.Count > 0)
			{
				foreach (string id in list)
				{
					try
					{
						if (ChannelProcess(id))
						{
							Article a = ArticleHelper.GetArticle(id);
							Processing ap = ArticleProcessHelper.GetArticleProcess(a);
							if (ap.ArticleState != ArticleStates.Checking)
							{
								string accName = AccountHelper.GetAccount(AccountID, new string[] { "LastName" }).LastName;
								ap.ProcessState = ProcessStates.FirstAudit;
								ap.ProcessDirection = ((int)ProcessAction.Next).ToString();
								ap.ProcessAccountID = AccountID;
								ap.ApproveName = accName;
								ArticleProcessHelper.SaveFlowInfoToDB(a, ap);
								count++;
							}
						}
					}
					catch
					{ }
				}
			}
			else
			{
				Messages.ShowError("请选择要提交的文章！");
				return;
			}
			string errMsg = "有 {0} 条审核提交未能成功，请检查。原因可能是这些文章的状态不允许审核。";
			if (count > 0)
				Messages.ShowMessage(string.Format("您已经成功提交审核{0}条记录", count));
			if (list.Count > count)
				Messages.ShowError(string.Format(errMsg, list.Count - count));

			LoadArticles();
		}


		public bool ChannelProcess(string id)
		{
			string channelProcessLayerNO = GetProcessLayerNO(id);
			if (channelProcessLayerNO != null && channelProcessLayerNO != "")
			{
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// 是否处于审核中
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		bool InProcess(Article a)
		{
			bool ret = false;
			if (ChannelProcess(a.ID))
			{
				ret = true;
				if (a.State != (int)ArticleStates.Checking && a.ProcessState == ((int)ProcessStates.EndAudit).ToString())
					ret = false;
			}
			return ret;
		}

		string GetProcessLayerNO(string id)
		{
			string channelID = ArticleHelper.GetArticle(id).OwnerID;
			Channel ch = ChannelHelper.GetChannel(channelID, null);
			if (ch != null && ch.Process == "1" && ch.ProcessLayerNO != null)
				return ch.ProcessLayerNO;
			else
				return "";
		}
		#endregion
		#region 共享操作


		protected void GetShareButton_Click(object sender, EventArgs e)
		{
			ShareEventFactory.Instance.OnFetchShareDataCommand(null);
			Messages.ShowMessage("后台获取最新共享信息命令已发出，请稍后进行刷新！");
		}

		protected void ShareButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (IncludeSubChannel)
				{
					Messages.ShowMessage("数据共享时请选择“不包含子栏目”后重新共享！");
					return;
				}
				List<string> list = new List<string>();
				list = GetIDs();
				MoreEventArgs eventArgs = new MoreEventArgs();
				ShareEventFactory.Instance.OnShareActive(list, eventArgs);
				//int count = ManualShareArticles();
				LoadArticles();
				Messages.ShowMessage("您成功共享" + eventArgs.ReturnValue + "条数据");
			}
			catch (Exception ex)
			{
				Messages.ShowError("您共享信息时出错！出错原因：" + ex.Message);
			}
		}


		#endregion


	}
}