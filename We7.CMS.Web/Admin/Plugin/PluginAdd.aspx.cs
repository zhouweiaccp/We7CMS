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
using We7.CMS.Web.Admin.Plugin.controls;
using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin.Modules.Plugin
{
    public partial class PluginAdd : BasePage
    {
        private PluginMessage message;
        private int ShopListPageSize = 10;

        public string TabID
        {
            get { return Request["tab"]; }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.FullMenu;
            }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            MenuTabLabel.Text = BuildNavString();
            PagePathLiteral.Text = BuildPagePath();
            message = new PluginMessage(PluginType);
            Init();
        }

        private void Init()
        {
            NameLabel.Text = message.InstallTitle;
            SummaryLabel.Text = message.InstallSummary;
            InfoLiteral.Text = message.InstallInfo;
        }


        internal string BuildNavString()
        {
            string strActive = @"<LI class=TabIn id=tab{0} style='display:{2}'><A>{1}</A> </LI>";
            string strLink = @"<LI class=TabOut id=tab{0}  style='display:{2}'><A  href={3}>{1}</A> </LI>";
            int tab = 1;
            string tabString = "";
            string dispay = "";
            string rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "tab");
            rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "saved");

            ContentHolder.Controls.Clear();

            if (TabID != null && We7Helper.IsNumber(TabID))
                tab = int.Parse(TabID);

            if (tab == 1)
            {
                tabString += string.Format(strActive, 1, "起始页", dispay);
                Control ctl = this.LoadControl("~/Admin/Plugin/controls/Plugin_Start.ascx");
                Plugin_Start pctr = ctl as Plugin_Start;
                if (pctr != null)
                    pctr.PluginType = PluginType;
                ContentHolder.Controls.Add(ctl);
            }
            else
                tabString += string.Format(strLink, 1, "起始页", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));


            if (IsQuery)
            {
                if (tab == 0)
                {
                    tabString += string.Format(strActive, 0, "搜索结果", dispay);
                    //AddListControl(PluginQueryType.USERDEFINED);
                    AddListCtr(9);
                }
            }

            if (tab == 2)
            {
                tabString += string.Format(strActive, 2, "待授权插件", dispay);
                LoadMyPluginsCtr();
            }
            else
                tabString += string.Format(strLink, 2, "待授权插件", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "2"));

            if (tab == 7)
            {
                tabString += string.Format(strActive, 7, "注册到本站的插件", dispay);
                LoadRegistedCtr();
            }
            else
                tabString += string.Format(strLink, 7, "注册到本站的插件", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "7"));


            if (tab == 3)
            {
                tabString += string.Format(strActive, 3, "精选插件", dispay);
                //AddListControl(PluginQueryType.HOT);
                AddListCtr(1);
            }
            else
                tabString += string.Format(strLink, 3, "精选插件", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "3"));


            if (tab == 4)
            {
                tabString += string.Format(strActive, 4, "热门插件", dispay);
                //AddListControl(PluginQueryType.LATEST);
                AddListCtr(2);
            }
            else
                tabString += string.Format(strLink, 4, "热门插件", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "4"));


            if (tab == 5)
            {
                tabString += string.Format(strActive, 5, "最新添加", dispay);
                //AddListControl(PluginQueryType.UPDATED);
                AddListCtr(3);
            }
            else
                tabString += string.Format(strLink, 5, "最新添加", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "5"));

            if (tab == 6)
            {
                tabString += string.Format(strActive, 6, "最新更新", dispay);
                //AddListControl(PluginQueryType.UPDATED);
                AddListCtr(4);
            }
            else
                tabString += string.Format(strLink, 6, "最新更新", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "6"));
            return tabString;
        }

        Plugin_ShopList AddListCtr(int type)
        {
            Plugin_ShopList ctr = LoadControl("~/Admin/Plugin/controls/Plugin_ShopList.ascx") as Plugin_ShopList;
            if (ctr != null)
            {
                ctr.PageSize = ShopListPageSize;
                ctr.QueryType = type;
                ContentHolder.Controls.Add(ctr);
            }
            return ctr;
        }

        Plugin_List AddListControl(PluginQueryType type)
        {
            Plugin_List ctr = null;
            Control ctl = this.LoadControl("~/Admin/Plugin/controls/Plugin_List.ascx");
            if (ctl is Plugin_List)
            {
                ctr = (Plugin_List)ctl;
                ctr.QueryTytpe = type;
                ctr.PluginType = PluginType;
            }
            ContentHolder.Controls.Add(ctl);
            return ctr;
        }

        Plugin_MyPlugins LoadMyPluginsCtr()
        {
            Plugin_MyPlugins ctr = LoadControl("~/Admin/Plugin/controls/Plugin_MyPlugins.ascx") as Plugin_MyPlugins;
            ContentHolder.Controls.Add(ctr);
            return ctr;
        }

        Plugin_Registed LoadRegistedCtr()
        {
            Plugin_Registed ctr = LoadControl("~/Admin/Plugin/controls/Plugin_Registed.ascx") as Plugin_Registed;
            ContentHolder.Controls.Add(ctr);
            return ctr;
        }

        /// <summary>
        /// 构建当前位置导航
        /// </summary>
        /// <returns></returns>
        internal string BuildPagePath()
        {
            string pos = "<a href='PluginList.aspx?pltype=" + PluginType + "'>插件<a> > <a >添加插件</a>";
            return pos;
        }

        private bool isQuery = false;
        internal bool IsQuery
        {
            get
            {
                return !String.IsNullOrEmpty(Request["tab"]) && Request["tab"].Trim() == "0" || isQuery;
            }
            set
            {
                isQuery = value;
            }
        }

        private PluginType PluginType
        {
            get
            {
                if (ViewState["WE7$PluginType"] == null)
                {
                    string pltype = Request["pltype"];
                    if (String.IsNullOrEmpty(pltype))
                    {
                        ViewState["WE7$PluginType"] = PluginType.PLUGIN;
                    }
                    else
                    {
                        switch (pltype.ToLower().Trim())
                        {
                            case "constrol":
                                ViewState["WE7$PluginType"] = PluginType.RESOURCE;
                                break;
                            case "plugin":
                                ViewState["WE7$PluginType"] = PluginType.PLUGIN;
                                break;
                            default:
                                ViewState["WE7$PluginType"] = PluginType.PLUGIN;
                                break;
                        }
                    }
                }
                return (PluginType)ViewState["WE7$PluginType"];
            }
        }

        internal string queryText, queryType;
    }
}
