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
using We7.CMS.Config;
using System.Text;
using System.Collections.Generic;
using RPluginInfo = We7.CMS.Web.Admin.PluginService.RemotePluginInfo;
using System.Reflection;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin.Modules.Plugin
{
    public partial class PluginDetails : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }
        protected string Version, Author, Update, Compatible, DownLoads,TitleText;
        protected int updatetype;

        public string TabID
        {
            get { return Request["tab"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MenuTabLabel.Text = BuildNavString();
        }

        string BuildNavString()
        {
            string strActive = @"<LI class=TabIn id=tab{0} style='display:{2}'><A>{1}</A> </LI>";
            string strLink = @"<LI class=TabOut id=tab{0}  style='display:{2}'><A  href={3}>{1}</A> </LI>";
            int tab = 1;
            string tabString = "";
            string dispay = "";
            string rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "tab");
            rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "saved");

            if (TabID != null && We7Helper.IsNumber(TabID))
                tab = int.Parse(TabID);

            //Article a = ArticleHelper.GetArticle(ChannelID);
            PluginInfo info = null,infoLocale=null;
            string isRemote=Request.QueryString["remote"];
            if (!String.IsNullOrEmpty(isRemote) && isRemote.Trim() == "1")
            {
                info = GetRemotePluginInfo();
            }
            else
            {
                info= PluginInfoCollection.CreateInstance(PluginType)[Request.QueryString["key"]];
            }

            InitLeftDiv(info);
            ContentLiteral.Text = "";

            if (tab == 1)
            {
                tabString += string.Format(strActive, 1, "简介", dispay);

                ContentLiteral.Text = info != null ? info.Description : "";
            }
            else
                tabString += string.Format(strLink, 1, "简介", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));

            if (tab == 2)
            {
                tabString += string.Format(strActive, 2, "安装", dispay);
                ContentLiteral.Text = info != null ? info.Deployment.Introduction : "";
            }
            else
                tabString += string.Format(strLink, 2, "安装", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "2"));


            if (tab == 3)
            {
                tabString += string.Format(strActive, 3, "截图", dispay);
               
                if (info != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string src in info.Snapshot)
                        sb.Append(String.Format("<img src='{2}/Temp/{0}/{1}' ></img>",Request.QueryString["key"],src,GeneralConfigs.GetConfig().PluginServer));
                    ContentLiteral.Text = sb.ToString();
                }
            }
            else
                tabString += string.Format(strLink, 3, "截图", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "3"));


            if (tab == 4)
            {
                tabString += string.Format(strActive, 4, "其它", dispay);
                ContentLiteral.Text = info != null ? info.Others : "";
            }
            else
                tabString += string.Format(strLink, 4, "其它", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "4"));         

            return tabString;
        }

        private PluginInfo GetRemotePluginInfo()
        {
            PluginService.PluginInfomation pluginInfo=new PluginService.PluginInfomation();
            pluginInfo.Url = new PluginInfo(PluginType).PluginService;

            PluginInfo result = null;
            RPluginInfo[] infos = pluginInfo.LoadServerInfo(RemotePluginType);
            foreach (RPluginInfo info in infos)
            {
                if (info.Directory == Request.QueryString["key"].Trim())
                {
                    result = new PluginInfo();
                    Clone(info, result);
                    result.Deployment = new Deployment();
                    Clone(info.Deployment, result.Deployment);
                    foreach (string str in info.Snapshot)
                    {
                        result.Snapshot.Add(str);
                    }
                    pluginInfo.CheckTempFile(info.Directory,RemotePluginType);
                }
            }
            return result;
        }

        private void Clone(object source,object target)
        {
            Type pType = source.GetType();
            Type rType = target.GetType();

            foreach (PropertyInfo t in rType.GetProperties())
            {
                PropertyInfo pinfo = pType.GetProperty(t.Name);
                if (pinfo!=null&&t.PropertyType.FullName.Equals(pinfo.PropertyType.FullName))
                {
                    if (pinfo != null)
                    {
                        object value = pType.GetProperty(t.Name).GetValue(source, null);
                        t.SetValue(target, value, null);
                    }
                }
            }
        }

        protected void InitLeftDiv(PluginInfo info)
        {
            if (info == null)
                return;
            Version = info.Version;
            Author = info.Author;
            Update = info.UpdateTime.ToString("yyyy年MM月dd日");
            Compatible = info.Compatible;
            DownLoads = info.Clicks.ToString();

            info=PluginInfoCollection.CreateInstance(PluginType)[Request.QueryString["key"]];

            TitleText= info== null ? "现在安装" : "现在更新";
            if (info == null)
            {
                switch (PluginType)
                {
                    case PluginType.RESOURCE:
                        updatetype = 10;
                        break;
                    case PluginType.PLUGIN:
                        updatetype = 0;
                        break;
                    default:
                        updatetype = 10;
                        break;
                }
            }
            else
            {
                switch (PluginType)
                {
                    case PluginType.RESOURCE:
                        updatetype = info.IsLocal ? 4 : 3;
                        break;
                    case PluginType.PLUGIN:
                        updatetype = info.IsLocal ? 2 : 1;
                        break;
                    default:
                        updatetype = info.IsLocal ? 2 : 1;
                        break;
                }
            }
        }

        protected PluginType PluginType
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

        protected PluginService.PluginType RemotePluginType
        {
            get
            {
                return (PluginService.PluginType)System.Enum.Parse(typeof(PluginService.PluginType),PluginType.ToString());
            }
        }


    }
}
