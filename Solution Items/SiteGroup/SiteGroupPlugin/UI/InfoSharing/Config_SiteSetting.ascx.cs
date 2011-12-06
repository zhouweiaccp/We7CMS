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
using WebEngine2007.WebServices.WD;
using We7.Plugin.DataSharing;
using System.Collections.Generic;
using We7.Framework;
using We7.CMS;

namespace We7.Plugin.SiteGroupPlugin.InfoSharing
{
    public partial class Config_SiteSetting : BaseUserControl
    {

        WDWebService GetWDWebService()
        {
            WDWebService client = new WDWebService();
            client.Url = CDHelper.SiteConfig.WebGroupServiceUrl;
            return client;
        }

        protected IDHelper IDHelper
        {
            get { return HelperFactory.GetHelper<IDHelper>(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initilize();
            }
        }

        private void Initilize()
        {
            try
            {
                WebEngine2007.WebServices.ID.SitePartnership[] sps;
                System.Text.StringBuilder sb;
                string siteID = CDHelper.GetSiteID();

                #region 绑定共享站点数据源
                //查找当前站群下所有可共享站点
                WDWebService wws = GetWDWebService();
                WebSite[] sites = wws.GetWebSites();
               
                //Tips:使用泛型集合类，可以避免检查数组越界
                IList<WebSite> sitesCopy = new List<WebSite>();
                foreach (WebSite item in sites)
                {
                    if (item.ID!=siteID)
                    {
                        if (item.IpOrDomain == 1 && item.ZoneName.ToLower().Equals("localhost"))
                            item.Url = item.ZoneName + ":" + item.Port.ToString();

                        sitesCopy.Add(item);
                    }
                }
                SiteListSharing.DataSource = sitesCopy;
                SiteListSharing.DataBind();

                //查找已创建共享站点
                object objEnum = (object)EnumLibrary.SitePartnership.Sharing;
                sps = null;
                sps = IDHelper.GetSharingSites(siteID, objEnum);
                sb = new System.Text.StringBuilder();
                EnumLibrary.SiteValidateStyle svs = EnumLibrary.SiteValidateStyle.NoMustReceived;
                if (sps != null)
                {
                    foreach (WebEngine2007.WebServices.ID.SitePartnership sp in sps)
                    {
                        svs = (EnumLibrary.SiteValidateStyle)
                            StateMgr.GetStateValueEnum(sp.EnumState, EnumLibrary.Business.SiteValidateStyle);

                        if (sb.Length > 0)
                        {
                            sb.Append(";");
                        }
                        sb.Append(sp.ToSiteID + "," + sp.ToSiteName);
                    }
                }
                
                //站点生效方式仅共享存在此值，且一个站点唯一
                switch (svs)
                {
                    case EnumLibrary.SiteValidateStyle.MustReceived:
                        ValidateStyle.Checked = true;
                        break;
                    case EnumLibrary.SiteValidateStyle.NoMustReceived:
                        ValidateStyle.Checked = false;
                        break;
                    default:
                        break;
                }

                SiteSharingAddsTextBox.Text = sb.ToString();
                SiteSharingDelsTextBox.Text = string.Empty;
                #endregion

                #region 绑定接收站点数据源
                //从SitePartnership查找所有可接收站点
                objEnum = (object)EnumLibrary.SitePartnership.Sharing;
                sps = null;
                sps = IDHelper.GetReceivingSites(siteID, objEnum);
                SiteListReceive.DataSource = sps;
                SiteListReceive.DataBind();
                
                objEnum = (object)EnumLibrary.SitePartnership.Receiving;
                sps = null;
                sps = IDHelper.GetReceivingSites(siteID, objEnum);
                sb = new System.Text.StringBuilder();
                if (sps != null)
                {
                    foreach (WebEngine2007.WebServices.ID.SitePartnership sp in sps)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(";");
                        }
                        sb.Append(sp.FromSiteID + "," + sp.FromSiteName);
                    }
                }

                SiteReceiveAddsTextBox.Text = sb.ToString();
                SiteReceiveDelsTextBox.Text = string.Empty;
                #endregion

                //并获取已经设定好的数据范围，并直接赋值给
                //SiteReceiveTextBox、SiteSharingTextBox由前台分解而后对已选站点进行打勾
                //当然需要注册启动脚本
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>onDocumentLoad();</script>");
            }
            catch (Exception ex)
            {
                Messages.ShowMessage("页面初始化出错！出错原因：" + ex.Message);
            }
        }

        protected void SaveSiteButton_Click(object sender, EventArgs e)
        {
            try
            {
                string siteID = CDHelper.GetSiteID();
                string siteName = CDHelper.GetCompanyName();

                //保存共享站点信息
                if (SiteSharingDelsTextBox.Text != "")
                {
                    string[] dels = SiteSharingDelsTextBox.Text.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    IDHelper.DelSharingSites(siteID, dels);
                }

                if (SiteSharingAddsTextBox.Text != "")
                {
                    string[] adds = SiteSharingAddsTextBox.Text.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    object objEnum = null;
                    if (ValidateStyle.Checked)
                    {
                        objEnum = (int)EnumLibrary.SiteValidateStyle.MustReceived;
                    }
                    else
                    {
                        objEnum = (int)EnumLibrary.SiteValidateStyle.NoMustReceived;
                    }
                    IDHelper.AddSharingSites(siteID, siteName, adds, objEnum);
                }

                //保存接受站点信息
                if (SiteReceiveDelsTextBox.Text != "")
                {
                    string[] dels = SiteReceiveDelsTextBox.Text.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    IDHelper.DelReceivingSites(siteID, dels);
                }

                if (SiteReceiveAddsTextBox.Text != "")
                {
                    string[] adds = SiteReceiveAddsTextBox.Text.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    IDHelper.AddReceivingSites(siteID, siteName, adds);
                }

                Initilize();

                Messages.ShowMessage("站点关联保存成功！");
            }
            catch (Exception ex)
            {
                Messages.ShowMessage("保存信息时出错！出错原因：" + ex.Message);
            }
        }

    }
}