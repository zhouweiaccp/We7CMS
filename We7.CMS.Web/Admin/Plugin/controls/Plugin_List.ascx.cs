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
using RPlugInfo = We7.CMS.Web.Admin.PluginService.RemotePluginInfo;
using System.Collections.Generic;
using System.Threading;
using We7.CMS.Config;
using We7.CMS.Plugin;
using We7.CMS;
using We7.CMS.Web.Admin.PluginService;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin.Plugin.controls
{
    public partial class Plugin_List : System.Web.UI.UserControl
    {
        private PluginHelper helper;
        protected PluginMessage message;

        protected void Page_Load(object sender, EventArgs e)
        {
            message = new PluginMessage(PluginType);
            helper = new PluginHelper(PluginType);
            if (!IsPostBack)
            {
                DataBind();
            }
        }

        /// <summary>
        /// 插件的查询类型
        /// </summary>
        public PluginQueryType QueryTytpe
        {
            get
            {
                if (ViewState["WE$QueryType"] == null)
                {
                    throw new Exception("没有设置" + message.PluginLabel + "的查询类型！");
                }
                return (PluginQueryType)ViewState["WE$QueryType"];
            }
            set
            {
                ViewState["WE$QueryType"] = value;
            }
        }

        public string UQueryText
        {
            get
            {
                return Request.QueryString["qtext"];
            }
        }

        public string UQueryType
        {
            get
            {
                return Request.QueryString["qtype"];
            }
        }

        public We7.CMS.Common.PluginType PluginType
        {
            get
            {
                if (ViewState["WE7$PluginType"] == null)
                {
                    ViewState["WE7$PluginType"] = We7.CMS.Common.PluginType.PLUGIN;
                }
                return (We7.CMS.Common.PluginType)ViewState["WE7$PluginType"];
            }
            set
            {
                ViewState["WE7$PluginType"] = value;
            }
        }

        /// <summary>
        /// 列表数据绑定
        /// </summary>
        private void DataBind()
        {
            List<RPlugInfo> list = DataSource;
            Pager.RecorderCount = list.Count;

            if (Pager.PageIndex < 0)
                Pager.PageIndex = 0;
            Pager.FreshMyself();

            PluginListGridView.DataSource = list.GetRange(Pager.Begin, Pager.Count);
            PluginListGridView.DataBind();
        }

        protected void Pager_Fired(object sender, EventArgs args)
        {
            DataBind();
        }


        /// <summary>
        /// 返回所有数据
        /// </summary>
        private List<RPlugInfo> DataSource
        {
            get
            {
                PluginService.PluginInfomation info = new PluginService.PluginInfomation();
                info.Url = new We7.CMS.Common.PluginInfo(PluginType).PluginService;

                List<RPlugInfo> list = new List<RPlugInfo>(info.LoadServerInfo(RemotePluginType));
                switch (QueryTytpe)
                {
                    case PluginQueryType.SPCIEL:
                        TitleLiteral.Text = "特色" + message.PluginLabel;
                        list = list.FindAll(delegate(RPlugInfo item)
                        {
                            return item.IsSpecial;
                        });
                        break;
                    case PluginQueryType.HOT:
                        TitleLiteral.Text = "热门" + message.PluginLabel;
                        list.Sort(delegate(RPlugInfo A, RPlugInfo B)
                        {
                            return B.Clicks.CompareTo(A.Clicks);
                        });
                        break;
                    case PluginQueryType.LATEST:
                        TitleLiteral.Text = "最新" + message.PluginLabel;
                        list.Sort(delegate(RPlugInfo A, RPlugInfo B)
                        {
                            return B.CreateTime.CompareTo(A.CreateTime);
                        });
                        break;
                    case PluginQueryType.UPDATED:
                        TitleLiteral.Text = "最近更新" + message.PluginLabel;
                        list.Sort(delegate(RPlugInfo A, RPlugInfo B)
                        {
                            return B.UpdateTime.CompareTo(A.UpdateTime);
                        });
                        break;
                    case PluginQueryType.USERDEFINED:
                        TitleLiteral.Text = "搜索结果";
                        list = list.FindAll(delegate(RPlugInfo item)
                        {
                            if (UQueryType == "1")
                            {
                                return item.Name.Contains(UQueryText.Trim());
                            }
                            else
                            {
                                return item.Author.Contains(UQueryText.Trim());
                            }
                        });
                        break;
                }
                return list;
            }
        }

        protected string GetProcessText(object obj)
        {
            string dir = obj as string;
            if (String.IsNullOrEmpty(dir))
                throw new Exception("服务器上" + message.PluginLabel + "命名不规范");

            return helper.isInstalled(dir) ? "更新" : "安装";

        }

        protected string GetActionText(object obj)
        {
            string dir = obj as string;
            if (String.IsNullOrEmpty(dir))
                throw new Exception("服务器上" + message.PluginLabel + "命名不规范");

            switch (PluginType)
            {
                case We7.CMS.Common.PluginType.PLUGIN:
                    return helper.isInstalled(dir) ? "remoteupdate" : "remoteinstall";
                case We7.CMS.Common.PluginType.RESOURCE:
                    return "insctr";
            }
            return helper.isInstalled(dir) ? "remoteupdate" : "remoteinstall";
        }

        protected string RemoteUrl
        {
            get
            {
                return GeneralConfigs.GetConfig().PluginServer;
            }
        }

        protected PluginService.PluginType RemotePluginType
        {
            get
            {
                return (PluginService.PluginType)System.Enum.Parse(typeof(PluginService.PluginType),PluginType.ToString());
            }
        }
    }
}