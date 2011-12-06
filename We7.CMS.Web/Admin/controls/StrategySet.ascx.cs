using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.CMS.Common.Enum;
using We7.CMS.Config;
using System.Text;
using System.ComponentModel;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
	public partial class StrategySet : BaseUserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Init();
			}
		}

		private List<StrategyInfo> SelectedList
		{
			get
			{
				if (ViewState["SelectedList"] == null)
				{
					ViewState["SelectedList"] = new List<StrategyInfo>();
				}
				return ViewState["SelectedList"] as List<StrategyInfo>;
			}
		}

		private List<StrategyInfo> UnSelectedList
		{
			get
			{
				if (ViewState["UnSelectedList"] == null)
				{
					ViewState["UnSelectedList"] = new List<StrategyInfo>();
				}
				return ViewState["UnSelectedList"] as List<StrategyInfo>;
			}
		}

		/// <summary>
		/// 当前的IPStrategy
		/// </summary>
		public string IPStrategy
		{
			set
			{
				ViewState["IPStrategy"] = value;
			}
			get
			{
				return ViewState["IPStrategy"] as string;
			}
		}

		private event EventHandler afterClick;
		/// <summary>
		/// 点击后的事件
		/// </summary>
		public event EventHandler AfterClick
		{
			add
			{
				afterClick += value;
			}
			remove
			{
				afterClick -= value;
			}
		}

		/// <summary>
		/// 策略类型
		/// </summary>
		public StrategyMode Mode
		{
			get
			{
				if (ViewState["WE$MODE"] == null)
				{
					throw new Exception("Mode不能为空");
				}
				return (StrategyMode)ViewState["WE$MODE"];
			}
			set
			{
				ViewState["WE$MODE"] = value;
			}
		}

		private string ChannelID
		{
			get { return Request["id"]; }
		}

		private string OwnerID
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
							ViewState["$VS_OwnerID"] = a.OwnerID;
						}
					}
					return (string)ViewState["$VS_OwnerID"];
				}
			}
		}

		private string ArticleID
		{
			get { return Request["id"]; }
		}

		/// <summary>
		/// 绑定数据
		/// </summary>
		private void bindData()
		{
			lstSelected.DataSource = SelectedList;
			lstSelected.DataTextField = "Name";
			lstSelected.DataValueField = "Key";
			lstSelected.DataBind();

			lstUnSelected.DataSource = UnSelectedList;
			lstUnSelected.DataTextField = "Name";
			lstUnSelected.DataValueField = "Key";
			lstUnSelected.DataBind();

			StringBuilder sb = new StringBuilder();
			foreach (StrategyInfo info in SelectedList)
			{
				sb.Append(info.Key).Append("|");
			}
			if (sb.Length > 0)
				sb.Remove(sb.Length - 1, 1);

			IPStrategy = sb.ToString();

			if (afterClick != null)
				afterClick(this, EventArgs.Empty);
		}

		private void Remove(List<StrategyInfo> list, StrategyInfo info)
		{
			StrategyInfo result = list.Find(delegate(StrategyInfo item)
			{
				return item.Key == info.Key;
			});
			if (result != null) list.Remove(result);
		}

		/// <summary>
		/// 数据初始化
		/// </summary>
		private void Init()
		{
			try
			{
				bool fileexist = System.IO.File.Exists(Server.MapPath("/Plugins/IPStrategyPlugin/Plugin.xml"));
				if (!fileexist)
				{
					Messages.ShowMessage(string.Format("如果要管理IP策略请安装IP策略管理插件！<a href=\"javascript:window.top.location.href='/admin/Plugin/PluginAdd.aspx?tab=0&qtext={0}&qtype=1&ptype=PLUGIN';void(0);\">去安装</a>", HttpUtility.UrlEncode("IP策略管理")), true);
				}
				else
				{
					bool isinstalled = new PluginInfo(Server.MapPath("/Plugins/IPStrategyPlugin/Plugin.xml")).IsInstalled;
					if (!isinstalled)
						Messages.ShowMessage("如果要管理IP策略请激活IP策略管理插件！<a href=\"javascript:window.top.location.href='/admin/Plugin/PluginList.aspx';void(0);\">去激活</a>", true);
					else
						Messages.ShowMessage("已激活IP策略管理插件！<a href=\"javascript:window.top.location.href='/Plugins/IPStrategyPlugin/UI/StrateList.aspx';void(0);\">去管理</a>", true);
				}
				switch (Mode)
				{
					case StrategyMode.ARTICLE:
						IPStrategy = ArticleHelper.QueryStrategy(ArticleID);
						break;
					case StrategyMode.CHANNEL:
						IPStrategy = ChannelHelper.QueryStrategy(ChannelID);
						break;
					case StrategyMode.CONVENTION:
						bttnSave.Visible = false;
						break;
				}

				UnSelectedList.AddRange(StrategyConfigs.Instance.Items);

				if (!String.IsNullOrEmpty(IPStrategy))
				{
					String[] strtgyList = IPStrategy.Split("|".ToCharArray());
					foreach (string strtgy in strtgyList)
					{
						StrategyInfo info = StrategyConfigs.Instance[strtgy];
						if (!String.IsNullOrEmpty(strtgy) && info != null)
						{
							SelectedList.Add(info);
							Remove(UnSelectedList, info);
							//UnSelectedList.Remove(info);
						}
					}
				}

				bindData();
			}
			catch (Exception ex)
			{
				Messages.ShowError("数据初始化出错!<Br />错误原因:" + ex.Message);
			}
		}

		protected void bttnRightAll_Click(object sender, EventArgs e)
		{
			try
			{
				SelectedList.AddRange(UnSelectedList);
				UnSelectedList.Clear();

				bindData();
			}
			catch (Exception ex)
			{
				Messages.ShowError("当前操作出错！<br />详细信息：" + ex.Message);
			}
		}

		protected void bttnRight_Click(object sender, EventArgs e)
		{
			try
			{
				foreach (ListItem item in lstUnSelected.Items)
				{
					if (item.Selected)
					{
						StrategyInfo info = StrategyConfigs.Instance[item.Value.Trim()];
						if (info == null)
							continue;
						SelectedList.Add(info);
						//UnSelectedList.Remove(info);
						Remove(UnSelectedList, info);
					}
				}
				bindData();
			}
			catch (Exception ex)
			{
				Messages.ShowError("当前操作出错！<br />详细信息：" + ex.Message);
			}
		}

		protected void bttnLeft_Click(object sender, EventArgs e)
		{
			try
			{
				foreach (ListItem item in lstSelected.Items)
				{
					if (item.Selected)
					{
						StrategyInfo info = StrategyConfigs.Instance[item.Value.Trim()];
						if (info == null)
							continue;
						UnSelectedList.Add(info);
						//SelectedList.Remove(info);
						Remove(SelectedList, info);
					}
				}
				bindData();
			}
			catch (Exception ex)
			{
				Messages.ShowError("当前操作出错！<br />详细信息：" + ex.Message);
			}
		}

		protected void bttnLeftAll_Click(object sender, EventArgs e)
		{
			try
			{
				UnSelectedList.AddRange(SelectedList);
				SelectedList.Clear();

				bindData();
			}
			catch (Exception ex)
			{
				Messages.ShowError("当前操作出错！<br />详细信息：" + ex.Message);
			}
		}

		protected void bttnSave_Click(object sender, EventArgs e)
		{
			try
			{
				switch (Mode)
				{
					case StrategyMode.ARTICLE:
						ArticleHelper.UpdateStrategy(ArticleID, IPStrategy);
						break;
					case StrategyMode.CHANNEL:
						ChannelHelper.UpdateStrategy(ChannelID, IPStrategy);
						break;
					case StrategyMode.CONVENTION:
						Messages.ShowError("目前没有实现　StrategyMode.CONVENTION的添加功能", true);
						break;
				}
				Messages.ShowMessage("保存成功!", true);
			}
			catch (Exception ex)
			{
				Messages.ShowError("保存数据出错！<br />详细信息：" + ex.Message);
			}
		}
	}
}