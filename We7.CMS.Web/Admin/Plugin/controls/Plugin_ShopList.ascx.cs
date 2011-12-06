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
using We7.Framework.Config;
using System.IO;

namespace We7.CMS.Web.Admin.Plugin.controls
{
    public partial class Plugin_ShopList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }



        public void BindData()
        {
            ShopService.ShopService service = new ShopService.ShopService();
            service.Url = GeneralConfigs.GetConfig().ShopService.TrimEnd('/').TrimEnd('\\') + "/Plugins/ShopPlugin/ShopService.asmx";

            try
            {
                if (QueryType == 9)
                {
                    PluginListGridView.DataSource = service.QueryPlugins(Request["qtext"]);
                }
                else
                {
                    PluginListGridView.DataSource = service.GetPlugins(QueryType, PageSize);
                }
                PluginListGridView.DataBind();
            }
            catch (Exception ex)
            {
                Messages.ShowError("与" + GeneralConfigs.GetConfig().ShopService + "的插件服务连接错误！");
            }
        }

        public int QueryType { get; set; }

        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        public string GetCaption()
        {
            switch (QueryType)
            {
                case 1:
                    return "推荐插件";
                case 2:
                    return "最新插件";
                case 3:
                    return "最新添加";
                case 4:
                    return "最新更新";
                case 9:
                    return "搜索结果";
            }
            return String.Empty;
        }

        public string ShopSite
        {
            get { return GeneralConfigs.GetConfig().ShopService; }
        }

        protected string GetStar(object point)
        {
            int intPoint = 0;
            if (point != null)
            {
                int.TryParse(point.ToString(), out intPoint);
            }
            if (intPoint > 5000)
            {
                intPoint = 5;
            }
            else if (intPoint >= 400)
            {
                intPoint = 4;
            }
            else if (intPoint >= 300)
            {
                intPoint = 3;
            }
            else if (intPoint >= 20)
            {
                intPoint = 2;
            }
            else if (intPoint >= 1)
            {
                intPoint = 1;
            }
            else
            {
                intPoint = 0;
            }
            return String.Format("<img src='/admin/images/star{0}.png' />", intPoint);
        }

        protected string GetBuyLink(object id)
        {
            return GeneralConfigs.GetConfig().ShopService.TrimEnd('/')+"/Plugins/ShopPlugin/UI/ReadyBuy.aspx?show=gobuy&amp;pid="+id;            
        }
    }
}