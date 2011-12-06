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

namespace We7.Model.UI.Panel.system
{
	public partial class ListPanel : ModelPanel
	{
		enum CURRENTPANEL { EDITOR, LIST, BOTH }

		/// <summary>
		/// 初始化数据
		/// </summary>
		protected override void InitPanel()
		{
			//UIHelper.RegisterErrorHandler(ErrorMessageHandler);

			BtnBackInit();
			if (Navigation != null)
			{
				phNavigation.Controls.Clear();
				phNavigation.Controls.Add(Navigation);
				Navigation.OnPreCommand += new ModelEventHandler(ucNavigation_OnPreCommand);
			}
			if (Condition != null)
			{
				phCondition.Controls.Clear();
				phCondition.Controls.Add(Condition);
				Condition.OnPreCommand += new ModelEventHandler(ucCondition_OnPreCommand);
				Condition.OnCommandComplete += new ModelEventHandler(ucCondition_OnCommandComplete);
			}
			if (List != null)
			{
				ModeInit();
				phList.Controls.Clear();
				phList.Controls.Add(List);
				List.OnCommandComplete += new ModelEventHandler(ucList_OnCommandComplete);
			}
			if (Command != null)
			{
				phCommand.Controls.Clear();
				phCommand.Controls.Add(Command);
			}
			if (Pager != null)
			{
				phPager.Controls.Clear();
				phPager.Controls.Add(Pager);
				Pager.OnCommand += new ModelEventHandler(ucPager_OnCommand);
			}

			//显示错误信息
			UIHelper.RegisterErrorHandler(delegate(string message, Hashtable ht)
			{
				if (string.IsNullOrEmpty(message))
				{
					msg.ShowError(message);
				}
			});

			//显示消息
			UIHelper.RegisterMessageHandler(delegate(string message, Hashtable ht)
			{
				msg.ShowMessage(message);
			});
		}

		/// <summary>
		/// 返回按钮初始化
		/// </summary>
		/// <returns></returns>
		private HtmlAnchor BtnBackInit()
		{
			HtmlAnchor btnBack = new HtmlAnchor();
			string parentModel = string.Empty;
			foreach (We7Control control in PanelContext.Model.Layout.Panels["edit"].EditInfo.Controls)
			{
				if (control.Type == "RelationSelect")
				{
					parentModel = control.Params["model"];
					break;
				}
			}
			btnBack.Attributes.Add("class", "btnBack");
			if (string.IsNullOrEmpty(parentModel))
			{
				btnBack.HRef = "javascript:history.go(-1);void(0);";
				btnBack.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
			}
			else
				btnBack.HRef = string.Format("/admin/addins/Modellist.aspx?notiframe=1&model={0}", parentModel);
			btnBack.Attributes.Add("title", "返回");
			HtmlImage img = new HtmlImage();
			img.Src = "/admin/images/back.png";
			btnBack.Controls.Add(img);
			return btnBack;
		}

		/// <summary>
		/// 列表模式初始化
		/// </summary>
		private void ModeInit()
		{
			phMode.Controls.Clear();
			HtmlGenericControl div = new HtmlGenericControl("div");
			div.Attributes.Add("class", "modeStyle");
			int tempMode = -1;
			foreach (Group group in PanelContext.Panel.ListInfo.Groups)
			{
				string curMode = We7Helper.GetParamValueFromUrl(Request.RawUrl, "mode");
				if (!string.IsNullOrEmpty(curMode))
					int.TryParse(curMode, out tempMode);
				if (group.Enable)
				{
					if (tempMode < 0) tempMode = group.Index;
					List.GroupIndex = tempMode;
				}
				HtmlAnchor a = new HtmlAnchor();
				a.InnerText = group.Name;
				if (group.Index == tempMode) a.Attributes.Add("class", "cur");
				a.HRef = We7Helper.AddParamToUrl(Request.RawUrl, "mode", group.Index.ToString());
				if (group.Index == 0 || group.Enable)
					div.Controls.Add(a);
			}
			div.Controls.Add(BtnBackInit());
			phMode.Controls.Add(div);
		}

        /// <summary>
        /// 执行命令前，在这里进行multirow多行值传递
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
		protected override void DoPreCommand(object sender, ModelEventArgs args)
		{
			if (args.CommandArguments != null)
			{
				string cmdarg = args.CommandArguments.ToString();
				if (cmdarg.ToLower() == "multirow")
				{
					args.PanelContext.State = List.GetDataKeys();
				}
			}
		}

		protected override void DoCommandComplete(object sender, ModelEventArgs args)
		{
			if (args.CommandArguments != null)
			{
				string cmdarg = args.CommandArguments.ToString();
				if (cmdarg.ToLower() == "multirow")
				{
					Condition.Refresh();
				}
			}
		}

		void ucNavigation_OnPreCommand(object sender, ModelEventArgs args)
		{
		}

		void ucPager_OnCommand(object sender, ModelEventArgs args)
		{
			args.Disable = true;
			Condition.SetPageIndex((int)args.CommandArguments);
			Condition.Refresh();
		}

		void ucList_OnCommandComplete(object sender, ModelEventArgs args)
		{
			if (args.CommandName == "get")
			{
				Editor.SetData(args.State as DataRow, args.PanelContext.DataKey.Values);
			}
			else
			{
				Condition.Refresh();
			}
		}

		void ucCondition_OnCommandComplete(object sender, ModelEventArgs args)
		{
			ListResult result = args.State as ListResult;
			if (result != null)
			{
				List.BindData(result);
				Condition.SetPageIndex(result.PageIndex);
				Pager.RecordCount = result.RecoredCount;
				Pager.CurrentPageIndex = result.PageIndex;
			}
		}

		void ucCondition_OnPreCommand(object sender, ModelEventArgs args)
		{
		}

		void ucEditor_OnPreCommand(object sender, ModelEventArgs args)
		{
		}

		void ucEditor_OnCommandComplete(object sender, ModelEventArgs args)
		{
			Condition.Refresh();
		}

		void command_OnCommand(object sender, ModelEventArgs args)
		{

		}
	}
}