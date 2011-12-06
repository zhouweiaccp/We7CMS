using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using We7.CMS.Web.Admin.controls;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Model.Core;
using We7.Framework;
using System.Collections.Generic;
using We7.CMS.Accounts;
using We7.CMS.Helpers;

namespace We7.CMS.Web.Admin.Addins
{
	public partial class ArticleEdit : BasePage
	{
		/// <summary>
		/// 是否判断用户权限
		/// </summary>
		protected override bool NeedAnPermission
		{
			get
			{
				if (AccountHelper.GetAccount(AccountID, new string[] { "UserType" }).UserType == 0)
				{
					return false;
				}
				return true;
			}
		}
		/// <summary>
		/// 是否加载左侧菜单
		/// </summary>
		protected override MasterPageMode MasterPageIs
		{
			get
			{
				if (Request["nomenu"] != null)
					return MasterPageMode.NoMenu;
				else
					return MasterPageMode.FullMenu;
			}
		}

		#region 传递过来的参数

		/// <summary>
		/// 选项卡ID
		/// </summary>
		public string TabID
		{
			get { return Request["tab"]; }
		}

		/// <summary>
		/// 是否是Wap
		/// </summary>
		public bool IsWap
		{
			get { return Request["wap"] != null; }
		}

		public string OwnerID
		{
			get
			{
				if (Request["oid"] != null)
					return Request["oid"];
				else
				{
					if (ViewState["$VS_OwnerID"] == null)
					{
						if (ArticleID != null)
						{
							Article a = ArticleHelper.GetArticle(ArticleID, null);
							if (a != null)
							{
								ViewState["$VS_OwnerID"] = a.OwnerID;
							}
						}
					}
					return (string)ViewState["$VS_OwnerID"];
				}
			}
		}

		public int ChannelContentType
		{
			get
			{
				if (ViewState["$ChannelContentType"] == null)
				{
					Article a = ArticleHelper.GetArticle(ArticleID, null);
					ViewState["$ChannelContentType"] = ModelHelper.GetModelValue(a.ModelName);
				}
				return (int)ViewState["$ChannelContentType"];
			}
		}
		public int ArticleContentType
		{
			get
			{
				if (ViewState["$VS_ArticleContentType"] == null)
				{
					if (ArticleID != null)
					{
						Article a = ArticleHelper.GetArticle(ArticleID);
						if (a != null)
						{
							int type = StateMgr.GetStateValue(a.EnumState, EnumLibrary.Business.ProductInfoType);
							ViewState["$VS_ArticleContentType"] = type;
						}
					}
				}
				return (int)ViewState["$VS_ArticleContentType"];
			}
		}
		public string ArticleID
		{
			get { return Request["id"]; }
		}
		#endregion

		#region 菜单优化
		public string InfomationType
		{
			get
			{
				return Request["type"];
			}
		}
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			Article a = ArticleHelper.GetArticle(ArticleID);

			if (a != null
				&& !String.IsNullOrEmpty(a.ModelName)
				&& !Constants.ArticleModelName.Equals(a.ModelName, StringComparison.OrdinalIgnoreCase)
				&& String.Compare(a.ModelName, "Article", true) == 0)
			{
				Response.Redirect(String.Format("~/Admin/Addins/ModelEditor.aspx?notiframe={2}&model={0}&ID={1}", a.ModelName, a.ID, Request["notiframe"]), true);
			}

			if (a != null)
			{

				Channel ch = ChannelHelper.GetChannel(OwnerID, null);
				if (ch != null
					&& !String.IsNullOrEmpty(ch.ModelName)
					&& String.Compare(ch.ModelName, "Article", true) != 0
					&& String.Compare(ch.ModelName, Constants.ArticleModelName) != 0)
				{
					Response.Redirect(String.Format("~/Admin/Addins/ModelEditor.aspx?notiframe={3}&model={0}&ID={1}&oid={2}", ch.ModelName, We7Helper.CreateNewID(), ch.ID, Request["notiframe"]), true);
				}

				Processing p = ProcessHelper.GetArticleProcess(a);
				List<string> contents = AccountHelper.GetPermissionContents(AccountID, a.OwnerID);
				if (a != null && a.AccountID != AccountID && a.State == (int)ArticleStates.Checking && !contents.Contains(p.CurLayerNOToChannel))
				{
					MessageLiteral.Text = "<p class='alertInfo' >本篇文章正在审核流程中，您不能进行编辑。</p>";
					//Response.End();
				}
				else
				{
					MenuTabLabel.Text = BuildNavString();
				}
			}
			else
			{
				MenuTabLabel.Text = BuildNavString();
			}

			PagePathLiteral.Text = BuildPagePath();
		}

		string LoadActiveTabString(string basictag, string tab, string loadName, string loadControl, string tabVisble)
		{
			string tabString = "";
			string strActive = @"<LI class=TabIn id=tab{0} style='display:{2}'><A>{1}</A> </LI>";
			if (ArticleID != null && ArticleID != "")
			{
				tabString += string.Format(strActive, tab, loadName, tabVisble);
				Control ctl = this.LoadControl(loadControl);
				if (ctl is StrategySet)
				{
					((StrategySet)ctl).Mode = StrategyMode.ARTICLE;
				}
				ContentHolder.Controls.Add(ctl);
			}
			else
			{
				if (tab == basictag)
				{
					tabString += string.Format(strActive, tab, loadName, "");
					Control ctl = this.LoadControl(loadControl);
					ContentHolder.Controls.Add(ctl);
				}
			}
			return tabString;
		}
		string LoadLinkTabString(string basictag, string tab, string loadName, string loadControl, string rawurl, string tabVisble)
		{
			string strLink = @"<LI class=TabOut id=tab{0}  style='display:{2}'><A  href={3}>{1}</A> </LI>";
			string tabString = "";
			if (ArticleID != null && ArticleID != "")
			{
				tabString += string.Format(strLink, tab, loadName, tabVisble, We7Helper.AddParamToUrl(rawurl, "tab", tab));
			}
			else
			{
				if (tab == basictag)
				{
					tabString += string.Format(strLink, tab, loadName, "", We7Helper.AddParamToUrl(rawurl, "tab", tab));

				}
			}
			return tabString;
		}
		string TabVisble(string tab)
		{
			string tabVisble = "";
			Article a = ArticleHelper.GetArticle(ArticleID);
			//if (tab=="Article_image")
			//{
			//    if(a.ContentType == (int)TypeOfArticle.NormalArticle)
			//    tabVisble = "none";
			//}
			if (tab == "Article_body" || tab == "Article_file")
			{
				if (a.ContentType == (int)TypeOfArticle.LinkArticle)
					tabVisble = "none";
			}
			bool existAccount = AccountHelper.GetAccount(AccountID, new string[] { "ID" }) != null;
			if (tab == "Article_tag" || tab == "Article_relates")
			{
				//thj,不太明白为什么要隐藏
				//if (existAccount)
				//   tabVisble = "none";
			}
			if (tab == "Article_wap")
			{
				if (existAccount || !IsWap)
					tabVisble = "none";

			}
			return tabVisble;

		}
		/// <summary>
		/// 构建标签项
		/// </summary>
		/// <returns></returns>
		string BuildNavString()
		{
			XmlDocument docType = new XmlDocument();
			if (Application["We7.docType"] == null)
			{
				//加载内容信息类型
				string fileName = Server.MapPath("Config/tab-index.xml");
				if (File.Exists(fileName))
				{
					docType.Load(fileName);
					Application.Add("We7.docType", docType);
				}
			}

			docType = Application["We7.docType"] as XmlDocument;
			string tab = "", basictag = "";
			string loadControl = "../controls/Article_option.ascx";
			string loadName = "基本信息";
			string tabString = "";
			string dispay = "";

			string rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "tab");
			rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "saved");
			if (TabID != null && TabID != "")
				tab = TabID;

			int menuCount = 0;
			XmlNodeList itemNodes = docType.SelectNodes("/configuration/item");
			foreach (XmlNode node in itemNodes)
			{
				string value = node.Attributes["value"].Value;
				if (ChannelContentType == Int32.Parse(value))
				{
					if (node.Attributes["basictag"] != null)
						basictag = node.Attributes["basictag"].Value;

					if (string.IsNullOrEmpty(tab) && node.ChildNodes.Count > 0)
						basictag = node.ChildNodes[0].Attributes["name"].Value; //默认第一个tab为基础tab
					if (string.IsNullOrEmpty(tab)) tab = basictag;

					foreach (XmlNode tagNode in node.ChildNodes)
					{
						if (tagNode.Attributes != null && !tagNode.HasChildNodes)
						{
							menuCount++;
							string tagValue = tagNode.Attributes["name"].Value;
							if (tagValue == "Article_ipStrategy" && !System.IO.File.Exists(Server.MapPath("/Plugins/IPStrategyPlugin/Plugin.xml"))) break;
							loadControl = tagNode.Attributes["control"].Value;
							loadName = tagNode.Attributes["value"].Value;
							if (ArticleID != null && ArticleID != "")
							{
								dispay = TabVisble(tagValue);
							}
							if (tab == tagValue)
							{
								tabString += LoadActiveTabString(basictag, tagValue, loadName, loadControl, dispay);
							}
							else
							{
								tabString += LoadLinkTabString(basictag, tagValue, loadName, loadControl, rawurl, dispay);
							}
						}
					}
				}
			}

			List<ChannelModuleMapping> mappings = HelperFactory.GetHelper<ChannelModuleHelper>().GetMappingByChannelID(OwnerID);
			foreach (ChannelModuleMapping m in mappings)
			{
				ColumnModule module = HelperFactory.GetHelper<ChannelModuleHelper>().GetModule(m.ModuleID);
				if (tab == menuCount.ToString())
				{
					tabString += LoadActiveTabString(basictag, menuCount.ToString(), module.Title, module.Path, String.Empty);
				}
				else
				{
					tabString += LoadLinkTabString(basictag, menuCount.ToString(), module.Title, module.Path, rawurl, String.Empty);
				}
				menuCount++;
			}

			return tabString;
		}

		/// <summary>
		/// 构建当前位置导航
		/// </summary>
		/// <returns></returns>
		string BuildPagePath()
		{
			string pos = "<a href='/admin/' target='_parent'>开始</a> > <a href='../AddIns/Articlelist.aspx?notiframe=1'>文章</a> >  <a href={0}>{1}</a> > {2}";
			string channelName = "";
			string article = "发布文章";
			Channel ch = ChannelHelper.GetChannel(OwnerID, null);
			if (ch != null)
			{
				channelName = ch.Name;
				if (ArticleID != null)
				{
					string url = "#";
					string title = "";
					string editArticle = " 编辑信息“ <a href={0} target=_blank>{1}</a>”";
					Article a = ArticleHelper.GetArticle(ArticleID);
					if (a != null)
					{
						if (a.Title.Length < 10)
							title = a.Title;
						else
							title = a.Title.Substring(0, 5) + "…" + a.Title.Substring(a.Title.Length - 5);

						url = a.GetFullUrlWithChannel(ch.FullUrl);
						article = string.Format(editArticle, url, title);
					}
				}
				string url2 = "../AddIns/ArticleList.aspx?oid=" + OwnerID;
				if (MasterPageIs == MasterPageMode.FullMenu)
					url2 = "../AddIns/ArticleList.aspx?notiframe=1&oid=" + OwnerID;
				return string.Format(pos, url2, "[栏目]" + channelName, article);
			}

			return "";

		}

		protected string CurrentChannelUrl
		{
			get
			{
				if (!string.IsNullOrEmpty(ArticleID))
				{
					Channel ch = ChannelHelper.GetChannel(OwnerID, null);
					if (ch != null)
					{
						return ch.FullUrl;
					}
				}
				return "";
			}
		}
	}
}
