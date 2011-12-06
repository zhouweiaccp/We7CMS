using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.IO;
using We7.CMS;
using We7.Framework.Config;
using We7.Framework.Util;

namespace WebEngine2007.CD.Web.Admin.ADInterface
{
    /// <summary>
    /// 依据不同广告管理菜单地址所传的参数进行广告页面调用前的准备工作
    /// </summary>
    public partial class CertificationCreate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //TransferParameterToAD();
            if (ADObjectName == null || ADObjectName == "")
            {
                Messages.ShowError("对不起，您访问地址不合法！");
                return;
            }

          TransferParameterToAD();
        }

        /// <summary>
        /// 广告业务对象名
        /// </summary>
        string ADObjectName
        {
            get
            {
                if (Request["adObject"] != null)
                {
                    return (string)Request["adObject"];
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 向广告页面传递所需参数
        /// </summary>
        protected void TransferParameterToAD()
        {            
            //获取当前系统参数
            //SystemInformation si = CDHelper.GetSystemInformation();
            SiteConfigInfo si = SiteConfigs.GetConfig();
            string adUrl = "";
            if (si.ADUrl != null && si.ADUrl != "")
            {
                adUrl = "http://" + si.ADUrl;
            }
            else
            {
                adUrl = "http://" + Request.Url.Host + ":" + Request.Url.Port.ToString() + "/Plugins/ADPlugin/UI/";  
              
            }
            SiteIDText.Text = si.SiteID;
            string rootRrl = "";
            if (si.RootUrl != null && si.RootUrl != "")
            {
                rootRrl = si.RootUrl;
               
            }
            else
            {
                rootRrl = "http://" + Request.Url.Host + ":" + Request.Url.Port.ToString();
            }
            if (!rootRrl.EndsWith("/"))
            {
                rootRrl = rootRrl + "/";
            }
            SiteUrlTextBox.Text = rootRrl;
            SiteNameText.Text = si.CompanyName;
            AccountIDText.Text = AccountID;
            AccountNameText.Text = AccountHelper.GetAccount(AccountID, new string[] { "LoginName" }).LoginName;
            
            ADObjectText.Text = ADObjectName;
            if (!adUrl.EndsWith("/"))
            {
                adUrl = adUrl + "/";
            }
            ADUrlText.Text = adUrl;

            //为了跨域问题，将此处理转至后台
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>DataReadyAndSend();</script>");

            string aid = AccountID.Substring(1, AccountID.Length - 2);
            string sid = si.SiteID;
            string sname = si.CompanyName;
            string surl = rootRrl;

            //此方法用于站群广告过滤，如果是总站值为1不过滤，看到所有的广告，分站值为0，值看到自己的广告
            string path = Server.MapPath("~/Config/general.config");
            if (!File.Exists(path))
            {
                ArticleHelper.Write(path);
            }
            GeneralConfigInfo config = GeneralConfigs.Deserialize(We7Utils.GetMapPath("~/Config/general.config"));
            string adVisble = config.ADVisbleToSite.ToString();

            //向请求页面发送数据
            //Encoding myEncoding = Encoding.GetEncoding("UTF-8");
            string address = adUrl + "CertificationResponse.aspx?AID=" + aid + "&SID="
                + sid + "&SNAME=" + Server.UrlEncode(sname) + "&SURL=" + surl + "&ADObject=" + ADObjectName + "&ADVisble=" + adVisble;
            //HttpWebRequest req = WebRequest.Create(address) as HttpWebRequest;
            //req.Headers.Add("P3P", "CP=CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR");
            //req.CookieContainer = new CookieContainer();
            //req.Method = "GET";

            //读取请求结果，检查Cookie是否创建成功
            //using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            //{ }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>RedirectToAD('" + address + "');</script>");

            //string returnUrl = "";
            //switch (ADObjectName)
            //{
            //    case "ADRes":
            //        returnUrl = adUrl + "ADResource/ADResourceManage.aspx";
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "AID", aid);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SID", sid);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SNAME", sname);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SURL", surl);
            //        break;

            //    case "ADZone":
            //        returnUrl = adUrl + "ADZones/ADZoneManage.aspx";
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "AID", aid);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SID", sid);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SNAME", sname);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SURL", surl);
            //        break;

            //    case "ADPublish":
            //        returnUrl = adUrl + "ADPublishes/ADPublishManage.aspx";
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "AID", aid);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SID", sid);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SNAME", sname);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SURL", surl);
            //        break;

            //    case "ADSetting":
            //        returnUrl = adUrl + "ADSetting.aspx";
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "AID", aid);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SID", sid);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SNAME", sname);
            //        returnUrl = Helper.AddParamToUrl(returnUrl, "SURL", surl);
            //        break;

            //    default:
            //        break;
            //}

            //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>RedirectToAD('" + returnUrl + "');</script>");

        }
    }
}
